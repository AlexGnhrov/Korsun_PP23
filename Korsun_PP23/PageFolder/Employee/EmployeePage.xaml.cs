using Korsun_PP23.ClassFolder;
using Korsun_PP23.DataFolder;
using Korsun_PP23.PageFolder.Employee;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Korsun_PP23.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : Page
    {
        bool blockEdit = false;

        public EmployeePage()
        {
            InitializeComponent();

            if(GlobalVariablesClass.currentRoleID == 3)
            {
                blockEdit = true;

                AddEmployeeBtn.Visibility = Visibility.Collapsed;
                EmployeeMenuCM.Visibility = Visibility.Collapsed;
            }

            EmployeeLV.ItemsSource = DBEntities.GetContext().User.ToList();
        }

        public void UpdateList()
        {
            try
            {
                EmployeeLV.ItemsSource = DBEntities.GetContext().User.Where(u => u.FirstNameUser.Contains(SearchTB.Text) ||
                                                                  u.MiddleNameUser.Contains(SearchTB.Text) ||
                                                                  u.LastNameUser.Contains(SearchTB.Text) ||
                                                                  u.Login.Contains(SearchTB.Text)).ToList();
            }
            catch (Exception ex)
            {

                MBClass.ErrorMB(ex);
                DBEntities.NullContext();
            }
        }

        private void DeleteTextLBtb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SearchTB.Text = "";
            UpdateList();
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AEEmployeePage(this,null));
        }

        private void SearchLB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateList();
        }


        private void Edit()
        {
            User user = EmployeeLV.SelectedItem as User;

            if (EmployeeLV.SelectedItem != null && !blockEdit)
            {
                NavigationService.Navigate(new AEEmployeePage(this, EmployeeLV.SelectedItem as User));
            }
        }

        private void DishInfoLV_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
            {
                Edit();
            }
        }

        private void RemoveEmployeeMI_Click(object sender, RoutedEventArgs e)
        {
            User user = EmployeeLV.SelectedItem as User;
            try
            {
                if (user.IdUser == 1) return;

                if(MBClass.QuestionMB("Вы действительно хотите удалить этого сотрудника?"))
                {

                    var check = DBEntities.GetContext().Transport.FirstOrDefault(u => u.IdUser == user.IdUser);

                    if(check != null)
                    {
                        MBClass.ErrorMB("Данный пользователь привязан к транспорту");
                        return;
                    }

                    DBEntities.GetContext().User.Remove(user);
                    DBEntities.GetContext().SaveChanges();
                }
            }
            catch (Exception ex)
            {

                MBClass.ErrorMB(ex);
            }
        }

        private void SearchTB_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                UpdateList();
            }
        }

        private void EditEmployeeMI_Click(object sender, RoutedEventArgs e)
        {
            Edit();
        }
    }
}
