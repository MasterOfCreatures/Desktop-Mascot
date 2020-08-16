using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Actor
{
    public class GameObject
    {
        private Rectangle _playArea;
        public Point Position;
        public Dimensions MyDimensions;
        public bool CursorDragging { private set; get; }
        public int screenLeft = SystemInformation.VirtualScreen.Left;
        public int screenTop = SystemInformation.VirtualScreen.Top;
        public int screenWidth = SystemInformation.VirtualScreen.Width;
        public int screenHeight = SystemInformation.VirtualScreen.Height;
        public Boolean gravitation { get; set; } = true;

        public GameObject(Control form, Rectangle playAreaBoundary)
        {
            this._playArea = playAreaBoundary;

            Position = new Point();
            MyDimensions = new Dimensions();

            // Subscribe to key press events.
            form.MouseDown += MouseClick;
            form.MouseUp += MouseUp;


            /*int screenLeft = SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;
            int screenWidth = SystemInformation.VirtualScreen.Width;
            int screenHeight = SystemInformation.VirtualScreen.Height;
            */
            // Starting position.
           MyDimensions.Width = form.Width / 2;
           MyDimensions.Height = form.Height / 3;

            Position.X = screenWidth / 2;
            Position.Y = screenHeight / 3;
            Console.WriteLine("screenTop: " + screenTop);
            Console.WriteLine("screenLeft: " + screenLeft);
            Console.WriteLine("screenWidth: " + screenWidth);
            Console.WriteLine("screenHeight: " + screenHeight);
        }

        /// <summary>
        /// Keeps game object inside of the play area's boundaries.
        /// </summary>
        public void KeepInsidePlayArea()
        {
            // If top of GameObject is higher than top of the boundary.
            if (Position.Y < _playArea.Y && !gravitation)
            {
                Position.Y = _playArea.Y;
                Console.WriteLine("BUMP_TOP");
            }
            // If bottom of GameObject is lower than bottom of the boundary.
            else if ((Position.Y + MyDimensions.Height) > (_playArea.Y + _playArea.Height))
            {
                Position.Y = _playArea.Y + (_playArea.Height - MyDimensions.Height);
            }

            // If left of GameObject is outside of boundary's left side.
            //does not work correctly anymore, is caused by different resolution settings between multiple screens but its not critical
            if (Position.X + MyDimensions.Width  <=0 )
            {
                //Position.X = _playArea.X + (MyDimensions.Width+1);
                Position.X = 0 + (MyDimensions.Width+1);
                Console.WriteLine("BUMP_LEFT");
                //  Position.X = screenLeft;
            }
            /*if (Position.X < screenLeft) {
                Position.X = screenLeft + MyDimensions.Width;
            }*/
            // If right of GameObject is outside of boundary's right side.
            /*else if (Position.X > screenWidth)
            {
                Position.X = screenWidth - MyDimensions.Width;
            }*/
            //else if ((Position.X) > (_playArea.X + _playArea.Width))
            else if ((Position.X) > (screenWidth))
            {
                //Position.X = (_playArea.X + _playArea.Width);
                Position.X = (screenWidth - (MyDimensions.Width/2));
                Console.WriteLine("BUMP_RIGHT");
            }
        }
        //this function is important because i managed to create some bugs along the journey
        public void resetActor() {
            Position.X = screenWidth / 2;
            Position.Y = screenHeight / 3;
        }

        #region Event Methods

        private void MouseClick(object sender, MouseEventArgs e)
        {
            CursorDragging = true;
            Console.WriteLine("CALL");
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            CursorDragging = false;
            Console.WriteLine("RELEASE");
        }

        #endregion Event Methods

        /// <summary>
        /// Returns point of left side at actor center height.
        /// </summary>
        /// <returns></returns>
        public Point Top()
        {
            return new Point(Position.X + MyDimensions.Width / 2, Position.Y);
        }

        public Point Bottom()
        {
            return new Point(Position.X + MyDimensions.Width / 2, Position.Y + MyDimensions.Height);
        }

        public Point Left()
        {
            return new Point(Position.X, Position.Y + MyDimensions.Height / 2);
        }

        public Point Right()
        {
            return new Point(Position.X + MyDimensions.Width, Position.Y + MyDimensions.Height / 2);
        }

        public struct Dimensions
        {
            public int Width;
            public int Height;
        }
    }
}