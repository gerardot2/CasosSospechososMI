using CasosSospechososMI.Domain.Account;
using CasosSospechososMI.Domain.User;
using CasosSospechososMI.Services.Account.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CasosSospechososMI.UseCases.Account
{
    public class UpdateActualUser
    {
        private IAccountService _usuarioService;

        public UpdateActualUser(IAccountService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public void Invoke(User data)
        {
            _usuarioService.ActualUser = data;
        }
    }
}
