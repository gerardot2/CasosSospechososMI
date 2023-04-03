using CasosSospechososMI.Domain.Account;
using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Domain.Family;
using CasosSospechososMI.Domain.Samples;
using CasosSospechososMI.Domain.User;
using CasosSospechososMI.Services.ApiDefinitions;
using CasosSospechososMI.Services.Family.Interfaces;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZXing.Common.ReedSolomon;

namespace CasosSospechososMI.Services.Family
{
    public class FamilyService : IFamilyService
    {
        IAuthenticatedService _apiService;
        public FamilyService(IAuthenticatedService apiService)
        {
            _apiService = apiService;
        }
        public async Task<HomeDataModel> GetFamilyHomeDataAsync(CancellationToken ct)
        {
            var resp = await _apiService.GetRestService<IFamilyApiDefinition>().GetFamilyHomeData(ct);
            return resp;
            
        }
        public async Task<HomeDataSupervisorModel> GetSupervisorHomeDataAsync(CancellationToken ct)
        {
            var resp = await _apiService.GetRestService<IFamilyApiDefinition>().GetSupervisorHomeData(ct);
            return resp;
            
        }
        public async Task<OperationResult<FamilyDataModel>> GetFamilyDataAsync(CancellationToken ct, int tipo)
        {
            return await _apiService.GetRestService<IFamilyApiDefinition>().GetFamilyData(ct,tipo);
        }
        public async Task<OperationResult<FamilyDataModel>> GetFamilyDataByCodeAsync(CancellationToken ct, string codigo)
        {
            return await _apiService.GetRestService<IFamilyApiDefinition>().GetFamilyDataByCode(ct,codigo);
        }
        public async Task<OperationResult<List<FormModel>>> GetFormItemsAsync(CancellationToken ct, HomeQuery hq)
        {
            return await _apiService.GetRestService<IFamilyApiDefinition>().GetFormItems(ct, int.Parse(hq.TipoAplicacion));
        }
        public async Task<OperationResult<ResponseGeneric>> RegisterSampleAsync(CancellationToken ct, FormRecordModel record, StreamPart image)
        {
            var result = await _apiService.GetRestService<IFamilyApiDefinition>().RegisterSample(Res1:record.Res1,
                Res2:record.Res2,
                Res3:record.Res3,
                Res4:record.Res4,
                Preg1:record.Preg1,
                Preg2:record.Preg2,
                Preg3:record.Preg3,
                Preg4:record.Preg4,
                Comentario:record.Comment,
                FechaCarga:record.Date,
                Latitud: record.Latitude,
                Longitud: record.Longitude,
                resultado:record.Resultado,
                dni: record.Dni,
                apellido: record.Apellido,
                nombre:record.Nombre,
                cantidad:record.Cantidad,
                domicilio: record.Domicilio,
                id_localidad:record.LocalidadId,
                telefono: record.Telefono,
                email:record.Email,
                Imagen:image, ct);
            return result;
           
        }

        public async Task<OperationResult<List<SampleModel>>> GetMySamplesAsync(CancellationToken ct)
        {
            return await _apiService.GetRestService<IFamilyApiDefinition>().GetMyLoadedSamples(ct);
        }
        public async Task<OperationResult<List<SampleModel>>> GetVisitsByIdAsync(CancellationToken ct, string id)
        {
            return await _apiService.GetRestService<IFamilyApiDefinition>().GetVisitsById(ct,id);
        }
        public async Task<ResponseGeneric> PostSaveDataUpdateAsync(CancellationToken ct, RegisterModel user)
        {
            var result = await _apiService.GetRestService<IFamilyApiDefinition>().PostSaveDataUpdate( 
                Dni:user.Dni,
                Nombre:user.Name,
                Apellido:user.Surname,
                IdLocalidad:user.CityId.ToString(),
                Domicilio:user.Address,
                Telefono:user.Phone,
                Cantidad:user.MembersQty.ToString(),
                Email:user.Email,
                IdRole:user.RoleId.ToString(),
                Codigo:user.Code,ct
                );
            return result;
        }
    }
}
