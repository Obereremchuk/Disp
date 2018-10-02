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

namespace Disp_WinForm
{
    public partial class detail : Form
    {
        string wl_sid = "6e0e4256cf5f1a76059f53c81d2d4e5c";
        public detail()
        {
            InitializeComponent();
            get_sensor_value();
            TreeView_zapolnyaem();
            mysql_get_hronologiya_trivog();
            mysql_get_group_alarm();
            restrict_un_group();
            comboBox_status_trevogi.Text = vars_form.id_status;
            this.Text = "ID Тривоги: " + vars_form.id_notif + ", " + vars_form.unit_name;
        }

        private void TreeView_zapolnyaem()
        {
            try
            {
                MyWebRequest myRequest = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + wl_sid + "&svc=core/search_items&params={\"spec\":{" + "\"itemsType\":\"avl_unit\"," + "\"propName\":\"sys_id\"," + "\"propValueMask\":\"" + "*" + vars_form.search_id + "*" + "\", " + "\"sortType\":\"sys_name\"," + "\"or_logic\":\"1\"}," + "\"or_logic\":\"1\"," + "\"force\":\"1\"," + "\"flags\":\"15208907\"," + "\"from\":\"0\"," + "\"to\":\"5\"}");//15208907
                string json = myRequest.GetResponse();
                var m = JsonConvert.DeserializeObject<RootObject>(json);

                TreeNode node1 = new TreeNode("Об'єкт охорони:");
                treeView_client_info.Nodes.Add(node1);
                treeView_client_info.Nodes[0].Expand();
                TreeNode node2 = new TreeNode("Відповідальні особи");
                treeView_client_info.Nodes.Add(node2);
                treeView_client_info.Nodes[1].Expand();
                TreeNode node3 = new TreeNode("Інформація про установку");
                treeView_client_info.Nodes.Add(node3);

                foreach (var keyvalue in m.items[0].flds)
                {
                    switch (keyvalue.Value.n)
                    {
                        case "0 УВАГА":
                            treeView_client_info.Nodes[0].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                            treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("Назва об'єкту:" + m.items[0].nm.ToString()));
                            treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("IMEI:" + m.items[0].uid.ToString()));

                            break;
                        case "2.1 І Відповідальна особа (основна)":
                            treeView_client_info.Nodes[1].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                            break;
                        case "2.2 ІІ Відповідальна особа":
                            treeView_client_info.Nodes[1].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                            break;
                        case "2.3 ІІІ Відповідальна особа":
                            treeView_client_info.Nodes[1].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                            break;
                        case "3.2.2 Установник-монтажник: ПІБ, №тел.":
                            treeView_client_info.Nodes[2].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                            break;
                        case "3.8.2 Місце установки \"тривожної кнопки\"":
                            treeView_client_info.Nodes[2].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                            break;
                        case "3.9.2 Штатні кнопки введення PIN-коду":
                            treeView_client_info.Nodes[2].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                            break;
                    }
                    // удаление у первого узла второго дочернего подузла
                    //treeView1.Nodes[0].Nodes.RemoveAt(1);
                    //// Удаление узла tovarNode и всех его дочерних узлов
                    //treeView1.Nodes.Remove(clientinfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }


        }

