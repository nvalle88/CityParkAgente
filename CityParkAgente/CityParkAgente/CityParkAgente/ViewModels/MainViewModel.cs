using GalaSoft.MvvmLight.Command;
using CityParkAgente.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Maps;
using System.ComponentModel;
using CityParkAgente.Models;

namespace CityParkAgente.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
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


        #region Singleton

        static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MainViewModel();
            }

            return instance;
        }

        #endregion

        #region Propeties
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }

        public ObservableCollection<VehiculosMultadosViewModels> VehiculosMultados { get; set; }



        
        public LoginViewModel NewLogin { get; set; }
        public PonerMultaViewModel NuevaMulta { get; set; }



        public ObservableCollection<Pin> Pins { get; set; }
        public VerificarAutoViewModel VerificarAuto { get; set; }




        public bool IsRefreshing
        {
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshing"));
                }
            }
            get
            {
                return isRefreshing;
            }
        }


        #endregion

        #region Attributes
        private bool isRefreshing = false;
        private NavigationService navigationService;
       

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Services

        private ApiService apiService;

        #endregion

        #region Constructors
        public  MainViewModel()
        {


            instance = this;
            VehiculosMultados = new ObservableCollection<VehiculosMultadosViewModels>();
            NuevaMulta = new PonerMultaViewModel();
            NuevaMulta.Valor = 10;
            Pins = new ObservableCollection<Pin>();
            apiService = new ApiService();
            Menu = new ObservableCollection<MenuItemViewModel>();
            navigationService = new NavigationService();
            NewLogin = new LoginViewModel();
            VerificarAuto = new VerificarAutoViewModel();
            LoadMenu();
        }




        #endregion

        #region Commands


        public ICommand VerificarAutoCommand { get { return new RelayCommand(VerificarAutonavigate); } }

        public async void VerificarAutonavigate()
        {

            await navigationService.Navigate("VerificarAutoPage");
            IsRefreshing = false;
        }

        public ICommand ConsultarMultasCommand { get { return new RelayCommand(ConsultarMultas); } }

        public async void ConsultarMultas()
        {

            await navigationService.Navigate("ConsultarMultas");
            IsRefreshing = false;
        }

        public ICommand RefreshCarrosCommand { get { return new RelayCommand(RefreshCarros); } }

        public void RefreshCarros()
        {

           
            IsRefreshing = false;
        }













        #endregion

        #region Methods

        public async void LoadVehiculosMultados()
        {

            IsRunning = true;
            var vehiculos = await apiService.loadVehiculosMultados(navigationService.GetAgenteActual().AgenteId.ToString());
            VehiculosMultados.Clear();

            foreach (var vehiculo in vehiculos)
            {
                VehiculosMultados.Add(new VehiculosMultadosViewModels
                {
                    Valor = vehiculo.AgenteId,
                    Agente = vehiculo.Agente,
                    AgenteId = vehiculo.AgenteId,
                    Latitud=vehiculo.Latitud,
                    Longitud=vehiculo.Longitud,
                    Placa=vehiculo.Placa,
                    Fecha = vehiculo.Fecha,
                    Foto = vehiculo.Foto,
                    MultaId = vehiculo.MultaId,
                    
                }
                );
            }
            IsRunning = false;

        }

        public void LoadMenu()
        {
            Menu.Clear();



            Menu.Add(new MenuItemViewModel
            {

                PageName = "VerificarAutoPage",
                Icon = "ic_Recarga_Prepago.png",
                Title = "Verificar vehículo",
                SubTitle = "",
                
            });

            Menu.Add(new MenuItemViewModel
            {

                PageName = "ConsultarMultas",
                Icon= "ic_Historial_Compras.png",
                Title = "Consultar multas del día",
                SubTitle = "",
            });

        }
        #endregion
    }
}
