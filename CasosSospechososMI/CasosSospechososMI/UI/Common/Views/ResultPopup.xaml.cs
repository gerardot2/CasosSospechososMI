using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CasosSospechososMI.UI.Common.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPopup : PopupPage
    {

        public event EventHandler<object> OnConfirmEvent;
        public ResultPopup(string title, string message)
        {
            InitializeComponent();

            rpTitle.Text = title;
            resultMessage.Text = message;
        }
        private async void Confirm_Tapped(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            await PopupNavigation.Instance.PopAsync();
            OnConfirmEvent?.Invoke(this, EventArgs.Empty);
            IsBusy = false;
        }

        
    }
}
