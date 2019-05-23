using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Media;

namespace CalculatorV3._0
{
    public partial class CalcEng : CalcNorm
    {
        /// <summary>
        /// <see cref = "chClose"> Проверка на нажатие на ) </see>
        /// <see cref = "chZnakClose"> Проверка на нажатие на ) для знаков </see>
        /// <see cref = "chClickZnack"> Проверка на нажатие знака </see>
        /// <see cref = "chNumClose"> Проверка на нажатие на ) для цифр </see>
        /// <see cref = "countResult"> Подсчет нажатий на = </see>
        /// <see cref = "current"> Строка для чисел для записи в польскую нотацию </see>
        /// <see cref = "signs"> Стек для знаков для записи в польскую нотацию </see>
        /// <see cref = "operand"> Строка для записи в стек для нахлждения ответа </see>
        /// <see cref = "ansv"> Стек для подсчета ответа сложного выражения записанного в польской нотации </see>
        /// <see cref = "countOpen"> Переменная для посчета кол-ва открывающих скобок </see>
        /// <see cref = "countClose"> Переменная для посчета кол-ва закрывающих скобок </see>
        /// <see cref = "neg"> Проверка на нажатие смены знака </see>
        /// <see cref = "temp"> Переменная для сохранения значения с измененным знаком для вывода в lable1 </see>
        /// </summary>

        bool chClose = false;       
        bool chZnakClose = false;
        bool chClickZnack = false;
        bool chNumClose = false;
        bool neg = false;

        double temp = 0;
        int countResult = 0;

        string current = "";        
        Stack<char> signs = new Stack<char>(); 
        
        string operand = "";
        Stack<double> ansv = new Stack<double>();


        int countOpen = 0;
        int countClose = 0;

        public CalcEng()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        //--------------------------------------------------------
        // Возникает при изменении textBox1
        //--------------------------------------------------------

        protected override void textBox1_TextChanged(object sender, EventArgs e)
        {
            FocusTextBox();
            label2.Visible = false;

            if (checkedResult)
            {
                textBox1.Clear();
                label1.Text = "";
                checkedResult = false;
                if (chClickZnack)
                    chClickZnack = false;

                FocusTextBox();
            }

            if (chClose)
            {
                textBox1.Clear();
                chNumClose = true;
                chClose = false;

                FocusTextBox();
            }

            if (chClickZnack)
            {
                chClickZnack = false;

                FocusTextBox();
            }

            FocusTextBox();
        }

        //--------------------------------------------------------
        // Возникает при изменении radioButton
        //--------------------------------------------------------

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Focus();
            if (!string.IsNullOrEmpty(textBox1.Text))
                textBox1.Select(textBox1.Text.Length, textBox1.Text.Length);
        }

        //--------------------------------------------------
        // Подсчет сложных вычислений
        //--------------------------------------------------{

        //--------------------------------------------------
        // Смена знака
        //--------------------------------------------------

        internal virtual void Negative(string text)
        {
            neg = true;

            chZnakClose = true;

            try
            {
                temp = checked(Convert.ToDouble(textBox1.Text));
            }
            catch (Exception)
            {
                butDelAll.PerformClick();
                textBox1.Text = "Неверный ввод.";
                checkedResult = true;
            }

            temp *= -1;
            textBox1.Clear();
            chZnakClose = true;

            string str = "(" + temp.ToString() + "-" + (2 * temp).ToString() + ")";
            ExpresstionToRPN(str);
            RPNToAnsver(current);
        }  

        //---------------------------------------------------------
        // Переопределение считывания значения
        //---------------------------------------------------------

        protected override void Reading(object sender, EventArgs e)
        {
            if (!chClickZnack)
            {
                if (textBox1.Text.Contains("-") && !chClose)
                {
                    Negative(textBox1.Text);
                }

                if (!chZnakClose)
                    ExpresstionToRPN(textBox1.Text + " " + (sender as Button).Text + " ");
                else if (chZnakClose && chNumClose)
                    ExpresstionToRPN(textBox1.Text + " " + (sender as Button).Text + " ");
                else
                    ExpresstionToRPN(" " + (sender as Button).Text + " ");

                if (label1.Text == "0")
                    label1.Text = "";

                textBox1.Clear();

                chZnakClose = false;
                chNumClose = false;
                chClickZnack = true;
            }
        }

        //------------------------------------------------
        // Знаки действия
        //------------------------------------------------

