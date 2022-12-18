using NUnit.Framework;
using System.Collections.Generic;
using StoolModel;

namespace Stool.UnitTests
{
    /// <summary>
    /// Класс тестирования полей класса табурета
    /// </summary>
    [TestFixture]
    public class StoolParametersTest
    {
        /// <summary>
        /// Объект класса табурета
        /// </summary>
        private readonly StoolParameters _stoolParameters = new StoolParameters(350, 20, 40, 400, 210, 55);

        /// <summary>
        /// Позитивный тест геттера Errors
        /// </summary>
        [Test(Description = "Позитивный тест геттера Errors")]
        public void TestErrorListGet_CorrectValue()
        {
            var expected = new Dictionary<ParameterType, string>();
            var actual = _stoolParameters.Errors;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Позитивный тест геттера Parameters
        /// </summary>
        [Test(Description = "Позитивный тест геттера Parameters")]
        public void TestParametersGet_CorrectValue()
        {
            var expected = new Dictionary<ParameterType, Parameter>()
            {
                {
                    ParameterType.SeatWidth,
                    new Parameter(300, 350, 400, ParameterType.SeatWidth,
                        _stoolParameters.Errors, "Ширина сиденья")
                },
                {
                    ParameterType.SeatHeight,
                    new Parameter(10, 20, 50, ParameterType.SeatHeight,
                        _stoolParameters.Errors, "Высота сиденья")
                },
                {
                    ParameterType.LegsWidth,
                    new Parameter(30, 40, 50, ParameterType.LegsWidth,
                        _stoolParameters.Errors, "Ширина ножек")
                },
                {
                    ParameterType.LegsHeight,
                    new Parameter(300, 400, 500, ParameterType.LegsHeight,
                        _stoolParameters.Errors, "Длина ножек")
                },
                {
                    ParameterType.LegSpacing,
                    new Parameter(140, 210, 280, ParameterType.LegSpacing,
                        _stoolParameters.Errors, "Расстояние между ножками")
                },
                { 
                    ParameterType.SideBarHeight,
                    new Parameter(20, 55, 90, ParameterType.SideBarHeight,
                        _stoolParameters.Errors, "Высота царги")
                }
            };
            var actual = _stoolParameters.Parameters;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Позитивный и негативный тест сеттера Parameters
        /// </summary>
        [Test(Description = "Позитивный и негативный тест сеттера Parameters")]
        [TestCase(350, 0, Description = "Позитивный тест сеттера Parameters")]
        [TestCase(400, 1, Description = "Негативный тест сеттера Parameters")]
        public void TestParametersSet_CorrectValue(double seatWidth, int expected)
        {
            const double seatHeight = 30;
            const double legsWidth = 40;
            const double legsHeight = 400;
            const double legSpacing = 210;
            const double sideBarHeight = 55;
            _stoolParameters.SetParameters(seatWidth, seatHeight, legsWidth,
                legsHeight, legSpacing, sideBarHeight);
            var actual = _stoolParameters.Errors.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
