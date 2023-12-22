using Android.App;
using Android.Runtime;
using Android.Widget;
using Android;
using Android.Content;
using Android.OS;
using Android.Service.Autofill;
using Android.App.Slices;
using Android.Graphics.Drawables;
using Android.Widget.Inline;
using AndroidX.AutoFill.Inline;
using AndroidX.AutoFill.Inline.V1;
using Android.Views.Autofill;
using Javax.Crypto;

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

                var externalActivityIntent = new Intent(context, typeof(AutofillExternalSelectionActivity));
                externalActivityIntent.PutExtra("isExternalAutofill", true);
                var mainActivityIntent = new Intent(context, typeof(MainActivity)); // We can send intent for MainActivity but it will also crash
                mainActivityIntent.PutExtra("autofill", true);

                var externalActivityPendingIntent = PendingIntent.GetActivity(context, 1, externalActivityIntent, PendingIntentFlags.CancelCurrent | PendingIntentFlags.Mutable );
                var mainActivityPendingIntent = PendingIntent.GetActivity(context, 1, mainActivityIntent, PendingIntentFlags.CancelCurrent | PendingIntentFlags.Mutable );

                // Build the presentation of the datasets
                RemoteViews externalActivityPresentation = new RemoteViews(PackageName,Android.Resource.Layout.SimpleListItem1);
                externalActivityPresentation.SetTextViewText(Android.Resource.Id.Text1, "Direct External Autofill");
                RemoteViews mainActivityPresentation = new RemoteViews(PackageName,Android.Resource.Layout.SimpleListItem1);
                mainActivityPresentation.SetTextViewText(Android.Resource.Id.Text1, "MainActivity");

                //Inline presentations
                var specs = request.InlineSuggestionsRequest?.InlinePresentationSpecs;
                var mainActivitySlice = CreateInlinePresentationSlice(specs.Last(), "MainActivity", "", 0, "", mainActivityPendingIntent, context);
                var mainActivityInlinePresentation = new InlinePresentation(mainActivitySlice, specs.Last(), false);
                var externalActivitySlice = CreateInlinePresentationSlice(specs.Last(), "Direct External Autofill", "", 0, "", externalActivityPendingIntent, context);
                var externalActivityInlinePresentation = new InlinePresentation(externalActivitySlice, specs.Last(), false);

                //Dataset builder
                var mainActivityDatasetBuilder = new Dataset.Builder();
                mainActivityDatasetBuilder.SetAuthentication(externalActivityPendingIntent?.IntentSender);
                mainActivityDatasetBuilder.SetValue(focusedId, AutofillValue.ForText("text"), externalActivityPresentation, externalActivityInlinePresentation);

                //Dataset builder
                var externalActivityDatasetBuilder = new Dataset.Builder();
                externalActivityDatasetBuilder.SetAuthentication(mainActivityPendingIntent?.IntentSender);
                externalActivityDatasetBuilder.SetValue(focusedId, AutofillValue.ForText("text"), mainActivityPresentation, mainActivityInlinePresentation);
                
                var fillResponseBuilder = new FillResponse.Builder().AddDataset(mainActivityDatasetBuilder.Build()).AddDataset(externalActivityDatasetBuilder.Build());
                callback.OnSuccess(fillResponseBuilder.Build());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        private static Slice CreateInlinePresentationSlice(
            InlinePresentationSpec inlinePresentationSpec,
            string text,
            string subtext,
            int iconId,
            string contentDescription,
            PendingIntent pendingIntent,
            Context context)
        {
            var imeStyle = inlinePresentationSpec.Style;
            if (!UiVersions.GetVersions(imeStyle).Contains(UiVersions.InlineUiVersion1))
            {
                return null;
            }
            var contentBuilder = InlineSuggestionUi.NewContentBuilder(pendingIntent)
                .SetContentDescription(contentDescription);
            if (!string.IsNullOrWhiteSpace(text))
            {
                contentBuilder.SetTitle(text);
            }
            if (!string.IsNullOrWhiteSpace(subtext))
            {
                contentBuilder.SetSubtitle(subtext);
            }
            if (iconId > 0)
            {
                var icon = Icon.CreateWithResource(context, iconId);
                if (icon != null)
                {
                    if (iconId == Resource.Drawable.dotnet_bot)
                    {
                        icon.SetTintBlendMode(Android.Graphics.BlendMode.Dst);
                    }
                    contentBuilder.SetStartIcon(icon);
                }
            }
            return contentBuilder.Build().JavaCast<InlineSuggestionUi.Content>()?.Slice;
        }

        public override void OnSaveRequest(SaveRequest request, SaveCallback callback)
        {
            throw new NotImplementedException();
        }
    }
}
