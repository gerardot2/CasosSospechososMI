using CasosSospechososMI.Domain.Account;
using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Domain.Family;
using CasosSospechososMI.Domain.Samples;
using CasosSospechososMI.Domain.User;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CasosSospechososMI.Services.Family.Interfaces
{
    public interface IFamilyService
    {
        Task<HomeDataModel> GetFamilyHomeDataAsync(CancellationToken ct);
        Task<HomeDataSupervisorModel> GetSupervisorHomeDataAsync(CancellationToken ct);
        Task<OperationResult<FamilyDataModel>> GetFamilyDataAsync(CancellationToken ct, int tipo);
        Task<OperationResult<FamilyDataModel>> GetFamilyDataByCodeAsync(CancellationToken ct, string codigo);
        Task<OperationResult<List<SampleModel>>> GetMySamplesAsync(CancellationToken ct);
        Task<OperationResult<List<SampleModel>>> GetVisitsByIdAsync(CancellationToken ct,string id);
        Task<OperationResult<List<FormModel>>> GetFormItemsAsync(CancellationToken ct, HomeQuery hq);
        Task<OperationResult<ResponseGeneric>> RegisterSampleAsync(CancellationToken ct, FormRecordModel record, StreamPart image);
        Task<ResponseGeneric> PostSaveDataUpdateAsync(CancellationToken ct, RegisterModel registerModel);
    }
}
