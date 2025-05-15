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
        //Hero variables
        Rectangle hero = new Rectangle(280, 540, 40, 10);
        int heroSpeed = 10;

        //Ball variables
        int ballSize = 10;
        int ballSpeed = 8;

        //List of balls
        List<Rectangle> ballList = new List<Rectangle>();

        int score = 0;
        int time = 100;

        bool leftPressed = false;
        bool rightPressed = false;

        SolidBrush greenBrush = new SolidBrush(Color.Green);
        SolidBrush whiteBrush = new SolidBrush(Color.White);

        Random randGen = new Random();
        int randValue = 0;

        int groundHeight = 50;

        public Form1()
        {
            InitializeComponent();
            Rectangle newBall = new Rectangle(100, 0, 10, 10);
            ballList.Add(newBall);

            newBall = new Rectangle(200, 0, 10, 10);
            ballList.Add(newBall);

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftPressed = true;
                    break;
                case Keys.Right:
                    rightPressed = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftPressed = false;
                    break;
                case Keys.Right:
                    rightPressed = false;
                    break;
            }
        }


        private void gameTime_Tick(object sender, EventArgs e)
        {
            //move hero
            if (leftPressed == true && hero.X > 0)
            {
                hero.X -= heroSpeed;
            }


            if (rightPressed == true && hero.X < this.Width - hero.Width)
            {
                hero.X += heroSpeed;
            }

            //move the balls down the screen
            for (int i = 0; i < ballList.Count; i++)
            {
                int y = ballList[i].Y + ballSpeed;
                ballList[i] = new Rectangle(ballList[i].X, y, ballList[i].Width, ballList[i].Height);
            }

            //check to see if a new ball should be created
            randValue = randGen.Next(1, 40);

            if (randValue < 99)
            {
                int x = randGen.Next(50, this.Width - 50);
                Rectangle newBall = new Rectangle(x, 0, 10, 10);
                ballList.Add(newBall);
            }

            //remove any balls that are no longer on screen
            for (int i = 0; i < ballList.Count; i++)
            {
                if (ballList[i].Y > this.Height - groundHeight)
                {
                    ballList.Remove(ballList[i]);
                }
            }

            //check for collision between hero and balls
            for (int i = 0; i < ballList.Count; i++)
            {
                if (hero.IntersectsWith(ballList[i]))
                {
                    ballList.Remove(ballList[i]);
                    score += 10;
                }
            }

            //check if time is done
            time--;

            if (time == 0)
            {
                gameTimer.Stop();
            }

            //redraw the screen
            Refresh();

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //update labels
            timeLabel.Text = $"Time Left: {time}";
            scoreLabel.Text = $"Score: {score}";

            //draw ground
            e.Graphics.FillRectangle(greenBrush, 0, this.Height - groundHeight, this.Width, groundHeight);

            //draw hero
            e.Graphics.FillRectangle(whiteBrush, hero);

            //draw balls
            for (int i = 0; i < ballList.Count; i++)
            {
                e.Graphics.FillEllipse(greenBrush, ballList[i]);
            }
        }


    }
}
