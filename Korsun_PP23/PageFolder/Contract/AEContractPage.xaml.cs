using Korsun_PP23.ClassFolder;
using Korsun_PP23.DataFolder;
using Korsun_PP23.WindowFolder.AddtitionalWin;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using TC_Application.AppFolder.GlobalClassFolder;

namespace Korsun_PP23.PageFolder.Contract
{
    /// <summary>
    /// Логика взаимодействия для AEContractPage.xaml
    /// </summary>
    public partial class AEContractPage : Page
    {
        ContractPage contractPage;
        DataFolder.Contract contract;

        bool isEdit = false;

        public AEContractPage(ContractPage contractPage, DataFolder.Contract contract)
        {
            InitializeComponent();

            DataContext = DBEntities.GetContext().Organization.ToList();

            this.contractPage = contractPage;
            this.contract = contract;

            try
            {
                OrganizationCB.ItemsSource = DBEntities.GetContext().Organization.ToList();

                if (contract != null)
                {
                    Title = "Редактирование контраката";
                    AddEdtBtn.Content = "Редактировать контракт";

                    isEdit = true;

                    NumContractTB.Text = contract.ContractNumber;
                    DateContractDP.SelectedDate = contract.ContractDate;
                    NameContractTB.Text = contract.ContractName;
                    OrganizationCB.SelectedValue = contract.IdOrganization;

                    AddEdtBtn.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex);
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void NumContractTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.OnlyNumsTB();
        }

        private void AddEdtBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {



                if(!isEdit)
                {
                    contract = new DataFolder.Contract();
                }

                contract.ContractNumber = NumContractTB.Text;
                contract.ContractDate = Convert.ToDateTime(DateContractDP.SelectedDate);
                contract.ContractName = NameContractTB.Text;
                contract.IdOrganization = Convert.ToInt32(OrganizationCB.SelectedValue);


                if(!isEdit)
                    DBEntities.GetContext().Contract.Add(contract);


                DBEntities.GetContext().SaveChanges();

                if(isEdit)
                    MBClass.InfoMB("Контракт успешно отредактирован");
                else
                    MBClass.InfoMB("Контракт успешно добавлен");

                contractPage.UpdateList();
                NavigationService.GoBack();

            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex);
                DBEntities.NullContext();
            }
        }

        private void AddOrganizationIm_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            new AEComboBoxes(OrganizationCB,null).ShowDialog();
        }


        private void EnableButton()
        {
            AddEdtBtn.IsEnabled = !(string.IsNullOrWhiteSpace(NumContractTB.Text) ||
                                  DateContractDP.SelectedDate == null ||
                                  string.IsNullOrWhiteSpace(NameContractTB.Text) ||
                                  OrganizationCB.SelectedValue == null);
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && AddEdtBtn.IsEnabled)
            {
                AddEdtBtn_Click(sender, e);
            }
        }

        private void OrganizationCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void DateContractDP_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void OrganizationCB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Right && OrganizationCB.SelectedValue != null)
            {
                new AEComboBoxes(OrganizationCB, OrganizationCB.SelectedItem).ShowDialog();

                OrganizationCB.ItemsSource = DBEntities.GetContext().Organization.ToList();

                object selctedValue = OrganizationCB.SelectedValue;

                OrganizationCB.SelectedValue = null;
                OrganizationCB.SelectedValue = selctedValue;

            }
        }
    }
}
