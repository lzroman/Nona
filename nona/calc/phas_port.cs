using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace nona.calc
{
    class phas_port
    {
        Bitmap bm_graph;
        Graphics gr_box;
        bool stop;

        private dynamic_compiler.Func func { get; set; }
        int numb_fun { get; set; }

        Brush br_black = new SolidBrush(Color.Black);
        Pen p_black = new Pen(Color.Black, 1);
        Font drawFont = new Font("Arial", 10);

        public phas_port()
        { }

        public phas_port(dynamic_compiler.Func func_in, int numb_fun_in)
        {
            func = func_in;
            numb_fun = numb_fun_in;
        }


        public Bitmap get_pp(double[] an, double[] al, double[] pars, double x1, double x2, double y1, double y2, int numb, int pars_n, int skip_n, int fp_a1, int fp_a2, int pb_w, int pb_h)
        {
            int i, j;
            int draw_x, draw_y;

            bm_graph = new Bitmap(pb_w, pb_h);
            gr_box = Graphics.FromImage(bm_graph);

            gr_box.Clear(Color.White);
            gr_box.DrawLine(p_black, new Point(0, pb_h - 1), new Point(pb_w - 1, pb_h - 1));
            gr_box.DrawLine(p_black, new Point(0, pb_h - 1), new Point(0, 0));
            gr_box.DrawString(Convert.ToString(x1), drawFont, br_black, new Point(1, pb_h - 25));
            gr_box.DrawString(Convert.ToString(x2), drawFont, br_black, new Point(pb_w - 20, pb_h - 25));
            gr_box.DrawString(Convert.ToString(y1), drawFont, br_black, new Point(8, pb_h - 15));
            gr_box.DrawString(Convert.ToString(y2), drawFont, br_black, new Point(8, 2));
            gr_box.DrawLine(p_black, new Point(pb_w - 1, pb_h - 1), new Point(pb_w - 1, pb_h - 7));
            gr_box.DrawLine(p_black, new Point(0, 0), new Point(6, 0));
            for (i = 1; i < 10; i++)
            {
                draw_x = Convert.ToInt32(pb_w * i / 10);
                draw_y = Convert.ToInt32(pb_h * i / 10);
                gr_box.DrawLine(p_black, new Point(draw_y, pb_h - 1), new Point(draw_y, pb_h - 7));
                gr_box.DrawString(Convert.ToString(x1 + (x2 - x1) * i / 10), drawFont, br_black, new Point(draw_x - 10, pb_h - 25));
                gr_box.DrawLine(p_black, new Point(0, pb_h - 1 - draw_x), new Point(6, pb_h - 1 - draw_x));
                gr_box.DrawString(Convert.ToString(y1 + (y2 - y1) * i / 10), drawFont, br_black, new Point(8, pb_h - 10 - draw_y));
            };

            //setka_end

            for (i = 0; i < numb; i++)
            {
                an = func(al, pars);

                for (j = 0; j < numb_fun; j++)
                {
                    if (Double.IsNaN(an[j]))
                        stop = true;
                };

                if (stop)
                    break;

                if (i > skip_n && ((Math.Abs(an[fp_a1]) < x1 || Math.Abs(an[fp_a1]) < x2) && (Math.Abs(an[fp_a2]) < y1 || Math.Abs(an[fp_a2]) < y2)))
                    gr_box.FillRectangle(br_black, new Rectangle(Convert.ToInt32(pb_w * (an[fp_a1] - x1) / (x2 - x1)), Convert.ToInt32(pb_h * (y2 - an[fp_a2]) / (y2 - y1)), 1, 1));

                al = an;
            };

            return bm_graph;
        }
    }
}
