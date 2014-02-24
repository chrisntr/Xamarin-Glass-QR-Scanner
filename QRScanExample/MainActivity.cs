using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Glass.App;
using Android.Media;
using Android.Glass.Media;
using Android.Views.Animations;

namespace QRScanExample
{
	[Activity (Label = "Scan Xamarin", Icon = "@drawable/Icon", MainLauncher = true, Enabled = true)]
	[IntentFilter (new String[]{ "com.google.android.glass.action.VOICE_TRIGGER" })]
	[MetaData ("com.google.android.glass.VoiceTrigger", Resource = "@xml/voicetriggerstart")]
	public class MainActivity : Activity
	{
		// The project requires the Google Glass Component from
		// https://components.xamarin.com/view/googleglass
		// so make sure you add that in to compile succesfully.
		protected override async void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Window.AddFlags (WindowManagerFlags.KeepScreenOn);

			var scanner = new ZXing.Mobile.MobileBarcodeScanner(this);
			scanner.UseCustomOverlay = true;
			scanner.CustomOverlay = LayoutInflater.Inflate(Resource.Layout.QRScan, null);;
			var result = await scanner.Scan();

			AudioManager audio = (AudioManager) GetSystemService(Context.AudioService);
			audio.PlaySoundEffect((SoundEffect)Sounds.Success);

			Window.ClearFlags (WindowManagerFlags.KeepScreenOn);

			if (result != null) {
				Console.WriteLine ("Scanned Barcode: " + result.Text);
				var card2 = new Card (this);
				card2.SetText (result.Text);
				card2.SetFootnote ("Just scanned!");
				SetContentView (card2.ToView());
			}
		}
	}
}
