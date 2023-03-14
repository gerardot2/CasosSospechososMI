using CasosSospechososMI.UI.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CasosSospechososMI.UI.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerPage : ContentPage
    {
        PlayerViewModel _viewModel;
        public PlayerPage()
        {
            InitializeComponent();
            _viewModel = App.ServiceProvider.GetService<PlayerViewModel>();
            this.BindingContext = _viewModel;
        }
    }
}