        private void mysql_get_group_alarm()//Execute SQL query
        {
            if (vars_form.restrict_un_group == false)
            {
                try
                {
                    MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True");
                    //string sql = string.Format("SELECT alarm_text, time_start_ack, time_end_ack, btk.Users.username, current_status_alarm FROM btk.alarm_ack, btk.Users where notification_idnotification='" + vars_form.id_notif + "' and users_chenge = idUsers");
                    string sql = string.Format("SELECT idnotification, unit_id, unit_name, type_alarm, product, curr_time, msg_time, group_alarm, Status FROM btk.notification WHERE idnotification <> '" + vars_form.id_notif + "' AND unit_id = '" + vars_form.search_id + "' AND (Status = 'Відкрито' OR Status = 'Обробляется')");
                    MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(sql, myConnection);
                    DataSet ds = new DataSet();
                    myDataAdapter.Fill(ds);
                    dataGridView_group_alarm.AutoGenerateColumns = false;
                    //dataGridView_group_alarm.RowHeadersVisible = false;
                    dataGridView_group_alarm.DataSource = ds.Tables[0];


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                try
                {
                    MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True");
                    //string sql = string.Format("SELECT alarm_text, time_start_ack, time_end_ack, btk.Users.username, current_status_alarm FROM btk.alarm_ack, btk.Users where notification_idnotification='" + vars_form.id_notif + "' and users_chenge = idUsers");
                    string sql = string.Format("SELECT idnotification, unit_id, unit_name, type_alarm, product, curr_time, msg_time, group_alarm, Status FROM btk.notification WHERE idnotification <> '" + vars_form.id_notif + "' AND unit_id = '" + vars_form.search_id + "' AND group_alarm = '" + vars_form.id_notif + "'");
                    MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(sql, myConnection);
                    DataSet ds = new DataSet();
                    myDataAdapter.Fill(ds);
                    dataGridView_group_alarm.AutoGenerateColumns = false;
                    //dataGridView_group_alarm.RowHeadersVisible = false;
                    dataGridView_group_alarm.DataSource = ds.Tables[0];


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            
        }

        private void mysql_get_hronologiya_trivog()//Execute SQL query
        {
            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True");
                string sql = string.Format("SELECT alarm_text, time_start_ack, time_end_ack, btk.Users.username, current_status_alarm FROM btk.alarm_ack, btk.Users where notification_idnotification='" + vars_form.id_notif + "' and users_chenge = idUsers");
                MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(sql, myConnection);
                DataSet ds = new DataSet();
                myDataAdapter.Fill(ds);
                dataGridView_hronologija_trivog.AutoGenerateColumns = false;
                dataGridView_hronologija_trivog.RowHeadersVisible = false;
                dataGridView_hronologija_trivog.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void button_vnesti_zapis_Click(object sender, EventArgs e)
        {
            if (textBox_otrabotka_trevogi.Text == "")
            {
                MessageBox.Show("Внеси запис про дію обробки тривоги");
                return;
            }
            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True");
                string sql = string.Format("insert into btk.alarm_ack(alarm_text, notification_idnotification, Users_chenge, time_start_ack, time_end_ack, current_status_alarm) values('" + textBox_otrabotka_trevogi.Text + "', '" + vars_form.id_notif + "','" + vars_form.user_login_id + "', '" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd hh:mm:ss") + "', '" + Convert.ToDateTime(dateTimePicker_okonchanie_dejstvija.Value).ToString("yyyy-MM-dd hh:mm:ss") + "', '" + comboBox_status_trevogi.Text + "'); UPDATE btk.notification SET Status = '" + comboBox_status_trevogi.Text + "', time_stamp = now(), Users_idUsers='" + vars_form.user_login_id + "' WHERE idnotification = '" + vars_form.id_notif + "'OR group_alarm = '" + vars_form.id_notif + "'; ");
                MySqlCommand MyCommand2 = new MySqlCommand(sql, myConnection);
                MySqlDataReader MyReader2;
                myConnection.Open();
                MyReader2 = MyCommand2.ExecuteReader();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            mysql_get_hronologiya_trivog();

            textBox_otrabotka_trevogi.Text = "";

            if (checkBox_send_email.Checked == true)
            {
                send_email();
            }

            checkBox_send_email.Checked = false;
            
        }

        private void detail_Load(object sender, EventArgs e)
        {
            //start mini map
            webBrowser1.Navigate("http://10.44.30.32/disp_app/HTMLPage_map.html?foo=" + vars_form.search_id);

            // loking opened alarm by user who used
            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True");
                string sql = string.Format("UPDATE btk.notification SET alarm_locked = '1', alarm_locked_user = '"+ vars_form.user_login_name +"' WHERE idnotification = '" + vars_form.id_notif + "';");
                MySqlCommand MyCommand2 = new MySqlCommand(sql, myConnection);
                MySqlDataReader MyReader2;
                myConnection.Open();
                MyReader2 = MyCommand2.ExecuteReader();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

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
                    MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True");
                    string sql = string.Format("UPDATE btk.notification SET group_alarm = '" + vars_form.id_notif + "', time_stamp = now() WHERE idnotification = '" + value1 + "'; ");
                    MySqlCommand MyCommand2 = new MySqlCommand(sql, myConnection);
                    MySqlDataReader MyReader2;
                    myConnection.Open();
                    MyReader2 = MyCommand2.ExecuteReader();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True");
                string sql2 = string.Format("insert into btk.alarm_ack(alarm_text, notification_idnotification, Users_chenge, time_start_ack, time_end_ack) values('Виконано групуваня', '" + vars_form.id_notif + "', '" + vars_form.user_login_id + "', '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "', '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "')");
                MySqlCommand MyCommand2 = new MySqlCommand(sql2, myConnection);
                MySqlDataReader MyReader2;
                myConnection.Open();
                MyReader2 = MyCommand2.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            mysql_get_group_alarm();
            mysql_get_hronologiya_trivog();
        }

        private void button_ungroup_alarm_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView_group_alarm.SelectedRows)
            {
                string value1 = row.Cells[0].Value.ToString();

                try
                {
                    MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True");
                    string sql = string.Format("UPDATE btk.notification SET group_alarm = null, time_stamp = now() WHERE idnotification = '" + value1 + "';");
                    MySqlCommand MyCommand2 = new MySqlCommand(sql, myConnection);
                    MySqlDataReader MyReader2;
                    myConnection.Open();
                    MyReader2 = MyCommand2.ExecuteReader();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True");
                string sql2 = string.Format("insert into btk.alarm_ack(alarm_text, notification_idnotification, Users_chenge, time_start_ack, time_end_ack) values('Виконано розгрупуваня', '" + vars_form.id_notif + "', '" + vars_form.user_login_id + "', '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "', '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "')");
                MySqlCommand MyCommand2 = new MySqlCommand(sql2, myConnection);
                MySqlDataReader MyReader2;
                myConnection.Open();
                MyReader2 = MyCommand2.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            mysql_get_group_alarm();
            mysql_get_hronologiya_trivog();
        }

        private void restrict_un_group()
        {
            if (vars_form.restrict_un_group == true)
            {
                button_group_alarm.Enabled = false;
                button_ungroup_alarm.Enabled = false;
            }
        }

        private void dataGridView_group_alarm_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView_group_alarm.Rows[e.RowIndex].Cells[5].Value.ToString() != "")
            {
                e.CellStyle.BackColor = Color.Gray;
            }
        }

        private void detail_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Unloking opened alarm by user who used
            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True");
                string sql = string.Format("UPDATE btk.notification SET alarm_locked = '0', alarm_locked_user = null WHERE idnotification = '" + vars_form.id_notif + "';");
                MySqlCommand MyCommand2 = new MySqlCommand(sql, myConnection);
                MySqlDataReader MyReader2;
                myConnection.Open();
                MyReader2 = MyCommand2.ExecuteReader();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void button_cancel_edit_Click(object sender, EventArgs e)
        {
            if (textBox_otrabotka_trevogi.Text != "")
            {
                DialogResult result = MessageBox.Show("Не збережена інформація буде втрачена?", "Підтвердіть", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    this.Close();
                }
                else if (result == DialogResult.No)
                {
                    return;
                }
            }
            this.Close();
        }

        private void send_email()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.venbest.com.ua");

                mail.From = new MailAddress("noreply@venbest.com.ua");
                mail.To.Add("<o.pustovit@venbest.com.ua>,<d.lenik@venbest.com.ua>,<a.oberemchuk@venbest.com.ua>,<a.oberemchuk@venbest.com.ua>");
                mail.Subject = "Реагування: Тривога ID: "+ vars_form.id_notif + ", Тривога: " + vars_form.alarm_name + ", Обєкт: " + vars_form.unit_name;
                mail.IsBodyHtml = true;
                mail.Body = htmlMessageBody(dataGridView_hronologija_trivog).ToString();

                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("noreply@venbest.com.ua", "L2TPr6");
                SmtpServer.EnableSsl = false;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

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
        }

        private void get_sensor_value()
        {
            try
            {
                MyWebRequest myRequest = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + wl_sid + "&svc=unit/calc_last_message&params={\"unitId\":\"" + vars_form.search_id + "\"," + "\"sensors\":\"\"," + "\"flags\":\"\"}");
                string json = myRequest.GetResponse();
                MyWebRequest myRequest2 = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + wl_sid + "&svc=core/search_items&params={\"spec\":{" + "\"itemsType\":\"avl_unit\"," + "\"propName\":\"sys_id\"," + "\"propValueMask\":\"" + "*" + vars_form.search_id + "*" + "\", " + "\"sortType\":\"sys_name\"," + "\"or_logic\":\"1\"}," + "\"or_logic\":\"1\"," + "\"force\":\"1\"," + "\"flags\":\"4198913\"," + "\"from\":\"0\"," + "\"to\":\"5\"}");//15208907
                string json2 = myRequest2.GetResponse();



                var m = JsonConvert.DeserializeObject<RootObject>(json2);

                var lat = m.items[0].pos["y"];
                var lon = m.items[0].pos["x"];

                foreach (var cmd in m.items[0].cmds)
                {
                    listBox_commads_wl.Items.Add(cmd.n.ToString());
                }
                
                dynamic stuff = JsonConvert.DeserializeObject(json);

                foreach (var keyvalue in m.items[0].sens)
                {
                    switch(keyvalue.Value.n)
                    {
                        case "Блокировка иммобилайзера":
                            if (stuff[keyvalue.Value.id.ToString()] == "1")
                            {label_relay_bl.Text = "Блокування: Увімкнуто";}
                            else { label_relay_bl.Text = "Блокування: Вимкнуто";}
                            //label_relay_bl.Text = "Блокування" + stuff[keyvalue.Value.id.ToString()];
                            break;
                        case "Датчик удара":
                            if (stuff[keyvalue.Value.id.ToString()] == "1")
                            { label_udar.Text = "Датчик удару: Увімкнуто"; }
                            else { label_udar.Text = "Датчик удару: Вимкнуто"; }
                            //label_udar.Text = "Датчик удару" + stuff[keyvalue.Value.id.ToString()];
                            break;
                        case "Напряжение АКБ":
                            label_akb.Text = "Напруга АКБ: " + stuff[keyvalue.Value.id.ToString()];
                            break;
                        case "Статус дверей":
                            if (stuff[keyvalue.Value.id.ToString()] == "1")
                            { label_door.Text = "Двері: Відкрито"; }
                            else { label_door.Text = "Двері: Закрито"; }
                            //label_door.Text = "Двері" + stuff[keyvalue.Value.id.ToString()];
                            break;
                        case "Статус охраны":
                            if (stuff[keyvalue.Value.id.ToString()] == "1")
                            { label_arm.Text = "Статус охорони: В охороні"; }
                            else { label_arm.Text = "Статус охорони: Не в охороні"; }
                            //label_arm.Text = "Статус охорони" + stuff[keyvalue.Value.id.ToString()];
                            break;
                    }
                }

                MyWebRequest myRequest3 = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + wl_sid + "&svc=resource/get_zones_by_point&params={\"spec\":{" + "\"zoneId\":{[" + "\"res_service\":\" \"]}" + "\"lat\":\"" + lat + "\"," + "\"lon\":\"" + lon + "\"}");
                string json3 = myRequest3.GetResponse();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

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
                    MyWebRequest myRequest2 = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + wl_sid + "&svc=unit/exec_cmd&params={\"itemId\":\"" + vars_form.search_id + "\"," + "\"commandName\":\"" + listBox_commads_wl.SelectedItem.ToString() + "\"," + "\"linkType\":\"tcp\"," + "\"param\":\"\"," + "\"timeout\":\"0\"," + "\"flags\":\"0\"}");
                    string json2 = myRequest2.GetResponse();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }

                try
                {
                    MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True");
                    string sql2 = string.Format("insert into btk.alarm_ack(alarm_text, notification_idnotification, Users_chenge, time_start_ack, time_end_ack) values('Відправлено команду: " + listBox_commads_wl.SelectedItem.ToString() + "', '" + vars_form.id_notif + "', '" + vars_form.user_login_id + "', '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "', '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "')");
                    MySqlCommand MyCommand2 = new MySqlCommand(sql2, myConnection);
                    MySqlDataReader MyReader2;
                    myConnection.Open();
                    MyReader2 = MyCommand2.ExecuteReader();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                mysql_get_hronologiya_trivog();
            }
            else if (result == DialogResult.No)
            {
                return;
            }
        }
    }
}
