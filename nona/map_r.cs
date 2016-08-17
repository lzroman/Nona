using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nona
{
    class map_r
    {
        int pmax;
        List<Form1.cnum_col> num_col = new List<Form1.cnum_col>();

        double[] a_i, b_i = new double[0];

        //public dynamic_compiler.Func func;
        private dynamic_compiler.Func func { get; set; }
        int numb_fun { get; set; }
        ProgressBar pbar_in { get; set; }

        int[,] t_count;
        int state = 0;

        Bitmap bm_graph;
        Graphics gr_box;
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

        public void col_set(List<Form1.cnum_col> num_col_in, int pmax_in)
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
        public Bitmap map(int N, double[] an, double[] al, double[] pars, double a1, double a2, int a_n, double b1, double b2, int b_n, int x_n, int x_s, double eps, double inf, int pp_a, int pp_p1, int pp_p2, int size_x,int size_y, bool t_ulast)
        {
            int i, j, k, draw_a, draw_b;
            double a_step, b_step, ad;
            bool col_sel = false;
            pbar = pbar_in;
            state = 0;

            double[] a0 = new double[numb_fun];
            for (i = 0; i < numb_fun; i++)
                a0[i] = an[i];

            t_count = new int[a_n, b_n];
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
                //funcs[i] = (dynamic_compiler.Func)func.Clone();
                taskArray[i] = Task.Factory.StartNew(() => calc(numb_fun, t_ulast, a0, pars, ac1, ac2, a_n1, a_n2, b1, b2, b_n, x_n, x_s, eps, inf, pp_a, pp_p1, pp_p2));
            };

            Task.WaitAll(taskArray);

            /*for (i = 0; i < a_n; i++)
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
                            };////
                        };

                        t_got = false;

                        al = an;
                    };

                    if (stop)
                    {
                        t_count[i, j] = -1;
                        stop = false;
                    }
                    else
                        t_count[i, j] = t_points.Count;
                    t_points.Clear();
                    if (t_count[i, j] > t_c_max)
                        t_c_max = t_count[i, j];

                    pars[pp_p2] += b_step;
                };

                pars[pp_p1] += a_step;
            };*/

            bm_graph = new Bitmap(size_x, size_y);
            Graphics gr_box = Graphics.FromImage(bm_graph);

            for (i = 0; i < a_n; i++)
            {
                for (j = 0; j < b_n; j++)
                {
                    col_sel = false;
                    for (k = 0; k < num_col.Count(); k++)
                    {
                        if (t_count[i, j] == num_col[k].num)
                        {
                            col_sel = true;
                            br_t = new SolidBrush(Color.FromArgb(num_col[k].r, num_col[k].g, num_col[k].b));
                            break;
                        };
                    };
                    if (!col_sel)
                    {
                        br_t = new SolidBrush(Color.FromArgb(num_col[1].r, num_col[1].g, num_col[1].b));
                    };
                    gr_box.FillRectangle(br_t, new Rectangle(Convert.ToInt32(j * 1.0 * size_x / b_n), Convert.ToInt32(size_y - (i + 1) * 1.0 * size_y / a_n), Convert.ToInt32(size_x * 1.0 / b_n + 1), Convert.ToInt32(size_y * 1.0 / a_n + 1)));
                };
            };

            //setka_start

            gr_box.DrawLine(p_black, new Point(0, size_y - 1), new Point(size_x - 1, size_y - 1));
            gr_box.DrawLine(p_black, new Point(0, size_y - 1), new Point(0, 0));
            gr_box.DrawString(Convert.ToString(b1), drawFont, br_black, new Point(1, size_y - 25));
            gr_box.DrawString(Convert.ToString(b2), drawFont, br_black, new Point(size_x - 20, size_y - 25));
            gr_box.DrawString(Convert.ToString(a1), drawFont, br_black, new Point(8, size_y - 15));
            gr_box.DrawString(Convert.ToString(a2), drawFont, br_black, new Point(8, 2));
            gr_box.DrawLine(p_black, new Point(size_x - 1, size_y - 1), new Point(size_x - 1, size_y - 7));
            gr_box.DrawLine(p_black, new Point(0, 0), new Point(6, 0));
            for (i = 1; i < 10; i++)
            {
                draw_a = Convert.ToInt32(size_x * i / 10);
                draw_b = Convert.ToInt32(size_y * i / 10);
                gr_box.DrawLine(p_black, new Point(draw_a, size_y - 1), new Point(draw_a, size_y - 7));
                gr_box.DrawString(Convert.ToString(b1 + (b2 - b1) * i / 10), drawFont, br_black, new Point(draw_b - 10, size_y - 25));
                gr_box.DrawLine(p_black, new Point(0, size_y - 1 - draw_b), new Point(6, size_y - 1 - draw_b));
                gr_box.DrawString(Convert.ToString(a1 + (a2 - a1) * i / 10), drawFont, br_black, new Point(8, size_y - 10 - draw_a));
            };

            //setka_end

            return bm_graph;
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
                        t_count[i, j] = -1;
                        stop = false;
                    }
                    else
                        t_count[i, j] = t_points.Count;
                    t_points.Clear();
                    if (t_count[i, j] > t_c_max)
                        t_c_max = t_count[i, j];

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
