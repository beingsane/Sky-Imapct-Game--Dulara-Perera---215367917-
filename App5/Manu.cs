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
    [Activity(Label = "Manu", MainLauncher = true, Icon = "@drawable/logo")]
    public class Manu : Activity
    {
        Button button;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            button = FindViewById<Button>(Resource.Id.StartGame);
            button.Click += newGame;

            FindViewById<Button>(Resource.Id.about_btn).Click += about;
            FindViewById<Button>(Resource.Id.exit_btn).Click += exit;
        }
        void newGame(object sender, EventArgs e)
        {
            Intent activity2 = new Intent(this, typeof(MainActivity));
            this.StartActivity(activity2);
            this.Finish();
        }

        void about(object sender, EventArgs e)
        {
            Intent activity2 = new Intent(this, typeof(About));
            this.StartActivity(activity2);
            this.Finish();
        }

        void exit(object sender, EventArgs e)
        {
        //    AlertDialog.Builder alert = new AlertDialog.Builder(this);
        //    alert.SetTitle("Exit");
        //    alert.SetMessage("Are you sure to exit from the game?");
        //    alert.SetPositiveButton("Exit", (senderAlert, args) => {
        //        System.Environment.Exit(0);
        //    });
        //    alert.SetNegativeButton("Cancel", (senderAlert, args) => {
                
        //    });
        //    alert.Show();
        }
    }
}