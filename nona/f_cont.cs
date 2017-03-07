using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nona
{
    class f_cont
    {
        dynamic_compiler.Func func, func_c;

        public double step;
        int f_n, sw, cond;

        public f_cont(dynamic_compiler.Func func_in, int f_n_in, double step_in, int sw_in, int cond_in)
        {
            func = func_in;
            f_n = f_n_in;
            step = step_in;
            sw = sw_in;
            cond = cond_in;
        }

        public dynamic_compiler.Func Func_Get()
        {
            switch (cond)
                {
                case 0:
                    func_c = new dynamic_compiler.Func(integ_step);
                    break;
                default:
                    func_c = new dynamic_compiler.Func(integ_extrem);
                    break;
            }
            
            return func_c;
        }

        double[] integ_extrem(double[] a, double[] p)
        {
            int i;
            double[] rt_ll = new double[f_n];
            double[] rt_l = new double[f_n];
            double[] rt = new double[f_n];
            rt_ll = a;
            rt_l = integ_step(rt_ll, p);
            rt = integ_step(rt_l, p);
            switch (cond)
            {
                case 1://смена знака производной
                    while (!(rt_l[sw] * rt[sw] < 0))
                    {
                        rt_l = rt;
                        rt = integ_step(rt_l, p);
                    }
                    break;
                case 2://экстремум
                    while (Math.Sign(rt_l[sw] - rt_ll[sw]) == Math.Sign(rt[sw] - rt_l[sw]))
                    {
                        rt_ll = rt_l;
                        rt_l = rt;
                        rt = integ_step(rt_l, p);
                    }
                    break;
                case 3://максимум
                    while (!(rt_ll[sw] < rt_l[sw] && rt_l[sw] > rt[sw]))
                    {
                        rt_ll = rt_l;
                        rt_l = rt;
                        rt = integ_step(rt_l, p);
                    }
                    break;
                case 4://минимум
                    while (!(rt_ll[sw] > rt_l[sw] && rt_l[sw] < rt[sw]))
                    {
                        rt_ll = rt_l;
                        rt_l = rt;
                        rt = integ_step(rt_l, p);
                    }
                    break;

            }
            return rt;
        }

        double[] integ_step(double[] a, double[] p)
        {
            int i, j;
            double[] rt = new double[f_n];
            double[] a_t = new double[f_n];//for every R-K coef
            double[] rkc = new double[f_n * 4];//R-K coefs
            a_t = func(a, p);
            for (i = 0; i < f_n; i++)
            {
                rkc[i] = a_t[i] * step;//1
                a_t[i] = a[i] + rkc[i] / 2;
            };
            a_t = func(a_t, p);
            for (i = 0; i < f_n; i++)
            {
                rkc[i + f_n] = a_t[i] * step;//2
                a_t[i] = a[i] + rkc[i + f_n] / 2;
            };
            a_t = func(a_t, p);
            for (i = 0; i < f_n; i++)
            {
                rkc[i + f_n * 2] = a_t[i] * step;//3
                a_t[i] = a[i] + rkc[i + f_n * 2];
            };
            a_t = func(a_t, p);
            for (i = 0; i < f_n; i++)
                rkc[i + f_n * 3] = a_t[i] * step;//4

            for (i = 0; i < f_n; i++)
            {
                rt[i] = rkc[i];
                rt[i] += rkc[i + f_n] * 2.0;
                rt[i] += rkc[i + f_n * 2] * 2.0;
                rt[i] += rkc[i + f_n * 3];
                rt[i] /= 6.0;
                rt[i] += a[i];
            };

            return rt;
        }
    }
}
