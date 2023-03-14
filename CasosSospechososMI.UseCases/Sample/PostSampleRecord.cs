using CasosSospechososMI.Domain.Account;
using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Domain.Family;
using CasosSospechososMI.Services.Account.Interfaces;
using CasosSospechososMI.Services.Family.Interfaces;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CasosSospechososMI.UseCases.Sample
{
    public class PostSampleRecord
    {
        IFamilyService _familyService;

        public PostSampleRecord(IFamilyService familyService)
        {
            _familyService = familyService;
        }
        public async Task<OperationResult<ResponseGeneric>> Invoke(CancellationToken ct, FormRecordModel form, StreamPart image)
        {
            var response = new OperationResult<ResponseGeneric>();
            try
            {
                return await _familyService.RegisterSampleAsync(ct, form, image);
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
