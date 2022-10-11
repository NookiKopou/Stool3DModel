using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stool
{
    public partial class StoolForm : Form
    {
        /// <summary>
        /// Цвет поля при ошибке
        /// </summary>
        private readonly Color ErrorColor = Color.LightPink;

        /// <summary>
        /// Цвет поля при корректных данных
        /// </summary>
        private readonly Color OkColor = Color.White;

        public StoolForm()
        {
            InitializeComponent();
        }

        private void SeatWidthTextBox_TextChanged(object sender, EventArgs e)
        {
            //SeatWidthTextBox.BackColor = ErrorColor;
        }

        private void SeatHeighTextBox_TextChanged(object sender, EventArgs e)
        {
            //SeatHeighTextBox.BackColor = ErrorColor;
        }

        private void LegsWidthTextBox_TextChanged(object sender, EventArgs e)
        {
            //LegsWidthTextBox.BackColor = ErrorColor;
        }

        private void LegsHeightTextBox_TextChanged(object sender, EventArgs e)
        {
            //LegsHeightTextBox.BackColor = ErrorColor;
        }

        private void LegSpacingTextBox_TextChanged(object sender, EventArgs e)
        {
            //LegSpacingTextBox.BackColor = ErrorColor;
        }

        private void BuildButton_Click(object sender, EventArgs e)
        {
            //ErrorsLabel.Text = "Ширина сиденья должна быть 300 – 400 мм \n" +
            //"Высота сиденья должна быть 10 – 50 мм \n" +
            //"Толщина ножек должна быть 20 – 60 мм \n" +
            //"Высота ножек должна быть 300 – 500 мм \n" +
            //"Расстояние между ножками должна быть 190 – 230 мм \n";
            //ErrorsLabel.ForeColor = Color.Red;
        }

        private void LegSpacingMMLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
