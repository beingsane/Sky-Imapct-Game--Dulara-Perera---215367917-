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
    class Settings:Activity
    {
       public static int sound_level = 20;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.settings);
            Settings.sound_level = FindViewById<SeekBar>(Resource.Id.level).Progress;
        }


    }
}