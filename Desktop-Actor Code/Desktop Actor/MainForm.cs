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
        private System.Windows.Forms.MenuItem menuItemClose, menuItemConsole, menuResetActor;
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

            this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.menuResetActor,this.menuItemConsole,this.menuItemClose });

            this.menuResetActor.Index = 0;
            this.menuResetActor.Text = "RESET BUDDY";
            this.menuResetActor.Click += new System.EventHandler(this.menuResetActor_Click);
            this.menuItemConsole.Index = 1;
            this.menuItemConsole.Text = "ENABLE/DISABLE CONSOLE";
            this.menuItemConsole.Click += new System.EventHandler(this.menuItemConsole_Click);
            this.menuItemClose.Index = 2;
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
                consoleEnabled = false;
                var handle = GetConsoleWindow();
                ShowWindow(handle, SW_HIDE);
            }
            else {
                consoleEnabled = true;
                var handle = GetConsoleWindow();
                ShowWindow(handle, SW_SHOW);
            }
        }

        private void menuResetActor_Click(object Sender, EventArgs e) {
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
    }
}