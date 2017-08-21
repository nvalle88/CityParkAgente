using CityParkAgente.Models;
using CityParkAgente.Pages;
using CityParkAgente.Services;
using GalaSoft.MvvmLight.Command;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CityParkAgente.ViewModels
{
   public class PonerMultaViewModel:Multa,INotifyPropertyChanged
    {

        public bool isRunning;

        public bool IsRunning
        {
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
            get { return isRunning; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private MediaFile file;

        private ImageSource imageSource;

        public ImageSource ImageSource
        {
            set
            {
                if (imageSource != value)
                {
                    imageSource = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ImageSource"));
                }
            }
            get
            {
                return imageSource;
            }
        }


        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;

        public PonerMultaViewModel()
        {
            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiService();
        }

        public ICommand TakePictureCommand { get { return new RelayCommand(TakePicture); } }

        private async void TakePicture()
        {
            await Plugin.Media.CrossMedia.Current.Initialize();

            if (!Plugin.Media.CrossMedia.Current.IsCameraAvailable || !Plugin.Media.CrossMedia.Current.IsTakePhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "Aceptar");
            }

            file = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Photos",
                Name = "NuevaMulta.jpg",
                CustomPhotoSize=15,
                CompressionQuality=30,
                
            });

            if (file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    
                    return stream;
                });
            }
        }


        public ICommand MultarCommand { get { return new RelayCommand(Multar); } }

        private async void Multar()
        {
            IsRunning = true;
            if (file==null)
            {
                await dialogService.ShowMessage("Error", "Debe tomar la foto...");
                IsRunning = false;
                return;
            }

            if (string.IsNullOrEmpty(Placa))
            {
                await dialogService.ShowMessage("Error", "Debe ingresar la placa del vehículo");
                IsRunning = false;
                return;
            }

            if((Valor==0))
            {
                await dialogService.ShowMessage("Error", "Debe ingresar un valor ...");
                IsRunning = false;
                return;
            }
            var PP = PonerMultaPage.GetInstance();
            var multa = new Multa
            {
                AgenteId=navigationService.GetAgenteActual().AgenteId,
                Fecha=DateTime.Now,
                Valor=Valor,
                Placa =Placa,
                Latitud=PP.Location.Latitude,
                Longitud=PP.Location.Longitude,
            };


             var response=  await apiService.InsertarMulta(multa);

            if (response.IsSuccess && file!=null)
            {
                var newMulta = (Multa)response.Result;
                var response2 = await apiService.SetPhotoAsync(newMulta.MultaId, file.GetStream());
                var filenName = string.Format("{0}.jpg", newMulta.MultaId);
                var folder = "~/Content/Multas";
                var fullPath = string.Format("{0}/{1}", folder, filenName);
                multa.Foto = fullPath;
             }



            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Error",response.Message);
                IsRunning = false;
            }

            var respuestaMulta = (Multa)response.Result;
            await dialogService.ShowMessage("OK", string.Format("Multa insertada correctamente... Placa: {0}", respuestaMulta.Placa));
            navigationService.SetMainPage(navigationService.GetAgenteActual());
            IsRunning = false;
        }
    }
}
