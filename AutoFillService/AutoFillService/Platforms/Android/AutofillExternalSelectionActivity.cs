using Android.App;
using Android.Content.PM;
using Android.OS;

namespace AutoFillService.Droid.Autofill
{
    [Activity(
        Theme = "@style/Maui.SplashTheme",
        NoHistory = true,
        LaunchMode = LaunchMode.SingleTop)]
    public class AutofillExternalSelectionActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            System.Diagnostics.Debug.WriteLine("AutofillExternalSelectionActivity Created!");

            SetResult(Result.Canceled);
            Finish();
            return;
        }
    }
}
