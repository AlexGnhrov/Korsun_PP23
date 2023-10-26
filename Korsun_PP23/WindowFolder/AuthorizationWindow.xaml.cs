using Korsun_PP23.ClassFolder;
using Korsun_PP23.DataFolder;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TC_Application.AppFolder.GlobalClassFolder;

namespace Korsun_PP23.WindowFolder
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        String password;

        private void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var sas = DBEntities.GetContext().User.FirstOrDefault(u => u.Login == LoginTB.Text.Trim());

                if (sas == null || sas.Password != password)
                {
                    MBClass.ErrorMB("Неверный логин или пароль");
                    return;
                }

                GlobalVariablesClass.currentUserID = sas.IdUser;
                GlobalVariablesClass.currentRoleID = sas.IdRole;

                new MainMenu().Show();



                Close();
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex);
                DBEntities.NullContext();
            }

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


        private void CloseIM_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void HideIM_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MBClass.InfoMB("Ну и ладно.");
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                if (LogInBtn.IsEnabled)
                {
                    LogInBtn_Click(sender, e);
                    return;
                }


                if (LoginTB.IsFocused)
                {
                    PasswordPB.Focus();
                }
                else if (PasswordPB.IsFocused)
                {
                    LoginTB.Focus();
                }

            }
        }

        private void LoginTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void PasswordCopyTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            password = PasswordCopyTB.Text;
            EnableButton();
        }

        private void PasswordPB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            password = PasswordPB.Password;
            EnableButton();
        }


        private void ShowPasswordIM_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (PasswordCopyTB.Visibility == Visibility.Visible)
            {
                PasswordCopyTB.Visibility = Visibility.Hidden;
                PasswordPB.Visibility = Visibility.Visible;

                PasswordPB.Password = PasswordCopyTB.Text;
            }
            else
            {
                PasswordCopyTB.Visibility = Visibility.Visible;
                PasswordPB.Visibility = Visibility.Hidden;

                PasswordCopyTB.Text = PasswordPB.Password;
            }

            Keyboard.ClearFocus();
        }

        private void EnableButton()
        {
            LogInBtn.IsEnabled = !(string.IsNullOrWhiteSpace(LoginTB.Text) ||
                                  (PasswordPB.Visibility == Visibility.Visible && string.IsNullOrWhiteSpace(PasswordPB.Password)) ||
                                  (PasswordCopyTB.Visibility == Visibility.Visible && string.IsNullOrWhiteSpace(PasswordCopyTB.Text)));
        }

        private void LoginTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Login();
        }

        private void PasswordPB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Password();
        }
    }
}
