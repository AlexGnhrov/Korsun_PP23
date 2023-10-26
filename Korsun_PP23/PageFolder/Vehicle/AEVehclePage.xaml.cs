using Korsun_PP23.ClassFolder;
using Korsun_PP23.DataFolder;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using TC_Application.AppFolder.GlobalClassFolder;

namespace Korsun_PP23.PageFolder.Vehicle
{
    /// <summary>
    /// Логика взаимодействия для AEVehclePage.xaml
    /// </summary>
    public partial class AEVehclePage : Page
    {
        Transport transport;
        VehiclePage vehiclePage;

        bool isEdit = false;

        public AEVehclePage(VehiclePage vehiclePage, Transport transport)
        {
            InitializeComponent();




            this.transport = transport;
            this.vehiclePage = vehiclePage;

            try
            {
                var data = DBEntities.GetContext();

                VehicleTypeCB.ItemsSource = data.TransportType.ToList();
                DriverCB.ItemsSource = data.User.Where(u=>u.IdRole == 2).ToList();

                if (transport != null)
                {
                    isEdit = true;

                    Title = "Редактирование транспорта";
                    AddEdtBtn.Content = "Редактировать транспорт";

                    VehicleTypeCB.SelectedValue = transport.IdTransportType;
                    NameVehicleTB.Text = transport.TransportName;
                    NumVehicleTB.Text = transport.GOSNumber;
                    DriverCB.SelectedValue = transport.IdUser;

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

        private void AddEdtBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (!isEdit)  transport = new Transport();

                transport.IdTransportType = Convert.ToInt32(VehicleTypeCB.SelectedValue);
                transport.TransportName = NameVehicleTB.Text.Trim();
                transport.GOSNumber = NumVehicleTB.Text.Trim();
                transport.IdUser = Convert.ToInt32(DriverCB.SelectedValue);



                if (!isEdit)
                {
                    DBEntities.GetContext().Transport.Add(transport);
                }

                DBEntities.GetContext().SaveChanges();

                if (isEdit)
                {
                    MBClass.InfoMB("Транспорт успешно отредактирован");
                }
                else
                {
                    MBClass.InfoMB("Транспорт успешно добавлен");
                }

                vehiclePage.UpdateList();
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex);
                DBEntities.NullContext();
            }
        }

        private void СB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButton();
        }

        private void TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
            Console.WriteLine(NumVehicleTB.Text.Trim());
        }


        private void EnableButton()
        {
            AddEdtBtn.IsEnabled = !(string.IsNullOrWhiteSpace(NameVehicleTB.Text) ||
                                    string.IsNullOrWhiteSpace(NumVehicleTB.Text) ||
                                    VehicleTypeCB.SelectedValue == null ||
                                    DriverCB.SelectedValue == null ||
                                    NumVehicleTB.Text.Length < 9);
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (AddEdtBtn.IsEnabled) AddEdtBtn_Click(null, null);
        }

        private void NumVehicleTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.OnlyRussian();
        }
    }
}
