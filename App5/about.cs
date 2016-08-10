using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App5
{
    [Activity(Label = "about")]
    public class About : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.about);
        }

        public override void OnBackPressed()
        {

            Intent activity2 = new Intent(this, typeof(Menu));
            this.StartActivity(activity2);
            this.Finish();
            SoundSetup.player.Stop();
        }
        }
        

    }
    