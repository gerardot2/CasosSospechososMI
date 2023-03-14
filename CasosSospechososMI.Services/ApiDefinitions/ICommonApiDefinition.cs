using CasosSospechososMI.Domain.Common;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CasosSospechososMI.Services.ApiDefinitions
{
    public interface ICommonApiDefinition
    {
        [Get("/api/procev/localidades")]
        Task<OperationResult<List<City>>> GetCities(CancellationToken ct);
        [Get("/api/procev/ovitrampa/numero_wpp")]
        Task<OperationResult<ResponseGeneric>> GetWhatsappNumber(CancellationToken ct);
        [Get("/api/procev/aspersor/version")]
        Task<VersionModel> GetLastVersion(CancellationToken ct);
    }
}
