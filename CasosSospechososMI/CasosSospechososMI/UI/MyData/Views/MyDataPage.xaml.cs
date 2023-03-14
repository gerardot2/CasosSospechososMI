using CasosSospechososMI.UI.MyData.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CasosSospechososMI.UI.MyData.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyDataPage : ContentPage
    {
        MyDataViewModel _viewModel;
        public MyDataPage()
        {
            InitializeComponent();
            _viewModel = App.ServiceProvider.GetService<MyDataViewModel>();
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