using CasosSospechososMI.Services.Interfaces;
using CasosSospechososMI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CasosSospechososMI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        IRoutingService routingService;
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel(routingService);
        }
    }
}