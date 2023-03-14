using System;
using System.Collections.Generic;
using System.Text;

namespace CasosSospechososMI.Domain.Account.Enums
{
    public enum LoginStateEnum : short
    {
        NotAuthenticated = 0,
        AppAuthenticated = 1,
        UserAuthenticated = 2
    }
}
