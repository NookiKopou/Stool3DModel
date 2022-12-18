using StoolModel;

namespace Stool.Wrapper
{
    /// <summary>
    /// Класс построения 3D-модели табурета
    /// </summary>
    public class StoolBuilder
    {
        /// <summary>
        /// Связь с Компас-3D
        /// </summary>
        private readonly KompasWrapper _wrapper = new KompasWrapper();
        
        /// <summary>
        /// Построение детали по заданным параметрам
        /// </summary>
        public void Build(StoolParameters stoolParameters)
        {
            _wrapper.StartKompas();
            _wrapper.CreateDocument();
            _wrapper.SetProperties();
            var seatWidth = 
                stoolParameters.Parameters[ParameterType.SeatWidth].Value;
            var seatHeight =
                stoolParameters.Parameters[ParameterType.SeatHeight].Value;
            var legsWidth =
                stoolParameters.Parameters[ParameterType.LegsWidth].Value;
            var legsHeight =
                stoolParameters.Parameters[ParameterType.LegsHeight].Value;
            var legSpacing =
                stoolParameters.Parameters[ParameterType.LegSpacing].Value;
            var sideBarHeight =
                stoolParameters.Parameters[ParameterType.SideBarHeight].Value;

            // Переменная, определяющая, насколько изменилась ширина сиденья 
            var seatWidthDiff = seatWidth - 350;
            // Переменная, определяющая, насколько изменилась высота ножек
            var legsWidthDiff = legsWidth - 40;
            // Переменная, определяющая, насколько изменилось расстояние между ножками 
            var legSpacingDiff = legSpacing - 210;
            // Переменная, определяющая, насколько изменилось расстояние между ножками 
            var sideBarHeightDiff = sideBarHeight - 55;

            BuildSeat(seatWidthDiff / 2, seatHeight);
            BuildLeg(legsWidthDiff, legsHeight, legSpacingDiff/2);
            BuildRung(legSpacing, legSpacingDiff/2);
            BuildSideBar(legSpacingDiff/2, sideBarHeight, sideBarHeightDiff);
        }

        /// <summary>
        /// Построение сидения
        /// </summary>
        private void BuildSeat(double seatWidthDiff, double seatHeight)
        {
            // Построение основания сиденья

            // Массив координат отрезка: индекс 0 - X начальное, индекс 1 - Y начальное,
            // индекс 2 - X конечное, индекс 3 - Y конечное
            double[,] segments =
            {
                { -175-seatWidthDiff, -145-seatWidthDiff, -175-seatWidthDiff, 145+seatWidthDiff },
                { 175+seatWidthDiff, -145-seatWidthDiff, 175+seatWidthDiff, 145+seatWidthDiff },
                { -145-seatWidthDiff, 175+seatWidthDiff, 145+seatWidthDiff, 175+seatWidthDiff },
                { -145-seatWidthDiff, -175-seatWidthDiff, 145+seatWidthDiff, -175-seatWidthDiff },
            };

            // Массив параметров дуги: индекс 0 - X центра, индекс 1 - Y центра,
            // индекс 2 - радиус дуги,
            // индекс 3 - X начальное, индекс 4 - Y начальное
            // индекс 5 - X конечное, индекс 6 - Y конечное
            // индекс 7 - направление
            double[,] arcs =
            {
                { -145-seatWidthDiff, 145+seatWidthDiff, 30, -175-seatWidthDiff, 145+seatWidthDiff, -145-seatWidthDiff, 175+seatWidthDiff, -1 },
                { 145+seatWidthDiff, 145+seatWidthDiff, 30, 145+seatWidthDiff, 175+seatWidthDiff, 175+seatWidthDiff, 145+seatWidthDiff, -1 },
                { 145+seatWidthDiff, -145-seatWidthDiff, 30, 145+seatWidthDiff, -175-seatWidthDiff, 175+seatWidthDiff, -145-seatWidthDiff, 1 },
                { -145-seatWidthDiff, -145-seatWidthDiff, 30, -145-seatWidthDiff, -175-seatWidthDiff, -175-seatWidthDiff, -145-seatWidthDiff, -1 },
            };

            // Созданный эскиз
            var sketch =
                _wrapper.BuildSegmentsWithArcsByDefaultPlane(segments, arcs, false);

            _wrapper.ExtrudeSketch(sketch, seatHeight, true);

            // Создание скругления отрезков

            const double radius = 15;
            double[,] edgeCoordinatesArray =
            {
                { 0, 0, seatHeight }
            };
            _wrapper.CreateFillet(edgeCoordinatesArray, radius, _wrapper.FaceType);
        }

