using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nona
{
    public partial class Form2_ph : Form
    {
        public Form2_ph()
        {
            InitializeComponent();
        }

        Bitmap bm_ph;

        public void set_bitmap(Bitmap bm_in)
        {
            int pb_w, pb_h;
            pb_w = pb_ph.Width;
            pb_h = pb_ph.Height;

            bm_ph = new Bitmap(pb_w, pb_h);
            bm_ph = bm_in.Clone(new Rectangle(0, 0, pb_w, pb_h), bm_in.PixelFormat);
            pb_ph.Refresh();
        }

        private void pb_ph_Paint(object sender, PaintEventArgs e)
        {
            int pb_h = pb_ph.Height, pb_w = pb_ph.Width;
            Graphics g = e.Graphics;
            /*if (b_graph)
            {
                g.DrawImage(bm_ph, 0, 0, pb_w, pb_h);
            };*/
            g.DrawImage(bm_ph, 0, 0, pb_w, pb_h);
        }
    }
}
