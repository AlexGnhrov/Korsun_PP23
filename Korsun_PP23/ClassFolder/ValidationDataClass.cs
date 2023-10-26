using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace TC_Application.AppFolder.GlobalClassFolder
{



    public static class ValidationDataClass
    {
        public static void OnlyNumsTB(this TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        public static void OnlyRussian(this TextCompositionEventArgs e)
        {
            e.Handled = !new Regex(@"^[а-яёА-ЯЁ0-9_!?]*$").IsMatch(e.Text);
        }

        public static void Password(this TextCompositionEventArgs e)
        {
            e.Handled = !new Regex($"^[a-zA-Z0-9_';:-@!#$%^&*()+№]*$").IsMatch(e.Text);
        }

        public static void Login(this TextCompositionEventArgs e)
        {
            e.Handled = !new Regex(@"^[a-zA-Z0-9_!?]*$").IsMatch(e.Text);
        }

        public static void FloatNums(this TextBox textBox)
        {
            string result = "";
            char[] validChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ',', '.', 'б' };

            bool CommaIsUsing = false;
            int i = 1;

            byte afterCommaleng = 0;

            foreach (char c in textBox.Text)
            {

                if (Array.IndexOf(validChars, c) != -1)
                {

                    if (c == ',' || c == '.' || c == 'б')
                    {
                        if (i != 1 && !CommaIsUsing)
                        {

                            result += ",";
                            CommaIsUsing = true;
                        }
                    }
                    else if (CommaIsUsing && afterCommaleng < 2)
                    {
                        result += c;
                        ++afterCommaleng;
                    }
                    else if (result.Length < textBox.MaxLength-3 && afterCommaleng < 2)
                    {
                        result += c;
                    }
                }


                ++i;
            }

            textBox.Text = result;



            if (!Keyboard.IsKeyDown(Key.Back))
            {

                if (textBox.CaretIndex == textBox.Text.Length ||
                    textBox.CaretIndex == 0)
                {
                    textBox.CaretIndex = textBox.Text.Length;
                }


            }
        }



        public static void Phone(this TextBox PhoneNumText)
        {

            try
            {
                if (!Keyboard.IsKeyDown(Key.Back))
                {
                    switch (PhoneNumText.Text.Length)
                    {
                        case 4:
                            PhoneNumText.Text = PhoneNumText.Text.Insert(3, "(");
                            PhoneNumText.CaretIndex = 6;
                            break;
                        case 8:
                            PhoneNumText.Text = PhoneNumText.Text.Insert(7, ") ");
                            PhoneNumText.CaretIndex = 10;
                            break;
                        case 12:
                            PhoneNumText.Text = PhoneNumText.Text.Insert(12, "-");
                            PhoneNumText.CaretIndex = 13;
                            break;
                        case 15:
                            PhoneNumText.Text = PhoneNumText.Text.Insert(15, "-");
                            PhoneNumText.CaretIndex = 16;
                            break;
                    }
                }

                else
                {
                    if (PhoneNumText.Text.Length < 4)
                    {
                        PhoneNumText.Text = "+7 ";
                        PhoneNumText.CaretIndex = 4;
                    }
                    switch (PhoneNumText.Text.Length)
                    {
                        case 7:
                            PhoneNumText.Text =
                                PhoneNumText.Text.Remove(PhoneNumText.Text.LastIndexOf("-"));
                            PhoneNumText.CaretIndex = 6;
                            break;
                        case 13:
                            PhoneNumText.Text =
                                PhoneNumText.Text.Remove(PhoneNumText.Text.LastIndexOf("-"));
                            PhoneNumText.CaretIndex = 12;
                            break;
                        case 16:
                            PhoneNumText.Text =
                                PhoneNumText.Text.Remove(PhoneNumText.Text.LastIndexOf("-"));
                            PhoneNumText.CaretIndex = 15;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }


    }
}
