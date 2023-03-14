using CasosSospechososMI.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CasosSospechososMI.Services.Common.Interfaces
{
    public interface ICommonService
    {
        Task<OperationResult<List<City>>> GetCitiesAsync(CancellationToken ct);
        Task<OperationResult<ResponseGeneric>> GetWhatsappNumberAsync(CancellationToken ct);
        Task<VersionModel> GetLastVersionAsync(CancellationToken ct);
    }
}
