using Android.App;
using Android.Content;
using Android.Provider;

namespace AutoFillService
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
            try
            {
                var intent = new Intent(Settings.ActionRequestSetAutofillService);
                intent.SetData(Android.Net.Uri.Parse("package:com.autofill.test"));
                activity.StartActivity(intent);
            }
            catch (ActivityNotFoundException)
            {
                var alertBuilder = new AlertDialog.Builder(activity);
                alertBuilder.SetMessage("Could not set Autofill Service.");
                alertBuilder.SetCancelable(true);
                alertBuilder.SetPositiveButton("Ok", (sender, args) =>
                {
                    (sender as AlertDialog)?.Cancel();
                });
                alertBuilder.Create().Show();
            }
        }
    }

}
