using CityParkAgente.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CityParkAgente.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConsultarAutoPage : ContentPage
    {
        public ConsultarAutoPage()
        {
            InitializeComponent();
            var main = (MainViewModel)this.BindingContext;
            base.Appearing += (object sender, EventArgs e) =>
            {
                main.LoadVehiculosMultados();
            };

        }

        public void OnMore(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("More Context Action", mi.CommandParameter + " more context action", "OK");
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("Delete Context Action"," delete context action", "OK");
        }
    }
}