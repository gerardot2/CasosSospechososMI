using CasosSospechososMI.Models;
using CasosSospechososMI.Services.Interfaces;
using CasosSospechososMI.ViewModels;
using CasosSospechososMI.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CasosSospechososMI.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;
        IRoutingService routingService;
        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel(routingService);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}