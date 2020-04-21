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
using System.Web;
using System.Windows.Forms;

namespace Disp_WinForm
{
    public partial class Activation_Form : Form
    {
        Macros macros = new Macros();
        private string id_new_user;
        private List<TreeNode> _unselectableNodes = new List<TreeNode>();
        public Activation_Form()
        {
            //TopMost = true;
            InitializeComponent();
            Read_data();
            load_form();
            build_list_account();
            button_add_2_account.Enabled = false;

        }

        //Function fo generete password
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

        private void Read_data()
        {
            if (vars_form.if_open_created_testing == 1)
            {
            }
        }


                //init command on load form
        private void load_form()
        {
            string json = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                    "\"spec\":{" +
                                                    "\"itemsType\":\"avl_unit\"," +
                                                    "\"propName\":\"sys_id\"," +
                                                    "\"propValueMask\":\"" + vars_form.id_wl_object_for_activation + "\", " +
                                                    "\"sortType\":\"sys_name\"," +
                                                    "\"or_logic\":\"1\"}," +
                                                    "\"force\":\"1\"," +
                                                    "\"flags\":\"4611686018427387903\"," +
                                                    "\"from\":\"0\"," +
                                                    "\"to\":\"1\"}");
            var m = JsonConvert.DeserializeObject<RootObject>(json);

            name_object_current_textBox.Text = m.items[0].nm;
            imei_object_textBox.Text = m.items[0].uid;
            name_obj_new_textBox.Text = m.items[0].nm;

            foreach (var keyvalue in m.items[0].flds)
            {

                if (keyvalue.Value.n.Contains("УВАГА"))
                {
                    //Chenge feild Uvaga
                    uvaga_textBox.Text = keyvalue.Value.v;
                }
                if (keyvalue.Value.n.Contains("Кодове "))
                {
                    //Chenge feild Кодове слово
                    kodove_slovo_textBox.Text = keyvalue.Value.v;
                }

                
                

            }
            //Открітие отработаніх активаций. ХЗ ли нужно
            ////load VO
            //DataTable VO = macros.GetData("SELECT Users_idUsers, comment, alarm_button FROM btk.Activation_object where Object_idObject = '"+vars_form.id_db_object_for_activation+"';");


