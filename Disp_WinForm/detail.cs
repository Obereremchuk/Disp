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
using Gecko;
using System.Threading;

namespace Disp_WinForm
{
    public partial class detail : Form
    {

        Macros macros = new Macros();
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
            string t = System.IO.Path.GetDirectoryName(Application.ExecutablePath) +  "\\Firefox"; 

            Xpcom.Initialize(t);
            
            // Looad varible data
            _id_notif = vars_form.id_notif;
            _id_status = vars_form.id_status;
            _user_login_id = vars_form.user_login_id;
            _search_id = vars_form.search_id;
            _unit_name = vars_form.unit_name;
            _restrict_un_group = vars_form.restrict_un_group;
            _alarm_name = vars_form.alarm_name;
            _eid = vars_form.eid;

            //get_remaynder();
            //accsses();

            dateTimePicker_close_plan_date.Value = DateTime.Now.AddDays(60);

            //get_sensor_value();
            //TreeView_zapolnyaem();
            //TreeView_harakteristiki();
            //mysql_get_hronologiya_trivog();
            //mysql_get_group_alarm();
            comboBox_status_trevogi.Text = _id_status;
            this.Text = "ID Тривоги: " + _id_notif + ", " + vars_form.unit_name + ", " + _alarm_name + ": " + vars_form.zvernenya;
            //SetTimer();
            //arhiv_object();
            //get_close_object_data();
            

        }

        

        private void accsses()
        {
            if (vars_form.user_login_id == "6" || vars_form.user_login_id == "2")
            {
                
            }
            else
            {
                tabControl2.TabPages.Remove(tabPage_dii_z_obectom);
            }
            
        }// Прячм вкладки если ты не из касты

        private void close_start_object()
        {

        }

        private void get_remaynder()
        {
            string sql2 = string.Format("select remaynder_activate from btk.notification where idnotification=" + _id_notif + ";");
            string remaynder_activate = macros.sql_command(sql2);
            if (remaynder_activate == "True")
            {
                remaynder_checkBox.Checked = true;
                string sql = string.Format("select remayder_date from btk.notification where idnotification=" + _id_notif + ";");
                string remayder_date = macros.sql_command(sql);
                DateTime rem = Convert.ToDateTime(remayder_date);
                remaynder_dateTimePicker.Value = rem;
            }
        }

        private void task()
        {
            Task task = new Task(() =>
            {
                while (true)
                {

                    get_sensor_value();
                    Thread.Sleep(10000);
                }
            });
            task.Start();
        }

