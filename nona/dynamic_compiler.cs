using System;
using System.Windows.Forms;
using System.CodeDom.Compiler;

public class dynamic_compiler
{
    DataGridView dgv_fun;
    DataGridView dgv_par;
    DataGridView dgv_usl;

    public delegate double[] Func(double[] a, double[] p);
    public Func func;
    public delegate double Plane(double[] a, double[] p);
    public Plane plane;
    string equation;

    public dynamic_compiler(DataGridView in_dgv_fun, DataGridView in_dgv_par, DataGridView in_dgv_usl)
	{
        dgv_fun = in_dgv_fun;
        dgv_par = in_dgv_par;
        dgv_usl = in_dgv_usl;
    }

    public dynamic_compiler(DataGridView in_dgv_fun, DataGridView in_dgv_par, string equation_in)
    {
        dgv_fun = in_dgv_fun;
        dgv_par = in_dgv_par;
        equation = equation_in;
    }
    
    public Func compile()
    {
        source();
        return func;
    }

    public Plane compile_equation()
    {
        source_equation();
        return plane;
    }

    public Func func_equation()
    {
        source_equation();
        return func;
    }

    private void source_equation()
    {
        int i, j, char_numb;
        int numb_fun = dgv_fun.Rows.Count - 1;
        string fun_s = fun_s = equation;
        string symb = "+-*/;,)<>=!%";

        for (i = 0; i < numb_fun; i++)
        {
            char_numb = fun_s.IndexOf(Convert.ToString(dgv_fun[0, i].Value));
            while (char_numb != -1)
            {
                if (char_numb + Convert.ToString(dgv_fun[0, i].Value).Length == fun_s.Length)
                {
                    fun_s = fun_s.Remove(char_numb, Convert.ToString(dgv_fun[0, i].Value).Length);
                    fun_s = fun_s.Insert(char_numb, "a[" + Convert.ToString(i) + "]");
                }
                else
                {
                    for (j = 0; j < symb.Length; j++)
                    {

                        if (fun_s[char_numb + Convert.ToString(dgv_fun[0, i].Value).Length].Equals(symb[j]))
                        {
                            fun_s = fun_s.Remove(char_numb, Convert.ToString(dgv_fun[0, i].Value).Length);
                            fun_s = fun_s.Insert(char_numb, "a[" + Convert.ToString(i) + "]");
                        };

                    };
                };
                char_numb++;
                char_numb = fun_s.IndexOf(Convert.ToString(dgv_fun[0, i].Value), char_numb);
            };
        };

        for (i = 0; i < dgv_par.Rows.Count - 1; i++)
        {
            char_numb = fun_s.IndexOf(Convert.ToString(dgv_par[0, i].Value));
            while (char_numb != -1)
            {
                for (j = 0; j < symb.Length; j++)
                {
                    if (fun_s[char_numb + Convert.ToString(dgv_par[0, i].Value).Length].Equals(symb[j]))
                    {
                        fun_s = fun_s.Remove(char_numb, Convert.ToString(dgv_par[0, i].Value).Length);
                        fun_s = fun_s.Insert(char_numb, "p[" + Convert.ToString(i) + "]");
                    };
                };
                char_numb++;
                char_numb = fun_s.IndexOf(Convert.ToString(dgv_par[0, i].Value), char_numb);
            };
        };
        string s = "using System; class C { public static double f(double[] a, double[] p) { return " + fun_s + ";}}";
        plane = (Plane)Delegate.CreateDelegate(typeof(Plane), compile(s).CompiledAssembly.CreateInstance("C").GetType().GetMethod("f"));
    }

