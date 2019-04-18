using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Xamarin.Essentials;
using System;

namespace App2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private bool flashlightToggle = false;
        private bool networkToggle = false;
        SensorSpeed speed = SensorSpeed.UI;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var flashlightBtn = FindViewById<Button>(Resource.Id.flashlightBtn);
            flashlightBtn.Click += FlashlightBtn_Click;

            var barometerBtn = FindViewById<Button>(Resource.Id.barometerBtn);
            barometerBtn.Click += BarometerBtn_Click;

            var networkBtn = FindViewById<Button>(Resource.Id.networkBtn);
            networkBtn.Click += NetworkBtn_Click;

            Barometer.ReadingChanged += Barometer_ReadingChanged;

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            OrientationSensor.ReadingChanged += OrientationSensor_ReadingChanged;

            var orientationBtn = FindViewById<Button>(Resource.Id.orientationBtn);
            orientationBtn.Click += OrientationBtn_Click;

            var batteryBtn = FindViewById<Button>(Resource.Id.batteryBtn);
            batteryBtn.Click += BatteryBtn_Click;
        }

        private void BatteryBtn_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void NetworkBtn_Click(object sender, EventArgs e)
        {
            if (!networkToggle)
                FindViewById<TextView>(Resource.Id.networkAccess).Text = "Network access: " + Connectivity.NetworkAccess.ToString();
            else
                FindViewById<TextView>(Resource.Id.networkAccess).Text = "Network access: off";

            networkToggle = !networkToggle;
        }

        private void OrientationBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (OrientationSensor.IsMonitoring)
                {
                    OrientationSensor.Stop();
                    FindViewById<TextView>(Resource.Id.orientation).Text = "Orientation: off";
                }
                else
                    OrientationSensor.Start(speed);
            }
            catch (Exception ex)
            {
                Toast.MakeText(ApplicationContext, ex.Message, ToastLength.Long).Show();
            }
        }

        void OrientationSensor_ReadingChanged(object sender, OrientationSensorChangedEventArgs e)
        {
            var data = e.Reading;
            FindViewById<TextView>(Resource.Id.orientation).Text = $"Reading: X: {data.Orientation.X}, Y: {data.Orientation.Y}, Z: {data.Orientation.Z}, W: {data.Orientation.W}";
        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if(networkToggle)
                FindViewById<TextView>(Resource.Id.networkAccess).Text = "Network access: " + e.NetworkAccess.ToString();
        }

        private void BarometerBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (Barometer.IsMonitoring)
                {
                    Barometer.Stop();
                    FindViewById<TextView>(Resource.Id.barometer).Text = "Pressure: off";
                }
                else
                {
                    Barometer.Start(speed);
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(ApplicationContext, ex.Message, ToastLength.Long).Show();
            }
        }

        void Barometer_ReadingChanged(object sender, BarometerChangedEventArgs e)
        {
            var data = e.Reading;
            var barometer = FindViewById<TextView>(Resource.Id.barometer);
            barometer.Text = "Pressure: " + data.PressureInHectopascals;
        }

        private void FlashlightBtn_Click(object sender, EventArgs e)
        {
            if (!flashlightToggle)
            {
                try
                {
                    Flashlight.TurnOnAsync();
                    FindViewById<TextView>(Resource.Id.flashlight).Text = "Flashlight state: on";
                    flashlightToggle = true;
                }
                catch (Exception ex)
                {
                    Toast.MakeText(ApplicationContext, ex.Message, ToastLength.Long).Show();
                }
            }
            else
            {
                Flashlight.TurnOffAsync();
                FindViewById<TextView>(Resource.Id.flashlight).Text = "Flashlight state: off";
                flashlightToggle = false;
            }
        }
    }
}