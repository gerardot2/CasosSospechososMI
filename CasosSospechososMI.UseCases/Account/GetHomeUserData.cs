using CasosSospechososMI.Domain.Account;
using CasosSospechososMI.Domain.User;
using CasosSospechososMI.Services.Account.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CasosSospechososMI.UseCases.Account
{
    public class GetHomeUserData
    {
        private IAccountService _usuarioService;

        public GetHomeUserData(IAccountService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public HomeDataModel Invoke()
        {
            return _usuarioService.HomeUserData;
        }
    }
}
