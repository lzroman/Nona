using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace nona.graphics
{
    class settings
    {

        public struct phas_port
        {
            public Point pboxsize;//размер рисунка
            public double ax_min, ax_max, ay_min, ay_max;//границы
        }

        public struct diagram
        {
            public Point pboxsize;//размер рисунка
            public double a_min, a_max, p_min, p_max;//границы аргументов
            public int p_num;//дробление параметра
        }

        public struct map_r
        {
            public Point pboxsize;//размер рисунка
            public double px_min, px_max, py_min, py_max;//границы
            public int px_num, py_num;//дробление
            public List<nona.Form1.cnum_col> num_col;//цвета
        }
    }
}
