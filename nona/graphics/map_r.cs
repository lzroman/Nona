using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace nona.graphics
{
    class map_r
    {
        settings.map_r settings;

        int[,] points;

        Bitmap btmap;
        Graphics gr_box;

        Brush br_black = new SolidBrush(Color.Black);
        Brush br_t = new SolidBrush(Color.FromArgb(0, 0, 0));
        Font drawFont = new Font("Arial", 10);
        Pen p_black = new Pen(Color.Black, 1);

        public map_r(settings.map_r set_in)
        {
            settings = set_in;
            btmap = new Bitmap(settings.pboxsize.X, settings.pboxsize.Y);
            gr_box = Graphics.FromImage(btmap);
            points = new int[settings.px_num, settings.py_num];
        }

        public int this[int ind_x, int ind_y]
        {
            get
            {
                return points[ind_x, ind_y];
            }
            set
            {
                points[ind_x, ind_y] = value;
            }
        }

        public Bitmap Draw()
        {
            int i, j, k, draw_a, draw_b;
            bool col_sel = false;

            for (i = 0; i < settings.px_num; i++)
            {
                for (j = 0; j < settings.py_num; j++)
                {
                    col_sel = false;
                    for (k = 0; k < settings.num_col.Count(); k++)
                    {
                        if (points[i, j] == settings.num_col[k].num)
                        {
                            col_sel = true;
                            br_t = new SolidBrush(Color.FromArgb(settings.num_col[k].r, settings.num_col[k].g, settings.num_col[k].b));
                            break;
                        };
                    };
                    if (!col_sel)
                    {
                        br_t = new SolidBrush(Color.FromArgb(settings.num_col[1].r, settings.num_col[1].g, settings.num_col[1].b));
                    };
                    gr_box.FillRectangle(br_t, new Rectangle(Convert.ToInt32(i * 1.0 * settings.pboxsize.X / settings.px_num), Convert.ToInt32(settings.pboxsize.Y - (j + 1) * 1.0 * settings.pboxsize.Y / settings.px_num), Convert.ToInt32(settings.pboxsize.X * 1.0 / settings.py_num + 1), Convert.ToInt32(settings.pboxsize.Y * 1.0 / settings.px_num + 1)));
                };
            };

            gr_box.DrawLine(p_black, new Point(0, settings.pboxsize.Y - 1), new Point(settings.pboxsize.X - 1, settings.pboxsize.Y - 1));
            gr_box.DrawLine(p_black, new Point(0, settings.pboxsize.Y - 1), new Point(0, 0));
            gr_box.DrawString(Convert.ToString(settings.px_min), drawFont, br_black, new Point(1, settings.pboxsize.Y - 25));
            gr_box.DrawString(Convert.ToString(settings.px_max), drawFont, br_black, new Point(settings.pboxsize.X - 20, settings.pboxsize.Y - 25));
            gr_box.DrawString(Convert.ToString(settings.py_min), drawFont, br_black, new Point(8, settings.pboxsize.Y - 15));
            gr_box.DrawString(Convert.ToString(settings.py_max), drawFont, br_black, new Point(8, 2));
            gr_box.DrawLine(p_black, new Point(settings.pboxsize.X - 1, settings.pboxsize.Y - 1), new Point(settings.pboxsize.X - 1, settings.pboxsize.Y - 7));
            gr_box.DrawLine(p_black, new Point(0, 0), new Point(6, 0));
            for (i = 1; i < 10; i++)
            {
                draw_a = Convert.ToInt32(settings.pboxsize.X * i / 10);
                draw_b = Convert.ToInt32(settings.pboxsize.Y * i / 10);
                gr_box.DrawLine(p_black, new Point(draw_a, settings.pboxsize.Y - 1), new Point(draw_a, settings.pboxsize.Y - 7));
                gr_box.DrawString(Convert.ToString(settings.px_min + (settings.px_max - settings.px_min) * i / 10), drawFont, br_black, new Point(draw_b - 10, settings.pboxsize.Y - 25));
                gr_box.DrawLine(p_black, new Point(0, settings.pboxsize.Y - 1 - draw_b), new Point(6, settings.pboxsize.Y - 1 - draw_b));
                gr_box.DrawString(Convert.ToString(settings.py_min + (settings.py_max - settings.py_min) * i / 10), drawFont, br_black, new Point(8, settings.pboxsize.Y - 10 - draw_a));
            };

            return btmap;
        }
    }
}
