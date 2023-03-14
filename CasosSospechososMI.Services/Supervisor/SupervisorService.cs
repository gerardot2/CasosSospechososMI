using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Domain.Family;
using CasosSospechososMI.Services.ApiDefinitions;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CasosSospechososMI.Services.Supervisor.Interfaces;

namespace CasosSospechososMI.Services.Supervisor
{
    public class SupervisorService : ISupervisorService
    {
        IAuthenticatedService _apiService;
        public SupervisorService(IAuthenticatedService authenticatedService)
        {
            _apiService = authenticatedService;
        }
        public async Task<OperationResult<ResponseGeneric>> RegisterVisitAsync(CancellationToken ct, FormRecordModel record, StreamPart image,string codigo)
        {
            var result = await _apiService.GetRestService<ISupervisorApiDefinition>().RegisterVisit(Codigo:codigo,
                Res1: record.Res1,
                Res2: record.Res2,
                Res3: record.Res3,
                Res4: record.Res4,
                Preg1: record.Preg1,
                Preg2: record.Preg2,
                Preg3: record.Preg3,
                Preg4: record.Preg4,
                Comentario: record.Comment,
                FechaCarga: record.Date,
                Latitud: record.Latitude,
                Longitud: record.Longitude,
                Imagen: image, ct);
            return result;

        }
    }
}
