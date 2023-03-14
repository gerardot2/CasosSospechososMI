using CasosSospechososMI.UI.FamilyData.ViewModels;
using CasosSospechososMI.UI.Registration.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CasosSospechososMI.UI.FamilyData.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FamilyDataPage : ContentPage
    {
        FamilyDataViewModel _viewModel;
        public FamilyDataPage()
        {
            InitializeComponent();
            _viewModel = App.ServiceProvider.GetService<FamilyDataViewModel>();
            this.BindingContext = _viewModel;
            dataLoaded = true;
        }
        bool dataLoaded;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
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
                await DisplayAlert("Error", "Intente capturar nuevamente el código.", "Aceptar");
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //_viewModel.Data = null;
            //_viewModel.SelectedCity = null;
            _viewModel.ShowFields = false;
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            try
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await scanner.Scan();
                if (result != null)
                {
                    codeEntry1.Text = result.Text;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Intente capturar nuevamente el código.", "Aceptar");
            }
        }
        private void CustomEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!dataLoaded)
            {
                _viewModel.SaveButtonEnabled = true;
            }
            dataLoaded = false;

        }

        private void localidadPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!dataLoaded)
            {
                _viewModel.SaveButtonEnabled = true;
            }
            dataLoaded = false;
        }
    }
}