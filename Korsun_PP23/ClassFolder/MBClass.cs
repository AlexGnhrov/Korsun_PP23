using System;
using System.Windows;

namespace Korsun_PP23.ClassFolder
{
    internal class MBClass
    {
        public static void ErrorMB(Exception text)
        {
            MessageBox.Show(text.Message, "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ErrorMB(string text)
        {
            MessageBox.Show(text, "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void InfoMB(string text)
        {
            MessageBox.Show(text, "Информация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static bool QuestionMB(string text)
        {
            return MessageBoxResult.Yes == MessageBox.Show(text, "Вопрос",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        public static void ExitMB()
        {
            bool resultMB = QuestionMB("Вы действительно " +
                "желаете выйти ?");
            if (resultMB == true)
                App.Current.Shutdown();
        }
    }
}