        protected override void butAdd_Click(object sender, EventArgs e)    // Сложение
        {
            Reading(sender, e);
            FocusTextBox();
        }

        protected override void butSub_Click(object sender, EventArgs e)    // Вычитание
        {
            Reading(sender, e);
            FocusTextBox();
        }

        protected override void butMult_Click(object sender, EventArgs e)   // Умножение
        {
            Reading(sender, e);
            FocusTextBox();
        }

        protected override void butDivision_Click(object sender, EventArgs e)  // Деление
        {
            Reading(sender, e);
            FocusTextBox();
        }

        private void butXY_Click(object sender, EventArgs e)    // Возведение в степень
        {
            Reading(sender, e);
            FocusTextBox();
        }

        private void butYQuarX_Click(object sender, EventArgs e)    // Извлечение корня
        {
            Reading(sender, e);
            FocusTextBox();
        }

        private void butMod_Click(object sender, EventArgs e)       // Остаток от деления
        {
            if (!chClickZnack)
            {
                if (textBox1.Text.Contains("-") && !chClose)
                {
                    Negative(textBox1.Text);
                }

                if (!chZnakClose)
                    ExpresstionToRPN(textBox1.Text + "%");
                else if (chZnakClose && chNumClose)
                    ExpresstionToRPN(textBox1.Text + "%");
                else
                    ExpresstionToRPN("%");

                if (label1.Text == "0")
                    label1.Text = "";

                textBox1.Clear();

                chZnakClose = false;
                chNumClose = false;
                chClickZnack = true;
            }
            FocusTextBox();
        }

        //--------------------------------------------------------
        // Переопределение результата вычислений (для сложных выражений)
        //--------------------------------------------------------

        internal override void butEqal_Click(object sender, EventArgs e)  
        {
            if (textBox1.Text == "")
                textBox1.Text = "0";

            if (textBox1.Text.Contains("-") && !chClose)
            {
                Negative(textBox1.Text);
            }

            if (countClose != countOpen)
            {
                //MessageBox.Show("Не хватает скобок.", "Внимание!", MessageBoxButtons.OK);
                SystemSounds.Beep.Play();
                label2.Visible = true;
            }
            else
            {
                ExpresstionToRPN(textBox1.Text);

                while (signs.Count > 0)
                    current += signs.Pop();

                if (!checkedResult)
                    textBox1.Text = Convert.ToString(RPNToAnsver(current));

                current = "";

                countResult++;
                checkedResult = true;
                chZnakClose = false;
                chNumClose = false;

                if (ansv.Count != 0)
                    while (ansv.Count != 0)
                        ansv.Pop();
            }
                

            FocusTextBox();
        }

        //---------------------------------------------------
        // Приоритет считываемых значений ( ^|√ = 4; *|/|% = 3 ; +|- = 2 ; ( = 1 ; ) = -1 ; number = 0 )
        //---------------------------------------------------

        private int GetPr(char token)
        {
            if (token == '^' || token == '√')
                return 4;
            else if (token == '*' || token == '/' || token == '%')
                return 3;
            else if (token == '+' || token == '-')
                return 2;
            else if (token == '(')
                return 1;
            else if (token == ')')
                return -1;
            else
                return 0;
        }

        //---------------------------------------------------
        // Запись в польскую нотацию ( ^|√ = 4; *|/|% = 3 ; +|- = 2 ; ( = 1 ; ) = -1 ; number = 0 )
        //---------------------------------------------------

