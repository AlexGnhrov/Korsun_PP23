using Korsun_PP23.ClassFolder;
using Korsun_PP23.DataFolder;
using Korsun_PP23.PageFolder;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Korsun_PP23.WindowFolder
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();

            GlobalVariablesClass.mainMenu = this;

            ShopBtn.Visibility = Visibility.Collapsed;
            HomeBtn.Visibility = Visibility.Collapsed;


            switch (GlobalVariablesClass.currentRoleID)
            {
                case 2: //Водитель
                    EmployeeBtn.Visibility = Visibility.Collapsed;
                    StorageBtn.Visibility = Visibility.Collapsed;
                    ContractBtn.Visibility = Visibility.Collapsed;
                    break;
                case 3: //Директор
                    break;
                case 4: //Бухгалтер
                    VehicleBtn.Visibility = Visibility.Collapsed;
                    StorageBtn.Visibility = Visibility.Collapsed;
                    break;
                case 5: //Диспечер
                    EmployeeBtn.Visibility = Visibility.Collapsed;
                    ContractBtn.Visibility = Visibility.Collapsed;
                    break;

            }


            //MainFrame.Navigate(new MainMenuPage());
            //currentBtn = HomeBtn;
            //currentBtn.Tag = "Using";



        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (WindowState == WindowState.Maximized)
            {
                Point mousePoint = Mouse.GetPosition(TopperBorder);
                WindowState = WindowState.Normal;

                Top = 0;
                Left = 840;
            }
           

            DragMove();
        }




        Button currentBtn = null;


        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            Button selectedBtn = sender as Button;

            DBEntities.NullContext();

            if (currentBtn != null && selectedBtn == currentBtn) return;

            switch (selectedBtn.Name)
            {
                case "HomeBtn":
                    MainFrame.Navigate(new MainMenuPage());
                    break;
                case "ShopBtn":
                    MainFrame.Navigate(new ShopPage());
                    break;
                case "VehicleBtn":
                    MainFrame.Navigate(new VehiclePage());
                    break;
                case "StorageBtn":
                    MainFrame.Navigate(new StoragePage());
                    break;
                case "ContractBtn":
                    MainFrame.Navigate(new ContractPage());
                    break;
                case "EmployeeBtn":
                    MainFrame.Navigate(new EmployeePage());
                    break;

            }

            if (currentBtn != null)
            {
                currentBtn.Tag = "NoUse";
            }

            currentBtn = selectedBtn;
            currentBtn.Tag = "Using";

        }

        private void CloseBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ExitBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MBClass.ExitMB();
        }


        private void QuitIM_LeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(MBClass.QuestionMB("Вы действительно хотите сменить учётную запись?"))
            {
                new AuthorizationWindow().Show();
                Close();
            }    
        }


        private void MainFrame_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.XButton1 || e.ChangedButton == MouseButton.XButton2)
            {
                e.Handled = true;
            }
        }

        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (MainFrame.Content.ToString().Contains("AE"))
            {
                ButtonPanelSP.IsEnabled = false;
            }
            else
            {
                ButtonPanelSP.IsEnabled = true;
            }
        }



    }
}
