using CasosSospechososMI.Domain.Account;
using CasosSospechososMI.Domain.Account.Enums;
using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Domain.Family;
using CasosSospechososMI.Domain.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CasosSospechososMI.Services.Account.Interfaces
{
    public interface IAccountService
    {
        User ActualUser { get; set; }
        HomeDataModel HomeUserData { get; set; }
        HomeDataSupervisorModel HomeUserSupervisorData { get; set; }
        DateTime LastSentSample { get; set; }
        Task LogOff(CancellationToken ct);
        Task<LoginStateEnum> GetLoginStateAsync(CancellationToken ct);
        Task<bool> RequestAuthTokenAsync(CancellationToken ct, string dni, string password);
        Task<OperationResult<ResponseGeneric>> RequestRegistrationAsync(CancellationToken ct, RegisterModel registerModel);
    }
}