        /// <summary>
        /// Построение ножек
        /// </summary>
        private void BuildLeg(double legsWidthDiff, double legsHeight, double diff3)
        {
            // Построение основания ножек

            // Массив координат отрезка: индекс 0 - X начальное, индекс 1 - Y начальное,
            // индекс 2 - X конечное, индекс 3 - Y конечное
            double[,] segments =
            {
                { -145-legsWidthDiff-diff3, 145+legsWidthDiff+diff3, -145-legsWidthDiff-diff3, 105+diff3 },
                { -145-legsWidthDiff-diff3, 105+diff3, -105-diff3, 105+diff3},
                { -105-diff3, 105+diff3, -105-diff3, 145+legsWidthDiff+diff3 },
                { -105-diff3, 145+legsWidthDiff+diff3, -145-legsWidthDiff-diff3, 145+legsWidthDiff+diff3 },
            };

            // Созданный эскиз
            var sketch = _wrapper.BuildSetSegmentsByDefaultPlane(segments, true);
            _wrapper.ExtrudeSketch(sketch, legsHeight, false);

            // Создание скругления отрезков
            const double radius = 5;
            double[,] edgeCoordinatesArray =
            {
                { 105+diff3, 145+legsWidthDiff+diff3, -150 },
                { 105+diff3, 105+diff3, -150 },
                { 145+legsWidthDiff+diff3, 105+diff3, -150 },
                { 145+legsWidthDiff+diff3, 145+legsWidthDiff+diff3, -150 }
            };
            _wrapper.CreateFillet(edgeCoordinatesArray, radius, _wrapper.EdgeType);
        }

