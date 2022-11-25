using System;
using System.Collections.Generic;

namespace StoolModel
{
    public class StoolParameters
    {
        /// <summary>
        /// Словарь "тип параметра - параметр"
        /// </summary>
        public Dictionary<ParameterType, Parameter> Parameters { get; set; }

        /// <summary>
        /// Словарь ошибок
        /// </summary>
        public Dictionary<ParameterType, string> Errors { get; set; }

        /// <summary>
        /// Создает объект класса табурета
        /// </summary>
        /// <param name="seatWidth">Длина сиденья</param>
        /// <param name="seatHeight">Ширина сиденья</param>
        /// <param name="legsWidth">Ширина ножек</param>
        /// <param name="legsHeight">Длина ножек</param>
        /// <param name="legSpacing">Расстояние между ножками</param>
        public StoolParameters(double seatWidth, double seatHeight,
            double legsWidth, double legsHeight, double legSpacing)
        {
            Errors = new Dictionary<ParameterType, string>();
            Parameters = new Dictionary<ParameterType, Parameter>()
            {
                { ParameterType.SeatWidth,
                    new Parameter(300, seatWidth, 400, ParameterType.SeatWidth, 
                    Errors, "Ширина сиденья")},
                { ParameterType.SeatHeight,
                    new Parameter(10, seatHeight, 50, ParameterType.SeatHeight, 
                    Errors, "Высота сиденья")},
                { ParameterType.LegsWidth,
                    new Parameter(30, legsWidth, 50, ParameterType.LegsWidth, 
                    Errors, "Ширина ножек")},
                { ParameterType.LegsHeight,
                    new Parameter(300, legsHeight, 500, ParameterType.LegsHeight, 
                    Errors, "Длина ножек")},
                { ParameterType.LegSpacing,
                    new Parameter(140, legSpacing, 280, ParameterType.LegSpacing, 
                    Errors, "Расстояние между ножками")}
            };
        }

        /// <summary>
        /// Создает объект для построения
        /// </summary>
        /// <param name="seatWidth">Ширина сиденья</param>
        /// <param name="legsWidth">Ширина ножек</param>
        /// <param name="legsHeight">Ширина ножек</param>
        /// <param name="legSpacing">Расстояние между ножками</param>
        /// <param name="seatHeight">Высота сиденья</param>
        public void SetParameters(double seatWidth, double seatHeight,
            double legsWidth, double legsHeight, double legSpacing)
        {
            Errors.Clear();
            Parameters[ParameterType.SeatWidth].Value = seatWidth;
            CheckParametersRelationship(legSpacing, seatWidth - 160,
                ParameterType.LegSpacing,
                "Расстояние между ножками не может быть меньше " +
                "ширины сиденья более чем на 190 мм.");
            if (!Errors.ContainsKey(ParameterType.LegSpacing))
            {
                CheckParametersRelationship(-legSpacing, -seatWidth+120,
                    ParameterType.LegSpacing,
                    "Ширина сиденья не может быть меньше расстояния между ножками.");
            }
            Parameters[ParameterType.SeatHeight].Value = seatHeight;
            Parameters[ParameterType.LegsWidth].Value = legsWidth;
            Parameters[ParameterType.LegsHeight].Value = legsHeight;
        }

        /// <summary>
        /// Проверка взаимосвязи параметров между собой
        /// </summary>
        /// <param name="value">Значение введенного параметра</param>
        /// <param name="mainParameter">Значение параметра, от которого зависимость</param>
        /// <param name="parameterType">Тип параметра</param>
        /// <param name="errorMessage">Сообщение об ошибке</param>
        private void CheckParametersRelationship(double value, double mainParameter,
            ParameterType parameterType, string errorMessage)
        {
            if (value < mainParameter)
            {
                Errors.Add(parameterType, errorMessage);
            }
            else
            {
                Parameters[parameterType].Value = Math.Abs(value);
            }
        }
    }
}
