using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Domain.Family;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CasosSospechososMI.Services.Supervisor.Interfaces
{
    public interface ISupervisorService
    {
        Task<OperationResult<ResponseGeneric>> RegisterVisitAsync(CancellationToken ct, FormRecordModel record, StreamPart image, string codigo);
    }
}
