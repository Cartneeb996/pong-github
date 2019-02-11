/*
 * Description:     A basic PONG simulator
 * Author:           
 * Date:            
 */

#region libraries

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;

#endregion

namespace Pong
{
    public partial class Form1 : Form
    {
        #region global values

      
        //graphics objects for drawing
        SolidBrush drawBrush = new SolidBrush(Color.White);
        Font drawFont = new Font("Courier New", 10);

        // Sounds for game
        SoundPlayer scoreSound = new SoundPlayer(Properties.Resources.score);
        SoundPlayer collisionSound = new SoundPlayer(Properties.Resources.collision);

        //determines whether a key is being pressed or not
        Boolean aKeyDown, zKeyDown, jKeyDown, mKeyDown;

        // check to see if a new game can be started
        Boolean newGameOk = true;

        //ball directions, speed, and rectangle
        Boolean ballMoveRight = true;
        Boolean ballMoveDown = true;
        Boolean obsMoveUp = true;
        int BALL_SPEED = 4;
        int ticker = 0;
        int ticker2 = 0;
 
        Rectangle ball;
        Rectangle Obs;

        //paddle speeds and rectangles
        const int PADDLE_SPEED = 6;
        Rectangle p1, p2;

        //player and game scores
        int player1Score = 0;
        int player2Score = 0;
        int gameWinScore = 10;  // number of points needed to win game

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.A:
                    aKeyDown = true;
                    break;
                case Keys.Z:
                    zKeyDown = true;
                    break;
                case Keys.J:
                    jKeyDown = true;
                    break;
                case Keys.M:
                    mKeyDown = true;
                    break;
                case Keys.Y:
                case Keys.Space:
                    if (newGameOk)
                    {
                        SetParameters();
                    }
                    break;
                case Keys.N:
                    if (newGameOk)
                    {
                        Close();
                    }
                    break;
            }
        }
        
        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //check to see if a key has been released and set its KeyDown value to false if it has
            switch (e.KeyCode)
            {
                case Keys.A:
                    aKeyDown = false;
                    break;
                case Keys.Z:
                    zKeyDown = false;
                    break;
                case Keys.J:
                    jKeyDown = false;
                    break;
                case Keys.M:
                    mKeyDown = false;
                    break;
            }
        }

        /// <summary>
        /// sets the ball and paddle positions for game start
        /// </summary>
        private void SetParameters()
        {
            if (newGameOk)
            {
                player1Score = player2Score = 0;
                newGameOk = false;
                startLabel.Visible = false;
                gameUpdateLoop.Start();
            }

            //set starting position for paddles on new game and point scored 
            const int PADDLE_EDGE = 20;  // buffer distance between screen edge and paddle            

            p1.Width = p2.Width = 10;    //height for both paddles set the same
            p1.Height = p2.Height = 40;  //width for both paddles set the same

            //p1 starting position
            p1.X = PADDLE_EDGE;
            p1.Y = this.Height / 2 - p1.Height / 2;

            //p2 starting position
            p2.X = this.Width - PADDLE_EDGE - p2.Width;
            p2.Y = this.Height / 2 - p2.Height / 2;

            // TODO set Width and Height of ball
            ball = new Rectangle(this.Width / 2 - PADDLE_EDGE, this.Height / 2 - PADDLE_EDGE, PADDLE_EDGE, PADDLE_EDGE);
            Obs = new Rectangle(this.Width / 2 - p2.Width, this.Height / 2 - 150, p2.Width, 50);
            this.Refresh();
            Thread.Sleep(1000);
            // TODO set starting X position for ball to middle of screen, (use this.Width and ball.Width)
            // TODO set starting Y position for ball to middle of screen, (use this.Height and ball.Height)


        }

        /// <summary>
        /// This method is the game engine loop that updates the position of all elements
        /// and checks for collisions.
        /// </summary>
        
        private void gameUpdateLoop_Tick(object sender, EventArgs e)
        {
            ticker++;
            if (ticker >= 5)
            {
               if(this.Width > 200) this.Width --;

                p2.X = this.Width - 20 - p2.Width;
                ticker = 0;
            }
            #region update ball position

            // TODO create code to move ball either left or right based on ballMoveRight and using BALL_SPEED
            if (ballMoveRight) ball.X += BALL_SPEED;
            else ball.X -= BALL_SPEED;
            if(ballMoveDown) ball.Y += BALL_SPEED;
            else ball.Y -= BALL_SPEED;
            // TODO create code move ball either down or up based on ballMoveDown and using BALL_SPEED

            #endregion
            if (Obs.Y + Obs.Height >= this.Height) obsMoveUp = true;
            else if (Obs.Y < 0) obsMoveUp = false;
            if(obsMoveUp)
            {
                Obs.Y -= PADDLE_SPEED;
            }
            else Obs.Y += PADDLE_SPEED;
            #region update paddle positions

            if (ball.Y + ball.Height < p2.Y && p2.Y > 0) p2.Y -= PADDLE_SPEED;
            else if (ball.Y > p2.Y && p2.Y + p2.Height < this.Height) p2.Y += PADDLE_SPEED;
           
            if (aKeyDown == true && p1.Y > 0)
            {
                p1.Y -= PADDLE_SPEED;
                // TODO create code to move player 1 paddle up using p1.Y and PADDLE_SPEED
            }
            else if (zKeyDown == true && p1.Y < this.Height)
            {
                p1.Y += PADDLE_SPEED;
                // TODO create code to move player 1 paddle up using p1.Y and PADDLE_SPEED
            }
            if (jKeyDown == true && p1.Y > 0)
            {
                p2.Y -= PADDLE_SPEED;
                // TODO create code to move player 1 paddle up using p1.Y and PADDLE_SPEED
            }
            else if (mKeyDown == true && p1.Y < this.Height)
            {
                p2.Y += PADDLE_SPEED;
                // TODO create code to move player 1 paddle up using p1.Y and PADDLE_SPEED
            }
            // TODO create an if statement and code to move player 1 paddle down using p1.Y and PADDLE_SPEED

            // TODO create an if statement and code to move player 2 paddle up using p2.Y and PADDLE_SPEED

            // TODO create an if statement and code to move player 2 paddle down using p2.Y and PADDLE_SPEED

            #endregion

            #region ball collision with top and bottom lines
            if (ball.IntersectsWith(Obs)) ballMoveRight = !ballMoveRight;
            if (ball.Y < 0 || ball.Y + ball.Height > this.Height) // if ball hits top line
            {
                ballMoveDown = !ballMoveDown;
                collisionSound.Play();
                // TODO use ballMoveDown boolean to change direction
                // TODO play a collision sound
            }
            

            // TODO In an else if statement use ball.Y, this.Height, and ball.Width to check for collision with bottom line
            // If true use ballMoveDown down boolean to change direction

            #endregion

            #region ball collision with paddles
           
            if (ball.IntersectsWith(p1) || ball.IntersectsWith(p2)) // if ball hits top line
            {
                ballMoveRight = !ballMoveRight;
                ticker2++;
                
                if (ticker2 >= 3)
                {
                    BALL_SPEED++ ;

                    ticker2 = 0;
                }
                collisionSound.Play();
            }
            
            

            // TODO create if statment that checks p2 collides with ball and if it does
            // --- play a "paddle hit" sound and
            // --- use ballMoveRight boolean to change direction

            /*  ENRICHMENT
             *  Instead of using two if statments as noted above see if you can create one
             *  if statement with multiple conditions to play a sound and change direction
             */

            #endregion

            #region ball collision with side walls (point scored)

            if (ball.X < 0)  // ball hits left wall logic
            {
                // TODO
                
                scoreSound.Play();
                player2Score++;
                if (player2Score >= gameWinScore)
                {
                    this.Width = 616;
                    SetParameters();
                    GameOver("Player 2");
                }
                else
                {

                    BALL_SPEED = 4;
                    ballMoveRight = true;
                    SetParameters();
                }

                // TODO use if statement to check to see if player 2 has won the game. If true run 
                // GameOver method. Else change direction of ball and call SetParameters method.

            }
            else if (ball.X + ball.Width > this.Width)  // ball hits left wall logic
            {
                // TODO
                
                scoreSound.Play();
                player1Score++;
                if (player1Score >= gameWinScore)
                {
                    this.Width = 616;
                    SetParameters();
                    GameOver("Player 1");
                }
                else
                {
                    BALL_SPEED = 4;
                    ballMoveRight = false;
                    SetParameters();
                }

                

            }

            

            #endregion

            //refresh the screen, which causes the Form1_Paint method to run
            this.Refresh();
        }
        
        /// <summary>
        /// Displays a message for the winner when the game is over and allows the user to either select
        /// to play again or end the program
        /// </summary>
        /// <param name="winner">The player name to be shown as the winner</param>
        private void GameOver(string winner)
        {
            newGameOk = true;

            
            startLabel.Text = winner + "\nWins!";
            startLabel.Visible = true;
            this.Refresh();
            gameUpdateLoop.Stop();
            BALL_SPEED = 4;
            Thread.Sleep(2000);
            
            startLabel.Text = "Play again?";
            

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(drawBrush, p1);
            e.Graphics.FillRectangle(drawBrush, p2);
            e.Graphics.FillEllipse(drawBrush, ball);
            e.Graphics.FillRectangle(drawBrush, Obs);
            // TODO draw paddles using FillRectangle

            // TODO draw ball using FillRectangle
            e.Graphics.DrawString(player1Score.ToString(), drawFont, drawBrush, new Point(this.Width / 3, this.Height / 10));
            e.Graphics.DrawString(player2Score.ToString(), drawFont, drawBrush, new Point(this.Width / 3 * 2, this.Height / 10));
            // TODO draw scores to the screen using DrawString
        }
        

    }
}
