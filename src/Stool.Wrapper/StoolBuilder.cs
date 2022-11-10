using Kompas6API5;
using Kompas6Constants3D;
using StoolModel;

namespace Stool.Wrapper
{
    class StoolBuilder
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
        /// Параметризированный конструктор
        /// </summary>
        /// <param name="kompas">Объект Компас API</param>
        public StoolBuilder(KompasObject kompas)
        {
            _kompas = kompas;
            var document = (ksDocument3D)kompas.Document3D();
            document.Create();
        }

        /// <summary>
        /// Построение детали по заданным параметрам
        /// </summary>
        /// <param name="stool">Объект </param>
        public void Build(StoolParameters StoolParameters)
        {
            double seatWidth = StoolParameters.SeatWidth;
            double seatHeight = StoolParameters.SeatHeight;
            double legsWidth = StoolParameters.LegsWidth;
            double legsHeight = StoolParameters.LegsHeight;
            double legSpacing = StoolParameters.LegSpacing;

            var document = (ksDocument3D)_kompas.ActiveDocument3D();

            _part = (ksPart)document.GetPart((short)Part_Type.pTop_Part);

            _part.name = "Stool";

            _part.SetAdvancedColor(8628426, 0.8, 0.8, 0.8, 0.8, 1);

            _part.Update();

            BuildSeat();
            BuildLeg();
            BuildRung();
            BuildSideBar();
        }

