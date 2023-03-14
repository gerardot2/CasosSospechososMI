using CasosSospechososMI.UseCases.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CasosSospechososMI.UI.Header.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderCustomView : ContentView
    {
        GetActualUser _actualUser;
        public HeaderCustomView()
        {
            InitializeComponent();
            _actualUser = App.ServiceProvider.GetService<GetActualUser>();
            var user = _actualUser.Invoke();
            labelName.Text = (user != null && !string.IsNullOrEmpty(user.SurName) && !string.IsNullOrEmpty(user.Name)) ?
                user.FullName : "Usuario";
        }
    }
}