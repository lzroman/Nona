using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace nona.graphics
{
    class phas_port
    {
        settings.phas_port settings;

        List<double[]> points;

        Bitmap btmap;
        Graphics gr_box;
        Brush br_black = new SolidBrush(Color.Black);
        Pen p_black = new Pen(Color.Black, 1);
        Font drawFont = new Font("Arial", 10);

        public phas_port(settings.phas_port set_in)
        {
            settings = set_in;
            btmap = new Bitmap(settings.pboxsize.X, settings.pboxsize.Y);
            gr_box = Graphics.FromImage(btmap);
            points = new List<double[]>();
        }

        public void Add(double a1, double a2)
        {
            points.Add(new double[2] { a1, a2});
        }

        public Bitmap Draw()
        {
            int i, draw_x, draw_y;
            gr_box.Clear(Color.White);
            gr_box.DrawLine(p_black, new Point(0, settings.pboxsize.Y - 1), new Point(settings.pboxsize.X - 1, settings.pboxsize.Y - 1));
            gr_box.DrawLine(p_black, new Point(0, settings.pboxsize.Y - 1), new Point(0, 0));
            gr_box.DrawString(Convert.ToString(settings.ax_min), drawFont, br_black, new Point(1, settings.pboxsize.Y - 25));
            gr_box.DrawString(Convert.ToString(settings.ax_max), drawFont, br_black, new Point(settings.pboxsize.X - 20, settings.pboxsize.Y - 25));
            gr_box.DrawString(Convert.ToString(settings.ay_min), drawFont, br_black, new Point(8, settings.pboxsize.Y - 15));
            gr_box.DrawString(Convert.ToString(settings.ay_max), drawFont, br_black, new Point(8, 2));
            gr_box.DrawLine(p_black, new Point(settings.pboxsize.X - 1, settings.pboxsize.Y - 1), new Point(settings.pboxsize.X - 1, settings.pboxsize.Y - 7));
            gr_box.DrawLine(p_black, new Point(0, 0), new Point(6, 0));
            for (i = 1; i < 10; i++)
            {
                draw_x = Convert.ToInt32(settings.pboxsize.X * i / 10);
                draw_y = Convert.ToInt32(settings.pboxsize.Y * i / 10);
                gr_box.DrawLine(p_black, new Point(draw_y, settings.pboxsize.Y - 1), new Point(draw_y, settings.pboxsize.Y - 7));
                gr_box.DrawString(Convert.ToString(settings.ax_min + (settings.ax_max - settings.ax_min) * i / 10), drawFont, br_black, new Point(draw_x - 10, settings.pboxsize.Y - 25));
                gr_box.DrawLine(p_black, new Point(0, settings.pboxsize.Y - 1 - draw_x), new Point(6, settings.pboxsize.Y - 1 - draw_x));
                gr_box.DrawString(Convert.ToString(settings.ay_min + (settings.ay_max - settings.ay_min) * i / 10), drawFont, br_black, new Point(8, settings.pboxsize.Y - 10 - draw_y));
            };

            for (i = 0; i < points.Count; i++)
                gr_box.FillRectangle(br_black, new Rectangle(Convert.ToInt32(settings.pboxsize.X * (points[i][0] - settings.ax_min) / (settings.ax_max - settings.ax_min)), Convert.ToInt32(settings.pboxsize.Y * (settings.ay_max - points[i][1]) / (settings.ay_max - settings.ay_min)), 1, 1));

            return btmap;
        }
    }
}
