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
    [Activity(Label = "GameOver")]
    public class GameOver : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.GameOver);
            FindViewById<TextView>(Resource.Id.score).Text = Intent.GetStringExtra("score") ;

            FindViewById<Button>(Resource.Id.main).Click += Manu;
            FindViewById<Button>(Resource.Id.retry).Click += Retry;



        }

        void Retry(object sender, EventArgs e)
        {
            Intent activity2 = new Intent(this, typeof(MainActivity));
            this.StartActivity(activity2);
            this.Finish();
        }
        void Manu(object sender, EventArgs e)
        {
            Intent activity2 = new Intent(this, typeof(Menu));
            this.StartActivity(activity2);
            this.Finish();
        }
    }
}