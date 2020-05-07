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
        private string wl_id;
        private string id_db_obj;
        private List<TreeNode> _unselectableNodes = new List<TreeNode>();
        private string id_new_user;
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
            wl_id = _search_id;
            id_db_obj = macros.sql_command("SELECT idObject FROM btk.Object where Object.Object_id_wl = '" + _search_id + "';");

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

            if (vars_form.user_login_id == "9" || vars_form.user_login_id == "34")
            {

            }
            else
            {
                tabControl2.TabPages.Remove(tabPage_edit_client);
            }

        }// Прячм вкладки если ты не из касты

        private void close_start_object()
        {

        }

        private void load_vo()
        {
            string json = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                    "\"spec\":{" +
                                                    "\"itemsType\":\"avl_unit\"," +
                                                    "\"propName\":\"sys_id\"," +
                                                    "\"propValueMask\":\"" + wl_id + "\", " +
                                                    "\"sortType\":\"sys_name\"," +
                                                    "\"or_logic\":\"1\"}," +
                                                    "\"force\":\"1\"," +
                                                    "\"flags\":\"4611686018427387903\"," +
                                                    "\"from\":\"0\"," +
                                                    "\"to\":\"1\"}");
            var m = JsonConvert.DeserializeObject<RootObject>(json);

            foreach (var keyvalue in m.items[0].flds)
            {
                if (keyvalue.Value.n.Contains("Кодове "))
                {
                    //Chenge feild Кодове слово
                    kodove_slovo_textBox.Text = keyvalue.Value.v;
                }

            }

            //load VO
            DataTable VO = macros.GetData("select Kontakti_idKontakti, VOcol_num_vo from btk.VO where Object_idObject = '" + id_db_obj + "';");
            if (VO.Rows.Count >= 1)
            {
                foreach (DataRow row in VO.Rows)
                {
                    if (row[1].ToString() == "1")
                    {

                        string VO_falilia = macros.sql_command("SELECT Kontakti_familia FROM btk.Kontakti where idKontakti = '" + row[0].ToString() + "';");
                        string VO_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,'')) FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + row[0].ToString() + "';");
                        string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + row[0].ToString() + "';");
                        if (VO_phone2 == "   -   -")
                        { VO_phone2 = ""; }
                        if (VO_falilia == "")
                        {
                            textBox_vo1.Text = "";
                        }
                        else
                        {
                            textBox_vo1.Text = VO_falilia.ToUpper() + " " + VO_imya_phone + ", " + VO_phone2;
                        }
                    }
                    if (row[1].ToString() == "2")
                    {
                        string VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,'')) FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + row[0].ToString() + "';");
                        string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + row[0].ToString() + "';");
                        if (VO_phone2 == "   -   -")
                        { VO_phone2 = ""; }
                        if (VO_familia_imya_phone == "" || VO_familia_imya_phone == "Пусто Пусто , ")
                        {
                            textBox_vo2.Text = "";
                        }
                        else
                        {
                            textBox_vo2.Text = VO_familia_imya_phone + ", " + VO_phone2;
                        }
                    }
                    if (row[1].ToString() == "3")
                    {
                        string VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,'')) FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + row[0].ToString() + "';");
                        string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + row[0].ToString() + "';");
                        if (VO_phone2 == "   -   -")
                        { VO_phone2 = ""; }
                        if (VO_familia_imya_phone == "" || VO_familia_imya_phone == "Пусто Пусто , ")
                        {
                            textBox_vo3.Text = "";
                        }
                        else
                        {
                            textBox_vo3.Text = VO_familia_imya_phone + ", " + VO_phone2;
                        }
                    }
                    

                }
                if (textBox_vo1.Text == "")
                {
                    vars_form.transfer_vo1_vo_form = "1";
                }
                if (textBox_vo2.Text == "")
                {
                    vars_form.transfer_vo2_vo_form = "1";
                }
                if (textBox_vo3.Text == "")
                {
                    vars_form.transfer_vo3_vo_form = "1";
                }
            }
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


            if (remaynder_checkBox.Checked == true)
            {
                string sql = string.Format("UPDATE btk.notification SET remayder_date ='" + Convert.ToDateTime(remaynder_dateTimePicker.Value).ToString("yyyy-MM-dd") + "', remaynder_activate= 1 where idnotification=" + _id_notif + ";");
                macros.sql_command(sql);
            }
            if (remaynder_checkBox.Checked == false)
            {
                string sql = string.Format("UPDATE btk.notification SET remayder_date = null, remaynder_activate= 0 where idnotification=" + _id_notif + ";");
                macros.sql_command(sql);
            }

            if (comboBox_status_trevogi.SelectedItem.ToString() == "Закрито")
            {
                this.Close();
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

        private void button_add_vo1_Click(object sender, EventArgs e)
        {
            vars_form.num_vo = 1;
            Kontacts Kontacts_form = new Kontacts();
            Kontacts_form.Activated += new EventHandler(form_vo_add_activated);
            Kontacts_form.FormClosed += new FormClosedEventHandler(form_vo_add_deactivated);
            Kontacts_form.Show();
        }

        private void button_add_vo2_Click(object sender, EventArgs e)
        {
            vars_form.num_vo = 2;
            Kontacts Kontacts_form = new Kontacts();
            Kontacts_form.Activated += new EventHandler(form_vo_add_activated);
            Kontacts_form.FormClosed += new FormClosedEventHandler(form_vo_add_deactivated);
            Kontacts_form.Show();
        }

        private void button_add_vo3_Click(object sender, EventArgs e)
        {
            vars_form.num_vo = 3;
            Kontacts Kontacts_form = new Kontacts();
            Kontacts_form.Activated += new EventHandler(form_vo_add_activated);
            Kontacts_form.FormClosed += new FormClosedEventHandler(form_vo_add_deactivated);
            Kontacts_form.Show();
        }

        private void form_vo_add_activated(object sender, EventArgs e)
        {
            this.Visible = false;// блокируем окно контрагентов пока открыто окно добавления контрагента
        }

        private void form_vo_add_deactivated(object sender, FormClosedEventArgs e)
        {
            if (vars_form.num_vo == 1)//VO1 1 familia make upper case
            {
                string VO_falilia = macros.sql_command("SELECT Kontakti_familia FROM btk.Kontakti where idKontakti = '" + vars_form.transfer_vo1_vo_form + "';");
                string VO_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,'')) FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + vars_form.transfer_vo1_vo_form + "';");
                string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + vars_form.transfer_vo1_vo_form + "';");
                if (VO_phone2 == "   -   -")
                { VO_phone2 = ""; }
                if (VO_falilia == "")
                {
                    textBox_vo1.Text = "";
                }
                else
                {
                    textBox_vo1.Text = VO_falilia.ToUpper() + " " + VO_imya_phone + ", " + VO_phone2;
                }

            }
            if (vars_form.num_vo == 2)
            {
                string VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,'')) FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + vars_form.transfer_vo2_vo_form + "';");
                string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + vars_form.transfer_vo2_vo_form + "';");
                if (VO_phone2 == "   -   -")
                { VO_phone2 = ""; }
                if (VO_familia_imya_phone == "" || VO_familia_imya_phone == "Пусто Пусто , ")
                {
                    textBox_vo2.Text = "";
                }
                else
                {
                    textBox_vo2.Text = VO_familia_imya_phone + ", " + VO_phone2;
                }

            }
            if (vars_form.num_vo == 3)
            {
                string VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,'')) FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + vars_form.transfer_vo3_vo_form + "';");
                string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + vars_form.transfer_vo3_vo_form + "';");
                if (VO_phone2 == "   -   -")
                { VO_phone2 = ""; }
                if (VO_familia_imya_phone == "" || VO_familia_imya_phone == "Пусто Пусто , ")
                {
                    textBox_vo3.Text = "";
                }
                else
                {
                    textBox_vo3.Text = VO_familia_imya_phone + ", " + VO_phone2;
                }

            }
            this.Visible = true;// разблокируем окно контрагентов кактолько закрыто окно добавления контрагента
            vars_form.num_vo = 0;
        }

        private void textBox_vo1_DoubleClick(object sender, EventArgs e)
        {
            textBox_vo1.Text = "";
            vars_form.transfer_vo1_vo_form = "";
        }

        private void textBox_vo2_DoubleClick(object sender, EventArgs e)
        {
            textBox_vo2.Text = "";
            vars_form.transfer_vo2_vo_form = "";
        }

        private void textBox_vo3_DoubleClick(object sender, EventArgs e)
        {
            textBox_vo3.Text = "";
            vars_form.transfer_vo3_vo_form = "";
        }

        private void button_vo_save_Click(object sender, EventArgs e)
        {
            //Загружаем произвольные поля объекта
            string json = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_id\"," +
                                                     "\"propValueMask\":\"" + wl_id + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"or_logic\":\"1\"," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"15208907\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"1\"}");//15208907

            var object_data = JsonConvert.DeserializeObject<RootObject>(json);


            foreach (var keyvalue in object_data.items[0].flds)
            {

                if (keyvalue.Value.n.Contains("Кодове "))
                {
                    if (keyvalue.Value.v != kodove_slovo_textBox.Text)
                    {
                        //Chenge feild Кодове слово
                        string json2 = macros.wialon_request_new("&svc=item/update_custom_field&params={" +
                                                             "\"itemId\":\"" + wl_id + "\"," +
                                                             "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                             "\"callMode\":\"update\"," +
                                                             "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                             "\"v\":\"" + kodove_slovo_textBox.Text + "\"}");
                    }
                }

                if (keyvalue.Value.n.Contains("2.1 І Від"))
                {
                    if (keyvalue.Value.v != textBox_vo1.Text)
                    {
                        if (textBox_vo1.Text != "1")
                        {
                            //Chenge feild ВО1
                            macros.sql_command("insert into btk.VO (Object_idObject,Kontakti_idKontakti,VOcol_num_vo,VOcol_date_add,Users_idUsers) values('" + id_db_obj + "','" + vars_form.transfer_vo1_vo_form + "','1','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','" + vars_form.user_login_id + "');");
                        }
                        string json2 = macros.wialon_request_new("&svc=item/update_custom_field&params={" +
                                                             "\"itemId\":\"" + wl_id + "\"," +
                                                             "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                             "\"callMode\":\"update\"," +
                                                             "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                             "\"v\":\"" + textBox_vo1.Text + "\"}");
                    }
                }

                if (keyvalue.Value.n.Contains("2.2 ІІ Від"))
                {
                    if (keyvalue.Value.v != textBox_vo2.Text)
                    {
                        if (textBox_vo2.Text != "1")
                        {
                            //Chenge feild ВО2
                            macros.sql_command("insert into btk.VO (Object_idObject,Kontakti_idKontakti,VOcol_num_vo,VOcol_date_add,Users_idUsers) values('" + id_db_obj + "','" + vars_form.transfer_vo2_vo_form + "','2','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','" + vars_form.user_login_id + "');");
                        }

                        string json2 = macros.wialon_request_new("&svc=item/update_custom_field&params={" +
                                                             "\"itemId\":\"" + wl_id + "\"," +
                                                             "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                             "\"callMode\":\"update\"," +
                                                             "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                             "\"v\":\"" + textBox_vo2.Text + "\"}");
                    }
                }

                if (keyvalue.Value.n.Contains("2.3 ІІІ Від"))
                {
                    if (keyvalue.Value.v != textBox_vo3.Text)
                    {
                        if (textBox_vo3.Text != "1")
                        {
                            //Chenge feild ВО3
                            macros.sql_command("insert into btk.VO (Object_idObject,Kontakti_idKontakti,VOcol_num_vo,VOcol_date_add,Users_idUsers) values('" + id_db_obj + "','" + vars_form.transfer_vo3_vo_form + "','3','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','" + vars_form.user_login_id + "');");
                        }
                        string json2 = macros.wialon_request_new("&svc=item/update_custom_field&params={" +
                                                             "\"itemId\":\"" + wl_id + "\"," +
                                                             "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                             "\"callMode\":\"update\"," +
                                                             "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                             "\"v\":\"" + textBox_vo3.Text + "\"}");
                    }
                }
            }
        }

        private void email_textBox_TextChanged(object sender, EventArgs e)
        {
            if (email_textBox.Text != "")
            {
                string json = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                        "\"spec\":{" +
                                                        "\"itemsType\":\"user\"," +
                                                        "\"propName\":\"sys_name\"," +
                                                        "\"propValueMask\":\"*" + email_textBox.Text + "*\"," +
                                                        "\"sortType\":\"sys_name\"," +
                                                        "\"or_logic\":\"1\"}," +
                                                        "\"force\":\"1\"," +
                                                        "\"flags\":\"1\"," +
                                                        "\"from\":\"0\"," +
                                                        "\"to\":\"5\"}"); //запрашиваем все елементі с искомім имайлом
                var m1 = JsonConvert.DeserializeObject<RootObject>(json);

                List<string> list = new List<string>();
                foreach (var t in m1.items) //Get user account where name like email, contains @
                {
                    if (t.nm.Contains("@"))
                        list.Add(t.nm);
                }

                listBox_activation_list_search.DataSource = list;
                if (m1.items.Count >= 1)
                {
                    if (email_textBox.Text == m1.items[0].nm)//if entered in textbox username is exist in WL => disable button create new user, enable button for update right fo exist user, abd make it green
                    {
                        id_new_user = m1.items[0].id.ToString();
                        accaunt_name_textBox.Text = email_textBox.Text;
                        account_create_button.Enabled = false;
                        button_add_2_account.Enabled = true;
                        accaunt_name_textBox.BackColor = Color.YellowGreen;
                    }
                    else
                    {
                        accaunt_name_textBox.Text = "";
                        account_create_button.Enabled = true;
                        button_add_2_account.Enabled = false;
                        accaunt_name_textBox.BackColor = Color.Empty;
                    }
                }
            }
            else
            {
                listBox_activation_list_search.DataSource = null;
                accaunt_name_textBox.Text = "";
                account_create_button.Enabled = true;
                button_add_2_account.Enabled = false;
                accaunt_name_textBox.BackColor = Color.Empty;
            }
        }

        private void treeView_user_accounts_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (_unselectableNodes.Contains(e.Node))
            {
                e.Cancel = true;
            }
        }

        private void build_list_account()
        {
            //vars_form.id_object_for_activation = "1098";


            string json = macros.wialon_request_new("&svc=core/check_accessors&params={" +
                                                    "\"items\":[\"" + wl_id + "\"]," +
                                                    "\"flags\":\"1\"}");//Получаем айди всех елементов у которых есть доступ к данному объекту            

            var wl_accounts = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, Dictionary<string, string>>>>(json);
            string get = "";
            for (int index = 0; index < wl_accounts[wl_id].Values.Count; index++)
            {
                var item = wl_accounts.ElementAt(0);
                int key_index = item.Value.ElementAt(index).Key;
                get = get + "," + key_index.ToString();
                //string json3 = macros.wialon_request_new("&svc=core/search_items&params={" +
                //                                         "\"spec\":{" +
                //                                         "\"itemsType\":\"\"," +
                //                                         "\"propName\":\"sys_id\"," +
                //                                         "\"propValueMask\":\"" + key_index + "\", " +
                //                                         "\"sortType\":\"sys_name\"," +
                //                                         "\"or_logic\":\"1\"}," +
                //                                         "\"force\":\"1\"," +
                //                                         "\"flags\":\"1\"," +
                //                                         "\"from\":\"0\"," +
                //                                         "\"to\":\"0\"}");
                //var m2 = JsonConvert.DeserializeObject<RootObject>(json3);
                //string nm = m2.items[0].nm;
                //string acl = item.Value[key_index]["acl"].ToString();
                //string dacl = item.Value[key_index]["dacl"].ToString();
                //openWith1.Clear();
                //openWith1.Add("acl",acl);
                //openWith1.Add("dacl", dacl);
                //openWith.Add(nm, openWith1);
            }// стрим список айди в один запрос 
            string json2 = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"\"," +
                                                     "\"propName\":\"sys_id\"," +
                                                     "\"propValueMask\":\"" + get + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Получаем подробности от елементов для полуяения имени логина

            var m = JsonConvert.DeserializeObject<RootObject>(json2);

            treeView_user_accounts.Nodes.Clear();
            Font boldFont = new Font(treeView_user_accounts.Font, FontStyle.Bold);
            TreeNode node1 = new TreeNode("Кабінети користувача приєднані до авто");
            treeView_user_accounts.Nodes.Add(node1);
            _unselectableNodes.Add(node1);
            treeView_user_accounts.BeginUpdate();

            try
            {
                int tree_index = 0;
                for (int index = 0; index < m.items.Count; index++)
                {

                    if (m.items[index].nm.Contains("@"))
                    {
                        treeView_user_accounts.Nodes[0].Nodes.Add(new TreeNode(m.items[index].nm)); //выводим в дерево все учетки которые похожи на почту, ищем по @
                        treeView_user_accounts.Nodes[0].Nodes[tree_index].NodeFont = boldFont;


                        string json1 = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                                 "\"spec\":{" +
                                                                 "\"itemsType\":\"user\"," +
                                                                 "\"propName\":\"sys_name\"," +
                                                                 "\"propValueMask\":\"" + m.items[index].nm + "\"," +
                                                                 "\"sortType\":\"sys_name\"," +
                                                                 "\"or_logic\":\"1\"}," +
                                                                 "\"force\":\"1\"," +
                                                                 "\"flags\":\"1\"," +
                                                                 "\"from\":\"0\"," +
                                                                 "\"to\":\"0\"}"); //запрашиваем все елементі с искомім имайлом
                        var m1 = JsonConvert.DeserializeObject<RootObject>(json1);
                        string json3 = macros.wialon_request_new("&svc=user/get_items_access&params={" +
                                                                 "\"userId\":\"" + m1.items[0].id + "\"," +
                                                                 "\"directAccess\":\"true\"," +
                                                                 "\"itemSuperclass\":\"avl_unit\"," +
                                                                 "\"flags\":\"1\"}");
                        var m3 = JsonConvert.DeserializeObject<Dictionary<string, string>>(json3);
                        string d = "";
                        foreach (KeyValuePair<string, string> kvp in m3)
                        {
                            d = d + "," + kvp.Key;
                        }

                        string json4 = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                                 "\"spec\":{" +
                                                                 "\"itemsType\":\"avl_unit\"," +
                                                                 "\"propName\":\"sys_id\"," +
                                                                 "\"propValueMask\":\"" + d + "\"," +
                                                                 "\"sortType\":\"sys_name\"," +
                                                                 "\"or_logic\":\"1\"}," +
                                                                 "\"force\":\"1\"," +
                                                                 "\"flags\":\"1\"," +
                                                                 "\"from\":\"0\"," +
                                                                 "\"to\":\"0\"}"); //запрашиваем все елементі с искомім имайлом
                        var m4 = JsonConvert.DeserializeObject<RootObject>(json4);

                        for (int index1 = 0; index1 < m4.items.Count; index1++)
                        {
                            treeView_user_accounts.Nodes[0].Nodes[tree_index].Nodes.Add(m4.items[index1].nm);
                            _unselectableNodes.Add(treeView_user_accounts.Nodes[0].Nodes[tree_index].Nodes[index1]);
                        }
                        tree_index++;

                    }
                }
                treeView_user_accounts.EndUpdate();
                treeView_user_accounts.ExpandAll();


            }
            catch (Exception e)
            {
                string er = e.ToString();
            }

        }

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghjklmnpqrstuvwxyz123456789";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        private void account_create_button_Click(object sender, EventArgs e)
        {
            if (email_textBox.Text.Contains("@") & email_textBox.Text.Contains("."))
            {
                DialogResult dialogResult = MessageBox.Show("Стровити кабінет: " + email_textBox.Text + "?", "Створити?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //generete password
                    //---------------------------------------------------------------
                    string pass = "";

                    if (checkBox_manual_pass.Checked == true)
                    {
                        if (textBox_account_pss.Text == "")
                        {
                            pass = CreatePassword(6);
                            textBox_account_pss.Text = pass;
                        }
                        else
                        {
                            pass = textBox_account_pss.Text;
                        }
                        checkBox_manual_pass.Checked = false;
                    }
                    else
                    {
                        pass = CreatePassword(6);
                        textBox_account_pss.Text = pass;
                    }
                    //---------------------------------------------------------------


                    // cteate new user 
                    string created_user_answer = macros.wialon_request_new("&svc=core/create_user&params={" +
                                                            "\"creatorId\":\"" + vars_form.wl_user_id + "\"," +
                                                            "\"name\":\"" + email_textBox.Text + "\"," +
                                                            "\"password\":\"" + pass + "\"," +
                                                            "\"dataFlags\":\"1\"}");
                    var created_user_data = JsonConvert.DeserializeObject<RootObject>(created_user_answer);

                    // create resours name: newuser
                    string created_resource_answer = macros.wialon_request_new("&svc=core/create_resource&params={" +
                                                            "\"creatorId\":\"" + created_user_data.item.id + "\"," +
                                                            "\"name\":\"" + email_textBox.Text + "\"," +
                                                            "\"dataFlags\":\"1\"," +
                                                            "\"skipCreatorCheck\":\"1\"}");
                    var created_resource_data = JsonConvert.DeserializeObject<RootObject>(created_resource_answer);

                    // create resours name: newuser_user
                    string created_resource_user_answer = macros.wialon_request_new("&svc=core/create_resource&params={" +
                                                             "\"creatorId\":\"" + created_user_data.item.id + "\"," +
                                                             "\"name\":\"" + email_textBox.Text + "_user" + "\"," +
                                                             "\"dataFlags\":\"1\"," +
                                                             "\"skipCreatorCheck\":\"1\"}");
                    var created_resource_user_data = JsonConvert.DeserializeObject<RootObject>(created_resource_user_answer);


                    //Доступ на объект for nwe user
                    string set_right_object_answer = macros.wialon_request_new("&svc=user/update_item_access&params={" +
                                                            "\"userId\":\"" + created_user_data.item.id + "\"," +
                                                            "\"itemId\":\"" + wl_id + "\"," +
                                                            "\"accessMask\":\"550611455877‬\"}");
                    var set_right_object = JsonConvert.DeserializeObject<RootObject>(set_right_object_answer);

                    DataTable users = macros.GetData("SELECT user_id_wl FROM btk.Users where accsess_lvl = '1' or accsess_lvl = '5' or accsess_lvl = '8' or accsess_lvl = '9';");

                    List<DataRow> woruser_list = users.AsEnumerable().ToList();
                    foreach (DataRow workuser in woruser_list)
                    {
                        if (workuser[0].ToString() != vars_form.user_login_id)
                        {

                            //set full Accsess client account for workuser 
                            string set_right_user_suport_answer = macros.wialon_request_new("&svc=user/update_item_access&params={" +
                                                                     "\"userId\":\"" + workuser[0].ToString() + "\"," +
                                                                     "\"itemId\":\"" + created_user_data.item.id + "\"," +
                                                                     "\"accessMask\":\"-1\"}");
                            var set_right_user_suport_ = JsonConvert.DeserializeObject<RootObject>(set_right_user_suport_answer);

                            //set full Accsess client resourse "username" for workuser 
                            string set_right_resource_support_answer = macros.wialon_request_new("&svc=user/update_item_access&params={" +
                                                                     "\"userId\":\"" + workuser[0].ToString() + "\"," +
                                                                     "\"itemId\":\"" + created_resource_data.item.id + "\"," +
                                                                     "\"accessMask\":\"-1\"}");
                            var set_right_resourc_support = JsonConvert.DeserializeObject<RootObject>(set_right_resource_support_answer);

                            //set full Accsess client resourse "username.._user" for workuser 
                            string set_right_resource_user_support_answer = macros.wialon_request_new("&svc=user/update_item_access&params={" +
                                                                     "\"userId\":\"" + workuser[0].ToString() + "\"," +
                                                                     "\"itemId\":\"" + created_resource_user_data.item.id + "\"," +
                                                                     "\"accessMask\":\"-1\"}");
                            var set_right_resource_user_support = JsonConvert.DeserializeObject<RootObject>(set_right_resource_user_support_answer);
                        }
                        else
                        {
                        }

                    }

                    //Доступ на ресурс: username
                    string set_right_resource_answer = macros.wialon_request_new("&svc=user/update_item_access&params={" +
                                                             "\"userId\":\"" + created_user_data.item.id + "\"," +
                                                             "\"itemId\":\"" + created_resource_data.item.id + "\"," +
                                                             "\"accessMask\":\"4648339329\"}");
                    var set_right_resource = JsonConvert.DeserializeObject<RootObject>(set_right_resource_answer);

                    //Доступ на ресурс: username_user
                    string set_right_resource_user_answer = macros.wialon_request_new("&svc=user/update_item_access&params={" +
                                                             "\"userId\":\"" + created_user_data.item.id + "\"," +
                                                             "\"itemId\":\"" + created_resource_user_data.item.id + "\"," +
                                                             "\"accessMask\":\"52785134440321\"}");
                    var set_right_resource_user = JsonConvert.DeserializeObject<RootObject>(set_right_resource_user_answer);



                    //get product id from id object
                    string product_id = get_product_from_id_object(wl_id);

                    //create notif depends product id

                    if (product_id == "10" || product_id == "11" || product_id == "13" || product_id == "14")//CNTP_910, CNTK_910
                    {
                        create_notif_910(email_textBox.Text, created_user_data.item.id, wl_id, created_resource_data.item.id, created_resource_user_data.item.id);
                    }
                    else if (product_id == "2" || product_id == "3")//CNTP, CNTK
                    {
                        create_notif_730(email_textBox.Text, created_user_data.item.id, wl_id, created_resource_data.item.id, created_resource_user_data.item.id);
                    }
                    else
                    {
                        MessageBox.Show("Невідомий продукт, сповіщення не створені");
                    }
                    

                    //Save in db client account

                    macros.GetData("insert into btk.Client_accounts (name, pass, date, reason, Object_idObject, Users_idUsers) value ('" + email_textBox.Text + "','" + pass + "','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','Create account from detail','" + id_db_obj + "','" + vars_form.user_login_id + "');");


                    //update treeView_user_accounts after making chenge
                    build_list_account();
                    email_textBox_TextChanged(email_textBox, EventArgs.Empty);

                }
                else if (dialogResult == DialogResult.No)
                {
                }
            }
            else
            {
                MessageBox.Show("Перевірьте імя користувача");
            }
        }

        private void button_add_2_account_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Надати авто в доступ: " + email_textBox.Text + "?", "Дозволити?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string user_data_answer = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                         "\"spec\":{" +
                                                         "\"itemsType\":\"user\"," +
                                                         "\"propName\":\"sys_name\"," +
                                                         "\"propValueMask\":\"" + email_textBox.Text + "\"," +
                                                         "\"sortType\":\"sys_name\"," +
                                                         "\"or_logic\":\"1\"}," +
                                                         "\"force\":\"1\"," +
                                                         "\"flags\":\"1\"," +
                                                         "\"from\":\"0\"," +
                                                         "\"to\":\"0\"}"); //запрашиваем все елементі с искомім имайлом
                var user_data = JsonConvert.DeserializeObject<RootObject>(user_data_answer);

                ///////////
                //Доступ на объект
                string set_object_accsess_answer = macros.wialon_request_new("&svc=user/update_item_access&params={" +
                                                         "\"userId\":\"" + user_data.items[0].id + "\"," +
                                                         "\"itemId\":\"" + wl_id + "\"," +
                                                         "\"accessMask\":\"550611455877\"}");
                var set_object_accsess = JsonConvert.DeserializeObject<RootObject>(set_object_accsess_answer);

                //get data resource for selected user account
                string get_resource_user_answer = macros.wl_core_search_items("avl_resource", "sys_name", email_textBox.Text, "sys_name", 1, 0, 1);
                var get_resource_user = JsonConvert.DeserializeObject<RootObject>(get_resource_user_answer);

                //get data resource_user for selected user account
                string get_resource_user_user_answer = macros.wl_core_search_items("avl_resource", "sys_name", email_textBox.Text + "_user", "sys_name", 1, 0, 1);
                var get_resource_user_user = JsonConvert.DeserializeObject<RootObject>(get_resource_user_user_answer);



                //get product id from id object
                string product_id = get_product_from_id_object(wl_id);

                //create notif depends product id

                if (product_id == "10" || product_id == "11")//CNTP_910, CNTK_910
                {
                    create_notif_910(email_textBox.Text, user_data.items[0].id, wl_id, get_resource_user.items[0].id, get_resource_user_user.items[0].id);
                }
                else if (product_id == "2" || product_id == "3")//CNTP, CNTK
                {
                    create_notif_730(email_textBox.Text, user_data.items[0].id, wl_id, get_resource_user.items[0].id, get_resource_user_user.items[0].id);
                }
                else
                {
                    MessageBox.Show("Невідомий продукт, сповіщення не створені");
                }

                //log user action
                macros.LogUserAction(vars_form.user_login_id, "Дозволити користувачу перегляд авто", "", "Надано доступ Account: " + email_textBox.Text + "до обєкту" + wl_id, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));

                //update treeView_user_accounts after making chenge
                build_list_account();

            }
            else if (dialogResult == DialogResult.No)
            {
            }
        }

        private string get_product_from_id_object(string id_oject)
        {
            //get product of testing device
            string get_produt_testing_device = macros.sql_command("select " +
                                                                   "products_has_Tarif.products_idproducts " +
                                                                   "from " +
                                                                   "btk.object_subscr, btk.Subscription, btk.products_has_Tarif, btk.Object " +
                                                                   "where " +
                                                                   "Object.Object_id_wl = " + id_oject + " and " +
                                                                   "Object.idObject=Object_idObject and " +
                                                                   "Subscription_idSubscr=idSubscr and " +
                                                                   "products_has_Tarif_idproducts_has_Tarif=idproducts_has_Tarif;");

            return get_produt_testing_device;
        }

        // CNTP_910 create nitif for new user account
        private void create_notif_910(string user_account_name, int user_account_id, string object_id, int resours_id, int resours_user_id)
        {
            //Cработка: Датчик удара/наклона/буксировки
            string CreteNotifAnswer = macros.wialon_request_new("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Cработка: Датчик удара/наклона\"," +                            /* название */
                                                            "\"txt\":\"%UNIT%: сработал датчик удара/наклона. Время сработки: %MSG_TIME%. В %POS_TIME% автомобиль находился около '%LOCATION%'.\"," +     /* текст уведомления */
                                                            "\"ta\":\"0\"," +                                                   /* время активации (UNIX формат) */
                                                            "\"td\":\"0\"," +                                                   /* время деактивации (UNIX формат) */
                                                            "\"ma\":\"0\"," +                                                   /* максимальное количество срабатываний (0 - не ограничено) */
                                                            "\"mmtd\":\"0\"," +                                                 /* максимальный временной интервал между сообщениями (секунд) */
                                                            "\"cdt\":\"0\"," +                                                  /* таймаут срабатывания(секунд) */
                                                            "\"mast\":\"0\"," +                                                 /* минимальная продолжительность тревожного состояния (секунд) */
                                                            "\"mpst\":\"30\"," +                                                 /* минимальная продолжительность предыдущего состояния (секунд) */
                                                            "\"cp\":\"0\"," +                                                   /* период контроля относительно текущего времени (секунд) */
                                                            "\"fl\":\"0\"," +                                                   /* флаги: 0=уведомление срабатывает на первое сообщение, 1=уведомление срабатывает на каждое сообщение, 2=уведомление выключено */
                                                            "\"tz\":\"7200\"," +                                                /* часовой пояс */
                                                            "\"la\":\"ru\"," +                                                  /* язык пользователя (двухбуквенный код) */
                                                            "\"ac\":\"0\"," +                                                   /* количество срабатываний */
                                                            "\"un\":[" + object_id + "]," +     /* массив ID объектов/групп объектов */
                                                            "\"sch\":{" +                                                       /* ограничение по времени */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"ctrl_sch\":{" +                                                  /* расписание периодов ограничения количества срабатывания */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"trg\":{" +                                                       /* контроль */
                                                                        "\"t\":\"sensor_value\"," +                             /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_kontrolja */
                                                                        "\"p\":{" +
                                                                                "\"lower_bound\":\"1\"," +                          /* значение датчика от */
                                                                                "\"merge\":\"1\"," +                                /* одинаковые датчики: 0 - считать отдельно, 1 - суммировать значения */
                                                                                "\"prev_msg_diff\":\"0\"," +                        /* флаг, позволяющий сформировать диапазон для текущего значения с помощью предыдущего значения(prev) следующим образом: [prev+lower_bound ; prev+upper_bound] -- таким образом диапазон для текущего значения всегда относителен и зависит от предыдущего значения; 0 - выключить опцию, 1 - включить опцию */
                                                                                "\"sensor_name_mask\":\"Сработка датчика ударов\"," +               /* маска названия датчика */
                                                                                "\"sensor_type\":\"digital\"," +                    /* тип датчика */
                                                                                "\"type\":\"0\"," +                                 /* срабатывать: 0 - в рамках установленных значений, 1 - за пределами установленных значений */
                                                                                "\"upper_bound\":\"1\"" +                           /* значение датчика до */
                                                                        "}" +
                                                            "}," +
                                                            "\"act\":[" +                                                       /* действия */
                                                                        "{" +
                                                                            "\"t\":\"message\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"color\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"email\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"email_to\":\"" + user_account_name + "\"," +
                                                                                    "\"html\":\"0\"," +
                                                                                    "\"img_attach\":\"0\"," +
                                                                                    "\"subj\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"event\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"flags\":\"0\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"mobile_apps\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

            //Блокировка двигателя
            string CreteNotifAnswer2 = macros.wialon_request_new("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Блокировка двигателя\"," +                                 /* название */
                                                            "\"txt\":\"%UNIT%: Произошла блокировка двигателя. Время сработки: %MSG_TIME%  В %POS_TIME% автомобиль двигался со скоростью %SPEED% около '%LOCATION%'.\"," +     /* текст уведомления */
                                                            "\"ta\":\"0\"," +                                                   /* время активации (UNIX формат) */
                                                            "\"td\":\"0\"," +                                                   /* время деактивации (UNIX формат) */
                                                            "\"ma\":\"0\"," +                                                   /* максимальное количество срабатываний (0 - не ограничено) */
                                                            "\"mmtd\":\"0\"," +                                                 /* максимальный временной интервал между сообщениями (секунд) */
                                                            "\"cdt\":\"0\"," +                                                  /* таймаут срабатывания(секунд) */
                                                            "\"mast\":\"0\"," +                                                 /* минимальная продолжительность тревожного состояния (секунд) */
                                                            "\"mpst\":\"30\"," +                                                 /* минимальная продолжительность предыдущего состояния (секунд) */
                                                            "\"cp\":\"0\"," +                                                   /* период контроля относительно текущего времени (секунд) */
                                                            "\"fl\":\"0\"," +                                                   /* флаги: 0=уведомление срабатывает на первое сообщение, 1=уведомление срабатывает на каждое сообщение, 2=уведомление выключено */
                                                            "\"tz\":\"7200\"," +                                                /* часовой пояс */
                                                            "\"la\":\"ru\"," +                                                  /* язык пользователя (двухбуквенный код) */
                                                            "\"ac\":\"0\"," +                                                   /* количество срабатываний */
                                                            "\"un\":[" + object_id + "]," +     /* массив ID объектов/групп объектов */
                                                            "\"sch\":{" +                                                       /* ограничение по времени */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"ctrl_sch\":{" +                                                  /* расписание периодов ограничения количества срабатывания */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"trg\":{" +                                                       /* контроль */
                                                                        "\"t\":\"sensor_value\"," +                             /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_kontrolja */
                                                                        "\"p\":{" +
                                                                                "\"lower_bound\":\"1\"," +                          /* значение датчика от */
                                                                                "\"merge\":\"1\"," +                                /* одинаковые датчики: 0 - считать отдельно, 1 - суммировать значения */
                                                                                "\"prev_msg_diff\":\"0\"," +                        /* флаг, позволяющий сформировать диапазон для текущего значения с помощью предыдущего значения(prev) следующим образом: [prev+lower_bound ; prev+upper_bound] -- таким образом диапазон для текущего значения всегда относителен и зависит от предыдущего значения; 0 - выключить опцию, 1 - включить опцию */
                                                                                "\"sensor_name_mask\":\"Автоматическая блокировка двигателя\"," +               /* маска названия датчика */
                                                                                "\"sensor_type\":\"digital\"," +                    /* тип датчика */
                                                                                "\"type\":\"0\"," +                                 /* срабатывать: 0 - в рамках установленных значений, 1 - за пределами установленных значений */
                                                                                "\"upper_bound\":\"1\"" +                           /* значение датчика до */
                                                                        "}" +
                                                            "}," +
                                                            "\"act\":[" +                                                       /* действия */
                                                                        "{" +
                                                                            "\"t\":\"message\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"color\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"email\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"email_to\":\"" + user_account_name + "\"," +
                                                                                    "\"html\":\"0\"," +
                                                                                    "\"img_attach\":\"0\"," +
                                                                                    "\"subj\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"event\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"flags\":\"0\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"mobile_apps\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

            //Низкое напряжения АКБ
            string CreteNotifAnswer3 = macros.wialon_request_new("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Низкое напряжения АКБ\"," +                                 /* название */
                                                            "\"txt\":\"%UNIT%: низкое напряжение АКБ. Автомобиль находился около '%LOCATION%'.\"," +     /* текст уведомления */
                                                            "\"ta\":\"0\"," +                                                   /* время активации (UNIX формат) */
                                                            "\"td\":\"0\"," +                                                   /* время деактивации (UNIX формат) */
                                                            "\"ma\":\"0\"," +                                                   /* максимальное количество срабатываний (0 - не ограничено) */
                                                            "\"mmtd\":\"0\"," +                                                 /* максимальный временной интервал между сообщениями (секунд) */
                                                            "\"cdt\":\"0\"," +                                                  /* таймаут срабатывания(секунд) */
                                                            "\"mast\":\"300\"," +                                                 /* минимальная продолжительность тревожного состояния (секунд) */
                                                            "\"mpst\":\"60\"," +                                                 /* минимальная продолжительность предыдущего состояния (секунд) */
                                                            "\"cp\":\"0\"," +                                                   /* период контроля относительно текущего времени (секунд) */
                                                            "\"fl\":\"0\"," +                                                   /* флаги: 0=уведомление срабатывает на первое сообщение, 1=уведомление срабатывает на каждое сообщение, 2=уведомление выключено */
                                                            "\"tz\":\"7200\"," +                                                /* часовой пояс */
                                                            "\"la\":\"ru\"," +                                                  /* язык пользователя (двухбуквенный код) */
                                                            "\"ac\":\"0\"," +                                                   /* количество срабатываний */
                                                            "\"un\":[" + object_id + "]," +     /* массив ID объектов/групп объектов */
                                                            "\"sch\":{" +                                                       /* ограничение по времени */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"ctrl_sch\":{" +                                                  /* расписание периодов ограничения количества срабатывания */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"trg\":{" +                                                       /* контроль */
                                                                        "\"t\":\"sensor_value\"," +                             /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_kontrolja */
                                                                        "\"p\":{" +
                                                                                "\"lower_bound\":\"0\"," +                          /* значение датчика от */
                                                                                "\"merge\":\"1\"," +                                /* одинаковые датчики: 0 - считать отдельно, 1 - суммировать значения */
                                                                                "\"prev_msg_diff\":\"0\"," +                        /* флаг, позволяющий сформировать диапазон для текущего значения с помощью предыдущего значения(prev) следующим образом: [prev+lower_bound ; prev+upper_bound] -- таким образом диапазон для текущего значения всегда относителен и зависит от предыдущего значения; 0 - выключить опцию, 1 - включить опцию */
                                                                                "\"sensor_name_mask\":\"Напряжение АКБ\"," +               /* маска названия датчика */
                                                                                "\"sensor_type\":\"voltage\"," +                    /* тип датчика */
                                                                                "\"type\":\"0\"," +                                 /* срабатывать: 0 - в рамках установленных значений, 1 - за пределами установленных значений */
                                                                                "\"upper_bound\":\"11.08\"" +                           /* значение датчика до */
                                                                        "}" +
                                                            "}," +
                                                            "\"act\":[" +                                                       /* действия */
                                                                        "{" +
                                                                            "\"t\":\"message\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"color\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"email\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"email_to\":\"" + user_account_name + "\"," +
                                                                                    "\"html\":\"0\"," +
                                                                                    "\"img_attach\":\"0\"," +
                                                                                    "\"subj\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"event\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"flags\":\"0\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"mobile_apps\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

            //Сработка: Датчики взлома
            string CreteNotifAnswer4 = macros.wialon_request_new("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Сработка: Датчики взлома\"," +                                 /* название */
                                                            "\"txt\":\"%UNIT%: Несанкционированное открытие дверей, капота или багажника. Время сработки: %MSG_TIME%. Автомобиль находился около '%LOCATION%'.\"," +     /* текст уведомления */
                                                            "\"ta\":\"0\"," +                                                   /* время активации (UNIX формат) */
                                                            "\"td\":\"0\"," +                                                   /* время деактивации (UNIX формат) */
                                                            "\"ma\":\"0\"," +                                                   /* максимальное количество срабатываний (0 - не ограничено) */
                                                            "\"mmtd\":\"0\"," +                                                 /* максимальный временной интервал между сообщениями (секунд) */
                                                            "\"cdt\":\"0\"," +                                                  /* таймаут срабатывания(секунд) */
                                                            "\"mast\":\"0\"," +                                                 /* минимальная продолжительность тревожного состояния (секунд) */
                                                            "\"mpst\":\"60\"," +                                                 /* минимальная продолжительность предыдущего состояния (секунд) */
                                                            "\"cp\":\"0\"," +                                                   /* период контроля относительно текущего времени (секунд) */
                                                            "\"fl\":\"0\"," +                                                   /* флаги: 0=уведомление срабатывает на первое сообщение, 1=уведомление срабатывает на каждое сообщение, 2=уведомление выключено */
                                                            "\"tz\":\"7200\"," +                                                /* часовой пояс */
                                                            "\"la\":\"ru\"," +                                                  /* язык пользователя (двухбуквенный код) */
                                                            "\"ac\":\"0\"," +                                                   /* количество срабатываний */
                                                            "\"un\":[" + object_id + "]," +     /* массив ID объектов/групп объектов */
                                                            "\"sch\":{" +                                                       /* ограничение по времени */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"ctrl_sch\":{" +                                                  /* расписание периодов ограничения количества срабатывания */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"trg\":{" +                                                       /* контроль */
                                                                        "\"t\":\"sensor_value\"," +                             /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_kontrolja */
                                                                        "\"p\":{" +
                                                                                "\"lower_bound\":\"1\"," +                          /* значение датчика от */
                                                                                "\"merge\":\"1\"," +                                /* одинаковые датчики: 0 - считать отдельно, 1 - суммировать значения */
                                                                                "\"prev_msg_diff\":\"0\"," +                        /* флаг, позволяющий сформировать диапазон для текущего значения с помощью предыдущего значения(prev) следующим образом: [prev+lower_bound ; prev+upper_bound] -- таким образом диапазон для текущего значения всегда относителен и зависит от предыдущего значения; 0 - выключить опцию, 1 - включить опцию */
                                                                                "\"sensor_name_mask\":\"Сработка открытие дверей в охране\"," +               /* маска названия датчика */
                                                                                "\"sensor_type\":\"digital\"," +                    /* тип датчика */
                                                                                "\"type\":\"0\"," +                                 /* срабатывать: 0 - в рамках установленных значений, 1 - за пределами установленных значений */
                                                                                "\"upper_bound\":\"1\"" +                           /* значение датчика до */
                                                                        "}" +
                                                            "}," +
                                                            "\"act\":[" +                                                       /* действия */
                                                                        "{" +
                                                                            "\"t\":\"message\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"color\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"email\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"email_to\":\"" + user_account_name + "\"," +
                                                                                    "\"html\":\"0\"," +
                                                                                    "\"img_attach\":\"0\"," +
                                                                                    "\"subj\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"event\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"flags\":\"0\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"mobile_apps\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );



            //Сработка: Датчики взлома
            string CreteNotifAnswer5 = macros.wialon_request_new("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"ТРЕВОЖНАЯ КНОПКА\"," +                                 /* название */
                                                            "\"txt\":\"%UNIT%: НАЖАТА ТРЕВОЖНАЯ КНОПКА!!!!! Время сработки: %MSG_TIME%. В %POS_TIME% объект двигался со скоростью %SPEED% около '%LOCATION%'.\"," +     /* текст уведомления */
                                                            "\"ta\":\"0\"," +                                                   /* время активации (UNIX формат) */
                                                            "\"td\":\"0\"," +                                                   /* время деактивации (UNIX формат) */
                                                            "\"ma\":\"0\"," +                                                   /* максимальное количество срабатываний (0 - не ограничено) */
                                                            "\"mmtd\":\"0\"," +                                                 /* максимальный временной интервал между сообщениями (секунд) */
                                                            "\"cdt\":\"0\"," +                                                  /* таймаут срабатывания(секунд) */
                                                            "\"mast\":\"0\"," +                                                 /* минимальная продолжительность тревожного состояния (секунд) */
                                                            "\"mpst\":\"30\"," +                                                 /* минимальная продолжительность предыдущего состояния (секунд) */
                                                            "\"cp\":\"0\"," +                                                   /* период контроля относительно текущего времени (секунд) */
                                                            "\"fl\":\"0\"," +                                                   /* флаги: 0=уведомление срабатывает на первое сообщение, 1=уведомление срабатывает на каждое сообщение, 2=уведомление выключено */
                                                            "\"tz\":\"7200\"," +                                                /* часовой пояс */
                                                            "\"la\":\"ru\"," +                                                  /* язык пользователя (двухбуквенный код) */
                                                            "\"ac\":\"0\"," +                                                   /* количество срабатываний */
                                                            "\"un\":[" + object_id + "]," +     /* массив ID объектов/групп объектов */
                                                            "\"sch\":{" +                                                       /* ограничение по времени */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"ctrl_sch\":{" +                                                  /* расписание периодов ограничения количества срабатывания */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"trg\":{" +                                                       /* контроль */
                                                                        "\"t\":\"sensor_value\"," +                             /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_kontrolja */
                                                                        "\"p\":{" +
                                                                                "\"lower_bound\":\"1\"," +                          /* значение датчика от */
                                                                                "\"merge\":\"1\"," +                                /* одинаковые датчики: 0 - считать отдельно, 1 - суммировать значения */
                                                                                "\"prev_msg_diff\":\"0\"," +                        /* флаг, позволяющий сформировать диапазон для текущего значения с помощью предыдущего значения(prev) следующим образом: [prev+lower_bound ; prev+upper_bound] -- таким образом диапазон для текущего значения всегда относителен и зависит от предыдущего значения; 0 - выключить опцию, 1 - включить опцию */
                                                                                "\"sensor_name_mask\":\"Тревожная кнопка_\"," +               /* маска названия датчика */
                                                                                "\"sensor_type\":\"digital\"," +                    /* тип датчика */
                                                                                "\"type\":\"0\"," +                                 /* срабатывать: 0 - в рамках установленных значений, 1 - за пределами установленных значений */
                                                                                "\"upper_bound\":\"1\"" +                           /* значение датчика до */
                                                                        "}" +
                                                            "}," +
                                                            "\"act\":[" +                                                       /* действия */
                                                                        "{" +
                                                                            "\"t\":\"message\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"color\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"email\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"email_to\":\"" + user_account_name + "\"," +
                                                                                    "\"html\":\"0\"," +
                                                                                    "\"img_attach\":\"0\"," +
                                                                                    "\"subj\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"event\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"flags\":\"0\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"mobile_apps\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

            //Сработка датчика глушения
            string CreteNotifAnswer6 = macros.wialon_request_new("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Сработка: датчика глушения\"," +                                 /* название */
                                                            "\"txt\":\"%UNIT%: Сработка датчика глушения. Время сработки: %MSG_TIME%. В %POS_TIME% объект двигался со скоростью %SPEED% около '%LOCATION%'.\"," +     /* текст уведомления */
                                                            "\"ta\":\"0\"," +                                                   /* время активации (UNIX формат) */
                                                            "\"td\":\"0\"," +                                                   /* время деактивации (UNIX формат) */
                                                            "\"ma\":\"0\"," +                                                   /* максимальное количество срабатываний (0 - не ограничено) */
                                                            "\"mmtd\":\"0\"," +                                                 /* максимальный временной интервал между сообщениями (секунд) */
                                                            "\"cdt\":\"0\"," +                                                  /* таймаут срабатывания(секунд) */
                                                            "\"mast\":\"0\"," +                                                 /* минимальная продолжительность тревожного состояния (секунд) */
                                                            "\"mpst\":\"30\"," +                                                 /* минимальная продолжительность предыдущего состояния (секунд) */
                                                            "\"cp\":\"0\"," +                                                   /* период контроля относительно текущего времени (секунд) */
                                                            "\"fl\":\"0\"," +                                                   /* флаги: 0=уведомление срабатывает на первое сообщение, 1=уведомление срабатывает на каждое сообщение, 2=уведомление выключено */
                                                            "\"tz\":\"7200\"," +                                                /* часовой пояс */
                                                            "\"la\":\"ru\"," +                                                  /* язык пользователя (двухбуквенный код) */
                                                            "\"ac\":\"0\"," +                                                   /* количество срабатываний */
                                                            "\"un\":[" + object_id + "]," +     /* массив ID объектов/групп объектов */
                                                            "\"sch\":{" +                                                       /* ограничение по времени */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"ctrl_sch\":{" +                                                  /* расписание периодов ограничения количества срабатывания */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"trg\":{" +                                                       /* контроль */
                                                                        "\"t\":\"sensor_value\"," +                             /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_kontrolja */
                                                                        "\"p\":{" +
                                                                                "\"lower_bound\":\"1\"," +                          /* значение датчика от */
                                                                                "\"merge\":\"1\"," +                                /* одинаковые датчики: 0 - считать отдельно, 1 - суммировать значения */
                                                                                "\"prev_msg_diff\":\"0\"," +                        /* флаг, позволяющий сформировать диапазон для текущего значения с помощью предыдущего значения(prev) следующим образом: [prev+lower_bound ; prev+upper_bound] -- таким образом диапазон для текущего значения всегда относителен и зависит от предыдущего значения; 0 - выключить опцию, 1 - включить опцию */
                                                                                "\"sensor_name_mask\":\"Сработка датчика глушения\"," +               /* маска названия датчика */
                                                                                "\"sensor_type\":\"digital\"," +                    /* тип датчика */
                                                                                "\"type\":\"0\"," +                                 /* срабатывать: 0 - в рамках установленных значений, 1 - за пределами установленных значений */
                                                                                "\"upper_bound\":\"1\"" +                           /* значение датчика до */
                                                                        "}" +
                                                            "}," +
                                                            "\"act\":[" +                                                       /* действия */
                                                                        "{" +
                                                                            "\"t\":\"message\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"color\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"email\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"email_to\":\"" + user_account_name + "\"," +
                                                                                    "\"html\":\"0\"," +
                                                                                    "\"img_attach\":\"0\"," +
                                                                                    "\"subj\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"event\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"flags\":\"0\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"mobile_apps\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

        }

        // CNTP_730 create nitif for new user account
        private void create_notif_730(string user_account_name, int user_account_id, string object_id, int resours_id, int resours_user_id)
        {
            //Cработка: Датчик удара/наклона/буксировки
            string CreteNotifAnswer = macros.wialon_request_new("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Cработка: Датчик удара/наклона\"," +                            /* название */
                                                            "\"txt\":\"%UNIT%: сработал датчик удара/наклона/буксировки. Время сработки: %MSG_TIME%. В %POS_TIME% автомобиль находился около '%LOCATION%'.'.\"," +     /* текст уведомления */
                                                            "\"ta\":\"0\"," +                                                   /* время активации (UNIX формат) */
                                                            "\"td\":\"0\"," +                                                   /* время деактивации (UNIX формат) */
                                                            "\"ma\":\"0\"," +                                                   /* максимальное количество срабатываний (0 - не ограничено) */
                                                            "\"mmtd\":\"0\"," +                                                 /* максимальный временной интервал между сообщениями (секунд) */
                                                            "\"cdt\":\"0\"," +                                                  /* таймаут срабатывания(секунд) */
                                                            "\"mast\":\"0\"," +                                                 /* минимальная продолжительность тревожного состояния (секунд) */
                                                            "\"mpst\":\"0\"," +                                                 /* минимальная продолжительность предыдущего состояния (секунд) */
                                                            "\"cp\":\"0\"," +                                                   /* период контроля относительно текущего времени (секунд) */
                                                            "\"fl\":\"0\"," +                                                   /* флаги: 0=уведомление срабатывает на первое сообщение, 1=уведомление срабатывает на каждое сообщение, 2=уведомление выключено */
                                                            "\"tz\":\"7200\"," +                                                /* часовой пояс */
                                                            "\"la\":\"ru\"," +                                                  /* язык пользователя (двухбуквенный код) */
                                                            "\"ac\":\"0\"," +                                                   /* количество срабатываний */
                                                            "\"un\":[" + object_id + "]," +     /* массив ID объектов/групп объектов */
                                                            "\"sch\":{" +                                                       /* ограничение по времени */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"ctrl_sch\":{" +                                                  /* расписание периодов ограничения количества срабатывания */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"trg\":{" +                                                       /* контроль */
                                                                        "\"t\":\"sensor_value\"," +                             /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_kontrolja */
                                                                        "\"p\":{" +
                                                                                "\"lower_bound\":\"1\"," +                          /* значение датчика от */
                                                                                "\"merge\":\"1\"," +                                /* одинаковые датчики: 0 - считать отдельно, 1 - суммировать значения */
                                                                                "\"prev_msg_diff\":\"0\"," +                        /* флаг, позволяющий сформировать диапазон для текущего значения с помощью предыдущего значения(prev) следующим образом: [prev+lower_bound ; prev+upper_bound] -- таким образом диапазон для текущего значения всегда относителен и зависит от предыдущего значения; 0 - выключить опцию, 1 - включить опцию */
                                                                                "\"sensor_name_mask\":\"Датчик удара\"," +               /* маска названия датчика */
                                                                                "\"sensor_type\":\"digital\"," +                    /* тип датчика */
                                                                                "\"type\":\"0\"," +                                 /* срабатывать: 0 - в рамках установленных значений, 1 - за пределами установленных значений */
                                                                                "\"upper_bound\":\"1\"" +                           /* значение датчика до */
                                                                        "}" +
                                                            "}," +
                                                            "\"act\":[" +                                                       /* действия */
                                                                        "{" +
                                                                            "\"t\":\"message\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"color\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"email\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"email_to\":\"" + user_account_name + "\"," +
                                                                                    "\"html\":\"0\"," +
                                                                                    "\"img_attach\":\"0\"," +
                                                                                    "\"subj\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"event\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"flags\":\"0\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"mobile_apps\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

            //Блокировка двигателя
            string CreteNotifAnswer2 = macros.wialon_request_new("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Блокировка двигателя\"," +                                 /* название */
                                                            "\"txt\":\"%UNIT%: Произошла блокировка двигателя. Время сработки: %MSG_TIME%  В %POS_TIME% автомобиль двигался со скоростью %SPEED% около '%LOCATION%'.\"," +     /* текст уведомления */
                                                            "\"ta\":\"0\"," +                                                   /* время активации (UNIX формат) */
                                                            "\"td\":\"0\"," +                                                   /* время деактивации (UNIX формат) */
                                                            "\"ma\":\"0\"," +                                                   /* максимальное количество срабатываний (0 - не ограничено) */
                                                            "\"mmtd\":\"0\"," +                                                 /* максимальный временной интервал между сообщениями (секунд) */
                                                            "\"cdt\":\"0\"," +                                                  /* таймаут срабатывания(секунд) */
                                                            "\"mast\":\"0\"," +                                                 /* минимальная продолжительность тревожного состояния (секунд) */
                                                            "\"mpst\":\"0\"," +                                                 /* минимальная продолжительность предыдущего состояния (секунд) */
                                                            "\"cp\":\"0\"," +                                                   /* период контроля относительно текущего времени (секунд) */
                                                            "\"fl\":\"0\"," +                                                   /* флаги: 0=уведомление срабатывает на первое сообщение, 1=уведомление срабатывает на каждое сообщение, 2=уведомление выключено */
                                                            "\"tz\":\"7200\"," +                                                /* часовой пояс */
                                                            "\"la\":\"ru\"," +                                                  /* язык пользователя (двухбуквенный код) */
                                                            "\"ac\":\"0\"," +                                                   /* количество срабатываний */
                                                            "\"un\":[" + object_id + "]," +     /* массив ID объектов/групп объектов */
                                                            "\"sch\":{" +                                                       /* ограничение по времени */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"ctrl_sch\":{" +                                                  /* расписание периодов ограничения количества срабатывания */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"trg\":{" +                                                       /* контроль */
                                                                        "\"t\":\"sensor_value\"," +                             /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_kontrolja */
                                                                        "\"p\":{" +
                                                                                "\"lower_bound\":\"1\"," +                          /* значение датчика от */
                                                                                "\"merge\":\"1\"," +                                /* одинаковые датчики: 0 - считать отдельно, 1 - суммировать значения */
                                                                                "\"prev_msg_diff\":\"0\"," +                        /* флаг, позволяющий сформировать диапазон для текущего значения с помощью предыдущего значения(prev) следующим образом: [prev+lower_bound ; prev+upper_bound] -- таким образом диапазон для текущего значения всегда относителен и зависит от предыдущего значения; 0 - выключить опцию, 1 - включить опцию */
                                                                                "\"sensor_name_mask\":\"Блокировка иммобилайзера\"," +               /* маска названия датчика */
                                                                                "\"sensor_type\":\"digital\"," +                    /* тип датчика */
                                                                                "\"type\":\"0\"," +                                 /* срабатывать: 0 - в рамках установленных значений, 1 - за пределами установленных значений */
                                                                                "\"upper_bound\":\"1\"" +                           /* значение датчика до */
                                                                        "}" +
                                                            "}," +
                                                            "\"act\":[" +                                                       /* действия */
                                                                        "{" +
                                                                            "\"t\":\"message\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"color\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"email\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"email_to\":\"" + user_account_name + "\"," +
                                                                                    "\"html\":\"0\"," +
                                                                                    "\"img_attach\":\"0\"," +
                                                                                    "\"subj\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"event\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"flags\":\"0\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"mobile_apps\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

            //Низкое напряжения АКБ
            string CreteNotifAnswer3 = macros.wialon_request_new("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Напряжение АКБ\"," +                                 /* название */
                                                            "\"txt\":\"%UNIT%: низкое напряжение АКБ. Автомобиль находился около '%LOCATION%'.\"," +     /* текст уведомления */
                                                            "\"ta\":\"0\"," +                                                   /* время активации (UNIX формат) */
                                                            "\"td\":\"0\"," +                                                   /* время деактивации (UNIX формат) */
                                                            "\"ma\":\"0\"," +                                                   /* максимальное количество срабатываний (0 - не ограничено) */
                                                            "\"mmtd\":\"0\"," +                                                 /* максимальный временной интервал между сообщениями (секунд) */
                                                            "\"cdt\":\"0\"," +                                                  /* таймаут срабатывания(секунд) */
                                                            "\"mast\":\"300\"," +                                                 /* минимальная продолжительность тревожного состояния (секунд) */
                                                            "\"mpst\":\"0\"," +                                                 /* минимальная продолжительность предыдущего состояния (секунд) */
                                                            "\"cp\":\"0\"," +                                                   /* период контроля относительно текущего времени (секунд) */
                                                            "\"fl\":\"0\"," +                                                   /* флаги: 0=уведомление срабатывает на первое сообщение, 1=уведомление срабатывает на каждое сообщение, 2=уведомление выключено */
                                                            "\"tz\":\"7200\"," +                                                /* часовой пояс */
                                                            "\"la\":\"ru\"," +                                                  /* язык пользователя (двухбуквенный код) */
                                                            "\"ac\":\"0\"," +                                                   /* количество срабатываний */
                                                            "\"un\":[" + object_id + "]," +     /* массив ID объектов/групп объектов */
                                                            "\"sch\":{" +                                                       /* ограничение по времени */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"ctrl_sch\":{" +                                                  /* расписание периодов ограничения количества срабатывания */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"trg\":{" +                                                       /* контроль */
                                                                        "\"t\":\"sensor_value\"," +                             /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_kontrolja */
                                                                        "\"p\":{" +
                                                                                "\"lower_bound\":\"0\"," +                          /* значение датчика от */
                                                                                "\"merge\":\"1\"," +                                /* одинаковые датчики: 0 - считать отдельно, 1 - суммировать значения */
                                                                                "\"prev_msg_diff\":\"0\"," +                        /* флаг, позволяющий сформировать диапазон для текущего значения с помощью предыдущего значения(prev) следующим образом: [prev+lower_bound ; prev+upper_bound] -- таким образом диапазон для текущего значения всегда относителен и зависит от предыдущего значения; 0 - выключить опцию, 1 - включить опцию */
                                                                                "\"sensor_name_mask\":\"Напряжение АКБ\"," +               /* маска названия датчика */
                                                                                "\"sensor_type\":\"voltage\"," +                    /* тип датчика */
                                                                                "\"type\":\"0\"," +                                 /* срабатывать: 0 - в рамках установленных значений, 1 - за пределами установленных значений */
                                                                                "\"upper_bound\":\"11.08\"" +                           /* значение датчика до */
                                                                        "}" +
                                                            "}," +
                                                            "\"act\":[" +                                                       /* действия */
                                                                        "{" +
                                                                            "\"t\":\"message\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"color\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"email\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"email_to\":\"" + user_account_name + "\"," +
                                                                                    "\"html\":\"0\"," +
                                                                                    "\"img_attach\":\"0\"," +
                                                                                    "\"subj\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"event\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"flags\":\"0\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"mobile_apps\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

            //Сработка: Датчики взлома
            string CreteNotifAnswer4 = macros.wialon_request_new("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Сработка: Датчики взлома\"," +                                 /* название */
                                                            "\"txt\":\"%UNIT%: Несанкционированное открытие дверей, капота или багажника. Время сработки: %MSG_TIME%. Автомобиль находился около '%LOCATION%'.\"," +     /* текст уведомления */
                                                            "\"ta\":\"0\"," +                                                   /* время активации (UNIX формат) */
                                                            "\"td\":\"0\"," +                                                   /* время деактивации (UNIX формат) */
                                                            "\"ma\":\"0\"," +                                                   /* максимальное количество срабатываний (0 - не ограничено) */
                                                            "\"mmtd\":\"0\"," +                                                 /* максимальный временной интервал между сообщениями (секунд) */
                                                            "\"cdt\":\"0\"," +                                                  /* таймаут срабатывания(секунд) */
                                                            "\"mast\":\"0\"," +                                                 /* минимальная продолжительность тревожного состояния (секунд) */
                                                            "\"mpst\":\"0\"," +                                                 /* минимальная продолжительность предыдущего состояния (секунд) */
                                                            "\"cp\":\"0\"," +                                                   /* период контроля относительно текущего времени (секунд) */
                                                            "\"fl\":\"0\"," +                                                   /* флаги: 0=уведомление срабатывает на первое сообщение, 1=уведомление срабатывает на каждое сообщение, 2=уведомление выключено */
                                                            "\"tz\":\"7200\"," +                                                /* часовой пояс */
                                                            "\"la\":\"ru\"," +                                                  /* язык пользователя (двухбуквенный код) */
                                                            "\"ac\":\"0\"," +                                                   /* количество срабатываний */
                                                            "\"un\":[" + object_id + "]," +     /* массив ID объектов/групп объектов */
                                                            "\"sch\":{" +                                                       /* ограничение по времени */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"ctrl_sch\":{" +                                                  /* расписание периодов ограничения количества срабатывания */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"trg\":{" +                                                       /* контроль */
                                                                        "\"t\":\"sensor_value\"," +                             /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_kontrolja */
                                                                        "\"p\":{" +
                                                                                "\"lower_bound\":\"1\"," +                          /* значение датчика от */
                                                                                "\"merge\":\"1\"," +                                /* одинаковые датчики: 0 - считать отдельно, 1 - суммировать значения */
                                                                                "\"prev_msg_diff\":\"0\"," +                        /* флаг, позволяющий сформировать диапазон для текущего значения с помощью предыдущего значения(prev) следующим образом: [prev+lower_bound ; prev+upper_bound] -- таким образом диапазон для текущего значения всегда относителен и зависит от предыдущего значения; 0 - выключить опцию, 1 - включить опцию */
                                                                                "\"sensor_name_mask\":\"Сработка сигнализации: двери\"," +               /* маска названия датчика */
                                                                                "\"sensor_type\":\"digital\"," +                    /* тип датчика */
                                                                                "\"type\":\"0\"," +                                 /* срабатывать: 0 - в рамках установленных значений, 1 - за пределами установленных значений */
                                                                                "\"upper_bound\":\"1\"" +                           /* значение датчика до */
                                                                        "}" +
                                                            "}," +
                                                            "\"act\":[" +                                                       /* действия */
                                                                        "{" +
                                                                            "\"t\":\"message\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"color\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"email\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"email_to\":\"" + user_account_name + "\"," +
                                                                                    "\"html\":\"0\"," +
                                                                                    "\"img_attach\":\"0\"," +
                                                                                    "\"subj\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"event\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"flags\":\"0\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"mobile_apps\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

            //Сработка: ТРЕВОЖНАЯ КНОПКА
            string CreteNotifAnswer5 = macros.wialon_request_new("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"ТРЕВОЖНАЯ КНОПКА\"," +                                 /* название */
                                                            "\"txt\":\"%UNIT%: НАЖАТА ТРЕВОЖНАЯ КНОПКА!!!!! Время сработки: %MSG_TIME%. В %POS_TIME% объект двигался со скоростью %SPEED% около '%LOCATION%'.\"," +     /* текст уведомления */
                                                            "\"ta\":\"0\"," +                                                   /* время активации (UNIX формат) */
                                                            "\"td\":\"0\"," +                                                   /* время деактивации (UNIX формат) */
                                                            "\"ma\":\"0\"," +                                                   /* максимальное количество срабатываний (0 - не ограничено) */
                                                            "\"mmtd\":\"0\"," +                                                 /* максимальный временной интервал между сообщениями (секунд) */
                                                            "\"cdt\":\"0\"," +                                                  /* таймаут срабатывания(секунд) */
                                                            "\"mast\":\"0\"," +                                                 /* минимальная продолжительность тревожного состояния (секунд) */
                                                            "\"mpst\":\"0\"," +                                                 /* минимальная продолжительность предыдущего состояния (секунд) */
                                                            "\"cp\":\"0\"," +                                                   /* период контроля относительно текущего времени (секунд) */
                                                            "\"fl\":\"0\"," +                                                   /* флаги: 0=уведомление срабатывает на первое сообщение, 1=уведомление срабатывает на каждое сообщение, 2=уведомление выключено */
                                                            "\"tz\":\"7200\"," +                                                /* часовой пояс */
                                                            "\"la\":\"ru\"," +                                                  /* язык пользователя (двухбуквенный код) */
                                                            "\"ac\":\"0\"," +                                                   /* количество срабатываний */
                                                            "\"un\":[" + object_id + "]," +     /* массив ID объектов/групп объектов */
                                                            "\"sch\":{" +                                                       /* ограничение по времени */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"ctrl_sch\":{" +                                                  /* расписание периодов ограничения количества срабатывания */
                                                                        "\"f1\":\"0\"," +                                       /* время начала интервала 1 (количество минут от полуночи) */
                                                                        "\"f2\":\"0\"," +                                       /* время начала интервала 2 (количество минут от полуночи) */
                                                                        "\"t1\":\"0\"," +                                       /* время окончания интервала 1 (количество минут от полуночи) */
                                                                        "\"t2\":\"0\"," +                                       /* время окончания интервала 2 (количество минут от полуночи) */
                                                                        "\"m\":\"0\"," +                                        /* маска дней месяца [1: 2^0, 31: 2^30] */
                                                                        "\"y\":\"0\"," +                                        /* маска месяцев [янв: 2^0, дек: 2^11] */
                                                                        "\"w\":\"0\"" +                                         /* маска дней недели [пн: 2^0, вс: 2^6] */
                                                            "}," +
                                                            "\"trg\":{" +                                                       /* контроль */
                                                                        "\"t\":\"sensor_value\"," +                             /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_kontrolja */
                                                                        "\"p\":{" +
                                                                                "\"lower_bound\":\"1\"," +                          /* значение датчика от */
                                                                                "\"merge\":\"1\"," +                                /* одинаковые датчики: 0 - считать отдельно, 1 - суммировать значения */
                                                                                "\"prev_msg_diff\":\"0\"," +                        /* флаг, позволяющий сформировать диапазон для текущего значения с помощью предыдущего значения(prev) следующим образом: [prev+lower_bound ; prev+upper_bound] -- таким образом диапазон для текущего значения всегда относителен и зависит от предыдущего значения; 0 - выключить опцию, 1 - включить опцию */
                                                                                "\"sensor_name_mask\":\"Тревожная кнопка\"," +               /* маска названия датчика */
                                                                                "\"sensor_type\":\"digital\"," +                    /* тип датчика */
                                                                                "\"type\":\"0\"," +                                 /* срабатывать: 0 - в рамках установленных значений, 1 - за пределами установленных значений */
                                                                                "\"upper_bound\":\"1\"" +                           /* значение датчика до */
                                                                        "}" +
                                                            "}," +
                                                            "\"act\":[" +                                                       /* действия */
                                                                        "{" +
                                                                            "\"t\":\"message\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"color\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"email\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"email_to\":\"" + user_account_name + "\"," +
                                                                                    "\"html\":\"0\"," +
                                                                                    "\"img_attach\":\"0\"," +
                                                                                    "\"subj\":\"\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"event\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"flags\":\"0\"" +
                                                                                   "}" +
                                                                        "}," +
                                                                        "{" +
                                                                            "\"t\":\"mobile_apps\"," +                                /* тип контроля: https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/resource/get_notification_data#tipy_dejstvij */
                                                                            "\"p\":{" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

        }

        private void pass_reset_button_Click(object sender, EventArgs e)
        {
            if (treeView_user_accounts.SelectedNode is null)
            {
                MessageBox.Show("Вибери користувача");
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Встановити та відправити новий пароль?", "Відправити?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //generete password
                    //---------------------------------------------------------------
                    string pass = "";

                    if (checkBox_manual_pass.Checked == true)
                    {
                        if (textBox_account_pss.Text == "")
                        {
                            pass = CreatePassword(6);
                            textBox_account_pss.Text = pass;
                        }
                        else
                        {
                            pass = textBox_account_pss.Text;
                        }
                        checkBox_manual_pass.Checked = false;
                    }
                    else
                    {
                        pass = CreatePassword(6);
                        textBox_account_pss.Text = pass;
                    }
                    //---------------------------------------------------------------

                    string json = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                            "\"spec\":{" +
                                                            "\"itemsType\":\"user\"," +
                                                            "\"propName\":\"sys_name\"," +
                                                            "\"propValueMask\":\"" + treeView_user_accounts.SelectedNode.Text + "\"," +
                                                            "\"sortType\":\"sys_name\"," +
                                                            "\"or_logic\":\"1\"}," +
                                                            "\"force\":\"1\"," +
                                                            "\"flags\":\"1\"," +
                                                            "\"from\":\"0\"," +
                                                            "\"to\":\"0\"}"); //запрашиваем все елементі с искомім имайлом
                    var m = JsonConvert.DeserializeObject<RootObject>(json);

                    string json1 = macros.wialon_request_new("&svc=user/update_password&params={" +
                                                             "\"userId\":\"" + m.items[0].id + "\"," +
                                                             "\"oldPassword\":\"\"," +
                                                             "\"newPassword\":\"" + pass + "\"}");
                    var m1 = JsonConvert.DeserializeObject<RootObject>(json1);

                    string Body = "<p>Добрий день!</p><p>;</p><p>Відповідно до Вашого запиту,</p><p>Для Вас було відновлено пароль для доступу в систему моніторингу ВЕНБЕСТ. </p><p>----------------------------------------------</p><p>Для входу в систему моніторингу за допомогою мобільного додатку:</p><p>1.Завантажте мобільний додаток Wialon Local: https://venbest.ua/gps-prilozheniia/</p> <p>2.При першому вході в мобільний додаток введіть такі дані:</p><p>a.Адреса серверу: https://navi.venbest.com.ua/;</p> <p>Посилання вводиться зверху сторінки вводу логіну та паролю. Після його введення необхідно натиснути на іконку у вигляді щита (праворуч рядка вводу).</p><p>b.Логін: " + treeView_user_accounts.SelectedNode.Text + "</p><p>c.Пароль: " + pass + " </p><p>Зверніть, будь ласка, увагу, що логін та пароль чутливий до регістру символів, які ви вводите.</p><p> <br></p><p>3.Якщо ви бажаєте отримувати сповіщення, увімкніть їх в налаштуваннях.</p><p>----------------------------------------------</p><p>Для входу в систему моніторингу за допомогою браузеру:</p><p>1.Перейдіть за посиланням: https://navi.venbest.com.ua/</p> <p>2.Введіть логін: " + treeView_user_accounts.SelectedNode.Text + "</p><p>3.Введіть пароль: " + pass + "</p><p>  <br></p><p>Змініть, будь ласка, пароль в налаштуваннях користувача при вході через браузер.</p><p>----------------------------------------------</p><p>Департамент супутникових систем охорони</p><p>Група Компаній «ВЕНБЕСТ»</p><p>Т 044 501 33 77;</p><p>auto@venbest.com.ua | https://venbest.ua/ohrana-avto-i-zashchita-ot-ugona</p>";
                    macros.send_mail_auto(treeView_user_accounts.SelectedNode.Text, "ВЕНБЕСТ. Вхід в систему моніторингу", Body + pass);
                    macros.send_mail_auto("auto@venbest.com.ua", "ВЕНБЕСТ. Вхід в систему моніторингу", Body + pass);

                    //Save in db client account
                    macros.GetData("insert into btk.Client_accounts (name, pass, date, reason, Object_idObject, Users_idUsers) value ('" + treeView_user_accounts.SelectedNode.Text + "','" + pass + "','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','Chenge pass account','" + vars_form.id_db_object_for_activation + "','" + vars_form.user_login_id + "');");

                }
                else if (dialogResult == DialogResult.No)
                {
                }

            }
        }

        private void button_user_account_on_off_Click(object sender, EventArgs e)
        {
            if (treeView_user_accounts.SelectedNode is null)
            {
                MessageBox.Show("Вибери користувача");
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Відключити користувача", "Відключити?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string json = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                            "\"spec\":{" +
                                                            "\"itemsType\":\"user\"," +
                                                            "\"propName\":\"sys_name\"," +
                                                            "\"propValueMask\":\"" + treeView_user_accounts.SelectedNode.Text + "\"," +
                                                            "\"sortType\":\"sys_name\"," +
                                                            "\"or_logic\":\"1\"}," +
                                                            "\"force\":\"1\"," +
                                                            "\"flags\":\"1\"," +
                                                            "\"from\":\"0\"," +
                                                            "\"to\":\"0\"}"); //запрашиваем все елементі с искомім имайлом
                    var m = JsonConvert.DeserializeObject<RootObject>(json);

                    string json2 = macros.wialon_request_new("&svc=user/update_user_flags&params={" +
                                                    "\"userId\":\"" + m.items[0].id + "\"," +
                                                    "\"flags\":\"1\"," +
                                                    "\"flagsMask\":\"1\"}");
                    var m2 = JsonConvert.DeserializeObject<RootObject>(json2);

                    //log user action
                    macros.LogUserAction(vars_form.user_login_id, "Відключити користувача", treeView_user_accounts.SelectedNode.Text, "", Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));

                }
                else if (dialogResult == DialogResult.No)
                {
                }

            }
        }

        private void button_user_on_Click(object sender, EventArgs e)
        {
            if (treeView_user_accounts.SelectedNode is null)
            {
                MessageBox.Show("Вибери користувача");
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Включити користувача", "Включити?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string json = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                            "\"spec\":{" +
                                                            "\"itemsType\":\"user\"," +
                                                            "\"propName\":\"sys_name\"," +
                                                            "\"propValueMask\":\"" + treeView_user_accounts.SelectedNode.Text + "\"," +
                                                            "\"sortType\":\"sys_name\"," +
                                                            "\"or_logic\":\"1\"}," +
                                                            "\"force\":\"1\"," +
                                                            "\"flags\":\"1\"," +
                                                            "\"from\":\"0\"," +
                                                            "\"to\":\"0\"}"); //запрашиваем все елементі с искомім имайлом
                    var m = JsonConvert.DeserializeObject<RootObject>(json);

                    string json2 = macros.wialon_request_new("&svc=user/update_user_flags&params={" +
                                                    "\"userId\":\"" + m.items[0].id + "\"," +
                                                    "\"flags\":\"0\"," +
                                                    "\"flagsMask\":\"1\"}");
                    var m2 = JsonConvert.DeserializeObject<RootObject>(json2);

                    //log user action
                    macros.LogUserAction(vars_form.user_login_id, "Відключити користувача", treeView_user_accounts.SelectedNode.Text, "", Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));

                }
                else if (dialogResult == DialogResult.No)
                {
                }

            }
        }

        private void button_remove_2_account_Click(object sender, EventArgs e)
        {
            if (treeView_user_accounts.SelectedNode is null)
            {
                MessageBox.Show("Вибери який кабінет відалити!");
            }
            else
            {
                if (treeView_user_accounts.SelectedNode.Text == "Кабінети користувача:")
                {
                    MessageBox.Show("Вибери який кабінет відалити!");
                }
                else
                {
                    string saving_selected_account = treeView_user_accounts.SelectedNode.Text;
                    DialogResult dialogResult =
                        MessageBox.Show("Відалити авто з доступу: " + treeView_user_accounts.SelectedNode.Text + "?",
                            "Видалити?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        //get user data
                        string UserDataAnswer = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                                "\"spec\":{" +
                                                                "\"itemsType\":\"user\"," +
                                                                "\"propName\":\"sys_name\"," +
                                                                "\"propValueMask\":\"" + treeView_user_accounts.SelectedNode.Text + "\"," +
                                                                "\"sortType\":\"sys_name\"," +
                                                                "\"or_logic\":\"1\"}," +
                                                                "\"force\":\"1\"," +
                                                                "\"flags\":\"1\"," +
                                                                "\"from\":\"0\"," +
                                                                "\"to\":\"0\"}"); //запрашиваем все елементі с искомім имайлом
                        var UserData = JsonConvert.DeserializeObject<RootObject>(UserDataAnswer);

                        //get resource data
                        string ResourceDataAnswer = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                                "\"spec\":{" +
                                                                "\"itemsType\":\"avl_resource\"," +
                                                                "\"propName\":\"sys_name\"," +
                                                                "\"propValueMask\":\"" + treeView_user_accounts.SelectedNode.Text + "\"," +
                                                                "\"sortType\":\"sys_name\"," +
                                                                "\"or_logic\":\"1\"}," +
                                                                "\"force\":\"1\"," +
                                                                "\"flags\":\"1025\"," +
                                                                "\"from\":\"0\"," +
                                                                "\"to\":\"0\"}"); //запрашиваем все елементі с искомім имайлом
                        var ResourceData = JsonConvert.DeserializeObject<RootObject>(UserDataAnswer);

                        if (ResourceData.items.Count >= 1)
                        {
                            if (ResourceData.items[0].Unf != null)
                            {
                                foreach (var num in ResourceData.items[0].Unf)
                                {
                                    if (num.Value.Un[0].ToString() == wl_id)
                                    {
                                        string result = delete_notif(ResourceData.items[0].id, num.Value.Id);
                                    }

                                }
                            }
                        }


                        ///////////
                        //Доступ на объект
                        string json4 = macros.wialon_request_new("&svc=user/update_item_access&params={" +
                                                                 "\"userId\":\"" + UserData.items[0].id + "\"," +
                                                                 "\"itemId\":\"" + wl_id + "\"," +
                                                                 "\"accessMask\":\"0\"}");
                        var m4 = JsonConvert.DeserializeObject<RootObject>(json4);

                        //log user action
                        macros.LogUserAction(vars_form.user_login_id, "Прибрати з доступу користувача", saving_selected_account, wl_id, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));


                        //update treeView_user_accounts after making chenge
                        build_list_account();


                    }
                    else if (dialogResult == DialogResult.No)
                    {
                    }
                }
            }
        }

        private void account_delete_button_Click(object sender, EventArgs e)
        {
            if (treeView_user_accounts.SelectedNode is null)
            {
                MessageBox.Show("Вибери який кабінет відалити!");
            }
            else
            {
                if (treeView_user_accounts.SelectedNode.Text == "Кабінети користувача:")
                {
                    MessageBox.Show("Вибери який кабінет відалити!");
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Видалити кабінет: " + treeView_user_accounts.SelectedNode.Text + "", "Видалити?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string json1 = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                             "\"spec\":{" +
                                                             "\"itemsType\":\"\"," +
                                                             "\"propName\":\"sys_name\"," +
                                                             "\"propValueMask\":\"*" + treeView_user_accounts.SelectedNode.Text + "*\"," +
                                                             "\"sortType\":\"sys_name\"," +
                                                             "\"or_logic\":\"1\"}," +
                                                             "\"force\":\"1\"," +
                                                             "\"flags\":\"1\"," +
                                                             "\"from\":\"0\"," +
                                                             "\"to\":\"0\"}");//запрашиваем все елементі с искомім имайлом

                        var m = JsonConvert.DeserializeObject<RootObject>(json1);
                        try
                        {
                            foreach (var t in m.items)
                            {
                                if (t.cls == 3)
                                {
                                    string json2 = macros.wialon_request_new("&svc=item/delete_item&params={" +
                                                                             "\"itemId\":\"" + t.id + "\"}");
                                }
                            }//удаляем сначала все ресурсы cls=3

                            foreach (var t in m.items)
                            {
                                if (t.cls == 1)
                                {
                                    string json2 = macros.wialon_request_new("&svc=item/delete_item&params={" +
                                                                             "\"itemId\":\"" + t.id + "\"}");
                                }
                            }// После пользователя.cls=1
                        }
                        catch
                        {
                        }


                        //Save in db client account
                        macros.GetData("insert into btk.Client_accounts (name, pass, date, reason, Object_idObject, Users_idUsers) value ('" + treeView_user_accounts.SelectedNode.Text + "','','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','Delete account','" + id_db_obj + "','" + vars_form.user_login_id + "');");

                        build_list_account();//обновляем тривив




                    }
                    else if (dialogResult == DialogResult.No)
                    {

                    }
                }
            }
        }

        private void checkBox_manual_pass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_manual_pass.Checked == true)
            {
                textBox_account_pss.ReadOnly = false;
            }
            else
            {
                textBox_account_pss.ReadOnly = true;
            }
        }

        // Delete notif for needed resource
        private string delete_notif(int resours_id, int notif_id)
        {
            string CreteNotifAnswer = macros.wialon_request_new("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"" + notif_id + "\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"delete\"" +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                        "}"
                                                    );
            return CreteNotifAnswer;
        }

        private void tabControl2_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (tabControl2.SelectedTab.Name == "tabPage_edit_client")
            {
                load_vo();
                build_list_account();
            }
        }
    }
}
