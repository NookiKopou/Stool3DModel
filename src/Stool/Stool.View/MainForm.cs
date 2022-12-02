using System;
using System.Drawing;
using System.Windows.Forms;
using StoolModel;
using Stool.Wrapper;
using System.Collections.Generic;
using System.Linq;

namespace Stool
{
    /// <summary>
    /// Класс основной формы
    /// </summary>
    public partial class MainForm : Form 
    {
        /// <summary>
        /// Объект StoolParameters
        /// </summary>
        private readonly StoolParameters _stoolParameters = new StoolParameters(350, 20, 40, 400, 210);

        /// <summary>
        /// Словарь для введенных параметров
        /// </summary>
        private readonly Dictionary<ParameterType, TextBox> _parameterToTextBox;

        /// <summary>
        /// Объект StoolBuilder
        /// </summary>
        private readonly StoolBuilder _stoolBuilder = new StoolBuilder();

        /// <summary>
        /// Цвет поля при ошибке
        /// </summary>
        private readonly Color _errorColor = Color.LightPink;

        /// <summary>
        /// Цвет поля при корректных данных
        /// </summary>
        private readonly Color _okColor = Color.White;

        /// <summary>
        /// Конструктор
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            _parameterToTextBox = new Dictionary<ParameterType, TextBox>
            {
                { ParameterType.SeatWidth, SeatWidthTextBox },
                { ParameterType.SeatHeight, SeatHeightTextBox },
                { ParameterType.LegsWidth, LegsWidthTextBox },
                { ParameterType.LegsHeight, LegsHeightTextBox },
                { ParameterType.LegSpacing, LegSpacingTextBox }
            };

            // Событие при вводе символов в форму

            SeatWidthTextBox.KeyPress += CheckBannedCharacters;
            SeatHeightTextBox.KeyPress += CheckBannedCharacters;
            LegsWidthTextBox.KeyPress += CheckBannedCharacters;
            LegsHeightTextBox.KeyPress += CheckBannedCharacters;
            LegSpacingTextBox.KeyPress += CheckBannedCharacters;

            // Событие при изменении текста

            SeatWidthTextBox.TextChanged += CheckErrors;
            SeatHeightTextBox.TextChanged += CheckErrors;
            LegsWidthTextBox.TextChanged += CheckErrors;
            LegsHeightTextBox.TextChanged += CheckErrors;
            LegSpacingTextBox.TextChanged += CheckErrors;
        }

        /// <summary>
        /// Проверка введенных значений 
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">Изменение текста в TextBox</param>
        private void CheckErrors(object sender, EventArgs e)
        {
            foreach (var keyValue in _parameterToTextBox)
            {
                keyValue.Value.BackColor = _okColor;
            }
            try
            {
                var seatWidth = double.Parse(SeatWidthTextBox.Text);
                var seatHeight = double.Parse(SeatHeightTextBox.Text);
                var legsWidth = double.Parse(LegsWidthTextBox.Text);
                var legsHeight = double.Parse(LegsHeightTextBox.Text);
                var legSpacing = double.Parse(LegSpacingTextBox.Text);
                _stoolParameters.SetParameters(seatWidth, seatHeight, legsWidth, 
                    legsHeight, legSpacing);

                foreach (var keyValue in _stoolParameters.Errors)
                {
                    _parameterToTextBox[keyValue.Key].BackColor = _errorColor;
                }

                LegSpacingMMLabel.Text = _stoolParameters.Parameters[ParameterType.SeatWidth].Value - 160 + @" – " + (_stoolParameters.Parameters[ParameterType.SeatWidth].Value - 120) + @" мм";
                LegSpacingMMLabel.ForeColor = Color.Blue;
            }
            catch
            {
                CheckEmptyTextBox();
            }
        }

        /// <summary>
        /// Поиск пустых TextBox
        /// </summary>
        /// <returns>Возвращает true, если нет пустых ячеек, иначе - false</returns>
        private bool CheckEmptyTextBox()
        {
            var counter = 0;
            foreach (var keyValue in _parameterToTextBox.Where
                         (keyValue => keyValue.Value.Text == string.Empty))
            {
                counter += 1;
                keyValue.Value.BackColor = _errorColor;
            }

            return counter == 0;
        }

        /// <summary>
        ///  Запрет ввода символов и больше одной точки в число
        /// </summary>
        /// <param name="sender">TextBox</param>
        /// <param name="e">Нажатие на клавишу клавиатуры</param>
        private static void CheckBannedCharacters(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)
                && !((e.KeyChar == ',') &&
                (((TextBox)sender).Text.IndexOf
                    (",", StringComparison.Ordinal) == -1)))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Построение при нажатии на кнопку
        /// </summary>
        /// <param name="sender">Кнопка</param>
        /// <param name="e">Нажатие на кнопку</param>
        private void BuildButton_Click(object sender, EventArgs e)
        {
            if (CheckEmptyTextBox())
            {
                if (_stoolParameters.Errors.Count > 0)
                {
                    var message = string.Empty;
                    foreach (var keyValue in _stoolParameters.Errors)
                    {
                        message += $"{keyValue.Value}\n\n";
                    }
                    
                    MessageBox.Show(message, @"Данные введены неверно!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    _stoolBuilder.Build(_stoolParameters);
                }
            }
            else
            {
                MessageBox.Show(
                    @"Невозможно построить деталь! Проверьте введенные данные.",
                    @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