        internal void ExpresstionToRPN(string expr)
        {
            int countHowMuchDelete = 0;     // Считает сколько нужно удалить при изменении выражения
            int priorety;                   // Приоритет считываемых значений

            if (checkedResult)
            {
                checkedResult = false;
                label1.Text = "";
            }
                
            for(int i = 0; i< expr.Length; i++)
            {
                priorety = GetPr(expr[i]);

                if (label1.Text == "0" || checkedResult == true)
                    label1.Text = "";

                if (priorety == 0)
                {
                    if (chNumClose)
                    {
                        current = current.Remove(current.Length - countHowMuchDelete , countHowMuchDelete );      
                        current += expr[i];

                        chNumClose = false;

                        int countBeforeOpenBr = 1;
                        string str = "";

                        for (int j = label1.Text.Length - 1; j > 0; j--)
                        {
                            if (label1.Text[j] != '(')
                                countBeforeOpenBr++;
                            else
                                break;
                        }

                        if (label1.Text.Length > 0)
                            str = label1.Text.Remove(label1.Text.Length - countBeforeOpenBr, countBeforeOpenBr);

                        label1.Text = str;
                    }
                    else
                        current += expr[i];
                }

                if (priorety == 1)
                {
                    if (chZnakClose)
                    {
                        current = current.Remove(current.Length - countHowMuchDelete, countHowMuchDelete);
                        signs.Push(expr[i]);
                        chZnakClose = false;
                    }
                    else
                        signs.Push(expr[i]);
                }

                if(priorety > 1)
                {
                    current += " ";

                    if (chZnakClose)
                        chZnakClose = false;

                    while(signs.Count > 0)
                    {
                        if (GetPr(signs.Peek()) >= priorety)
                            current += signs.Pop();
                        else
                            break;
                    }
                    signs.Push(expr[i]);
                }

                if(priorety == -1)
                {
                    current += " ";

                    if (GetPr(signs.Peek()) == -1)
                        expr = ")";

                    while(GetPr(signs.Peek()) != 1)
                    {
                        if(signs.Peek() != ' ')
                            countHowMuchDelete++;
                        current += signs.Pop();
                    }

                    countHowMuchDelete = ((2 * countHowMuchDelete) + 1) + (2 * countHowMuchDelete);
                        
                    signs.Pop();
                }
            }

            if (!neg)
                label1.Text += expr;
            else
            {
                label1.Text += "-" + temp.ToString();
                temp = 0;
                neg = false;
            }
        }

        //---------------------------------------------------
        // Нахождение ответа из записи в польской нотации
        //---------------------------------------------------

