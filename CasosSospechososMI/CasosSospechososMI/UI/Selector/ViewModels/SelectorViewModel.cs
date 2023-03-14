using CasosSospechososMI.Services.Interfaces;
using CasosSospechososMI.UI.Base.ViewModels;
using CasosSospechososMI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CasosSospechososMI.UI.Selector.ViewModels
{
    public class SelectorViewModel : BaseViewModel
    {
        public SelectorViewModel(IRoutingService routingService) : base(routingService) { }
    }
}
