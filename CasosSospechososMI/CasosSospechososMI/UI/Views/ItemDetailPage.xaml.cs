using CasosSospechososMI.Services.Interfaces;
using CasosSospechososMI.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace CasosSospechososMI.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        IRoutingService routingService;
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel(routingService);
        }
    }
}