﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;


namespace Desktop_Actor
{
    public class Animator
    {
        private GameObject gameObject;
        private Animations anims;
        private int currentGifFrame = 0;

        public Animator(GameObject gameObject)
        {
            this.gameObject = gameObject;

            // Save anim json data and deserialize an Animation class.
            string jsonData = File.ReadAllText(@Directory.GetCurrentDirectory() + "\\Data\\Anims.json");
            anims = JsonConvert.DeserializeObject<Animations>(jsonData);

            ArraySetup(); // Setup arrays for overflow handling.
        }

        public DateTime PrevFrameTime;
        public DateTime FrameTime;

        public double CalculateFPS()
        {
            // Default starting prev to earlier time for fps.
            if (PrevFrameTime == FrameTime)
                PrevFrameTime = DateTime.Now.AddMilliseconds(-2);
            else
                PrevFrameTime = FrameTime;

            FrameTime = DateTime.Now;
            return (FrameTime - PrevFrameTime).TotalMilliseconds / 1000;
        }

        private Point velocity;     // Calculate velocity based on average distance per sec.
        private int n = 0, d = 6;   // Array overflows at length 5, d is n-1.
        private Point[] prevPos = new Point[7];     // Save previous positions to get distance moved.
        private Point[] curPosDif = new Point[7];   // Save distances between previous positions.

        private void ArraySetup()
        {
            for (int i = 0; i < prevPos.Length; i++)
            {
                prevPos[i] = gameObject.Position;
                curPosDif[i].X = 0;
                curPosDif[i].Y = 0;
            }
        }

        public void UpdatePositions(double framesPerSecond)
        {
            // Ensure that the motion is moving at a
            var speed = 1200; // X units within 1 second
            var moveDistPerSecond = (int)(speed * framesPerSecond);

            // Follow cursor or apply gravity.
            if (gameObject.CursorDragging)
                CursorDragActor();
            else
                Gravity(moveDistPerSecond);

            gameObject.KeepInsidePlayArea(); // Keep gameobject inside boundaries.

            // Update cur Point and calculate dist moved from last pos.
            prevPos[n] = gameObject.Position;
            curPosDif[n].X = prevPos[n].X - prevPos[d].X;
            curPosDif[n].Y = prevPos[n].Y - prevPos[d].Y;

            // Increment so that curPos overflows at array end.
            n = OverflowInt(n);
            d = OverflowInt(d);

            // Calculate avg of total differences between movement in array.
            // Then move gameobject based on that to simulate velocity/physics.
            velocity = SumAvg(curPosDif);
            PhysicsMovement(moveDistPerSecond);
        }

        /// <summary>
        /// Set gameobject position to be centered with cursor.
        /// </summary>
        private void CursorDragActor()
        {
            gameObject.Position.X = (int)(Cursor.Position.X - (gameObject.MyDimensions.Width / 2));
            gameObject.Position.Y = (int)(Cursor.Position.Y - (gameObject.MyDimensions.Height / 2));
        }

        /// <summary>
        /// Apply gravitational force and logic.
        /// </summary>
        /// <param name="moveDistPerSecond">Allowed fall distance per second.</param>
        public void Gravity(int moveDistPerSecond)
        {
            gameObject.Position.Y += (int)Math.Floor(moveDistPerSecond*0.2);
        }

        /// <summary>
        /// Return incremented integar with overflow.
        /// </summary>
        /// <param name="i">Integar value to increment or overflow.</param>
        /// <returns></returns>
        private int OverflowInt(int i)
        {
            i++;
            if (i < 0)
                return prevPos.Length - 1;
            if (i > prevPos.Length - 1)
                return 0;
            return i;
        }

        /// <summary>
        /// Returns fake velocity based on averaged distance moved per update.
        /// </summary>
        /// <param name="points">Array of distances to average.</param>
        /// <returns></returns>
        private Point SumAvg(Point[] points)
        {
            Point sumAvg = new Point(0, 0);
            for (int i = 0; i < points.Length - 1; i++)
            {
                sumAvg.X += points[i].X;
                sumAvg.Y += points[i].Y;
            }
            sumAvg.X /= points.Length;
            sumAvg.Y /= points.Length;

            return sumAvg;
        }

