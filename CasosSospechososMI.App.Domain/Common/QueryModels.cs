using Refit;
using System;
using System.Collections.Generic;
using System.Text;

namespace CasosSospechososMI.Domain.Common
{
    public class HomeQuery
    {
        [AliasAs("tipo_aplicacion")]
        public string TipoAplicacion { get; set; }
    }
    public class QueryModels2
    {
        [AliasAs("tipo_aplicacion")]
        public string TipoAplicacion { get; set; }
    }
}
