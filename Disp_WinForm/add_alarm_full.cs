using Newtonsoft.Json;
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
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Disp_WinForm
{
    public partial class add_alarm_full : Form
    {
        Macros macros = new Macros();
        string object_name;
        string object_product = "CNTK";
        
        public add_alarm_full()
        {
            InitializeComponent();
            TreeView_zapolnyaem();
            comboBox_account.Enabled = false; 
            textBox_email_account.Enabled = false; 
            textBox_primitka.Enabled = false;
            dateTimePicker_start_rouming.Value = DateTime.Now.Date;
            dateTimePicker_end_rouming.Value = DateTime.Now.Date;
        }

        private void build_list_account()
        {
            //vars_form.id_object_for_activation = "1098";


            string json = macros.WialonRequest("&svc=core/check_accessors&params={" +
                                                    "\"items\":[\"" + vars_form.add_alarm_unit_id + "\"]," +
                                                    "\"flags\":\"1\"}");//Получаем айди всех елементов у которых есть доступ к данному объекту            

            var wl_accounts = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, Dictionary<string, string>>>>(json);
            string get = "";
            for (int index = 0; index < wl_accounts[vars_form.add_alarm_unit_id].Values.Count; index++)
            {
                var item = wl_accounts.ElementAt(0);
                int key_index = item.Value.ElementAt(index).Key;
                get = get + "," + key_index.ToString();
                
            }// стрим список айди в один запрос 
            string json2 = macros.WialonRequest("&svc=core/search_items&params={" +
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

            //treeView_object_info.Nodes.Clear();
            Font boldFont = new Font(treeView_object_info.Font, FontStyle.Bold);
            TreeNode node5 = new TreeNode("Кабінети користувача приєднані до авто");
            treeView_object_info.Nodes.Add(node5);
            treeView_object_info.BeginUpdate();

            try
            {
                int tree_index = 0;
                for (int index = 0; index < m.items.Count; index++)
                {

                    if (m.items[index].nm.Contains("@"))
                    {
                        treeView_object_info.Nodes[0].Nodes.Add(new TreeNode(m.items[index].nm)); //выводим в дерево все учетки которые похожи на почту, ищем по @
                        treeView_object_info.Nodes[0].Nodes[tree_index].NodeFont = boldFont;
                    }
                }
                treeView_object_info.EndUpdate();
                treeView_object_info.ExpandAll();


            }
            catch (Exception e)
            {
                string er = e.ToString();
            }

        }

        private void TreeView_zapolnyaem()//Получаем произвольные поля из Виалона и заполняем дерево
        {
            try
            {
                string json = macros.WialonRequest("&svc=core/search_items&params={" +
                                                        "\"spec\":{" +
                                                        "\"itemsType\":\"avl_unit\"," +
                                                        "\"propName\":\"sys_id\"," +
                                                        "\"propValueMask\":\"" + vars_form.add_alarm_unit_id + "\", " +
                                                        "\"sortType\":\"sys_name\"," +
                                                        "\"or_logic\":\"1\"}," +
                                                        "\"force\":\"1\"," +
                                                        "\"flags\":\"15208907\"," +
                                                        "\"from\":\"0\"," +
                                                        "\"to\":\"5\"}");

                var m = JsonConvert.DeserializeObject<RootObject>(json);

                object_name = m.items[0].nm.ToString();

                build_list_account();
                TreeNode node1 = new TreeNode("Об'єкт охорони:");
                treeView_object_info.Nodes.Add(node1);
                treeView_object_info.Nodes[0].Expand();
                TreeNode node2 = new TreeNode("Відповідальні особи");
                treeView_object_info.Nodes.Add(node2);
                treeView_object_info.Nodes[1].Expand();
                TreeNode node3 = new TreeNode("Інформація про установку");
                treeView_object_info.Nodes.Add(node3);
                treeView_object_info.Nodes[2].Expand();
                TreeNode node4 = new TreeNode("Адміністративні поля");
                treeView_object_info.Nodes.Add(node4);
                treeView_object_info.Nodes[3].Expand();

                foreach (var keyvalue in m.items[0].flds)
                {
                    if (keyvalue.Value.n.Contains("УВАГА"))
                    {
                        treeView_object_info.Nodes[1].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                        treeView_object_info.Nodes[1].Nodes.Add(new TreeNode("Назва об'єкту:" + m.items[0].nm.ToString()));
                        treeView_object_info.Nodes[1].Nodes.Add(new TreeNode("IMEI:" + m.items[0].uid.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("І Відповідальна"))
                    {
                        treeView_object_info.Nodes[2].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("ІІ Відповідальна"))
                    {
                        treeView_object_info.Nodes[2].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("ІІІ Відповідальна"))
                    {
                        treeView_object_info.Nodes[2].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("Установник"))
                    {
                        treeView_object_info.Nodes[3].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("тривожної"))
                    {
                        treeView_object_info.Nodes[3].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("Штатні"))
                    {
                        treeView_object_info.Nodes[3].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("Кодове"))
                    {
                        treeView_object_info.Nodes[4].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                    }  
                }

                foreach (var keyvalue in m.items[0].aflds)
                {
                    if (keyvalue.Value.n.Contains("PUK"))
                    {
                        treeView_object_info.Nodes[4].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void add_alarm_full_Load(object sender, EventArgs e)
        {
            comboBox_source_in.SelectedIndex = comboBox_source_in.FindStringExact("Звернення від кліента");
        }

        private void button_vnesty_zapis_Click(object sender, EventArgs e)
        {
            if (comboBox_source_in.SelectedIndex < 2)
            {
                macros.sql_command("INSERT INTO btk.notification(" +
                               "unit_name, " +
                               "unit_id, " +
                               "curr_time, " +
                               "msg_time, " +
                               "product, " +
                               "type_alarm, " +
                               "Users_idUsers, status) " +
                               "VALUES('" + object_name + "'," +
                               "'" + vars_form.add_alarm_unit_id + "'," +
                               "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                               "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                               "'" + object_product + "'," +
                               "'" + comboBox_source_in.GetItemText(comboBox_source_in.SelectedItem) + "'," +
                               "'" + vars_form.user_login_id + "','Відкрито')");
            }
            else if (comboBox_source_in.SelectedIndex == 2)
            {   
                if (!IsValidEmail(textBox_email_account.Text))
                {
                    MessageBox.Show("Невірна електронна пошта!");
                    return;
                }
                if (comboBox_account.SelectedIndex == -1)
                { 
                    MessageBox.Show("Обери що робимо з обіковим записом?"); 
                    return;
                }

                macros.sql_command("INSERT INTO btk.notification(" +
                               "unit_name, " +
                               "unit_id, " +
                               "curr_time, " +
                               "msg_time, " +
                               "product, " +
                               "type_alarm, " +
                               "time_stamp, " +
                               "Users_idUsers, status) " +
                               "VALUES('" + object_name + "'," +
                               "'" + vars_form.add_alarm_unit_id + "'," +
                               "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                               "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                               "'" + object_product + "'," +
                               "'" + comboBox_source_in.GetItemText(comboBox_source_in.SelectedItem) + "'," +
                               "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                               "'" + vars_form.user_login_id + "','Учетки')");

                string id_created_notification = macros.sql_command("SELECT max(idnotification) FROM btk.notification;");

                string text_data = MySqlHelper.EscapeString(comboBox_account.GetItemText(comboBox_account.SelectedItem) + "\n" + "Примітка: " + textBox_primitka.Text);

                macros.sql_command("insert into btk.alarm_ack(" +
                                    "alarm_text, " +
                                    "notification_idnotification, " +
                                    "Users_chenge, time_start_ack, " +
                                    "current_status_alarm, " +
                                    "vizov_police, " +
                                    "vizov_gmp) " +
                                    "values('" + text_data + "', " +
                                    "'" + id_created_notification + "'," +
                                    "'" + vars_form.user_login_id + "', " +
                                    "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                    " '" + "Учетки" + "', " +
                                    "'0', " +
                                    "'0'); ");
                macros.sql_command("insert into btk.alarm_ack(" +
                                    "alarm_text, " +
                                    "notification_idnotification, " +
                                    "Users_chenge, time_start_ack, " +
                                    "current_status_alarm, " +
                                    "vizov_police, " +
                                    "vizov_gmp) " +
                                    "values('" + textBox_email_account.Text + "', " +
                                    "'" + id_created_notification + "'," +
                                    "'" + vars_form.user_login_id + "', " +
                                    "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                    " '" + "Учетки" + "', " +
                                    "'0', " +
                                    "'0'); ");
            }
            if (comboBox_source_in.SelectedIndex == 3)
            {
                if (dateTimePicker_start_rouming.Value >= dateTimePicker_end_rouming.Value)
                {
                    MessageBox.Show("Перевір дату");
                    return;
                }
                macros.sql_command("INSERT INTO btk.notification(" +
                               "unit_name, " +
                               "unit_id, " +
                               "curr_time, " +
                               "msg_time, " +
                               "product, " +
                               "type_alarm, " +
                               "Users_idUsers, status) " +
                               "VALUES('" + object_name + "'," +
                               "'" + vars_form.add_alarm_unit_id + "'," +
                               "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                               "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                               "'" + object_product + "'," +
                               "'" + comboBox_source_in.GetItemText(comboBox_source_in.SelectedItem) + "'," +
                               "'" + vars_form.user_login_id + "','Роумінг')");

                string id_created_notification = macros.sql_command("SELECT max(idnotification) FROM btk.notification;");

                string text_data = MySqlHelper.EscapeString( "Запит на активацію роумінгу." + "\n" + "Початок: " + dateTimePicker_start_rouming.Value.Date.ToString("yyyy-MM-dd") + "\n" + "Кінець: " + dateTimePicker_end_rouming.Value.Date.ToString("yyyy-MM-dd"));
                macros.sql_command("insert into btk.alarm_ack(" +
                                    "alarm_text, " +
                                    "notification_idnotification, " +
                                    "Users_chenge, time_start_ack, " +
                                    "current_status_alarm, " +
                                    "vizov_police, " +
                                    "vizov_gmp) " +
                                    "values('" + text_data + "', " +
                                    "'" + id_created_notification + "'," +
                                    "'" + vars_form.user_login_id + "', " +
                                    "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                    " '" + "Роумінг" + "', " +
                                    "'0', " +
                                    "'0'); ");

                macros.sql_command("insert into btk.RoumingZayavka(" +
                                    "StartDate, " +
                                    "EndDate, " +
                                    "notification_idnotification)" +
                                    "values('" + Convert.ToDateTime(dateTimePicker_start_rouming.Value).Date.ToString("yyyy-MM-dd") + "', " +
                                    "'" + Convert.ToDateTime(dateTimePicker_end_rouming.Value).Date.ToString("yyyy-MM-dd") + "'," +
                                    "'" + id_created_notification + "' " +
                                    "); ");



                string json = macros.WialonRequest("&svc=core/search_item&params={"
                                                         + "\"id\":\"" + vars_form.add_alarm_unit_id + "\","
                                                         + "\"flags\":\"8388873\"}"); //
                var test_out = JsonConvert.DeserializeObject<RootObject>(json);

                string project = "";
                foreach (var keyvalue in test_out.item.flds)
                {
                    if (keyvalue.Value.n.Contains("Проект"))
                    {
                        project = keyvalue.Value.v.ToString();
                        break;
                    }
                }
                string vin = "";
                string registration_plate = "";
                string brand = "";
                string model = "";
                foreach (var keyvalue in test_out.item.pflds)
                {
                    if (keyvalue.Value.n.Contains("vin"))
                    {
                        vin = keyvalue.Value.v.ToString();
                    }
                    if (keyvalue.Value.n.Contains("registration_plate"))
                    {
                        registration_plate = keyvalue.Value.v.ToString();
                    }
                    if (keyvalue.Value.n.Contains("brand"))
                    {
                        brand = keyvalue.Value.v.ToString();
                    }
                    if (keyvalue.Value.n.Contains("model"))
                    {
                        model = keyvalue.Value.v.ToString();
                    }
                }



                string Subject = "Запит на активацію послуги Роумінг."+ " Проект: " + project + ", VIN: " + vin + ", Держ. Номер: " + registration_plate;
                
                DataTable users_send_mail = macros.GetData("SELECT user_mail FROM btk.Users where (dept_user = '115' or dept_user = '113') and State != '0';");

                string recip = "";
                foreach (DataRow user in users_send_mail.Rows)
                {
                    recip += "<" + user[0].ToString() + ">,";
                }
                recip = recip.TrimEnd(recip[recip.Length - 1]);
                
                
                DataTable dt = new DataTable();

                dt.Columns.Add("<b>Параметр</b>");
                dt.Columns.Add("<b>Значення</b>");
                object[] row = { "Проект", project };
                dt.Rows.Add(row);
                object[] row0 = { "VIN", vin };
                dt.Rows.Add(row0);
                object[] row1 = { "Держ. Номер", registration_plate };
                dt.Rows.Add(row1);
                object[] row2 = { "Марка", brand };
                dt.Rows.Add(row2);
                object[] row3 = { "Модель", model };
                dt.Rows.Add(row3);
                object[] row4 = { "Початок", dateTimePicker_start_rouming.Value.ToString("d") };
                dt.Rows.Add(row4);
                object[] row5 = { "Кінець", dateTimePicker_end_rouming.Value.ToString("d") };
                dt.Rows.Add(row5);
                object[] row6 = { "Договір", "" };
                dt.Rows.Add(row6);
                object[] row7 = { "<br />", "" };
                dt.Rows.Add(row7);
                object[] row8 = { "<br />", "" };
                dt.Rows.Add(row8);
                object[] row9 = { "Назва обєкту", test_out.item.nm };
                dt.Rows.Add(row9);
                object[] row10 = { "IMEI", test_out.item.uid };
                dt.Rows.Add(row10);
                


                string Body = "Вітаємо!"
                    + "<br />"
                    + "<br />"
                    + "До нас звернувся клієнт з проханням активувати послугу Роумінг. "
                    + "<br />"
                    + "Якщо ви погоджуєте активацію послуги - прохання надіслати у відповідь відповідну заявку."
                    + "<br />"
                    + "<br />"
                    + macros.ConvertDataTableToHTML(dt);


                macros.send_mail(recip, Subject, Body);
            }


            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox_source_in_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_source_in.SelectedIndex == 2)
            { 
                comboBox_account.Enabled = true; textBox_email_account.Enabled = true; textBox_primitka.Enabled = true;
                comboBox_account.Visible = true; textBox_email_account.Visible = true; textBox_primitka.Visible = true;
                dateTimePicker_start_rouming.Visible = false; dateTimePicker_end_rouming.Visible = false;
                label3.Visible = true; label4.Visible = true; label5.Visible = true;
                label3.Text = "Дії з обліковим записом"; label4.Text = "Електронна пошта"; label5.Text = "Примітка";
            }
            else if (comboBox_source_in.SelectedIndex == 3)
            {
                comboBox_account.Enabled = false; textBox_email_account.Enabled = false; textBox_primitka.Enabled = false;
                comboBox_account.Visible = false; textBox_email_account.Visible = false; textBox_primitka.Visible = false;
                dateTimePicker_start_rouming.Visible = true; dateTimePicker_end_rouming.Visible = true;
                label3.Visible = true; label4.Visible = true; label5.Visible = false;
                label3.Text = "Початок"; label4.Text = "Кінець"; label5.Text = "Примітка";
            }
            else 
            { 
                comboBox_account.Enabled = false; textBox_email_account.Enabled = false; textBox_primitka.Enabled = false;
                comboBox_account.Visible = false; textBox_email_account.Visible = false; textBox_primitka.Visible = false;
                dateTimePicker_start_rouming.Visible = false; dateTimePicker_end_rouming.Visible = false;
                label3.Visible = false; label4.Visible = false; label5.Visible = false;
            }
        }

        private bool IsValidEmail(string source)
        {
            string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
            if (!Regex.IsMatch(source, pattern, RegexOptions.IgnoreCase))
            {
                return false;
            }
            return true;
        }

  

        private void textBox_email_account_TextChanged(object sender, EventArgs e)
        {
            if (comboBox_account.Enabled is true)
            {
                if (textBox_email_account.Text.Any(char.IsUpper))
                {
                    textBox_email_account.Text = textBox_email_account.Text.ToLower();
                    textBox_email_account.SelectionStart = textBox_email_account.Text.Length;
                }
            }

            if (!IsValidEmail(textBox_email_account.Text))
            {
                textBox_email_account.BackColor = Color.LightPink;
            }
            else { textBox_email_account.BackColor = Color.Empty; }

        }

        private void treeView_object_info_Click(object sender, EventArgs e)
        {
            TreeViewHitTestInfo info = treeView_object_info.HitTest(treeView_object_info.PointToClient(Cursor.Position));
            if (info != null)
                if(info.Node.Text.Contains("@"))
                    textBox_email_account.Text= info.Node.Text;
        }
    }
}
