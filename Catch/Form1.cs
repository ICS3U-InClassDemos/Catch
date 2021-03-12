using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Catch
{
    public partial class Form1 : Form
    {
        int heroX = 280;
        int heroY = 540;
        int heroWidth = 40;
        int heroHeight = 10;
        int heroSpeed = 10;

        List<int> ballXList = new List<int>();
        List<int> ballYList = new List<int>();
        List<int> ballSpeedList = new List<int>();
        List<string> ballColourList = new List<string>();
        int ballSize = 10;

        int score = 0;
        int time = 500;

        bool leftDown = false;
        bool rightDown = false;

        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush greenBrush = new SolidBrush(Color.Green);
        SolidBrush goldBrush = new SolidBrush(Color.Gold);
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        Random randGen = new Random();
        int randValue = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftDown = true;
                    break;
                case Keys.Right:
                    rightDown = true;
                    break;
            }

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftDown = false;
                    break;
                case Keys.Right:
                    rightDown = false;
                    break;
            }

        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move hero
            if (leftDown == true && heroX > 0)
            {
                heroX -= heroSpeed;
            }

            if (rightDown == true && heroX < ClientSize.Width - heroWidth)
            {
                heroX += heroSpeed;
            }

            //check to see if a new ball should be created
            randValue = randGen.Next(0, 101);

            if (randValue < 1) //1% chance of gold ball, (extra time)
            {
                ballXList.Add(randGen.Next(10, this.Width - ballSize * 2));
                ballYList.Add(10);
                ballSpeedList.Add(randGen.Next(2, 10));
                ballColourList.Add("gold");
            }
            else if (randValue < 6) //5% change of red ball, (lose points)
            {
                ballXList.Add(randGen.Next(10, this.Width - ballSize * 2));
                ballYList.Add(10);
                ballSpeedList.Add(randGen.Next(2, 10));
                ballColourList.Add("red");
            }
            else if (randValue < 11) //5% change of green ball, (get points)
            {
                ballXList.Add(randGen.Next(10, this.Width - ballSize * 2));
                ballYList.Add(10);
                ballSpeedList.Add(randGen.Next(2, 10));
                ballColourList.Add("green");
            }

            // move balls
            for (int i = 0; i < ballXList.Count(); i++)
            {
                ballYList[i] += ballSpeedList[i];
            }

            //check if ball is below play area and remove it if it is
            for (int i = 0; i < ballXList.Count(); i++)
            {
                if (ballYList[i] > 550)
                {
                    ballXList.RemoveAt(i);
                    ballYList.RemoveAt(i);
                    ballSpeedList.RemoveAt(i);
                    ballColourList.RemoveAt(i);
                    break;
                }
            }

            //check collision of ball and paddle
            Rectangle heroRec = new Rectangle(heroX, heroY, heroWidth, heroHeight);

            for (int i = 0; i < ballXList.Count(); i++)
            {
                Rectangle ballRec = new Rectangle(ballXList[i], ballYList[i], ballSize, ballSize);

                if (heroRec.IntersectsWith(ballRec))
                {
                    if (ballColourList[i] == "green")
                    {
                        score += 5;
                    }
                    else if (ballColourList[i] == "red")
                    {
                        score -= 10;
                    }
                    else if (ballColourList[i] == "gold")
                    {
                        time += 50;
                    }

                    ballXList.RemoveAt(i);
                    ballYList.RemoveAt(i);
                    ballSpeedList.RemoveAt(i);
                    ballColourList.RemoveAt(i);
                    break;
                }
            }

            //decrease time counter and check to see if time is up
            time--;

            if (time == 0)
            {
                gameTimer.Enabled = false;
            }


            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //update labels
            timeLabel.Text = $"Time Left: {time}";
            scoreLabel.Text = $"Score: {score}";

            //draw ground
            e.Graphics.FillRectangle(greenBrush, 0, 550, 600, 50);

            //draw hero
            e.Graphics.FillRectangle(whiteBrush, heroX, heroY, heroWidth, heroHeight);

            //draw balls
            for (int i = 0; i < ballXList.Count(); i++)
            {
                if (ballColourList[i] == "red")
                {
                    e.Graphics.FillEllipse(redBrush, ballXList[i], ballYList[i], ballSize, ballSize);
                }
                else if (ballColourList[i] == "green")
                {
                    e.Graphics.FillEllipse(greenBrush, ballXList[i], ballYList[i], ballSize, ballSize);
                }
                else if (ballColourList[i] == "gold")
                {
                    e.Graphics.FillEllipse(goldBrush, ballXList[i], ballYList[i], ballSize, ballSize);
                }
            }

        }
    }
}
