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
using Newtonsoft.Json;
//using Sys

namespace Disp_WinForm
{

    public partial class Main_window : Form
    {       
        //public event System.Windows.Forms.DataGridViewCellFormattingEventHandler CellFormatting;
        public Main_window()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            TimeSpan ts1 = new TimeSpan(00, 00, 0);
            dateTimePicker1.Value = DateTime.Now.Date;
            //dateTimePicker1.Value = DateTime.Now.Date.AddHours(-24);
            dateTimePicker1.Value = dateTimePicker1.Value + ts1;

            TimeSpan ts2 = new TimeSpan(23, 59, 59);
            dateTimePicker2.Value = DateTime.Now.Date;
            dateTimePicker2.Value = dateTimePicker2.Value + ts2;

            mysql_open_alarm();
            mysql_close_alarm();
            timer();
        }

        private void timer()
        {
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = 5000;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // code goes here
            mysql_close_alarm();
            mysql_open_alarm();
        }

        public void mysql_open_alarm()
        {
            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True");
                string sql = string.Format("SELECT unit_id, unit_name, idnotification, type_alarm, product, speed, curr_time, msg_time, location, last_location, time_stamp, Status, group_alarm, Users_idUsers, alarm_locked_user FROM btk.notification WHERE Status = 'Відкрито' OR Status = 'Обробляется';");
                MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(sql, myConnection);
                DataSet ds = new DataSet();
                DataSet ds_cmp = new DataSet();
                myDataAdapter.Fill(ds_cmp);
                if (ds_cmp.Tables[0].Rows.Count != dataGridView_open_alarm.Rows.Count)
                {
                    myDataAdapter.Fill(ds);
                    dataGridView_open_alarm.AutoGenerateColumns = false;
                    dataGridView_open_alarm.RowHeadersVisible = false;
                    dataGridView_open_alarm.DataSource = ds.Tables[0];
                    dataGridView_open_alarm.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public void mysql_close_alarm()
        {
            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True");
                string sql = string.Format("SELECT idnotification, unit_id, unit_name,idnotification, type_alarm, product, speed, curr_time, msg_time, location, last_location, time_stamp, group_alarm, Status, Users_idUsers, alarm_locked_user FROM btk.notification WHERE Status = 'Закрито' AND (time_stamp BETWEEN '" + Convert.ToDateTime(dateTimePicker1.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + Convert.ToDateTime(dateTimePicker2.Value).ToString("yyyy-MM-dd HH:mm:ss") + "');");
                MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(sql, myConnection);
                DataSet ds_close_alarm = new DataSet();
                DataSet ds_close_alarm_cmp = new DataSet();
                myDataAdapter.Fill(ds_close_alarm_cmp);
                if (ds_close_alarm_cmp.Tables[0].Rows.Count != dataGridView_close_alarm.Rows.Count)
                {
                    myDataAdapter.Fill(ds_close_alarm);
                    dataGridView_close_alarm.AutoGenerateColumns = false;
                    dataGridView_close_alarm.RowHeadersVisible = false;
                    dataGridView_close_alarm.DataSource = ds_close_alarm.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (dataGridView_open_alarm.Rows[e.RowIndex].Cells[7].Value.ToString() == "Обробляется")
            {
                e.CellStyle.BackColor = Color.Pink;
            }

            if (dataGridView_open_alarm.Rows[e.RowIndex].Cells[9].Value.ToString() != "")
            {
                e.CellStyle.BackColor = Color.Gray;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (dataGridView_open_alarm.Rows[e.RowIndex].Cells[9].Value.ToString() == "")
            {
                vars_form.search_id = dataGridView_open_alarm.Rows[e.RowIndex].Cells[8].Value.ToString(); ;
                vars_form.id_notif = dataGridView_open_alarm.Rows[e.RowIndex].Cells[0].Value.ToString();
                vars_form.id_status = dataGridView_open_alarm.Rows[e.RowIndex].Cells[7].Value.ToString();
                vars_form.unit_name = dataGridView_open_alarm.Rows[e.RowIndex].Cells[2].Value.ToString();
                vars_form.alarm_name = dataGridView_open_alarm.Rows[e.RowIndex].Cells[3].Value.ToString();
                vars_form.restrict_un_group = false;      
            }
            else
            {

                vars_form.search_id = dataGridView_open_alarm.Rows[e.RowIndex].Cells[8].Value.ToString(); ;
                vars_form.id_notif = dataGridView_open_alarm.Rows[e.RowIndex].Cells[9].Value.ToString();
                vars_form.id_status = dataGridView_open_alarm.Rows[e.RowIndex].Cells[7].Value.ToString();
                vars_form.unit_name = dataGridView_open_alarm.Rows[e.RowIndex].Cells[2].Value.ToString();
                vars_form.alarm_name = dataGridView_open_alarm.Rows[e.RowIndex].Cells[3].Value.ToString();
                vars_form.restrict_un_group = false;
                //var dataIndexNo = dataGridView1.Rows[e.RowIndex].Index.ToString();
                //string id_notif = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                // MessageBox.Show(cellValue);
            }

            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True");
                string sql = string.Format("SELECT alarm_locked, alarm_locked_user FROM btk.notification WHERE idnotification = '" + vars_form.id_notif + "';");
                MySqlCommand myDataAdapter = new MySqlCommand(sql, myConnection);
                myConnection.Open();
                MySqlDataReader reader = myDataAdapter.ExecuteReader();
                List<string> results = new List<string>();

                while (reader.Read())
                {
                    results.Add(reader["alarm_locked"].ToString());
                    results.Add(reader["alarm_locked_user"].ToString());
                }
                if (vars_form.user_login_name != "admin")
                {
                    if (results[0] == "1")
                    {
                        MessageBox.Show("Користувач: " + vars_form.user_login_name + " вже опрацовуе тривогу.");
                        return;
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            detail subwindow = new detail();
            subwindow.Show();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_close_alarm.Rows[e.RowIndex].Cells[9].Value.ToString() == "")
            {
                vars_form.search_id = dataGridView_close_alarm.Rows[e.RowIndex].Cells[8].Value.ToString(); ;
                vars_form.id_notif = dataGridView_close_alarm.Rows[e.RowIndex].Cells[0].Value.ToString();
                vars_form.id_status = dataGridView_close_alarm.Rows[e.RowIndex].Cells[7].Value.ToString();
                vars_form.unit_name = dataGridView_close_alarm.Rows[e.RowIndex].Cells[2].Value.ToString();
                vars_form.alarm_name = dataGridView_close_alarm.Rows[e.RowIndex].Cells[3].Value.ToString();
                vars_form.restrict_un_group = true;
                detail subwindow = new detail();
                subwindow.Show();
            }
            else
            {

                vars_form.search_id = dataGridView_close_alarm.Rows[e.RowIndex].Cells[8].Value.ToString(); ;
                vars_form.id_notif = dataGridView_close_alarm.Rows[e.RowIndex].Cells[9].Value.ToString();
                vars_form.id_status = dataGridView_close_alarm.Rows[e.RowIndex].Cells[7].Value.ToString();
                vars_form.unit_name = dataGridView_close_alarm.Rows[e.RowIndex].Cells[2].Value.ToString();
                vars_form.alarm_name = dataGridView_close_alarm.Rows[e.RowIndex].Cells[3].Value.ToString();
                vars_form.restrict_un_group = true;

                detail subwindow = new detail();
                subwindow.Show();
            }
        }

        private void Main_window_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView_close_alarm.Rows[e.RowIndex].Cells[9].Value.ToString() != "")
            {
                e.CellStyle.BackColor = Color.Gray;
            }
        }

    }

    public static class vars_form
    {
        public static string id_notif { get; set; }
        public static string id_status { get; set; }
        public static string user_login_id{ get; set; }
        public static string search_id { get; set; }
        public static string user_login_name { get; set; }
        public static string unit_name { get; set; }
        public static bool restrict_un_group { get; set; }
        public static string alarm_name { get; set; }
    }
}
