using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoolModel
{
    public class StoolParameters
    {
        // ПОМЕНЯТЬ КОММЫ!!!!!!!!

        /// <summary>
        /// Длина всей втулки
        /// </summary>
        private double _seatWidth; 

        /// <summary>
        /// Длина верхней части втулки
        /// </summary>
        private double _seatHeight;

        /// <summary>
        /// Диаметр верхней части втулки
        /// </summary>
        private double _legsWidth;

        /// <summary>
        /// Внешний диаметр втулки
        /// </summary>
        private double _legsHeight;

        /// <summary>
        /// Внутренний диаметр втулки
        /// </summary>
        private double _legSpacing;

        // все поля переделать в словарь

        /// <summary>
        /// Возвращает и устанавливает значение длины всей втулки
        /// </summary>
        public double TotalLength
        {
            get
            {
                return _seatWidth;
            }

            set
            {
                
                _seatWidth = value;
            }
        }

        /// <summary>
        /// Возвращает и устанавливает значение длины верхней части втулки
        /// </summary>
        public double TopLength
        {
            get
            {
                return _seatHeight;
            }

            set
            {
                
                _seatHeight = value;
            }
        }

        /// <summary>
        /// Возвращает и устанавливает значение диаметра верхей части втулки
        /// </summary>
        public double TopDiametr
        {
            get
            {
                return _legsWidth;
            }

            set
            {
                
                _legsWidth = value;
            }
        }


        /// <summary>
        /// Возвращает и устанавливает значение внешнего диаметра втулки
        /// </summary>
        public double OuterDiametr
        {
            get
            {
                return _legsHeight;
            }

            set
            {
                
                _legsHeight = value;
            }
        }

        /// <summary>
        /// Возвращает и устанавливает значение внутреннего диаметра втулки
        /// </summary>
        public double InnerDiametr
        {
            get
            {
                return _legSpacing;
            }

            set
            {
                
                _legSpacing = value;
            }
        }

        /// <summary>
        /// Конструктор втулки без гравировки
        /// </summary>
        /// <param name="totalLength">Длина всей втулки</param>
        /// <param name="topLength">Длина верхней части втулки</param>
        /// <param name="topDiametr">Диаметр верхней части втулки</param>
        /// <param name="outerDiametr">Внешний диаметр втулки</param>
        /// <param name="innerDiametr">Внутренний диаметр втулки</param>
        public StoolParameters(double totalLength, double topLength,
            double topDiametr, double outerDiametr, double innerDiametr)
        {
            TotalLength = totalLength;
            TopLength = topLength;
            TopDiametr = topDiametr;
            OuterDiametr = outerDiametr;
            InnerDiametr = innerDiametr;           
        }

            //"Ширина сиденья должна быть 300 – 400 мм \n" +
            //"Высота сиденья должна быть 10 – 50 мм \n" +
            //"Толщина ножек должна быть 20 – 60 мм \n" +
            //"Высота ножек должна быть 300 – 500 мм \n" +
            //"Расстояние между ножками должна быть 190 – 230 мм \n";
    }
}
