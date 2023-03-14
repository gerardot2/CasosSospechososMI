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
    public partial class ConfirmPopup : PopupPage
    {
        public event EventHandler<object> OnConfirmEvent;
        public event EventHandler<object> OnCloseEvent;
        public ConfirmPopup(string title, string message, string confirm = null, EventHandler<object> ConfirmAction = null,EventHandler<object> CloseAction = null,bool alert = false)
        {
            InitializeComponent();

            rpTitle.Text = title;
            resultMessage.Text = message;
            confirmLabel.Text = confirm;
            OnConfirmEvent = ConfirmAction;
            OnCloseEvent = CloseAction;
            if (alert) { popupPK.BackgroundColor = Color.LightGreen; resultMessage.TextColor = Color.Black; }
        }
        private async void Confirm_Tapped(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            await PopupNavigation.Instance.PopAsync();
            OnConfirmEvent?.Invoke(this, EventArgs.Empty);
            IsBusy = false;
        }

        private async void Cancel_Tapped(object sender, EventArgs e)
        {
            if (IsBusy) return;
            IsBusy = true;
            OnCloseEvent?.Invoke(this, EventArgs.Empty);
            await PopupNavigation.Instance.PopAsync();
            IsBusy = false;
        }
    }
}
