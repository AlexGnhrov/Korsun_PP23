using GMap.NET;
using GMap.NET.MapProviders;
using Korsun_PP23.ClassFolder;
using System;
using System.Net;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Korsun_PP23.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для ShopPage.xaml
    /// </summary>
    public partial class ShopPage : Page
    {
        public ShopPage()
        {
            InitializeComponent();

            try
            {

                MapGMC.DragButton = MouseButton.Left;
                MapGMC.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;

                MapGMC.ShowCenter = false;

                MapGMC.MapProvider = GMapProviders.GoogleMap;
                GMaps.Instance.Mode = AccessMode.ServerAndCache;
                MapGMC.Position = new PointLatLng(55.75263744159643, 37.62308264994724);

                GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
                GMapProvider.WebProxy.Credentials = CredentialCache.DefaultCredentials;

                MapGMC.Zoom = 10;
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex);
                NavigationService.GoBack();
            }
        }

        private void SearchLB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void DeleteTextLBtb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SearchTB.Text = "";
        }


        private void SearchShopLB_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void DeleteTextShopLBtb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SearchShopTB.Text = "";
        }

    }
}
