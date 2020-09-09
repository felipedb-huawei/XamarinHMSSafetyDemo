using System;
using System.Security.Policy;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Com.Huawei.Agconnect.Config;
using Com.Huawei.Hmf.Tasks;
using Com.Huawei.Hms.Support.Api.Entity.Safetydetect;
using Com.Huawei.Hms.Support.Api.Safetydetect;

namespace HmsMapDemo
{




    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnSuccessListener, IOnFailureListener
    {
        public const string APP_ID = "102839041";

        private ISafetyDetectClient client;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            AGConnectServicesConfig config = AGConnectServicesConfig.FromContext(ApplicationContext);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SafetyDetect.GetClient(this).InitUserDetect();

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            Snackbar.Make(view, "User detection start", Snackbar.LengthLong)
                .SetAction("Close", (Android.Views.View.IOnClickListener)null).Show();
            detect();
        }

        public void checkPermission(string[] permissions, int requestCode)
        {
            foreach (string permission in permissions)
            {
                if (ContextCompat.CheckSelfPermission(this, permission) == Permission.Denied)
                {
                    ActivityCompat.RequestPermissions(this, permissions, requestCode);
                }
            }
        }

        private void detect()
        {
            client = SafetyDetect.GetClient(this);
            client.UserDetection(APP_ID).AddOnSuccessListener(this, this).AddOnFailureListener(this, this);
        }

        public void OnFailure(Java.Lang.Exception p0)
        {
            //The Verification failed. Something went wrong  
            Log.Info("Result", "verify: Failure result = " + p0.ToString());
        }

        public void OnSuccess(Java.Lang.Object p0)
        {
            UserDetectResponse response = (UserDetectResponse)p0;

            Log.Info("Result", "verify: Success result = " + response.ResponseToken.ToString());
        }
    }
}