using System.Collections.Generic;
using StoolModel;
using NUnit.Framework;

namespace Stool.UnitTests
{
    /// <summary>
    /// Класс тестирования класса параметра
    /// </summary>
    [TestFixture]
    class ParameterTest
    {
        /// <summary>
        /// Позитивный тест геттера Value
        /// </summary>
        [Test(Description = "Позитивный тест геттера Value")]
        public void TestValueGet_CorrectValue()
        {
            var parameter = new Parameter(1, 10, 20, ParameterType.SeatWidth,
                new Dictionary<ParameterType, string>(), "Value");
            const int expected = 10;
            var actual = parameter.Value;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Негативный тест геттера Value, когда Value меньше MinValue
        /// </summary>
        [Test(Description = "Негативный тест геттера Value, когда Value меньше MinValue")]
        public void TestValueMixValueGet_IncorrectValue()
        {
            var parameter = new Parameter(10, 1, 20, ParameterType.SeatWidth,
                new Dictionary<ParameterType, string>(), "Value");
            const int expected = 0;
            var actual = parameter.Value;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Негативный тест геттера Value, когда Value больше MaxValue
        /// </summary>
        [Test(Description = "Негативный тест геттера Value, когда Value больше MaxValue")]
        public void TestValueMaxValueGet_IncorrectValue()
        {
            var parameter = new Parameter(1, 20, 10, ParameterType.SeatWidth,
                new Dictionary<ParameterType, string>(),"Value");
            const int expected = 0;
            var actual = parameter.Value;
            Assert.AreEqual(expected, actual);
        }
    }
}
