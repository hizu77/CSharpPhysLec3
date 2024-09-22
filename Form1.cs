using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{

    public sealed partial class Form1 : Form
    {
        private readonly Timer _timer;
        private float _radius;
        private float _velocity;
        private float _time;
        private readonly int _centerY;
        
        private bool _clicked;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Width = ClientSize.Width;
            Height = ClientSize.Height;
            
            _radius = 50;
            _velocity = 100;
            _centerY = Height / 2;

            _timer = new Timer();
            _timer.Interval = 15;
            _timer.Tick += OnTick;
            _timer.Start();
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (_clicked)
                _time += 0.03f;

            if (_time <= (Width - _radius) / _velocity)
            {
                Invalidate();
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Escape) return;
            
            Close();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var omega = _velocity / _radius;

            // ====== Оси X и Y с разметкой ======
            // Ось X (время), начиная с (0, центрY)
            g.DrawLine(Pens.Black, 0, _centerY, Width, _centerY);
            g.DrawString("Time (s)", Font, Brushes.Black, Width - 60, _centerY + 20);

            // Ось Y (высота), начиная с (1, 0)
            g.DrawLine(Pens.Black, 1, 0, 1, Height);

            // Разметка оси X (время) начиная с нуля
            for (float i = 0; i <= Width; i += 50)
            {
                var timeMark = i / _velocity;
                g.DrawLine(Pens.Black, i, _centerY - 5, i, _centerY + 5);
                g.DrawString($"{timeMark:F1}", Font, Brushes.Black, i - 10, _centerY + 10);
            }

            // Разметка оси Y (высота) начиная с нуля
            for (float i = 0; i <= Height; i += 50)
            {
                var heightMark = _centerY - i;
                g.DrawLine(Pens.Black, 1, i, 1, i);
                g.DrawString($"{heightMark}", Font, Brushes.Black, 1, i - 5);
            }

            // ======= Колесо ========
            // Положение центра колеса по оси X
            var centerX = _velocity * _time;

            // Центр колеса над осью O_x на радиус выше
            var centerY = _centerY - _radius;

            // Рисуем колесо
            g.DrawEllipse(Pens.Red, centerX - _radius, centerY - _radius, _radius * 2, _radius * 2);

            // Траектория точки на ободе(циклоида)
            // Вращение по часовой стрелке - изменяем знак угловой скорости omega
            var cycloidX = centerX - _radius * (float)Math.Sin(-omega * _time);
            var cycloidY = centerY - _radius * (float)Math.Cos(-omega * _time);

            // Отрисовка текущей точки по циклоиде
            g.FillEllipse(Brushes.Blue, cycloidX - 5, cycloidY - 5, 10, 10);

            // Отрисовка траектории точки(циклоида)
            for (float t = 0; t < _time; t += 0.01f)
            {
                var xt = omega * t * _radius - _radius * (float)Math.Sin(-omega * t);
                var yt = centerY - _radius * (float)Math.Cos(-omega * t);
                g.FillEllipse(Brushes.Blue, xt - 2, yt - 2, 4, 4);
            }
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            _clicked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _clicked = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _time = 0;
            _clicked = false;
            Invalidate();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(textBox1.Text, out var value))
            {
                _velocity = value;
            }
            else
            {
                _velocity = 100;
            }
            
            _time = 0;
            Invalidate();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(Radius.Text, out var value))
            {
                _radius = value;
            }
            else
            {
                _radius = 50;
            }
            
            _time = 0;
            Invalidate();
        }
    }
}