using CasosSospechososMI.UI.Login.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZXing;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CasosSospechososMI.UI.Login.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel _viewModel;
        public LoginPage(bool supervisor = false)
        {
            InitializeComponent();
            _viewModel = App.ServiceProvider.GetService<LoginViewModel>();
            this.BindingContext = _viewModel;
            if (supervisor)
            {
                loginGrid.Children.Remove(trapCodeRow);
            }
            else
            {
                loginGrid.Children.Remove(passwordRow);
            }
            _viewModel.IsSupervisor = supervisor;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.CancellationTokenSource = new CancellationTokenSource();
            _viewModel.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            _viewModel.CancellationTokenSource.Cancel();
            _viewModel.IsBusy = false;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await scanner.Scan();
                if (result != null)
                {
                    codeEntry.Text = result.Text;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}