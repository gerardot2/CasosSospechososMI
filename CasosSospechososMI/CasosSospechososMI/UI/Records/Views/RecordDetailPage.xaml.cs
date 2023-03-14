using CasosSospechososMI.Domain.Samples;
using CasosSospechososMI.UI.Records.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CasosSospechososMI.UI.Records.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecordDetailPage : ContentPage
    {
        RecordDetailViewModel _viewModel;
        public RecordDetailPage(SampleModel sample)
        {
            InitializeComponent();
            _viewModel = App.ServiceProvider.GetService<RecordDetailViewModel>();
            this.BindingContext = _viewModel;
            _viewModel.Sample = sample;
            _viewModel.General = sampleItems;
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