using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace CalculatorV3._0
{
    public partial class CalcPr : CalcNorm
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
        /// <see cref = "chNeg"> Проверка на нажатие смены знака </see>
        /// <see cref = "bin"> Массив значений которые используются в двоичной системе счисления </see>
        /// <see cref = "oct"> Массив значений которые используются в восьмеричной системе счисления </see>
        /// <see cref = "dec"> Массив значений которые используются в десятичной системе счисления </see>
        /// <see cref = "hex"> Массив значений которые используются в 16-ричной системе счисления </see>
        /// <see cref = "radBin"> Проверка на выбор двоичной системы счисления </see>
        /// <see cref = "radOct"> Проверка на выбор восьмеричной системы счисления </see>
        /// <see cref = "radDec"> Проверка на выбор десятичной системы счисления </see>
        /// <see cref = "radHex"> Проверка на выбор 16-ричной системы счисления </see>
        /// <see cref="changeSyst"> Проверка на смену системы счисления </see>
        /// <see cref="chChecked"> Проверка на превышение максимального значения </see>
        /// <see cref="chConvResult"> Проверка на перевод в выбранную систему счисления при нахождении результата вычисслений </see>
        /// </summary>

        internal bool chClose = false;
        internal bool chZnakClose = false;
        internal bool chClickZnack = false;
        internal bool chNumClose = false;
        
        int countResult = 0;

        string current = "";
        Stack<char> signs = new Stack<char>();

        string operand = "";
        Stack<int> ansv = new Stack<int>();

        int countOpen = 0;
        int countClose = 0;

        internal char[] bin = { '0', '1' };
        internal char[] oct = { '0', '1', '2', '3', '4', '5', '6', '7' };
        internal char[] dec = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        internal char[] hex = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            'A', 'B', 'C', 'D', 'E', 'F',
            'a', 'b', 'c', 'd', 'e', 'f'};

        bool radHex = false;
        bool radDec = true;
        bool radOct = false;
        bool radBin = false;

        bool changeSyst = false;    // возникает при смене системы

        bool chChecked = false;

        bool chConvResult = false;

        bool chNeg = false;

        public CalcPr()
        {
            butComma.Enabled = false;   // что бы запятая не отображалась с начала работы 
            InitializeComponent();
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

        private void groupBox2_Enter(object sender, EventArgs e)
        {
            textBox1.Focus();
            if (!string.IsNullOrEmpty(textBox1.Text))
                textBox1.Select(textBox1.Text.Length, textBox1.Text.Length);
        }

        //--------------------------------------------------------
        // Проверка на смену системы
        //--------------------------------------------------------

        private void ChangeSystem()
        {
            if (changeSyst)
            {
                textBox1.Clear();
                chNeg = false;
                changeSyst = false;
            }
        }

        //--------------------------------------------------------
        // Видимость кнопок при различных системах счисления
        //--------------------------------------------------------

        private void radioButHex_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0)
            {
                string text = textBox1.Text;
                text = ConvertToHex(text);

                radHex = true;
                radDec = false;
                radOct = false;
                radBin = false;

                textBox1.Clear();
                text = DelNull(text);
                textBox1.Text = text;
            }

            radHex = true;
            radDec = false;
            radOct = false;
            radBin = false;

            if (!radioButHex.Checked)
            {
                butA.Enabled = false;
                butB.Enabled = false;
                butC.Enabled = false;
                butD.Enabled = false;
                butE.Enabled = false;
                butF.Enabled = false;
            }
            else
            {
                butA.Enabled = true;
                butB.Enabled = true;
                butC.Enabled = true;
                butD.Enabled = true;
                butE.Enabled = true;
                butF.Enabled = true;
            }
            changeSyst = true;
            FocusTextBox();
        }

        private void radioButDec_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0)
            {
                string text = textBox1.Text;
                text = ConvertToDec(text);

                radHex = false;
                radDec = true;
                radOct = false;
                radBin = false;

                textBox1.Clear();
                text = DelNull(text);
                textBox1.Text = text;
            }

            radHex = false;
            radDec = true;
            radOct = false;
            radBin = false;

            changeSyst = true;
            FocusTextBox();
        }

        private void radioButOct_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0)
            {
                string text = textBox1.Text;
                text = ConvertToOct(text);

                radHex = false;
                radDec = false;
                radOct = true;
                radBin = false;

                textBox1.Clear();
                text = DelNull(text);
                textBox1.Text = text;
            }

            radHex = false;
            radDec = false;
            radOct = true;
            radBin = false;

            if (radioButOct.Checked)
            {
                but8.Enabled = false;
                but9.Enabled = false;
            }
            else
            {
                but8.Enabled = true;
                but9.Enabled = true;
            }
            changeSyst = true;
            FocusTextBox();
        }

        private void radioButBin_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length != 0)
            {
                string text = textBox1.Text;
                text = ConvertToBin(text);

                radHex = false;
                radDec = false;
                radOct = false;
                radBin = true;

                textBox1.Clear();
                text = DelNull(text);
                textBox1.Text = text;
            }

            radHex = false;
            radDec = false;
            radOct = false;
            radBin = true;

            if (radioButBin.Checked)
            {
                but2.Enabled = false;
                but3.Enabled = false;
                but4.Enabled = false;
                but5.Enabled = false;
                but6.Enabled = false;
                but7.Enabled = false;
                but8.Enabled = false;
                but9.Enabled = false;
            }
            else
            {
                but2.Enabled = true;
                but3.Enabled = true;
                but4.Enabled = true;
                but5.Enabled = true;
                but6.Enabled = true;
                but7.Enabled = true;
                but8.Enabled = true;
                but9.Enabled = true;
            }
            changeSyst = true;
            FocusTextBox();
        }

        //----------------------------------------------------------
        // Удаление лишних нулей
        //----------------------------------------------------------

        internal string DelNull(string nulSt)
        {
            int count = 0;
            for (int i = 0; i < nulSt.Length - 1; i++)
            {
                if (nulSt[i] == '0')
                    count++;
                else
                    break;
            }
            nulSt = nulSt.Remove(0, count);
            return nulSt;
        }

        //----------------------------------------------------------
        // Перевод в двоичную систему счисления
        //----------------------------------------------------------

        internal string ConvertToBin(string conv)
        {
            string concl = ""; // Выходное значение

            if (radBin && !chConvResult)
            {
                if (chNeg)
                {
                    if (concl.Length < 16)
                    {
                        for (int i = 0; concl.Length != 16; i++)
                        {
                            concl = "1" + concl;
                        }
                    }
                }
                else
                {
                    if (concl.Length < 16)
                    {
                        for (int i = 0; concl.Length != 16; i++)
                        {
                            concl = "0" + concl;
                        }
                    }
                }

                concl = conv;
            }
            else if (radDec || chConvResult)    
            {
                int x = 0;
                if(conv != "")
                if (conv[0] == '-' || chNeg)
                {
                    if(conv[0] == '-')
                    conv = conv.Remove(0, 1);

                    x = Convert.ToInt32(conv);
                    while (x != 0 && x != 1)
                    {
                        if (x % 2 == 1)
                            concl = "1" + concl;
                        if (x % 2 == 0)
                            concl = "0" + concl;
                        x /= 2;
                    }

                    concl = x.ToString() + concl;

                    for(int i = 0; i < concl.Length; i++)
                    {
                        if(concl[i] == '1')
                        {
                            concl = concl.Remove(i, 1);
                            concl = concl.Insert(i, "0");
                        }
                        else
                        {
                            concl = concl.Remove(i, 1);
                            concl = concl.Insert(i, "1");
                        }
                    }

                    for(int i = concl.Length-1; i >= 0; i--)
                    {
                        if(concl[i] == '1')
                        {
                            concl = concl.Remove(i, 1);
                            concl = concl.Insert(i, "0");
                        }
                        else
                        {
                            concl = concl.Remove(i, 1);
                            concl = concl.Insert(i, "1");
                            break;
                        }
                    }

                    if (concl.Length < textBox1.MaxLength)
                    {
                        for (int i = 0; concl.Length != 16; i++)
                        {
                            concl = "1" + concl;
                        }
                    }
                }
                else
                {
                    x = Convert.ToInt32(conv);
                    while (x != 0 && x != 1)
                    {
                        if (x % 2 == 1)
                            concl = "1" + concl;
                        if (x % 2 == 0)
                            concl = "0" + concl;
                        x /= 2;
                    }

                    concl = x.ToString() + concl;

                    if (concl.Length < textBox1.MaxLength)
                    {
                        for (int i = 0; concl.Length != 16; i++)
                        {
                            concl = "0" + concl;
                        }
                    }
                }

            }
            else if (radOct)
            {
                for (int i = conv.Length - 1; i >= 0; i--)
                {
                    switch (conv[i])
                    {
                        case '0':
                            concl = "000" + concl;
                            break;
                        case '1':
                            concl = "001" + concl;
                            break;
                        case '2':
                            concl = "010" + concl;
                            break;
                        case '3':
                            concl = "011" + concl;
                            break;
                        case '4':
                            concl = "100" + concl;
                            break;
                        case '5':
                            concl = "101" + concl;
                            break;
                        case '6':
                            concl = "110" + concl;
                            break;
                        case '7':
                            concl = "111" + concl;
                            break;
                        default:
                            break;
                    }
                }

                concl = DelNull(concl);

                if (!chNeg)
                {
                    if (concl.Length < textBox1.MaxLength)
                    {
                        for (int i = 0; concl.Length != 16; i++)
                        {
                            concl = "0" + concl;
                        }
                    }
                }
                else
                {
                    if (concl.Length < textBox1.MaxLength)
                    {
                        for (int i = 0; concl.Length != 16; i++)
                        {
                            concl = "1" + concl;
                        }
                    }
                }
            }
            else if (radHex)
            {
                for (int i = conv.Length - 1; i >= 0; i--)
                {
                    switch (conv[i])
                    {
                        case '0':
                            concl = "0000" + concl;
                            break;
                        case '1':
                            concl = "0001" + concl;
                            break;
                        case '2':
                            concl = "0010" + concl;
                            break;
                        case '3':
                            concl = "0011" + concl;
                            break;
                        case '4':
                            concl = "0100" + concl;
                            break;
                        case '5':
                            concl = "0101" + concl;
                            break;
                        case '6':
                            concl = "0110" + concl;
                            break;
                        case '7':
                            concl = "0111" + concl;
                            break;
                        case '8':
                            concl = "1000" + concl;
                            break;
                        case '9':
                            concl = "1001" + concl;
                            break;
                        case 'A':
                            concl = "1010" + concl;
                            break;
                        case 'B':
                            concl = "1011" + concl;
                            break;
                        case 'C':
                            concl = "1100" + concl;
                            break;
                        case 'D':
                            concl = "1101" + concl;
                            break;
                        case 'E':
                            concl = "1110" + concl;
                            break;
                        case 'F':
                            concl = "1111" + concl;
                            break;
                        default:
                            break;
                    }
                }

                concl = DelNull(concl);

                if (!chNeg)
                {
                    if (concl.Length < textBox1.MaxLength)
                    {
                        for (int i = 0; concl.Length != 16; i++)
                        {
                            concl = "0" + concl;
                        }
                    }
                }
                else
                {
                    if (concl.Length < textBox1.MaxLength)
                    {
                        for (int i = 0; concl.Length != 16; i++)
                        {
                            concl = "1" + concl;
                        }
                    }
                }
                
            }

            concl = DelNull(concl);

            return concl;
        }

        //----------------------------------------------------------
        // Перевод в восьмеричную систему счисления
        //----------------------------------------------------------

        internal string ConvertToOct(string conv)
        {
            string concl = "";  // Выходное значение

            if (radOct && !chConvResult)
            {
                concl = conv;
            }
            else if (radBin)
            {
                int count = 0;
                string threeLast = "";
                for (int i = conv.Length - 1; i >= 0; i--)
                {
                    count++;
                    threeLast = conv[i] + threeLast;

                    if (count == 3 || i == 0)
                    {
                        count = Convert.ToInt32(threeLast);
                        switch (count)
                        {
                            case 0:
                                concl = "0" + concl;
                                break;
                            case 1:
                                concl = "1" + concl;
                                break;
                            case 10:
                                concl = "2" + concl;
                                break;
                            case 11:
                                concl = "3" + concl;
                                break;
                            case 100:
                                concl = "4" + concl;
                                break;
                            case 101:
                                concl = "5" + concl;
                                break;
                            case 110:
                                concl = "6" + concl;
                                break;
                            case 111:
                                concl = "7" + concl;
                                break;
                            default:
                                break;
                        }
                        count = 0;
                        threeLast = "";
                    }
                }
            }
            else if (radHex)
            {
                string convBin = ConvertToBin(conv);
                int count = 0;
                string threeLast = "";
                for (int i = convBin.Length - 1; i >= 0; i--)
                {
                    count++;
                    threeLast = convBin[i] + threeLast;

                    if (count == 3 || i == 0)
                    {
                        count = Convert.ToInt32(threeLast);
                        switch (count)
                        {
                            case 0:
                                concl = "0" + concl;
                                break;
                            case 1:
                                concl = "1" + concl;
                                break;
                            case 10:
                                concl = "2" + concl;
                                break;
                            case 11:
                                concl = "3" + concl;
                                break;
                            case 100:
                                concl = "4" + concl;
                                break;
                            case 101:
                                concl = "5" + concl;
                                break;
                            case 110:
                                concl = "6" + concl;
                                break;
                            case 111:
                                concl = "7" + concl;
                                break;
                            default:
                                break;
                        }
                        count = 0;
                        threeLast = "";
                    }
                }
            }
            else if (radDec || chConvResult)
            {
                string convBin = ConvertToBin(conv);
                int count = 0;
                string threeLast = "";
                for (int i = convBin.Length - 1; i >= 0; i--)
                {
                    count++;
                    threeLast = convBin[i] + threeLast;

                    if (count == 3 || i == 0)
                    {
                        count = Convert.ToInt32(threeLast);
                        switch (count)
                        {
                            case 0:
                                concl = "0" + concl;
                                break;
                            case 1:
                                concl = "1" + concl;
                                break;
                            case 10:
                                concl = "2" + concl;
                                break;
                            case 11:
                                concl = "3" + concl;
                                break;
                            case 100:
                                concl = "4" + concl;
                                break;
                            case 101:
                                concl = "5" + concl;
                                break;
                            case 110:
                                concl = "6" + concl;
                                break;
                            case 111:
                                concl = "7" + concl;
                                break;
                            default:
                                break;
                        }
                        count = 0;
                        threeLast = "";
                    }
                }
            }

            concl = DelNull(concl);

            return concl;
        }

        //----------------------------------------------------------
        // Перевод в шеснадцатеричную систему счисления
        //----------------------------------------------------------

        internal string ConvertToHex(string conv)
        {
            string concl = "";  // Выходное значение

            if (radHex && !chConvResult)
            {
                concl = conv;
            }
            else if (radBin)
            {
                string convBin = ConvertToBin(conv);
                int count = 0;
                string fourLast = "";

                for (int i = convBin.Length - 1; i >= 0; i--)
                {
                    count++;
                    fourLast = convBin[i] + fourLast;

                    if (count == 4 || i == 0)
                    {
                        count = Convert.ToInt32(fourLast);
                        switch (count)
                        {
                            case 0:
                                concl = "0" + concl;
                                break;
                            case 1:
                                concl = "1" + concl;
                                break;
                            case 10:
                                concl = "2" + concl;
                                break;
                            case 11:
                                concl = "3" + concl;
                                break;
                            case 100:
                                concl = "4" + concl;
                                break;
                            case 101:
                                concl = "5" + concl;
                                break;
                            case 110:
                                concl = "6" + concl;
                                break;
                            case 111:
                                concl = "7" + concl;
                                break;
                            case 1000:
                                concl = "8" + concl;
                                break;
                            case 1001:
                                concl = "9" + concl;
                                break;
                            case 1010:
                                concl = "A" + concl;
                                break;
                            case 1011:
                                concl = "B" + concl;
                                break;
                            case 1100:
                                concl = "C" + concl;
                                break;
                            case 1101:
                                concl = "D" + concl;
                                break;
                            case 1110:
                                concl = "E" + concl;
                                break;
                            case 1111:
                                concl = "F" + concl;
                                break;
                            default:
                                break;
                        }
                        count = 0;
                        fourLast = "";
                    }
                }
            }
            else if (radOct)
            {
                string convBin = ConvertToBin(conv);
                int count = 0;
                string threeLast = "";

                for (int i = convBin.Length - 1; i >= 0; i--)
                {
                    count++;
                    threeLast = convBin[i] + threeLast;

                    if (count == 4 || i == 0)
                    {
                        count = Convert.ToInt32(threeLast);
                        switch (count)
                        {
                            case 0:
                                concl = "0" + concl;
                                break;
                            case 1:
                                concl = "1" + concl;
                                break;
                            case 10:
                                concl = "2" + concl;
                                break;
                            case 11:
                                concl = "3" + concl;
                                break;
                            case 100:
                                concl = "4" + concl;
                                break;
                            case 101:
                                concl = "5" + concl;
                                break;
                            case 110:
                                concl = "6" + concl;
                                break;
                            case 111:
                                concl = "7" + concl;
                                break;
                            case 1000:
                                concl = "8" + concl;
                                break;
                            case 1001:
                                concl = "9" + concl;
                                break;
                            case 1010:
                                concl = "A" + concl;
                                break;
                            case 1011:
                                concl = "B" + concl;
                                break;
                            case 1100:
                                concl = "C" + concl;
                                break;
                            case 1101:
                                concl = "D" + concl;
                                break;
                            case 1110:
                                concl = "E" + concl;
                                break;
                            case 1111:
                                concl = "F" + concl;
                                break;
                            default:
                                break;
                        }
                        count = 0;
                        threeLast = "";
                    }
                }
            }
            else if (radDec || chConvResult)
            {
                string convBin = ConvertToBin(conv);
                int count = 0;
                string threeLast = "";

                for (int i = convBin.Length - 1; i >= 0; i--)
                {
                    count++;
                    threeLast = convBin[i] + threeLast;

                    if (count == 4 || i == 0)
                    {
                        count = Convert.ToInt32(threeLast);
                        switch (count)
                        {
                            case 0:
                                concl = "0" + concl;
                                break;
                            case 1:
                                concl = "1" + concl;
                                break;
                            case 10:
                                concl = "2" + concl;
                                break;
                            case 11:
                                concl = "3" + concl;
                                break;
                            case 100:
                                concl = "4" + concl;
                                break;
                            case 101:
                                concl = "5" + concl;
                                break;
                            case 110:
                                concl = "6" + concl;
                                break;
                            case 111:
                                concl = "7" + concl;
                                break;
                            case 1000:
                                concl = "8" + concl;
                                break;
                            case 1001:
                                concl = "9" + concl;
                                break;
                            case 1010:
                                concl = "A" + concl;
                                break;
                            case 1011:
                                concl = "B" + concl;
                                break;
                            case 1100:
                                concl = "C" + concl;
                                break;
                            case 1101:
                                concl = "D" + concl;
                                break;
                            case 1110:
                                concl = "E" + concl;
                                break;
                            case 1111:
                                concl = "F" + concl;
                                break;
                            default:
                                break;
                        }
                        count = 0;
                        threeLast = "";
                    }
                }
            }

            concl = DelNull(concl);

            return concl;
        }

        //----------------------------------------------------------
        // Перевод в десятичную систему счисления
        //----------------------------------------------------------

        internal string ConvertToDec(string conv)
        {
            string concl = ""; // Выходное значение

            if (radDec && !chChecked)
            {
                concl = conv;
            }
            else if (radBin || chChecked)
            {
                int summ = 0;

                if (conv.Length != 16)
                {
                    if (!chNeg)
                        for (int i = conv.Length; conv.Length < 16; i++)
                        {
                            conv = "0" + conv;
                        }
                    else
                        for (int i = conv.Length; conv.Length < 16; i++)
                        {
                            conv = "1" + conv;
                        }
                }

                if (conv[0] == '1' || chNeg)
                {
                    summ = summ - (int.Parse(conv[0].ToString()) * (int)Math.Pow(2, (conv.Length - 1)));
                    for (int i = 1; i < conv.Length; i++)
                    {
                        int count = int.Parse(conv[i].ToString());
                        summ = summ + (count * (int)Math.Pow(2, (conv.Length - i - 1)));
                    }
                }
                else
                {
                    for (int i = 0; i < conv.Length; i++)
                    {
                        int count = int.Parse(conv[i].ToString());
                        summ = summ + (count * (int)Math.Pow(2, (conv.Length - i - 1)));

                    }
                }

                concl = summ.ToString();
            }
            else if (radOct)
            {
                string convBin = ConvertToBin(conv);
                int summ = 0;
                if (conv.Length != 16)
                {
                    if (!chNeg)
                        for (int i = convBin.Length; convBin.Length < 16; i++)
                        {
                            convBin = "0" + convBin;
                        }
                    else
                        for (int i = convBin.Length; convBin.Length < 16; i++)
                        {
                            convBin = "1" + convBin;
                        }
                }

                if (convBin[0] == '1' || chNeg)
                {
                    summ = summ - (int.Parse(convBin[0].ToString()) * (int)Math.Pow(2, (convBin.Length - 1)));
                    for (int i = 1; i < convBin.Length; i++)
                    {
                        int count = int.Parse(convBin[i].ToString());
                        summ = summ + (count * (int)Math.Pow(2, (convBin.Length - i - 1)));
                    }
                }
                else
                {
                    for (int i = 0; i < convBin.Length; i++)
                    {
                        int count = int.Parse(convBin[i].ToString());
                        summ = summ + (count * (int)Math.Pow(2, (convBin.Length - i - 1)));
                    }
                }

                concl = summ.ToString();
            }
            else if (radHex)
            {
                string convBin = ConvertToBin(conv);
                int summ = 0;
                if (conv.Length != 16)
                {
                    if (!chNeg)
                        for (int i = convBin.Length; convBin.Length < 16; i++)
                        {
                            convBin = "0" + convBin;
                        }
                    else
                        for (int i = convBin.Length; convBin.Length < 16; i++)
                        {
                            convBin = "1" + convBin;
                        }
                }
                
                if (convBin[0] == '1' || chNeg)
                {
                    summ = summ - (int.Parse(convBin[0].ToString()) * (int)Math.Pow(2, (convBin.Length - 1)));
                    for (int i = 1; i < convBin.Length; i++)
                    {
                        int count = int.Parse(convBin[i].ToString());
                        summ = summ + (count * (int)Math.Pow(2, (convBin.Length - i - 1)));
                    }
                }
                else
                {
                    for (int i = 0; i < convBin.Length; i++)
                    {
                        int count = int.Parse(convBin[i].ToString());
                        summ = summ + (count * (int)Math.Pow(2, (convBin.Length - i - 1)));
                    }
                }

                concl = summ.ToString();
            }

            concl = DelNull(concl);

            return concl;
        }

        //----------------------------------------------------------
        // Перевод в текущюю систему счисления
        //----------------------------------------------------------

        private string WhichSyst(string convert)
        {
            if (radBin)
                convert = ConvertToBin(convert);
            else if (radOct)
                convert = ConvertToOct(convert);
            else if (radHex)
                convert = ConvertToHex(convert);
            else if (radDec)
                convert = ConvertToDec(convert);

            return convert;
        }

        //----------------------------------------------------------
        // Максимальное возможное значение (доделать)
        //----------------------------------------------------------

        private void radioBut2bytes_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.MaxLength = 15;
            textBox1.Text = ConvertToBin(textBox1.Text);
            if (textBox1.Text.Length > 15)
                textBox1.Text = textBox1.Text.Remove(0, textBox1.Text.Length - 15);

            chChecked = true;
            textBox1.Text = WhichSyst(textBox1.Text);
            chChecked = false;

            FocusTextBox();
        } 
        
        //--------------------------------------------------------
        // Возникает при изменении textBox1
        //--------------------------------------------------------

        protected override void textBox1_TextChanged(object sender, EventArgs e)
        {
            FocusTextBox();

            label2.Visible = false;

            if(textBox1.MaxLength == 16)
            {
                if(ConvertToBin(textBox1.Text).Length > textBox1.MaxLength)
                {
                    textBox1.Text = textBox1.Text.Remove(0, 1);
                    checkedResult = true;
                }

                FocusTextBox();
            }

            if (checkedResult)
            {
                textBox1.Clear();
                label1.Text = "";
                checkedResult = false;

                chNeg = false;

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

        //--------------------------------------------------
        // Смена знака
        //--------------------------------------------------

        private void butNot_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                string negative = textBox1.Text;
                negative = ConvertToDec(negative);
                int variable = 0;
                try
                {
                    variable = Convert.ToInt32(negative);
                }
                catch (Exception)
                {

                }
                
                variable = ~variable;

                negative = variable.ToString();
                textBox1.Clear();

                    if (!chNeg)
                    chNeg = true;
                else
                    chNeg = false;

                chConvResult = true;
                negative = WhichSyst(negative);
                negative = DelNull(negative);
                textBox1.Text = negative;

                changeSyst = true;
                chConvResult = false;
            }
                

            FocusTextBox();
        }

        //---------------------------------------------------------
        // Переопределение считывания значения
        //---------------------------------------------------------

        protected override void Reading(object sender, EventArgs e)
        {
            if (!chClickZnack)
            {
                if (textBox1.Text == "")
                    textBox1.Text = "0";

                textBox1.Text = ConvertToDec(textBox1.Text);

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
            ChangeSystem();
            FocusTextBox();
        }

        protected override void butSub_Click(object sender, EventArgs e)    // Вычитание
        {
            Reading(sender, e);
            ChangeSystem();
            FocusTextBox();
        }

        protected override void butMult_Click(object sender, EventArgs e)   // Умножение
        {
            Reading(sender, e);
            ChangeSystem();
            FocusTextBox();
        }

        protected override void butDivision_Click(object sender, EventArgs e)  // Деление
        {
            Reading(sender, e);
            ChangeSystem();
            FocusTextBox();
        }

        private void butAND_Click(object sender, EventArgs e)               // Поразрядное И
        {
            if (!chClickZnack)
            {
                textBox1.Text = ConvertToDec(textBox1.Text);

                if (!chZnakClose)
                    ExpresstionToRPN(textBox1.Text + " " + "$" + " ");
                else if (chZnakClose && chNumClose)
                    ExpresstionToRPN(textBox1.Text + " " + "$" + " ");
                else
                    ExpresstionToRPN(" " + "$" + " ");

                if (label1.Text == "0")
                    label1.Text = "";

                textBox1.Clear();

                chZnakClose = false;
                chNumClose = false;
                chClickZnack = true;
            }
            ChangeSystem();
            FocusTextBox();
        }                           

        private void butOR_Click(object sender, EventArgs e)                // Поразрядное ИЛИ
        {
            if (!chClickZnack)
            {
                textBox1.Text = ConvertToDec(textBox1.Text);

                if (!chZnakClose)
                    ExpresstionToRPN(textBox1.Text + " " + "|" + " ");
                else if (chZnakClose && chNumClose)
                    ExpresstionToRPN(textBox1.Text + " " + "|" + " ");
                else
                    ExpresstionToRPN(" " + "|" + " ");

                if (label1.Text == "0")
                    label1.Text = "";

                textBox1.Clear();

                chZnakClose = false;
                chNumClose = false;
                chClickZnack = true;
            }
            ChangeSystem();
            FocusTextBox();
        }

        private void butYShiftL_Click(object sender, EventArgs e)           // Сдвиг влево
        {
            if (!chClickZnack)
            {
                textBox1.Text = ConvertToDec(textBox1.Text);

                if (!chZnakClose)
                    ExpresstionToRPN(textBox1.Text + " " + "<" + " ");
                else if (chZnakClose && chNumClose)
                    ExpresstionToRPN(textBox1.Text + " " + "<" + " ");
                else
                    ExpresstionToRPN(" " + "<" + " ");

                if (label1.Text == "0")
                    label1.Text = "";

                textBox1.Clear();

                chZnakClose = false;
                chNumClose = false;
                chClickZnack = true;
            }
            ChangeSystem();
            FocusTextBox();
        }        

        private void butYShiftR_Click(object sender, EventArgs e)           // Сдвиг вправо
        {
            if (!chClickZnack)
            {
                textBox1.Text = ConvertToDec(textBox1.Text);

                if (!chZnakClose)
                    ExpresstionToRPN(textBox1.Text + " " + ">" + " ");
                else if (chZnakClose && chNumClose)
                    ExpresstionToRPN(textBox1.Text + " " + ">" + " ");
                else
                    ExpresstionToRPN(" " + ">" + " ");

                if (label1.Text == "0")
                    label1.Text = "";

                textBox1.Clear();

                chZnakClose = false;
                chNumClose = false;
                chClickZnack = true;
            }
            ChangeSystem();
            FocusTextBox();
        }

        private void butXor_Click(object sender, EventArgs e)               // Исключающее ИЛИ
        {
            if (!chClickZnack)
            {
                textBox1.Text = ConvertToDec(textBox1.Text);

                if (!chZnakClose)
                    ExpresstionToRPN(textBox1.Text + " " + "^" + " ");
                else if (chZnakClose && chNumClose)
                    ExpresstionToRPN(textBox1.Text + " " + "^" + " ");
                else
                    ExpresstionToRPN(" " + "^" + " ");

                if (label1.Text == "0")
                    label1.Text = "";

                textBox1.Clear();

                chZnakClose = false;
                chNumClose = false;
                chClickZnack = true;
            }
            ChangeSystem();
            FocusTextBox();
        }       

        //--------------------------------------------------------
        // Переопределение результата вычислений (для сложных выражений (переведенных в другие системы счисления))
        //--------------------------------------------------------

        internal override void butEqal_Click(object sender, EventArgs e)
        {
            textBox1.Text = ConvertToDec(textBox1.Text);

            if (countClose != countOpen)
            {
                SystemSounds.Beep.Play();
                label2.Visible = true;

            }
            else
            {
                ExpresstionToRPN(textBox1.Text);
                while (signs.Count > 0)
                    current += signs.Pop();
                chConvResult = true;

                if (!checkedResult)
                {
                    textBox1.Clear();
                    textBox1.Text = RPNToAnsver(current);
                }

                current = "";

                chConvResult = false;

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

        //--------------------------------------------------------
        // Буквы для 16-ричной системы счисления
        //--------------------------------------------------------

        private void butA_Click(object sender, EventArgs e)
        {
            ClickNum(sender);
        }

        private void butB_Click(object sender, EventArgs e)
        {
            ClickNum(sender);
        }

        private void butC_Click(object sender, EventArgs e)
        {
            ClickNum(sender);
        }

        private void butD_Click(object sender, EventArgs e)
        {
            ClickNum(sender);
        }

        private void butE_Click(object sender, EventArgs e)
        {
            ClickNum(sender);
        }

        private void butF_Click(object sender, EventArgs e)
        {
            ClickNum(sender);
        }

        //---------------------------------------------------
        // Переопределение нажатия на цифру (добавляются буквы)
        //---------------------------------------------------

        protected override void ClickNum(object text)
        {
            ChangeSystem();
            if (textBox1.Text.Length != 0)
                if (textBox1.Text[0] == '0')
                    textBox1.Text = textBox1.Text.Remove(0, 1);

            textBox1.Text += (text as Button).Text;
            FocusTextBox();
        }

        //---------------------------------------------------
        // Приоритет считываемых значений ( * | / = 7 ; + | - = 6 ; << | >> = 5;
        // &(AND) = 4 ; ^(Xor) = 3 ; |(OR) = 2 ; ( = 1 ; ) = -1 ; number = 0 )
        //---------------------------------------------------

        private int GetPr(char token)
        {
            if (token == '*' || token == '/')
                return 7;
            else if (token == '+' || token == '-')
                return 6;
            else if (token == '<' || token == '>')
                return 5;
            else if (token == '&')
                return 4;
            else if (token == '$')
                return 3;
            else if (token == '|')
                return 2;
            else if (token == '(')
                return 1;
            else if (token == ')')
                return -1;
            else
                return 0;
        }

        //---------------------------------------------------
        // Запись в польскую нотацию ( * | / = 7 ; + | - = 6 ; << | >> = 5;
        // &(AND) = 4 ; ^(Xor) = 3 ; |(OR) = 2 ; ( = 1 ; ) = -1 ; number = 0 )
        //---------------------------------------------------

        internal void ExpresstionToRPN(string expr)
        {
            int countHowMuchDelete = 0;     // Считает сколько нужно удалить при изменении выражения
            int priorety;   // Приоритет считываемых значений

            if (checkedResult)
            {
                checkedResult = false;
                label1.Text = "";
            }

            for (int i = 0; i < expr.Length; i++)
            {
                priorety = GetPr(expr[i]);

                if (label1.Text == "0" || checkedResult == true)
                    label1.Text = "";

                if (priorety == 0)
                {
                    if (chNumClose)
                    {
                        current = current.Remove(current.Length - countHowMuchDelete, countHowMuchDelete);
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

                if (priorety > 1)
                {
                    current += " ";

                    if (chZnakClose)
                        chZnakClose = false;

                    while (signs.Count > 0)
                    {
                        if (GetPr(signs.Peek()) >= priorety)
                            current += signs.Pop();
                        else
                            break;
                    }
                    signs.Push(expr[i]);
                }

                if (priorety == -1)
                {
                    current += " ";

                    if (GetPr(signs.Peek()) == -1)
                        expr = ")";

                    while (GetPr(signs.Peek()) != 1)
                    {
                        if (signs.Peek() != ' ')
                            countHowMuchDelete++;
                        current += signs.Pop();
                    }

                    countHowMuchDelete = ((2 * countHowMuchDelete) + 1) + (2 * countHowMuchDelete);

                    signs.Pop();
                }
            }

            if (!chNeg)
                label1.Text += expr;
            else
            {
                label1.Text += expr;
                chNeg = false;
            }
        }

        //---------------------------------------------------
        // Нахождение ответа из записи в польской нотации
        //---------------------------------------------------

        internal string RPNToAnsver(string rpn)
        {
            for (int i = 0; i < rpn.Length; i++)
            {
                if (rpn[i] == ' ') continue;

                if (GetPr(rpn[i]) == 0)
                {
                    while (rpn[i] != ' ' && GetPr(rpn[i]) == 0)
                    {
                        operand += rpn[i++];
                        if (i == rpn.Length)
                            break;
                    }

                    try
                    {
                        ansv.Push(checked(int.Parse(operand)));
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
                        int sec = 0;
                        int first = 0;

                        if (ansv.Count != 0)
                            sec = ansv.Pop();

                        if (ansv.Count != 0)
                            first = ansv.Pop();

                        if (znak == '+')
                            ansv.Push(first + sec);
                        if (znak == '-')
                            ansv.Push(first - sec);
                        if (znak == '*')
                            ansv.Push(first * sec);
                        if (znak == '/')
                            ansv.Push(first / sec);
                        if (znak == '$')
                            ansv.Push(first & sec);
                        if (znak == '|')
                            ansv.Push(first | sec);
                        if (znak == '^')
                            ansv.Push(first ^ sec);
                        if (znak == '<')
                            ansv.Push(first << sec);
                        if (znak == '>')
                            ansv.Push(first >> sec);
                    }
                }
                catch (Exception)
                {

                }
            }

            string ansver = "";

            if (ansv.Count != 0)
            {
                if (!chClose)
                    ansver = ansv.Pop().ToString();
                else
                    ansver = ansv.Peek().ToString();
            }

            ansver = WhichSyst(ansver);

            return ansver;
        }

        //--------------------------------------------------
        // Скобки
        //--------------------------------------------------

        private void butOpenBrPr_Click(object sender, EventArgs e)
        {
            ChangeSystem();
            if (label1.Text == "0")
                label1.Text = "";

            countOpen++;

            if (chZnakClose)
            {
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

                countOpen--;
                countClose--;
                textBox1.Clear();
            }

            ExpresstionToRPN((sender as Button).Text);

            FocusTextBox();
        }

        private void butCloseBrPr_Click(object sender, EventArgs e)
        {
            ChangeSystem();
            if (textBox1.Text == "")
                textBox1.Text = "0";

            textBox1.Text = ConvertToDec(textBox1.Text);

            if (countClose >= countOpen)
            {
                System.Media.SystemSounds.Asterisk.Play();
            }
            else
            {
                countClose++;

                textBox1.Text = ConvertToDec(textBox1.Text);

                if (label1.Text == "0")
                    label1.Text = "";

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
                chConvResult = true;

                textBox1.Clear();
                textBox1.Text = RPNToAnsver(current);

                chConvResult = false;
                chClose = true;

                FocusTextBox();
            }
        }

        //---------------------------------------------------
        // Сдвиг влево на 1 бит
        //---------------------------------------------------

        private void butShiftL_Click(object sender, EventArgs e)
        {
            string shift = textBox1.Text;
            shift = ConvertToDec(shift);
            int variable = Convert.ToInt32(shift);
            variable = variable << 1;
            textBox1.Clear();

            chConvResult = true;
            shift = WhichSyst(variable.ToString());
            textBox1.Text = shift;
            chConvResult = false;

            ChangeSystem();
            FocusTextBox();
        }

        //---------------------------------------------------
        // Сдвиг вправо на 1 бит
        //---------------------------------------------------

        private void butShiftR_Click(object sender, EventArgs e)
        {
            string shift = textBox1.Text;
            shift = ConvertToDec(shift);
            int variable = Convert.ToInt32(shift);
            variable = variable >> 1;
            textBox1.Clear();

            chConvResult = true;
            shift = WhichSyst(variable.ToString());
            textBox1.Text = shift;
            chConvResult = false;

            ChangeSystem();
            FocusTextBox();
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

            if (textBox1.Text == "0")
            {
                if (textBox1.Text.Length != 0)
                    textBox1.Text = textBox1.Text.Remove(0, 1);
            }


            if (radioButHex.Checked && (hex.Contains(e.KeyChar)))
            {
                ChangeSystem();
                if (e.KeyChar == 'a' || e.KeyChar == 'A')
                {
                    e.Handled = true;
                    butA.PerformClick();
                }
                if (e.KeyChar == 'b' || e.KeyChar == 'B')
                {
                    e.Handled = true;
                    butB.PerformClick();
                }
                if (e.KeyChar == 'c' || e.KeyChar == 'C')
                {
                    e.Handled = true;
                    butC.PerformClick();
                }
                if (e.KeyChar == 'd' || e.KeyChar == 'D')
                {
                    e.Handled = true;
                    butD.PerformClick();
                }
                if (e.KeyChar == 'e' || e.KeyChar == 'E')
                {
                    e.Handled = true;
                    butE.PerformClick();
                }
                if (e.KeyChar == 'f' || e.KeyChar == 'F')
                {
                    e.Handled = true;
                    butF.PerformClick();
                }
                return;
            }
            else if (radioButDec.Checked && (dec.Contains(e.KeyChar)))
            {
                ChangeSystem();
                    return;
            }
            else if (radioButOct.Checked && (oct.Contains(e.KeyChar)))
            {
                ChangeSystem();
                return;
            }
            else if (radioButBin.Checked && (bin.Contains(e.KeyChar)))
            {
                ChangeSystem();
                return;
            }
            else
                e.Handled = true;

            if (char.IsNumber(e.KeyChar))
            {
                ChangeSystem();
                return;
            }

            if (e.KeyChar == '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',')
            {
                e.Handled = true;
            }

            if (char.IsControl(e.KeyChar))
            {
                // Enter, Backspace
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
                    ChangeSystem();
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

            if (e.KeyChar == '(')
            {
                e.Handled = true;
                if (textBox1.Text.Length != 0)
                    textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                butOpenBrPr.PerformClick();
                return;
            }

            if (e.KeyChar == ')')
            {
                e.Handled = true;
                if (textBox1.Text.Length != 0)
                    textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                butCloseBrPr.PerformClick();
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

            if (signs.Count != 0)
                signs.Clear();
            if (ansv.Count != 0)
                ansv.Clear();

            checkedResult = false;
            chClose = false;
            chZnakClose = false;
            chNumClose = false;
            chConvResult = false;
            changeSyst = false;

            countResult = 0;

            chNeg = false;

            countOpen = 0;
            countClose = 0;

            FocusTextBox();
        }
    }
}
