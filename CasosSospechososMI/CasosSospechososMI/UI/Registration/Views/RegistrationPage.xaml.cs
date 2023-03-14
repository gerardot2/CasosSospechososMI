using CasosSospechososMI.UI.Registration.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CasosSospechososMI.UI.Registration.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        RegistrationViewModel _viewModel;
        public RegistrationPage(bool supervisor = false)
        {
            InitializeComponent();
            _viewModel = App.ServiceProvider.GetService<RegistrationViewModel>();
            
            if (supervisor)
            {
                //items.Children.Remove(addressInput);
                items.Children.Remove(membersInput);
                items.Children.Remove(codeInput);
                //addressInput.IsVisible = false;
                //membersInput.IsVisible = false;
                //codeInput.IsVisible = false;
            }
            else
            {
                items.Children.Remove(passInput);
                items.Children.Remove(passRepInput);
            }
            this.BindingContext = _viewModel;
            passRepEntry.IsPassword = true;
            passEntry.IsPassword = true;
            _viewModel.IsSupervisor = supervisor;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
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
                await DisplayAlert("Error","Intente capturar nuevamente el código.","Aceptar");
            }
        }

        private void CustomEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (passEntry.Text != passRepEntry.Text || passRepEntry.Text.Length < 6)
            {
                passRepEntry.TextColor = Color.Red;
                _viewModel.PassMatch = false;
            }
            else
            {
                passRepEntry.TextColor = Color.Black;
                _viewModel.PassMatch = true;
            }
        }
        private void verPass_Tapped(object sender, EventArgs e)
        {
            passEntry.IsPassword = !passEntry.IsPassword;
        }
        private void verPassRep_Tapped(object sender, EventArgs e)
        {
            passRepEntry.IsPassword = !passRepEntry.IsPassword;
        }

        private void passEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (passEntry.Text.Length >= 6)
            {
                passEntry.TextColor = Color.Black;
                
            }
            else
            {
                passEntry.TextColor = Color.Red;
            }
        }

        
    }
}