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
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.IO;
using System.Timers;

namespace Disp_WinForm
{
    public partial class detail : Form
    {
        Macros macros = new Macros();
        private static System.Timers.Timer aTimer;
        private string _id_notif;
        private string _id_status;
        private string _user_login_id;
        private string _search_id;
        private string _unit_name;
        private bool _restrict_un_group;
        private string _alarm_name;
        private string _eid;
        public detail()
        {
            this.Font = new System.Drawing.Font("Arial", vars_form.setting_font_size);

            InitializeComponent();
            
            // Looad varible data
            _id_notif = vars_form.id_notif;
            _id_status = vars_form.id_status;
            _user_login_id = vars_form.user_login_id;
            _search_id = vars_form.search_id;
            _unit_name = vars_form.unit_name;
            _restrict_un_group = vars_form.restrict_un_group;
            _alarm_name = vars_form.alarm_name;
            _eid = vars_form.eid;

            get_remaynder();

            get_sensor_value();
            TreeView_zapolnyaem();
            TreeView_harakteristiki();
            mysql_get_hronologiya_trivog();
            mysql_get_group_alarm();
            comboBox_status_trevogi.Text =  _id_status;
            this.Text = "ID Тривоги: " + _id_notif + ", " + vars_form.unit_name + ", " + _alarm_name + ": " + vars_form.zvernenya ;
            SetTimer();
            arhiv_object();

        }

        private void get_remaynder()
        {
            

            string sql2 = string.Format("select remaynder_activate from btk.notification where idnotification=" + _id_notif + ";");
            string remaynder_activate = macros.sql_command2(sql2);
            if (remaynder_activate == "True")
            {
                remaynder_checkBox.Checked = true;
                string sql = string.Format("select remayder_date from btk.notification where idnotification=" + _id_notif + ";");
                string remayder_date = macros.sql_command2(sql);
                DateTime rem = Convert.ToDateTime(remayder_date);
                remaynder_dateTimePicker.Value = rem;
            }
        }

        private void SetTimer()
        {
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 5000;
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new System.Timers.ElapsedEventHandler(OnTimedEvent)
                    , new object[] { sender, e });

                return; // в этом побочном потоке ничего не делаем больше
            }

