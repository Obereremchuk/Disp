using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;


namespace Disp_WinForm
{

    public partial class Main_window : Form
    {

        private static System.Timers.Timer aTimer2;
        Macros macros = new Macros();

        public Main_window()
        {
            
            this.Font = new System.Drawing.Font("Arial", vars_form.setting_font_size);

            InitializeComponent();

            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            dateTimePicker1.ValueChanged -= dateTimePicker1_ValueChanged;
            dateTimePicker2.ValueChanged -= dateTimePicker2_ValueChanged;
            TimeSpan ts1 = new TimeSpan(00, 00, 0);
            dateTimePicker1.Value = DateTime.Now.Date;
            dateTimePicker1.Value = dateTimePicker1.Value + ts1;

            TimeSpan ts2 = new TimeSpan(23, 59, 59);
            dateTimePicker2.Value = DateTime.Now.Date;
            dateTimePicker2.Value = dateTimePicker2.Value + ts2;
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
            dateTimePicker2.ValueChanged += dateTimePicker2_ValueChanged;

            comboBox_sort_column.SelectedIndexChanged -= checkBox_sort_order_CheckedChanged;
            comboBox_sort_close.SelectedIndexChanged -= checkBox_sort_order_CheckedChanged;

            comboBox_sort_column.SelectedItem = "Час тривоги";
            checkBox_sort_order.Checked = true;
            vars_form.order_sort = "desc";
            checkBox_hide_groupe_alarm.Checked = true;
            vars_form.hide_group_alarm = " and group_alarm is null";

            comboBox_sort_close.SelectedItem = "Час тривоги";
            checkBox_sort_order_close.Checked = true;
            comboBox_sort_column.SelectedIndexChanged += checkBox_sort_order_CheckedChanged;
            comboBox_sort_close.SelectedIndexChanged += checkBox_sort_order_CheckedChanged;
            
            mysql_909_n();
            mysql_808_n();
            mysql_dilery();
            mysql_sales();
            build_testing_listwiev();
            timer();



            Pen pen = new Pen(Color.Red);
            vars_form.c_color = pen;
            SolidBrush pen1 = new SolidBrush(Color.Red);
            vars_form.b_color = pen1;
        }

