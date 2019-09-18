using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
//using System.Threading;

namespace Disp_WinForm
{
    public partial class Login_Form : Form
    {
        Macros macros = new Macros();
        public Login_Form()
        {
            def_Font_get();
            InitializeComponent();
            textBox_font.Text = Convert.ToString(vars_form.setting_font_size);
        }

        private void def_Font_get()
        {
            var config = File.ReadAllLines("Settings.txt");
            string[] split = config[0].Split(new Char[] { '=' });
            int i = 0;
            Int32.TryParse(split[1], out i);
            vars_form.setting_font_size = i;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", vars_form.setting_font_size);
            
        }

        private void def_Font_set()
        {
            try
            {
                var config = File.ReadAllLines("Settings.txt");
                string[] split = config[0].Split(new Char[] { '=' });
                int n;
                bool isNumeric = Int32.TryParse(textBox_font.Text, out n);
                if (isNumeric is true || textBox_font.Text.ToString() != "")
                {
                    split[0] += "=" + textBox_font.Text;
                    split[1] = "";
                    File.WriteAllLines("Settings.txt", split);
                }
                else
                {
                    File.WriteAllLines("Settings.txt", split, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "def_Font_set()");
            }

        }


        private void Login()
        {
            def_Font_set();

            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True");
                string sql = string.Format("Select * From Users WHERE username='" + textBox_Login.Text + "' AND Password='" + textBox_Pass.Text + "' AND State='" + 1 + "';");
                MySqlCommand myDataAdapter = new MySqlCommand(sql, myConnection);
                myConnection.Open();
                MySqlDataReader reader = myDataAdapter.ExecuteReader();
                List<string> results = new List<string>();

                while (reader.Read())
                {                    
                    results.Add(reader["idUsers"].ToString());
                    results.Add(reader["username"].ToString());
                    results.Add(reader["user_mail"].ToString());
                    results.Add(reader["user_token"].ToString());

                }
                if (results.Count != 0)
                {
                    vars_form.user_login_id = results[0].ToString();
                    vars_form.user_login_name = textBox_Login.Text;
                    vars_form.user_login_email = results[2].ToString();
                    vars_form.user_token = results[3].ToString();
                }
                
                reader.Close();
                
                if (myDataAdapter.ExecuteScalar() != null)
                {
                    Main_window form = new Main_window();
                    form.Show();
                    this.Hide();
                    macros.get_eid_from_token();

                    //try
                    //{
                    //    MyWebRequest myRequest = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "&svc=token/login&params={\"token\":\"d1207c47958c32b224682b5b080fb908CAF3A4507360712CD18AE69617A54F2FC61EFF5D\"}");
                    //    string json = myRequest.GetResponse();
                    //    var m = JsonConvert.DeserializeObject<RootObject>(json);
                    //    vars_form.eid = m.eid;
                    //}


                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show(ex.Message.ToString() + "Login()");
                    //}
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
                myDataAdapter.Dispose();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }


        private void button_Login(object sender, EventArgs e)
        {
            
            Login();

        }

        private void textBox_Pass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login();
            }
        }

        private void textBox_Pass_Enter(object sender, EventArgs e)
        {
            textBox_Pass.SelectAll();
        }

        private void textBox_Login_Enter(object sender, EventArgs e)
        {
            textBox_Login.SelectAll();
        }
    }
}
