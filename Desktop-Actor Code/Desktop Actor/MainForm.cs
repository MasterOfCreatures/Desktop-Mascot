using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace Desktop_Actor
{
    public partial class MainForm : Form
    {
        private GameObject actor;
        private readonly Animator Animator;
        private Rectangle gameObjectPlayArea;
        private System.Windows.Forms.NotifyIcon notify;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem menuItemClose, menuItemConsole, menuResetActor, menuGravitationBuddy, menuMinusGravitation, menuPlusGravitation, menuBiggerBuddy, menuSmallerBuddy, menuResetSize;
        private bool consoleEnabled = false;

        public MainForm()
        {
           
            InitializeComponent();
            int screenLeft = SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;
            int screenWidth = SystemInformation.VirtualScreen.Width;
            int screenHeight = SystemInformation.VirtualScreen.Height;
            this.Size = new System.Drawing.Size(screenWidth, screenHeight);
            this.Location = new System.Drawing.Point(screenLeft, screenTop);
            DoubleBuffered = true;
            Focus();
            BringToFront();
            TabStop = false;
            TopMost = true;
            KeyPreview = true;  // Prioritize parent key press over child.
            BackColor = Color.LimeGreen;
            TransparencyKey = Color.LimeGreen;
            ShowInTaskbar = false;
            WindowState = FormWindowState.Normal;
            FormBorderStyle = FormBorderStyle.None;


            //contextmenu with spare robot icon looks nice and closes the programm without taskmanager
            this.components = new Container();
            this.contextMenu = new ContextMenu();
            this.menuItemClose = new MenuItem();
            this.menuItemConsole = new MenuItem();
            this.menuResetActor = new MenuItem();
            this.menuBiggerBuddy = new MenuItem();
            this.menuSmallerBuddy = new MenuItem();
            this.menuResetSize = new MenuItem();
            this.menuGravitationBuddy = new MenuItem();
            this.menuMinusGravitation = new MenuItem();
            this.menuPlusGravitation = new MenuItem();

            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.menuResetActor,this.menuBiggerBuddy, this.menuSmallerBuddy,this.menuResetSize,this.menuGravitationBuddy,this.menuPlusGravitation,this.menuMinusGravitation,this.menuItemConsole,this.menuItemClose });

            this.menuResetActor.Index = 0;
            this.menuResetActor.Text = "RESET BUDDY POSITION";
            this.menuResetActor.Click += new System.EventHandler(this.menuResetPosition_Click);
            this.menuBiggerBuddy.Index = 1;
            this.menuBiggerBuddy.Text = "MAKE BUDDY BIGGER";
            this.menuBiggerBuddy.Click += new System.EventHandler(this.menuBiggerBuddy_Click);
            this.menuSmallerBuddy.Index = 2;
            this.menuSmallerBuddy.Text = "MAKE BUDDY SMALLER";
            this.menuSmallerBuddy.Click += new System.EventHandler(this.menuSmallerBuddy_Click);
            this.menuResetSize.Index = 3;
            this.menuResetSize.Text = "RESET BUDDY SIZE";
            this.menuResetSize.Click += new System.EventHandler(this.menuResetSize_Click);
            this.menuGravitationBuddy.Index = 4;
            this.menuGravitationBuddy.Text = "GRAVITATION OFF";
            this.menuGravitationBuddy.Click += new System.EventHandler(this.menuGravitation_Click);
            this.menuPlusGravitation.Index = 5;
            this.menuPlusGravitation.Text = "+++ GRAVITATION";
            this.menuPlusGravitation.Click += new System.EventHandler(this.menuPlusGravitation_Click);
            this.menuMinusGravitation.Index = 6;
            this.menuMinusGravitation.Text = "--- GRAVITATION";
            this.menuMinusGravitation.Click += new System.EventHandler(this.menuMinusGravitation_Click);
            this.menuItemConsole.Index = 7;
            this.menuItemConsole.Text = "ENABLE/DISABLE CONSOLE";
            this.menuItemConsole.Click += new System.EventHandler(this.menuItemConsole_Click);
            this.menuItemClose.Index = 8;
            this.menuItemClose.Text = "EXIT";
            this.menuItemClose.Click += new System.EventHandler(this.menuItemClose_Click);
            

            this.notify = new NotifyIcon(this.components);
            this.notify.Icon = Desktop_Actor.Properties.Resources.robot;
            this.notify.ContextMenu = this.contextMenu;
            this.notify.Text = "DESKTOP_MASKOT";
            this.notify.Visible = true;

            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);

            // Default boundary. Still Buggy caused by calculating different resolution works best when every monitor has the same resolution settings
            gameObjectPlayArea = new Rectangle(screenLeft, screenTop, screenWidth, screenHeight);

            // Player char and animator component.
            actor = new GameObject(this, gameObjectPlayArea);
            Animator = new Animator(actor);
        }

        protected override void OnPaint(PaintEventArgs eventArgs)
        {
            base.OnPaint(eventArgs);

            // Render actor.
            var gfx = eventArgs.Graphics;
            //old function
            //Animator.RenderActorFrame(gfx);
            //new function supports gif animation:
            Animator.renderTargetAnimation(gfx);
            gfx.ResetTransform();

            // Update movement position relative to fps.
            Animator.UpdatePositions(Animator.CalculateFPS());

            Invalidate(); // Force control to be redrawn.
        }

        private void menuItemClose_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.Close();
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        private void menuItemConsole_Click(object Sender, EventArgs e) {
            //change Console output
            if (consoleEnabled)
            {
                this.menuItemConsole.Text = "ENABLE CONSOLE";
                consoleEnabled = false;
                var handle = GetConsoleWindow();
                ShowWindow(handle, SW_HIDE);
            }
            else {
                this.menuItemConsole.Text="DISABLE CONSOLE";
                consoleEnabled = true;
                var handle = GetConsoleWindow();
                ShowWindow(handle, SW_SHOW);
            }
        }

        private void menuResetPosition_Click(object Sender, EventArgs e) {
            //do this multiple time so the actor will stay (does not work btw but creates funny effects)
            actor.resetActor();
            actor.resetActor();
            actor.resetActor();
            actor.resetActor();
            actor.resetActor();
            actor.resetActor();
            actor.resetActor();
            actor.resetActor();
            actor.resetActor();
            actor.resetActor();
        }

        private void menuBiggerBuddy_Click(object Sender, EventArgs e) {
            Animator.resizeFactor = Animator.resizeFactor + 0.1;
            Console.WriteLine("BIGGER BUDDY: "+Animator.resizeFactor);
        }
        private void menuSmallerBuddy_Click(object Sender, EventArgs e)
        {
            if (Animator.resizeFactor <= 0.11) {
                //Animator.resizeFactor = Animator.resizeFactor - 0.01;
                Console.WriteLine("CANT MAKE SMALLER");
            }
            else
            {
                Animator.resizeFactor = Animator.resizeFactor - 0.1;
                Console.WriteLine("SMALLER BUDDY: "+Animator.resizeFactor);
            }
        }
        private void menuResetSize_Click(Object Sender, EventArgs e) {
            Animator.resizeFactor = 1.0;
            Console.WriteLine("BUDDY SIZE: "+Animator.resizeFactor);
        }

        private void menuGravitation_Click(Object Sender, EventArgs e) {

            if (Animator.gravitation)
            {
                menuGravitationBuddy.Text = "ENABLE GRAVITATION";
                actor.gravitation = false;
                Animator.gravitation = false;
            }
            else {
                menuGravitationBuddy.Text = "DISABLE GRAVITATION";
                actor.gravitation = true;
                Animator.gravitation = true;
            }

        }
        private void menuPlusGravitation_Click(Object Sender, EventArgs e)
        {
            if (Animator.gravitationFactor <= 3)
            {
                Animator.gravitationFactor = Animator.gravitationFactor + 0.1;
                Console.WriteLine("Gravitation Factor:" + Animator.gravitationFactor);
            }
            else {
                Animator.gravitationFactor = 1.0;
                Console.WriteLine("Gravitation Factor Reached Max 3.0");
                Console.WriteLine("Gravitation Factor:" + Animator.gravitationFactor);
            }
        }

        private void menuMinusGravitation_Click(Object Sender, EventArgs e) {
            if (Animator.gravitationFactor >= -3)
            {
                Animator.gravitationFactor = Animator.gravitationFactor - 0.1;
                Console.WriteLine("Gravitation Factor:" + Animator.gravitationFactor);
            }
            else
            {
                Animator.gravitationFactor = 1.0;
                Console.WriteLine("Gravitation Factor Reached Min -3.0");
                Console.WriteLine("Gravitation Factor:" + Animator.gravitationFactor);
            }
        }

    }

}