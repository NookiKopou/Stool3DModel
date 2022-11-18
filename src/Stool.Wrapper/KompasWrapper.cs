using System;
using System.Runtime.InteropServices;
using Kompas6API5;
using Kompas6Constants3D;

namespace Stool.Wrapper
{
    public class KompasWrapper
    {
        /// <summary>
        /// Объект Компас API
        /// </summary>
        private KompasObject _kompas;

        /// <summary>
        /// Деталь
        /// </summary>
        private ksPart _part;

        /// <summary>
        /// Документ-модель
        /// </summary>
        private ksDocument3D _document;

        /// <summary>
        /// Возвращает тип перечисления грани
        /// </summary>
        public Obj3dType FaceType => Obj3dType.o3d_face;

        /// <summary>
        /// Возвращает тип перечисления ребра
        /// </summary>
        public Obj3dType EdgeType => Obj3dType.o3d_edge;

        /// <summary>
        /// Запуск Компас-3D
        /// </summary>
        public void StartKompas()
        {
            try
            {
                if (_kompas != null)
                {
                    _kompas.Visible = true;
                    _kompas.ActivateControllerAPI();
                }

                if (_kompas != null) return;
                {
                    var kompasType = Type.GetTypeFromProgID("KOMPAS.Application.5");
                    _kompas = (KompasObject)Activator.CreateInstance(kompasType);
                    StartKompas();
                    if (_kompas == null)
                    {
                        throw new Exception("Не удается открыть Компас-3D.");
                    }
                }
            }
            catch (COMException)
            {
                _kompas = null;
                StartKompas();
            }
        }

        /// <summary>
        /// Создание файла в Компас 3D
        /// </summary>
        public void CreateDocument()
        {
            try
            {
                _document = (ksDocument3D)_kompas.Document3D();
                _document.Create();
                _document = (ksDocument3D)_kompas.ActiveDocument3D();
            }
            catch
            {
                throw new ArgumentException("Не удается построить деталь");
            }
        }

        /// <summary>
        /// Установка свойств модели
        /// </summary>
        public void SetProperties()
        {
            _part = (ksPart)_document.GetPart((short)Part_Type.pTop_Part);
            _part.name = "Stool";
            _part.SetAdvancedColor(8628426, 0.8, 0.8, 0.8, 0.8);
            _part.Update();
        }

        /// <summary>
        /// Создание на эскизе отрезков
        /// </summary>
        /// <param name="sketchEdit">Объект графического документа</param>
        /// <param name="segmentPoints">Массив точек для построения отрезков</param>
        /// <param name="isMustBeMirrored">Координаты должны быть отражены</param>
        private static void CreateSegments(ksDocument2D sketchEdit,
            double[,] segmentPoints, bool isMustBeMirrored)
        {
            for (var i = 0; i < segmentPoints.GetLength(0); i++)
            {
                sketchEdit.ksLineSeg(segmentPoints[i, 0],
                    segmentPoints[i, 1],
                    segmentPoints[i, 2],
                    segmentPoints[i, 3], 1);
                if (!isMustBeMirrored) continue;

                // Инвертирование по оси Y
                sketchEdit.ksLineSeg(segmentPoints[i, 0],
                    -segmentPoints[i, 1],
                    segmentPoints[i, 2],
                    -segmentPoints[i, 3], 1);

                // Инвертирование по оси X и Y
                sketchEdit.ksLineSeg(-segmentPoints[i, 0],
                    -segmentPoints[i, 1],
                    -segmentPoints[i, 2],
                    -segmentPoints[i, 3], 1);

                // Инвертирование по оси X
                sketchEdit.ksLineSeg(-segmentPoints[i, 0],
                    segmentPoints[i, 1],
                    -segmentPoints[i, 2],
                    segmentPoints[i, 3], 1);
            }
        }

        /// <summary>
        /// Создание на эскизе дуг
        /// </summary>
        /// <param name="sketchEdit">Объект графического документа</param>
        /// <param name="arcs">Массив дуг для построения</param>
        private static void CreateArs(ksDocument2D sketchEdit, double[,] arcs)
        {
            for (var i = 0; i < arcs.GetLength(0); i++)
            {
                sketchEdit.ksArcByPoint(arcs[i, 0],
                    arcs[i, 1],
                    arcs[i, 2],
                    arcs[i, 3],
                    arcs[i, 4],
                    arcs[i, 5],
                    arcs[i, 6],
                    (short)arcs[i, 7],
                    1);
            }
        }

