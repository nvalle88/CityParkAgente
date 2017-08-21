using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace CityParkAgente.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PonerMultaPage : ContentPage
    {

        static PonerMultaPage instance;
        public Plugin.Geolocator.Abstractions.Position Location;

        public static PonerMultaPage GetInstance()
        {
            if (instance == null)
            {
                instance = new PonerMultaPage();
            }

            return instance;
        }
        public PonerMultaPage()
        {
            InitializeComponent();
            instance = this;
            Location = new Plugin.Geolocator.Abstractions.Position();
            Locator();

        }

        private async void Locator()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            
             Location = await locator.GetPositionAsync(timeoutMilliseconds: 5000);
        }
    }
}