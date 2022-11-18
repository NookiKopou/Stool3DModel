using StoolModel;

namespace Stool.Wrapper
{
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

            var diff1 = seatWidth - 350;
            var diff2 = legsWidth - 40;
            var diff3 = legSpacing - 210;

            BuildSeat(diff1 / 2, seatHeight);
            BuildLeg(diff2, legsHeight, diff3/2);
            BuildRung(legSpacing, diff3/2);
            BuildSideBar(diff3/2);
        }

        /// <summary>
        /// Построение сидения
        /// </summary>
        private void BuildSeat(double diff1, double seatHeight)
        {
            // Массив координат отрезка: индекс 0 - X начальное, индекс 1 - Y начальное,
            // индекс 2 - X конечное, индекс 3 - Y конечное
            double[,] segments =
            {
                { -175-diff1, -145-diff1, -175-diff1, 145+diff1 },
                { 175+diff1, -145-diff1, 175+diff1, 145+diff1 },
                { -145-diff1, 175+diff1, 145+diff1, 175+diff1 },
                { -145-diff1, -175-diff1, 145+diff1, -175-diff1 },
            };

            // Массив параметров дуги: индекс 0 - X центра, индекс 1 - Y центра,
            // индекс 2 - радиус дуги,
            // индекс 3 - X начальное, индекс 4 - Y начальное
            // индекс 5 - X конечное, индекс 6 - Y конечное
            // индекс 7 - направление
            double[,] arcs =
            {
                { -145-diff1, 145+diff1, 30, -175-diff1, 145+diff1, -145-diff1, 175+diff1, -1 },
                { 145+diff1, 145+diff1, 30, 145+diff1, 175+diff1, 175+diff1, 145+diff1, -1 },
                { 145+diff1, -145-diff1, 30, 145+diff1, -175-diff1, 175+diff1, -145-diff1, 1 },
                { -145-diff1, -145-diff1, 30, -145-diff1, -175-diff1, -175-diff1, -145-diff1, -1 },
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
        /// Построение ног
        /// </summary>
        private void BuildLeg(double diff2, double legsHeight, double diff3)
        {
            // Массив координат отрезка: индекс 0 - X начальное, индекс 1 - Y начальное,
            // индекс 2 - X конечное, индекс 3 - Y конечное
            double[,] segments =
            {
                { -145-diff2-diff3, 145+diff2+diff3, -145-diff2-diff3, 105+diff3 },
                { -145-diff2-diff3, 105+diff3, -105-diff3, 105+diff3},
                { -105-diff3, 105+diff3, -105-diff3, 145+diff2+diff3 },
                { -105-diff3, 145+diff2+diff3, -145-diff2-diff3, 145+diff2+diff3 },
            };

            // Созданный эскиз
            var sketch = _wrapper.BuildSetSegmentsByDefaultPlane(segments, true);
            _wrapper.ExtrudeSketch(sketch, legsHeight, false);

            // Создание скругления отрезков

            const double radius = 5;
            double[,] edgeCoordinatesArray =
            {
                { 105+diff3, 145+diff2+diff3, -150 },
                { 105+diff3, 105+diff3, -150 },
                { 145+diff2+diff3, 105+diff3, -150 },
                { 145+diff2+diff3, 145+diff2+diff3, -150 }
            };
            _wrapper.CreateFillet(edgeCoordinatesArray, radius, _wrapper.EdgeType);
        }

        /// <summary>
        /// Построение проножки
        /// </summary>
        private void BuildRung(double legSpacing, double diff3)
        {
            // Координата точки: индекс 0 - X, индекс 1 - Y, индекс 2 - Z.
            var point = new[] { 105 + diff3, 125 + diff3, -275 };

            // Массив координат отрезка: индекс 0 - X начальное, индекс 1 - Y начальное,
            // индекс 2 - X конечное, индекс 3 - Y конечное
            var segments = new double[,]
            {
                { 190, -110, 210, -110 },
                { 215, -115, 215, -135 },
                { 210, -140, 190, -140 },
                { 185, -115, 185, -135 },
            };

            // Массив параметров дуги: индекс 0 - X центра, индекс 1 - Y центра,
            // индекс 2 - радиус дуги,
            // индекс 3 - X начальное, индекс 4 - Y начальное
            // индекс 5 - X конечное, индекс 6 - Y конечное
            // индекс 7 - направление
            var arcs = new double[,]
            {
                { 190, -135, 5, 185, -135, 190, -140, 1 },
                { 190, -115, 5, 185, -115, 190, -110, -1 },
                { 210, -115, 5, 210, -110, 215, -115, -1 },
                { 210, -135, 5, 210, -140, 215, -135, 1 },
            };

            // Созданный эскиз
            var sketch = _wrapper.BuildSegmentsWithArcsByPoint(point, segments, arcs, false);
            _wrapper.ExtrudeSketch(sketch, legSpacing, true);

            // Координата точки: индекс 0 - X, индекс 1 - Y, индекс 2 - Z
            point = new[] { -105 - diff3, 125 + diff3, -275 };

            // Массив координат отрезка: индекс 0 - X начальное, индекс 1 - Y начальное,
            // индекс 2 - X конечное, индекс 3 - Y конечное
            segments = new double[,]
            {
                { -190, 110, -210, 110 },
                { -215, 115, -215, 135 },
                { -210, 140, -190, 140 },
                { -185, 115, -185, 135 },
            };

            // Массив параметров дуги: индекс 0 - X центра, индекс 1 - Y центра,
            // индекс 2 - радиус дуги,
            // индекс 3 - X начальное, индекс 4 - Y начальное
            // индекс 5 - X конечное, индекс 6 - Y конечное
            // индекс 7 - направление
            arcs = new double[,]
            {
                { -190, 135, 5, -185, 135, -190, 140, 1 },
                { -190, 115, 5, -185, 115, -190, 110, -1 },
                { -210, 115, 5, -210, 110, -215, 115, -1 },
                { -210, 135, 5, -210, 140, -215, 135, 1 },
            };

            // Созданный эскиз
            sketch = _wrapper.BuildSegmentsWithArcsByPoint(point, segments, arcs, false);
            _wrapper.ExtrudeSketch(sketch, legSpacing, true);

            // Координата точки: индекс 0 - X, индекс 1 - Y, индекс 2 - Z
            point = new[] { -125 - diff3, -105 - diff3, -225 };

            // Массив координат отрезка: индекс 0 - X начальное, индекс 1 - Y начальное,
            // индекс 2 - X конечное, индекс 3 - Y конечное
            segments = new double[,]
            {
                { -115, 185, -135, 185 },
                { -140, 190, -140, 210 },
                { -135, 215, -115, 215 },
                { -110, 190, -110, 210 },
            };

            // Массив параметров дуги: индекс 0 - X центра, индекс 1 - Y центра,
            // индекс 2 - радиус дуги,
            // индекс 3 - X начальное, индекс 4 - Y начальное
            // индекс 5 - X конечное, индекс 6 - Y конечное
            // индекс 7 - направление
            arcs = new double[,]
            {
                { -115, 210, 5, -110, 210, -115, 215, 1 },
                { -115, 190, 5, -110, 190, -115, 185, -1 },
                { -135, 190, 5, -135, 185, -140, 190, -1 },
                { -135, 210, 5, -135, 215, -140, 210, 1 },
            };

            // Созданный эскиз
            sketch = _wrapper.BuildSegmentsWithArcsByPoint(point, segments, arcs, false);
            _wrapper.ExtrudeSketch(sketch, legSpacing, true);

            // Координата точки: индекс 0 - X, индекс 1 - Y, индекс 2 - Z
            point = new[] { 125 + diff3, 105 + diff3, -225 };

            // Массив координат отрезка: индекс 0 - X начальное, индекс 1 - Y начальное,
            // индекс 2 - X конечное, индекс 3 - Y конечное
            segments = new double[,]
            {
                { 115, -185, 135, -185 },
                { 140, -190, 140, -210 },
                { 135, -215, 115, -215 },
                { 110, -190, 110, -210 },
            };

            // Массив параметров дуги: индекс 0 - X центра, индекс 1 - Y центра,
            // индекс 2 - радиус дуги,
            // индекс 3 - X начальное, индекс 4 - Y начальное
            // индекс 5 - X конечное, индекс 6 - Y конечное
            // индекс 7 - направление
            arcs = new double[,]
            {
                { 115, -210, 5, 110, -210, 115, -215, 1 },
                { 115, -190, 5, 110, -190, 115, -185, -1 },
                { 135, -190, 5, 135, -185, 140, -190, -1 },
                { 135, -210, 5, 135, -215, 140, -210, 1 },
            };

            // Созданный эскиз
            sketch = _wrapper.BuildSegmentsWithArcsByPoint(point, segments, arcs, false);
            _wrapper.ExtrudeSketch(sketch, legSpacing, true);
        }

        /// <summary>
        /// Построение царги
        /// </summary>
        private void BuildSideBar(double diff3)
        {
            // Координата точки: индекс 0 - X, индекс 1 - Y, индекс 2 - Z
            double[] point = { 0, 0, 0 };

            // Массив координат отрезка: индекс 0 - X начальное, индекс 1 - Y начальное,
            // индекс 2 - X конечное, индекс 3 - Y конечное
            double[,] segments =
            {
                { 110+diff3, -105-diff3, 140+diff3, -105-diff3 },
                { 140+diff3, -105-diff3, 140+diff3, 105+diff3 },
                { 140+diff3, 105+diff3, 110+diff3, 105+diff3 },
                { 110+diff3, 105+diff3, 110+diff3, -105-diff3 },
                { -105-diff3, 140+diff3, 105+diff3, 140+diff3 },
                { -105-diff3, 110+diff3, 105+diff3, 110+diff3 },
                { -105-diff3, 140+diff3, -105-diff3, 110+diff3 },
                { 105+diff3, 140+diff3, 105+diff3, 110+diff3 },
                { -110-diff3, 105+diff3, -110-diff3, -105-diff3 },
                { -140-diff3, -105-diff3, -140-diff3, 105+diff3 },
                { -140-diff3, 105+diff3, -110-diff3, 105+diff3 },
                { -140-diff3, -105-diff3, -110-diff3, -105-diff3 },
                { -105-diff3, -140-diff3, 105+diff3, -140-diff3 },
                { 105+diff3, -110-diff3, -105-diff3, -110-diff3 },
                { -105-diff3, -110-diff3, -105-diff3, -140-diff3 },
                { 105+diff3, -110-diff3, 105+diff3, -140-diff3 },
            };

            // Созданный эскиз
            var sketch = _wrapper.BuildSetSegmentsByPoint(point, segments, false);
            _wrapper.ExtrudeSketch(sketch, 55, true);

            // Создание скругления отрезков
            const double radius = 5;
            double[,] filletEdgeCoordinates =
            {
                { -110-diff3, 0-diff3, -55 },
                { -140-diff3, 0-diff3, -55 },
                { 0-diff3, -140-diff3, -55 },
                { 0-diff3 , -110-diff3, -55 }
            };
            _wrapper.CreateFillet(filletEdgeCoordinates, radius, _wrapper.EdgeType);
        }
    }
}
