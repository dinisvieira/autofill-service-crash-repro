﻿using Android.App;
using Android.Content.PM;
using Android.OS;

namespace AutoFillService
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            var isAutofill = Intent.GetBooleanExtra("autofill", false);
            base.OnCreate(savedInstanceState); 
            System.Diagnostics.Debug.WriteLine("MainActivity Created!");
        }
    }
}