    private void source()
    {
        int i, j, char_numb;
        int numb_fun = dgv_fun.Rows.Count - 1;
        string fun_s = "";
        string symb = "+-*/;,)<>=!%";
        for (i = 0; i < numb_fun; i++)
        {
            if ((bool)dgv_fun[2, i].Value == true)
            {
                //fun_s += "rt[" + Convert.ToString(i) + "] = a[" + Convert.ToString(i) + "]; ";
                j = 0;
                while (!dgv_usl[0, j].Value.Equals(dgv_fun[0, i].Value))
                    j++;
                while (dgv_usl[0, j].Value.Equals(dgv_fun[0, i].Value))
                {
                    fun_s += "if (" + Convert.ToString(dgv_usl[1, j].Value) + ") " + "rt[" + Convert.ToString(i) + "] = " + Convert.ToString(dgv_usl[2, j].Value) + "; ";
                    j++;
                    if (j == dgv_usl.Rows.Count - 1)
                        break;
                };
            }
            else
                fun_s += "rt[" + Convert.ToString(i) + "] = " + Convert.ToString(dgv_fun[1, i].Value) + "; ";
        };

        for (i = 0; i < numb_fun; i++)
        {
            char_numb = fun_s.IndexOf(Convert.ToString(dgv_fun[0, i].Value));
            while (char_numb != -1)
            {
                for (j = 0; j < symb.Length; j++)
                {
                    if (fun_s[char_numb + Convert.ToString(dgv_fun[0, i].Value).Length].Equals(symb[j]))
                    {
                        fun_s = fun_s.Remove(char_numb, Convert.ToString(dgv_fun[0, i].Value).Length);
                        fun_s = fun_s.Insert(char_numb, "a[" + Convert.ToString(i) + "]");
                    };
                };
                char_numb++;
                char_numb = fun_s.IndexOf(Convert.ToString(dgv_fun[0, i].Value), char_numb);
            };
        };

        for (i = 0; i < dgv_par.Rows.Count - 1; i++)
        {
            char_numb = fun_s.IndexOf(Convert.ToString(dgv_par[0, i].Value));
            while (char_numb != -1)
            {
                for (j = 0; j < symb.Length; j++)
                {
                    if (fun_s[char_numb + Convert.ToString(dgv_par[0, i].Value).Length].Equals(symb[j]))
                    {
                        fun_s = fun_s.Remove(char_numb, Convert.ToString(dgv_par[0, i].Value).Length);
                        fun_s = fun_s.Insert(char_numb, "p[" + Convert.ToString(i) + "]");
                    };
                };
                char_numb++;
                char_numb = fun_s.IndexOf(Convert.ToString(dgv_par[0, i].Value), char_numb);
            };
        };

        for (i = 0; i < numb_fun; i++)
        {
            if (dgv_fun[4, i].Value != null)
            {
                char_numb = fun_s.IndexOf(Convert.ToString(dgv_fun[4, i].Value));
                while (char_numb != -1)
                {
                    for (j = 0; j < symb.Length; j++)
                    {
                        if (fun_s[char_numb + Convert.ToString(dgv_fun[4, i].Value).Length].Equals(symb[j]))
                        {
                            fun_s = fun_s.Remove(char_numb, Convert.ToString(dgv_fun[4, i].Value).Length);
                            fun_s = fun_s.Insert(char_numb, "rt[" + Convert.ToString(i) + "]");
                        };
                    };
                    char_numb++;
                    char_numb = fun_s.IndexOf(Convert.ToString(dgv_fun[0, i].Value), char_numb);
                };
            };
        };

        string s = "using System; class C { public static double[] f(double[] a, double[] p) { double[] rt = new double[" + Convert.ToString(numb_fun) + "]; " + fun_s + "return rt; } }";
        func = (Func)Delegate.CreateDelegate(typeof(Func), compile(s).CompiledAssembly.CreateInstance("C").GetType().GetMethod("f"));
    }

    static CompilerResults compile(string source)
    {
        CodeDomProvider csharp = Microsoft.CSharp.CSharpCodeProvider.CreateProvider("CSharp");
        CompilerParameters cp = new CompilerParameters();
        cp.GenerateExecutable = false;
        cp.GenerateInMemory = true;
        cp.IncludeDebugInformation = false;
        cp.ReferencedAssemblies.Add("System.dll");
        CompilerResults cr = csharp.CompileAssemblyFromSource(cp, source.Split('\n'));
        if (cr.Errors != null && cr.Errors.Count > 0)
        {
            for (int i = 0; i < cr.Errors.Count; i++)
                MessageBox.Show(Convert.ToString(cr.Errors[i].ErrorText));
        }
        return cr;
    }
}
