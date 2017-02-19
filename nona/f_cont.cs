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
        int f_n;

        public f_cont(dynamic_compiler.Func func_in, int f_n_in, double step_in)
        {
            func = func_in;
            f_n = f_n_in;
            step = step_in;
        }

        public dynamic_compiler.Func Func_Get()
        {
            func_c = new dynamic_compiler.Func(integ_step);
            return func_c;
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
