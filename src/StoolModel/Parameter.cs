using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoolModel
{
    class Parameter
    {
        /// <summary>
        /// Максимальная величина
        /// </summary>
        private readonly int _maxValue;

        /// <summary>
        /// Минимальная величина
        /// </summary>
        private readonly int _minValue;

        /// <summary>
        /// Введенная величина
        /// </summary>
        private double _value;

        /// <summary>
        /// 
        /// </summary>
        private readonly ParameterType _parameterType;

        /// <summary>
        /// Возвращает и устанавливает значение
        /// </summary>
        public double Value
        {
            get => _value;
            set
            {
                if (CheckValues(value))
                {
                    _value = value;
                }
            }
        }

        public Parameter(int minValue, double value, int maxValue, ParameterType parameterType)           
        {
            _minValue = minValue;
            _maxValue = maxValue;
            _parameterType = parameterType;
            Value = value;
        }

        private bool CheckValues(double value)
        {
            if (value < _minValue)
            {
                //ошибка
                return false;
            }
            if (!(value > _maxValue)) return true;
            //ошибка
            return false;
        }
    }
}
