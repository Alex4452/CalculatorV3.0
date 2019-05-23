using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculatorV3._0
{
    public partial class CalcNorm : Form
    {
        /// <summary>
        /// <see cref = "firstNum"> Первое заносимое число </see>
        /// <see cref = "secondNum"> Второе заносимое число </see>
        /// <see cref = "checkedResult"> Проверка на выполнение метода Result </see>
        /// <see cref = "checkedReading"> Проверка на выполнение метода Reading </see>
        /// <see cref = "chClickZnack"> Проверка на нажатие знака </see>
        /// <see cref = "MatOper"> Делегат для математических операций </see>
        /// <see cref = "oper"> Переменная делегата </see>
        /// </summary>

        protected double firstNum = 0;
        protected double secondNum = 0;
        protected double result = 0;
        internal delegate double MatOper(double first, double second);
        internal MatOper oper = null;
        internal bool checkedResult = false;
        internal bool checkedReading = false;
        bool chClickZnack = false;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CalcNorm"/>.
        /// </summary>
        public CalcNorm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Left = 20;
        }

        //---------------------------------------------------
        // Проверка нанажатие =
        //---------------------------------------------------

        protected virtual void textBox1_TextChanged(object sender, EventArgs e)
        {
            FocusTextBox();

            if (checkedResult)
            {
                secondNum = 0;
                textBox1.Clear();
                label1.Text = "";
                checkedResult = false; 
            }

            if (checkedReading && textBox1.Text != "")
            {
                textBox1.Clear();
                checkedReading = false;
            }
            

            FocusTextBox();
        }

        //---------------------------------------------------
        // Проверка на вводимые значения
        //---------------------------------------------------

        internal virtual void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            FocusTextBox();

            if (e.KeyChar == '0')
            {
                if(textBox1.Text.IndexOf('0') != -1 && textBox1.Text.Length == 1)
                {
                    // если ноль уже есть
                    e.Handled = true;
                }
            }
            
            if (textBox1.Text == "0" && e.KeyChar != ',' &&  textBox1.Text.IndexOf(',') == -1)
            {
                if (textBox1.Text.Length != 0)
                    textBox1.Text = textBox1.Text.Remove(0, 1);
            }

            if (char.IsNumber(e.KeyChar))
            {
                chClickZnack = false;
                return;
            }

            if (e.KeyChar == '.')
            {
                if (textBox1.Text.IndexOf(',') != -1)
                {
                    // если запятая уже есть
                    e.Handled = true;
                }
                // заменяем точку на запятую
                else
                {
                    e.Handled = true;
                    butComma.PerformClick();
                    return;
                }
            }

            if(e.KeyChar == ',')
            {
                if (textBox1.Text.IndexOf(',') != -1)
                {
                    // если запятая уже есть
                    e.Handled = true;
                }
                else
                {
                    e.Handled = true;
                    butComma.PerformClick();
                    return;
                }
            }

            if (char.IsControl(e.KeyChar))
            {
                // Enter, Backspace, Esc
                if(e.KeyChar == (char)Keys.Enter)
                {
                    // устанавливаем Enter на =
                    e.Handled = true;
                    butEqal.PerformClick();
                    //textBox1.Focus();
                }

                if(e.KeyChar == (char)Keys.Back)
                {
                    // устанавливаем Backspace на <-
                    e.Handled = true;
                    butDelOne.PerformClick();
                }

                return;
            }

            if(e.KeyChar == '+')
            {
                e.Handled = true;
                if (textBox1.Text.Length != 0)
                    textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                butAdd.PerformClick();
                return;
            }

            if(e.KeyChar == '-')
            {
                e.Handled = true;
                if(textBox1.Text.Length != 0)
                    textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                butSub.PerformClick();
                return;
            }

            if (e.KeyChar == '*')
            {
                e.Handled = true;
                if (textBox1.Text.Length != 0)
                    textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                butMult.PerformClick();
                return;
            }

            if (e.KeyChar == '/')
            {
                e.Handled = true;
                if (textBox1.Text.Length != 0)
                    textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                butDivision.PerformClick();
                return;
            }
            
    
            e.Handled = true;
        }

        //--------------------------------------------------------
        // Переключение фокуса на textBox1
        //--------------------------------------------------------

        internal void FocusTextBox()
        {
            textBox1.Focus();
            if (!string.IsNullOrEmpty(textBox1.Text))
                textBox1.Select(textBox1.Text.Length, textBox1.Text.Length);
        }

        //-----------------------------------
        // Цифры и запятая (вывод в textBox1)
        //------------------------------------

        protected virtual void ClickNum(object text)    // Нажатие на цифру 
        {
            if (textBox1.Text.Length != 0)
                if (textBox1.Text[0] == '0' && textBox1.Text.IndexOf(',') == -1)
                    textBox1.Text = textBox1.Text.Remove(0, 1);

            chClickZnack = false;
            textBox1.Text += (text as Button).Text;
            FocusTextBox();
        }

        protected virtual void but6_Click(object sender, EventArgs e)
        {
            ClickNum(sender);
        }

        protected virtual void but7_Click(object sender, EventArgs e)
        {
            ClickNum(sender);
        }

        protected virtual void but8_Click(object sender, EventArgs e)
        {
            ClickNum(sender);
        }

        protected virtual void but4_Click(object sender, EventArgs e)
        {
            ClickNum(sender);
        }

        protected virtual void but5_Click(object sender, EventArgs e)
        {
            ClickNum(sender);
        }

        protected virtual void but9_Click(object sender, EventArgs e)
        {
            ClickNum(sender);
        }

        protected virtual void but3_Click(object sender, EventArgs e)
        {
            ClickNum(sender);
        }

        protected virtual void but2_Click(object sender, EventArgs e)
        {
            ClickNum(sender);
        }

        protected virtual void but1_Click(object sender, EventArgs e)
        {
            ClickNum(sender);
        }

        protected virtual void but0_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.IndexOf('0') == -1 || textBox1.Text.Length != 1)
                ClickNum(sender);
            FocusTextBox();
        }

        protected virtual void butComma_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.IndexOf(',') == -1)                        
            {
                if (textBox1.Text.Length == 0)
                    textBox1.Text += "0";

                textBox1.Text += (sender as Button).Text;
            }

            FocusTextBox();
        }

        //------------------------------------------------
        // Знаки действия
        //------------------------------------------------

        protected virtual void Reading(object sender, EventArgs e)  // считывание в firstNum
        {
            if (!chClickZnack)
            {
                firstNum = 0;
                secondNum = 0;
                checkedReading = true;
                try
                {
                    checked
                    {
                        firstNum = Convert.ToDouble(textBox1.Text);
                    }
                }
                catch (Exception)
                {
                    textBox1.Text = "Введите число.";
                }

                textBox1.Clear();
                if (label1.Text == "0" || checkedResult == true)
                    label1.Text = "";

                if (oper == null)
                {
                    if (firstNum < 0)
                        label1.Text += "(" + firstNum.ToString() + ")" + " ";
                    else
                        label1.Text += firstNum.ToString();
                }
                

                label1.Text += " " + (sender as Button).Text + " ";
                textBox1.Text = result.ToString();

            }
            textBox1.Focus();
        }

        protected virtual void butAdd_Click(object sender, EventArgs e)
        {
            if (!chClickZnack)
            {
                if (oper != null)
                    Result(firstNum);

                Reading(sender, e);
                oper += Add;
                chClickZnack = true;
            }

            FocusTextBox();
        }

        protected virtual void butSub_Click(object sender, EventArgs e)
        {
            if (!chClickZnack)
            {
                if (oper != null)
                    Result(firstNum);
                if (!chClickZnack)
                    Reading(sender, e);
                oper += Sub;
                chClickZnack = true;
            }

            FocusTextBox();
        }

        protected virtual void butMult_Click(object sender, EventArgs e)
        {
            if (!chClickZnack)
            {
                if (oper != null)
                    Result(firstNum);
                if (!chClickZnack)
                    Reading(sender, e);
                oper += Mult;
                chClickZnack = true;
            }

            FocusTextBox();
        }

        protected virtual void butDivision_Click(object sender, EventArgs e)
        {
            if (!chClickZnack)
            {
                if (oper != null)
                    Result(firstNum);
                if (!chClickZnack)
                    Reading(sender, e);
                oper += Divide;
                chClickZnack = true;
            }

            FocusTextBox();
        }

        //--------------------------------------------------------
        // Методы математических операций
        //--------------------------------------------------------

        internal virtual double Add(double first, double second)
        {
            return (first + second);
        }

        internal virtual double Sub(double first, double second)
        {
            return (first - second);
        }

        internal virtual double Mult(double first, double second)
        {
            return (checked(first * second));
        }

        internal virtual double Divide(double first, double second)
        {
            try
            {
                if (second == 0)
                {
                    textBox1.Text = "Неверный ввод.";
                    checkedResult = true;
                    return 0;
                }
                else
                    return (first / second);
            }
            catch (DivideByZeroException e)
            {
                textBox1.Clear();
                label1.Text = " ";
                textBox1.Text = "На ноль делить нельзя.";
                checkedResult = true;
                return 0;
            }
        }

        internal virtual bool Factorial(double n, out double ansver)
        {
            int f;
            int k;
            bool ok = true;

            if (n < 0)
                ok = false;
            try
            {
                checked
                {
                    f = 1;
                    for (k = 2; k <= n; k++)
                    {
                        f = f * k;
                    }
                }
            }
            catch (Exception)
            {
                f = 0;
                ok = false;
            }
            ansver = f;
            return ok;
        }

        //--------------------------------------------------------
        // Результат вычислений
        //--------------------------------------------------------

        internal virtual void Result(double first)
        {
            try
            {
                checked
                {
                    secondNum = Convert.ToDouble(textBox1.Text);
                }
            }
            catch (Exception)
            {
                textBox1.Text = "Введите число.";
            }
            
            if (label1.Text == "0")
                label1.Text = "";

            if (!checkedReading)
            {
                if (secondNum < 0)
                    label1.Text += "(" + secondNum.ToString() + ")";
                else
                    label1.Text += secondNum.ToString();
            }

            if (oper != null)
            {
                result = oper(first, secondNum);
                textBox1.Text = result.ToString();
            }

            chClickZnack = false;
            checkedReading = false;
        }

        internal virtual void butEqal_Click(object sender, EventArgs e)
        {
            if(!checkedResult)
                Result(firstNum);
            checkedResult = true;
            oper = null;

            FocusTextBox();
        }
        
        //-----------------------------------------------
        // Удаление и очистка textBox1
        //-----------------------------------------------

        protected virtual void butDelAll_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            label1.Text = "";
            firstNum = secondNum = 0;

            FocusTextBox();
        }

        protected virtual void butDelOne_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0)
            {
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                if (textBox1.Text.Length != 0)
                {
                    if (textBox1.Text[0] == '-' && textBox1.Text.Length == 1)
                        textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                }
            }
                
            FocusTextBox();
        }

        //-----------------------------------------------
        // Смена знака
        //-----------------------------------------------

        private void butPlusMin_Click(object sender, EventArgs e)
        {
            double num = 0;
            try
            {
                checked
                {
                    num = Convert.ToDouble(textBox1.Text);
                }
            }
            catch (Exception)
            {
                textBox1.Text = "Введите число.";
            }
            num *= -1;
            textBox1.Text = num.ToString();

            FocusTextBox();
        }
        
        //------------------------------------------------
        // Верхняя панель
        //------------------------------------------------

        private void обычныйToolStrip_Click(object sender, EventArgs e)
        {
            checkedResult = false;
            checkedReading = false;
            Hide();
            CalcNorm f1 = new CalcNorm();
            f1.ShowDialog();
            this.Close();
        }

        private void инженерныйToolStrip_Click(object sender, EventArgs e)
        {
            checkedResult = false;
            checkedReading = false;
            Hide();
            CalcEng f2 = new CalcEng();
            f2.ShowDialog();
            this.Close();
        }

        private void программистToolStrip_Click(object sender, EventArgs e)
        {
            checkedResult = false;
            checkedReading = false;
            Hide();
            CalcPr f3 = new CalcPr();
            f3.ShowDialog();
            this.Close();
        }
    }
}
