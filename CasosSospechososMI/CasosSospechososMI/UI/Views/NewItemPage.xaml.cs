using CasosSospechososMI.Models;
using CasosSospechososMI.Services.Interfaces;
using CasosSospechososMI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CasosSospechososMI.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }
        IRoutingService routingService;
        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel(routingService);
        }
    }
}