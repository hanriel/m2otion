using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace m2otion
{
    public partial class Form1 : Form
    {
        List<Mc> mcs = new List<Mc>();
        Random r = new Random();

        double dR = 5;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            double mass = r.Next(1,10);
            mcs.Add(new Mc(mass, mass*10, new Vector2(e.X, e.Y), Color.FromArgb(r.Next(20, 255), r.Next(20, 255), r.Next(20, 255)), new Vector2(r.Next(-5,5), r.Next(-5, 5))));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            using (Graphics g = this.CreateGraphics())
            {
                g.Clear(Color.White);
                foreach (Mc mc in mcs)
                {

                    if (mc.POS.X - mc.R < 0 || (mc.POS.X + mc.R) > ClientSize.Width) {
                        mc.setVEL(mc.VEL.X * -1, mc.VEL.Y);
                    }
                    if (mc.POS.Y - mc.R < 0 || (mc.POS.Y + mc.R) > ClientSize.Height) {
                        mc.setVEL(mc.VEL.X, mc.VEL.Y * -1);
                    }

                    foreach (Mc target in mcs)
                    {
                        if (target.POS % mc.POS <= (target.R + mc.R) && !target.Equals(mc))
                        {

                            double c1 = mc.VEL.X / (Math.Sqrt(Math.Pow(mc.VEL.X, 2) + Math.Pow(mc.VEL.Y, 2)));
                            double c2 = target.VEL.X / (Math.Sqrt(Math.Pow(target.VEL.X, 2) + Math.Pow(target.VEL.Y, 2)));
                            double c3 = (mc.VEL * target.VEL) / (mc.VEL.Length * target.VEL.Length);

                            double o1 = Math.Acos(c1) * 180 / Math.PI;  //Угол 1 вектора
                            double o2 = Math.Acos(c2) * 180 / Math.PI;  //Угол 2 вектора
                            double f = Math.Acos(c3) * 180 / Math.PI;   //Угол столкновения

                            double v1 = mc.VEL.Length;                     //Модуль вектора скорости 1
                            double v2 = target.VEL.Length;                 //Модуль вектора скорости 2

                            double m1 = mc.Mass;                        //Масса 1 тела
                            double m2 = target.Mass;                    //Масса 2 тела

                            mc.setVEL(
                                ((v1 * Math.Cos(o1 - f) * (m1 - m2) + 2 * m2 * v2 * Math.Cos(o2 - f)) / (m1 + m2)) * Math.Cos(f) + v1 * Math.Sin(o1 - f) * Math.Cos(f + (Math.PI / 2)),
                                ((v1 * Math.Cos(o1 - f) * (m1 - m2) + 2 * m2 * v2 * Math.Cos(o2 - f)) / (m1 + m2)) * Math.Sin(f) + v1 * Math.Sin(o1 - f) * Math.Sin(f + (Math.PI / 2))
                            );

                            target.setVEL(
                                ((v2 * Math.Cos(o2 - f) * (m2 - m1) + 2 * m1 * v2 * Math.Cos(o1 - f)) / (m2 + m1)) * Math.Cos(f) + v1 * Math.Sin(o2 - f) * Math.Cos(f + (Math.PI / 2)),
                                ((v2 * Math.Cos(o2 - f) * (m2 - m1) + 2 * m1 * v2 * Math.Cos(o1 - f)) / (m2 + m1)) * Math.Sin(f) + v1 * Math.Sin(o2 - f) * Math.Sin(f + (Math.PI / 2))
                            );
                        }
                    }
                    mc.POS += mc.VEL;
                    g.FillEllipse(new Pen(mc.Color).Brush, (float)(mc.POS.X - mc.R), (float)(mc.POS.Y - mc.R), (float)mc.R * 2, (float)mc.R * 2);

                    //g.DrawLine(new Pen(Color.Red), (float)(mc.POS.X), (float)(mc.POS.Y), (float)(mc.POS.X + mc.VEL.X * 5), (float)(mc.POS.Y + mc.VEL.Y * 5));
                    //Font drawfont = new Font("arial", 16);
                    //StringFormat drawformat = new StringFormat();
                    //string text = "x" + (int)mc.VEL.X + " y" + (int)mc.VEL.Y;
                    //g.DrawString(text, drawfont, new Pen(Color.Black).Brush, (float)(mc.POS.X - mc.R), (float)(mc.POS.Y - mc.R));
                    //drawfont.Dispose();
                }
                g.Dispose();
            }
            
        }

        
    }

    class Mc
    {
        double m, r;    //pos x, pos y, mass, radius
        private Vector2 pos, vel;

        public double R { get { return r; } }
        public double Mass { get { return m; } }

        public void setVEL(double x, double y)
        {
            vel.X = x;
            vel.Y = y;
        }

        public Vector2 VEL
        {
            get { return vel; }
            set { this.vel = value; }
        }
        public Vector2 POS
        {
            get { return pos; }
            set { this.pos = value; }
        }

        public Color Color
        {
            get;
            private set;
        }

        public Mc(double m, double r, Vector2 pos, Color c)
        {
            this.m = m;
            this.r = r;
            this.pos = pos;
            this.Color = Color;
        }

        public Mc(double m, double r, Vector2 pos, Color c, Vector2 v)
        {
            this.m = m;
            this.r = r;
            this.pos = pos;
            this.Color = c;
            this.VEL = v;
        }
    }

    public struct Vector2
    {
        private double x;
        private double y;

        public Vector2(double X, double Y)
        {
            this.x = X;
            this.y = Y;
        }
        public Vector2(double x1, double y1, double x2, double y2)
        {
            this.x = x2 - x1;
            this.y = y2 - y1;
        }

        public double X
        {
            get { return x; }
            set { x = value; }
        }
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public double Length
        {
            get { return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)); }
        }

        public static Vector2 operator +(Vector2 A, Vector2 B)
        {
            Vector2 res = new Vector2(A.x + B.x, A.y + B.y);
            return res;
        }
        public static Vector2 operator -(Vector2 A, Vector2 B)
        {
            Vector2 res = new Vector2(A.x - B.x, A.y - B.y);
            return res;
        }
        public static Vector2 operator *(Vector2 A, double k)
        {
            return new Vector2(A.X * k, A.Y * k);
        }
        public static double operator *(Vector2 A, Vector2 B)
        {
            return A.X * B.X + A.Y * B.Y;
        }
        public static double operator %(Vector2 A, Vector2 B)
        {
            double d = Math.Sqrt(Math.Pow(A.X - B.X, 2) + Math.Pow(A.Y - B.Y, 2));
            return d;
        }
    }
}