        /// <summary>
        /// Apply physics forces on gameobject.
        /// </summary>
        /// <param name="moveDistPerSec">Allowed movement distance per second.</param>
        public void PhysicsMovement(int moveDistPerSec)
        {
            gameObject.Position.X += velocity.X;
            gameObject.Position.Y += velocity.Y;
        }

        /// <summary>
        /// Clamps speed but limiting maximum movement each update from exceeding maximum.
        /// </summary>
        /// <param name="prevPosition"></param>
        /// <param name="moveDistPerSecond"></param>
        private void ClampSpeed(Point prevPosition, float moveDistPerSecond)
        {
            // Get distance from positions before and after movement during this update.
            var distance = new Point(prevPosition.X - gameObject.Position.X, prevPosition.Y - gameObject.Position.X);
            var clampSpeedAmount = (int)((moveDistPerSecond*0.4) * .32f); // Amount to reduce speed by.

            if (distance.X <= 0 || distance.Y <= 0) return;
            // Apply clamp to X axis.
            if (gameObject.Position.X < 0)
                gameObject.Position.X -= clampSpeedAmount;
            else
                gameObject.Position.X += clampSpeedAmount;
            // Apply clamp to Y axis.
            if (gameObject.Position.Y < 0)
                gameObject.Position.Y -= clampSpeedAmount;
            else
                gameObject.Position.Y += clampSpeedAmount;
        }

        // Render target image.
        public void RenderActorFrame(Graphics gfx)
        {
            string path = Directory.GetCurrentDirectory() + "\\Data\\Frames\\" + GetAnimState();
            var img = FromFileImage(path);
            gameObject.MyDimensions.Width = img.Width;
            gameObject.MyDimensions.Height = img.Height;

            gfx.SmoothingMode = SmoothingMode.AntiAlias;
            gfx.CompositingQuality = CompositingQuality.HighQuality;

            gfx.DrawImage(img, gameObject.Position.X, gameObject.Position.Y);
        }
         //added this function for getting the exact frame from gif
        public void renderTargetAnimation(Graphics gfx) {
            string path = Directory.GetCurrentDirectory() + "\\Data\\Frames\\" + GetAnimState();
            var inImg = FromFileImage(path);
            //Get frames  
            var dimension = new FrameDimension(inImg.FrameDimensionsList[0]);
            int frameCount = inImg.GetFrameCount(dimension);
            Image img = null;
            int index = 0;
            int duration = 0;
            if(currentGifFrame >= frameCount)
            {
                currentGifFrame = 0;
            }

            inImg.SelectActiveFrame(dimension, currentGifFrame);
            var frame = inImg.Clone() as Image;
            img = frame;
            currentGifFrame++;
           
            gameObject.MyDimensions.Width = img.Width;
            gameObject.MyDimensions.Height = img.Height;

            gfx.SmoothingMode = SmoothingMode.AntiAlias;
            gfx.CompositingQuality = CompositingQuality.HighQuality;

            
            gfx.DrawImage(img, gameObject.Position.X, gameObject.Position.Y);

        }
        //the currentGifFrame int keeps track of current frame,
        //TODO: make boolean for move up down etc and keep track for frame so animation plays without glitch
        private string GetAnimState()
        {
            string[] animFrames = anims.Idle;
            Point velocity = SumAvg(curPosDif);

            if (velocity.Y < 0) // UP
            {
                animFrames = anims.Idle;
                //currentGifFrame = 0;
            }
            else if (velocity.Y > 0) // Down.
            {
                animFrames = anims.Air_vertical;
                currentGifFrame = 0;
            }

            if (velocity.X < 0) // Left.
            {
                animFrames = anims.Carry_left;
                currentGifFrame = 0;
            }
            else if (velocity.X > 0) // Right.
            {
                animFrames = anims.Carry_right;
                currentGifFrame = 0;
            }

            return animFrames[0];
        }

        // Return target image.
        private Image FromFileImage(string filePath)
        {
            return Image.FromFile(filePath);
        }
    }
}