using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Data;

namespace nona
{
    class fileworker
    {
        public void file_save(string path, List<DataGridView> objects_in)
        {
            /*
            List<DataGridView> objects_out = new List<DataGridView>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<List<DataTable>>));
            
            objects_out.Add(objects_in[0]);
            objects_out.Add(objects_in[1]);
            objects_out.Add(objects_in[2]);
            */
            
            List<List<string>> objects_out = new List<List<string>>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<List<string>>));
            //XmlSerializer serializer = new XmlSerializer(typeof(List<string>));

            
            //objects_out.Add("1.0");
            
            
            int i, n;

            DataGridView dgv_temp = new DataGridView();


            dgv_temp = (DataGridView)objects_in[0];

            n = dgv_temp.RowCount;

            List<String> l_fun_0 = new List<string>();
            for (i = 0; i < n - 1; i++)
                l_fun_0.Add(dgv_temp[0, i].Value.ToString());
            objects_out.Add(l_fun_0);
            List<String> l_fun_1= new List<string>();
            for (i = 0; i < n - 1; i++)
                l_fun_1.Add(dgv_temp[1, i].Value.ToString());
            objects_out.Add(l_fun_1);
            List<string> l_fun_2 = new List<string>();
            for (i = 0; i < n - 1; i++)
                l_fun_2.Add(dgv_temp[2, i].Value.ToString());
            objects_out.Add(l_fun_2);
            List<String> l_fun_3 = new List<string>();
            for (i = 0; i < n - 1; i++)
                l_fun_3.Add(dgv_temp[3, i].Value.ToString());
            objects_out.Add(l_fun_3);
            List<String> l_fun_4 = new List<string>();
            for (i = 0; i < n - 1; i++)
                l_fun_4.Add(dgv_temp[4, i].Value.ToString());
            objects_out.Add(l_fun_4);



            dgv_temp = (DataGridView)objects_in[1];
            n = dgv_temp.RowCount;

            List<String> l_par_0 = new List<string>();
            for (i = 0; i < n - 1; i++)
                l_par_0.Add(dgv_temp[0, i].Value.ToString());
            objects_out.Add(l_par_0);
            List<String> l_par_1 = new List<string>();
            for (i = 0; i < n - 1; i++)
                l_par_1.Add(dgv_temp[1, i].Value.ToString());
            objects_out.Add(l_par_1);



            dgv_temp = (DataGridView)objects_in[2];
            n = dgv_temp.RowCount;

            List<String> l_usl_0 = new List<string>();
            for (i = 0; i < n - 1; i++)
                l_usl_0.Add(dgv_temp[0, i].Value.ToString());
            objects_out.Add(l_usl_0);
            List<String> l_usl_1 = new List<string>();
            for (i = 0; i < n - 1; i++)
                l_usl_1.Add(dgv_temp[1, i].Value.ToString());
            objects_out.Add(l_usl_1);
            List<String> l_usl_2 = new List<string>();
            for (i = 0; i < n - 1; i++)
                l_usl_2.Add(dgv_temp[2, i].Value.ToString());
            objects_out.Add(l_usl_2);
            
            


            //using (var fs = File.OpenWrite(path))
            //    serializer.Serialize(fs, objects_out);
            //serializer.Serialize(fs, objects_out);
            using (FileStream fs = new FileStream(path, FileMode.Create))
                serializer.Serialize(fs, objects_out);

        }

        public void file_open(string path, List<DataGridView> objects_in)
        {
            List<DataGridView> objects_out = new List<DataGridView>();
            List<List<string>> objects_in_file = new List<List<string>>();

            XmlSerializer serializer = new XmlSerializer(typeof(List<List<string>>));

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                objects_in_file = (List<List<string>>)serializer.Deserialize(fs);
            }
            /*
            switch ((string)objects_in[0])
            {
                case "1.0":
                    objects_out = open_v_1_0(objects_in);
                    break;
            }
            */

            int i;



            for (i = 0; i < objects_in_file[0].Count; i++)
            {
                objects_in[0].Rows.Add(objects_in_file[0][i], objects_in_file[1][i], "", objects_in_file[3][i], objects_in_file[4][i]);
                if (objects_in_file[2][i] == "True")
                    objects_in[0][2, i].Value = true;
            }

            for (i = 0; i < objects_in_file[5].Count; i++)
                objects_in[1].Rows.Add(objects_in_file[5][i], objects_in_file[6][i]);

            for (i = 0; i < objects_in_file[7].Count; i++)
                objects_in[2].Rows.Add(objects_in_file[7][i], objects_in_file[8][i], objects_in_file[9][i]);
        }

        private List<object> open_v_1_0(List<object> objects_in)
        {
            List<object> objects_out = new List<object>();

            objects_out.Add(objects_in[1]);
            objects_out.Add(objects_in[2]);
            objects_out.Add(objects_in[3]);

            return objects_out;
        }
    }
}
