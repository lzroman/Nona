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
        bool stop;

        dynamic_compiler.Func func;
        int numb_fun;

        graphics.settings.phas_port graph_set;
        graphics.phas_port graph;

        public phas_port(dynamic_compiler.Func func_in, int numb_fun_in)
        {
            func = func_in;
            numb_fun = numb_fun_in;
        }


        public Bitmap get_pp(double[] an, double[] al, double[] pars, double x1, double x2, double y1, double y2, int numb, int pars_n, int skip_n, int fp_a1, int fp_a2, int pb_w, int pb_h)
        {
            int i, j;

            graph_set.pboxsize.X = pb_w;
            graph_set.pboxsize.Y = pb_h;
            graph_set.ax_min = x1;
            graph_set.ax_max = x2;
            graph_set.ay_min = y1;
            graph_set.ay_max = y2;
            graph = new graphics.phas_port(graph_set);

            for (i = 0; i < numb; i++)
            {
                an = func(al, pars);

                for (j = 0; j < numb_fun; j++)
                {
                    if (Double.IsNaN(an[j]))
                    {
                        stop = true;
                        break;
                    }
                };

                if (stop)
                    break;

                if (i > skip_n && ((Math.Abs(an[fp_a1]) < x1 || Math.Abs(an[fp_a1]) < x2) && (Math.Abs(an[fp_a2]) < y1 || Math.Abs(an[fp_a2]) < y2)))
                    graph.Add(an[fp_a1], an[fp_a2]);

                al = an;
            };

            return graph.Draw();
        }
    }
}