        private void BuildSeat()
        {
            ksEntity entitySketch = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_sketch);
            ksSketchDefinition sketchDefinition = (ksSketchDefinition)entitySketch.GetDefinition();
            ksEntity basePlane = (ksEntity)_part.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);
            sketchDefinition.SetPlane(basePlane);
            entitySketch.Create();

            ksDocument2D sketchEdit = (ksDocument2D)sketchDefinition.BeginEdit();

            sketchEdit.ksLineSeg(-175, -145, -175, 145, 1);
            sketchEdit.ksLineSeg(175, -145, 175, 145, 1);
            sketchEdit.ksLineSeg(-145, 175, 145, 175, 1);
            sketchEdit.ksLineSeg(-145, -175, 145, -175, 1);
            sketchEdit.ksArcByPoint(-145, 145, 30, -175, 145, -145, 175, -1, 1);
            sketchEdit.ksArcByPoint(145, 145, 30, 145, 175, 175, 145, -1, 1);
            sketchEdit.ksArcByPoint(145, -145, 30, 145, -175, 175, -145, 1, 1);
            sketchEdit.ksArcByPoint(-145, -145, 30, -145, -175, -175, -145, -1, 1);

            sketchDefinition.EndEdit();

            ExtrudeSketch(_part, entitySketch, 20, true);
            
            // скругление

            ksEntity obj = _part.NewEntity((short)Obj3dType.o3d_fillet);
            ksFilletDefinition iDefinition = obj.GetDefinition();
            iDefinition.radius = 15;
            iDefinition.tangent = true;
            ksEntityCollection iArray = iDefinition.array();
            ksEntityCollection iCollection = _part.EntityCollection((short)Obj3dType.o3d_face);
            iCollection.SelectByPoint(0, 0, 20);
            ksEntity iFace = iCollection.Last();
            iArray.Add(iFace);
            obj.Create();
        }

        private void BuildLeg()
        {
            ksEntity entitySketch = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_sketch);
            ksSketchDefinition sketchDefinition = (ksSketchDefinition)entitySketch.GetDefinition();
            ksEntity basePlane = (ksEntity)_part.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);
            sketchDefinition.SetPlane(basePlane);
            entitySketch.Create();

            ksDocument2D sketchEdit = (ksDocument2D)sketchDefinition.BeginEdit();

            // ножки
            sketchEdit.ksLineSeg(-145, 145, -145, 105, 1);
            sketchEdit.ksLineSeg(-145, 105, -105, 105, 1);
            sketchEdit.ksLineSeg(-105, 105, -105, 145, 1);
            sketchEdit.ksLineSeg(-105, 145, -145, 145, 1);

            sketchEdit.ksLineSeg(105, 145, 105, 105, 1);
            sketchEdit.ksLineSeg(105, 105, 145, 105, 1);
            sketchEdit.ksLineSeg(145, 105, 145, 145, 1);
            sketchEdit.ksLineSeg(145, 145, 105, 145, 1);

            sketchEdit.ksLineSeg(145, -105, 105, -105, 1);
            sketchEdit.ksLineSeg(105, -105, 105, -145, 1);
            sketchEdit.ksLineSeg(105, -145, 145, -145, 1);
            sketchEdit.ksLineSeg(145, -145, 145, -105, 1);

            sketchEdit.ksLineSeg(-145, -105, -105, -105, 1);
            sketchEdit.ksLineSeg(-105, -105, -105, -145, 1);
            sketchEdit.ksLineSeg(-105, -145, -145, -145, 1);
            sketchEdit.ksLineSeg(-145, -105, -145, -145, 1);

            sketchDefinition.EndEdit();

            ExtrudeSketch(_part, entitySketch, 400, false);

            // скругление
            ksEntity obj = _part.NewEntity((short)Obj3dType.o3d_fillet);
            ksFilletDefinition iDefinition = obj.GetDefinition();
            iDefinition.radius = 5;
            iDefinition.tangent = true;
            ksEntityCollection iArray = iDefinition.array();

            ksEntityCollection iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(105, 145, -150);
            ksEntity iEdge = iCollection.Last();
            iArray.Add(iEdge);
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(105, 105, -150);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(145, 105, -150);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(145, 145, -150);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);

            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(-105, 145, -150);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(-105, 105, -150);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(-145, 105, -150);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(-145, 145, -150);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);

            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(105, -145, -150);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(105, -105, -150);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(145, -105, -150);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(145, -145, -150);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);

            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(-145, -145, -150);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(-105, -145, -150);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(-105, -105, -150);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(-145, -105, -150);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);

            obj.Create();
        }

        private void BuildRung()
        {
            ksEntity iSketch = _part.NewEntity((short)Obj3dType.o3d_sketch);
            ksSketchDefinition iDefinition = (ksSketchDefinition)iSketch.GetDefinition();
            ksEntityCollection iCollection = _part.EntityCollection((short)Obj3dType.o3d_face);
            iCollection.SelectByPoint(105, 125, -275);
            ksEntity iPlane = iCollection.First();
            iDefinition.SetPlane(iPlane);
            iSketch.Create();

            ksDocument2D _ksDocument2D = (ksDocument2D)iDefinition.BeginEdit();

            _ksDocument2D.ksLineSeg(190, -110, 210, -110, 1);
            _ksDocument2D.ksLineSeg(215, -115, 215, -135, 1);
            _ksDocument2D.ksLineSeg(210, -140, 190, -140, 1);
            _ksDocument2D.ksLineSeg(185, -115, 185, -135, 1);
            _ksDocument2D.ksArcByPoint(190, -135, 5, 185, -135, 190, -140, 1, 1);
            _ksDocument2D.ksArcByPoint(190, -115, 5, 185, -115, 190, -110, -1, 1);
            _ksDocument2D.ksArcByPoint(210, -115, 5, 210, -110, 215, -115, -1, 1);
            _ksDocument2D.ksArcByPoint(210, -135, 5, 210, -140, 215, -135, 1, 1);
            iDefinition.EndEdit();
            ExtrudeSketch(_part, iSketch, 210, true);

            iSketch = _part.NewEntity((short)Obj3dType.o3d_sketch);
            iDefinition = (ksSketchDefinition)iSketch.GetDefinition();
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_face);
            iCollection.SelectByPoint(-105, 125, -275);
            iPlane = iCollection.First();
            iDefinition.SetPlane(iPlane);
            iSketch.Create();

            _ksDocument2D = (ksDocument2D)iDefinition.BeginEdit();

            _ksDocument2D.ksLineSeg(-190, 110, -210, 110, 1);
            _ksDocument2D.ksLineSeg(-215, 115, -215, 135, 1);
            _ksDocument2D.ksLineSeg(-210, 140, -190, 140, 1);
            _ksDocument2D.ksLineSeg(-185, 115, -185, 135, 1);
            _ksDocument2D.ksArcByPoint(-190, 135, 5, -185, 135, -190, 140, 1, 1);
            _ksDocument2D.ksArcByPoint(-190, 115, 5, -185, 115, -190, 110, -1, 1);
            _ksDocument2D.ksArcByPoint(-210, 115, 5, -210, 110, -215, 115, -1, 1);
            _ksDocument2D.ksArcByPoint(-210, 135, 5, -210, 140, -215, 135, 1, 1);
            iDefinition.EndEdit();
            ExtrudeSketch(_part, iSketch, 210, true);

            iSketch = _part.NewEntity((short)Obj3dType.o3d_sketch);
            iDefinition = (ksSketchDefinition)iSketch.GetDefinition();
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_face);
            iCollection.SelectByPoint(-125, -105, -225);
            iPlane = iCollection.First();
            iDefinition.SetPlane(iPlane);
            iSketch.Create();

            _ksDocument2D = (ksDocument2D)iDefinition.BeginEdit();

            _ksDocument2D.ksLineSeg(-115, 185, -135, 185, 1);
            _ksDocument2D.ksLineSeg(-140, 190, -140, 210, 1);
            _ksDocument2D.ksLineSeg(-135, 215, -115, 215, 1);
            _ksDocument2D.ksLineSeg(-110, 190, -110, 210, 1);
            _ksDocument2D.ksArcByPoint(-115, 210, 5, -110, 210, -115, 215, 1, 1);
            _ksDocument2D.ksArcByPoint(-115, 190, 5, -110, 190, -115, 185, -1, 1);
            _ksDocument2D.ksArcByPoint(-135, 190, 5, -135, 185, -140, 190, -1, 1);
            _ksDocument2D.ksArcByPoint(-135, 210, 5, -135, 215, -140, 210, 1, 1);
            iDefinition.EndEdit();
            ExtrudeSketch(_part, iSketch, 210, true);

            iSketch = _part.NewEntity((short)Obj3dType.o3d_sketch);
            iDefinition = (ksSketchDefinition)iSketch.GetDefinition();
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_face);
            iCollection.SelectByPoint(125, 105, -225);
            iPlane = iCollection.First();
            iDefinition.SetPlane(iPlane);
            iSketch.Create();

            _ksDocument2D = (ksDocument2D)iDefinition.BeginEdit();

            _ksDocument2D.ksLineSeg(115, -185, 135, -185, 1);
            _ksDocument2D.ksLineSeg(140, -190, 140, -210, 1);
            _ksDocument2D.ksLineSeg(135, -215, 115, -215, 1);
            _ksDocument2D.ksLineSeg(110, -190, 110, -210, 1);
            _ksDocument2D.ksArcByPoint(115, -210, 5, 110, -210, 115, -215, 1, 1);
            _ksDocument2D.ksArcByPoint(115, -190, 5, 110, -190, 115, -185, -1, 1);
            _ksDocument2D.ksArcByPoint(135, -190, 5, 135, -185, 140, -190, -1, 1);
            _ksDocument2D.ksArcByPoint(135, -210, 5, 135, -215, 140, -210, 1, 1);
            iDefinition.EndEdit();
            ExtrudeSketch(_part, iSketch, 210, true);
        }

        private void BuildSideBar()
        {
            ksEntity iSketch = _part.NewEntity((short)Obj3dType.o3d_sketch);
            ksSketchDefinition iDefinition = (ksSketchDefinition)iSketch.GetDefinition();
            ksEntityCollection iCollection = _part.EntityCollection((short)Obj3dType.o3d_face);
            iCollection.SelectByPoint(0, 0, 0);
            ksEntity iPlane = iCollection.First();
            iDefinition.SetPlane(iPlane);
            iSketch.Create();

            ksDocument2D _ksDocument2D = (ksDocument2D)iDefinition.BeginEdit();

            _ksDocument2D.ksLineSeg(110, -105, 140, -105, 1);
            _ksDocument2D.ksLineSeg(140, -105, 140, 105, 1);
            _ksDocument2D.ksLineSeg(140, 105, 110, 105, 1);
            _ksDocument2D.ksLineSeg(110, 105, 110, -105, 1);

            _ksDocument2D.ksLineSeg(-105, 140, 105, 140, 1);
            _ksDocument2D.ksLineSeg(-105, 110, 105, 110, 1);
            _ksDocument2D.ksLineSeg(-105, 140, -105, 110, 1);
            _ksDocument2D.ksLineSeg(105, 140, 105, 110, 1);

            _ksDocument2D.ksLineSeg(-110, 105, -110, -105, 1);
            _ksDocument2D.ksLineSeg(-140, -105, -140, 105, 1);
            _ksDocument2D.ksLineSeg(-140, 105, -110, 105, 1);
            _ksDocument2D.ksLineSeg(-140, -105, -110, -105, 1);

            _ksDocument2D.ksLineSeg(-105, -140, 105, -140, 1);
            _ksDocument2D.ksLineSeg(105, -110, -105, -110, 1);
            _ksDocument2D.ksLineSeg(-105, -110, -105, -140, 1);
            _ksDocument2D.ksLineSeg(105, -110, 105, -140, 1);

            iDefinition.EndEdit();
            ExtrudeSketch(_part, iSketch, 55, true);

            // скругление
            ksEntity obj = _part.NewEntity((short)Obj3dType.o3d_fillet);
            ksFilletDefinition iDefinition1 = obj.GetDefinition();
            iDefinition1.radius = 5;
            iDefinition1.tangent = true;
            ksEntityCollection iArray = iDefinition1.array();

            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(-110, 0, -55);
            ksEntity iEdge = iCollection.Last();
            iArray.Add(iEdge);
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(-140, 0, -55);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);

            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(0, -140, -55);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(0, -110, -55);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);

            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(140, 0, -55);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(110, 0, -55);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);

            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(0, 140, -55);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);
            iCollection = _part.EntityCollection((short)Obj3dType.o3d_edge);
            iCollection.SelectByPoint(0, 110, -55);
            iEdge = iCollection.Last();
            iArray.Add(iEdge);

            obj.Create();
        }

        /// <summary>
        /// Выдавливание
        /// </summary>
        /// <param name="part"></param>
        /// <param name="sketch"></param>
        /// <param name="height"></param>
        /// <param name="type"></param>
        private void ExtrudeSketch(ksPart part, ksEntity sketch, double height, bool type)
        {
            ksEntity entityExtrusion = (ksEntity)part.NewEntity((short)
                Obj3dType.o3d_baseExtrusion);
            ksBaseExtrusionDefinition extrusionDefinition =
                (ksBaseExtrusionDefinition)entityExtrusion.GetDefinition();
            if (type == false)
            {
                extrusionDefinition.directionType = (short)Direction_Type.dtReverse;
                extrusionDefinition.SetSideParam(false,(short)End_Type.etBlind, height);
            }
            if (type == true)
            {
                extrusionDefinition.directionType = (short)Direction_Type.dtNormal;
                extrusionDefinition.SetSideParam(true, (short)End_Type.etBlind, height);
            }
            extrusionDefinition.SetSketch(sketch);
            entityExtrusion.Create();
        }

    }
}
