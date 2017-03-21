using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Training
{
    public partial class MainPage : ContentPage
    {
        string translatedNumber;

        public MainPage()
        {
            InitializeComponent();

            translateButon.Clicked += new EventHandler(OnButtonEvent_Click);
            callButton.Clicked += new EventHandler(OnButtonEvent_Click);
            callHistoryButton.Clicked += new EventHandler(OnButtonEvent_Click);
        }

        void OnButtonEvent_Click(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button.Equals(translateButon))
            {
                OnTranslate();
            }
            else if (button.Equals(callButton))
            {
                OnCall();
            }
            else if (button.Equals(callHistoryButton))
            {
                OnCallHistory();
            }
        }

        private async void OnCallHistory()
        {
            await Navigation.PushAsync(new CallHistoryPage());
        }

        void OnTranslate()
        {
            translatedNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);
            if (!string.IsNullOrWhiteSpace(translatedNumber))
            {
                callButton.IsEnabled = true;
                callButton.Text = "Call " + translatedNumber;
            }
            else
            {
                callButton.IsEnabled = false;
                callButton.Text = "Call";
            }
        }

        async void OnCall()
        {
            if (await this.DisplayAlert(
                    "Dial a Number",
                    "Would you like to call " + translatedNumber + "?",
                    "Yes",
                    "No"))
            {
                var dialer = DependencyService.Get<IDialer>();
                if (dialer != null)
                {
                    App.PhoneNumbers.Add(translatedNumber);
                    callHistoryButton.IsEnabled = true;
                    dialer.Dial(translatedNumber);
                }
            }
        }
    }
}