        /// <summary>
        /// Создание плоскости построения по точке
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        /// <param name="z">Координата Z</param>
        /// <returns>Сформированный эскиз</returns>
        private ksEntity CreatePlaneByPoint(double x, double y, double z)
        {
            ksEntityCollection collection =
                _part.EntityCollection((short)Obj3dType.o3d_face);
            collection.SelectByPoint(x, y, z);
            ksEntity plane = collection.First();
            return plane;
        }

        /// <summary>
        /// Построение набора отрезков
        /// </summary>
        /// <param name="plane">Плоскость построения</param>
        /// <param name="segments">Массив отрезков</param>
        /// <param name="isMustBeMirrored">Координаты должны быть отражены</param>
        /// <returns>Сформированный эскиз</returns>
        public ksEntity BuildSetSegments(ksEntity plane, double[,] segments, bool isMustBeMirrored)
        {
            ksEntity entitySketch = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_sketch);
            ksSketchDefinition sketchDefinition = (ksSketchDefinition)entitySketch.GetDefinition();
            sketchDefinition.SetPlane(plane);
            entitySketch.Create();
            ksDocument2D sketchEdit = (ksDocument2D)sketchDefinition.BeginEdit();
            CreateSegments(sketchEdit, segments, isMustBeMirrored);
            sketchDefinition.EndEdit();
            return entitySketch;
        }

