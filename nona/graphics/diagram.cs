using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace nona.graphics
{
    class diagram
    {
        //внутриклассовое говно
        Bitmap btmap;
        Graphics gr_box;
        List<double>[] points;
        settings.diagram settings;
        Brush br_black = new SolidBrush(Color.Black);
        Pen p_black = new Pen(Color.Black, 1);
        Font drawFont = new Font("Arial", 10);

        public diagram(settings.diagram set_in)//конструктору - настройки
        {
            settings = set_in;
            btmap = new Bitmap(settings.pboxsize.X, settings.pboxsize.Y);
            gr_box = Graphics.FromImage(btmap);
            points = new List<double>[settings.p_num];
            for (int i = 0; i < settings.p_num; i++)
                points[i] = new List<double>();
        }

        public void Add(int par, double val)//добавление значения
        {
            points[par].Add(val);
        }

        public Bitmap Draw()
        {
            int i, j, draw_a, draw_x;
            gr_box.Clear(Color.White);
            gr_box.DrawLine(p_black, new Point(0, settings.pboxsize.Y - 1), new Point(settings.pboxsize.X - 1, settings.pboxsize.Y - 1));
            gr_box.DrawLine(p_black, new Point(0, settings.pboxsize.Y - 1), new Point(0, 0));
            gr_box.DrawString(Convert.ToString(settings.p_min), drawFont, br_black, new Point(1, settings.pboxsize.Y - 25));
            gr_box.DrawString(Convert.ToString(settings.p_max), drawFont, br_black, new Point(settings.pboxsize.X - 20, settings.pboxsize.Y - 25));
            gr_box.DrawString(Convert.ToString(settings.a_min), drawFont, br_black, new Point(8, settings.pboxsize.Y - 15));
            gr_box.DrawString(Convert.ToString(settings.a_max), drawFont, br_black, new Point(8, 2));
            gr_box.DrawLine(p_black, new Point(settings.pboxsize.X - 1, settings.pboxsize.Y - 1), new Point(settings.pboxsize.X - 1, settings.pboxsize.Y - 7));
            gr_box.DrawLine(p_black, new Point(0, 0), new Point(6, 0));
            for (i = 1; i < 10; i++)
            {
                draw_a = Convert.ToInt32(settings.pboxsize.X * i / 10);
                draw_x = Convert.ToInt32(settings.pboxsize.Y * i / 10);
                gr_box.DrawLine(p_black, new Point(draw_a, settings.pboxsize.Y - 1), new Point(draw_a, settings.pboxsize.Y - 7));
                gr_box.DrawString(Convert.ToString(settings.p_min + (settings.p_max - settings.p_min) * i / 10), drawFont, br_black, new Point(draw_a - 10, settings.pboxsize.Y - 25));
                gr_box.DrawLine(p_black, new Point(0, settings.pboxsize.Y - 1 - draw_x), new Point(6, settings.pboxsize.Y - 1 - draw_x));
                gr_box.DrawString(Convert.ToString(settings.a_min + (settings.a_max - settings.a_min) * i / 10), drawFont, br_black, new Point(8, settings.pboxsize.Y - 10 - draw_x));
            };

            for (i = 0; i < settings.p_num; i++)
            {
                for (j = 0; j < points[i].Count; j++)
                    gr_box.FillRectangle(br_black, new Rectangle(Convert.ToInt32(i * settings.pboxsize.X / settings.p_num), Convert.ToInt32(settings.pboxsize.Y - 1 - (points[i][j] - settings.a_min) * settings.pboxsize.Y / (settings.a_max - settings.a_min)), 1, 1));
            };

            return btmap;
        }
    }
}
