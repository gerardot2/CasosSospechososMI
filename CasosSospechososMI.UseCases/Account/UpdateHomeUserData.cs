using CasosSospechososMI.Domain.Account;
using CasosSospechososMI.Domain.User;
using CasosSospechososMI.Services.Account.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CasosSospechososMI.UseCases.Account
{
    public class UpdateHomeUserData
    {
        private IAccountService _usuarioService;

        public UpdateHomeUserData(IAccountService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public void Invoke(HomeDataModel data)
        {
            _usuarioService.HomeUserData = data;
        }
    }
}
