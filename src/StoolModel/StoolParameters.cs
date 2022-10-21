using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoolModel
{
    public class StoolParameters
    {
        /// <summary>
        /// Длина сиденья
        /// </summary>
        private double _seatWidth = 350;

        /// <summary>
        /// Высота сиденья
        /// </summary>
        private double _seatHeight = 30;

        /// <summary>
        /// Ширина ножек
        /// </summary>
        private double _legsWidth = 40;

        /// <summary>
        /// Длина ножек
        /// </summary>
        private double _legsHeight = 400;

        /// <summary>
        /// Расстояние между ножками
        /// </summary>
        private double _legSpacing = 210;

        // все поля переделать в словарь

        /// <summary>
        /// Возвращает и устанавливает значение длины сиденья
        /// </summary>
        public double SeatWidth
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
        /// Возвращает и устанавливает значение высоты сиденья
        /// </summary>
        public double SeatHeight
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
        /// Возвращает и устанавливает значение ширины ножек
        /// </summary>
        public double LegsWidth
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
        /// Возвращает и устанавливает значение длины ножек
        /// </summary>
        public double LegsHeight
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
        /// Возвращает и устанавливает значение расстояния между ножками
        /// </summary>
        public double LegSpacing
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
        /// Конструктор
        /// </summary>
        /// <param name="seatWidth"></param>
        /// <param name="seatHeight"></param>
        /// <param name="legsWidtht"></param>
        /// <param name="legsHeight"></param>
        /// <param name="legSpacing"></param>
        public StoolParameters(double seatWidth, double seatHeight, 
            double legsWidtht, double legsHeight, double legSpacing)
        {
            SeatWidth = seatWidth;
            SeatHeight = seatHeight;
            LegsWidth = legsWidtht;
            LegsHeight = legsHeight;
            LegSpacing = legSpacing;           
        }

            //"Ширина сиденья должна быть 300 – 400 мм \n" +
            //"Высота сиденья должна быть 10 – 50 мм \n" +
            //"Толщина ножек должна быть 20 – 60 мм \n" +
            //"Высота ножек должна быть 300 – 500 мм \n" +
            //"Расстояние между ножками должна быть 190 – 230 мм \n";
    }
}
