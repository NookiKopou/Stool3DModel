using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoolModel
{
    class ParameterType
    {
        /// <summary>
        /// Максимальная величина
        /// </summary>
        private int _maxValue;

        /// <summary>
        /// Минимальная величина
        /// </summary>
        private int _minValue;

        /// <summary>
        /// Введенная величина
        /// </summary>
        private double _value;

        /// <summary>
        /// Название параметра
        /// </summary>
        private Parameter _parameterName;

        /// <summary>
        /// Возвращает и устанавливает максимальное значение 
        /// </summary>
        public int MaxValue
        {
            get
            {
                return _maxValue;
            }

            set
            {

                _maxValue = value;
            }
        }

        /// <summary>
        /// Возвращает и устанавливает минимальное значение
        /// </summary>
        public int MinValue
        {
            get
            {
                return _minValue;
            }

            set
            {

                _minValue = value;
            }
        }

        /// <summary>
        /// Возвращает и устанавливает значение
        /// </summary>
        public double Value
        {
            get
            {
                return _value;
            }

            set
            {

                _value = value;
            }
        }

        /// <summary>
        /// Возвращает и устанавливает имя параметра 
        /// </summary>
        public Parameter ParameterName
        {
            get
            {
                return _parameterName;
            }

            set
            {

                _parameterName = value;
            }
        }
    }
}
