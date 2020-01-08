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

        }

        private void TreeView_zapolnyaem()//Получаем произвольные поля из Виалона и заполняем дерево
        {
            try
            {
                string json = macros.wialon_request_new("&svc=core/search_items&params={" +
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
                        treeView_object_info.Nodes[0].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                        treeView_object_info.Nodes[0].Nodes.Add(new TreeNode("Назва об'єкту:" + m.items[0].nm.ToString()));
                        treeView_object_info.Nodes[0].Nodes.Add(new TreeNode("IMEI:" + m.items[0].uid.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("І Відповідальна"))
                    {
                        treeView_object_info.Nodes[1].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("ІІ Відповідальна"))
                    {
                        treeView_object_info.Nodes[1].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("ІІІ Відповідальна"))
                    {
                        treeView_object_info.Nodes[1].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("Установник"))
                    {
                        treeView_object_info.Nodes[2].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("тривожної"))
                    {
                        treeView_object_info.Nodes[2].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("Штатні"))
                    {
                        treeView_object_info.Nodes[2].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                    }
                    else if (keyvalue.Value.n.Contains("Кодове"))
                    {
                        treeView_object_info.Nodes[3].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                    }  
                }

                foreach (var keyvalue in m.items[0].aflds)
                {
                    if (keyvalue.Value.n.Contains("PUK"))
                    {
                        treeView_object_info.Nodes[3].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
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
            //if (textBox_zvernennya.Text == "")//Запись в хронологию обработки не может быть внесена без комментария к действию
            //{
            //    MessageBox.Show("Опишіть суть звернення");
            //    return;
            //}

            macros.sql_command("INSERT INTO btk.notification(" +
                               "unit_name, " +
                               "unit_id, " +
                               "curr_time, " +
                               "msg_time, " +
                               "product, " +
                               "type_alarm, " +
                               "Users_idUsers) " +
                               "VALUES('" + object_name + "'," +
                               "'" + vars_form.add_alarm_unit_id + "'," +
                               "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                               "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                               "'" + object_product + "'," +
                               "'" + comboBox_source_in.GetItemText(comboBox_source_in.SelectedItem) + "'," +
                               "'" + vars_form.user_login_id + "')");

            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
