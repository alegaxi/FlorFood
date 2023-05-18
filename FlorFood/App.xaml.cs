namespace FlorFood;

public partial class App : Application
{
	public App()
	{
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt+QHJqVk1hXk5Hd0BLVGpAblJ3T2ZQdVt5ZDU7a15RRnVfR11lSHdRdEdmX3xZeA==;Mgo+DSMBPh8sVXJ1S0R+X1pFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF5jTH5Vd0xhW31bdnFXTg==;ORg4AjUWIQA/Gnt2VFhiQlJPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXtTcERqW31cdXJTTmg=;MTk1MTkxMEAzMjMxMmUzMjJlMzNPVW1nS0NtRHBMZGgvbFR5djgyQ1VzQjB4YjRhd21Vd2VMMlNUMFRjT3BBPQ==;MTk1MTkxMUAzMjMxMmUzMjJlMzNaQXhxUWNKR2VVUy92dUs3RXBzNU5XWCthRWVtdkNBRFNGMGluZnFHR0xBPQ==;NRAiBiAaIQQuGjN/V0d+Xk9HfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5WdkNiUH1ccnFSRWFU;MTk1MTkxM0AzMjMxMmUzMjJlMzNpaEh3Y3M4Nzl6TFliUmNQc0NMbFNYdno2V0huSjdhOFBteGJVQmlWdGlNPQ==;MTk1MTkxNEAzMjMxMmUzMjJlMzNJZEx3Sms2Q0RRdTY3V2xpN0t0TGVjQXFiWnA0dStrelRiR2RmTkg5dlhVPQ==;Mgo+DSMBMAY9C3t2VFhiQlJPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXtTcERqW31cdXNcRGg=;MTk1MTkxNkAzMjMxMmUzMjJlMzNaa3BtVlk0bGxMc2tZN0pYRGsvOHNZa3NrRTNFVkpYQkxhYlBxZXR4dktzPQ==;MTk1MTkxN0AzMjMxMmUzMjJlMzNhMVZoL2ZsY1paeFRTRkpBcGQ0OTJzS2xNYjR4dEVKcWhXdFZ5VDM0VVFjPQ==;MTk1MTkxOEAzMjMxMmUzMjJlMzNpaEh3Y3M4Nzl6TFliUmNQc0NMbFNYdno2V0huSjdhOFBteGJVQmlWdGlNPQ==");
        InitializeComponent();
        var login = new NavigationPage(new MainPage());
        login.BarBackgroundColor = Colors.White;
        MainPage = login;
	}
}