        /// <summary>
        /// Создание эскиза набора отрезков по базовой плоскости
        /// </summary>
        /// <param name="segments">Массив координат отрезков</param>
        /// <param name="isMustBeMirrored">Координаты должны быть отражены</param>
        /// <returns>Сформированный эскиз</returns>
        public ksEntity BuildSetSegmentsByDefaultPlane(double[,] segments, bool isMustBeMirrored)
        {
            ksEntity plane = (ksEntity)_part.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);
            ksEntity sketch =
                BuildSetSegments(plane, segments, isMustBeMirrored);
            return sketch;
        }

        /// <summary>
        /// Создание эскиза набора отрезков по точке
        /// </summary>
        /// <param name="planePoint">Массив координат центра плоскости</param>
        /// <param name="segments">Массив координат отрезков</param>
        /// <param name="isMustBeMirrored">Координаты должны быть отражены</param>
        /// <returns>Сформированный эскиз</returns>
        public ksEntity BuildSetSegmentsByPoint(double[] planePoint,
            double[,] segments, bool isMustBeMirrored)
        {
            ksEntity plane = CreatePlaneByPoint(planePoint[0], planePoint[1], planePoint[2]);
            ksEntity sketch =
                BuildSetSegments(plane, segments, isMustBeMirrored);
            return sketch;
        }

        /// <summary>
        /// Создание эскиза из отрезков и дуг
        /// </summary>
        /// <param name="plane">Плоскость построения</param>
        /// <param name="segments">Массив точек, являющихся концами отрезков</param>
        /// <param name="arcs">Массив дуг</param>
        /// <param name="isMustBeMirrored">Координаты должны быть отражены</param>
        /// <returns>Сформированный эскиз</returns>
        public ksEntity BuildSegmentsWithArcs(ksEntity plane,
            double[,] segments, double[,] arcs, bool isMustBeMirrored)
        {
            ksEntity entitySketch = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_sketch);
            ksSketchDefinition sketchDefinition = (ksSketchDefinition)entitySketch.GetDefinition();
            sketchDefinition.SetPlane(plane);
            entitySketch.Create();
            ksDocument2D sketchEdit = (ksDocument2D)sketchDefinition.BeginEdit();
            CreateSegments(sketchEdit, segments, isMustBeMirrored);
            CreateArs(sketchEdit, arcs);
            sketchDefinition.EndEdit();
            return entitySketch;
        }

        /// <summary>
        /// Создание эскиза отрезков и дуг по базовой плоскости
        /// </summary>
        /// <param name="segments">Массив координат отрезков</param>
        /// <param name="arcs">Массив параметров дуг</param>
        /// <param name="isMustBeMirrored">Координаты должны быть отражены</param>
        /// <returns>Сформированный эскиз</returns>
        public ksEntity BuildSegmentsWithArcsByDefaultPlane(
            double[,] segments, double[,] arcs, bool isMustBeMirrored)
        {
            ksEntity plane = (ksEntity)_part.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);
            ksEntity sketch =
                BuildSegmentsWithArcs(plane, segments, arcs, isMustBeMirrored);
            return sketch;
        }

        /// <summary>
        /// Создание эскиза отрезков и дуг по точке
        /// </summary>
        /// <param name="planePoint">Массив координат центра плоскости</param>
        /// <param name="segments">Массив координат отрезков</param>
        /// <param name="arcs">Массив параметров дуг</param>
        /// <param name="isMustBeMirrored">Координаты должны быть отражены</param>
        /// <returns>Сформированный эскиз</returns>
        public ksEntity BuildSegmentsWithArcsByPoint(double[] planePoint,
            double[,] segments, double[,] arcs, bool isMustBeMirrored)
        {
            ksEntity plane = CreatePlaneByPoint(planePoint[0], planePoint[1], planePoint[2]);
            ksEntity sketch =
                BuildSegmentsWithArcs(plane, segments, arcs, isMustBeMirrored);
            return sketch;
        }

        /// <summary>
        /// Выдавливание эскиза
        /// </summary>
        /// <param name="sketch">Эскиз</param>
        /// <param name="height">Высота выдавливания</param>
        /// <param name="type">Тип выдавливания</param>
        public void ExtrudeSketch(ksEntity sketch, double height, bool type)
        {
            ksEntity entityExtrusion = (ksEntity)_part.NewEntity((short)
                Obj3dType.o3d_baseExtrusion);
            ksBaseExtrusionDefinition extrusionDefinition =
                (ksBaseExtrusionDefinition)entityExtrusion.GetDefinition();
            if (type == false)
            {
                extrusionDefinition.directionType = (short)Direction_Type.dtReverse;
            }
            else
            {
                extrusionDefinition.directionType = (short)Direction_Type.dtNormal;
            }
            extrusionDefinition.SetSideParam(type, (short)End_Type.etBlind, height);
            extrusionDefinition.SetSketch(sketch);
            entityExtrusion.Create();
        }

        /// <summary>
        /// Создания скругления
        /// </summary>
        /// <param name="edgeCoordinatesArray">Массив координат ребер, где будет скругление</param>
        /// <param name="radius">Радиус скругления</param>
        /// <param name="type">Тип скругления: плоскость или ребро</param>
        public void CreateFillet(double[,] edgeCoordinatesArray, double radius, Obj3dType type)
        {
            ksEntity sketch = _part.NewEntity((short)Obj3dType.o3d_fillet);
            ksFilletDefinition definition = sketch.GetDefinition();
            definition.radius = radius;
            definition.tangent = true;
            ksEntityCollection array = definition.array();
            for (var i = 0; i < edgeCoordinatesArray.GetLength(0); i++)
            {
                ksEntityCollection collection =
                    _part.EntityCollection((short)type);
                collection.SelectByPoint(edgeCoordinatesArray[i, 0],
                    edgeCoordinatesArray[i, 1],
                    edgeCoordinatesArray[i, 2]);
                ksEntity edge = collection.Last();
                array.Add(edge);
                if (type != Obj3dType.o3d_edge) continue;

                // Инвертирование координат по X
                collection =
                    _part.EntityCollection((short)type);
                collection.SelectByPoint(-edgeCoordinatesArray[i, 0],
                    edgeCoordinatesArray[i, 1],
                    edgeCoordinatesArray[i, 2]);
                edge = collection.Last();
                array.Add(edge);

                // Инвертирование координат по X и Y
                collection =
                    _part.EntityCollection((short)type);
                collection.SelectByPoint(-edgeCoordinatesArray[i, 0],
                    -edgeCoordinatesArray[i, 1],
                    edgeCoordinatesArray[i, 2]);
                edge = collection.Last();
                array.Add(edge);

                // Инвертирование координат по Y
                collection =
                    _part.EntityCollection((short)type);
                collection.SelectByPoint(edgeCoordinatesArray[i, 0],
                    -edgeCoordinatesArray[i, 1],
                    edgeCoordinatesArray[i, 2]);
                edge = collection.Last();
                array.Add(edge);
            }

            sketch.Create();
        }
    }
}
