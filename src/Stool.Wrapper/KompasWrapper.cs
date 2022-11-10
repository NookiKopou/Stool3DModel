using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Kompas6API5;
using Kompas6Constants3D;
using StoolModel;

namespace Stool.Wrapper
{
    public class KompasWrapper
    {
        /// <summary>
        /// Объект Компас API
        /// </summary>
        private KompasObject _kompas;

        private ksPart _part;

        private ksDocument3D _document;

        public Obj3dType DefaultPlane => Obj3dType.o3d_planeXOY;

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
            _part.SetAdvancedColor(8628426, 0.5, 0.6, 0.8, 1, 0.5);
            _part.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="planePoint"></param>
        /// <param name="sketch"></param>
        /// <returns></returns>
        private ksSketchDefinition CreateSketchDefinition(X, Y, Z, ksEntity sketch)
        {
            ksSketchDefinition definition =
                (ksSketchDefinition)sketch.GetDefinition();
            ksEntity plane = CreatePlaneByPoint(X, Y, Z);
            definition.SetPlane(plane);
            sketch.Create();
            return definition;
        }

        private ksEntity CreatePlaneByPoint(X, Y, Z)
        {
            ksEntityCollection collection =
                _part.EntityCollection((short)Obj3dType.o3d_face);
            collection.SelectByPoint(X, Y, Z);
            ksEntity plane = collection.First();
            return plane;
        }

        //линия
        
        //скругление

        //дуга

        /// <summary>
        /// Выдавливание эскиза на определенное расстояние.
        /// </summary>
        /// <param name="sketch">Эскиз.</param>
        /// <param name="height">Высота выдавливания.</param>
        /// <param name="direction">Направление: true - прямое, false - обратное.</param>
        /// <param name="draftValue">Угол, на который изменяется проекция эскиза.</param>
        /// <param name="isMustBeThin">Толщина стенок: true - выдавливается контур,
        /// false - эскиз.</param>
        public void ExtrudeSketch(ksEntity sketch, double height, bool direction,
            double draftValue, bool isMustBeThin)
        {
            ksEntity entity =
                (ksEntity)_part.NewEntity((short)Obj3dType.o3d_baseExtrusion);
            ksBaseExtrusionDefinition definition =
                (ksBaseExtrusionDefinition)entity.GetDefinition();
            if (direction)
            {
                definition.directionType = (short)Direction_Type.dtNormal;
            }
            else
            {
                definition.directionType = (short)Direction_Type.dtReverse;
            }
            definition.SetSideParam(direction, (short)End_Type.etBlind,
                height, draftValue);
            if (isMustBeThin)
            {
                definition.SetThinParam(true, (short)End_Type.etBlind,
                    1, 0);
            }
            definition.SetSketch(sketch);
            entity.Create();
        }

        /// <summary>
        /// Построение детали
        /// </summary>
        /// <param name="parameters"></param>
        public void BuildStool(StoolParameters parameters)
        {
            try
            {
                StoolBuilder detail = new StoolBuilder(_kompas);
                detail.Build(parameters);
            }
            catch
            {
                throw new ArgumentException("Не удается построить деталь");
            }
        }

    }

}
