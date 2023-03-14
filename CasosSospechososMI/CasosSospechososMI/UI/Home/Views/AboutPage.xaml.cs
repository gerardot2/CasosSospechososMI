using CasosSospechososMI.UI.Home.ViewModels;
using CasosSospechososMI.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CasosSospechososMI.UI.Home.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        AboutViewModel _viewModel;
        public AboutPage()
        {
            InitializeComponent();
            _viewModel = App.ServiceProvider.GetService<AboutViewModel>();
            this.BindingContext = _viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}