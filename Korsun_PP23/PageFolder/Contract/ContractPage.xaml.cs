using Korsun_PP23.ClassFolder;
using Korsun_PP23.DataFolder;
using Korsun_PP23.PageFolder.Contract;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Korsun_PP23.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для ContractPage.xaml
    /// </summary>
    public partial class ContractPage : Page
    {
        public bool blockEditing = false;

        public ContractPage()
        {
            InitializeComponent();



            if (GlobalVariablesClass.currentRoleID == 3)             //Директор
            {
                blockEditing = true;

                AddContractBtn.Visibility = Visibility.Collapsed;
                ActionDGTC.Visibility = Visibility.Collapsed;
                OrganizationDGTC.CellStyle = (Style)FindResource("RContenLefttDGCell");
            }

            UpdateList();
        }

        private void SearchLB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateList();
        }

        private void SearchTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateList();
            }
        }


        public void UpdateList()
        {

            try
                {
            ContractListDG.ItemsSource = DBEntities.GetContext().Contract.
                                         Where(u=>u.Organization.OrganizationName.Contains(SearchTB.Text) ||
                                         u.ContractNumber.Contains(SearchTB.Text) ||
                                         u.ContractName.Contains(SearchTB.Text)).ToList();
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

        private void AddContractBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AEContractPage(this,null));
        }

        private void ContractListDG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataFolder.Contract contract = (DataFolder.Contract)ContractListDG.SelectedItem;

            if (e.ChangedButton == MouseButton.Left && contract != null && !blockEditing)
            {
                NavigationService.Navigate(new AEContractPage(this, contract));
            }
        }

        private void EditVehicleMI_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AEContractPage(this, (DataFolder.Contract)ContractListDG.SelectedItem));
        }

        private void RemoveVehicleMI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MBClass.QuestionMB("Вы действительно хотите удалить этот контракт?"))
                {
                    DBEntities.GetContext().Contract.Remove((DataFolder.Contract)ContractListDG.SelectedItem);

                    DBEntities.GetContext().SaveChanges();

                    UpdateList();
                }
            }
            catch (Exception ex)
            {

                MBClass.ErrorMB(ex);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            button.ContextMenu.PlacementTarget = (UIElement)sender;
            button.ContextMenu.IsOpen = true;
        }
    }
}
