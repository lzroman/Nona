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

        graphics.settings.diagram graph_set;
        bool stop;
        public settings set;
        graphics.diagram graph;

        dynamic_compiler.Func func;
        int numb_fun;

        public diagram(dynamic_compiler.Func func_in, int numb_fun_in)
        {
            func = func_in;
            numb_fun = numb_fun_in;
            //сюда запихать настройки графики

        }

        public Bitmap diag(double[] an, double[] al, double[] pars, double a1, double a2, int t_a, double x1, double x2, int t_x, int skip_x, int dia_a, int dia_p, int pb_w, int pb_h)
        {
            graph_set.pboxsize.X = pb_w;
            graph_set.pboxsize.Y = pb_h;
            graph_set.a_min = x1;
            graph_set.a_max = x2;
            graph_set.p_min = a1;
            graph_set.p_max = a2;
            graph_set.p_num = t_a;

            graph = new graphics.diagram(graph_set);

            int i, j, k;
            double step_a = (a2 - a1) / t_a;
            
            double[] a0 = new double[numb_fun];
            for (i = 0; i < numb_fun; i++)
                a0[i] = an[i];

            pars[dia_p] = a1;

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
                        graph.Add(j, an[dia_a]);
                    al = an;
                };
                pars[dia_p] += step_a;
            };
            return graph.Draw();
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