            get_sensor_value();
        }

        private void update_session()
        {
            try
            {
                MyWebRequest myRequest = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "&svc=token/login&params={\"token\":\"d1207c47958c32b224682b5b080fb908CAF3A4507360712CD18AE69617A54F2FC61EFF5D\"}");
                string json = myRequest.GetResponse();
                var m = JsonConvert.DeserializeObject<RootObject>(json);
                _eid = m.eid;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "update_session()");
            }
        }

        private void TreeView_harakteristiki()//Получаем произвольные поля из Виалона и заполняем дерево
        {
            try
            {
                MyWebRequest myRequest = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + _eid + "&svc=core/search_items&params={\"spec\":{" + "\"itemsType\":\"avl_unit\"," + "\"propName\":\"sys_id\"," + "\"propValueMask\":\"" + _search_id + "\", " + "\"sortType\":\"sys_name\"," + "\"or_logic\":\"1\"}," + "\"or_logic\":\"1\"," + "\"force\":\"1\"," + "\"flags\":\"15208907\"," + "\"from\":\"0\"," + "\"to\":\"5\"}");//15208907
                string json = myRequest.GetResponse();
                var m = JsonConvert.DeserializeObject<RootObject>(json);

                if (m.error == 1)
                {
                    update_session();
                    json = myRequest.GetResponse();
                    m = JsonConvert.DeserializeObject<RootObject>(json);
                }

                TreeNode node1 = new TreeNode("Характеристики:");
                treeView_haracteristiki.Nodes.Add(node1);
                treeView_haracteristiki.Nodes[0].Expand();
                

                foreach (var keyvalue in m.items[0].pflds)
                {
                    if (keyvalue.Value.n.Contains("vehicle_type"))
                    {
                        treeView_haracteristiki.Nodes[0].Nodes.Add(new TreeNode("Тип Т/З: " + keyvalue.Value.v.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("vin"))
                    {
                        treeView_haracteristiki.Nodes[0].Nodes.Add(new TreeNode("VIN: " + keyvalue.Value.v.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("registration_plate"))
                    {
                        treeView_haracteristiki.Nodes[0].Nodes.Add(new TreeNode("Державний номер: " + keyvalue.Value.v.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("brand"))
                    {
                        treeView_haracteristiki.Nodes[0].Nodes.Add(new TreeNode("Марка: " + keyvalue.Value.v.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("model"))
                    {
                        treeView_haracteristiki.Nodes[0].Nodes.Add(new TreeNode("Модель: " + keyvalue.Value.v.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("year"))
                    {
                        treeView_haracteristiki.Nodes[0].Nodes.Add(new TreeNode("Рік випуску: " + keyvalue.Value.v.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("color"))
                    {
                        treeView_haracteristiki.Nodes[0].Nodes.Add(new TreeNode("Колір: " + keyvalue.Value.v.ToString()));
                    }
                    //else if (keyvalue.Value.n.Contains("cargo_type"))
                    //{
                    //    treeView_haracteristiki.Nodes[0].Nodes.Add(new TreeNode(": " + keyvalue.Value.v.ToString()));
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "TreeView_harakteristiki()");
            }
        }

        private void TreeView_zapolnyaem()//Получаем произвольные поля из Виалона и заполняем дерево
        {
            try
            {
                
                MyWebRequest myRequest = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + _eid + "&svc=core/search_items&params={\"spec\":{" + "\"itemsType\":\"avl_unit\"," + "\"propName\":\"sys_id\"," + "\"propValueMask\":\"" + _search_id  + "\", " + "\"sortType\":\"sys_name\"," + "\"or_logic\":\"1\"}," + "\"or_logic\":\"1\"," + "\"force\":\"1\"," + "\"flags\":\"15208907\"," + "\"from\":\"0\"," + "\"to\":\"5\"}");//15208907
                string json = myRequest.GetResponse();
                var m = JsonConvert.DeserializeObject<RootObject>(json);

                if (m.error == 1)
                {
                    update_session();
                    json = myRequest.GetResponse();
                    m = JsonConvert.DeserializeObject<RootObject>(json);
                }

                if (m.items[0].flds.Count == 0)//Если поля Увага не существует блокируем кнопку изменения этого поля
                {
                    button_izmenit_uvaga.Enabled = false;
                    textBox_Uvaga.Text = "Поля увага не існує!";
                }
                if (m.items[0].flds.Count != 0) //Если первое поле не содержит Увага блокируем кнопку изменения этого поля
                {
                    if (!m.items[0].flds[1].n.Contains("УВАГА"))
                    {
                        button_izmenit_uvaga.Enabled = false;
                        textBox_Uvaga.Text = "Поля увага не існує!";
                    }
                }
                

                TreeNode node1 = new TreeNode("Об'єкт охорони:");
                treeView_client_info.Nodes.Add(node1);
                treeView_client_info.Nodes[0].Expand();
                TreeNode node2 = new TreeNode("Загальна інформація");
                treeView_client_info.Nodes.Add(node2);
                treeView_client_info.Nodes[1].Expand();
                TreeNode node3 = new TreeNode("Адміністративні поля");
                treeView_client_info.Nodes.Add(node3);
                treeView_client_info.Nodes[2].Expand();

                foreach (var keyvalue in m.items[0].flds)
                {

                    if (keyvalue.Value.n.Contains("УВАГА") & !keyvalue.Value.n.Contains("алгоритм"))
                    {
                        treeView_client_info.Nodes[0].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                        textBox_Uvaga.Text = keyvalue.Value.v.ToString();
                        treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("Назва об'єкту:" + m.items[0].nm.ToString()));
                        treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("IMEI:" + m.items[0].uid.ToString()));
                        treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("SIM:" + m.items[0].ph.ToString()));
                    }
                    treeView_client_info.Nodes[1].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                    if (keyvalue.Value.n.Contains("Кодов"))
                    {
                        treeView_client_info.Nodes[0].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                    }
                }

                foreach (var keyvalue in m.items[0].aflds)
                {
                    treeView_client_info.Nodes[2].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "TreeView_zapolnyaem()");
            }
        }

        private void mysql_get_group_alarm()//Получаем групированые тривоги для заполнения таблицы
        {
            if (_restrict_un_group == false)
            {
                try
                {
                    MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                    string sql = string.Format("SELECT idnotification, unit_id, unit_name, type_alarm, product, curr_time, msg_time, group_alarm, Status FROM btk.notification WHERE idnotification <> '" + _id_notif + "' AND unit_id = '" + _search_id + "' AND (Status = 'Відкрито' OR Status = 'Обробляется')");
                    MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(sql, myConnection);

                    //Для прорисовки в listView из запроса к БД вместо табличного представления, пока не хочу менять.
                    //DataTable ds = new DataTable();
                    //myDataAdapter.Fill(ds);
                    //foreach (DataRow row in ds.Rows)
                    //{
                    //    ListViewItem item = new ListViewItem(row[0].ToString());
                    //    listView1.View = View.Details;
                    //    for (int i = 1; i < ds.Columns.Count; i++)
                    //    {
                    //        item.SubItems.Add(row[i].ToString());
                    //    }
                    //    listView1.Items.Add(item);
                    //}

                    DataSet ds = new DataSet();
                    myDataAdapter.Fill(ds);
                    dataGridView_group_alarm.AutoGenerateColumns = false;
                    dataGridView_group_alarm.DataSource = ds.Tables[0];

                    myDataAdapter.Dispose();
                    myConnection.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString() + "mysql_get_group_alarm()");
                }//Если тревога открыта
            }
            else
            {
                try
                {
                    MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                    string sql = string.Format("SELECT idnotification, unit_id, unit_name, type_alarm, product, curr_time, msg_time, group_alarm, Status FROM btk.notification WHERE idnotification <> '" + _id_notif + "' AND unit_id = '" + _search_id + "' AND group_alarm = '" + _id_notif + "'");
                    MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(sql, myConnection);
                    DataSet ds = new DataSet();
                    myDataAdapter.Fill(ds);
                    dataGridView_group_alarm.AutoGenerateColumns = false;
                    dataGridView_group_alarm.DataSource = ds.Tables[0];
                    myDataAdapter.Dispose();
                    myConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString() + "mysql_get_group_alarm()");
                }//Если тревога закрыта
                //деактевируем кнопки группировки
                button_group_alarm.Enabled = false;
                button_ungroup_alarm.Enabled = false;
            }
            
        }

        private void mysql_get_hronologiya_trivog()//Получаем изаполняем таблицу хронологии обработки открытой тревоги
        {
            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                string sql = string.Format("SELECT alarm_text, time_start_ack, btk.alarm_ack.time_stamp, btk.Users.username, current_status_alarm, vizov_police, vizov_gmp FROM btk.alarm_ack, btk.Users where notification_idnotification='" + _id_notif + "' and users_chenge = idUsers");
                MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(sql, myConnection);
                DataSet ds = new DataSet();
                myDataAdapter.Fill(ds);
                dataGridView_hronologija_trivog.AutoGenerateColumns = false;
                dataGridView_hronologija_trivog.RowHeadersVisible = false;
                dataGridView_hronologija_trivog.DataSource = ds.Tables[0];
                myDataAdapter.Dispose();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "mysql_get_hronologiya_trivog()");
            }
        }

        private void button_vnesti_zapis_Click(object sender, EventArgs e)//Вносим запись в хронологию обработки тревоги
        {

            if (textBox_otrabotka_trevogi.Text == "")//Запись в хронологию обработки не может быть внесена без комментария к действию
            {
                MessageBox.Show("Внеси запис про дію обробки тривоги");
                return;
            }
            try
            {
                int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                int police = checkBox_vizov_police.Checked ? 1 : 0;
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                string sql = string.Format("insert into btk.alarm_ack(alarm_text, notification_idnotification, Users_chenge, time_start_ack, current_status_alarm, vizov_police, vizov_gmp) values('" + textBox_otrabotka_trevogi.Text + "', '" + _id_notif + "','" + _user_login_id + "', '" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "', '" + comboBox_status_trevogi.Text + "', '" + police + "', '" + gmr + "'); UPDATE btk.notification SET Status = '" + comboBox_status_trevogi.Text + "', time_stamp = now(), Users_idUsers='" + _user_login_id + "' WHERE idnotification = '" + _id_notif + "'OR group_alarm = '" + _id_notif + "'; ");
                MySqlCommand MyCommand2 = new MySqlCommand(sql, myConnection);
                MySqlDataReader MyReader2;
                myConnection.Open();
                MyReader2 = MyCommand2.ExecuteReader();
                MyCommand2.Dispose();
                myConnection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "button_vnesti_zapis_Click()");
            }
            mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог

            textBox_otrabotka_trevogi.Text = "";

            if (comboBox_status_trevogi.SelectedItem.ToString() == "909" & ( _id_status == "Обробляется" ||  _id_status == "Відкрито"))//если изменяем статус с Обробляется на 909  - отправляем меил со всейхронологией обработки тревоги
            {
                string recipient = "<" + vars_form.user_login_email + ">,"+ "<o.pustovit@venbest.com.ua>,<d.yacenko@venbest.com.ua>,<a.oberemchuk@venbest.com.ua>,<d.lenik@venbest.com.ua>,<mc@venbest.com.ua>,<v.bogoley@venbest.com.ua>";
                send_email(recipient); 
            }

            if (comboBox_status_trevogi.SelectedItem.ToString() == "808" & ( _id_status == "Обробляется" ||  _id_status == "Відкрито" || _id_status == "Продажи" || _id_status == "Дилеры"))//если изменяем статус с Обробляется на 808  - отправляем меил со всейхронологией обработки тревоги
            {
                string recipient = "<" + vars_form.user_login_email + ">," + "<o.pustovit@venbest.com.ua>,<d.lenik@venbest.com.ua>,<mc@venbest.com.ua>,<e.remekh@venbest.com.ua>";
                send_email(recipient);
            }
            if (comboBox_status_trevogi.SelectedItem.ToString() == "Продажи" & (_id_status == "Обробляется" || _id_status == "Відкрито" || _id_status == "808" || _id_status == "Дилеры"))//если изменяем статус с Обробляется на 808  - отправляем меил со всейхронологией обработки тревоги
            {
                string recipient = "<" + vars_form.user_login_email + ">," + "<a.lozinskiy@venbest.com.ua>";
                send_email(recipient);
            }
            if (comboBox_status_trevogi.SelectedItem.ToString() == "Дилеры" & (_id_status == "Обробляется" || _id_status == "Відкрито" || _id_status == "808" || _id_status == "Продажи"))//если изменяем статус с Обробляется на 808  - отправляем меил со всейхронологией обработки тревоги
            {
                string recipient = "<" + vars_form.user_login_email + ">," + "<e.danilchenko@venbest.com.ua>,<a.andreasyan@venbest.com.ua>";
                send_email(recipient);
            }

            if (checkBox_send_email.Checked == true)//если отмечено Надислаты звит - отправляем меил со всейхронологией обработки тревоги
            {
                string recipient = "<" + vars_form.user_login_email + ">," + "<a.lozinskiy@venbest.com.ua>,<o.pustovit@venbest.com.ua>,<d.yacenko@venbest.com.ua>,<a.oberemchuk@venbest.com.ua>,<e.danilchenko@venbest.com.ua>, <d.lenik@venbest.com.ua>,<a.andreasyan@venbest.com.ua>,<mc@venbest.com.ua>";
                send_email(recipient);
            }

            _id_status = comboBox_status_trevogi.Text;
            checkBox_send_email.Checked = false;

             

            if (comboBox_status_trevogi.SelectedItem.ToString() == "Закрито")
            {
                aTimer.Enabled = false;
                this.Close();
            }

            if (remaynder_checkBox.Checked == true)
            {
                string sql = string.Format("UPDATE btk.notification SET remayder_date ='"+ Convert.ToDateTime(remaynder_dateTimePicker.Value).ToString("yyyy-MM-dd") + "', remaynder_activate= 1 where idnotification="+_id_notif+";");
                macros.sql_command(sql);
            }
            if (remaynder_checkBox.Checked == false)
            {
                string sql = string.Format("UPDATE btk.notification SET remayder_date = null, remaynder_activate= 0 where idnotification=" + _id_notif + ";");
                macros.sql_command(sql);
            }
        }

        private void detail_Load(object sender, EventArgs e)//При открытии тревоги отрисовуем объект по открытой тревоге на карту с текущим местоположением 
        {
            //start mini map
            webBrowser1.Navigate("http://10.44.30.32/disp_app/HTMLPage_map.html?foo=" + _search_id);
            //webBrowser1.ScriptErrorsSuppressed = true;
            //webBrowser1.Size = webBrowser1.Document.Body.ScrollRectangle.Size;

            

            //if opened start detail - automaticly chenge status alarm in combobox = Обробляется
            comboBox_status_trevogi.SelectedIndex = comboBox_status_trevogi.FindStringExact("Обробляется");
        }

        private void button_group_alarm_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView_group_alarm.SelectedRows)
            {
                string value1 = row.Cells[0].Value.ToString();

                try
                {
                    MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                    string sql = string.Format("UPDATE btk.notification SET group_alarm = '" + _id_notif + "', time_stamp = '"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE idnotification = '" + value1 + "'; ");
                    MySqlCommand MyCommand2 = new MySqlCommand(sql, myConnection);
                    MySqlDataReader MyReader2;
                    myConnection.Open();
                    MyReader2 = MyCommand2.ExecuteReader();
                    MyCommand2.Dispose();
                    myConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString() + "button_group_alarm_Click()");
                }
            }//указываем к какой тревоге сгрупированы выделенные тревоги

            try//вносим в хронологию обработки тревоги кто и когда сгрупироввал
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                string sql2 = string.Format("insert into btk.alarm_ack(alarm_text, notification_idnotification, Users_chenge, time_start_ack, time_end_ack) values('Виконано групуваня', '" + _id_notif + "', '" + _user_login_id + "', '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "', '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "')");
                MySqlCommand MyCommand2 = new MySqlCommand(sql2, myConnection);
                MySqlDataReader MyReader2;
                myConnection.Open();
                MyReader2 = MyCommand2.ExecuteReader();
                MyCommand2.Dispose();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "button_group_alarm_Click()");
            }
            mysql_get_group_alarm();//обновляем таблицу группированых тревог
            mysql_get_hronologiya_trivog();//обновляем таблицу хронология обработаных тревог
        }//Группируем выделенные тревоги как одну

        private void button_ungroup_alarm_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView_group_alarm.SelectedRows)
            {
                string value1 = row.Cells[0].Value.ToString();

                try
                {
                    MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                    string sql = string.Format("UPDATE btk.notification SET group_alarm = null, time_stamp = now() WHERE idnotification = '" + value1 + "';");
                    MySqlCommand MyCommand2 = new MySqlCommand(sql, myConnection);
                    MySqlDataReader MyReader2;
                    myConnection.Open();
                    MyReader2 = MyCommand2.ExecuteReader();
                    MyCommand2.Dispose();
                    myConnection.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString()+ "button_ungroup_alarm_Click");
                }
            }

            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                string sql2 = string.Format("insert into btk.alarm_ack(alarm_text, notification_idnotification, Users_chenge, time_start_ack, time_end_ack) values('Виконано розгрупуваня', '" + _id_notif + "', '" + _user_login_id + "', '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "', '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "')");
                MySqlCommand MyCommand2 = new MySqlCommand(sql2, myConnection);
                MySqlDataReader MyReader2;
                myConnection.Open();
                MyReader2 = MyCommand2.ExecuteReader();
                MyCommand2.Dispose();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "button_ungroup_alarm_Click");
            }//убераем указатель к какой тревоге сгрупированы выделенные тревоги
            mysql_get_group_alarm();//обновляем талицу
            mysql_get_hronologiya_trivog();//обновляем талицу
        }//Разгруппируем выделенные тревоги как одну

        private void dataGridView_group_alarm_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView_group_alarm.Rows[e.RowIndex].Cells[5].Value.ToString() != "")
            {
                e.CellStyle.BackColor = Color.Gray;
            }
        }//Группированые строки в таблице отмечаем сервм цветом

        private void detail_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Unloking opened alarm by user who used
            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                string sql = string.Format("UPDATE btk.notification SET alarm_locked = '0', alarm_locked_user = null WHERE idnotification = '" + _id_notif + "';");
                MySqlCommand MyCommand2 = new MySqlCommand(sql, myConnection);
                MySqlDataReader MyReader2;
                myConnection.Open();
                MyReader2 = MyCommand2.ExecuteReader();
                aTimer.Enabled=false;
                MyCommand2.Dispose();
                myConnection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "detail_FormClosing");
            }
        }//если форма закрывается оператором - снимаем блокировку одновременного открытия

        private void button_cancel_edit_Click(object sender, EventArgs e)
        {
            if (textBox_otrabotka_trevogi.Text != "")
            {
                DialogResult result = MessageBox.Show("Не збережена інформація буде втрачена?", "Підтвердіть", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    aTimer.Enabled = false;
                    this.Close();
                }
                else if (result == DialogResult.No)
                {
                    return;
                }
            }
            aTimer.Enabled = false;
            this.Close();
        }//если внесена информация и не сохранена, предупреждаем оператора перед выходом

        private void send_email(string recipient)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.venbest.com.ua");

                mail.From = new MailAddress("noreply@venbest.com.ua");
                mail.To.Add(recipient);
                if (checkBox_send_email.Checked == true & checkBox_vizov_gmr.Checked == false & checkBox_vizov_police.Checked == false)
                {
                    mail.Subject = "Звіт: " + comboBox_status_trevogi.SelectedItem.ToString() + ", Обєкт: " + _unit_name + ", Тривога: " + _alarm_name + " Тривога ID: " + _id_notif;
                }
                else if (checkBox_send_email.Checked == true & checkBox_vizov_gmr.Checked == true & checkBox_vizov_police.Checked == false)
                {
                    mail.Subject = "Звіт, ГМР: " + comboBox_status_trevogi.SelectedItem.ToString() + ", Обєкт: " + _unit_name + ", Тривога: " + _alarm_name + " Тривога ID: " + _id_notif;
                }
                else if (checkBox_send_email.Checked == true & checkBox_vizov_gmr.Checked == false & checkBox_vizov_police.Checked == true)
                {
                    mail.Subject = "Звіт, Поліція-ГМР: " + comboBox_status_trevogi.SelectedItem.ToString() + ", Обєкт: " + _unit_name + ", Тривога: " + _alarm_name + " Тривога ID: " + _id_notif;
                }
                else
                {
                    mail.Subject = "Статус: " + comboBox_status_trevogi.SelectedItem.ToString() + ", Обєкт: " + _unit_name + ", Тривога: " + _alarm_name + " Тривога ID: " + _id_notif;
                }
                
                mail.IsBodyHtml = true;
                mail.Body = htmlMessageBody(dataGridView_hronologija_trivog).ToString();

                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("noreply@venbest.com.ua", "L2TPr6");
                SmtpServer.EnableSsl = false;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "send_email()");
            }
        }//Отправляем имеил

        private StringBuilder htmlMessageBody(DataGridView dg)
        {
            StringBuilder strB = new StringBuilder();
            //create html & table
            strB.AppendLine("<html><body><left><" +
                            "table border='1' cellpadding='0' cellspacing='0'>");
            strB.AppendLine("<tr>");
            //cteate table header
            for (int i = 0; i < dg.Columns.Count; i++)
            {
                strB.AppendLine("<td align='center' valign='middle'>" +
                                dg.Columns[i].HeaderText + "</td>");
            }
            //create table body
            strB.AppendLine("<tr>");
            for (int i = 0; i < dg.Rows.Count; i++)
            {
                strB.AppendLine("<tr>");
                foreach (DataGridViewCell dgvc in dg.Rows[i].Cells)
                {
                    strB.AppendLine("<td align='center' valign='middle'>" +
                                    dgvc.Value.ToString() + "</td>");
                }
                strB.AppendLine("</tr>");

            }
            //table footer & end of html file
            strB.AppendLine("</table></center></body></html>");
            return strB;
        } //Готовим таблицу для оправки

        private void get_sensor_value()
        {
            try
            {
                MyWebRequest myRequest = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + _eid + "&svc=unit/calc_last_message&params={\"unitId\":\"" + _search_id + "\"," + "\"sensors\":\"\"," + "\"flags\":\"\"}");//получаем датчики объекта
                string json = myRequest.GetResponse();
                MyWebRequest myRequest2 = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + _eid + "&svc=core/search_items&params={\"spec\":{" + "\"itemsType\":\"avl_unit\"," + "\"propName\":\"sys_id\"," + "\"propValueMask\":\"" + _search_id + "\", " + "\"sortType\":\"sys_name\"," + "\"or_logic\":\"1\"}," + "\"or_logic\":\"1\"," + "\"force\":\"1\"," + "\"flags\":\"6820354\"," + "\"from\":\"0\"," + "\"to\":\"5\"}");//получаем текущее местоположение объекта
                string json2 = myRequest2.GetResponse();

                var m = JsonConvert.DeserializeObject<RootObject>(json2);

                if (m.error == 1)
                {
                    update_session();

                    myRequest = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + _eid + "&svc=unit/calc_last_message&params={\"unitId\":\"" + _search_id + "\"," + "\"sensors\":\"\"," + "\"flags\":\"\"}");//получаем датчики объекта
                    json = myRequest.GetResponse();
                    myRequest2 = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + _eid + "&svc=core/search_items&params={\"spec\":{" + "\"itemsType\":\"avl_unit\"," + "\"propName\":\"sys_id\"," + "\"propValueMask\":\"" + _search_id + "\", " + "\"sortType\":\"sys_name\"," + "\"or_logic\":\"1\"}," + "\"or_logic\":\"1\"," + "\"force\":\"1\"," + "\"flags\":\"6820354\"," + "\"from\":\"0\"," + "\"to\":\"5\"}");//получаем текущее местоположение объекта
                    json2 = myRequest2.GetResponse();

                    m = JsonConvert.DeserializeObject<RootObject>(json2);
                }

                var lat = m.items[0].pos["y"];
                var lon = m.items[0].pos["x"];

                listBox_commads_wl.Items.Clear();
                if (m.items[0].cmds != null)
                {
                    foreach (var cmd in m.items[0].cmds)
                    {
                        if (cmd.n.ToString() == "__app__chatterbox_coords_vrt")// в месте с командами выгружаются какието __app__chatterbox_coords_vrt . если попадается, отсекаем!
                        {
                            break;
                        }
                        listBox_commads_wl.Items.Add(cmd.n.ToString());
                    }
                }
                

                dynamic stuff = JsonConvert.DeserializeObject(json);

                if (m.items[0].netconn == 1)
                { checkBox_active.Checked = true; }
                else
                { checkBox_active.Checked = false; }

                 

                foreach (var keyvalue in m.items[0].sens)
                {
                    if (keyvalue.Value.n.Contains("Блокировка иммобилайзера"))
                        if (stuff[keyvalue.Value.id.ToString()] == "1")
                        { label_relay_bl.Text = "Блокування: Увімкнуто"; }
                        else { label_relay_bl.Text = "Блокування: Вимкнуто"; }

                    else if (keyvalue.Value.n.Contains("Датчик удара"))
                        if (stuff[keyvalue.Value.id.ToString()] == "1")
                        { label_udar.Text = "Датчик удару: Увімкнуто"; }
                        else { label_udar.Text = "Датчик удару: Вимкнуто"; }

                    else if (keyvalue.Value.n.Contains("Напряжение АКБ"))
                        label_akb.Text = "Напруга АКБ: " + stuff[keyvalue.Value.id.ToString()];

                    else if (keyvalue.Value.n.Contains("Статус дверей"))
                        if (stuff[keyvalue.Value.id.ToString()] == "1")
                        { label_door.Text = "Двері: Відкрито"; }
                        else { label_door.Text = "Двері: Закрито"; }

                    else if (keyvalue.Value.n.Contains("Статус охраны"))
                        if (stuff[keyvalue.Value.id.ToString()] == "1")
                        { label_arm.Text = "Статус охорони: В охороні"; }
                        else { label_arm.Text = "Статус охорони: Не в охороні"; }

                    else if (keyvalue.Value.n.Contains("Качество связи GSM"))
                        label_gsm.Text = "Якість GSM: " + stuff[keyvalue.Value.id.ToString()] + "%";

                    else if (keyvalue.Value.n.Contains("Сработка сигнализации: двери"))
                        if (stuff[keyvalue.Value.id.ToString()] == "1")
                        { label_vzlom.Text = "Датчик взлому: Так"; }
                        else { label_vzlom.Text = "Датчик взлому: Ні"; }

                    else if (keyvalue.Value.n.Contains("Тревожная кнопка"))
                        if (stuff[keyvalue.Value.id.ToString()] == "1")
                        { label_TK.Text = "Тревожна кнопка: Так"; }
                        else { label_TK.Text = "Тревожна кнопка: Ні"; }

                    else if (keyvalue.Value.n.Contains("Заряд АКБ"))
                        label_akb.Text = "Заряд АКБ: " + stuff[keyvalue.Value.id.ToString()] + "%";

                    else if (keyvalue.Value.n.Contains("Зажигание"))
                        if (stuff[keyvalue.Value.id.ToString()] == "1")
                        { label_ignition.Text = "Запалювання: ввімкнуто"; }
                        else { label_ignition.Text = "10. Запалювання: вимкнуто"; }

                    else if (keyvalue.Value.n.Contains("ГЛУШЕНИЕ GSM (ОХРАНА)"))
                        if (stuff[keyvalue.Value.id.ToString()] == "1")
                        { label_glushenie.Text = "Глушіння GSM: ввімкнуто"; }
                        else { label_glushenie.Text = "Глушіння GSM: вимкнуто"; }

                    
                }

                MyWebRequest myRequest3 = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + _eid + "&svc=resource/get_zones_by_point&params={\"spec\":{" + "\"zoneId\":{" + "\"28\":[],\"" + "241\":[] }" + "," + "\"lat\":" + lat + "," + "\"lon\":" + lon + "}}");
                string json3 = myRequest3.GetResponse();//получаем id геозон в которых находится объект, ресурс res_service id=28
                var geozone = JsonConvert.DeserializeObject<Dictionary<int, List<dynamic>>>(json3);
                var array_geozone = geozone.Values.ToArray();//по другому не вышло, не могу достать из лист значения, перевем в аррай
                var array_geozone_keys = geozone.Keys.ToArray();

                listBox_geozone_wl.Items.Clear();

                if (array_geozone.Length > 0)
                {

                    if (array_geozone[0].Count > 0)
                    {
                        foreach (var keyvalue_ in array_geozone[0])//для каждого айди геозон найдем имя и заполним лист формы геозон
                        {
                            if (array_geozone_keys[0].ToString() == "28")
                            {
                                MyWebRequest myRequest4 = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + _eid + "&svc=resource/get_zone_data&params={" + "\"itemId\":\"28\"," + "\"col\":[" + keyvalue_ + "]," + "\"flags\":\"16\"}");//
                                string json4 = myRequest4.GetResponse();//
                                var x = JsonConvert.DeserializeObject<List<Value_>>(json4);
                                listBox_geozone_wl.Items.Add(x[0].n.ToString());
                            }
                            else if (array_geozone_keys[0].ToString() == "241")
                            {
                                MyWebRequest myRequest4 = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + _eid + "&svc=resource/get_zone_data&params={" + "\"itemId\":\"241\"," + "\"col\":[" + keyvalue_ + "]," + "\"flags\":\"16\"}");//
                                string json4 = myRequest4.GetResponse();//
                                var x = JsonConvert.DeserializeObject<List<Value_>>(json4);
                                listBox_geozone_wl.Items.Add(x[0].n.ToString());
                            }

                        }
                    }

                    if (array_geozone.Length > 1)
                    {
                        if (array_geozone_keys[1].ToString() == "241")
                        {
                            foreach (var keyvalue_ in array_geozone[1])//для каждого айди геозон найдем имя и заполним лист формы геозон
                            {
                                MyWebRequest myRequest4 = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + _eid + "&svc=resource/get_zone_data&params={" + "\"itemId\":\"241\"," + "\"col\":[" + keyvalue_ + "]," + "\"flags\":\"16\"}");//
                                string json4 = myRequest4.GetResponse();//
                                var x = JsonConvert.DeserializeObject<List<Value_>>(json4);
                                listBox_geozone_wl.Items.Add(x[0].n.ToString());
                            }
                        }
                        if (array_geozone_keys[1].ToString() == "28")
                        {
                            foreach (var keyvalue_ in array_geozone[1])//для каждого айди геозон найдем имя и заполним лист формы геозон
                            {
                                MyWebRequest myRequest4 = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + _eid + "&svc=resource/get_zone_data&params={" + "\"itemId\":\"28\"," + "\"col\":[" + keyvalue_ + "]," + "\"flags\":\"16\"}");//
                                string json4 = myRequest4.GetResponse();//
                                var x = JsonConvert.DeserializeObject<List<Value_>>(json4);
                                listBox_geozone_wl.Items.Add(x[0].n.ToString());
                            }
                        }
                    }

                }                    
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "get_sensor_value()");
            }
        }//получаем текущие значения датчиков, нахождение в ГЕО-зонах объекта

        private void label_akb_Click(object sender, EventArgs e)
        {
            get_sensor_value();
        }

        private void label_arm_Click(object sender, EventArgs e)
        {
            get_sensor_value();
        }

        private void label_door_Click(object sender, EventArgs e)
        {
            get_sensor_value();
        }

        private void label_udar_Click(object sender, EventArgs e)
        {
            get_sensor_value();
        }

        private void label_relay_bl_Click(object sender, EventArgs e)
        {
            get_sensor_value();
        }

        private void button_send_cmd_wl_Click(object sender, EventArgs e)
        {
            if (listBox_commads_wl.SelectedItem == null)
            {
                MessageBox.Show("Виберіть команду");
                return;
            }
            DialogResult result = MessageBox.Show("Відправляється команда: " + listBox_commads_wl.SelectedItem.ToString(), "Підтвердіть", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    update_session();
                    MyWebRequest myRequest2 = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + _eid + "&svc=unit/exec_cmd&params={\"itemId\":\"" + _search_id + "\"," + "\"commandName\":\"" + listBox_commads_wl.SelectedItem.ToString() + "\"," + "\"linkType\":\"tcp\"," + "\"param\":\"\"," + "\"timeout\":\"0\"," + "\"flags\":\"0\"}");
                    string json2 = myRequest2.GetResponse();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString() + "button_send_cmd_wl_Click()");
                }

                try
                {
                    MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                    string sql2 = string.Format("insert into btk.alarm_ack(alarm_text, notification_idnotification, Users_chenge, time_start_ack, time_end_ack) values('Відправлено команду: " + listBox_commads_wl.SelectedItem.ToString() + "', '" + _id_notif + "', '" + _user_login_id + "', '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "', '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "')");
                    MySqlCommand MyCommand2 = new MySqlCommand(sql2, myConnection);
                    MySqlDataReader MyReader2;
                    myConnection.Open();
                    MyReader2 = MyCommand2.ExecuteReader();
                    MyCommand2.Dispose();
                    myConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString() + "button_send_cmd_wl_Click()");
                }
                mysql_get_hronologiya_trivog();
            }
            else if (result == DialogResult.No)
            {
                return;
            }
        }//Говорим виалону отправить выбранную комманду на объект


        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "template.txt");
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string[] columnnames = file.ReadLine().Split(' ');
            DataTable dt = new DataTable();
            foreach (string c in columnnames)
            {
                dt.Columns.Add(c);
            }
            string newline;
            while ((newline = file.ReadLine()) != null)
            {
                DataRow dr = dt.NewRow();
                string[] values = newline.Split('&');
                for (int i = 0; i < values.Length; i++)
                {
                    dr[i] = values[i];
                }
                dt.Rows.Add(dr);
            }
            file.Close();

            contextMenuStrip1.Items.Clear();

            foreach (DataRow row in dt.Rows)//создаем и заполняем kонтекстное меню данными из template.txt
            {
                ToolStripMenuItem submenu;

                submenu = new ToolStripMenuItem();
                submenu.Text = row[1].ToString();
                
                contextMenuStrip1.Items.Add(submenu);
            }
            contextMenuStrip1.Items.Add(new ToolStripSeparator());
            contextMenuStrip1.Items.Add("Копіювати");
            contextMenuStrip1.Items.Add("Вставити");


        }//читаем при нажатии правой клавиши по окну обрабоки тревог файл template.txt и строим из него контекстное меню


        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text== "Копіювати") {
                Clipboard.SetText(textBox_otrabotka_trevogi.SelectedText.ToString());
            }

            if (e.ClickedItem.Text == "Вставити") {
                textBox_otrabotka_trevogi.Text += Clipboard.GetText();
            }

            if (e.ClickedItem.Text != "Вставити" || e.ClickedItem.Text == "Копіювати")
            {textBox_otrabotka_trevogi.Text += e.ClickedItem.Text;}
            
        }//При нажати объекта из контекстного меню вставляется текст в окно обраюотки тревог

        private void button_sinhronize_time_Click(object sender, EventArgs e)
        {
            dateTimePicker_nachalo_dejstvia.Value = DateTime.Now;
            dateTimePicker_nachalo_dejstvia_data.Value = DateTime.Now;
        }

        private void button_izmenit_uvaga_Click(object sender, EventArgs e)
        {
            
            MyWebRequest myRequest = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + _eid + "&svc=core/search_items&params={\"spec\":{" + "\"itemsType\":\"avl_unit\"," + "\"propName\":\"sys_id\"," + "\"propValueMask\":\""  + _search_id + "\", " + "\"sortType\":\"sys_name\"," + "\"or_logic\":\"1\"}," + "\"or_logic\":\"1\"," + "\"force\":\"1\"," + "\"flags\":\"15208907\"," + "\"from\":\"0\"," + "\"to\":\"5\"}");//15208907
            string json = myRequest.GetResponse();
            var m = JsonConvert.DeserializeObject<RootObject>(json);

            if (m.error == 1)
            {
                update_session();
                json = myRequest.GetResponse();
                m = JsonConvert.DeserializeObject<RootObject>(json);
            }

            MyWebRequest myRequest2 = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + _eid + "&svc=item/update_custom_field&params={\"itemId\":\"" + _search_id + "\"," + "\"id\":\"1\"," + "\"callMode\":\"update\",\"n\":\"" + m.items[0].flds[1].n + "\"," + "\"v\":\"" + textBox_Uvaga.Text + "\"}");//получаем датчики объекта
            string json2 = myRequest2.GetResponse();
            MessageBox.Show("Поле Увага змінено!", "Повідомлення");

        }


        private void textBox_otrabotka_trevogi_MouseDown(object sender, MouseEventArgs e)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "template.txt");
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string[] columnnames = file.ReadLine().Split(' ');
            DataTable dt = new DataTable();
            foreach (string c in columnnames)
            {
                dt.Columns.Add(c);
            }
            string newline;
            while ((newline = file.ReadLine()) != null)
            {
                DataRow dr = dt.NewRow();
                string[] values = newline.Split('&');
                for (int i = 0; i < values.Length; i++)
                {
                    dr[i] = values[i];
                }
                dt.Rows.Add(dr);
            }
            file.Close();

            contextMenuStrip1.Items.Clear();

            foreach (DataRow row in dt.Rows)//создаем и заполняем kонтекстное меню данными из template.txt
            {
                ToolStripMenuItem submenu;

                submenu = new ToolStripMenuItem();
                submenu.Text = row[1].ToString();

                contextMenuStrip1.Items.Add(submenu);
            }
        }

        private void arhiv_object()
        {
            try
            {
            MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
            string sql = string.Format("SELECT idnotification, type_alarm, msg_time, Status, last_location FROM btk.notification where unit_id = " + _search_id + " and group_alarm is null");
            MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(sql, myConnection);

            DataSet ds = new DataSet();
            myDataAdapter.Fill(ds);
            dataGridView_trivogi_objecta.AutoGenerateColumns = false;
            dataGridView_trivogi_objecta.DataSource = ds.Tables[0];
            myDataAdapter.Dispose();
            myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "arhiv_object()");
            }//
        }

        private void dataGridView_trivogi_objecta_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            var temp = dataGridView_trivogi_objecta.Rows[e.RowIndex].Cells[0].Value.ToString();

            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                string sql = string.Format("SELECT alarm_text, vizov_gmp, vizov_police, time_stamp, (SELECT username FROM btk.Users WHERE idUsers = btk.alarm_ack.Users_chenge) FROM btk.alarm_ack where notification_idnotification= '" + temp + "';");
                MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(sql, myConnection);

                DataSet ds = new DataSet();
                myDataAdapter.Fill(ds);
                dataGridView_rezultat_trevog.AutoGenerateColumns = false;
                dataGridView_rezultat_trevog.DataSource = ds.Tables[0];
                myDataAdapter.Dispose();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "");
            }//
        }

        private void treeView_client_info_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var mySelectedNode = treeView_client_info.GetNodeAt(e.X, e.Y);
            treeView_client_info.LabelEdit = true;
            mySelectedNode.BeginEdit();
        }

        private void treeView_haracteristiki_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var mySelectedNode = treeView_haracteristiki.GetNodeAt(e.X, e.Y);
            treeView_haracteristiki.LabelEdit = true;
            mySelectedNode.BeginEdit();
        }


    }
}
