using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FriendlyCompany
{
    public class CirclePanel : Panel
    {
        public delegate void CrossingEdgeEventHandler(object sender, EventArgs e);
        public event CrossingEdgeEventHandler CrossingEdge;

        public CirclePanel()
        {
            Size = new Size(30, 30);
            BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var brush = new SolidBrush(Color.Black))
                e.Graphics.FillEllipse(brush, 0, 0, Width, Height);
        }

        public void MoveCircle(int x)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => MoveCircle(x)));
            }
            else
            {
                this.Left += x;

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
        private Timer _timer;
        private List<CirclePanel> _circles;

        private int _count;
        private const int MaxCount = 5;

        public Form1()
        {
            InitializeComponent();
            _circles = new List<CirclePanel>();
            _count = 0;
            _timer = new Timer();

            _timer.Interval = 200;
            _timer.Tick += timer_Tick;
            _timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (_circles.Count < MaxCount)
            {
                var cp = new CirclePanel();
                cp.Location = new Point(0, 30 * _count);
                cp.CrossingEdge += Panel_CrossingEdge;

                _circles.Add(cp);
                Controls.Add(_circles[_count]);

                _count++;
            }
            else
            {
                foreach (var circle in _circles)
                {
                    circle.MoveCircle(10);
                }
            }
        }

        private void Panel_CrossingEdge(object sender, EventArgs e)
        {
            CirclePanel panelCrossing = sender as CirclePanel;
            panelCrossing.Location = new Point(0, panelCrossing.Location.Y);
        }
    }
}