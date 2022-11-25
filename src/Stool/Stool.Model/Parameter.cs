﻿using System.Collections.Generic;

namespace StoolModel
{
    public class Parameter
    {
        /// <summary>
        /// Минимальная величина
        /// </summary>
        private readonly int _minValue;

        /// <summary>
        /// Максимальная величина
        /// </summary>
        private readonly int _maxValue;

        /// <summary>
        /// Введенная величина
        /// </summary>
        private double _value;

        /// <summary>
        /// Тип параметра
        /// </summary>
        private readonly ParameterType _parameterType;

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        private readonly string _valueErrorMessage;

        /// <summary>
        /// Словарь ошибок для значений параметров
        /// </summary>
        private readonly Dictionary<ParameterType, string> _errors;

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

        /// <summary>
        /// /// Создание объекта класса Parameter
        /// </summary>
        /// <param name="minValue">Минимальная величина</param>
        /// <param name="value">Введенная величина</param>
        /// <param name="maxValue">Максимальная величина</param>
        /// <param name="parameterType">Тип параметра</param>
        /// <param name="errors">Словарь ошибок для значений параметров</param>
        /// <param name="errorMessage">Сообщение о параметре</param>
        public Parameter(int minValue, double value, int maxValue, ParameterType parameterType,
            Dictionary<ParameterType, string> errors, string errorMessage)           
        {           
            _minValue = minValue;           
            _maxValue = maxValue;
            _parameterType = parameterType;
            _errors = errors;
            _valueErrorMessage = errorMessage + " не может быть менее чем " + _minValue + " мм" + 
                " и более чем " +_maxValue + " мм.";
            Value = value;
        }

        /// <summary>
        /// Проверка введенных значений параметров
        /// </summary>
        /// <param name="value">Введенная величина</param>
        /// <returns>Возвращает true, если нет ошибок, иначе - false</returns>
        private bool CheckValues(double value)
        {
            if ((value < _minValue) || (value > _maxValue))
            {
                _errors.Add(_parameterType, _valueErrorMessage);
                return false;
            }
            else return true;
        }
    }
}