            //load VO
            DataTable VO = macros.GetData("select Kontakti_idKontakti, VOcol_num_vo from btk.VO where Object_idObject = '" + vars_form.id_db_object_for_activation + "';");
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
                    if (row[1].ToString() == "4")
                    {
                        string VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,'')) FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + row[0].ToString() + "';");
                        string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + row[0].ToString() + "';");
                        if (VO_phone2 == "   -   -")
                        { VO_phone2 = ""; }
                        if (VO_familia_imya_phone == "" || VO_familia_imya_phone == "Пусто Пусто , ")
                        {
                            textBox_vo4.Text = "";
                        }
                        else
                        {
                            textBox_vo4.Text = VO_familia_imya_phone + ", " + VO_phone2;
                        }
                    }
                    if (row[1].ToString() == "5")
                    {
                        string VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,'')) FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + row[0].ToString() + "';");
                        string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + row[0].ToString() + "';");
                        if (VO_phone2 == "   -   -")
                        { VO_phone2 = ""; }
                        if (VO_familia_imya_phone == "" || VO_familia_imya_phone == "Пусто Пусто , ")
                        {
                            textBox_vo5.Text = "";
                        }
                        else
                        {
                            textBox_vo5.Text = VO_familia_imya_phone + ", " + VO_phone2;
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
                if (textBox_vo4.Text == "")
                {
                    vars_form.transfer_vo4_vo_form = "1";
                }
                if (textBox_vo5.Text == "")
                {
                    vars_form.transfer_vo5_vo_form = "1";
                }


            }

            //Load data from opened activation
            string tk_tested = macros.sql_command("SELECT alarm_button FROM btk.Activation_object where idActivation_object = '"+vars_form.id_db_activation_for_activation+"'");
            if (tk_tested == "1")
            { checkBox_tk_tested.Checked = true; }
            else if (tk_tested == "0" || tk_tested == "")
            { checkBox_tk_tested.Checked = false; }

            string pin_chenged = macros.sql_command("SELECT pin_chenged FROM btk.Activation_object where idActivation_object = '" + vars_form.id_db_activation_for_activation + "'");
            if (pin_chenged == "1")
            { checkBox_pin_chenged.Checked = true; }
            else if(pin_chenged == "0" || pin_chenged == "") 
            { checkBox_pin_chenged.Checked = false; }

            //load data from zayavki
            DataTable table2 = new DataTable();
            table2 = macros.GetData("SELECT " +
                "Sobstvennik_avto_neme ," +
                "Kontakt_name_avto_1 ," +
                "Kontakt_phone_avto_1 ," +
                "Kontakt_name_avto_2 ," +
                "Kontakt_phone_avto_2 ," +
                "email ," +
                "products.product_name ," +
                "Kontragenti.Kontragenti_short_name " +
                "from " +
                "btk.Zayavki, btk.products, btk.Kontragenti " +
                "where " +
                "Zayavki.products_idproducts = products.idproducts " +
                "and Kontragenti.idKontragenti = Zayavki.Kontragenti_idKontragenti_zakazchik " +
                "and Zayavki.idZayavki = '" +vars_form.id_db_zayavki_for_activation +"'" +
                ";");

            textBox_vlasnik.Text = table2.Rows[0][0].ToString();
            textBox_kont1.Text = table2.Rows[0][1].ToString();
            maskedTextBox_kont_phone1.Text = table2.Rows[0][2].ToString();
            textBox_kont2.Text = table2.Rows[0][3].ToString();
            maskedTextBox_cont_phone2.Text = table2.Rows[0][4].ToString();
            textBox_email.Text = table2.Rows[0][5].ToString();
            textBox_product.Text = table2.Rows[0][6].ToString();
            textBox_zamovnik.Text = table2.Rows[0][7].ToString();
            //load coments for activation
            DataTable table = new DataTable();

            
                table = macros.GetData("SELECT " +
                    "coments as 'Коментарій'," +
                    "date_insert as 'Дата'," +
                    "Users.username as 'Користувач' " +
                    "FROM btk.activation_comments, btk.Users " +
                    "where " +
                    "Users.idUsers = activation_comments.Users_idUsers " +
                    "and Activation_object_idActivation_object = '" +vars_form.id_db_activation_for_activation+"'; ");

            dataGridView_activation_coments.DataSource = table;

            dataGridView_activation_coments.Columns["Коментарій"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView_activation_coments.Columns["Дата"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_activation_coments.Columns["Користувач"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_activation_coments.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            
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

        //build list of user account where exist right for object
        private void build_list_account()
        {
            //vars_form.id_object_for_activation = "1098";


            string json = macros.wialon_request_new("&svc=core/check_accessors&params={" +
                                                    "\"items\":[\""+ vars_form.id_wl_object_for_activation + "\"]," +
                                                    "\"flags\":\"1\"}");//Получаем айди всех елементов у которых есть доступ к данному объекту            

            var wl_accounts = JsonConvert.DeserializeObject < Dictionary <string, Dictionary<int, Dictionary<string, string>>>>(json);
            string get="";
            for (int index = 0; index < wl_accounts[vars_form.id_wl_object_for_activation].Values.Count; index++)
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

        // set node of treeview as unselectable
        private void treeView_user_accounts_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (_unselectableNodes.Contains(e.Node))
            {
                e.Cancel = true;
            }
        }


        /// User account manage
        ///
        // User account live search with outout to listBox_activation_list_search. Or new user name input
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

        //On two double mouse click selectet needed user account for add right for object
        private void listBox_activation_list_search_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox_activation_list_search.SelectedItem != null)
            {
                email_textBox.Text = listBox_activation_list_search.SelectedValue.ToString();
            }
        }

        //Remove selected user account
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
                        macros.GetData("insert into btk.Client_accounts (name, pass, date, reason, Object_idObject, Users_idUsers) value ('" + treeView_user_accounts.SelectedNode.Text + "','','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','Delete account','" + vars_form.id_db_object_for_activation + "','" + vars_form.user_login_id + "');");

                        build_list_account();//обновляем тривив


                        
                        
                    }
                    else if (dialogResult == DialogResult.No)
                    {

                    }
                }
            }
        }

        // give right on selectetd object to exist user account
        private void button_add_2_account_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("Надати авто в доступ: "+email_textBox.Text+"?", "Дозволити?", MessageBoxButtons.YesNo);
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
                                                         "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\"," +
                                                         "\"accessMask\":\"550611455877\"}");
                var set_object_accsess = JsonConvert.DeserializeObject<RootObject>(set_object_accsess_answer);

                //get data resource for selected user account
                string get_resource_user_answer = macros.wl_core_search_items("avl_resource", "sys_name", email_textBox.Text, "sys_name",1,0,1);
                var get_resource_user = JsonConvert.DeserializeObject<RootObject>(get_resource_user_answer);

                //get data resource_user for selected user account
                string get_resource_user_user_answer = macros.wl_core_search_items("avl_resource", "sys_name", email_textBox.Text + "_user", "sys_name", 1, 0, 1);
                var get_resource_user_user = JsonConvert.DeserializeObject<RootObject>(get_resource_user_user_answer);

                

                //get product id from id object
                string product_id = get_product_from_id_object(vars_form.id_wl_object_for_activation);

                //create notif depends product id

                if (product_id == "10" || product_id == "11")//CNTP_910, CNTK_910
                {
                    create_notif_910(email_textBox.Text, user_data.items[0].id, vars_form.id_wl_object_for_activation, get_resource_user.items[0].id, get_resource_user_user.items[0].id);
                }
                else if (product_id == "2" || product_id == "3")//CNTP, CNTK
                {
                    create_notif_730(email_textBox.Text, user_data.items[0].id, vars_form.id_wl_object_for_activation, get_resource_user.items[0].id, get_resource_user_user.items[0].id);
                }
                else
                {
                    MessageBox.Show("Невідомий продукт, сповіщення не створені");
                }

                //log user action
                macros.LogUserAction(vars_form.user_login_id, "Дозволити користувачу перегляд авто", "", "Надано доступ Account: " + email_textBox.Text + "до обєкту" + vars_form.id_wl_object_for_activation, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));

                //update treeView_user_accounts after making chenge
                build_list_account();

            }
            else if (dialogResult == DialogResult.No)
            {
            }
        }

        // remove right to object on selectes user account  
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
                                    if (num.Value.Un[0].ToString() == vars_form.id_wl_object_for_activation)
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
                                                                 "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\"," +
                                                                 "\"accessMask\":\"0\"}");
                        var m4 = JsonConvert.DeserializeObject<RootObject>(json4);

                        //log user action
                        macros.LogUserAction(vars_form.user_login_id, "Прибрати з доступу користувача", saving_selected_account, vars_form.id_wl_object_for_activation, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));


                        //update treeView_user_accounts after making chenge
                        build_list_account();

                        
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                    }
                }
            }
        }
        //create new user account and giv right to object
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
                                                            "\"creatorId\":\""+vars_form.wl_user_id+"\"," +
                                                            "\"name\":\"" + email_textBox.Text + "\"," +
                                                            "\"password\":\"" +pass+ "\"," +
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
                                                            "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\"," +
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
                    string product_id = get_product_from_id_object(vars_form.id_wl_object_for_activation);

                    //create notif depends product id
                    
                    if (product_id == "10" || product_id == "11" || product_id == "12" || product_id == "13")//CNTP_910, CNTK_910
                    {
                        create_notif_910(email_textBox.Text, created_user_data.item.id, vars_form.id_wl_object_for_activation, created_resource_data.item.id, created_resource_user_data.item.id);
                    }
                    else if (product_id == "2" || product_id == "3")//CNTP, CNTK
                    {
                        create_notif_730(email_textBox.Text, created_user_data.item.id, vars_form.id_wl_object_for_activation, created_resource_data.item.id, created_resource_user_data.item.id);
                    }
                    else 
                    { 
                        MessageBox.Show("Невідомий продукт, сповіщення не створені"); 
                    }
                    string Body = "<p>Добрий день!</p><p></p><p>Дякуємо за ваш вибір!</p><p>Для вас було створено доступ в систему моніторингу ВЕНБЕСТ. </p><p>----------------------------------------------</p><p>Для входу в систему моніторингу за допомогою мобільного додатку:</p><p>1.Завантажте мобільний додаток Wialon Local: https://venbest.ua/gps-prilozheniia/</p> <p>2.При першому вході в мобільний додаток введіть такі дані:</p><p>a.Адреса серверу: https://navi.venbest.com.ua/;</p> <p>Посилання вводиться зверху сторінки вводу логіну та паролю. Після його введення необхідно натиснути на іконку у вигляді щита (праворуч рядка вводу).</p><p>b.Логін: " + email_textBox.Text + "</p><p>c.Пароль: " + pass + " </p><p>Зверніть, будь ласка, увагу, що логін та пароль чутливий до регістру символів, які ви вводите.</p><p> <br></p><p>3.Якщо ви бажаєте отримувати сповіщення, увімкніть їх в налаштуваннях.</p><p>----------------------------------------------</p><p>Для входу в систему моніторингу за допомогою браузеру:</p><p>1.Перейдіть за посиланням: https://navi.venbest.com.ua/</p> <p>2.Введіть логін: " + email_textBox.Text + "</p><p>3.Введіть пароль: " + pass + "</p><p>  <br></p><p>Змініть, будь ласка, пароль в налаштуваннях користувача при вході через браузер.</p><p>----------------------------------------------</p><p>Департамент супутникових систем охорони</p><p>Група Компаній «ВЕНБЕСТ»</p><p>Т 044 501 33 77;</p><p>auto@venbest.com.ua | https://venbest.ua/ohrana-avto-i-zashchita-ot-ugona</p>";
                    macros.send_mail_auto(email_textBox.Text, "ВЕНБЕСТ. Вхід в систему моніторингу", Body);
                    macros.send_mail_auto("auto@venbest.com.ua", "ВЕНБЕСТ. Вхід в систему моніторингу", Body);

                    //Save in db client account

                    macros.GetData("insert into btk.Client_accounts (name, pass, date, reason, Object_idObject, Users_idUsers) value ('" + email_textBox.Text + "','" + pass + "','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','Create account','" + vars_form.id_db_object_for_activation + "','" + vars_form.user_login_id + "');");


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

        //generate new pass for user account, send email to user
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

                    string Body = "<p>Добрий день!</p><p>;</p><p>Відповідно до Вашого запиту,</p><p>Для Вас було відновлено пароль для доступу в систему моніторингу ВЕНБЕСТ. </p><p>----------------------------------------------</p><p>Для входу в систему моніторингу за допомогою мобільного додатку:</p><p>1.Завантажте мобільний додаток Wialon Local: https://venbest.ua/gps-prilozheniia/</p> <p>2.При першому вході в мобільний додаток введіть такі дані:</p><p>a.Адреса серверу: https://navi.venbest.com.ua/;</p> <p>Посилання вводиться зверху сторінки вводу логіну та паролю. Після його введення необхідно натиснути на іконку у вигляді щита (праворуч рядка вводу).</p><p>b.Логін: "+ treeView_user_accounts.SelectedNode.Text + "</p><p>c.Пароль: " + pass + " </p><p>Зверніть, будь ласка, увагу, що логін та пароль чутливий до регістру символів, які ви вводите.</p><p> <br></p><p>3.Якщо ви бажаєте отримувати сповіщення, увімкніть їх в налаштуваннях.</p><p>----------------------------------------------</p><p>Для входу в систему моніторингу за допомогою браузеру:</p><p>1.Перейдіть за посиланням: https://navi.venbest.com.ua/</p> <p>2.Введіть логін: "+ treeView_user_accounts.SelectedNode.Text + "</p><p>3.Введіть пароль: "+ pass + "</p><p>  <br></p><p>Змініть, будь ласка, пароль в налаштуваннях користувача при вході через браузер.</p><p>----------------------------------------------</p><p>Департамент супутникових систем охорони</p><p>Група Компаній «ВЕНБЕСТ»</p><p>Т 044 501 33 77;</p><p>auto@venbest.com.ua | https://venbest.ua/ohrana-avto-i-zashchita-ot-ugona</p>";
                    macros.send_mail_auto(treeView_user_accounts.SelectedNode.Text, "ВЕНБЕСТ. Вхід в систему моніторингу", Body+ pass);
                    macros.send_mail_auto("auto@venbest.com.ua", "ВЕНБЕСТ. Вхід в систему моніторингу", Body + pass);

                    //Save in db client account
                    macros.GetData("insert into btk.Client_accounts (name, pass, date, reason, Object_idObject, Users_idUsers) value ('" + treeView_user_accounts.SelectedNode.Text + "','" + pass + "','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','Chenge pass account','" + vars_form.id_db_object_for_activation + "','" + vars_form.user_login_id + "');");
                    
                }
                else if (dialogResult == DialogResult.No)
                {
                }

            }
        }

        // Delete notif for needed resource
        private string delete_notif(int resours_id, int notif_id)
        {
            string CreteNotifAnswer = macros.wialon_request_new("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\""+notif_id+"\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"delete\"" +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                        "}"
                                                    );
            return CreteNotifAnswer;
        }

        // CNTP_910 create nitif for new user account
        private void create_notif_910(string user_account_name, int user_account_id, string object_id, int resours_id, int resours_user_id)
        {
            //Cработка: Датчик удара/наклона/буксировки
            string CreteNotifAnswer = macros.wialon_request_new("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\""+ resours_id +"\"," +                                             /* ID ресурса */ 
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
                                                                                    "\"email_to\":\""+ user_account_name +"\"," +
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
                                                                                    "\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\"+"\"" + ":" + "[" + user_account_id + "]" + "}\"" +
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

        private void button_chenge_uvaga_Click(object sender, EventArgs e)
        {
            string json = macros.wialon_request_new("&svc=item/update_name&params={" +
                                                    "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\"," +
                                                    "\"name\":\"" + name_obj_new_textBox.Text.ToString() + "\"}");
            var m = JsonConvert.DeserializeObject<RootObject>(json);

            if (m.error == 0)
            {

                string st = macros.sql_command("UPDATE btk.Object " +
                               "SET " +
                               "Object_name = '" + name_obj_new_textBox.Text + "' " +
                               "WHERE " +
                               "idObject = '" + vars_form.id_db_object_for_activation + "';");
                if (st == "")
                {
                    load_form();

                    //log user action
                    macros.LogUserAction(vars_form.user_login_id, "Зміна поля Увага", name_object_current_textBox.Text, vars_form.id_db_object_for_activation , Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));

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

        private void button_add_vo1_Click(object sender, EventArgs e)
        {
            vars_form.num_vo = 1;
            VO_add vo_add_form = new VO_add();
            vo_add_form.Activated += new EventHandler(form_vo_add_activated);
            vo_add_form.FormClosed += new FormClosedEventHandler(form_vo_add_deactivated);
            vo_add_form.Show();
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
                {textBox_vo2.Text = VO_familia_imya_phone + ", " + VO_phone2;
                }
                
            }
            if (vars_form.num_vo == 3)
            {
                string VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,'')) FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + vars_form.transfer_vo3_vo_form + "';");
                string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + vars_form.transfer_vo3_vo_form + "';");
                if (VO_phone2 == "   -   -" )
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
            if (vars_form.num_vo == 4)
            {
                string VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,'')) FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + vars_form.transfer_vo4_vo_form + "';");
                string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + vars_form.transfer_vo4_vo_form + "';");
                if (VO_phone2 == "   -   -")
                { VO_phone2 = ""; }
                textBox_vo4.Text = VO_familia_imya_phone + ", " + VO_phone2;
            }
            if (vars_form.num_vo == 5)
            {
                string VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,'')) FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + vars_form.transfer_vo5_vo_form + "';");
                string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + vars_form.transfer_vo5_vo_form + "';");
                if (VO_phone2 == "   -   -")
                { VO_phone2 = ""; }
                textBox_vo5.Text = VO_familia_imya_phone + ", " + VO_phone2;
            }
            this.Visible = true;// разблокируем окно контрагентов кактолько закрыто окно добавления контрагента
        }

        private void button_add_vo2_Click(object sender, EventArgs e)
        {
            vars_form.num_vo = 2;
            VO_add vo_add_form = new VO_add();
            vo_add_form.Activated += new EventHandler(form_vo_add_activated);
            vo_add_form.FormClosed += new FormClosedEventHandler(form_vo_add_deactivated);
            vo_add_form.Show();
        }

        private void button_add_vo3_Click(object sender, EventArgs e)
        {
            vars_form.num_vo = 3;
            VO_add vo_add_form = new VO_add();
            vo_add_form.Activated += new EventHandler(form_vo_add_activated);
            vo_add_form.FormClosed += new FormClosedEventHandler(form_vo_add_deactivated);
            vo_add_form.Show();
        }

        private void button_add_vo4_Click(object sender, EventArgs e)
        {
            vars_form.num_vo = 4;
            VO_add vo_add_form = new VO_add();
            vo_add_form.Activated += new EventHandler(form_vo_add_activated);
            vo_add_form.FormClosed += new FormClosedEventHandler(form_vo_add_deactivated);
            vo_add_form.Show();
        }
        private void button_add_vo5_Click(object sender, EventArgs e)
        {
            vars_form.num_vo = 5;
            VO_add vo_add_form = new VO_add();
            vo_add_form.Activated += new EventHandler(form_vo_add_activated);
            vo_add_form.FormClosed += new FormClosedEventHandler(form_vo_add_deactivated);
            vo_add_form.Show();
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

        private void save_button_Click(object sender, EventArgs e)
        {
            if (comboBox_activation_result.SelectedIndex == -1)
            {
                MessageBox.Show("Оберіть результат тестування");
                return;
            }
            else if ((comboBox_activation_result.SelectedIndex == 1 | comboBox_activation_result.SelectedIndex == 2) & textBox_comments.Text=="")
            {
                MessageBox.Show("Опишіть ситуацію в коментрі.");
                return;
            }

            if ((comboBox_activation_result.SelectedIndex == 1 | comboBox_activation_result.SelectedIndex == 2) | textBox_comments.Text != "")
            {
                macros.sql_command("insert into btk.activation_comments (" +
                    "coments," +
                    "date_insert," +
                    "Activation_object_idActivation_object," +
                    "Users_idUsers" +
                    ") values (" +
                    "'"+textBox_comments.Text+"', " +
                    "'"+ Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                    "'"+vars_form.id_db_activation_for_activation+"'," +
                    "'"+vars_form.user_login_id+"'" +
                    "); ");
            }

            //Загружаем произвольные поля объекта
            string json = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_id\"," +
                                                     "\"propValueMask\":\"" + vars_form.id_wl_object_for_activation + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"or_logic\":\"1\"," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"15208907\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"1\"}");//15208907

            var object_data = JsonConvert.DeserializeObject<RootObject>(json);


            

            if (name_object_current_textBox.Text != name_obj_new_textBox.Text)
            {
                //Меняем имя об"екта
                string name_answer = macros.wialon_request_lite("&svc=item/update_name&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\","
                                                                + "\"name\":\"" + name_obj_new_textBox.Text + "\"}");
            }
            
            foreach (var keyvalue in object_data.items[0].flds)
            {

                if (keyvalue.Value.n.Contains("УВАГА"))
                {
                    if (keyvalue.Value.v != uvaga_textBox.Text)
                    {
                        //Chenge feild Uvaga
                        string json2 = macros.wialon_request_new("&svc=item/update_custom_field&params={" +
                                                                 "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\"," +
                                                                 "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                                 "\"callMode\":\"update\"," +
                                                                 "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                                 "\"v\":\"" + uvaga_textBox.Text + "\"}");
                    }
                }
                if (keyvalue.Value.n.Contains("Кодове "))
                {
                    if (keyvalue.Value.v != kodove_slovo_textBox.Text)
                    {
                        //Chenge feild Кодове слово
                        string json2 = macros.wialon_request_new("&svc=item/update_custom_field&params={" +
                                                             "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\"," +
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
                            macros.sql_command("insert into btk.VO (Object_idObject,Kontakti_idKontakti,VOcol_num_vo,VOcol_date_add,Users_idUsers) values('" + vars_form.id_db_object_for_activation + "','" + vars_form.transfer_vo1_vo_form + "','1','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','" + vars_form.user_login_id + "');");
                        }
                        string json2 = macros.wialon_request_new("&svc=item/update_custom_field&params={" +
                                                             "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\"," +
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
                            macros.sql_command("insert into btk.VO (Object_idObject,Kontakti_idKontakti,VOcol_num_vo,VOcol_date_add,Users_idUsers) values('" + vars_form.id_db_object_for_activation + "','" + vars_form.transfer_vo2_vo_form + "','2','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','" + vars_form.user_login_id + "');");
                        }

                        string json2 = macros.wialon_request_new("&svc=item/update_custom_field&params={" +
                                                             "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\"," +
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
                            macros.sql_command("insert into btk.VO (Object_idObject,Kontakti_idKontakti,VOcol_num_vo,VOcol_date_add,Users_idUsers) values('" + vars_form.id_db_object_for_activation + "','" + vars_form.transfer_vo3_vo_form + "','3','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','" + vars_form.user_login_id + "');");
                        }
                        string json2 = macros.wialon_request_new("&svc=item/update_custom_field&params={" +
                                                             "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\"," +
                                                             "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                             "\"callMode\":\"update\"," +
                                                             "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                             "\"v\":\"" + textBox_vo3.Text + "\"}");
                    }
                }
            }




            // insert/update activation

            if (vars_form.if_open_created_activation == 1)
            {
                string t = comboBox_activation_result.GetItemText(comboBox_activation_result.SelectedItem);
                macros.sql_command("update btk.Activation_object " +
                    "set " +
                    "Activation_date = '" + Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy-MM-dd") + "', " +
                    "Users_idUsers = '" + vars_form.user_login_id + "'," +
                    "Object_idObject = '" + vars_form.id_db_object_for_activation + "'," +
                    "Activation_objectcol_result = '" + comboBox_activation_result.GetItemText(comboBox_activation_result.SelectedItem) + "'," +
                    "new_name_obj = '" + name_obj_new_textBox.Text + "'," +
                    "new_pole_uvaga = '" + uvaga_textBox.Text + "'," +
                    "vo1 = '" + vars_form.transfer_vo1_vo_form + "'," +
                    "vo2 = '" + vars_form.transfer_vo2_vo_form + "'," +
                    "vo3 = '" + vars_form.transfer_vo3_vo_form + "'," +
                    "vo4 = '" + vars_form.transfer_vo4_vo_form + "'," +
                    "vo5 = '" + vars_form.transfer_vo5_vo_form + "'," +
                    "kodove_slovo = '" + kodove_slovo_textBox.Text + "'," +
                    "alarm_button = '" + (checkBox_tk_tested.Checked ? "1" : "0") + "'," +
                    "pin_chenged = '" + (checkBox_pin_chenged.Checked ? "1" : "0") + "'" +
                    "where " +
                    "idActivation_object = '"+ vars_form.id_db_activation_for_activation + "'" +
                    ";");

                

            }
            else if (vars_form.if_open_created_activation == 0)
            {
                macros.sql_command("insert into btk.Activation_object (" +
                                                                    "Activation_date, " +
                                                                    "Users_idUsers, " +
                                                                    "Object_idObject," +
                                                                    "Activation_objectcol_result," +
                                                                    "new_name_obj," +
                                                                    "new_pole_uvaga," +
                                                                    "vo1," +
                                                                    "vo2," +
                                                                    "vo3," +
                                                                    "vo4," +
                                                                    "vo5, " +
                                                                    "kodove_slovo," +
                                                                    "alarm_button," +
                                                                    "pin_chenged" +
                                                                    ") " +
                                                                    "values (" +
                                                                    "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                    "'" + vars_form.user_login_id + "'," +
                                                                    "'" + vars_form.id_db_object_for_activation + "'," +
                                                                    "'" + comboBox_activation_result.GetItemText(comboBox_activation_result.SelectedItem) + "'," +
                                                                    "'" + name_obj_new_textBox.Text + "'," +
                                                                    "'" + uvaga_textBox.Text + "'," +
                                                                    "'" + vars_form.transfer_vo1_vo_form + "'," +
                                                                    "'" + vars_form.transfer_vo2_vo_form + "'," +
                                                                    "'" + vars_form.transfer_vo3_vo_form + "'," +
                                                                    "'" + vars_form.transfer_vo4_vo_form + "'," +
                                                                    "'" + vars_form.transfer_vo5_vo_form + "'," +
                                                                    "'" + kodove_slovo_textBox.Text + "'," +
                                                                    "'" + (checkBox_tk_tested.Checked ? "1" : "0") + "'," +
                                                                    "'" + (checkBox_pin_chenged.Checked ? "1" : "0") + "'" +
                                                                    ");");

                string get_id_activacii = macros.sql_command("SELECT MAX(idActivation_object) FROM btk.Activation_object;");
                macros.sql_command("update btk.Zayavki set Activation_object_idActivation_object = '" + get_id_activacii + "' where idZayavki = '" + vars_form.id_db_zayavki_for_activation + "';");
            }
            this.Close();
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

        private void button_call_kont1_Click(object sender, EventArgs e)
        {
            string path = macros.GetProcessPath("microsip");
            if (path=="")
            { MessageBox.Show("Для виклику необхідно запустити Microsip!"); return; }
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + path + " " + maskedTextBox_kont_phone1.Text;
            process.StartInfo = startInfo;
            process.Start();


        }

        private void button_call_cont2_Click(object sender, EventArgs e)
        {
            string path = macros.GetProcessPath("microsip");
            if (path == "")
            { MessageBox.Show("Для виклику необхідно запустити Microsip!"); return; }
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + path + " " + maskedTextBox_cont_phone2.Text;
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}
