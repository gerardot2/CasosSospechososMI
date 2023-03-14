using CasosSospechososMI.Models;
using CasosSospechososMI.UI.Login.Views;
using CasosSospechososMI.UI.Selector.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CasosSospechososMI.UI.Selector.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectorPage : ContentPage
    {
        SelectorViewModel _viewModel;
        public SelectorPage()
        {
            InitializeComponent();
            _viewModel = App.ServiceProvider.GetService<SelectorViewModel>();
            this.BindingContext = _viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }
        private async void Family_Tapped(object sender, EventArgs e)
        {
            _viewModel.IsBusy = true;
            await Application.Current.MainPage.Navigation.PushModalAsync(new LoginPage());
            _viewModel.IsBusy = false;
        }
        private async void Supervisor_Tapped(object sender, EventArgs e)
        {
            _viewModel.IsBusy = true;
            await Application.Current.MainPage.Navigation.PushModalAsync(new LoginPage(true));
            _viewModel.IsBusy = false;
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            MessagingCenter.Send(new GenericMessage(), "Shutdown");
            return true;
        }

    }
}