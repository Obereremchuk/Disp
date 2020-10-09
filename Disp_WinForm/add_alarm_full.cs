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
                               "Users_idUsers, status) " +
                               "VALUES('" + object_name + "'," +
                               "'" + vars_form.add_alarm_unit_id + "'," +
                               "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                               "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                               "'" + object_product + "'," +
                               "'" + comboBox_source_in.GetItemText(comboBox_source_in.SelectedItem) + "'," +
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


            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox_source_in_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_source_in.SelectedIndex == 2)
            { comboBox_account.Enabled = true; textBox_email_account.Enabled = true; textBox_primitka.Enabled = true; }
            else { comboBox_account.Enabled = false; textBox_email_account.Enabled = false; textBox_primitka.Enabled = false; }
        }

        private void comboBox_account_SelectedIndexChanged(object sender, EventArgs e)
        {
            
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