        /// <summary>
        /// Построение проножки
        /// </summary>
        private void BuildRung(double legSpacing, double legSpacingDiff)
        {
            // Построение основания проножки на плоскости ножки

            // Координата точки: индекс 0 - X, индекс 1 - Y, индекс 2 - Z.
            var point = new[] { 105 + legSpacingDiff, 125 + legSpacingDiff, -275 };

            // Массив координат отрезка: индекс 0 - X начальное, индекс 1 - Y начальное,
            // индекс 2 - X конечное, индекс 3 - Y конечное
            var segments = new[,]
            {
                { 190, -110 - legSpacingDiff, 210, -110 - legSpacingDiff},
                { 215, -115 - legSpacingDiff, 215, -135 - legSpacingDiff},
                { 210, -140 - legSpacingDiff, 190, -140 - legSpacingDiff},
                { 185, -115 - legSpacingDiff, 185, -135 - legSpacingDiff},
            };

            // Массив параметров дуги: индекс 0 - X центра, индекс 1 - Y центра,
            // индекс 2 - радиус дуги,
            // индекс 3 - X начальное, индекс 4 - Y начальное
            // индекс 5 - X конечное, индекс 6 - Y конечное
            // индекс 7 - направление
            var arcs = new[,]
            {
                { 190, -135 - legSpacingDiff, 5, 185, -135 - legSpacingDiff, 190, -140 - legSpacingDiff, 1 },
                { 190, -115 - legSpacingDiff, 5, 185, -115 - legSpacingDiff, 190, -110 - legSpacingDiff, -1 },
                { 210, -115 - legSpacingDiff, 5, 210, -110 - legSpacingDiff, 215, -115 - legSpacingDiff, -1 },
                { 210, -135 - legSpacingDiff, 5, 210, -140 - legSpacingDiff, 215, -135 - legSpacingDiff, 1 },
            };

            // Созданный эскиз
            var sketch = _wrapper.BuildSegmentsWithArcsByPoint(point, segments, arcs, false);
            _wrapper.ExtrudeSketch(sketch, legSpacing, true);

            // Построение основания проножки на плоскости ножки

            // Координата точки: индекс 0 - X, индекс 1 - Y, индекс 2 - Z
            point = new[] { -105 - legSpacingDiff, 125 + legSpacingDiff, -275 };

            // Массив координат отрезка: индекс 0 - X начальное, индекс 1 - Y начальное,
            // индекс 2 - X конечное, индекс 3 - Y конечное
            segments = new[,]
            {
                { -190, 110 + legSpacingDiff, -210, 110 + legSpacingDiff },
                { -215, 115 + legSpacingDiff, -215, 135 + legSpacingDiff },
                { -210, 140 + legSpacingDiff, -190, 140 + legSpacingDiff },
                { -185, 115 + legSpacingDiff, -185, 135 + legSpacingDiff },
            };

            // Массив параметров дуги: индекс 0 - X центра, индекс 1 - Y центра,
            // индекс 2 - радиус дуги,
            // индекс 3 - X начальное, индекс 4 - Y начальное
            // индекс 5 - X конечное, индекс 6 - Y конечное
            // индекс 7 - направление
            arcs = new[,]
            {
                { -190, 135 + legSpacingDiff, 5, -185, 135 + legSpacingDiff, -190, 140 + legSpacingDiff, 1 },
                { -190, 115 + legSpacingDiff, 5, -185, 115 + legSpacingDiff, -190, 110 + legSpacingDiff, -1 },
                { -210, 115 + legSpacingDiff, 5, -210, 110 + legSpacingDiff, -215, 115 + legSpacingDiff, -1 },
                { -210, 135 + legSpacingDiff, 5, -210, 140 + legSpacingDiff, -215, 135 + legSpacingDiff, 1 },
            };

            // Созданный эскиз
            sketch = _wrapper.BuildSegmentsWithArcsByPoint(point, segments, arcs, false);
            _wrapper.ExtrudeSketch(sketch, legSpacing, true);

            // Построение основания проножки на плоскости ножки

            // Координата точки: индекс 0 - X, индекс 1 - Y, индекс 2 - Z
            point = new[] { -125 - legSpacingDiff, -105 - legSpacingDiff, -225 };

            // Массив координат отрезка: индекс 0 - X начальное, индекс 1 - Y начальное,
            // индекс 2 - X конечное, индекс 3 - Y конечное
            segments = new[,]
            {
                { -115 - legSpacingDiff, 185, -135 - legSpacingDiff, 185 },
                { -140 - legSpacingDiff, 190, -140 - legSpacingDiff, 210 },
                { -135 - legSpacingDiff, 215, -115 - legSpacingDiff, 215 },
                { -110 - legSpacingDiff, 190, -110 - legSpacingDiff, 210 },
            };

            // Массив параметров дуги: индекс 0 - X центра, индекс 1 - Y центра,
            // индекс 2 - радиус дуги,
            // индекс 3 - X начальное, индекс 4 - Y начальное
            // индекс 5 - X конечное, индекс 6 - Y конечное
            // индекс 7 - направление
            arcs = new[,]
            {
                { -115 - legSpacingDiff, 210, 5, -110 - legSpacingDiff, 210, -115 - legSpacingDiff, 215, 1 },
                { -115 - legSpacingDiff, 190, 5, -110 - legSpacingDiff, 190, -115 - legSpacingDiff, 185, -1 },
                { -135 - legSpacingDiff, 190, 5, -135 - legSpacingDiff, 185, -140 - legSpacingDiff, 190, -1 },
                { -135 - legSpacingDiff, 210, 5, -135 - legSpacingDiff, 215, -140 - legSpacingDiff, 210, 1 },
            };

            // Созданный эскиз
            sketch = _wrapper.BuildSegmentsWithArcsByPoint(point, segments, arcs, false);
            _wrapper.ExtrudeSketch(sketch, legSpacing, true);

            // Построение основания проножки на плоскости ножки

            // Координата точки: индекс 0 - X, индекс 1 - Y, индекс 2 - Z
            point = new[] { 125 + legSpacingDiff, 105 + legSpacingDiff, -225 };

            // Массив координат отрезка: индекс 0 - X начальное, индекс 1 - Y начальное,
            // индекс 2 - X конечное, индекс 3 - Y конечное
            segments = new[,]
            {
                { 115 + legSpacingDiff, -185, 135 + legSpacingDiff, -185 },
                { 140 + legSpacingDiff, -190, 140 + legSpacingDiff, -210 },
                { 135 + legSpacingDiff, -215, 115 + legSpacingDiff, -215 },
                { 110 + legSpacingDiff, -190, 110 + legSpacingDiff, -210 },
            };

            // Массив параметров дуги: индекс 0 - X центра, индекс 1 - Y центра,
            // индекс 2 - радиус дуги,
            // индекс 3 - X начальное, индекс 4 - Y начальное
            // индекс 5 - X конечное, индекс 6 - Y конечное
            // индекс 7 - направление
            arcs = new[,]
            {
                { 115 + legSpacingDiff, -210, 5, 110 + legSpacingDiff, -210, 115 + legSpacingDiff, -215, 1 },
                { 115 + legSpacingDiff, -190, 5, 110 + legSpacingDiff, -190, 115 + legSpacingDiff, -185, -1 },
                { 135 + legSpacingDiff, -190, 5, 135 + legSpacingDiff, -185, 140 + legSpacingDiff, -190, -1 },
                { 135 + legSpacingDiff, -210, 5, 135 + legSpacingDiff, -215, 140 + legSpacingDiff, -210, 1 },
            };

            // Созданный эскиз
            sketch = _wrapper.BuildSegmentsWithArcsByPoint(point, segments, arcs, false);
            _wrapper.ExtrudeSketch(sketch, legSpacing, true);
        }

