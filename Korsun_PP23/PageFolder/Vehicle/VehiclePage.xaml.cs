using Korsun_PP23.ClassFolder;
using Korsun_PP23.DataFolder;
using Korsun_PP23.PageFolder.Vehicle;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;


namespace Korsun_PP23.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для VehiclePage.xaml
    /// </summary>
    public partial class VehiclePage : Page
    {

        public bool blockEditing = false;

        public VehiclePage()
        {
            InitializeComponent();


            if (GlobalVariablesClass.currentRoleID == 3             //Директор
               || GlobalVariablesClass.currentRoleID == 2)         //Водитель
            {
                blockEditing = true;

                AddVechileBtn.Visibility = Visibility.Collapsed;
                ActionDGTC.Visibility = Visibility.Collapsed;
                DriverDGTC.CellStyle = (Style)FindResource("RContenLefttDGCell");
            }

            UpdateList();
        }

        private void SearchLB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateList();
        }

        public void UpdateList()
        {
            try
            {
                if (GlobalVariablesClass.currentRoleID == 2)// Водитель
                {
                    VehclieListDG.ItemsSource = DBEntities.GetContext().Transport.Where(u => u.TransportName.Contains(SearchTB.Text) ||
                                                                  u.TSNumber.Contains(SearchTB.Text) ||
                                                                  u.User.FirstNameUser.Contains(SearchTB.Text) ||
                                                                  u.User.MiddleNameUser.Contains(SearchTB.Text) ||
                                                                  u.User.LastNameUser.Contains(SearchTB.Text)).
                                                                  Where(u => u.IdUser == GlobalVariablesClass.currentUserID).ToList();
                }
                else
                {
                    VehclieListDG.ItemsSource = DBEntities.GetContext().Transport.Where(u => u.TransportName.Contains(SearchTB.Text) ||
                                                  u.TSNumber.Contains(SearchTB.Text) ||
                                                  u.User.FirstNameUser.Contains(SearchTB.Text) ||
                                                  u.User.MiddleNameUser.Contains(SearchTB.Text) ||
                                                  u.User.LastNameUser.Contains(SearchTB.Text)).ToList();
                }
            }
            catch (Exception ex)
            {

                MBClass.ErrorMB(ex);

                DBEntities.NullContext();
            }
        }

        private void DeleteTextLBtb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SearchTB.Text = null;
            UpdateList();


        }

        private void AddVechileBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AEVehclePage(this, null,false));
        }

        private void SearchTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateList();
            }
        }

        private void VehclieListDG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {

                if (VehclieListDG.SelectedItem != null) //Водитель
                {
                    NavigationService.Navigate(new AEVehclePage(this, VehclieListDG.SelectedItem as Transport, true));
                }

            }
        }

 



        private void EditVehicleMI_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AEVehclePage(this, VehclieListDG.SelectedItem as Transport,false));
        }

        private void RemoveVehicleMI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MBClass.QuestionMB("Вы действительно хотите удалить этот транспорт?"))
                {
                    DBEntities.GetContext().Transport.Remove(VehclieListDG.SelectedItem as Transport);
                    DBEntities.GetContext().SaveChanges();

                    UpdateList();
                }
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex);

                DBEntities.NullContext();
                UpdateList();
            }
                
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            button.ContextMenu.PlacementTarget = (UIElement)sender;
            button.ContextMenu.IsOpen = true;
        }

        private void MoreInfoMI_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AEVehclePage(this, VehclieListDG.SelectedItem as Transport,true));
        }
    }
}
