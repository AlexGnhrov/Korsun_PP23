using Korsun_PP23.ClassFolder;
using Korsun_PP23.DataFolder;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using TC_Application.AppFolder.GlobalClassFolder;

namespace Korsun_PP23.PageFolder.Employee
{
    /// <summary>
    /// Логика взаимодействия для AEEmployeePage.xaml
    /// </summary>
    /// 




    public partial class AEEmployeePage : Page
    {

        EmployeePage employeePage;
        User user;

        bool isEdit = false;
        string saveLogin = "";
        string savePhone = "";

        string selectedPhotoFile = "";

        public AEEmployeePage(EmployeePage employeePage, User user)
        {
            this.employeePage = employeePage;
            this.user = user;

            InitializeComponent();

            try
            {
                RoleCB.ItemsSource = DBEntities.GetContext().Role.ToList();
                PhoneEmpTB.Text = "+7 ";

                if (user != null)
                {
                    isEdit = true;
                    
                    if(user.IdUser == 1)
                    {
                        RoleCB.IsEnabled = false;
                    }

                    Title = "Редактирование сотрудника";
                    AddEdtBtn.Content = "Редактировать сотрудника";


                    FirstNameEmpTB.Text = user.FirstNameUser;
                    MiddleNameEmpTB.Text = user.MiddleNameUser;
                    LastNameEmpTB.Text = user.LastNameUser;
                    PhoneEmpTB.Text = savePhone = user.PhoneNum;
                    DateOfBirthDP.SelectedDate = user.BirthDate;
                    LogintTB.Text = saveLogin = user.Login;
                    PasswordTB.Text = user.Password;
                    RoleCB.SelectedValue = Convert.ToInt32(user.IdRole);
                    selectedPhotoFile = "Фото есть";
                    PhotoIB.ImageSource = LoadReadImageClass.BytesToImage(user.Photo);

                    AddEdtBtn.IsEnabled = false;

                }

            }
            catch (Exception ex)
            {

                MBClass.ErrorMB(ex);
                NavigationService.GoBack();
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void PhotoB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void AddEdtBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var checkLogin = DBEntities.GetContext().User.FirstOrDefault(u => u.Login == LogintTB.Text);

                if (checkLogin != null && saveLogin != LogintTB.Text)
                {
                    MBClass.ErrorMB("Такой логин уже существует");
                    PasswordTB.Focus();
                    return;
                }

                var checkPhone = DBEntities.GetContext().User.FirstOrDefault(u => u.PhoneNum == PhoneEmpTB.Text);

                if (checkPhone != null && savePhone != PhoneEmpTB.Text)
                {
                    MBClass.ErrorMB("Такой телефон уже существует");
                    PhoneEmpTB.Focus();
                    return;
                }


                if (!isEdit)
                {
                    user = new User();
                }


                user.FirstNameUser = FirstNameEmpTB.Text.Trim();
                user.LastNameUser = LastNameEmpTB.Text.Trim();
                user.MiddleNameUser = string.IsNullOrWhiteSpace(MiddleNameEmpTB.Text) ? " " : MiddleNameEmpTB.Text.Trim();
                user.PhoneNum = PhoneEmpTB.Text.Trim();
                user.BirthDate = DateOfBirthDP.SelectedDate;
                user.Login = LogintTB.Text.Trim();
                user.Password = PasswordTB.Text.Trim();
                user.IdRole = Convert.ToInt32(RoleCB.SelectedValue);

                if (selectedPhotoFile != "Фото есть")
                    user.Photo = LoadReadImageClass.ImageToByte(selectedPhotoFile);


                if (!isEdit) DBEntities.GetContext().User.Add(user);

                DBEntities.GetContext().SaveChanges();

                if (isEdit)
                    MBClass.InfoMB("Сотрудник успешно отредактирован");
                else
                    MBClass.InfoMB("Сотрудник успешно добавлен");


                employeePage.UpdateList();

                NavigationService.GoBack();

            }
            catch (Exception ex)
            {

                MBClass.ErrorMB(ex);
                DBEntities.NullContext();
            }
        }


        private void EnableButton()
        {
            AddEdtBtn.IsEnabled = !(string.IsNullOrWhiteSpace(FirstNameEmpTB.Text) ||
                                   string.IsNullOrWhiteSpace(LastNameEmpTB.Text) ||   
                                   string.IsNullOrWhiteSpace(PhoneEmpTB.Text) ||  
                                   DateOfBirthDP.SelectedDate == null||  
                                   string.IsNullOrWhiteSpace(LogintTB.Text) ||  
                                   string.IsNullOrWhiteSpace(PasswordTB.Text) ||
                                   selectedPhotoFile == "" || PhoneEmpTB.Text.Length < 17 ||
                                   RoleCB.SelectedValue == null);
        }


        private void DateOfBirthDP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void TB_TextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox textBox = sender as TextBox;

            if (textBox.Name == "PhoneEmpTB")
            {
                textBox.Phone();
            }


            EnableButton();
        }

        private void RoleCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            EnableButton();
        }

        private void PasswordTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Password();
        }

        private void LogintTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Login();
        }

        private void PhoneEmpTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.OnlyNumsTB();
        }
    }
}
