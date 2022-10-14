using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kompas6Constants3D;
using Kompas6API5;
using StoolModel;

namespace Stool.Wrapper
{
    class StoolBuilder
    {
        /// <summary>
        /// Объект Компас API
        /// </summary>
        private KompasObject _kompas;

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
        /// <param name="bushing">Объект втулки</param>
        public void CreateDetail(StoolParameters stool)
        {
            var document = (ksDocument3D)_kompas.ActiveDocument3D();
            _part = (ksPart)document.GetPart((short)Part_Type.pTop_Part);
            ExampleCreate();

        }

        private void ExampleCreate()
        {
            ksEntity entitySketch = (ksEntity)_part.NewEntity((short)
                Obj3dType.o3d_sketch);
            ksSketchDefinition sketchDefinition = (ksSketchDefinition)
                entitySketch.GetDefinition();
            ksEntity basePlane = (ksEntity)_part.GetDefaultEntity((short)
                Obj3dType.o3d_planeXOY);
            sketchDefinition.SetPlane(basePlane);
            entitySketch.Create();
            ksDocument2D sketchEdit = (ksDocument2D)sketchDefinition.
                BeginEdit();
            sketchEdit.ksLineSeg(-175, -145, -175, 145, 1);
            sketchEdit.ksLineSeg(175, -145, 175, 145, 1);
            sketchEdit.ksLineSeg(-145, 175, 145, 175, 1);
            sketchEdit.ksLineSeg(-145, -175, 145, -175, 1);
            sketchEdit.ksArcByPoint(-145, 145, 30, -175, 145, -145, 175, -1, 1);
            sketchEdit.ksArcByPoint(145, 145, 30, 145, 175, 175, 145, -1, 1);
            sketchEdit.ksArcByPoint(145, -145, 30, 145, -175, 175, -145, 1, 1);
            sketchEdit.ksArcByPoint(-145, -145, 30, -145, -175, -175, -145, -1, 1);
            sketchDefinition.EndEdit();
            ExtrudeSketch(_part, entitySketch, 20, false);
            //var iPart7 = _kompas.TopPart;
            
            ksEntity obj = _part.NewEntity((short)Obj3dType.o3d_fillet);
            ksFilletDefinition iDefinition = obj.GetDefinition();
            iDefinition.radius = 15;
            iDefinition.tangent = true;
            ksEntityCollection iArray = iDefinition.array();
            ksEntityCollection iCollection = _part.EntityCollection((short)Obj3dType.o3d_face);
            iCollection.SelectByPoint(0, 0, -20);
            ksEntity iFace = iCollection.Last();
            iArray.Add(iFace);
            obj.Create();
        }

        private void ExtrudeSketch(ksPart part, ksEntity sketch,
            double height, bool type)
        {
            ksEntity entityExtrusion = (ksEntity)part.NewEntity((short)
                Obj3dType.o3d_baseExtrusion);
            ksBaseExtrusionDefinition extrusionDefinition =
                (ksBaseExtrusionDefinition)entityExtrusion.GetDefinition();
            if (type == false)
            {
                extrusionDefinition.directionType = (short)Direction_Type.
                    dtReverse;
                extrusionDefinition.SetSideParam(false,
                    (short)End_Type.etBlind, height);
            }
            if (type == true)
            {
                extrusionDefinition.directionType = (short)Direction_Type.
                    dtNormal;
                extrusionDefinition.SetSideParam(true,
                    (short)End_Type.etBlind, height);
            }
            extrusionDefinition.SetSketch(sketch);
            entityExtrusion.Create();
        }

    }
}
