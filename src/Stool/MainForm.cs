using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StoolModel;
using Stool.Wrapper;

namespace Stool
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Цвет поля при ошибке
        /// </summary>
        private readonly Color ErrorColor = Color.LightPink;

        /// <summary>
        /// Цвет поля при корректных данных
        /// </summary>
        private readonly Color OkColor = Color.White;

        public MainForm()
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

        KompasWrapper _kompasWrapper = new KompasWrapper();

        private void BuildButton_Click(object sender, EventArgs e)
        {
            try
            {
                StoolParameters stool = null;

                stool = new StoolParameters(350, 30, 40, 400, 210);
                _kompasWrapper.StartKompas();
                _kompasWrapper.BuildStool(stool);
            }
            catch
            {
                MessageBox.Show("Невозможно построить деталь!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
