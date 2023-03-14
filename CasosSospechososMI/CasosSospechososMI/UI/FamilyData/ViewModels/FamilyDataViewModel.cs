using CasosSospechososMI.Domain.Account;
using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Domain.Family;
using CasosSospechososMI.Models;
using CasosSospechososMI.Services.Interfaces;
using CasosSospechososMI.UI.Base.ViewModels;
using CasosSospechososMI.UI.Visit.Views;
using CasosSospechososMI.UseCases.Account;
using CasosSospechososMI.UseCases.Common;
using CasosSospechososMI.UseCases.Family;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;

namespace CasosSospechososMI.UI.FamilyData.ViewModels
{
    //----  Consultar y editar info de familia (supervisor)  ----
    public class FamilyDataViewModel : BaseViewModel
    {
        public ICommand OnSearchByCodeCommand { get; }
        public ICommand OnSaveDataChangesCommand { get; }
        public ICommand OnRecordsListCommand { get; }

        
        GetFamilyDataByCode _getFamilyDataByCode;
        GetCities _getCities;
        PostSaveFamilyDataChanges _postUpdateData;
        IRoutingService _routingService;
        GetActualUser _getActual;
        public FamilyDataViewModel(GetFamilyDataByCode getFamilyDataByCode,
            GetCities getCities,
            IRoutingService routingService,
            PostSaveFamilyDataChanges postUpdateData,
            GetActualUser getActual) :
base(routingService)        {
            _getFamilyDataByCode = getFamilyDataByCode;
            _getCities = getCities;
            OnSearchByCodeCommand = new Command<string>(OnSearchFamilyData);
            OnSaveDataChangesCommand = new Command(SaveDataChanges);
            OnRecordsListCommand = new Command<string>(OnRecordsList);
            _routingService = routingService;
            _data = new RegisterModel();
            _postUpdateData = postUpdateData;
            _selectedFamily = new FamilyDataModel();
            _getActual = getActual;
        }

        private async void OnRecordsList(string obj)
        {
            if (string.IsNullOrEmpty(Data.Dni) || string.IsNullOrEmpty(Data.Code))
            {
                return;
            }
            if (IsBusy) return;
            IsBusy = true;

            await _routingService.PushModal(new VisitListPage(_selectedFamily));
            IsBusy = false;

        }

        private async void SaveDataChanges(object obj)
        {
            if (IsBusy) return;
            IsBusy = true;
            Data.CityId = (SelectedCity != null) ? (int)SelectedCity.Id : 0;
            var result = await _postUpdateData.Invoke(CancellationTokenSource.Token, Data);
            if (result != null && int.Parse(result.codigo) == 0)
            {
                IsBusy = false;

                await OpenResultWindow("Actualización de datos", "¡Datos actualizados con éxito!");

            }
            else if (result != null && result.mensaje != null)
            {
                IsBusy = false;
                //await OpenResultWindow("Registro", $"Error\n\n{result.Mensaje}.");
                await OpenResultWindow("Actualización de datos", $"Error\n\n{result.mensaje}.");
            }
            else
            {
                IsBusy = false;
                await OpenResultWindow("Actualización de datos", $"Error\n\nPor favor intente de nuevo.");
            }
            IsBusy = false;
        }
        #region Properties
        ObservableCollection<City> _cities;
        public ObservableCollection<City> Cities
        {
            get => _cities;
            set
            {
                SetProperty(ref _cities, value);
                
            }
        }
        City _selectedCity;
        public City SelectedCity
        {
            get => _selectedCity;
            set
            {
                SetProperty(ref _selectedCity, value);
            }
        }
        RegisterModel _data;
        FamilyDataModel _selectedFamily;
        public RegisterModel Data
        {
            get => _data;
            set
            {
                SetProperty(ref _data, value);
            }
        }
        string _localidad = string.Empty;
        public string Localidad
        {
            get => _localidad;
            set
            {
                SetProperty(ref _localidad, value);

            }
        }
        string _buttonLabel = "Consultar";
        public string ButtonLabel
        {
            get => _buttonLabel;
            set
            {
                SetProperty(ref _buttonLabel, value);

            }
        }
        bool _showFields = false;
        public bool ShowFields
        {
            get => _showFields;
            set
            {
                SetProperty(ref _showFields, value);
            }
        }
        string _code = string.Empty;
        public string Code
        {
            get => _code;
            set
            {
                SetProperty(ref _code, value);
            }
        }
        bool _saveButtonEnabled = false;
        public bool SaveButtonEnabled
        {
            get => _saveButtonEnabled;
            set
            {
                SetProperty(ref _saveButtonEnabled, value);
            }
        }
        #endregion 
        private async void OnSearchFamilyData(string obj)
        {
            var actual = _getActual.Invoke();
            if (!IsConnected && !actual.HasCachedRecord && actual.HasCachedForm)
            {
                if (IsBusy) return;
                IsBusy = true;
                
                await Shell.Current.GoToAsync($"SampleRecording?code={Code}");
                IsBusy = false;
                return;
            }

            if (IsBusy) return;
            IsBusy = true;
            
            try
            {
                if (!string.IsNullOrEmpty(obj))
                {

                    var data = await _getFamilyDataByCode.Invoke(CancellationTokenSource.Token, obj);
                
                    var cities = await _getCities.Invoke(CancellationTokenSource.Token);
                    if (data != null && data.Data != null && int.Parse(data.Codigo) == 0)
                    {
                        _selectedFamily = data.Data;
                        ShowFields = true;
                        var user = data.Data;
                    

                        Data = new RegisterModel()
                        {
                            Name = user.Nombre,
                            Surname = user.Apellido,
                            Dni = user.Dni,
                            Email = user.Email,
                            CityId = user.IdLocalidad,
                            Address = user.Domicilio,
                            MembersQty = user.Cantidad,
                            Code = user.CodigoOvitrampa,
                            RoleId = 4,
                            Phone = user.Telefono
                        };
                    
                    }
                    else if(data != null && data.Data == null && int.Parse(data.Codigo) == 0)
                    {
                        await OpenResultWindow("Código incorrecto", $"{data.Mensaje}");
                    }
                    else
                    {
                        await OpenResultWindow("Código incorrecto", $"No se encontró el código ingresado.",Pr_GoBackHome);
                    }
                    if (cities != null && cities.Data != null && !string.IsNullOrEmpty(Data.CityId.ToString()))
                    {
                        Cities = new ObservableCollection<City>(cities.Data);
                        SelectedCity = cities.Data.Where(x => x.Id == Data.CityId).FirstOrDefault();
                    }
                    else
                    {
                        Cities = new ObservableCollection<City>();
                        SelectedCity = null;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            SaveButtonEnabled = false;
            IsBusy = false;
        }
        internal async void OnAppearing()
        {

            CancellationTokenSource = new CancellationTokenSource();
            if (!IsConnected && _getActual.Invoke().HasCachedForm)
            {
                ButtonLabel = "Registrar";
            }

        }
        private async void Pr_GoBackHome(object sender, object e)
        {
            await _routingService.NavigateTo("///AboutPage");
        }
    }
}
