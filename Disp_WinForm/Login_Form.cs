using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
//using System.Threading;

namespace Disp_WinForm
{
    public partial class Login_Form : Form
    {
        public Login_Form()
        {
            InitializeComponent();
        }

        private void Login()
        {
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
                }
                if (results.Count != 0)
                {
                    vars_form.user_login_id = results[0].ToString();
                    vars_form.user_login_name = textBox_Login.Text;
                }
                
                reader.Close();
                if (myDataAdapter.ExecuteScalar() != null)
                {
                    Main_window form = new Main_window();
                    form.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
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
