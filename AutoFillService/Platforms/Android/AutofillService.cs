using Android.App;
using Android.Runtime;
using Android.Widget;
using Android;
using Android.Content;
using Android.OS;
using Android.Service.Autofill;

namespace AutoFillService.Droid.Autofill
{
    [Service(Permission = Manifest.Permission.BindAutofillService, Label = "Test Fill Service", Exported = true)]
    [IntentFilter(new string[] { "android.service.autofill.AutofillService" })]
    [MetaData("android.autofill", Resource = "@xml/autofillservice")]
    [Register("com.autofill.test.Autofill.AutofillService")]
    public class AutofillService : Android.Service.Autofill.AutofillService
    {
        public override async void OnFillRequest(FillRequest request, CancellationSignal cancellationSignal, FillCallback callback)
        {
            try
            {
                var focusedId = request.FillContexts?.LastOrDefault()?.FocusedId;
                var context = ApplicationContext;

                var intent = new Intent(context, typeof(AutofillExternalSelectionActivity));
                //var intent = new Intent(context, typeof(MainActivity)); // We can send intent for MainActivity but it will also crash

                var pendingIntent = PendingIntent.GetActivity(context, 1, intent, PendingIntentFlags.CancelCurrent | PendingIntentFlags.Mutable );

                // Build the presentation of the datasets
                RemoteViews usernamePresentation = new RemoteViews(PackageName,Android.Resource.Layout.SimpleListItem1);
                usernamePresentation.SetTextViewText(Android.Resource.Id.Text1, "Click to Crash!");

                var datasetBuilder = new Dataset.Builder();
                datasetBuilder.SetAuthentication(pendingIntent?.IntentSender);
                datasetBuilder.SetValue(focusedId, null, usernamePresentation);

                var fillResponseBuilder = new FillResponse.Builder().AddDataset(datasetBuilder.Build()); 
                callback.OnSuccess(fillResponseBuilder.Build());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        public override void OnSaveRequest(SaveRequest request, SaveCallback callback)
        {
            throw new NotImplementedException();
        }
    }
}
