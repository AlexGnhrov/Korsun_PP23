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

namespace Korsun_PP23.PageFolder.Storage
{
    /// <summary>
    /// Логика взаимодействия для AEStoragePage.xaml
    /// </summary>
    public partial class AEStoragePage : Page
    {
        StoragePage storagePage;
        Warehouse warehouse;
        Address address;

        bool isEdit = false;
        string saveName = "";
        string savePhone = "";

        public AEStoragePage(StoragePage storagePage, Warehouse warehouse)
        {
            InitializeComponent();

            this.storagePage = storagePage;
            this.warehouse = warehouse;

            isEdit = !(warehouse is null);

            try
            {
                RegionCB.ItemsSource = DBEntities.GetContext().Region.ToList();
                CityCB.ItemsSource = DBEntities.GetContext().City.ToList();
                StreetCB.ItemsSource = DBEntities.GetContext().Street.ToList();
                ContractCB.ItemsSource = DBEntities.GetContext().Contract.ToList();

                PhoneNumTB.Text += "+7 ";

                if (isEdit)
                {
                    Title = "Редактирование склада";
                    AddEdtBtn.Content = "Редактировать склад";

                    address = DBEntities.GetContext().Address.FirstOrDefault(u => u.IdAddress == warehouse.IdAddress);


                    RegionCB.SelectedValue = address.IdRegion;
                    CityCB.SelectedValue = address.ICity;
                    StreetCB.SelectedValue = address.IdStreet;

                    HouseTB.Text = address.House;
                    BuildingTB.Text = address.Builing;

                    CompanyNameTB.Text = warehouse.WarehouseName;
                    PhoneNumTB.Text = savePhone = warehouse.WarPhoneNumber;
                    ContractCB.SelectedValue = warehouse.IdContract;

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



        private void AddIM_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;


            switch (image.Name)
            {
                case "AddRegionIm":
                    new AEComboBoxes(RegionCB,null).ShowDialog();
                    break;
                case "AddCityIm":
                    new AEComboBoxes(CityCB, null).ShowDialog();
                    break;
                case "AddStreetIM":
                    new AEComboBoxes(StreetCB, null).ShowDialog();
                    break;
            }

        }

        private void CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }



        private void CB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (e.ChangedButton == MouseButton.Right && comboBox.SelectedValue != null)
            {
                object selctedValue = null;
                new AEComboBoxes(comboBox, comboBox.SelectedItem).ShowDialog();

                switch (comboBox.Name)
                {
                    case "RegionCB":
                        comboBox.ItemsSource = DBEntities.GetContext().Region.ToList();

                        selctedValue = RegionCB.SelectedValue;

                        RegionCB.SelectedValue = null;
                        RegionCB.SelectedValue = selctedValue;

                        break;
                    case "CityCB":
                        comboBox.ItemsSource = DBEntities.GetContext().City.ToList();

                        selctedValue = CityCB.SelectedValue;

                        CityCB.SelectedValue = null;
                        CityCB.SelectedValue = selctedValue;

                        break;
                    case "StreetCB":
                        comboBox.ItemsSource = DBEntities.GetContext().Street.ToList();


                        selctedValue = StreetCB.SelectedValue;

                        StreetCB.SelectedValue = null;
                        StreetCB.SelectedValue = selctedValue;

                        break;
                }

            }
        }

        private void AddEdtBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var checkLogin = DBEntities.GetContext().Contract.FirstOrDefault(u => u.ContractName == CompanyNameTB.Text.Trim());

                if (checkLogin != null && saveName != CompanyNameTB.Text.Trim())
                {
                    MBClass.ErrorMB("Такой склад уже существует");
                    CompanyNameTB.Focus();
                    return;
                }

                var checkPhone = DBEntities.GetContext().Warehouse.FirstOrDefault(u => u.WarPhoneNumber == PhoneNumTB.Text);

                if (checkPhone != null && savePhone != PhoneNumTB.Text)
                {
                    MBClass.ErrorMB("Такой телефон уже существует");
                    PhoneNumTB.Focus();
                    return;
                }


                if (!isEdit)
                {
                    warehouse = new Warehouse();
                    address = new Address();
                }


                address.IdRegion = Convert.ToInt32(RegionCB.SelectedValue);
                address.ICity = Convert.ToInt32(CityCB.SelectedValue);
                address.IdStreet = Convert.ToInt32(StreetCB.SelectedValue);

                address.House = HouseTB.Text.Trim();
                address.Builing = string.IsNullOrWhiteSpace(BuildingTB.Text) ? "-" : BuildingTB.Text.Trim();


                warehouse.WarehouseName = CompanyNameTB.Text.Trim();
                warehouse.WarPhoneNumber = PhoneNumTB.Text;
                warehouse.IdContract = Convert.ToInt32(ContractCB.SelectedValue);


                if (!isEdit)
                {
                    DBEntities.GetContext().Address.Add(address);
                    warehouse.IdAddress = address.IdAddress;

                    DBEntities.GetContext().Warehouse.Add(warehouse);                   
                }

                DBEntities.GetContext().SaveChanges();


                if (isEdit) MBClass.InfoMB("Склад успешно отредактирован");
                else MBClass.InfoMB("Склад успешно добавлен");
                
                storagePage.UpdateList();
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
            AddEdtBtn.IsEnabled = !(RegionCB.SelectedValue == null ||
                                    CityCB.SelectedValue == null ||
                                    StreetCB.SelectedValue == null ||
                                    ContractCB.SelectedValue == null ||
                                    string.IsNullOrWhiteSpace(HouseTB.Text) ||
                                    string.IsNullOrWhiteSpace(CompanyNameTB.Text) ||
                                    string.IsNullOrWhiteSpace(PhoneNumTB.Text) ||
                                    PhoneNumTB.Text.Length < 18);
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && AddEdtBtn.IsEnabled)
            {
                AddEdtBtn_Click(sender, e);
            }
        }

        private void TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if((sender as TextBox).Name == "PhoneNumTB")
            {
                (sender as TextBox).Phone();
            }
            EnableButton();
        }

        private void TB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.OnlyNumsTB();
        }
    }
}
