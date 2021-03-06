﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Graphics.Drawables;
using Android.Media;

namespace App5
{
    [Activity(Label = "Game")]
    public class MainActivity : Activity, ISensorEventListener
    {
        float bx = 0;
        float by = 0;
        int dx = 1;
        int dy = 1;
        int xA = 0;
        int score = 0;
        int level = 1;
        System.Timers.Timer etimer;
       

        private static readonly object myLock = new object();
        private SensorManager mySensorManager;
        private float x = 0, y = 0, z = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.game);
            if (savedInstanceState != null)
            {
                score = savedInstanceState.GetInt("score");
                level = savedInstanceState.GetInt("level");

                TextView scoreText = FindViewById<TextView>(Resource.Id.score);
                scoreText.Text = score.ToString();
            }
            else
            {

                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Alert");
                alert.SetMessage("Use gyroscope to move rocket up and down!");
                alert.SetPositiveButton("Play", (senderAlert, args) => {

                });




                alert.Show();
            }
          

            enemyMove();
           MoveBackground();
            backgroundMusic();

            mySensorManager = (SensorManager)
            GetSystemService(SensorService);

            mySensorManager.RegisterListener(this, mySensorManager.
            GetDefaultSensor(SensorType.Accelerometer),
            SensorDelay.Ui);

            Button  button = FindViewById<Button>(Resource.Id.shoot);
            button.Click += shoot;

