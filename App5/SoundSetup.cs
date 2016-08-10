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
using Android.Media;

namespace App5
{
    [Activity(Label = "SoundSetup")]
    public class SoundSetup : Activity
    {

        public static float sound_level = 1f;
        public static MediaPlayer player;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.settings);



            SeekBar seekBar = FindViewById<SeekBar>(Resource.Id.level);
            Button back = FindViewById<Button>(Resource.Id.back);
           

            seekBar.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
                if (e.FromUser)
                {
                    sound_level = Convert.ToInt16(seekBar.Progress)/1000f;
                    SoundSetup.player.SetVolume(SoundSetup.sound_level, SoundSetup.sound_level);
                }
            };

            back.Click += backe;
            }

        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(Menu));
            StartActivity(intent);
            player.Stop();
        }

        void backe(object sender, EventArgs e)
        {

          OnBackPressed();
        }
        }
}