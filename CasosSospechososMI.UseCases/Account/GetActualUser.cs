using CasosSospechososMI.Domain.Account;
using CasosSospechososMI.Services.Account.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CasosSospechososMI.UseCases.Account
{
    public class GetActualUser
    {
        private IAccountService _usuarioService;

        public GetActualUser(IAccountService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public User Invoke()
        {
            return _usuarioService.ActualUser;
        }
    }
}
