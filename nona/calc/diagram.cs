using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace nona.calc
{
    class diagram
    {
        public struct settings
        {
            public double[] an, al, pars;
            public double a1, a2, x1, x2;
            public int t_a, t_x, skip_x, diag_a, diag_p;
        }

        Bitmap bm_graph;
        Graphics gr_box;
        bool stop;
        public settings set;

        private dynamic_compiler.Func func { get; set; }
        int numb_fun { get; set; }

        Brush br_black = new SolidBrush(Color.Black);
        Pen p_black = new Pen(Color.Black, 1);
        Font drawFont = new Font("Arial", 10);

        public diagram()
        { }

        public diagram(dynamic_compiler.Func func_in, int numb_fun_in)
        {
            func = func_in;
            numb_fun = numb_fun_in;
        }

        public Bitmap diag(double[] an, double[] al, double[] pars, double a1, double a2, int t_a, double x1, double x2, int t_x, int skip_x, int dia_a, int dia_p, int pb_w, int pb_h)
        {
            int i, j, k, draw_a, draw_x;
            double step_a = (a2 - a1) / t_a;
            
            double[] a0 = new double[numb_fun];
            for (i = 0; i < numb_fun; i++)
                a0[i] = an[i];

            pars[dia_p] = a1;

            bm_graph = new Bitmap(pb_w, pb_h);
            gr_box = Graphics.FromImage(bm_graph);

            gr_box.Clear(Color.White);
            gr_box.DrawLine(p_black, new Point(0, pb_h - 1), new Point(pb_w - 1, pb_h - 1));
            gr_box.DrawLine(p_black, new Point(0, pb_h - 1), new Point(0, 0));
            gr_box.DrawString(Convert.ToString(a1), drawFont, br_black, new Point(1, pb_h - 25));
            gr_box.DrawString(Convert.ToString(a2), drawFont, br_black, new Point(pb_w - 20, pb_h - 25));
            gr_box.DrawString(Convert.ToString(x1), drawFont, br_black, new Point(8, pb_h - 15));
            gr_box.DrawString(Convert.ToString(x2), drawFont, br_black, new Point(8, 2));
            gr_box.DrawLine(p_black, new Point(pb_w - 1, pb_h - 1), new Point(pb_w - 1, pb_h - 7));
            gr_box.DrawLine(p_black, new Point(0, 0), new Point(6, 0));
            for (i = 1; i < 10; i++)
            {
                draw_a = Convert.ToInt32(pb_w * i / 10);
                draw_x = Convert.ToInt32(pb_h * i / 10);
                gr_box.DrawLine(p_black, new Point(draw_a, pb_h - 1), new Point(draw_a, pb_h - 7));
                gr_box.DrawString(Convert.ToString(a1 + (a2 - a1) * i / 10), drawFont, br_black, new Point(draw_a - 10, pb_h - 25));
                gr_box.DrawLine(p_black, new Point(0, pb_h - 1 - draw_x), new Point(6, pb_h - 1 - draw_x));
                gr_box.DrawString(Convert.ToString(x1 + (x2 - x1) * i / 10), drawFont, br_black, new Point(8, pb_h - 10 - draw_x));
            };

            for (j = 0; j < t_a; j++)
            {
                for (i = 0; i < numb_fun; i++)
                    al[i] = a0[i];
                stop = false;

                for (i = 0; i < t_x; i++)
                {
                    an = func(al, pars);

                    for (k = 0; k < numb_fun; k++)
                    {
                        if (Double.IsNaN(an[k]))
                            stop = true;
                    };

                    if (stop)
                        break;

                    if (i > skip_x && (Math.Abs(an[dia_a]) < x1 || Math.Abs(an[dia_a]) < x2))
                        gr_box.FillRectangle(br_black, new Rectangle(Convert.ToInt32((pars[dia_p] - a1) * pb_w / (a2 - a1)), Convert.ToInt32(pb_h - 1 - (an[dia_a] - x1) * pb_h / (x2 - x1)), 1, 1));

                    al = an;
                };
                pars[dia_p] += step_a;
            };
            return bm_graph;
        }

        public void diag_file(dynamic_compiler.Func func, int numb_fun, double[] an, double[] al, double[] pars, double a1, double a2, int t_a, int t_x, int dia_a, int dia_p, System.IO.StreamWriter file_work)
        {
            int i, j;
            double step_a;
            double[] a0 = new double[numb_fun];

            for (i = 0; i < numb_fun; i++)
                a0[i] = an[i];

            step_a = (a2 - a1) / t_a;
            pars[dia_p] = a1;

            for (j = 0; j < t_a; j++)
            {
                for (i = 0; i < numb_fun; i++)
                    al[i] = a0[i];
                for (i = 0; i < t_x; i++)
                {
                    an = func(al, pars);

                    file_work.WriteLine(pars[dia_p] + "\t" + an[dia_a]);

                    al = an;
                };
                pars[dia_p] += step_a;
            };
        }

        }
}
