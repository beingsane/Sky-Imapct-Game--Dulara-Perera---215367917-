using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using Android.Graphics.Drawables;

namespace App5
{
    [Activity(Label = "App5")]
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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.game);

            // Get our button from the layout resource,
            // and attach an event to it

            enemyMove();
            MoveBackground();

            mySensorManager = (SensorManager)
               GetSystemService(SensorService);

            mySensorManager.RegisterListener(this, mySensorManager.
            GetDefaultSensor(SensorType.Accelerometer),
            SensorDelay.Ui);

          Button  button = FindViewById<Button>(Resource.Id.shoot);
            button.Click += shoot;

        }

        public void enemyMove()
        {
            ImageView enemyr = FindViewById<ImageView>(Resource.Id.mini);
            enemyr.Animate()
                .SetDuration(3000)
                .Rotation(360);

           etimer = new System.Timers.Timer();
            etimer.Interval = 5;
            etimer.Elapsed += (sender, e) =>
            {

                RunOnUiThread(() =>
                {
                    ImageView enemy = FindViewById<ImageView>(Resource.Id.mini);
                    ImageView rocket = FindViewById<ImageView>(Resource.Id.rocket);

                    var metrics = Resources.DisplayMetrics;


                    int width = metrics.WidthPixels - 100;
                    int height = metrics.HeightPixels - 100;

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

                    if (rocket.GetY()+50<by && by > rocket.GetY() && rocket.GetX() + 50 > bx) {
                        ProgressBar userLife =  FindViewById<ProgressBar>(Resource.Id.user_life);
                        userLife.Progress -= 20;
                       
                        by = 0;
                        bx = width;

                        if (userLife.Progress == 0) {
                            etimer.Close();
                            AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            alert.SetTitle("Game Over");
                            alert.SetMessage("Unfortunately enemy has hit your rocket.");
                            alert.SetPositiveButton("Continue", (senderAlert, args) => {
                                Intent activity2 = new Intent(this, typeof(GameOver));
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

                float cZpos = (metrics.HeightPixels - (metrics.HeightPixels / 9) * currentZ) - 100; ;




                ship.SetY(cZpos);
                // ship.SetY(current*10);
            }

        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            throw new NotImplementedException();
        }

        private async void MoveBackground()
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

                    if(xA%50 == 0)
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


      

            laser.SetY(rocket.GetY()) ;
            laser.SetImageResource(Resource.Drawable.laser);
            laser.Alpha = 10;
            laser.Animate()
        .SetDuration(1000)
        .Alpha(0);

           

            if (rY + 25 > eY && eY+25 > rY) {
                enemyLife.Progress -= 10;

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
                        etimer.Start(); 
                    });
                    alert.SetNegativeButton("Exit", (senderAlert, args) => {
                        Intent activity2 = new Intent(this, typeof(Menu));
                        this.StartActivity(activity2);
                        this.Finish();
                    });
               
                    alert.Show();



                }
          
            }
        }
    }
}

