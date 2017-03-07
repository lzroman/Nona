using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nona.calc
{
    class map_r
    {
        int pmax;
        List<nona.Form1.cnum_col> num_col = new List<nona.Form1.cnum_col>();

        double[] a_i, b_i = new double[0];

        graphics.settings.map_r graph_set;
        graphics.map_r graph;

        dynamic_compiler.Func func;
        int numb_fun;
        ProgressBar pbar_in;

        int state = 0;

        ProgressBar pbar;

        Brush br_black = new SolidBrush(Color.Black);
        Brush br_t = new SolidBrush(Color.FromArgb(0, 0, 0));
        Font drawFont = new Font("Arial", 10);
        Pen p_black = new Pen(Color.Black, 1);

        public map_r()
        { }

        public map_r(dynamic_compiler.Func func_in, int numb_fun_in, ProgressBar pbar_in_in)
        {
            func = func_in;
            numb_fun = numb_fun_in;
            pbar_in = pbar_in_in;
        }

        public void col_set(List<nona.Form1.cnum_col> num_col_in, int pmax_in)
        {
            num_col = num_col_in;
            pmax = pmax_in;
        }

        /*
        dynamic_compiler.Func func
            ссылка на матфункцию
        int numb_fun
            число переменных
        double[] an
            текущие переменные
        double[] al
            старые переменные
        double[] pars
            параметры
        */
        public Bitmap map(int N, double[] an, double[] al, double[] pars, double a1, double a2, int a_n, double b1, double b2, int b_n, int x_n, int x_s, double eps, double inf, int pp_a, int pp_p1, int pp_p2, int pb_w, int pb_h, bool t_ulast)
        {
            int i, j;
            double a_step, b_step, ad;
            pbar = pbar_in;
            state = 0;

            graph_set.pboxsize.X = pb_w;
            graph_set.pboxsize.Y = pb_h;
            graph_set.px_min = a1;
            graph_set.px_max = a2;
            graph_set.py_min = b1;
            graph_set.py_max = b2;
            graph_set.px_num = a_n;
            graph_set.py_num = b_n;
            graph_set.num_col = num_col;
            graph = new graphics.map_r(graph_set);

            double[] a0 = new double[numb_fun];
            for (i = 0; i < numb_fun; i++)
                a0[i] = an[i];

            List<double> t_points = new List<double>();
            t_points.Clear();

            ad = a2 - a1;
            a_step = ad / a_n;
            b_step = (b2 - b1) / b_n;


            Task[] taskArray = new Task[N];
            dynamic_compiler.Func[] funcs = new dynamic_compiler.Func[N];

            for (i = 0; i < N; i++)
            {
                int a_n1 = Convert.ToInt32(a_n * i / N), a_n2 = Convert.ToInt32(a_n * (i + 1) / N);
                double ac1 = a1 + a_n1 * ad / a_n, ac2 = a1 + a_n2 * ad / a_n;
                taskArray[i] = Task.Factory.StartNew(() => calc(numb_fun, t_ulast, a0, pars, ac1, ac2, a_n1, a_n2, b1, b2, b_n, x_n, x_s, eps, inf, pp_a, pp_p1, pp_p2));
            };

            Task.WaitAll(taskArray);

            return graph.Draw();
        }

        private void calc(int numb_fun, bool t_ulast, double[] a0, double[] pars0, double a1, double a2, int a_n1, int a_n2, double b1, double b2, int b_n, int x_n, int x_s, double eps, double inf, int pp_a, int pp_p1, int pp_p2)
        {
            int i, j, k, l, t_c_max = 0;
            double a_step, b_step;
            bool t_got = false, stop = false;
            double[] an = new double[numb_fun];
            double[] al = new double[numb_fun];
            double[] pars = new double[pars0.Length];
            for (i = 0; i < pars0.Length; i++)
            {
                pars[i] = pars0[i];
            };

            List<double> t_points = new List<double>();
            t_points.Clear();

            a_step = (a2 - a1) / (a_n2 - a_n1);
            b_step = (b2 - b1) / b_n;

            pars[pp_p1] = a1 + a_step / 2;

            for (i = a_n1; i < a_n2; i++)
            {
                pars[pp_p2] = b1 + b_step / 2;
                for (j = 0; j < b_n; j++)
                {
                    if (!t_ulast)
                    {
                        for (k = 0; k < numb_fun; k++)
                            al[k] = a0[k];
                    };
                    for (k = 0; k < x_n; k++)
                    {
                        an = func(al, pars);

                        if (Math.Abs(an[pp_a]) >= inf)
                        {
                            stop = true;
                        }

                        for (l = 0; l < numb_fun; l++)
                        {
                            if (Double.IsNaN(an[l]))
                            {
                                stop = true;
                                break;
                            }
                        };

                        if (stop)
                            break;

                        if (k > x_s)
                        {
                            if (t_points.Count < pmax)
                            {
                                if (t_points.Count == 0)
                                {
                                    t_points.Add(an[pp_a]);
                                }
                                else
                                {
                                    for (l = 0; l < t_points.Count; l++)
                                    {
                                        if (Math.Abs(an[pp_a] - t_points[l]) <= eps)
                                        {
                                            t_got = true;
                                            break;
                                        };
                                    };
                                    if (t_got == false)
                                        t_points.Add(an[pp_a]);
                                };
                            }
                            /*else////отдельный поиск бесконечности
                            {
                                for (l = 0; l < numb_fun; l++)
                                {
                                    if (Double.IsNaN(an[l]))
                                    {
                                        stop = true;
                                        break;
                                    }
                                };
                                if (stop)
                                    break;
                            };*/
                        };

                        t_got = false;

                        al = an;
                    };

                    if (stop)
                    {
                        graph[i, j] = -1;
                        stop = false;
                    }
                    else
                        graph[i, j] = t_points.Count;
                    t_points.Clear();
                    if (graph[i, j] > t_c_max)
                        t_c_max = graph[i, j];

                    pars[pp_p2] += b_step;
                    state++;
                    setstate();
                };

                pars[pp_p1] += a_step;
            };
        }

        private void setstate()
        {
            if (pbar.InvokeRequired)
            {
                pbar.BeginInvoke(new MethodInvoker(delegate { pbar.Value = state - 1; }));
                return;
            };
        }

        public double lyap_elder(int numb_fun, double[] a0, double[] pars, double eps, int n, int norm)
        {
            double value = 0, vozm_value = 0;
            int i, j;
            Random v_r = new Random();
            double[] a = new double[numb_fun];
            for (i = 0; i < numb_fun; i++)
                a[i] = a0[i];
            double[] av = new double[numb_fun];
            double[] vozm = new double[numb_fun];


            for (i = 0; i < numb_fun; i++)//рандомим возмущение
            {
                vozm[i] = v_r.Next(1000);
                vozm_value += Math.Pow(vozm[i], 2);
            }

            vozm_value = Math.Sqrt(vozm_value);

            for (i = 0; i < numb_fun; i++)//нормируем возмущение и пихаем его
            {
                vozm[i] *= eps / vozm_value;
                av[i] = a[i] + vozm[i];
            }

            for (i = 0; i < n; i += norm)//мутим мутки
            {
                for (j = 0; j < norm; j++)
                {
                    a = func(a, pars);//норм аргументы
                    av = func(av, pars);//возмущённые аргументы
                }
                vozm_value = 0;
                for (j = 0; j < numb_fun; j++)//считаем разницу
                {
                    vozm[j] = av[j] - a[j];
                    vozm_value += Math.Pow(vozm[j], 2);
                }
                vozm_value = Math.Sqrt(vozm_value);
                value += Math.Log(vozm_value / eps);//кусок ляпунчика
                for (j = 0; j < numb_fun; j++)//новые возмущённые аргументы
                {
                    vozm[j] *= eps / vozm_value;
                    av[j] = a[j] + vozm[j];
                }
            }

            value /= n;

            return value;
        }
    }
}
