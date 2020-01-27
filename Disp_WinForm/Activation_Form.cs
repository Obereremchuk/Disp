using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Activation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Disp_WinForm
{
    public partial class Activation_Form : Form
    {
        Macros macros = new Macros();
        private string id_new_user;
        private List<TreeNode> _unselectableNodes = new List<TreeNode>();
        private string new_pass = "";
        public Activation_Form()
        {
            InitializeComponent();
            load_form();
            build_list_account();
            button_add_2_account.Enabled = false;
            create_notif();


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
        private void create_notif()
        {
            string json3 = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_resource\"," +
                                                     "\"propName\":\"sys_name\"," +
                                                     "\"propValueMask\":\"operator_messages\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");
            var m2 = JsonConvert.DeserializeObject<RootObject>(json3);

            string json6 = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_name\"," +
                                                     "\"propValueMask\":\"C_N 354836080344970 ТЕСТ\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");

            string json5 = macros.wialon_request_new("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"241\"," +
                                                            "\"id\":\"0\"," +
                                                            "\"callMode\":\"create\"," +
                                                            "\"e\":\"1\"," + 
                                                            "\"n\":\"АХА охрана, двери 72 часа\"," +
                                                            "\"txt\":\"АХА охрана, двери 72 часа не менялось значение\"," +
                                                            "\"ta\":\"0\"," +
                                                            "\"td\":\"0\"," +
                                                            "\"ma\":\"0\"," +
                                                            "\"mmtd\":\"0\"," +
                                                            "\"cdt\":\"0\"," +
                                                            "\"mast\":\"90000\"," +
                                                            "\"mpst\":\"0\"," +
                                                            "\"cp\":\"0\"," +
                                                            "\"fl\":\"0\"," +
                                                            "\"tz\":\"7200\"," +
                                                            "\"la\":\"ru\"," +
                                                            "\"un\":[" + vars_form.id_object_for_activation + ", 16994]," +
                                                            "\"sch\":{" +
                                                                        "\"f1\":\"0\"," +
                                                                        "\"f2\":\"0\"," +
                                                                        "\"t1\":\"0\"," +
                                                                        "\"t2\":\"0\"," +
                                                                        "\"m\":\"0\"," +
                                                                        "\"y\":\"0\"," +
                                                                        "\"w\":\"0\"" +
                                                            "}," +
                                                            "\"ctrl_sch\":{" +
                                                                        "\"f1\":\"0\"," +
                                                                        "\"f2\":\"0\"," +
                                                                        "\"t1\":\"0\"," +
                                                                        "\"t2\":\"0\"," +
                                                                        "\"m\":\"0\"," +
                                                                        "\"y\":\"0\"," +
                                                                        "\"w\":\"0\"" +
                                                            "}," +
                                                            "\"trg\":{" +
                                                                        "\"t\":\"sensor_value\"," +
                                                                        "\"p\":{" +
                                                                                "\"lower_bound\":\"0\"," +
                                                                                "\"merge\":\"1\"," +
                                                                                "\"prev_msg_diff\":\"0\"," +
                                                                                "\"sensor_name_mask\":\"*охран*\"," +
                                                                                "\"sensor_type\":\"digital\"," +
                                                                                "\"type\":\"0\"," +
                                                                                "\"upper_bound\":\"0\"" +
                                                                        "}" +
                                                            "}," +
                                                            "\"act\":[" +
                                                                        "{" +
                                                                            "\"t\":\"event\"," +
                                                                            "\"p\":{" +
                                                                                    "\"flags\":\"0\"" +
                                                                            "}" +
                                                                        "}"+
                                                            "]" +
                                                    "}"
                                                    );//Получаем айди всех елементов у которых есть доступ к данному объекту

        }

        private void build_list_account()
        {
            
            //vars_form.id_object_for_activation = "1098";

            string json = macros.wialon_request_new("&svc=core/check_accessors&params={" +
                                                    "\"items\":[\""+ vars_form.id_object_for_activation + "\"]," +
                                                    "\"flags\":\"1\"}");//Получаем айди всех елементов у которых есть доступ к данному объекту

            //Dictionary<string, string> openWith1 =new Dictionary<string,string>();
            //Dictionary<string, Dictionary<string,string>> openWith = new Dictionary<string, Dictionary<string, string>>();
            

            var wl_accounts = JsonConvert.DeserializeObject < Dictionary <string, Dictionary<int, Dictionary<string, string>>>>(json);
            string get="";
            for (int index = 0; index < wl_accounts[vars_form.id_object_for_activation].Values.Count; index++)
            {
                var item = wl_accounts.ElementAt(0);
                int key_index = item.Value.ElementAt(index).Key;
                get = get+ "," + key_index.ToString();
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
                        tree_index ++;

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

                        treeView_user_accounts.Nodes.Clear();
                        build_list_account();//обновляем тривив
                        treeView_user_accounts.Nodes[0].Expand();
                    }
                    else if (dialogResult == DialogResult.No)
                    {

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
                foreach (var t in m1.items)
                {
                    if (t.nm.Contains("@"))
                        list.Add(t.nm);
                }

                listBox_activation_list_search.DataSource = list;
                if (m1.items.Count >=1)
                {
                    if (email_textBox.Text == m1.items[0].nm)
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



            //if (email_textBox.Text.Contains("@"))
            //{
            //    string json1 = macros.wialon_request_new("&svc=core/search_items&params={" +
            //                                             "\"spec\":{" +
            //                                             "\"itemsType\":\"user\"," +
            //                                             "\"propName\":\"sys_name\"," +
            //                                             "\"propValueMask\":\"" + email_textBox.Text + "\"," +
            //                                             "\"sortType\":\"sys_name\"," +
            //                                             "\"or_logic\":\"1\"}," +
            //                                             "\"force\":\"1\"," +
            //                                             "\"flags\":\"1\"," +
            //                                             "\"from\":\"0\"," +
            //                                             "\"to\":\"1\"}"); //запрашиваем все елементі с искомім имайлом
            //    var m = JsonConvert.DeserializeObject<RootObject>(json1);

                

            //    if (m.items.Count>=1)
            //    {
            //        id_new_user = m.items[0].id.ToString();
            //        accaunt_name_textBox.Text= email_textBox.Text;
            //        account_create_button.Enabled = false;
            //        button_add_2_account.Enabled = true;
            //    }
            //    else
            //    {
            //        accaunt_name_textBox.Text = "";
            //        account_create_button.Enabled = true;
            //        button_add_2_account.Enabled = false;
            //    }
            //}
            //else
            //{
            //    accaunt_name_textBox.Text = "";
            //}


        }

        private void button_add_2_account_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("Надати авто в доступ: "+email_textBox.Text+"?", "Дозволити?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string json = macros.wialon_request_new("&svc=core/search_items&params={" +
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
                var m = JsonConvert.DeserializeObject<RootObject>(json);

                ///////////
                //Доступ на объект
                string json4 = macros.wialon_request_new("&svc=user/update_item_access&params={" +
                                                         "\"userId\":\"" + m.items[0].id + "\"," +
                                                         "\"itemId\":\"" + vars_form.id_object_for_activation + "\"," +
                                                         "\"accessMask\":\"550594678661\"}");
                var m4 = JsonConvert.DeserializeObject<RootObject>(json4);

                treeView_user_accounts.Nodes.Clear();
                build_list_account();//обновляем тривив
                treeView_user_accounts.Nodes[0].Expand();
            }
            else if (dialogResult == DialogResult.No)
            {
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
                    DialogResult dialogResult =
                        MessageBox.Show("Відалити авто з доступу: " + treeView_user_accounts.SelectedNode.Text + "?",
                            "Видалити?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string json = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                                "\"spec\":{" +
                                                                "\"itemsType\":\"user\"," +
                                                                "\"propName\":\"sys_name\"," +
                                                                "\"propValueMask\":\"" +
                                                                treeView_user_accounts.SelectedNode.Text + "\"," +
                                                                "\"sortType\":\"sys_name\"," +
                                                                "\"or_logic\":\"1\"}," +
                                                                "\"force\":\"1\"," +
                                                                "\"flags\":\"1\"," +
                                                                "\"from\":\"0\"," +
                                                                "\"to\":\"0\"}"); //запрашиваем все елементі с искомім имайлом
                        var m = JsonConvert.DeserializeObject<RootObject>(json);

                        ///////////
                        //Доступ на объект
                        string json4 = macros.wialon_request_new("&svc=user/update_item_access&params={" +
                                                                 "\"userId\":\"" + m.items[0].id + "\"," +
                                                                 "\"itemId\":\"" + vars_form.id_object_for_activation +
                                                                 "\"," +
                                                                 "\"accessMask\":\"0\"}");
                        var m4 = JsonConvert.DeserializeObject<RootObject>(json4);

                        treeView_user_accounts.Nodes.Clear();
                        build_list_account(); //обновляем тривив
                        treeView_user_accounts.Nodes[0].Expand();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                    }
                }
            }
        }

        private void account_create_button_Click(object sender, EventArgs e)
        {
            if (email_textBox.Text.Contains("@") & email_textBox.Text.Contains("."))
            {
                DialogResult dialogResult = MessageBox.Show("Стровити кабінет: " + email_textBox.Text + "?", "Створити?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string g = CreatePassword(6);
                    string json = macros.wialon_request_new("&svc=core/create_user&params={" +
                                                            "\"creatorId\":\"25\"," +
                                                            "\"name\":\"" + email_textBox.Text + "\"," +
                                                            "\"password\":\"" + g + "\"," +
                                                            "\"dataFlags\":\"1\"}");
                    var m = JsonConvert.DeserializeObject<RootObject>(json);

                    string json2 = macros.wialon_request_new("&svc=core/create_resource&params={" +
                                                            "\"creatorId\":\"" + m.item.id + "\"," +
                                                            "\"name\":\"" + email_textBox.Text + "\"," +
                                                            "\"dataFlags\":\"1\"," +
                                                            "\"skipCreatorCheck\":\"1\"}");
                    var m2 = JsonConvert.DeserializeObject<RootObject>(json2);

                    string json3 = macros.wialon_request_new("&svc=core/create_resource&params={" +
                                                             "\"creatorId\":\"" + m.item.id + "\"," +
                                                             "\"name\":\"" + email_textBox.Text + "_user" + "\"," +
                                                             "\"dataFlags\":\"1\"," +
                                                             "\"skipCreatorCheck\":\"1\"}");
                    var m3 = JsonConvert.DeserializeObject<RootObject>(json3);


                    ///////////
                    //Доступ на объект
                    string json4 = macros.wialon_request_new("&svc=user/update_item_access&params={" +
                                                            "\"userId\":\"" + m.item.id + "\"," +
                                                            "\"itemId\":\"" + vars_form.id_object_for_activation + "\"," +
                                                            "\"accessMask\":\"550594678661\"}");
                    var m4 = JsonConvert.DeserializeObject<RootObject>(json4);
                    //Доступ на ресурс 1
                    string json5 = macros.wialon_request_new("&svc=user/update_item_access&params={" +
                                                             "\"userId\":\"" + m.item.id + "\"," +
                                                             "\"itemId\":\"" + m2.item.id + "\"," +
                                                             "\"accessMask\":\"4648339329\"}");
                    var m5 = JsonConvert.DeserializeObject<RootObject>(json5);
                    //Доступ на ресурс 2
                    string json6 = macros.wialon_request_new("&svc=user/update_item_access&params={" +
                                                             "\"userId\":\"" + m.item.id + "\"," +
                                                             "\"itemId\":\"" + m3.item.id + "\"," +
                                                             "\"accessMask\":\"52785134440321\"}");
                    var m6 = JsonConvert.DeserializeObject<RootObject>(json6);

                    treeView_user_accounts.Nodes.Clear();
                    build_list_account();//обновляем тривив
                    treeView_user_accounts.Nodes[0].Expand();

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

        private void load_form()
        {
            string json = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                    "\"spec\":{" +
                                                    "\"itemsType\":\"avl_unit\"," +
                                                    "\"propName\":\"sys_id\"," +
                                                    "\"propValueMask\":\"" + vars_form.id_object_for_activation + "\", " +
                                                    "\"sortType\":\"sys_name\"," +
                                                    "\"or_logic\":\"1\"}," +
                                                    "\"force\":\"1\"," +
                                                    "\"flags\":\"4611686018427387903\"," +
                                                    "\"from\":\"0\"," +
                                                    "\"to\":\"5\"}");
            var m = JsonConvert.DeserializeObject<RootObject>(json);

            name_object_current_textBox.Text = m.items[0].nm;
            imei_object_textBox.Text = m.items[0].uid;
            uvaga_textBox.Text = m.items[0].flds[1].v;
            name_obj_new_textBox.Text = m.items[0].nm;
        }

        private void button_chenge_uvaga_Click(object sender, EventArgs e)
        {
            string json = macros.wialon_request_new("&svc=item/update_name&params={" +
                                                    "\"itemId\":\""+vars_form.id_object_for_activation+"\"," +
                                                    "\"name\":\""+name_obj_new_textBox.Text.ToString()+"\"}");
            var m = JsonConvert.DeserializeObject<RootObject>(json);

            if (m.error == 0)
            {
                
                string st =macros.sql_command("UPDATE btk.Object " +
                               "SET " +
                               "Object_name = '" + name_obj_new_textBox.Text + "' " +
                               "WHERE " +
                               "idObject = '" + vars_form.id_db_object_for_activation + "';");
                if (st == "")
                {
                    load_form();
                    MessageBox.Show("Змінено!");
                }
                else
                {
                    MessageBox.Show("Упс, здається не вийшло");
                }
            }
            else
            {
                MessageBox.Show("Упс, здається не вийшло");
            }

        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            if (name_obj_new_textBox.Text == name_object_current_textBox.Text)
            {
                this.Close();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Назва обєкту змінена", "Зберегти?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //button_chenge_uvaga.PerformClick();
                    this.Close();

                }
                else if (dialogResult == DialogResult.No)
                {
                    this.Close();
                }
            }
        }

        private void treeView_user_accounts_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (_unselectableNodes.Contains(e.Node))
            {
                e.Cancel = true;
            }   
        }

        private void listBox_activation_list_search_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox_activation_list_search.SelectedItem != null)
            {
                email_textBox.Text = listBox_activation_list_search.SelectedValue.ToString();
            }
            
           
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
                    new_pass = CreatePassword(6);
                    string json1 = macros.wialon_request_new("&svc=user/update_password&params={" +
                                                             "\"userId\":\"" + m.items[0].id + "\"," +
                                                             "\"oldPassword\":\"\"," +
                                                             "\"newPassword\":\"" + new_pass + "\"}");
                    var m1 = JsonConvert.DeserializeObject<RootObject>(json1);

                }
                else if (dialogResult == DialogResult.No)
                {
                }
                
            }
        }

        private void button_add_vo1_Click(object sender, EventArgs e)
        {
            VO_add vo_add_form = new VO_add();
            vo_add_form.Show();
        }

        private void button_add_vo2_Click(object sender, EventArgs e)
        {
            VO_add vo_add_form = new VO_add();
            vo_add_form.Show();
        }

        private void button_add_vo3_Click(object sender, EventArgs e)
        {
            VO_add vo_add_form = new VO_add();
            vo_add_form.Show();
        }

        private void button_add_vo4_Click(object sender, EventArgs e)
        {
            VO_add vo_add_form = new VO_add();
            vo_add_form.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            VO_add vo_add_form = new VO_add();
            vo_add_form.Show();
        }
    }
}
