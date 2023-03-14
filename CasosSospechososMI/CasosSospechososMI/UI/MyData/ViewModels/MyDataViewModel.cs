using CasosSospechososMI.Domain.Family;
using CasosSospechososMI.Services.Interfaces;
using CasosSospechososMI.UI.Base.ViewModels;
using CasosSospechososMI.UseCases.Common;
using CasosSospechososMI.UseCases.Family;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CasosSospechososMI.UI.MyData.ViewModels
{
    public class MyDataViewModel : BaseViewModel
    {
        IRoutingService _routingService;
        GetFamilyData _getFamilyData;
        GetCities _getCities;
        public MyDataViewModel(GetFamilyData getFamilyData, IRoutingService routing, GetCities getCities) : base(routing)
        {
            _getFamilyData = getFamilyData;
            _routingService = routing;
            _getCities = getCities;
        }
        FamilyDataModel _data;
        public FamilyDataModel Data
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
        internal async void OnAppearing()
        {
            CancellationTokenSource = new CancellationTokenSource();
            //if (!IsConnected){
            //    await OpenResultWindow("Sin Conexión", "Hubo un error obteniendo datos.\nSi el error persiste, cierre sesión y vuelva a ingresar.", Pr_VolverInicio);
            //    }
            IsBusy = true;
            var result = await _getFamilyData.Invoke(CancellationTokenSource.Token);
            var localidades = await _getCities.Invoke(CancellationTokenSource.Token);
            if (result != null && result.Data != null && int.Parse(result.Codigo) == 0)
            {
                Data = result.Data;
            }
            else
            {
                await OpenResultWindow("Error de Datos", "Hubo un error obteniendo datos.\nSi el error persiste, cierre sesión y vuelva a ingresar.", Pr_VolverInicio);
            }
            if (localidades != null && localidades.Data != null && !string.IsNullOrEmpty(Data.IdLocalidad.ToString()))
            {
                Localidad = localidades.Data.Where(x => x.Id == Data.IdLocalidad).FirstOrDefault().Descripcion;
            }
            else
            {
                Localidad = "Sin definir";
            }
           
            IsBusy = false;
        }

       
    }
}
