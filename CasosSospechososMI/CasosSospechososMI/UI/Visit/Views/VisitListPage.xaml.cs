using CasosSospechososMI.Domain.Family;
using CasosSospechososMI.UI.FamilyData.ViewModels;
using CasosSospechososMI.UI.Visit.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CasosSospechososMI.UI.Visit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VisitListPage : ContentPage
    {
        VisitListViewModel _viewModel;
        public VisitListPage(FamilyDataModel familyDataModel)
        {
            InitializeComponent();
            _viewModel = App.ServiceProvider.GetService<VisitListViewModel>();
            this.BindingContext = _viewModel;
            _viewModel.SelectedFamily = familyDataModel;
            //_viewModel.Code = familyDataModel.CodigoOvitrampa;

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