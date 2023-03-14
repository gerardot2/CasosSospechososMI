using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CasosSospechososMI.UI.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormQuestionCheckView : ContentView
    {
        public FormQuestionCheckView(string pregunta)
        {
            InitializeComponent();
            preguntaText.Text = pregunta;
        }

        private void TapGestureRecognizer_TappedYes(object sender, EventArgs e)
        {
            checkYes.Source = "radio_button_checked.png";
            checkNo.Source = "radio_button_unchecked.png";
        }
        private void TapGestureRecognizer_TappedNo(object sender, EventArgs e)
        {
            checkNo.Source = "radio_button_checked.png"; 
            checkYes.Source = "radio_button_unchecked.png";
        }
    }
}