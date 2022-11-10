using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoolModel
{
    /// <summary>
    /// Перечисление параметров
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// Длина сиденья
        /// </summary>
        SeatWidth,

        /// <summary>
        /// Ширина сиденья
        /// </summary>
        SeatHeight,

        /// <summary>
        /// Ширина ножек
        /// </summary>
        LegsWidth,

        /// <summary>
        /// Длина ножек
        /// </summary>
        LegsHeight,

        /// <summary>
        /// Расстояние между ножками
        /// </summary>
        LegSpacing
    }
}
