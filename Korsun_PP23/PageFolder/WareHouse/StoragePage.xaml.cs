using Korsun_PP23.ClassFolder;
using Korsun_PP23.DataFolder;
using Korsun_PP23.PageFolder.Storage;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Korsun_PP23.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для StoragePage.xaml
    /// </summary>
    public partial class StoragePage : Page
    {
        private bool blockEditing;

        public StoragePage()
        {
            InitializeComponent();


            if (GlobalVariablesClass.currentRoleID == 3)             //Директор
            {
                blockEditing = true;

                StorageMenuCM.Visibility = Visibility.Collapsed;
                AddStorageBT.Visibility = Visibility.Collapsed;
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
            StorageLV.ItemsSource = DBEntities.GetContext().Warehouse.Where(u => u.WarehouseName.Contains(SearchTB.Text) ||
                                                                                 u.WarPhoneNumber.Contains(SearchTB.Text) ||
                                                                                 u.Contract.ContractName.Contains(SearchTB.Text) ||
                                                                                 u.Address.Region.RegionName.Contains(SearchTB.Text) ||
                                                                                 u.Address.City.CityName.Contains(SearchTB.Text) ||
                                                                                 u.Address.Street.StreetName.Contains(SearchTB.Text) ||
                                                                                 u.Address.House.Contains(SearchTB.Text)).ToList();
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

        private void AddStorageBT_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AEStoragePage(this,null));
        }

        private void StorageLV_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left && StorageLV.SelectedItem != null && !blockEditing)
            {
                NavigationService.Navigate(new AEStoragePage(this, StorageLV.SelectedItem as Warehouse));
            }
        }


        private void RemoveStorageMI_Click(object sender, RoutedEventArgs e)
        {
            if (StorageLV.SelectedItem != null && MBClass.QuestionMB("Вы действительно хотите удалить данный склад?"))
            {
                DBEntities.GetContext().Warehouse.Remove(StorageLV.SelectedItem as Warehouse);

                DBEntities.GetContext().SaveChanges();

                UpdateList();
            }
        }

        private void SearchTB_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                UpdateList();
            }
        }

        private void EditStorageMI_Click(object sender, RoutedEventArgs e)
        {
            if (StorageLV.SelectedItem != null)
            {
                NavigationService.Navigate(new AEStoragePage(this, StorageLV.SelectedItem as Warehouse));
            }
        }
    }
}