        private void update_session()
        {
            try
            {
                MyWebRequest myRequest = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "&svc=token/login&params={\"token\":\"d1207c47958c32b224682b5b080fb908CAF3A4507360712CD18AE69617A54F2FC61EFF5D\"}");
                string json = myRequest.GetResponse();
                var m = JsonConvert.DeserializeObject<RootObject>(json);
                vars_form.eid = m.eid;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "update_session()");
            }
        }

        private void timer()
        {
            aTimer2 = new System.Timers.Timer();
            aTimer2.Interval = 5000;
            aTimer2.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer2.AutoReset = true;
            aTimer2.Enabled = true;
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new System.Timers.ElapsedEventHandler(OnTimedEvent)
                    , new object[] { sender, e });

                return; // в этом побочном потоке ничего не делаем больше
            }
            //mysql_close_alarm();
            mysql_open_alarm();
            mysql_909_n();
            mysql_808_n();
            mysql_dilery();
            mysql_sales();

        }
        
        public void mysql_open_alarm()
        {
            try
            {          
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                string sql = string.Format("SELECT unit_id, unit_name, idnotification, type_alarm, product, speed, curr_time, msg_time, location, last_location, time_stamp, Status, group_alarm, Users_idUsers, alarm_locked_user FROM btk.notification WHERE Status = 'Відкрито' OR Status = 'Обробляется' " + vars_form.hide_group_alarm + "  order by btk.notification." + vars_form.sort.ToString() + " " + vars_form.order_sort + ";");
                MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(sql, myConnection);
                DataSet ds = new DataSet();
                
                //dataGridView_open_alarm.SuspendLayout(); //need to delete, i think

                myDataAdapter.Fill(ds);
                dataGridView_open_alarm.AutoGenerateColumns = false;
                dataGridView_open_alarm.RowHeadersVisible = false;
                
                int scrollPosition = this.dataGridView_open_alarm.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы

                dataGridView_open_alarm.DataSource = ds.Tables[0];
                myDataAdapter.Dispose();
                myConnection.Close();



                if (ds.Tables[0].Rows.Count > 0)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
                {
                    if (scrollPosition == -1)
                    {
                        scrollPosition = 0;
                    }
                    this.dataGridView_open_alarm.FirstDisplayedScrollingRowIndex = scrollPosition;
                }

                //dataGridView_open_alarm.ResumeLayout();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "mysql_open_alarm()");
            }
        }

        public void mysql_dilery()
        {
            try
            {
                int scrollPosition = this.dataGridView_dilery.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы

                // Set up the DataGridView.
                dataGridView_dilery.Dock = DockStyle.Fill;
                
                // Automatically generate the DataGridView columns.
                dataGridView_dilery.AutoGenerateColumns = true;

                // Set up the data source.
                dataGridView_dilery.DataSource = macros.GetData("SELECT idnotification as 'ID тривоги'," +
                                                                "product as 'Продукт'," +
                                                               "unit_name as 'Назва об’єкту'," +
                                                               "type_alarm as 'Тип тривоги'," +
                                                               "unit_id as 'ID'," +
                                                               "Status as 'Статус'," +
                                                               "alarm_locked_user as 'Обробляє'," +
                                                               "msg_time as 'Час створення'," +
                                                               "group_alarm as 'Згруповано до', " +
                                                                "Users_idUsers as 'Створив'," +
                                                               "speed FROM btk.notification WHERE Status = 'Дилеры' " + vars_form.hide_group_alarm + "  order by btk.notification." + vars_form.sort.ToString() + " " + vars_form.order_sort + ";");
                //dataGridView_dilery.DataSource = macros.GetData("SELECT unit_id, unit_name, idnotification, type_alarm, product, speed, curr_time, msg_time, location, last_location, time_stamp, Status, group_alarm, Users_idUsers, alarm_locked_user FROM btk.notification WHERE Status = 'Дилеры' " + vars_form.hide_group_alarm + "  order by btk.notification." + vars_form.sort.ToString() + " " + vars_form.order_sort + ";");
                //dataGridView_dilery.DataSource = macros.GetData("SELECT unit_id, unit_name, idnotification, type_alarm, product, speed, curr_time, msg_time, location, last_location, time_stamp, Status, group_alarm, Users_idUsers, alarm_locked_user FROM btk.notification WHERE Status = 'Відкрито' OR Status = 'Обробляется' " + vars_form.hide_group_alarm + "  order by btk.notification." + vars_form.sort.ToString() + " " + vars_form.order_sort + ";");
                //dataGridView_dilery.DataSource = macros.GetData("SELECT" +
                //                                                  " idnotification as 'ID тривоги'," +
                //                                                  " unit_name as 'Назва об’єкту'," +
                //                                                  " type_alarm as 'Тип тривоги'," +
                //                                                  " product as 'Продукт'," +
                //                                                  " msg_time as 'Час створення'," +
                //                                                  " Status as 'Статус'," +
                //                                                  " Users_idUsers as 'Створив'," +
                //                                                  " alarm_locked_user as 'Обробляє'" +
                //                                                  " FROM" +
                //                                                  " btk.notification WHERE" +
                //                                                  " Status = 'Дилеры' " +
                //                                                  vars_form.hide_group_alarm + " " +
                //                                                  "order by btk.notification." +
                //                                                  vars_form.sort.ToString() + " " +
                //                                                  vars_form.order_sort + ";");

                // Automatically resize the visible rows.
                dataGridView_dilery.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
                dataGridView_dilery.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

                // Set the DataGridView control's border.
                dataGridView_dilery.BorderStyle = BorderStyle.Fixed3D;

                // Put the cells in edit mode when user enters them.
                dataGridView_dilery.EditMode = DataGridViewEditMode.EditOnEnter;

                if (dataGridView_dilery.Rows.Count > 0
                ) // если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
                {
                    if (scrollPosition == -1)
                    {
                        scrollPosition = 0;
                    }

                    this.dataGridView_dilery.FirstDisplayedScrollingRowIndex = scrollPosition;
                }
            }
            catch(MySqlException)
            {
                MessageBox.Show("rquest troble", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void mysql_sales()
        {
            try
            {
                // Set up the DataGridView.
                dataGridView_sales.Dock = DockStyle.Fill;

                // Automatically generate the DataGridView columns.
                dataGridView_sales.AutoGenerateColumns = true;

                // Set up the data source.

                dataGridView_sales.DataSource = macros.GetData("SELECT idnotification as 'ID тривоги'," +
                                                               "product as 'Продукт'," +
                                                               "unit_name as 'Назва об’єкту'," +
                                                               "type_alarm as 'Тип тривоги'," +
                                                               "unit_id as 'ID'," +
                                                               "Status as 'Статус'," +
                                                               "alarm_locked_user as 'Обробляє'," +
                                                               "msg_time as 'Час створення'," +
                                                               "group_alarm as 'Згруповано до', " +
                                                               "Users_idUsers as 'Створив'," +
                                                               "speed, " +
                                                               "remaynder_activate as 'Нагадати', " +
                                                               "remayder_date as 'Дата нагадування'  FROM btk.notification WHERE Status = 'Продажи' " + vars_form.hide_group_alarm + "  order by btk.notification." + vars_form.sort.ToString() + " " + vars_form.order_sort + ";");

                //dataGridView_sales.DataSource = macros.GetData("SELECT idnotification as 'ID тривоги'," +
                //                                               "product as 'Продукт'," +
                //                                               "unit_name as 'Назва об’єкту'," +
                //                                               "type_alarm as 'Тип тривоги'," +
                //                                               "unit_id as 'ID'," +
                //                                               "Status as 'Статус'," +
                //                                               "alarm_locked_user as 'Обробляє'," +
                //                                               "msg_time as 'Час створення'," +
                //                                               "group_alarm as 'Згруповано до', " +
                //                                               "Users_idUsers as 'Створив'," +
                //                                               "speed FROM btk.notification WHERE Status = 'Продажи' " + vars_form.hide_group_alarm + "  order by btk.notification." + vars_form.sort.ToString() + " " + vars_form.order_sort + ";");
                //dataGridView_sales.DataSource = macros.GetData("SELECT" +
                //                                                  " idnotification as 'ID тривоги'," +
                //                                                  " unit_name as 'Назва об’єкту'," +
                //                                                  " type_alarm as 'Тип тривоги'," +
                //                                                  " product as 'Продукт'," +
                //                                                  " msg_time as 'Час створення'," +
                //                                                  " Status as 'Статус'," +
                //                                                  " Users_idUsers as 'Створив'," +
                //                                                  " alarm_locked_user as 'Обробляє'" +
                //                                                  " FROM" +
                //                                                  " btk.notification WHERE" +
                //                                                  " Status = 'Дилеры' " +
                //                                                  vars_form.hide_group_alarm + " " +
                //                                                  "order by btk.notification." +
                //                                                  vars_form.sort.ToString() + " " +
                //                                                  vars_form.order_sort + ";");

                //dataGridView_sales.Columns["Amount"].Visible = false;
                // Automatically resize the visible rows.
                dataGridView_sales.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
                dataGridView_sales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

                // Set the DataGridView control's border.
                dataGridView_sales.BorderStyle = BorderStyle.Fixed3D;

                // Put the cells in edit mode when user enters them.
                dataGridView_sales.EditMode = DataGridViewEditMode.EditOnEnter;
            }
            catch (MySqlException)
            {
                MessageBox.Show("rquest troble", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void mysql_808_n()
        {
            try
            {
                int scrollPosition = this.dataGridView_808_n.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы

                // Set up the DataGridView.
                dataGridView_808_n.Dock = DockStyle.Fill;

                // Automatically generate the DataGridView columns.
                dataGridView_808_n.AutoGenerateColumns = true;

                // Set up the data source.
                dataGridView_808_n.DataSource = macros.GetData("SELECT idnotification as 'ID тривоги'," +
                                                                "product as 'Продукт'," +
                                                               "unit_name as 'Назва об’єкту'," +
                                                               "type_alarm as 'Тип тривоги'," +
                                                               "unit_id as 'ID'," +
                                                               "Status as 'Статус'," +
                                                               "alarm_locked_user as 'Обробляє'," +
                                                               "msg_time as 'Час створення'," +
                                                               "group_alarm as 'Згруповано до', " +
                                                               "Users_idUsers as 'Створив'," +
                                                               "speed, " +
                                                               "remaynder_activate as 'Нагадати', " +
                                                               "remayder_date as 'Дата нагадування'  FROM btk.notification WHERE Status = '808' " + vars_form.hide_group_alarm + "  order by btk.notification." + vars_form.sort.ToString() + " " + vars_form.order_sort + ";");

                // Automatically resize the visible rows.
                dataGridView_808_n.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
                dataGridView_808_n.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;



                // Set the DataGridView control's border.
                dataGridView_808_n.BorderStyle = BorderStyle.Fixed3D;

                // Put the cells in edit mode when user enters them.
                dataGridView_808_n.EditMode = DataGridViewEditMode.EditOnEnter;

                if (dataGridView_808_n.Rows.Count > 0
                ) // если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
                {
                    if (scrollPosition == -1)
                    {
                        scrollPosition = 0;
                    }

                    this.dataGridView_808_n.FirstDisplayedScrollingRowIndex = scrollPosition;
                }
            }
            catch (MySqlException)
            {
                MessageBox.Show("rquest troble", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void mysql_909_n()
        {
            try
            {
                int scrollPosition = this.dataGridView_909_n.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы

                // Set up the DataGridView.
                dataGridView_909_n.Dock = DockStyle.Fill;

                // Automatically generate the DataGridView columns.
                dataGridView_909_n.AutoGenerateColumns = true;

                // Set up the data source.
                dataGridView_909_n.DataSource = macros.GetData("SELECT idnotification as 'ID тривоги'," +
                                                                "product as 'Продукт'," +
                                                               "unit_name as 'Назва об’єкту'," +
                                                               "type_alarm as 'Тип тривоги'," +
                                                               "unit_id as 'ID'," +
                                                               "Status as 'Статус'," +
                                                               "alarm_locked_user as 'Обробляє'," +
                                                               "msg_time as 'Час створення'," +
                                                               "group_alarm as 'Згруповано до', " +
                                                               "Users_idUsers as 'Створив'," +
                                                               "speed, " +
                                                               "remaynder_activate as 'Нагадати', " +
                                                               "remayder_date as 'Дата нагадування'  FROM btk.notification WHERE Status = '909' " + vars_form.hide_group_alarm + "  order by btk.notification." + vars_form.sort.ToString() + " " + vars_form.order_sort + ";");

                // Automatically resize the visible rows.
                dataGridView_909_n.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
                dataGridView_909_n.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

                

                // Set the DataGridView control's border.
                dataGridView_909_n.BorderStyle = BorderStyle.Fixed3D;

                // Put the cells in edit mode when user enters them.
                dataGridView_909_n.EditMode = DataGridViewEditMode.EditOnEnter;

                if (dataGridView_909_n.Rows.Count > 0
                ) // если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
                {
                    if (scrollPosition == -1)
                    {
                        scrollPosition = 0;
                    }

                    this.dataGridView_909_n.FirstDisplayedScrollingRowIndex = scrollPosition;
                }
            }
            catch (MySqlException)
            {
                MessageBox.Show("rquest troble", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void mysql_close_alarm()
        {
            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                //string sql = string.Format("SELECT idnotification, unit_id, unit_name,idnotification, type_alarm, product, speed, curr_time, msg_time, location, last_location, btk.notification.time_stamp, group_alarm, Status, Users_idUsers, alarm_locked_user, (SELECT username FROM btk.Users WHERE idUsers = btk.notification.Users_idUsers) FROM btk.notification WHERE Status = 'Закрито' AND (btk.notification.time_stamp BETWEEN '" + Convert.ToDateTime(dateTimePicker1.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + Convert.ToDateTime(dateTimePicker2.Value).ToString("yyyy-MM-dd HH:mm:ss") + "');");
                string sql = string.Format("SELECT idnotification, unit_id, unit_name, type_alarm, product, speed, curr_time, msg_time, location, last_location, btk.notification.time_stamp, group_alarm, Status, Users_idUsers, alarm_locked_user, (SELECT username FROM btk.Users WHERE idUsers = btk.notification.Users_idUsers) FROM btk.notification WHERE Status = 'Закрито' " + vars_form.hide_group_alarm + "  and btk.notification.unit_name LIKE '%" + search_close_alarm.Text.ToString() + "%' and btk.notification.type_alarm like '%" + comboBox_close_alarm_type.Text.ToString() + "%' and (SELECT username FROM btk.Users WHERE idUsers = btk.notification.Users_idUsers) like '%" + textBox_close_user_chenge.Text.ToString() + "%' AND (btk.notification.time_stamp BETWEEN '" + Convert.ToDateTime(dateTimePicker1.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + Convert.ToDateTime(dateTimePicker2.Value).ToString("yyyy-MM-dd HH:mm:ss") + "') order by btk.notification." + vars_form.sort_close + " " + " " + vars_form.order_close_sort +" limit " + textBox_limit_close.Text.ToString() +" ");
                MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(sql, myConnection);
                DataSet ds_close_alarm = new DataSet();
                
                

                myDataAdapter.Fill(ds_close_alarm);
                dataGridView_close_alarm.AutoGenerateColumns = false;
                dataGridView_close_alarm.RowHeadersVisible = false;

                int scrollPosition = this.dataGridView_close_alarm.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы

                dataGridView_close_alarm.DataSource = ds_close_alarm.Tables[0];
                myDataAdapter.Dispose();
                myConnection.Close();

                if (ds_close_alarm.Tables[0].Rows.Count > 0)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
                {
                    if (scrollPosition == -1)
                    {
                        scrollPosition = 0;
                    }
                    this.dataGridView_close_alarm.FirstDisplayedScrollingRowIndex = scrollPosition;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "mysql_close_alarm()");
            }
            
        }

        void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dataGridView_open_alarm.SuspendLayout();

            if (dataGridView_open_alarm.Rows[e.RowIndex].Cells[4].Value.ToString() == "Обробляется")
            {
                e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#6ace23");
            }

            if (dataGridView_open_alarm.Rows[e.RowIndex].Cells[4].Value.ToString() == "909")
            {
                e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#f4bf42");
            }

            if (dataGridView_open_alarm.Rows[e.RowIndex].Cells[4].Value.ToString() == "808")
            {
                e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#bcccf2");
            }

            if (dataGridView_open_alarm.Rows[e.RowIndex].Cells[10].Value.ToString() != "")
            {
                e.CellStyle.BackColor = Color.Gray;
            }
            dataGridView_open_alarm.ResumeLayout();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            if (dataGridView_open_alarm.Rows[e.RowIndex].Cells[10].Value.ToString() == "")//Згруповано до ID тривоги(9)
            {
                vars_form.search_id = dataGridView_open_alarm.Rows[e.RowIndex].Cells[9].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_open_alarm.Rows[e.RowIndex].Cells[0].Value.ToString();//ID Тривоги (0)
                vars_form.id_status = dataGridView_open_alarm.Rows[e.RowIndex].Cells[4].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_open_alarm.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_open_alarm.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_open_alarm.Rows[e.RowIndex].Cells[8].Value.ToString();//Звернення(4)
            }
            else
            {

                vars_form.search_id = dataGridView_open_alarm.Rows[e.RowIndex].Cells[9].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_open_alarm.Rows[e.RowIndex].Cells[10].Value.ToString();//Згруповано до ID тривоги(9)
                vars_form.id_status = dataGridView_open_alarm.Rows[e.RowIndex].Cells[4].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_open_alarm.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_open_alarm.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_open_alarm.Rows[e.RowIndex].Cells[8].Value.ToString();//Звернення(4)
                //var dataIndexNo = dataGridView1.Rows[e.RowIndex].Index.ToString();
                //string id_notif = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                // MessageBox.Show(cellValue);
            }

            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
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
                if (vars_form.user_login_name != "admin" & vars_form.user_login_name != results[1])
                {
                    if (results[0] == "1")
                    {
                        MessageBox.Show("Користувач: " + results[1] + " вже опрацовуе тривогу.");
                        return;
                    }
                }
                reader.Close();
                myDataAdapter.Dispose();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "dataGridView1_CellContentClick");
            }

            // Блокируем обработку тревоги однвременно двумя операторами, и вносим информацию кто открыл тревогу
            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                string sql = string.Format("UPDATE btk.notification SET alarm_locked = '1', alarm_locked_user = '" + vars_form.user_login_name + "' WHERE idnotification = '" + vars_form.id_notif + "';");
                MySqlCommand MyCommand2 = new MySqlCommand(sql, myConnection);
                MySqlDataReader MyReader2;
                myConnection.Open();
                MyReader2 = MyCommand2.ExecuteReader();
                MyCommand2.Dispose();
                myConnection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "detail_Load()");
            }

            detail subwindow = new detail();
            subwindow.Show();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_close_alarm.Rows[e.RowIndex].Cells[9].Value.ToString() == "")
            {
                vars_form.search_id = dataGridView_close_alarm.Rows[e.RowIndex].Cells[8].Value.ToString();
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

        private void button_add_alarm_Click(object sender, EventArgs e)
        {
            Add_alarm subwindow2 = new Add_alarm();
            subwindow2.Show();
        }

        private void comboBox_sort_column_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_sort_column.SelectedItem.ToString() == "ID Тривоги")
            {
                vars_form.sort = "idnotification";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "Продукт")
            {
                vars_form.sort = "product";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "Назва обєкту")
            {
                vars_form.sort = "unit_name";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "Тип тривоги")
            {
                vars_form.sort = "type_alarm";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "Статус")
            {
                vars_form.sort = "Status";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "Обробляется")
            {
                vars_form.sort = "alarm_locked_user";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "Час тривоги")
            {
                vars_form.sort = "msg_time";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "Час отримання тривоги")
            {
                vars_form.sort = "curr_time";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "Звернення")
            {
                vars_form.sort = "speed";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "ID обєкту")
            {
                vars_form.sort = "Unit_id";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "Згруповано до ID тривоги")
            {
                vars_form.sort = "group_alarm";
            }
            mysql_open_alarm();
        }

        private void checkBox_sort_order_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_sort_order.Checked)
            {
                vars_form.order_sort = "desc";
            }
            else
            {
                vars_form.order_sort = "asc";
            }
            mysql_open_alarm();
        }

        private void comboBox_sort_close_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_sort_close.SelectedItem.ToString() == "ID Тривоги")
            {
                vars_form.sort_close = "idnotification";
            }
            if (comboBox_sort_close.SelectedItem.ToString() == "Продукт")
            {
                vars_form.sort_close = "product";
            }
            if (comboBox_sort_close.SelectedItem.ToString() == "Назва об’єкту")
            {
                vars_form.sort_close = "unit_name";
            }
            if (comboBox_sort_close.SelectedItem.ToString() == "Тип тривоги")
            {
                vars_form.sort_close = "type_alarm";
            }
            if (comboBox_sort_close.SelectedItem.ToString() == "Час тривоги")
            {
                vars_form.sort_close = "msg_time";
            }
            if (comboBox_sort_close.SelectedItem.ToString() == "Час отримання тривоги")
            {
                vars_form.sort_close = "curr_time";
            }
            if (comboBox_sort_close.SelectedItem.ToString() == "Статус")
            {
                vars_form.sort_close = "Status";
            }
            if (comboBox_sort_close.SelectedItem.ToString() == "ID Об’єкту")
            {
                vars_form.sort_close = "Unit_id";
            }
            if (comboBox_sort_close.SelectedItem.ToString() == "Згруповано до ID Тривоги:")
            {
                vars_form.sort_close = "group_alarm";
            }
            if (comboBox_sort_close.SelectedItem.ToString() == "Остання зміна")
            {
                vars_form.sort_close = "time_stamp";
            }
            if (comboBox_sort_close.SelectedItem.ToString() == "Користувач що змінив")
            {
                vars_form.sort_close = "(SELECT username FROM btk.Users WHERE idUsers = btk.notification.Users_idUsers)";
            }
            if (comboBox_sort_close.SelectedItem.ToString() == "Обробляєтся")
            {
                vars_form.sort_close = "alarm_locked_user";
            }

            mysql_close_alarm();
        }

        private void search_close_alarm_TextChanged(object sender, EventArgs e)
        {
            mysql_close_alarm();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            mysql_close_alarm();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            mysql_close_alarm();
        }

        private void comboBox_close_alarm_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_close_alarm_type.Text.ToString() == "Всі")
            {
                comboBox_close_alarm_type.Text = "";
            }
            mysql_close_alarm();
        }

        private void textBox_close_user_chenge_TextChanged(object sender, EventArgs e)
        {
            mysql_close_alarm();
        }

        private void checkBox_sort_order_close_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_sort_order_close.Checked)
            {
                vars_form.order_close_sort = "desc";
            }
            else
            {
                vars_form.order_close_sort = "asc";
            }
            mysql_close_alarm();
        }

        private void textBox_limit_close_TextChanged(object sender, EventArgs e)
        {
            
            if (textBox_limit_close.Text.ToString() == "")
            { textBox_limit_close.Text = "1"; }
            mysql_close_alarm();
        }

        private void button_test_sel_obj_yes_Click(object sender, EventArgs e)
        {
            //get_sensor_value();
            if (listBox_test_search_result.SelectedItems.Count <= 0)
            {
                return;
            }

            vars_form.id_object_for_test = listBox_test_search_result.SelectedValue.ToString();
            vars_form.id_db_object_for_test = macros.sql_command2("SELECT idObject FROM btk.Object where Object_id_wl="+vars_form.id_object_for_test+";");


            Testing form_Testing = new Testing();
            form_Testing.Activated += new EventHandler(form_Testing_activated);
            form_Testing.FormClosed += new FormClosedEventHandler(form_Testing_deactivated);
            form_Testing.Show();
        }
        private void form_Testing_activated(object sender, EventArgs e)
        {
            this.Visible = false;// блокируем окно контрагентов пока открыто окно добавления контрагента
            aTimer2.Enabled = false;
        }
        private void form_Testing_deactivated(object sender, FormClosedEventArgs e)
        {
            this.Visible = true;// разблокируем окно контрагентов кактолько закрыто окно добавления контрагента
            aTimer2.Enabled = true;
        }

        private void label_status_ohoroni_Paint(object sender, PaintEventArgs e)
        {
           // label_status_ohoroni.BackColor = Color.Red;
        }

        private void textBox_test_search_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //// если все поля поиска пустыет - выводим полный перечень. Думаю это не рационально, может быть дохера записей...
                string sql = string.Format("SELECT Object_id_wl, concat(Object_name, ' (',Object_imei, ')') as name_id  FROM btk.Object where Object_name like '%" + textBox_test_search.Text.ToString() + "%'  or Object_imei like '%" + textBox_test_search.Text.ToString() + "%';");
                listBox_test_search_result.DataSource = macros.sql_request_dataTable(sql);
                listBox_test_search_result.DisplayMember = "name_id";
                listBox_test_search_result.ValueMember = "Object_id_wl";

                if (textBox_test_search.Text == "")
                {
                    listBox_test_search_result.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "textBox_filter_object_ohrani_TextChanged");
            }
        }

        private void listBox_test_search_result_DoubleClick(object sender, EventArgs e)
        {
            if (listBox_test_search_result.SelectedIndex <= -1)
            {
                MessageBox.Show("Необхідно вибрати об'єкт");
                return;
            }
            vars_form.test_id = listBox_test_search_result.SelectedValue.ToString();
        }

        private void checkBox_hide_groupe_alarm_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_hide_groupe_alarm.Checked == true)
            { 
                vars_form.hide_group_alarm = " and group_alarm is null";
            }
            else
            {
                vars_form.hide_group_alarm = "";
            }
        }

        private void build_testing_listwiev()
        {
            listView_testing.Columns.Clear();
            listView_testing.View = View.Tile;
            listView_testing.FullRowSelect = true;
            listView_testing.GridLines = true;
            listView_testing.View = View.Details;
            listView_testing.ShowItemToolTips = true;

            // Attach Subitems to the ListView
            listView_testing.Columns.Add("idtesting_object", 1, HorizontalAlignment.Left);
            listView_testing.Columns.Add("Продукт", -2, HorizontalAlignment.Left);
            listView_testing.Columns.Add("Об’єкт", -2, HorizontalAlignment.Left);
            listView_testing.Columns.Add("Дата", -2, HorizontalAlignment.Left);
            listView_testing.Columns.Add("Результат", -2, HorizontalAlignment.Left);
            //ПОлучим последний созданный айди тестирование, и возмем из него дату для верного отображения которую покладем в даттаймпикер
            string maxID = macros.sql_command2("SELECT MAX(idtesting_object) FROM btk.testing_object;");
            string t = macros.sql_command2("SELECT testing_objectcol_edit_timestamp FROM btk.testing_object where idtesting_object =" + maxID + ";");
            if (t =="")
            {
                t = DateTime.Now.ToString();
            }

            dateTimePicker_testing_date.Value= DateTime.Parse(t);
        }

        

        private void get_testing_data()
        {
            MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
            string sql = string.Format("select concat(Object.Object_name, '(' ,Object.Object_imei, ')') AS objectname_imei, Object.products_idproducts, testing_object.idtesting_object, testing_object.testing_objectcol_edit_timestamp, testing_objectcol_result FROM btk.Object join btk.testing_object on testing_object.Object_idObject = Object.idObject and testing_objectcol_edit_timestamp >= '" + dateTimePicker_testing_date.Value.ToString("yyyy-MM-dd") + "' and Object.Object_imei like '%" + textBox_search_testing.Text+ "%' and Object.Object_name like'%" + textBox_search_testing.Text + "%';");
            
            MySqlCommand myDataAdapter = new MySqlCommand(sql, myConnection);
            myConnection.Open();
            MySqlDataReader reader = myDataAdapter.ExecuteReader();

            listView_testing.Items.Clear();

            while (reader.Read())
            {
                var item = new ListViewItem();
                item.Text = reader["idtesting_object"].ToString();
                item.SubItems.Add(macros.sql_command2("SELECT product_name FROM btk.products where idproducts=" + reader["products_idproducts"].ToString() + ";"));
                item.SubItems.Add(reader["objectname_imei"].ToString());
                item.SubItems.Add(reader["testing_objectcol_edit_timestamp"].ToString());
                item.SubItems.Add(reader["testing_objectcol_result"].ToString());
                listView_testing.Items.Add(item);
            }
            reader.Close();
            myDataAdapter.Dispose();
            myConnection.Close();


            //listView_testing.View = View.Details;
        }

        private void dateTimePicker_testing_date_ValueChanged(object sender, EventArgs e)
        {
            get_testing_data();
        }

        private void textBox_search_testing_TextChanged(object sender, EventArgs e)
        {
            get_testing_data();
        }

        private void dataGridView_dilery_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            if (dataGridView_dilery.Rows[e.RowIndex].Cells[9].Value.ToString() == "")//Згруповано до ID тривоги(9)
            {
                vars_form.search_id = dataGridView_dilery.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_dilery.Rows[e.RowIndex].Cells[0].Value.ToString();//ID Тривоги (0)
                vars_form.id_status = dataGridView_dilery.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_dilery.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_dilery.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_dilery.Rows[e.RowIndex].Cells[11].Value.ToString();//Звернення(4)
            }
            else
            {

                vars_form.search_id = dataGridView_dilery.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_dilery.Rows[e.RowIndex].Cells[0].Value.ToString();//Згруповано до ID тривоги(9)
                vars_form.id_status = dataGridView_dilery.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_dilery.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_dilery.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_dilery.Rows[e.RowIndex].Cells[10].Value.ToString();//Звернення(4)
                //var dataIndexNo = dataGridView1.Rows[e.RowIndex].Index.ToString();
                //string id_notif = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                // MessageBox.Show(cellValue);
            }

            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
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
                if (vars_form.user_login_name != "admin" & vars_form.user_login_name != results[1])
                {
                    if (results[0] == "1")
                    {
                        MessageBox.Show("Користувач: " + results[1] + " вже опрацовуе тривогу.");
                        return;
                    }
                }
                reader.Close();
                myDataAdapter.Dispose();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "dataGridView_dilery_CellContentClick");
            }

            // Блокируем обработку тревоги однвременно двумя операторами, и вносим информацию кто открыл тревогу
            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                string sql = string.Format("UPDATE btk.notification SET alarm_locked = '1', alarm_locked_user = '" + vars_form.user_login_name + "' WHERE idnotification = '" + vars_form.id_notif + "';");
                MySqlCommand MyCommand2 = new MySqlCommand(sql, myConnection);
                MySqlDataReader MyReader2;
                myConnection.Open();
                MyReader2 = MyCommand2.ExecuteReader();
                MyCommand2.Dispose();
                myConnection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "detail_Load()");
            }

            detail subwindow = new detail();
            subwindow.Show();
        }

        private void dataGridView_sales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            if (dataGridView_sales.Rows[e.RowIndex].Cells[9].Value.ToString() == "")//Згруповано до ID тривоги(9)
            {
                vars_form.search_id = dataGridView_sales.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_sales.Rows[e.RowIndex].Cells[0].Value.ToString();//ID Тривоги (0)
                vars_form.id_status = dataGridView_sales.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_sales.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_sales.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_sales.Rows[e.RowIndex].Cells[10].Value.ToString();//Звернення(4)
            }
            else
            {

                vars_form.search_id = dataGridView_sales.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_sales.Rows[e.RowIndex].Cells[0].Value.ToString();//Згруповано до ID тривоги(9)
                vars_form.id_status = dataGridView_sales.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_sales.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_sales.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_sales.Rows[e.RowIndex].Cells[10].Value.ToString();//Звернення(4)
                //var dataIndexNo = dataGridView1.Rows[e.RowIndex].Index.ToString();
                //string id_notif = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                // MessageBox.Show(cellValue);
            }

            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
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
                if (vars_form.user_login_name != "admin" & vars_form.user_login_name != results[1])
                {
                    if (results[0] == "1")
                    {
                        MessageBox.Show("Користувач: " + results[1] + " вже опрацовуе тривогу.");
                        return;
                    }
                }
                reader.Close();
                myDataAdapter.Dispose();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "dataGridView_sales_CellContentClick");
            }

            // Блокируем обработку тревоги однвременно двумя операторами, и вносим информацию кто открыл тревогу
            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                string sql = string.Format("UPDATE btk.notification SET alarm_locked = '1', alarm_locked_user = '" + vars_form.user_login_name + "' WHERE idnotification = '" + vars_form.id_notif + "';");
                MySqlCommand MyCommand2 = new MySqlCommand(sql, myConnection);
                MySqlDataReader MyReader2;
                myConnection.Open();
                MyReader2 = MyCommand2.ExecuteReader();
                MyCommand2.Dispose();
                myConnection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "detail_Load()");
            }

            detail subwindow = new detail();
            subwindow.Show();
        }

        private void dataGridView_909_n_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            if (dataGridView_909_n.Rows[e.RowIndex].Cells[9].Value.ToString() == "")//Згруповано до ID тривоги(9)
            {
                vars_form.search_id = dataGridView_909_n.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_909_n.Rows[e.RowIndex].Cells[0].Value.ToString();//ID Тривоги (0)
                vars_form.id_status = dataGridView_909_n.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_909_n.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_909_n.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_909_n.Rows[e.RowIndex].Cells[11].Value.ToString();//Звернення(4)
            }
            else
            {

                vars_form.search_id = dataGridView_909_n.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_909_n.Rows[e.RowIndex].Cells[0].Value.ToString();//Згруповано до ID тривоги(9)
                vars_form.id_status = dataGridView_909_n.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_909_n.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_909_n.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_909_n.Rows[e.RowIndex].Cells[10].Value.ToString();//Звернення(4)
            }

            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
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
                if (vars_form.user_login_name != "admin" & vars_form.user_login_name != results[1])
                {
                    if (results[0] == "1")
                    {
                        MessageBox.Show("Користувач: " + results[1] + " вже опрацовуе тривогу.");
                        return;
                    }
                }
                reader.Close();
                myDataAdapter.Dispose();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "dataGridView_909_n");
            }

            // Блокируем обработку тревоги однвременно двумя операторами, и вносим информацию кто открыл тревогу
            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                string sql = string.Format("UPDATE btk.notification SET alarm_locked = '1', alarm_locked_user = '" + vars_form.user_login_name + "' WHERE idnotification = '" + vars_form.id_notif + "';");
                MySqlCommand MyCommand2 = new MySqlCommand(sql, myConnection);
                MySqlDataReader MyReader2;
                myConnection.Open();
                MyReader2 = MyCommand2.ExecuteReader();
                MyCommand2.Dispose();
                myConnection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "detail_Load()");
            }

            detail subwindow = new detail();
            subwindow.Show();
        }

        private void dataGridView_909_n_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dataGridView_909_n.SuspendLayout();

            if (dataGridView_909_n.Rows[e.RowIndex].Cells[11].Value is true)
            {
                if (Convert.ToDateTime(dataGridView_909_n.Rows[e.RowIndex].Cells[12].Value) == DateTime.Now.Date)
                {
                   e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#bcccf2");
                }
                else if (Convert.ToDateTime(dataGridView_909_n.Rows[e.RowIndex].Cells[12].Value) <= DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F9A780");
                }
                else if (Convert.ToDateTime(dataGridView_909_n.Rows[e.RowIndex].Cells[12].Value) >= DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#C8FAB7");
                }
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }

            dataGridView_909_n.Columns[10].Visible = false;
            dataGridView_909_n.Columns[8].Visible = false;
            dataGridView_909_n.Columns[4].Visible = false;
            dataGridView_909_n.ResumeLayout();
        }

        private void dataGridView_808_n_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            if (dataGridView_808_n.Rows[e.RowIndex].Cells[9].Value.ToString() == "")//Згруповано до ID тривоги(9)
            {
                vars_form.search_id = dataGridView_808_n.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_808_n.Rows[e.RowIndex].Cells[0].Value.ToString();//ID Тривоги (0)
                vars_form.id_status = dataGridView_808_n.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_808_n.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_808_n.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_808_n.Rows[e.RowIndex].Cells[11].Value.ToString();//Звернення(4)
            }
            else
            {

                vars_form.search_id = dataGridView_808_n.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_808_n.Rows[e.RowIndex].Cells[0].Value.ToString();//Згруповано до ID тривоги(9)
                vars_form.id_status = dataGridView_808_n.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_808_n.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_808_n.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_808_n.Rows[e.RowIndex].Cells[10].Value.ToString();//Звернення(4)
            }

            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
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
                if (vars_form.user_login_name != "admin" & vars_form.user_login_name != results[1])
                {
                    if (results[0] == "1")
                    {
                        MessageBox.Show("Користувач: " + results[1] + " вже опрацовуе тривогу.");
                        return;
                    }
                }
                reader.Close();
                myDataAdapter.Dispose();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "dataGridView_909_n");
            }

            // Блокируем обработку тревоги однвременно двумя операторами, и вносим информацию кто открыл тревогу
            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                string sql = string.Format("UPDATE btk.notification SET alarm_locked = '1', alarm_locked_user = '" + vars_form.user_login_name + "' WHERE idnotification = '" + vars_form.id_notif + "';");
                MySqlCommand MyCommand2 = new MySqlCommand(sql, myConnection);
                MySqlDataReader MyReader2;
                myConnection.Open();
                MyReader2 = MyCommand2.ExecuteReader();
                MyCommand2.Dispose();
                myConnection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "detail_Load()");
            }

            detail subwindow = new detail();
            subwindow.Show();
        }

        private void dataGridView_808_n_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dataGridView_808_n.SuspendLayout();

            if (dataGridView_808_n.Rows[e.RowIndex].Cells[11].Value is true)
            {
                if (Convert.ToDateTime(dataGridView_808_n.Rows[e.RowIndex].Cells[12].Value) == DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#bcccf2");
                }
                else if (Convert.ToDateTime(dataGridView_808_n.Rows[e.RowIndex].Cells[12].Value) <= DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F9A780");
                }
                else if (Convert.ToDateTime(dataGridView_808_n.Rows[e.RowIndex].Cells[12].Value) >= DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#C8FAB7");
                }
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }

            dataGridView_808_n.Columns[10].Visible = false;
            dataGridView_808_n.Columns[8].Visible = false;
            dataGridView_808_n.Columns[4].Visible = false;
            dataGridView_808_n.ResumeLayout();
        }

        private void dataGridView_sales_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dataGridView_sales.SuspendLayout();

            if (dataGridView_sales.Rows[e.RowIndex].Cells[11].Value is true)
            {
                if (Convert.ToDateTime(dataGridView_sales.Rows[e.RowIndex].Cells[12].Value) == DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#bcccf2");
                }
                else if (Convert.ToDateTime(dataGridView_sales.Rows[e.RowIndex].Cells[12].Value) <= DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F9A780");
                }
                else if (Convert.ToDateTime(dataGridView_sales.Rows[e.RowIndex].Cells[12].Value) >= DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#C8FAB7");
                }
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }

            dataGridView_sales.Columns[10].Visible = false;
            dataGridView_sales.Columns[8].Visible = false;
            dataGridView_sales.Columns[4].Visible = false;
            dataGridView_sales.ResumeLayout();
        }

        private void button_start_activation_Click(object sender, EventArgs e)
        {
            Activation_Form activation_form = new Activation_Form();
            activation_form.Activated += new EventHandler(activation_form_activated);
            activation_form.FormClosed += new FormClosedEventHandler(activation_form_deactivated);
            activation_form.Show();
        }

        private void activation_form_activated(object sender, EventArgs e)
        {
            this.Visible = false;// блокируем окно  пока открыто окно добавления 
            aTimer2.Enabled = false;
        }
        private void activation_form_deactivated(object sender, FormClosedEventArgs e)
        {
            this.Visible = true;// разблокируем окно  кактолько закрыто окно добавления 
            aTimer2.Enabled = true;
        }
    }

    internal class List_add_alarm
    {
        public List_add_alarm()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }

    public static class vars_form
    {
        public static string id_notif { get; set; }//айди уведомления
        public static string id_status { get; set; }
        public static int id_status_old { get; set; }
        public static string user_login_id{ get; set; }
        public static string search_id { get; set; }
        public static string user_login_name { get; set; }
        public static string user_login_email { get; set; }
        public static string unit_name { get; set; }
        public static bool restrict_un_group { get; set; }
        public static string alarm_name { get; set; }
        public static string add_alarm_unit_id { get; set; }
        public static string zvernenya { get; set; }
        public static string sort { get; set; }
        public static string sort_close { get; set; }
        public static string type_alarm_sort_close { get; set; }
        public static string order_sort { get; set; }
        public static string order_close_sort { get; set; }
        public static int setting_font_size{ get; set; }
        public static Pen c_color { get; set; }
        public static Brush b_color { get; set; }
        public static string test_id { get; set; }
        public static string hide_group_alarm { get; set; }
        public static string btk_idkontragents { get; set; }
        public static string id_object_for_test { get; set; }
        public static string id_db_object_for_test { get; set; }
        public static string eid { get; set; }//Храним eid ссесии для Wialon
        public static string wl_user_nm { get; set; }//Храним имя пользователя в Wialon который прошел авторизацию по токену
        public static int wl_user_id { get; set; }//Храним id пользователя в Wialon который прошел авторизацию по токену
        public static string user_token { get; set; }//Храним token авторизации для Wialon из БД
        public static object rootobject { get; set; }//


    }
    //public static class GraphicsExtensions
    //{
    //    public static void DrawCircle(this Graphics g, Pen pen,
    //                                  float centerX, float centerY, float radius)
    //    {
    //        g.DrawEllipse(pen, centerX - radius, centerY - radius,
    //                      radius + radius, radius + radius);
    //    }

    //    public static void FillCircle(this Graphics g, Brush brush,
    //                                  float centerX, float centerY, float radius)
    //    {
    //        g.FillEllipse(brush, centerX - radius, centerY - radius,
    //                      radius + radius, radius + radius);
    //    }
    //}
}
