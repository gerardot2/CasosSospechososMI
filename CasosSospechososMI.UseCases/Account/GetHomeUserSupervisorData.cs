using CasosSospechososMI.Domain.Account;
using CasosSospechososMI.Domain.User;
using CasosSospechososMI.Services.Account.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CasosSospechososMI.UseCases.Account
{
    public class GetHomeUserSupervisorData
    {
        private IAccountService _usuarioService;

        public GetHomeUserSupervisorData(IAccountService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public HomeDataSupervisorModel Invoke()
        {
            return _usuarioService.HomeUserSupervisorData;
        }
    }
}