        private void TreeView_harakteristiki()//Получаем произвольные поля из Виалона и заполняем дерево
        {

            string json = macros.wialon_request_new("&svc=core/search_items&params={\"" +
                                                       "spec\":{" + 
                                                       "\"itemsType\":\"avl_unit\"," + 
                                                       "\"propName\":\"sys_id\"," + 
                                                       "\"propValueMask\":\"" + _search_id + "\", " + 
                                                       "\"sortType\":\"sys_name\"," + 
                                                       "\"or_logic\":\"1\"}," + 
                                                       "\"or_logic\":\"1\"," + 
                                                       "\"force\":\"1\"," + 
                                                       "\"flags\":\"15208907\"," + 
                                                       "\"from\":\"0\"," + 
                                                       "\"to\":\"5\"}");

            var m = JsonConvert.DeserializeObject<RootObject>(json);

            TreeNode node1 = new TreeNode("Характеристики:");
            treeView_haracteristiki.Nodes.Add(node1);
            treeView_haracteristiki.Nodes[0].Expand();

            if (m.items.Count!=0)
            {
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
        }

        private void TreeView_zapolnyaem()//Получаем произвольные поля из Виалона и заполняем дерево
        {
            string json = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                    "\"spec\":{" + 
                                                    "\"itemsType\":\"avl_unit\"," + 
                                                    "\"propName\":\"sys_id\"," + 
                                                    "\"propValueMask\":\"" + _search_id + "\", " + 
                                                    "\"sortType\":\"sys_name\"," + 
                                                    "\"or_logic\":\"1\"}," + 
                                                    "\"or_logic\":\"1\"," + 
                                                    "\"force\":\"1\"," + 
                                                    "\"flags\":\"15208907\"," + 
                                                    "\"from\":\"0\"," + 
                                                    "\"to\":\"5\"}");

            var m = JsonConvert.DeserializeObject<RootObject>(json);

            if (m.error != 1)
            {
                if (m.items.Count==0)
                {
                    
                }
                else
                {
                    if (m.items[0].flds.Count == 0
                    ) //Если поля Увага не существует блокируем кнопку изменения этого поля
                    {
                        button_izmenit_uvaga.Enabled = false;
                        textBox_Uvaga.Text = "Поля увага не існує!";
                    }

                    if (m.items[0].flds.Count != 0
                    ) //Если первое поле не содержит Увага блокируем кнопку изменения этого поля
                    {
                        try
                        {
                            if (!m.items[0].flds[1].n.Contains("УВАГА"))
                            {
                                button_izmenit_uvaga.Enabled = false;
                                textBox_Uvaga.Text = "Поля увага не існує!";
                            }
                        }
                        catch
                        {
                            textBox_Uvaga.Text = "Неможливо відобразити поле Увага";
                            button_izmenit_uvaga.Enabled = false;
                        }
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

                if (m.items.Count != 0)
                {
                    foreach (var keyvalue in m.items[0].flds)
                    {

                        if (keyvalue.Value.n.Contains("УВАГА") & !keyvalue.Value.n.Contains("алгоритм"))
                        {
                            treeView_client_info.Nodes[0].Nodes
                                .Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                            textBox_Uvaga.Text = keyvalue.Value.v.ToString();
                            treeView_client_info.Nodes[0].Nodes
                                .Add(new TreeNode("Назва об'єкту:" + m.items[0].nm.ToString()));
                            treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("IMEI:" + m.items[0].uid.ToString()));
                            treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("SIM:" + m.items[0].ph.ToString()));
                        }

                        treeView_client_info.Nodes[1].Nodes
                            .Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                        if (keyvalue.Value.n.Contains("Кодов"))
                        {
                            treeView_client_info.Nodes[0].Nodes
                                .Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                        }
                    }


                    foreach (var keyvalue in m.items[0].aflds)
                    {
                        treeView_client_info.Nodes[2].Nodes
                            .Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));

                    }
                }
            }
        }

        private void mysql_get_group_alarm()//Получаем групированые тривоги для заполнения таблицы
        {
            if (_restrict_un_group == false)
            {
                dataGridView_group_alarm.AutoGenerateColumns = false;
                dataGridView_group_alarm.DataSource = macros.GetData("SELECT " +
                                                                     "idnotification, " +
                                                                     "unit_id, " +
                                                                     "unit_name, " +
                                                                     "type_alarm, " +
                                                                     "product, " +
                                                                     "curr_time, " +
                                                                     "msg_time, " +
                                                                     "group_alarm, " +
                                                                     "Status " +
                                                                     "FROM btk.notification " +
                                                                     "WHERE " +
                                                                     "idnotification <> '" + _id_notif + "' AND " +
                                                                     "unit_id = '" + _search_id + "' AND " +
                                                                     "(Status = 'Відкрито' OR Status = 'Обробляется')");//Если тревога открыта
            }
            else
            {
                dataGridView_group_alarm.AutoGenerateColumns = false;
                dataGridView_group_alarm.DataSource = macros.GetData("SELECT " +
                                                                     "idnotification, " +
                                                                     "unit_id, " +
                                                                     "unit_name, " +
                                                                     "type_alarm, " +
                                                                     "product, " +
                                                                     "curr_time, " +
                                                                     "msg_time, " +
                                                                     "group_alarm, " +
                                                                     "Status " +
                                                                     "FROM btk.notification " +
                                                                     "WHERE " +
                                                                     "idnotification <> '" + _id_notif + "' AND " +
                                                                     "unit_id = '" + _search_id + "' AND " +
                                                                     "group_alarm = '" + _id_notif + "'");//Если тревога закрыта


                button_group_alarm.Enabled = false;
                button_ungroup_alarm.Enabled = false;
            }
            
        }

        private void mysql_get_hronologiya_trivog()//Получаем изаполняем таблицу хронологии обработки открытой тревоги
        {
            dataGridView_hronologija_trivog.AutoGenerateColumns = false;
            dataGridView_hronologija_trivog.RowHeadersVisible = false;
            dataGridView_hronologija_trivog.DataSource = macros.GetData("SELECT alarm_text, time_start_ack, btk.alarm_ack.time_stamp, btk.Users.username, current_status_alarm, vizov_police, vizov_gmp FROM btk.alarm_ack, btk.Users where notification_idnotification='" + _id_notif + "' and users_chenge = idUsers");
        }

        private void button_vnesti_zapis_Click(object sender, EventArgs e)//Вносим запись в хронологию обработки тревоги
        {

            if (textBox_otrabotka_trevogi.Text == "")//Запись в хронологию обработки не может быть внесена без комментария к действию
            {
                MessageBox.Show("Внеси запис про дію обробки тривоги");
                return;
            }

            textBox_otrabotka_trevogi.Text = MySqlHelper.EscapeString(textBox_otrabotka_trevogi.Text);
            int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
            int police = checkBox_vizov_police.Checked ? 1 : 0;
            macros.sql_command("insert into btk.alarm_ack(" +
                                "alarm_text, " +
                                "notification_idnotification, " +
                                "Users_chenge, time_start_ack, " +
                                "current_status_alarm, " +
                                "vizov_police, " +
                                "vizov_gmp) " +
                                "values('" + textBox_otrabotka_trevogi.Text + "', " +
                                "'" + _id_notif + "'," +
                                "'" + _user_login_id + "', " +
                                "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                " '" + comboBox_status_trevogi.Text + "', '" +
                                "" + police + "', " +
                                "'" + gmr + "'); UPDATE btk.notification SET Status = '" + comboBox_status_trevogi.Text + "', time_stamp = now(), Users_idUsers='" + _user_login_id + "' WHERE idnotification = '" + _id_notif + "'OR group_alarm = '" + _id_notif + "'; ");

            macros.sql_command("update btk.notification set time_stamp='"+ Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "' where idnotification = '"+_id_notif+"';");

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
                string recipient = "<" + vars_form.user_login_email + ">," + "<a.lozinskiy@venbest.com.ua>, <y.kravchenko@venbest.com.ua>";
                send_email(recipient);
            }
            if (comboBox_status_trevogi.SelectedItem.ToString() == "Дилеры" & (_id_status == "Обробляется" || _id_status == "Відкрито" || _id_status == "808" || _id_status == "Продажи"))//если изменяем статус с Обробляется на 808  - отправляем меил со всейхронологией обработки тревоги
            {
                string recipient = "<" + vars_form.user_login_email + ">," + "<e.danilchenko@venbest.com.ua>,<a.andreasyan@venbest.com.ua>,<s.gregul@venbest.com.ua>";
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

        private void button_group_alarm_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView_group_alarm.SelectedRows)
            {
                string value1 = row.Cells[0].Value.ToString();

                macros.sql_command("UPDATE btk.notification " +
                                   "SET " +
                                   "group_alarm = '" + _id_notif + "', " +
                                   "time_stamp = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                                   "WHERE idnotification = '" + value1 + "'; ");

            }//указываем к какой тревоге сгрупированы выделенные тревоги

            macros.sql_command("insert into btk.alarm_ack(" +
                               "alarm_text, " +
                               "notification_idnotification, " +
                               "Users_chenge, " +
                               "time_start_ack, " +
                               "time_end_ack) " +
                               "values('Виконано групуваня', '" + _id_notif + "', " +
                               "'" + _user_login_id + "', " +
                               "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "', " +
                               "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "')");

            mysql_get_group_alarm();//обновляем таблицу группированых тревог
            mysql_get_hronologiya_trivog();//обновляем таблицу хронология обработаных тревог
        }//Группируем выделенные тревоги как одну

        private void button_ungroup_alarm_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView_group_alarm.SelectedRows)
            {
                string value1 = row.Cells[0].Value.ToString();

                macros.sql_command("UPDATE btk.notification SET group_alarm = null, time_stamp = now() WHERE idnotification = '" + value1 + "';");
            }


            macros.sql_command("insert into btk.alarm_ack" +
                               "(alarm_text, " +
                               "notification_idnotification, " +
                               "Users_chenge, " +
                               "time_start_ack, " +
                               "time_end_ack) " +
                               "values('Виконано розгрупуваня', '" + _id_notif + "'," +
                               " '" + _user_login_id + "', " +
                               "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "'," +
                               " '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "')");//убераем указатель к какой тревоге сгрупированы выделенные тревоги

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

            macros.sql_command("UPDATE btk.notification SET alarm_locked = '0', alarm_locked_user = null WHERE idnotification = '" + _id_notif + "';");
            
        }//если форма закрывается оператором - снимаем блокировку одновременного открытия

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
            string json = macros.wialon_request_new("&svc=unit/calc_last_message&params={" +
                                                    "\"unitId\":\"" + _search_id + "\"," + 
                                                    "\"sensors\":\"\"," + 
                                                    "\"flags\":\"\"}");//получаем датчики объекта
            string json2 = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                     "\"spec\":{" + 
                                                     "\"itemsType\":\"avl_unit\"," + 
                                                     "\"propName\":\"sys_id\"," + 
                                                     "\"propValueMask\":\"" + _search_id + "\", " + 
                                                     "\"sortType\":\"sys_name\"," + 
                                                     "\"or_logic\":\"1\"}," + 
                                                     "\"or_logic\":\"1\"," + 
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"6821378\"," + 
                                                     "\"from\":\"0\"," + 
                                                     "\"to\":\"5\"}");//получаем текущее местоположение объекта


            var m = JsonConvert.DeserializeObject<RootObject>(json2);

            if(m.items.Count != 0)
            {
                if (m.items[0].lmsg !=null)
                {
                    DateTime lmsg = macros.UnixTimeStampToDateTime(m.items[0].lmsg.t);
                    groupBox7.Text = "Об’єкт на мапі. Останнє повідомлення: " + macros.UnixTimeStampToDateTime(m.items[0].lmsg.t).ToString();


                }
            }

            if (m.items.Count!=0)
            {
                var lat = "";
                var lon = "";
                if (m.items[0].pos != null)
                {
                    lat = m.items[0].pos["y"];
                    lon = m.items[0].pos["x"];
                }

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

                if (m.items[0].pos != null)
                {
                    string json3 = macros.wialon_request_new("&svc=resource/get_zones_by_point&params={" +
                                                         "\"spec\":{" +
                                                         "\"zoneId\":{" +
                                                         "\"28\":[],\"" +
                                                         "241\":[] }" + "," +
                                                         "\"lat\":" + lat + "," +
                                                         "\"lon\":" + lon + "}}");//получаем id геозон в которых находится объект, ресурс res_service id=28

                    var geozone = JsonConvert.DeserializeObject<Dictionary<int, List<dynamic>>>(json3);
                    var array_geozone = geozone.Values.ToArray();//по другому не вышло, не могу достать из лист значения, перевем в аррай
                    var array_geozone_keys = geozone.Keys.ToArray();
                }

            }

            

            //listBox_geozone_wl.Items.Clear();

            //if (array_geozone.Length > 0)
            //{

            //    if (array_geozone[0].Count > 0)
            //    {
            //        foreach (var keyvalue_ in array_geozone[0])//для каждого айди геозон найдем имя и заполним лист формы геозон
            //        {
            //            if (array_geozone_keys[0].ToString() == "28")
            //            {
            //                string json4 = macros.wialon_request_new("&svc=resource/get_zone_data&params={" + 
            //                                                         "\"itemId\":\"28\"," + 
            //                                                         "\"col\":[" + keyvalue_ + "]," + 
            //                                                         "\"flags\":\"16\"}");//

            //                var x = JsonConvert.DeserializeObject<List<Value_>>(json4);
            //                listBox_geozone_wl.Items.Add(x[0].n.ToString());
            //            }
            //            else if (array_geozone_keys[0].ToString() == "241")
            //            {
            //                string json4 = macros.wialon_request_new("&svc=resource/get_zone_data&params={" + 
            //                                                         "\"itemId\":\"241\"," + 
            //                                                         "\"col\":[" + keyvalue_ + "]," + 
            //                                                         "\"flags\":\"16\"}");//

            //                var x = JsonConvert.DeserializeObject<List<Value_>>(json4);
            //                listBox_geozone_wl.Items.Add(x[0].n.ToString());
            //            }

            //        }
            //    }

            //    if (array_geozone.Length > 1)
            //    {
            //        if (array_geozone_keys[1].ToString() == "241")
            //        {
            //            foreach (var keyvalue_ in array_geozone[1])//для каждого айди геозон найдем имя и заполним лист формы геозон
            //            {
            //                string json4 = macros.wialon_request_new("&svc=resource/get_zone_data&params={" + 
            //                                                         "\"itemId\":\"241\"," + 
            //                                                         "\"col\":[" + keyvalue_ + "]," + 
            //                                                         "\"flags\":\"16\"}");//
                            
            //                var x = JsonConvert.DeserializeObject<List<Value_>>(json4);
            //                listBox_geozone_wl.Items.Add(x[0].n.ToString());
            //            }
            //        }
            //        if (array_geozone_keys[1].ToString() == "28")
            //        {
            //            foreach (var keyvalue_ in array_geozone[1])//для каждого айди геозон найдем имя и заполним лист формы геозон
            //            {
            //                string json4 = macros.wialon_request_new("&svc=resource/get_zone_data&params={" + 
            //                                                         "\"itemId\":\"28\"," + 
            //                                                         "\"col\":[" + keyvalue_ + "]," + 
            //                                                         "\"flags\":\"16\"}");//
            //                var x = JsonConvert.DeserializeObject<List<Value_>>(json4);
            //                listBox_geozone_wl.Items.Add(x[0].n.ToString());
            //            }
            //        }
               // }

            //}                    
        }//получаем текущие значения датчиков, нахождение в ГЕО-зонах объекта

       

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

                string json2 = macros.wialon_request_new("&svc=unit/exec_cmd&params={" +
                                          "\"itemId\":\"" + _search_id + "\"," + 
                                          "\"commandName\":\"" + listBox_commads_wl.SelectedItem.ToString() + "\"," + 
                                          "\"linkType\":\"tcp\"," + 
                                          "\"param\":\"\"," + 
                                          "\"timeout\":\"0\"," + 
                                          "\"flags\":\"0\"}");

                macros.sql_command("insert into btk.alarm_ack(" +
                                   "alarm_text, " +
                                   "notification_idnotification, " +
                                   "Users_chenge, " +
                                   "time_start_ack, " +
                                   "time_end_ack) " +
                                   "values('Відправлено команду: " + listBox_commads_wl.SelectedItem.ToString() + "', " +
                                   "'" + _id_notif + "', " +
                                   "'" + _user_login_id + "', " +
                                   "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "', " +
                                   "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "')");


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
            string json = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                     "\"spec\":{" + 
                                                     "\"itemsType\":\"avl_unit\"," + 
                                                     "\"propName\":\"sys_id\"," + 
                                                     "\"propValueMask\":\"" + _search_id + "\", " + 
                                                     "\"sortType\":\"sys_name\"," + 
                                                     "\"or_logic\":\"1\"}," + 
                                                     "\"or_logic\":\"1\"," + 
                                                     "\"force\":\"1\"," + 
                                                     "\"flags\":\"15208907\"," + 
                                                     "\"from\":\"0\"," + 
                                                     "\"to\":\"5\"}");//15208907

            var m = JsonConvert.DeserializeObject<RootObject>(json);


            string json2 = macros.wialon_request_new("&svc=item/update_custom_field&params={" +
                                                     "\"itemId\":\"" + _search_id + "\"," + 
                                                     "\"id\":\"1\"," + 
                                                     "\"callMode\":\"update\"," +
                                                     "\"n\":\"" + m.items[0].flds[1].n + "\"," +
                                                     "\"v\":\"" + textBox_Uvaga.Text.Replace("\"", "%5C%22") + "\"}");//получаем датчики объекта

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

            dataGridView_trivogi_objecta.AutoGenerateColumns = false;
            dataGridView_trivogi_objecta.DataSource = macros.GetData("SELECT idnotification, type_alarm, msg_time, Status, last_location FROM btk.notification where unit_id = " + _search_id + " and group_alarm is null");

        }

        private void dataGridView_trivogi_objecta_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            var temp = dataGridView_trivogi_objecta.Rows[e.RowIndex].Cells[0].Value.ToString();

            dataGridView_rezultat_trevog.AutoGenerateColumns = false;
            dataGridView_rezultat_trevog.DataSource = macros.GetData("SELECT " +
                                                                     "alarm_text, " +
                                                                     "vizov_gmp, " +
                                                                     "vizov_police, " +
                                                                     "time_stamp, " +
                                                                     "(SELECT username FROM btk.Users " +
                                                                     "WHERE idUsers = btk.alarm_ack.Users_chenge) " +
                                                                     "FROM btk.alarm_ack where notification_idnotification= '" + temp + "';");

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

        private void get_close_object_data()
        {
            // get id_object DB where id_wl=
            string sql1 = string.Format("SELECT idObject FROM btk.Object where Object_id_wl='" + _search_id + "';");
            string idobject = macros.sql_command(sql1);

            string sql2 = string.Format("SELECT Close_object.close_reason,Close_object.close_plan_date,Close_object.close_start_user,Close_object.close_start_ok, Users.username FROM btk.Close_object, btk.Users where Users.idUsers= Close_object.close_start_user and Object_idObject='" + idobject + "';");
            var answer = macros.GetData(sql2);

            if (answer.Rows.Count >= 1)
            {
                if (answer.Rows[0][3].ToString() == "1")
                {
                    textBox_close_reason.Text = answer.Rows[0][0].ToString();
                    textBox_close_reason.Enabled = false;

                    dateTimePicker_close_plan_date.Value = DateTime.Parse(answer.Rows[0][1].ToString());
                    dateTimePicker_close_plan_date.Enabled = false;

                    textBox_close_start_user.Text = answer.Rows[0][4].ToString();
                    textBox_close_start_user.Enabled = false;

                    button_close_start.Enabled = false;
                    button_close_stop.Enabled = true;
                }
            }
            else
            {
                textBox_close_reason.Enabled = true;
                textBox_close_reason.Text = "";
                dateTimePicker_close_plan_date.Enabled = true;
                button_close_start.Enabled = true;

                button_close_stop.Enabled = false;
                textBox_close_start_user.Enabled = false;
                textBox_close_start_user.Text = "";
            }
        }

        private void button_close_start_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Запусти процес?", "Старт", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (textBox_close_reason.Text == "")
                {
                    MessageBox.Show("Причина зміни?");
                    return;
                }

                // get id_object DB where id_wl=
                string sql1 = string.Format("SELECT idObject FROM btk.Object where Object_id_wl='"+_search_id+"';");
                string idobject = macros.sql_command(sql1);


                // удаляем объект в базе
                string sql2 = string.Format("insert into " +
                                            "btk.Close_object " +
                                            "(Object_idObject,close_reason,close_plan_date,close_start_date,close_start_user, close_start_ok) " +
                                            "values('"+ idobject + "', '"+ textBox_close_reason.Text +"', '" + Convert.ToDateTime(dateTimePicker_close_plan_date.Value).ToString("yyyy-MM-dd hh:mm:ss") + "', '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss") + "', '" + _user_login_id + "', '1');");
                macros.sql_command(sql2);

                get_close_object_data();
            }
        }

        private void button_close_stop_Click(object sender, EventArgs e)
        {
            // get id_object DB where id_wl
            string sql1 = string.Format("SELECT idObject FROM btk.Object where Object_id_wl='" + _search_id + "';");
            string idobject = macros.sql_command(sql1);

            string sql2 = string.Format("delete FROM btk.Close_object where Object_idObject='" + idobject + "';");
            var answer = macros.GetData(sql2);

            get_close_object_data();

        }

        private void detail_Shown(object sender, EventArgs e)
        {
            
        }

        private void detail_Load(object sender, EventArgs e)
        {
            get_remaynder();
            accsses();
            TreeView_zapolnyaem();
            TreeView_harakteristiki();
            mysql_get_hronologiya_trivog();
            mysql_get_group_alarm();
            arhiv_object();
            get_close_object_data();
            //task();
            get_sensor_value();
            //start mini map
            try
            {
                geckoWebBrowser1.Navigate("http://10.44.30.32/disp_app/HTMLPage_map.html?foo=" + _search_id);
            }
            catch
            {
            }


            //webBrowser1.ScriptErrorsSuppressed = true;
            //webBrowser1.Size = webBrowser1.Document.Body.ScrollRectangle.Size;



            //if opened start detail - automaticly chenge status alarm in combobox = Обробляется
            comboBox_status_trevogi.SelectedIndex = comboBox_status_trevogi.FindStringExact(_id_status);
        }
    }
}