        internal double RPNToAnsver(string rpn)
        {
            for (int i = 0; i < rpn.Length; i++)
            {
                if (rpn[i] == ' ') continue;

                if (GetPr(rpn[i]) == 0)
                {
                    while(rpn[i] != ' ' && GetPr(rpn[i]) == 0)
                    {
                        operand += rpn[i++];
                        if (i == rpn.Length)
                            break;
                    }

                    try
                    {
                        ansv.Push(checked(double.Parse(operand)));
                    }
                    catch (Exception)
                    {
                        butDelAll.PerformClick();
                        textBox1.Text = "Неверный ввод.";
                        checkedResult = true;
                    }   
                    
                    operand = "";
                }

                try
                {
                    if (GetPr(rpn[i]) > 1)
                    {
                        char znak = rpn[i];
                        double sec = 0;
                        double first = 0;

                        if (ansv.Count != 0)
                            sec = ansv.Pop();

                        if (ansv.Count != 0)
                            first = ansv.Pop();

                        if (znak == '+')
                            ansv.Push(Add(first, sec));
                        if (znak == '-')
                            ansv.Push(Sub(first, sec));
                        if (znak == '*')
                            ansv.Push(Mult(first, sec));
                        if (znak == '/')
                            ansv.Push(Divide(first, sec));
                        if (znak == '^')
                            ansv.Push(Math.Pow(first, sec));
                        if (znak == '√')
                        {
                            if (first >= 0 && sec != 0)
                                ansv.Push(Math.Pow(first, (1 / sec)));
                            else
                            {
                                textBox1.Text = "Неверное значение.";
                                checkedResult = true;
                                chClickZnack = true;
                            }
                        }
                        if (znak == '%')
                        {
                            if (sec != 0)
                                ansv.Push(first % sec);
                            else
                            {
                                textBox1.Text = "Неверное значение.";
                                checkedResult = true;
                                chClickZnack = true;
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            if (ansv.Count != 0)
            {
                if (!chClose)
                {
                    return ansv.Pop();
                }
                else
                {
                    chClose = false;
                    return ansv.Peek();
                }
            }
            else
                return 0;
           
        }

        //---------------------------------------------------}

        //--------------------------------------------------
        // Скобки
        //--------------------------------------------------

        internal virtual void butOpenBr_Click(object sender, EventArgs e)
        {
            if (label1.Text == "0")
                label1.Text = "";

            countOpen++;

            if (chZnakClose)
            {
                int countBeforeOpenBr = 1;
                string str = "";

                for(int j = label1.Text.Length - 1; j > 0; j--)
                {
                    if (label1.Text[j] != '(')
                        countBeforeOpenBr++;
                    else
                        break;
                }

                if(label1.Text.Length > 0)
                    str = label1.Text.Remove(label1.Text.Length - countBeforeOpenBr, countBeforeOpenBr);

                label1.Text = str;

                countOpen--;
                countClose--;
                textBox1.Clear();
            }

            ExpresstionToRPN((sender as Button).Text);

            FocusTextBox();
        }

        internal virtual void butCloseBr_Click(object sender, EventArgs e)
        {
            if (countClose >= countOpen)
            {
                System.Media.SystemSounds.Asterisk.Play();
            }
            else
            {
                countClose++;

                if (textBox1.Text == "")
                    textBox1.Text = "0";

                if (label1.Text == "0")
                    label1.Text = "";

                if (textBox1.Text.Contains("-") && !chClose)
                {
                    Negative(textBox1.Text);
                }


                if (label1.Text.Last() != ')' && !chZnakClose)
                    ExpresstionToRPN(textBox1.Text + (sender as Button).Text);
                else
                    ExpresstionToRPN((sender as Button).Text);

                if (countClose == countOpen)
                {
                    chZnakClose = false;

                    while (signs.Count > 0)
                        current += signs.Pop();
                }

                chClose = true;
                chZnakClose = true;

                textBox1.Text = (RPNToAnsver(current)).ToString();

                chClose = true;

                FocusTextBox();
            }
        }

        //--------------------------------------------------
        // Переопределение проверки на вводимое значение
        //--------------------------------------------------

        internal override void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            FocusTextBox();

            if (e.KeyChar == '0')
            {
                if (textBox1.Text.IndexOf('0') != -1 && textBox1.Text.Length == 1)
                {
                    // если ноль уже есть
                    e.Handled = true;
                }
            }

            if (textBox1.Text == "0" && e.KeyChar != ',' && textBox1.Text.IndexOf(',') == -1)
            {
                if (textBox1.Text.Length != 0)
                    textBox1.Text = textBox1.Text.Remove(0, 1);
            }

            if (char.IsNumber(e.KeyChar))
            {
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

            if (e.KeyChar == ',')
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
                if (e.KeyChar == (char)Keys.Enter)
                {
                    // устанавливаем Enter на =
                    e.Handled = true;
                    butEqal.PerformClick();
                    //textBox1.Focus();
                }

                if (e.KeyChar == (char)Keys.Back)
                {
                    // устанавливаем Backspace на <-
                    e.Handled = true;
                    butDelOne.PerformClick();
                }

                return;
            }

            if (e.KeyChar == '+')
            {
                e.Handled = true;
                if (textBox1.Text.Length != 0)
                    textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                butAdd.PerformClick();
                return;
            }

            if (e.KeyChar == '-')
            {
                e.Handled = true;
                if (textBox1.Text.Length != 0)
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

            if(e.KeyChar == '^')
            {
                e.Handled = true;
                if (textBox1.Text.Length != 0)
                    textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                butXY.PerformClick();
                return;
            }

            if (e.KeyChar == '√')
            {
                e.Handled = true;
                if (textBox1.Text.Length != 0)
                    textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                butYQuarX.PerformClick();
                return;
            }

            if (e.KeyChar == '(')
            {
                e.Handled = true;
                if (textBox1.Text.Length != 0)
                    textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                butOpenBr.PerformClick();
                return;
            }

            if (e.KeyChar == ')')
            {
                e.Handled = true;
                if (textBox1.Text.Length != 0)
                    textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                butCloseBr.PerformClick();
                return;
            }


            e.Handled = true;
        }

        //--------------------------------------------------
        // Переопределение очистки всего
        //--------------------------------------------------

        protected override void butDelAll_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            label1.Text = "";
            firstNum = secondNum = 0;

            current = "";
            operand = "";

            if(signs.Count != 0)
                signs.Clear();
            if (ansv.Count != 0)
                ansv.Clear();

            chClose = false;
            chClickZnack = false;
            chZnakClose = false;
            chNumClose = false;

            countResult = 0;

            countOpen = 0;
            countClose = 0;

            FocusTextBox();
        }

        //--------------------------------------------------
        // Натуральный логарифм
        //--------------------------------------------------

        private void butLN_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = "0";

            double i = Convert.ToDouble(textBox1.Text);
            i = Math.Log(i);
            textBox1.Text = i.ToString();

            FocusTextBox();
        }

        //---------------------------------------------------
        // Десятичный логарифм
        //---------------------------------------------------

        private void butLog_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = "0";

            double i = Convert.ToDouble(textBox1.Text);
            i = Math.Log10(i);
            textBox1.Text = i.ToString();

            FocusTextBox();
        }

        //--------------------------------------------------
        // Возведение в квадрат
        //--------------------------------------------------

        private void butX2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = "0";

            double i = Convert.ToDouble(textBox1.Text);
            i = Math.Pow(i,2);
            textBox1.Text = i.ToString();

            FocusTextBox();
        }

        //----------------------------------------------------
        // Факториал
        //----------------------------------------------------

        private void butFactorial_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = "0";

            double i = Convert.ToDouble(textBox1.Text);
            double j;
            if (Factorial(i, out j))
                textBox1.Text = j.ToString();
            else
            {
                textBox1.Text = "Неверный ввод.";
                checkedResult = true;
            }

            FocusTextBox();
        }