            FindViewById<Button>(Resource.Id.more).Click += shoWMenu;
        }

        private void backgroundMusic()
        {
            SoundSetup.player = MediaPlayer.Create(this, Resource.Raw.backsound);
            SoundSetup.player.Start();
            SoundSetup.player.Looping = true;
            SoundSetup.player.SetVolume(SoundSetup.sound_level, SoundSetup.sound_level);

            
        }

        private void shoWMenu(object sender, EventArgs e)

        {
            etimer.Stop();
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Pause");
            alert.SetMessage("Please select a option" + level);
            alert.SetPositiveButton("Resume", (senderAlert, args) => {
                etimer.Start();
            });
            alert.SetNegativeButton("Quit Game", (senderAlert, args) => {
                OnBackPressed();
            });

           

            alert.Show();

        }

        public void enemyMove()
        {
          
            etimer = new System.Timers.Timer();
            etimer.Interval = 24 - (level*4);
            etimer.Elapsed += (sender, e) =>
            {

                RunOnUiThread(() =>
                {
                    ImageView enemy = FindViewById<ImageView>(Resource.Id.mini);
                    ImageView rocket = FindViewById<ImageView>(Resource.Id.rocket);

                    var metrics = Resources.DisplayMetrics;


                    int width = metrics.WidthPixels - enemy.Width;
                    int height = metrics.HeightPixels - (enemy.Width*2);

                    if (bx > width)
                    {
                        dx = -1;

                    }
                    if (bx < 20)
                    {
                        dx = 1;

                    }


                    if (by > height)
                    {
                        dy = -1;


                    }
                    if (by < 0)
                    {
                        dy = 1;
                    }




                    if ((rocket.GetY()+(rocket.Height/2)<= by+enemy.Height || (rocket.GetY() + (rocket.Height / 2) <= by) && by >= rocket.GetY()) && rocket.GetX() + rocket.Width >= bx) {
                        ProgressBar userLife =  FindViewById<ProgressBar>(Resource.Id.user_life);
                        userLife.Progress -= 20;


                        rocket.Alpha = 0;
                        rocket.Animate()
                    .SetDuration(5000)
                    .Alpha(10);


                        MediaPlayer player1;
                        player1 = MediaPlayer.Create(this, Resource.Raw.blast);
                        player1.Start();
                        player1.SetVolume(SoundSetup.sound_level, SoundSetup.sound_level);


                        by = 0;
                        bx = width;

                        if (userLife.Progress == 0) {
                            etimer.Close();
                            AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            alert.SetTitle("Game Over");
                            alert.SetMessage("Unfortunately enemy has hit your rocket.");
                            alert.SetPositiveButton("Continue", (senderAlert, args) => {
                                Intent activity2 = new Intent(this, typeof(GameOver));
                                string score = FindViewById<TextView>(Resource.Id.score).Text;
                                activity2.PutExtra("score", score);
                                this.StartActivity(activity2);
                                this.Finish();
                            });
                            
                            alert.Show();

                        }
                    }

                    bx = bx + dx;
                    enemy.SetX(bx);

                    by = by + dy;
                    enemy.SetY(by);

                });

            };
            etimer.Enabled = true;

            


        }


        public void OnSensorChanged(SensorEvent e)
        {
           
            lock (myLock)
            {
                float currentX = e.Values[0];
                float currentY = e.Values[1];
                float currentZ = e.Values[2];
                // First time
                if (z == 0 && y == 0 && z == 0)
                {
                    z = currentZ;
                    y = currentY;
                    x = currentX;
                }
                else if (Math.Abs(currentX / x) > 2 || Math.Abs(currentY / y) > 2 || Math.Abs(currentZ / z) > 2)
                {
                    z = currentZ;
                    y = currentY;
                    x = currentX;
                    
                }

                ImageView ship = (ImageView)FindViewById<ImageView>(Resource.Id.rocket);

                var metrics = Resources.DisplayMetrics;



                if (currentZ > 7)
                {
                    currentZ = 7;

                }
                else if (currentZ < 1)
                {
                    currentZ = 1;

                }

                Button more = (Button)FindViewById<Button>(Resource.Id.more);
                var dh = more.GetY() ;
                float cZpos = (dh - (dh / 9) * currentZ) ; ;




                ship.SetY(cZpos);
                // ship.SetY(current*10);
            }

        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            throw new NotImplementedException();
        }

        private void MoveBackground()
        {


            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 50;
            timer.Elapsed += (sender, e) =>
            {

                RunOnUiThread(() =>
                {
                    LinearLayout ll = (LinearLayout)FindViewById<LinearLayout>(Resource.Id.back);
                    ll.ScrollTo(xA, 0);
                    xA++;

                    ImageView newImg = new ImageView(this);
                    newImg.SetImageResource(Resource.Drawable.loop);
                    newImg.SetScaleType(ImageView.ScaleType.CenterCrop);
                    newImg.SetPadding(0, 0, 0, 0);
                    LinearLayout.LayoutParams parms = new LinearLayout.LayoutParams(500, LinearLayout.LayoutParams.MatchParent);
                    newImg.LayoutParameters = parms;

                    if (xA % 50 == 0)
                        ll.AddView(newImg);
                });

            };
            timer.Enabled = true;



        }

        void shoot(object sender, EventArgs e)
        {
            ImageView enemy = FindViewById<ImageView>(Resource.Id.mini);
            ImageView rocket = FindViewById<ImageView>(Resource.Id.rocket);
            ImageView laser = FindViewById<ImageView>(Resource.Id.laser);
            ProgressBar enemyLife = FindViewById<ProgressBar>(Resource.Id.enemy_life);

            float eY = enemy.GetY();
            float rY = rocket.GetY();

            MediaPlayer player;
            player = MediaPlayer.Create(this, Resource.Raw.laser);
            player.Start();


            laser.SetY(rY+(rocket.Height/2)-(laser.Height/2)) ;
            laser.SetImageResource(Resource.Drawable.laser);
            laser.Alpha = 10;
            laser.Animate()
        .SetDuration(1000)
        .Alpha(0);

           

            if ((rY + rocket.Height > eY && eY+enemy.Height >rY) || (eY + enemy.Height > rY && rY + rocket.Height> eY)) {
                enemyLife.Progress -= 5;

                score += (100 - enemyLife.Progress);
                TextView scoreText = FindViewById<TextView>(Resource.Id.score);
                scoreText.Text = score.ToString();

                if (enemyLife.Progress==0) {
                    etimer.Close();
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Level up");
                    alert.SetMessage("You have successfully completed level "+level);
                    alert.SetPositiveButton("Continue", (senderAlert, args) => {
                        FindViewById<ProgressBar>(Resource.Id.enemy_life).Progress = 100;
                        level++;
                        enemyMove();

                        float scalingFactor = (10-2*level)/10f; 
                        enemy.ScaleX =scalingFactor;
                        enemy.ScaleY = scalingFactor;

                    });
                    alert.SetNegativeButton("Exit", (senderAlert, args) => {
                        Intent activity2 = new Intent(this, typeof(Android.Views.Menu));
                        this.StartActivity(activity2);
                        this.Finish();
                    });
               
                    alert.Show();



                }
          
            }
        }


        public override void OnBackPressed()
        {
          
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Exit");
            alert.SetMessage("You sure to exit the game?");
            alert.SetPositiveButton("Yes", (senderAlert, args) => {
                Intent activity2 = new Intent(this, typeof(Menu));
                this.StartActivity(activity2);
                this.Finish();
            });
            alert.SetNegativeButton("No", (senderAlert, args) => {
               
            });

            alert.Show();
        }
        protected override void OnSaveInstanceState(Bundle outState)
        {
            SoundSetup.player.Stop();
            outState.PutInt("score", score);
            outState.PutInt("level", level);
            base.OnSaveInstanceState(outState);
        }
        protected override void OnPause()
        {
            SoundSetup.player.Stop();
            base.OnPause();
        }
    }
}

