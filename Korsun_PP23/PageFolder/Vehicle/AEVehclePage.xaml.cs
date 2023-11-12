using Korsun_PP23.ClassFolder;
using Korsun_PP23.DataFolder;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
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

        string selectedPhotoFile = "";
        string saveNum = "";
        string saveSTS = "";
        string savePTS = "";

        public AEVehclePage(VehiclePage vehiclePage, Transport transport, bool noEdit)
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
                    NumVehicleTB.Text = saveNum = transport.TSNumber;
                    DriverCB.SelectedValue = transport.IdUser;
                    StsTB.Text = saveSTS = transport.STS;
                    PtsTB.Text = savePTS = transport.PTS;
                    PowerTB.Text = transport.PowerHP;
                    TypeEngineTB.Text = transport.EngineType;
                    YearOfProductionTB.Text = transport.YearOfProuction;

                    if (transport.Photo != null)
                    {
                        selectedPhotoFile = "Фото есть";

                        PhotoIB.ImageSource = LoadReadImageClass.BytesToImage(transport.Photo);
                    }

                    AddEdtBtn.IsEnabled = false;
                }

                if (noEdit)
                {
                    Title = "Информация о транспорте";

                    AddEdtBtn.Visibility = Visibility.Collapsed;

                    VehicleTypeCB.IsReadOnly = true;
                    NameVehicleTB.IsReadOnly = true;
                    NumVehicleTB.IsReadOnly = true;
                    DriverCB.IsReadOnly = true;
                    StsTB.IsReadOnly = true;
                    PtsTB.IsReadOnly = true;
                    PowerTB.IsReadOnly = true;
                    TypeEngineTB.IsReadOnly = true;
                    YearOfProductionTB.IsReadOnly = true;
                    BorderPhoto.IsEnabled = false;
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

                var checkNum = DBEntities.GetContext().Transport.FirstOrDefault(u => u.TSNumber == NumVehicleTB.Text);

                if (checkNum != null && saveNum != NumVehicleTB.Text)
                {
                    MBClass.ErrorMB("Такой номер уже существует");
                    NumVehicleTB.Focus();
                    return;
                }

                var checkSts = DBEntities.GetContext().Transport.FirstOrDefault(u => u.STS == StsTB.Text);

                if (checkNum != null && saveSTS != StsTB.Text)
                {
                    MBClass.ErrorMB("Такое свидетельство о регистрации транспортного средства уже существует");
                    StsTB.Focus();
                    return;
                }

                var checkPts = DBEntities.GetContext().Transport.FirstOrDefault(u => u.PTS == PtsTB.Text);

                if (checkPts != null && savePTS != PtsTB.Text)
                {
                    MBClass.ErrorMB("Такой паспорт транспортного средства уже существует");
                    PtsTB.Focus();
                    return;
                }



                if (!isEdit)  transport = new Transport();

                transport.IdTransportType = Convert.ToInt32(VehicleTypeCB.SelectedValue);
                transport.TransportName = NameVehicleTB.Text.Trim();
                transport.TSNumber = NumVehicleTB.Text.Trim();                             
                transport.IdUser = Convert.ToInt32(DriverCB.SelectedValue);
                transport.STS = StsTB.Text.Trim();
                transport.PTS = PtsTB.Text.Trim();
                transport.PowerHP = PowerTB.Text.Trim();
                transport.EngineType = TypeEngineTB.Text.Trim();
                transport.YearOfProuction = YearOfProductionTB.Text.Trim();

                if (selectedPhotoFile != "Фото есть")
                    transport.Photo = LoadReadImageClass.ImageToByte(selectedPhotoFile);


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
                                    string.IsNullOrWhiteSpace(StsTB.Text) ||
                                    string.IsNullOrWhiteSpace(PtsTB.Text) ||
                                    string.IsNullOrWhiteSpace(PowerTB.Text) ||
                                    string.IsNullOrWhiteSpace(TypeEngineTB.Text) ||
                                    string.IsNullOrWhiteSpace(YearOfProductionTB.Text) ||
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

        private void NameVehicleTB_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                Border underBrush = (Border)textBox.Template.FindName("underBrush", textBox);

                if (underBrush != null)
                {
                    DoubleAnimation animation = new DoubleAnimation
                    {
                        From = 0,
                        To = textBox.ActualWidth,
                        Duration = TimeSpan.FromSeconds(0.23),
                        AccelerationRatio = 1
                    };

                    //PowerEase easing = new PowerEase { Power = 6 };
                    //animation.EasingFunction = easing;

                    underBrush.BeginAnimation(Border.WidthProperty, animation);
                }
            }
        }

        private void PowerTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.OnlyNumsTB();
        }

        private void PhotoB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image files (*.png *.jpeg)|*.png;*.jpeg";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


            if (openFileDialog.ShowDialog() == true)
            {
                PhotoIB.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName));
                selectedPhotoFile = openFileDialog.FileName;

                EnableButton();
            }
        }
    }
}