        //----------------------------------------------------
        // Извлечение квадратного корня
        //----------------------------------------------------

        private void butQuarX_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = "0";

            double i = Convert.ToDouble(textBox1.Text);
            if (i >= 0)
            {
                i = Math.Pow(i, 0.5);
                textBox1.Text = i.ToString();
            }
            else
            {
                textBox1.Text = "Неверное значение.";
                chClickZnack = true;
                checkedReading = true;
            }

            FocusTextBox();
        }

        //----------------------------------------------------
        // Возведение 10 в определенную степень
        //----------------------------------------------------

        private void but10X_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = "0";

            double i = Convert.ToDouble(textBox1.Text);
            i = Math.Pow(10, i);
            textBox1.Text = i.ToString();

            FocusTextBox();
        }
        
        //----------------------------------------------------
        // Синус 
        //----------------------------------------------------

        private void butSin_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = "0";

            double x = Convert.ToDouble(textBox1.Text);

            if (label1.Text == "0")
                label1.Text = "";

            if (radioButGragus.Checked)
                x = x * 0.0175;
            else if (radioButGrad.Checked)
                x = x * 0.0157;

            x = Math.Sin(x);
            textBox1.Text = x.ToString();

            FocusTextBox();
        }

        //----------------------------------------------------
        // Косинус 
        //----------------------------------------------------

        private void butCos_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = "0";

            double x = Convert.ToDouble(textBox1.Text);

            if (label1.Text == "0")
                label1.Text = "";

            if (radioButGragus.Checked)
                x = x * 0.0175;
            else if (radioButGrad.Checked)
                x = x * 0.0157;

            x = Math.Cos(x);
            textBox1.Text = x.ToString();

            FocusTextBox();
        }

        //------------------------------------------------------
        // Тангенс 
        //------------------------------------------------------

        private void butTg_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = "0";

            double x = Convert.ToDouble(textBox1.Text);

            if (label1.Text == "0")
                label1.Text = "";

            if (radioButGragus.Checked)
                x = x * 0.0175;
            else if (radioButGrad.Checked)
                x = x * 0.0157;

            x = Math.Tan(x);
            textBox1.Text = x.ToString();

            FocusTextBox();
        }

        //-------------------------------------------------------
        // Арксинус числа
        //-------------------------------------------------------

        private void butASin_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = "0";

            double x = Convert.ToDouble(textBox1.Text);
            x = Math.Asin(x);
            textBox1.Text = x.ToString();

            FocusTextBox();
        }

        //--------------------------------------------------------
        // Арккосинус числа
        //--------------------------------------------------------

        private void butACos_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = "0";

            double x = Convert.ToDouble(textBox1.Text);
            x = Math.Acos(x);
            textBox1.Text = x.ToString();

            FocusTextBox();
        }

        //---------------------------------------------------------
        // Арктангенс числа
        //---------------------------------------------------------

        private void butATg_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = "0";

            double x = Convert.ToDouble(textBox1.Text);
            x = Math.Atan(x);
            textBox1.Text = x.ToString();

            FocusTextBox();
        }

        //--------------------------------------------------------
        // Число Пи
        //--------------------------------------------------------

        private void butPi_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox1.Text = Math.PI.ToString();

            FocusTextBox();
        }

        //---------------------------------------------------------
        // Преобразование в целочисленный тип
        //---------------------------------------------------------

        private void butInt_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                textBox1.Text = "0";

            double x = Convert.ToDouble(textBox1.Text);
            textBox1.Text = ((int)x).ToString();

            FocusTextBox();
        }

        //--------------------------------------------------------
        // Число e
        //--------------------------------------------------------

        private void butE_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox1.Text = Math.E.ToString();

            FocusTextBox();
        }
    }
}
