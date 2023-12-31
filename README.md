## Sample Android MAUI App for reproducing crash when trying to open the app from an "Autofill context".

### Steps to Crash:
- Run app
- Press "Click Me" button
- On the Autofill service screen choose "Test Fill Service"
- Without closing the app go to a different app (for example a browser)
- Press any input field and an option overlay should appear
- Click the option "Click to Crash!"
- App will crash on base.OnCreate()

### To Run App without crashing do the same steps after doing the change below on App.xaml.cs
- Comment "MainPage = new NavigationPage(new MainPage());"
- Uncomment "MainPage = new AppShell();"

### Notes:
- Same crash can also happen on MainActivity. For testing that just change the intent in AutofillService.cs to be MainActivity instead of AutofillExternalSelectionActivity
- Crashes with both MAUI .Net 8 RC1 and RC2
- Running on Visual Studio 2022 Preview (17.8.0 Preview 4)
