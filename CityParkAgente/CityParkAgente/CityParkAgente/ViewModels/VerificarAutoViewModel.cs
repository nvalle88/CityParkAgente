﻿using CityParkAgente.Classes;
using CityParkAgente.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CityParkAgente.ViewModels
{
   public class VerificarAutoViewModel:INotifyPropertyChanged
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


        public string  Placa { get; set; }

        private ApiService apiService;
        private NavigationService navigationService;
        private DialogService dialogService;

        public event PropertyChangedEventHandler PropertyChanged;

        public VerificarAutoViewModel()
        {
            apiService = new ApiService();
            navigationService = new NavigationService();
            dialogService = new DialogService();
        }

        public ICommand VerificarAutoCommand { get { return new RelayCommand(VerificarAutosAsync); } }

        private async  void VerificarAutosAsync()
        {
            IsRunning = true;
            if (string.IsNullOrEmpty(Placa))
            {
                await dialogService.ShowMessage("Información", "Debe introducir la placa del vehículo que desea verificar...");
                IsRunning = false;
                return;
            }
            
            apiService.VerificarAuto(Placa);
            IsRunning = false;
             
        }
    }


}
