using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace FriendlyCompany
{

    public class CirclePanel : Panel
    {
        public delegate void CrossingEdgeEventHandler(object sender, EventArgs e);
        public event CrossingEdgeEventHandler CrossingEdge;
            
        public CirclePanel(int x, int y)
        {
            Location = new Point(x, y);
            Size = new Size(30, 30);
            BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var brush = new SolidBrush(Color.Black))
                e.Graphics.FillEllipse(brush, 0, 0, Width, Height);
        }

        public void MoveCircle(int speed)
        {
            if (!IsDisposed && Parent != null)
            {
                if (Parent.InvokeRequired)
                {
                    Parent.BeginInvoke(new Action(() => Location = new Point(Location.X + speed, Location.Y)));
                }
                else
                {
                    Location = new Point(Location.X + speed, Location.Y);
                }
                // Check if edge is crossed and fire the event
                if (this.Location.X > this.Parent.ClientSize.Width)
                {
                    CrossingEdge?.Invoke(this, new EventArgs());
                }
            }
        }
    }

    public partial class Form1 : Form
    {
        private const int _сircleSpeed = 5;
        private const int _maxCircles = 5;

        private List<CirclePanel> _circles;
        private List<Thread> _circleThreads;

        public Form1()
        {
            InitializeComponent();

            _circles = new List<CirclePanel>();
            _circleThreads = new List<Thread>();

            Timer timer = new Timer();
            timer.Interval = 500;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_circles.Count < _maxCircles)
            {
                CirclePanel circle = new CirclePanel(0, 30 * _circles.Count);
                circle.CrossingEdge += Panel_CrossingEdge;
                _circles.Add(circle);
                Controls.Add(circle);

                Thread thread = new Thread(() =>
                {
                    while (true)
                    {
                        circle.MoveCircle(_сircleSpeed);
                        Thread.Sleep(100);
                    }
                });

                _circleThreads.Add(thread);
            }
            else
            {
                ((Timer)sender).Stop();
                StartAllCicles();
            }
        }

        private void StartAllCicles()
        {
            foreach (var thread in _circleThreads)
            {
                thread.Start();
            }
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach(var thread in _circleThreads)
            {
                thread.Abort();
                thread.Join();
            }
        }

        private void Panel_CrossingEdge(object sender, EventArgs e)
        {
            CirclePanel panelCrossing = sender as CirclePanel;
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => panelCrossing.Location = new Point(0, panelCrossing.Location.Y)));
            }
            else
            {
                panelCrossing.Location = new Point(0, panelCrossing.Location.Y);
            }
        }
    }
}
