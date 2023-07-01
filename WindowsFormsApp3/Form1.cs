using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FriendlyCompany
{
    public class CirclePanel : Panel
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var brush = new SolidBrush(Color.Black))
                e.Graphics.FillEllipse(brush, 0, 0, Width, Height);
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

            _timer.Interval = 1000;
            _timer.Tick += timer_Tick;
            _timer.Start();

            DoubleBuffered = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (_circles.Count < MaxCount)
            {
                var cp = new CirclePanel();
                cp.Location = new Point(0, 30 * _count); //adjust if needed
                cp.Size = new Size(30, 30); //adjust if needed
                cp.BackColor = Color.Transparent;

                _circles.Add(cp);
                Controls.Add(_circles[_count]);

                _count++;
            }
            else
            {
                foreach (var circle in _circles)
                {
                    circle.Location = new Point(circle.Location.X + 10, circle.Location.Y); //move right
                    if (circle.Location.X > this.ClientSize.Width)
                    {
                        circle.Location = new Point(0, circle.Location.Y);
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //private void InitializeComponent()
        //{
        //}
    }
}