        /// <summary>
        /// Построение царги
        /// </summary>
        private void BuildSideBar(double legSpacingDiff, double sideBarHeight,
            double sideBarHeightDiff)
        {
            // Построение боковой части царги

            // Координата точки: индекс 0 - X, индекс 1 - Y, индекс 2 - Z
            double[] point = { 0, 0, 0 };

            // Массив координат отрезка: индекс 0 - X начальное, индекс 1 - Y начальное,
            // индекс 2 - X конечное, индекс 3 - Y конечное
            double[,] segments =
            {
                { 110+legSpacingDiff, -105-legSpacingDiff, 140+legSpacingDiff, -105-legSpacingDiff },
                { 140+legSpacingDiff, -105-legSpacingDiff, 140+legSpacingDiff, 105+legSpacingDiff },
                { 140+legSpacingDiff, 105+legSpacingDiff, 110+legSpacingDiff, 105+legSpacingDiff },
                { 110+legSpacingDiff, 105+legSpacingDiff, 110+legSpacingDiff, -105-legSpacingDiff },
                { -105-legSpacingDiff, 140+legSpacingDiff, 105+legSpacingDiff, 140+legSpacingDiff },
                { -105-legSpacingDiff, 110+legSpacingDiff, 105+legSpacingDiff, 110+legSpacingDiff },
                { -105-legSpacingDiff, 140+legSpacingDiff, -105-legSpacingDiff, 110+legSpacingDiff },
                { 105+legSpacingDiff, 140+legSpacingDiff, 105+legSpacingDiff, 110+legSpacingDiff },
                { -110-legSpacingDiff, 105+legSpacingDiff, -110-legSpacingDiff, -105-legSpacingDiff },
                { -140-legSpacingDiff, -105-legSpacingDiff, -140-legSpacingDiff, 105+legSpacingDiff },
                { -140-legSpacingDiff, 105+legSpacingDiff, -110-legSpacingDiff, 105+legSpacingDiff },
                { -140-legSpacingDiff, -105-legSpacingDiff, -110-legSpacingDiff, -105-legSpacingDiff },
                { -105-legSpacingDiff, -140-legSpacingDiff, 105+legSpacingDiff, -140-legSpacingDiff },
                { 105+legSpacingDiff, -110-legSpacingDiff, -105-legSpacingDiff, -110-legSpacingDiff },
                { -105-legSpacingDiff, -110-legSpacingDiff, -105-legSpacingDiff, -140-legSpacingDiff },
                { 105+legSpacingDiff, -110-legSpacingDiff, 105+legSpacingDiff, -140-legSpacingDiff },
            };

            // Созданный эскиз
            var sketch = _wrapper.BuildSetSegmentsByPoint(point, segments, false);
            _wrapper.ExtrudeSketch(sketch, sideBarHeight, true);

            // Создание скругления отрезков
            const double radius = 5;
            double[,] filletEdgeCoordinates =
            {
                { -110-legSpacingDiff, 0-legSpacingDiff, -55 - sideBarHeightDiff },
                { -140-legSpacingDiff, 0-legSpacingDiff, -55 - sideBarHeightDiff},
                { 0-legSpacingDiff, -140-legSpacingDiff, -55 - sideBarHeightDiff},
                { 0-legSpacingDiff , -110-legSpacingDiff, -55 - sideBarHeightDiff}
            };
            _wrapper.CreateFillet(filletEdgeCoordinates, radius, _wrapper.EdgeType);
        }
    }
}
