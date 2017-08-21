using CityParkAgente.Classes;
using CityParkAgente.Models;
using CityParkAgente.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CityParkAgente.Services
{
  public class ApiService
  {
        private DialogService dialogService;
        private NavigationService navigationService;

        public ApiService()
        {
            dialogService = new DialogService();
            navigationService = new NavigationService();
        }

        public async Task<Response> Login(string usuario, string contrasena)
        {
            try
            {
                var loginRequest = new LoginRequest
                {
                    Agente = usuario,
                    Contrasena = contrasena,
                };

                var request = JsonConvert.SerializeObject(loginRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://QuitoParkApp.somee.com");
                var url = "/api/Agentes/Login";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Usuario o Contraseña incorrecto",
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<Agente>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Login Ok",
                    Result = user,
                };



            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
                throw;
            }
        }

        public async Task<Response> InsertarMulta(Multa multa)
        {
            try
            {

                var request = JsonConvert.SerializeObject(multa);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(" http://QuitoParkApp.somee.com");
                var url = "/api/Multas/InsertarMulta";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Error al Insertar la multa",
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var multar = JsonConvert.DeserializeObject<Multa>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Multa Registrada  Ok",
                    Result = multar,
                };



            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
                throw;
            }
        }

        public async Task<Response> SetPhotoAsync(int multaId, Stream stream)
        {
            try
            {
                var array = ReadFully(stream);

                var photoRequest = new PhotoRequest
                {
                    Id = multaId,
                    Array = array,
                };

                var request = JsonConvert.SerializeObject(photoRequest);
                var body = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(" http://QuitoParkApp.somee.com");
                var url = "/api/Multas/SetFoto";
                var response = await client.PostAsync(url, body);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Foto asignada Ok",
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }

        }


        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }


        internal async void VerificarAuto(string placa)
        {
            try
            {
                var placaRequest = new PlacaRequest
                {
                    Placa=placa,
                };

                var request = JsonConvert.SerializeObject(placaRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(" http://QuitoParkApp.somee.com");
                var url = "/api/Parqueos/BuscarPlaca";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    await dialogService.ShowMessage("Información", "El vehículo posee tiempo de parqueo...");
                    return ;
                }

                var a = MainViewModel.GetInstance();
                a.NuevaMulta.Placa = placa;
                await navigationService.Navigate("PonerMulta");
                return ;





            }
            catch (Exception)
            {
                return ;

            }
        }

        public async Task<List<Multa>>loadVehiculosMultados(string usuarioId)
        {
            try
            {
                var agenteRequest = new AgenteRequest
                {
                    AgenteId = usuarioId,
                };

                var request = JsonConvert.SerializeObject(agenteRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(" http://QuitoParkApp.somee.com");
                var url = "/api/Multas/GetMultas";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                var multas = JsonConvert.DeserializeObject<List<Multa>>(result);

                return multas;



            }
            catch (Exception )
            {
                return null;
                throw;
            }



        }




        //public static byte[] ReadFully(Stream input)
        //{
        //    byte[] buffer = new byte[16 * 1024];
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        int read;
        //        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
        //        {
        //            ms.Write(buffer, 0, read);
        //        }
        //        return ms.ToArray();
        //    }
        //}


        //public async Task<Response> SetPhoto(int customerId, Stream stream)
        //{
        //    try
        //    {
        //        var array = ReadFully(stream);

        //        var photoRequest = new PhotoRequest
        //        {
        //            Id = customerId,
        //            Array = array,
        //        };

        //        var request = JsonConvert.SerializeObject(photoRequest);
        //        var body = new StringContent(request, Encoding.UTF8, "application/json");
        //        var client = new HttpClient();
        //        client.BaseAddress = new Uri("http://zulu-software.com");
        //        var url = "/ECommerce/api/Customers/SetPhoto";
        //        var response = await client.PostAsync(url, body);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return new Response
        //            {
        //                IsSuccess = false,
        //                Message = response.StatusCode.ToString(),
        //            };
        //        }

        //        return new Response
        //        {
        //            IsSuccess = true,
        //            Message = "Foto asignada Ok",
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Response
        //        {
        //            IsSuccess = false,
        //            Message = ex.Message,
        //        };
        //    }

        //}

    }
}
