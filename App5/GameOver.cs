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

            SetContentView(Resource.Layout.activity_game_over);


            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Exit");
            alert.SetMessage("Are you sure to exit from the game?");
            alert.SetPositiveButton("Exit", (senderAlert, args) => {
                System.Environment.Exit(0);
            });
            alert.SetNegativeButton("Cancel", (senderAlert, args) => {

            });
            alert.Show();

            string score = Intent.GetStringExtra("score");
            Console.WriteLine("---------------------------------------------------------" + score);
            FindViewById<TextView>(Resource.Id.score).SetWidth(10);
        }
    }
}