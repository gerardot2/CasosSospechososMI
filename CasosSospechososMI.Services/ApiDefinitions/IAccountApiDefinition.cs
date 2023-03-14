using CasosSospechososMI.Domain.Account;
using CasosSospechososMI.Domain.Common;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CasosSospechososMI.Services.ApiDefinitions
{
    public interface IAccountApiDefinition
    {
        //[Headers("Content-Type: application/x-www-form-urlencoded")]
        [Post("/api/procev/aspersor/register")]
        Task<OperationResult<ResponseGeneric>> Registration([Body] CancellationToken ct, RegisterModel registerModel);
    }
}
