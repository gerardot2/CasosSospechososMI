using CasosSospechososMI.Models;
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
    public partial class MissingConnectivityView : ContentView
    {
        public MissingConnectivityView()
        {
            InitializeComponent();
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            MessagingCenter.Send(new GenericMessage(), "ReloadConnection");
        }
    }
}