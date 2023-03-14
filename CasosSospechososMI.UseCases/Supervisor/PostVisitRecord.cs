using CasosSospechososMI.Domain.Account;
using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Domain.Family;
using CasosSospechososMI.Services.Account.Interfaces;
using CasosSospechososMI.Services.Supervisor.Interfaces;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CasosSospechososMI.UseCases.Supervisor
{
    public class PostVisitRecord
    {
        ISupervisorService _supervisorService;

        public PostVisitRecord(ISupervisorService supervisorService)
        {
            _supervisorService = supervisorService;
        }
        public async Task<OperationResult<ResponseGeneric>> Invoke(CancellationToken ct, FormRecordModel form, StreamPart image,string codigo)
        {
            var response = new OperationResult<ResponseGeneric>();
            try
            {
                return await _supervisorService.RegisterVisitAsync(ct, form, image,codigo);
            }
            catch (TaskCanceledException ex)
            {
                //si cancelo la peticion, no devuelvo nada
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
