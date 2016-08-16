using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace nona
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        dynamic_compiler compiler;
        dynamic_compiler.Func func;
        diagram diag;
        fileworker filework = new fileworker();

        phas_port ph_port;

        map_r map = new map_r();
        bool map_ready = false;

        Task map_drawer;

        public class cnum_col
        {
            public int num { get; set; }
            public int r { get; set; }
            public int g { get; set; }
            public int b { get; set; }
        }

        List<cnum_col> num_col = new List<cnum_col>();

        double zoning = 0;
        double[] xy = new double[2];
        double[] an = new double[0];
        double[] al = new double[0];
        double[] pars = new double[0];
        int pmax = 9, wtd, numb_fun, fp_a1, fp_a2, dia_a, dia_p, pp_a, pp_p1, pp_p2;
        String file_path;

        Bitmap bm_graph, bm_form2;
        bool b_graph = false, col_sel = false, ph_form2 = false, stop = false;

        Point c1pos, c2pos;

        Color dgv_col;

        Brush br_white = new SolidBrush(Color.White);
        Brush br_black = new SolidBrush(Color.Black);

        Pen p_red = new Pen(Color.Red, 1);
        Pen p_black = new Pen(Color.Black, 1);

        Font drawFont = new Font("Arial", 10);

        Form2_ph form2;

        private void draw()
        {
            int pb_w, pb_h, skip_n, numb;
            double x1, x2, y1, y2;

            stop = false;

            x1 = Convert.ToDouble(tb_x1.Text);
            x2 = Convert.ToDouble(tb_x2.Text);
            y1 = Convert.ToDouble(tb_y1.Text);
            y2 = Convert.ToDouble(tb_y2.Text);
            skip_n = Convert.ToInt32(tb_fp_skip.Text);
            numb = Convert.ToInt32(tb_numb.Text);

            pb_w = pictureBox1.Width;
            pb_h = pictureBox1.Height;

            if (!ph_form2)
            {
                bm_graph = ph_port.get_pp(an, al, pars, x1, x2, y1, y2, numb, dgv_par.Rows.Count - 1, skip_n, fp_a1, fp_a2, pb_w, pb_h);
            }
            else
            {
                bm_form2 = ph_port.get_pp(an, al, pars, x1, x2, y1, y2, numb, dgv_par.Rows.Count - 1, skip_n, fp_a1, fp_a2, pb_w, pb_h);
            };

            if (!ph_form2)
            {
                b_graph = true;
                pictureBox1.Refresh();
            }
        }

        private void bsave_Click(object sender, EventArgs e)
        {
            get_all();
            draw();
            wtd = 1;
            b_zone.Enabled = true;
            b_graph = true;
        }

        private void b_dia_Click(object sender, EventArgs e)
        {
            int t_x, skip_x, t_a, pb_w, pb_h, numb;
            double a1, a2, x1, x2;

            numb = Convert.ToInt32(tb_numb.Text);

            b_dia.Enabled = false;
            b_dia.Text = "Отрисовка";
            this.Refresh();
            wtd = 2;

            get_all();
            a1 = Convert.ToDouble(tb_dia_a1.Text);
            a2 = Convert.ToDouble(tb_dia_a2.Text);
            t_a = Convert.ToInt32(tb_dia_a.Text);
            x1 = Convert.ToDouble(tb_dia_x1.Text);
            x2 = Convert.ToDouble(tb_dia_x2.Text);
            t_x = Convert.ToInt32(tb_dia_x.Text);
            skip_x = Convert.ToInt32(tb_dia_n.Text);

            pb_w = pictureBox1.Width;
            pb_h = pictureBox1.Height;

            b_graph = false;

            bm_graph = diag.diag(an, al, pars, a1, a2, t_a, x1, x2, t_x, skip_x, dia_a, dia_p, pb_w, pb_h);

            b_dia.Enabled = true;
            b_dia.Text = "Диаграмма";
            b_zone.Enabled = true;

            b_graph = true;
            pictureBox1.Refresh();
        }

        private void b_file_path_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                file_path = saveFileDialog1.FileName;
                b_file_write.Enabled = true;
            }
            else
            {
                b_file_write.Enabled = false;
            };
        }

        private void b_file_write_Click(object sender, EventArgs e)
        {
            int t_x,t_a;
            double a1, a2;

            b_file_write.Enabled = false;
            b_file_write.Text = "Идёт запись";
            b_file_path.Enabled = false;
            b_file_path.Text = "Идёт запись";
            this.Refresh();

            get_all();
            a1 = Convert.ToDouble(tb_dia_a1.Text);
            a2 = Convert.ToDouble(tb_dia_a2.Text);
            t_a = Convert.ToInt32(tb_dia_a.Text);
            t_x = Convert.ToInt32(tb_dia_x.Text);

            System.IO.StreamWriter file_work = new System.IO.StreamWriter(file_path);

            diag.diag_file(func, numb_fun, an, al, pars, a1, a2, t_a, t_x, dia_a, dia_p, file_work);

            b_file_write.Enabled = true;
            b_file_write.Text = "Запись в файл";
            b_file_path.Enabled = true;
            b_file_path.Text = "Выбрать файл";
        }

        private void b_zone_Click(object sender, EventArgs e)
        {
            zoning = 1;
            b_zone.Enabled = false;
            b_zone.Text = "Поставьте первую точку";
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (zoning == 1)
            {
                int pb_w, pb_h;

                pb_w = pictureBox1.Width;
                pb_h = pictureBox1.Height;

                c1pos = this.PointToClient(Cursor.Position);
                c1pos.X -= pictureBox1.Location.X;
                c1pos.Y -= pictureBox1.Location.Y;
                b_graph = false;
                Graphics gr_box = Graphics.FromImage(bm_graph);
                gr_box.DrawLine(p_black, new Point(0, c1pos.Y), new Point(pb_w - 1, c1pos.Y));
                gr_box.DrawLine(p_black, new Point(c1pos.X, pb_h - 1), new Point(c1pos.X, 0));

                b_zone.Text = "Поставьте вторую точку";

                zoning = 2;

                b_graph = true;
                pictureBox1.Refresh();
            }
            else
            {
                if (zoning == 2)
                {
                    int pb_w, pb_h;
                    double a1, a2, x1, x2, y1, y2, b1, b2;

                    c2pos = this.PointToClient(Cursor.Position);
                    c2pos.X -= pictureBox1.Location.X;
                    c2pos.Y -= pictureBox1.Location.Y;

                    pb_w = pictureBox1.Width;
                    pb_h = pictureBox1.Height;

                    switch (wtd)
                    {
                        case 1:
                            {
                                x1 = Convert.ToDouble(tb_x1.Text);
                                x2 = Convert.ToDouble(tb_x2.Text);
                                y1 = Convert.ToDouble(tb_y1.Text);
                                y2 = Convert.ToDouble(tb_y2.Text);

                                if (c1pos.X < c2pos.X)
                                {
                                    tb_x1.Text = Convert.ToString(x1 + c1pos.X * (x2 - x1) / pb_w);
                                    tb_x2.Text = Convert.ToString(x1 + c2pos.X * (x2 - x1) / pb_w);
                                }
                                else
                                {
                                    tb_x1.Text = Convert.ToString(x1 + c2pos.X * (x2 - x1) / pb_w);
                                    tb_x2.Text = Convert.ToString(x1 + c1pos.X * (x2 - x1) / pb_w);
                                };

                                if (c1pos.Y < c2pos.Y)
                                {
                                    tb_y1.Text = Convert.ToString(y1 + (pb_h - 1 - c2pos.Y) * (y2 - y1) / pb_h);
                                    tb_y2.Text = Convert.ToString(y1 + (pb_h - 1 - c1pos.Y) * (y2 - y1) / pb_h);
                                }
                                else
                                {
                                    tb_y1.Text = Convert.ToString(y1 + (pb_h - 1 - c1pos.Y) * (y2 - y1) / pb_h);
                                    tb_y2.Text = Convert.ToString(y1 + (pb_h - 1 - c2pos.Y) * (y2 - y1) / pb_h);
                                };
                                break;
                            };

                        case 2:
                            {
                                a1 = Convert.ToDouble(tb_dia_a1.Text);
                                a2 = Convert.ToDouble(tb_dia_a2.Text);
                                x1 = Convert.ToDouble(tb_dia_x1.Text);
                                x2 = Convert.ToDouble(tb_dia_x2.Text);

                                if (c1pos.X < c2pos.X)
                                {
                                    tb_dia_a1.Text = Convert.ToString(a1 + c1pos.X * (a2 - a1) / pb_w);
                                    tb_dia_a2.Text = Convert.ToString(a1 + c2pos.X * (a2 - a1) / pb_w);
                                }
                                else
                                {
                                    tb_dia_a1.Text = Convert.ToString(a1 + c2pos.X * (a2 - a1) / pb_w);
                                    tb_dia_a2.Text = Convert.ToString(a1 + c1pos.X * (a2 - a1) / pb_w);
                                };

                                if (c1pos.Y < c2pos.Y)
                                {
                                    tb_dia_x1.Text = Convert.ToString(x1 + (pb_h - 1 - c2pos.Y) * (x2 - x1) / pb_h);
                                    tb_dia_x2.Text = Convert.ToString(x1 + (pb_h - 1 - c1pos.Y) * (x2 - x1) / pb_h);
                                }
                                else
                                {
                                    tb_dia_x1.Text = Convert.ToString(x1 + (pb_h - 1 - c1pos.Y) * (x2 - x1) / pb_h);
                                    tb_dia_x2.Text = Convert.ToString(x1 + (pb_h - 1 - c2pos.Y) * (x2 - x1) / pb_h);
                                };
                                break;
                            };

                        case 3:
                            {
                                b1 = Convert.ToDouble(tb_t_b1.Text);
                                b2 = Convert.ToDouble(tb_t_b2.Text);
                                a1 = Convert.ToDouble(tb_t_a1.Text);
                                a2 = Convert.ToDouble(tb_t_a2.Text);

                                if (c1pos.X < c2pos.X)
                                {
                                    tb_t_b1.Text = Convert.ToString(b1 + c1pos.X * (b2 - b1) / pb_w);
                                    tb_t_b2.Text = Convert.ToString(b1 + c2pos.X * (b2 - b1) / pb_w);
                                }
                                else
                                {
                                    tb_t_b1.Text = Convert.ToString(b1 + c2pos.X * (b2 - b1) / pb_w);
                                    tb_t_b2.Text = Convert.ToString(b1 + c1pos.X * (b2 - b1) / pb_w);
                                };

                                if (c1pos.Y < c2pos.Y)
                                {
                                    tb_t_a1.Text = Convert.ToString(a1 + (pb_h - 1 - c2pos.Y) * (a2 - a1) / pb_h);
                                    tb_t_a2.Text = Convert.ToString(a1 + (pb_h - 1 - c1pos.Y) * (a2 - a1) / pb_h);
                                }
                                else
                                {
                                    tb_t_a1.Text = Convert.ToString(a1 + (pb_h - 1 - c1pos.Y) * (a2 - a1) / pb_h);
                                    tb_t_a2.Text = Convert.ToString(a1 + (pb_h - 1 - c2pos.Y) * (a2 - a1) / pb_h);
                                };
                                break;
                            };
                    };

                    b_graph = false;
                    Graphics gr_box = Graphics.FromImage(bm_graph);

                    gr_box.DrawLine(p_black, new Point(0, c2pos.Y), new Point(pb_w - 1, c2pos.Y));
                    gr_box.DrawLine(p_black, new Point(c2pos.X, 0), new Point(c2pos.X, pb_h - 1));

                    zoning = 0;
                    b_zone.Text = "Выбрать область";
                    b_zone.Enabled = true;

                    b_graph = true;
                    pictureBox1.Refresh();
                }
                else
                if (wtd == 3)
                {
                    int pb_w, pb_h;
                    double a1, a2, b1, b2;

                    pb_w = pictureBox1.Width;
                    pb_h = pictureBox1.Height;

                    c1pos = this.PointToClient(Cursor.Position);
                    c1pos.X -= pictureBox1.Location.X;
                    c1pos.Y -= pictureBox1.Location.Y;

                    if (form2 == null)
                    {
                        form2 = new Form2_ph();
                    };
                    ph_form2 = true;

                    b1 = Convert.ToDouble(tb_t_b1.Text);
                    b2 = Convert.ToDouble(tb_t_b2.Text);
                    a1 = Convert.ToDouble(tb_t_a1.Text);
                    a2 = Convert.ToDouble(tb_t_a2.Text);
                    pars[pp_p1] = a2 - (a2 - a1) * c1pos.Y / pb_h;
                    pars[pp_p2] = b1 + (b2 - b1) * c1pos.X / pb_w;
                    get_last();

                    draw();
                    form2.set_bitmap(bm_form2);
                    form2.Show();
                };
            };
        }

        private void b_default_Click(object sender, EventArgs e)
        {
            switch (wtd)
            {
                case 1:
                    {
                        tb_x1.Text = Convert.ToString(-2);
                        tb_x2.Text = Convert.ToString(2);
                        tb_y1.Text = Convert.ToString(-2);
                        tb_y2.Text = Convert.ToString(2);
                        break;
                    };

                case 2:
                    {
                        tb_dia_a1.Text = Convert.ToString(0);
                        tb_dia_a2.Text = Convert.ToString(1.2);
                        tb_dia_x1.Text = Convert.ToString(-2);
                        tb_dia_x2.Text = Convert.ToString(2);
                        break;
                    };

                case 3:
                    {
                        tb_t_a1.Text = Convert.ToString(0);
                        tb_t_a2.Text = Convert.ToString(1.5);
                        tb_t_b1.Text = Convert.ToString(0.3);
                        tb_t_b2.Text = Convert.ToString(1.8);
                        break;
                    };
            };
            tb_dia_a1.Text = Convert.ToString(0);
            tb_dia_a2.Text = Convert.ToString(1.2);
            tb_dia_x1.Text = Convert.ToString(-2);
            tb_dia_x2.Text = Convert.ToString(2);
        }

        private void b_t_Click(object sender, EventArgs e)
        {
            b_t.Text = "Идёт рассчёт";
            b_t.Enabled = false;
            b_t_tofile.Text = "Идёт рассчёт";
            b_t_tofile.Enabled = false;

            map_drawer = new Task(() => get_plos_par_pb(pictureBox1.Width, pictureBox1.Height));
            map_drawer.Start();
        }

        private void get_plos_par(int size_x, int size_y)
        {
            int a_n, b_n, x_n, x_s, N;
            double a1, a2, b1, b2, eps, inf;
            bool t_ulast;

            stop = false;

            eps = Convert.ToDouble(tb_t_e.Text);

            a_n = Convert.ToInt32(tb_t_a.Text);
            b_n = Convert.ToInt32(tb_t_b.Text);
            BeginInvoke(new MethodInvoker(delegate
            {
                pbar.Maximum = a_n * b_n - 1;
            }));

            t_ulast = cb_t_last.Checked;

            int[,] t_count = new int[a_n, b_n];
            List<double> t_points = new List<double>();
            t_points.Clear();

            a1 = Convert.ToDouble(tb_t_a1.Text);
            a2 = Convert.ToDouble(tb_t_a2.Text);
            b1 = Convert.ToDouble(tb_t_b1.Text);
            b2 = Convert.ToDouble(tb_t_b2.Text);

            x_n = Convert.ToInt32(tb_t_xn.Text);
            x_s = Convert.ToInt32(tb_t_x_skip.Text);

            inf = Convert.ToInt32(tb_inf.Text);

            N = Convert.ToInt32(tb_taskscount.Text);

            //get_all();
            bm_graph = map.map(N, an, al, pars, a1, a2, a_n, b1, b2, b_n, x_n, x_s, eps, inf, pp_a, pp_p1, pp_p2, size_x, size_y, t_ulast);

            EncoderParameter myEncoderParameter;

            myEncoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);

            bm_graph.Save(saveFileDialog2.FileName);

            BeginInvoke(new MethodInvoker(delegate
            {
                b_t.Text = "Карта режимов";
                b_t.Enabled = true;
                b_t_tofile.Text = "Сохранить в файл";
                b_t_tofile.Enabled = true;
            }));
        }

        private void get_plos_par_pb(int size_x, int size_y)
        {
            int a_n, b_n, x_n, x_s, N;
            double a1, a2, b1, b2, eps, inf;
            bool t_ulast;

            stop = false;

            eps = Convert.ToDouble(tb_t_e.Text);

            a_n = Convert.ToInt32(tb_t_a.Text);
            b_n = Convert.ToInt32(tb_t_b.Text);
            BeginInvoke(new MethodInvoker(delegate
            {
                pbar.Maximum = a_n * b_n - 1;
            }));

            t_ulast = cb_t_last.Checked;

            int[,] t_count = new int[a_n, b_n];
            List<double> t_points = new List<double>();
            t_points.Clear();

            a1 = Convert.ToDouble(tb_t_a1.Text);
            a2 = Convert.ToDouble(tb_t_a2.Text);
            b1 = Convert.ToDouble(tb_t_b1.Text);
            b2 = Convert.ToDouble(tb_t_b2.Text);

            x_n = Convert.ToInt32(tb_t_xn.Text);
            x_s = Convert.ToInt32(tb_t_x_skip.Text);

            inf = Convert.ToInt32(tb_inf.Text);

            N = Convert.ToInt32(tb_taskscount.Text);

            //get_all();
            b_graph = false;
            bm_graph = map.map(N, an, al, pars, a1, a2, a_n, b1, b2, b_n, x_n, x_s, eps, inf, pp_a, pp_p1, pp_p2, size_x, size_y, t_ulast);


            //get_plos_par(pictureBox1.Width, pictureBox1.Height);

            b_graph = true;
            //pictureBox1.Refresh();

            wtd = 3;

            BeginInvoke(new MethodInvoker(delegate
            {
                b_zone.Enabled = true;
                b_t.Text = "Карта режимов";
                b_t.Enabled = true;
                b_t_tofile.Text = "Сохранить в файл";
                b_t_tofile.Enabled = true;
                pictureBox1.Refresh();
            }));
        }

        private void b_t_tofile_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                file_path = saveFileDialog2.FileName;

                b_t.Text = "Идёт рассчёт";
                b_t.Enabled = false;
                b_t_tofile.Text = "Идёт рассчёт";
                b_t_tofile.Enabled = false;

                map_drawer = new Task(() => get_plos_par(Convert.ToInt32(tb_t_f_x.Text), Convert.ToInt32(tb_t_f_y.Text)));
                map_drawer.Start();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int pb_h = pictureBox1.Height, pb_w = pictureBox1.Width;
            Graphics g = e.Graphics;
            if (b_graph)
            {
                g.DrawImage(bm_graph, 0, 0, pb_w, pb_h);
            };
        }
        //////////////////////////////////////////////////////////////////////DRAW
        //////////////////////////////////////////////////////////////////////DRAW
        //////////////////////////////////////////////////////////////////////DRAW
        


        private void b_col_add_Click(object sender, EventArgs e)
        {
            dgv_colors.Rows.Add(0, 255, 255, 255);
            dvg_updcol();
        }

        private void b_col_del_Click(object sender, EventArgs e)
        {
            if (dgv_colors.CurrentCellAddress.Y > 1 && dgv_colors.CurrentCellAddress.Y < dgv_colors.Rows.Count - 1)
            {
                dgv_colors.Rows.RemoveAt(dgv_colors.CurrentCellAddress.Y);
            };
        }

        private void col_save()
        {
            int i;
            num_col.Clear();
            for (i = 0; i < dgv_colors.Rows.Count - 1; i++)
            {
                num_col.Add(new cnum_col() { num = Convert.ToInt32(dgv_colors[0, i].Value), r = Convert.ToInt32(dgv_colors[1, i].Value), g = Convert.ToInt32(dgv_colors[2, i].Value), b = Convert.ToInt32(dgv_colors[3, i].Value) });
            };
            pmax = 0;
            for (i = 0; i < num_col.Count; i++)
            {
                if (num_col[i].num > pmax)
                    pmax = num_col[i].num;
            };
            pmax++;
            if (map_ready)
                map.col_set(num_col, pmax);
        }

        private void b_col_save_Click(object sender, EventArgs e)
        {
            col_save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgv_colors.Rows.Add(-1, 255, 255, 255);
            dgv_colors.Rows.Add(0, 127, 127, 127);


            dgv_colors.Rows.Add(1, 255, 0, 0);
            dgv_colors.Rows.Add(2, 127, 0, 0);
            dgv_colors.Rows.Add(3, 127, 127, 0);
            dgv_colors.Rows.Add(4, 0, 127, 0);
            dgv_colors.Rows.Add(5, 0, 127, 127);
            dgv_colors.Rows.Add(6, 0, 0, 127);
            dgv_colors.Rows.Add(7, 0, 0, 255);
            dgv_colors.Rows.Add(8, 0, 127, 255);
            dgv_colors.Rows.Add(9, 127, 127, 255);
            dgv_colors.Rows.Add(10, 0, 255, 255);
            dgv_colors.Rows.Add(11, 127, 50, 127);
            dgv_colors.Rows.Add(12, 255, 127, 127);
            dgv_colors.Rows.Add(13, 100, 255, 127);
            dgv_colors.Rows.Add(14, 127, 150, 200);
            dgv_colors.Rows.Add(15, 150, 0, 150);
            dgv_colors.Rows.Add(16, 50, 10, 127);

            col_save();

            lb_fun_shab.Items.Add("Эно");
            lb_fun_shab.Items.Add("Ван дер Поль");
            lb_fun_shab.Items.Add("Ресслер_1");
            lb_fun_shab.Items.Add("Лоренц");
            lb_fun_shab.Items.Add("Анищенко-Астахов, d");
            lb_fun_shab.Items.Add("Анищенко-Астахов, exp");
            lb_fun_shab.Items.Add("Богданова-Такенса");
            lb_fun_shab.Items.Add("Ресслер_2");
            /*lb_fun_shab.Items.Add("Неймарка-Сакера");
            lb_fun_shab.Items.Add("Дуффинга");*/

            saveFileDialog_fun.Filter = "Discrete solver file (.dsf)|*.dsf";
            openFileDialog_fun.Filter = "Discrete solver file (.dsf)|*.dsf";

            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1)
                fun_file_open(args[1]);

            form2 = new Form2_ph();
        }

        private void dvg_updcol()
        {
            int i;
            for (i = 0; i < dgv_colors.Rows.Count - 1; i++)
            {
                dgv_colors.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(Convert.ToInt32(dgv_colors[1, i].Value), Convert.ToInt32(dgv_colors[2, i].Value), Convert.ToInt32(dgv_colors[3, i].Value));
            };
        }

        private void dgv_colors_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dvg_updcol();
        }

        private void dgv_colors_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dvg_updcol();
        }

        private void b_col_change_Click(object sender, EventArgs e)
        {
            int i;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                dgv_col = colorDialog1.Color;
            };
            i = dgv_colors.CurrentCellAddress.Y;
            dgv_colors[1, i].Value = dgv_col.R;
            dgv_colors[2, i].Value = dgv_col.G;
            dgv_colors[3, i].Value = dgv_col.B;
            dvg_updcol();
        }
        
        private void b_funkcii_Click(object sender, EventArgs e)
        {
            numb_fun = dgv_fun.Rows.Count - 1;
            get_all();

            compiler = new dynamic_compiler(dgv_fun, dgv_par, dgv_usl);
            func = (dynamic_compiler.Func)compiler.compile();
            map = new map_r(func, numb_fun, pbar);
            map_ready = true;
            ph_port = new phas_port(func, numb_fun);
            diag = new diagram(func, numb_fun);
            col_save();
        }

        private void get_all()
        {
            int i;

            an = new double[numb_fun];
            for (i = 0; i < numb_fun; i++)
                an[i] = Convert.ToDouble(dgv_fun[3, i].Value);

            al = new double[numb_fun];
            for (i = 0; i < numb_fun; i++)
                al[i] = Convert.ToDouble(dgv_fun[3, i].Value);

            pars = new double[dgv_par.Rows.Count - 1];
            for (i = 0; i < dgv_par.Rows.Count - 1; i++)
                pars[i] = Convert.ToDouble(dgv_par[1, i].Value);

            cb_fp_a1.Items.Clear();
            for (i = 0; i < numb_fun; i++)
                cb_fp_a1.Items.Add(dgv_fun[0, i].Value);

            cb_fp_a2.Items.Clear();
            for (i = 0; i < numb_fun; i++)
                cb_fp_a2.Items.Add(dgv_fun[0, i].Value);

            cb_dia_a.Items.Clear();
            for (i = 0; i < numb_fun; i++)
                cb_dia_a.Items.Add(dgv_fun[0, i].Value);

            cb_dia_p.Items.Clear();
            for (i = 0; i < dgv_par.Rows.Count - 1; i++)
                cb_dia_p.Items.Add(Convert.ToString(dgv_par[0, i].Value));

            cb_pp_a.Items.Clear();
            for (i = 0; i < numb_fun; i++)
                cb_pp_a.Items.Add(dgv_fun[0, i].Value);

            cb_pp_p1.Items.Clear();
            for (i = 0; i < dgv_par.Rows.Count - 1; i++)
                cb_pp_p1.Items.Add(Convert.ToString(dgv_par[0, i].Value));

            cb_pp_p2.Items.Clear();
            for (i = 0; i < dgv_par.Rows.Count - 1; i++)
                cb_pp_p2.Items.Add(Convert.ToString(dgv_par[0, i].Value));
        }

        private void b_lyap_Click(object sender, EventArgs e)
        {
            int i, n, norm;
            double eps, value;
            b_lyap.Enabled = false;
            an = new double[numb_fun];
            for (i = 0; i < numb_fun; i++)
                an[i] = Convert.ToDouble(dgv_fun[3, i].Value);
            n = Convert.ToInt32(tb_lyap_norm.Text);
            norm = Convert.ToInt32(tb_lyap_n.Text);
            eps = Convert.ToDouble(tb_lyap_eps.Text);
            value = map.lyap_elder(numb_fun, al, pars, eps, n, norm);
            tb_lyap_elder.Text = Convert.ToString(value);
            b_lyap.Enabled = true;
        }

        private void get_last()
        {
            int i;
            for (i = 0; i < numb_fun; i++)
                al[i] = Convert.ToDouble(dgv_fun[3, i].Value);
        }

        private void b_fun_clear_Click(object sender, EventArgs e)
        {
            dgv_fun.Rows.Clear();
            dgv_par.Rows.Clear();
            dgv_usl.Rows.Clear();
            cb_fp_a1.Items.Clear();
            cb_fp_a2.Items.Clear();
            cb_dia_a.Items.Clear();
            cb_dia_p.Items.Clear();
            cb_pp_a.Items.Clear();
            cb_pp_p1.Items.Clear();
            cb_pp_p2.Items.Clear();
        }

        private void b_add_eno_Click(object sender, EventArgs e)
        {
            int i = lb_fun_shab.SelectedIndex;
            switch (i)
            {
                case 0:
                    {
                        dgv_fun.Rows.Add("x", "1-a*Math.Pow(x,2)+y", "", "1", "");
                        dgv_fun.Rows.Add("y", "b*x", "", "1", "");
                        dgv_par.Rows.Add("a", "0,3");
                        dgv_par.Rows.Add("b", "0,3");
                        break;
                    };
                case 1:
                    {
                        dgv_fun.Rows.Add("y", "y+e*(l*y-Math.Pow(x,2)*y-x)", "", "1", "yn", "");
                        dgv_fun.Rows.Add("x", "x+e*yn", "", "1", "");
                        dgv_par.Rows.Add("e", "0,3");
                        dgv_par.Rows.Add("l", "0,3");
                        break;
                    };
                case 2:
                    {
                        dgv_fun.Rows.Add("x", "x-e*(y+z)", "", "1", "");
                        dgv_fun.Rows.Add("y", "y+e*(x+a*y)", "", "1", "");
                        dgv_fun.Rows.Add("z", "z+e*b+e*(x-r)*z", "", "1", "");
                        dgv_par.Rows.Add("a", "-1,5");
                        dgv_par.Rows.Add("b", "0,2");
                        dgv_par.Rows.Add("e", "0,1");
                        dgv_par.Rows.Add("r", "0");
                        break;
                    };
                case 3:
                    {
                        dgv_fun.Rows.Add("x", "x+e*s*(y-x)", "", "1", "");
                        dgv_fun.Rows.Add("y", "y+e*(r*x-y-x*z)", "", "1", "");
                        dgv_fun.Rows.Add("z", "z-e*(b*z-x*y)", "", "1", "");
                        dgv_par.Rows.Add("s", "10");
                        dgv_par.Rows.Add("b", "6,666");
                        dgv_par.Rows.Add("e", "0,1");
                        dgv_par.Rows.Add("r", "28");
                        break;
                    };
                case 4:
                    {
                        dgv_fun.Rows.Add("x", "x+e*(m*x+y-x*z)", "", "1", "");
                        dgv_fun.Rows.Add("y", "y-e*x", "", "1", "");
                        dgv_fun.Rows.Add("z", "", "", "1", "");
                        dgv_fun[2, dgv_fun.Rows.Count - 2].Value = true;
                        dgv_usl.Rows.Add("z", "x>0", "z-e*g*(z-Math.Pow(x,2))");
                        dgv_usl.Rows.Add("z", "x<=0", "z-e*g*z");
                        dgv_par.Rows.Add("m", "0");
                        dgv_par.Rows.Add("g", "0");
                        dgv_par.Rows.Add("e", "0,1");
                        break;
                    };
                case 5:
                    {
                        dgv_fun.Rows.Add("x", "x+e*(m*x+y-x*z)", "", "1", "");
                        dgv_fun.Rows.Add("y", "y-e*x", "", "1", "");
                        dgv_fun.Rows.Add("z", "z-e*g*(z-Math.Exp(x)-1)", "", "1", "");
                        dgv_par.Rows.Add("m", "0");
                        dgv_par.Rows.Add("g", "0");
                        dgv_par.Rows.Add("e", "0,1");
                        break;
                    };
                case 6:
                    {
                        dgv_fun.Rows.Add("y", "y+e*((u-x)*y-a+Math.Pow(x,2))", "", "1", "yn", "");
                        dgv_fun.Rows.Add("x", "x+e*yn", "", "1", "");
                        dgv_par.Rows.Add("e", "0,1");
                        dgv_par.Rows.Add("u", "1");
                        dgv_par.Rows.Add("a", "1");
                        break;
                    };
                case 7:
                    {
                        dgv_fun.Rows.Add("x", "x+e*(-(y+z))", "", "1", "");
                        dgv_fun.Rows.Add("y", "y+e*(x+a*y)", "", "1", "");
                        dgv_fun.Rows.Add("z", "z+e*(b+z*(x-c))", "", "1", "");
                        dgv_par.Rows.Add("a", "1");
                        dgv_par.Rows.Add("b", "1");
                        dgv_par.Rows.Add("c", "1");
                        dgv_par.Rows.Add("e", "0,1");
                        break;
                    };
                    /*case 7:
                        {
                            dgv_fun.Rows.Add("y", "y+e*y*(l+u*Math.Pow(x,2)-Math.Pow(x,4))-e*x", "", "1", "yn");
                            dgv_fun.Rows.Add("x", "x+e*yn", "", "1");
                            dgv_par.Rows.Add("e", "0,2");
                            dgv_par.Rows.Add("l", "1");
                            dgv_par.Rows.Add("u", "1");
                            break;
                        };
                    case 8:
                        {
                            dgv_fun.Rows.Add("y", "y+e*(-d*y-a*x-b*Math.Pow(x,3))+f*Math.Sin(w*t)", "", "0", "yn");
                            dgv_fun.Rows.Add("x", "x+e*yn", "", "0");
                            dgv_fun.Rows.Add("t", "t+e", "", "0");
                            dgv_fun.Rows.Add("p", "", "", "0");
                            dgv_fun[2, dgv_fun.Rows.Count - 2].Value = true;
                            dgv_fun.Rows.Add("v", "", "", "0");
                            dgv_fun[2, dgv_fun.Rows.Count - 2].Value = true;
                            dgv_usl.Rows.Add("p", "(Math.Abs((w*t)%(2*Math.PI))<=e*w/2)||(Math.Abs(((w*t)%(2*Math.PI))-2*Math.PI)<=e*w/2)", "x");
                            dgv_usl.Rows.Add("p", "!(Math.Abs((w*t)%(2*Math.PI))<=e*w/2)||(Math.Abs(((w*t)%(2*Math.PI))-2*Math.PI)<=e*w/2)", "p");
                            dgv_usl.Rows.Add("v", "(Math.Abs((w*t)%(2*Math.PI))<=e*w/2)||(Math.Abs(((w*t)%(2*Math.PI))-2*Math.PI)<=e*w/2)", "y");
                            dgv_usl.Rows.Add("v", "!(Math.Abs((w*t)%(2*Math.PI))<=e*w/2)||(Math.Abs(((w*t)%(2*Math.PI))-2*Math.PI)<=e*w/2)", "v");
                            dgv_par.Rows.Add("e", "0,1");
                            dgv_par.Rows.Add("d", "1");
                            dgv_par.Rows.Add("a", "0");
                            dgv_par.Rows.Add("b", "1");
                            dgv_par.Rows.Add("f", "1");
                            dgv_par.Rows.Add("w", "1");
                            break;
                        };*/
            };
        }

        private void dgv_fun_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (dgv_fun.Rows.Count != 1)
                dgv_fun[2, dgv_fun.Rows.Count - 2].Value = false;
        }

        private void b_dgv_f_del_Click(object sender, EventArgs e)
        {
            if (dgv_fun.CurrentCellAddress.Y < dgv_fun.Rows.Count - 1)
            {
                dgv_fun.Rows.RemoveAt(dgv_fun.CurrentCellAddress.Y);
            };
        }

        private void b_dgv_par_d_Click(object sender, EventArgs e)
        {
            if (dgv_par.CurrentCellAddress.Y < dgv_par.Rows.Count - 1)
            {
                dgv_par.Rows.RemoveAt(dgv_par.CurrentCellAddress.Y);
            };
        }

        private void b_dgv_usl_d_Click(object sender, EventArgs e)
        {
            if (dgv_usl.CurrentCellAddress.Y < dgv_usl.Rows.Count - 1)
            {
                dgv_usl.Rows.RemoveAt(dgv_usl.CurrentCellAddress.Y);
            };
        }

        private void cb_pp_a_SelectedIndexChanged(object sender, EventArgs e)
        {
            pp_a = cb_pp_a.SelectedIndex;
        }

        private void cb_pp_p1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pp_p1 = cb_pp_p1.SelectedIndex;
        }

        private void cb_pp_p2_SelectedIndexChanged(object sender, EventArgs e)
        {
            pp_p2 = cb_pp_p2.SelectedIndex;
        }

        private void cb_dia_a_SelectedIndexChanged(object sender, EventArgs e)
        {
            dia_a = cb_dia_a.SelectedIndex;
        }

        private void cb_dia_p_SelectedIndexChanged(object sender, EventArgs e)
        {
            dia_p = cb_dia_p.SelectedIndex;
        }

        private void cb_fp_a1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fp_a1 = cb_fp_a1.SelectedIndex;
        }

        private void cb_fp_a2_SelectedIndexChanged(object sender, EventArgs e)
        {
            fp_a2 = cb_fp_a2.SelectedIndex;
        }

        ///////////////////////////////////////////////////РАБОТА С ФАЙЛАМИ ФУНКЦИЙ
        ///////////////////////////////////////////////////РАБОТА С ФАЙЛАМИ ФУНКЦИЙ
        ///////////////////////////////////////////////////РАБОТА С ФАЙЛАМИ ФУНКЦИЙ
        ///////////////////////////////////////////////////РАБОТА С ФАЙЛАМИ ФУНКЦИЙ
        ///////////////////////////////////////////////////РАБОТА С ФАЙЛАМИ ФУНКЦИЙ


        private void b_fun_file_open_Click(object sender, EventArgs e)
        {
            if (openFileDialog_fun.ShowDialog() == DialogResult.OK)
            {
                fun_file_open(openFileDialog_fun.FileName);
            }
        }

        private void fun_file_open(string path)
        {
            dgv_fun.Rows.Clear();
            dgv_par.Rows.Clear();
            dgv_usl.Rows.Clear();
            List<DataGridView> objects_out = new List<DataGridView>();
            objects_out.Add(dgv_fun);
            objects_out.Add(dgv_par);
            objects_out.Add(dgv_usl);
            filework.file_open(path, objects_out);
        }

        private void b_fun_file_save_Click(object sender, EventArgs e)
        {
            if (saveFileDialog_fun.ShowDialog() == DialogResult.OK)
            {
                List<DataGridView> objects_tofile = new List<DataGridView>();
                objects_tofile.Add(dgv_fun);
                objects_tofile.Add(dgv_par);
                objects_tofile.Add(dgv_usl);

                filework.file_save(saveFileDialog_fun.FileName, objects_tofile);
            }
        }
    }
}
