using CasosSospechososMI.UI.Sample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CasosSospechososMI.UI.Sample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SampleRecordingView : ContentPage
    {
        SampleRecordingViewModel _viewModel;
        public SampleRecordingView()
        {
            InitializeComponent();
            _viewModel = App.ServiceProvider.GetService<SampleRecordingViewModel>();
            this.BindingContext = _viewModel;
            _viewModel.General = formItemsContent;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //------------ Controlar todo el form cuando se envia formulario y sale de tab
            //new SampleRecordingView();
            if (_viewModel.completed || !_viewModel.popupOpened)
            {
                _viewModel.FormItems = new System.Collections.ObjectModel.ObservableCollection<Domain.Family.FormModel>();
                _viewModel.General.Children.Clear();
                _viewModel.FormRecord = new Domain.Family.FormRecordModel();
                _viewModel.completed = false;
                _viewModel.PhotoPath = null;
                _viewModel.HasPhoto = false;

                formItemsContent = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical
                };
            }
        }
        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            // Perform required operation after examining e.Value
        }
        protected override bool OnBackButtonPressed()
        {
            //if (_viewModel.FormRecord.DataLoaded)
            //{
            //    _viewModel.completed = true;
            //}
            return base.OnBackButtonPressed();
        }

        void CachedImage_Finish(System.Object sender, FFImageLoading.Forms.CachedImageEvents.FinishEventArgs e)
        {
            _viewModel.IsImageLoading = false;
        }


    }
}