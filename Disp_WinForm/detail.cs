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
using System.IO;
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
        private string idSimCard;
        private string idSim2Card;
        private bool StateBlockButton;
        private bool StateServiceButton;
        private bool StateAutostartButton;
        private bool StateArmtButton;
        private string VIN_object;
        private string RoumingAccept;
        private string IMEI_object;
        private int Parking1ExistInWL = 0;
        private int Parking2ExistInWL = 0;
        private int Parking3ExistInWL = 0;
        private int Parking4ExistInWL = 0;
        private int Parking5ExistInWL = 0;

        private DateTime DateOpened;
        private DateTime DateClosed;
        private string StatusOpened;
        private string StatusClosed;





        public detail()
        {

            this.Font = new System.Drawing.Font("Arial", vars_form.setting_font_size);

            InitializeComponent();

            string t = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\Firefox";
            Xpcom.Initialize(t);
            GeckoPreferences.User["dom.max_script_run_time"] = 0;
            //GeckoPreferences.User["javascript.enabled"] = false;

            // Looad varible data
            _id_notif = vars_form.id_notif;
            _id_status = vars_form.id_status;
            _user_login_id = vars_form.user_login_id;
            _search_id = vars_form.search_id;
            _unit_name = vars_form.unit_name;
            _restrict_un_group = vars_form.restrict_un_group;
            _alarm_name = vars_form.alarm_name.Replace("\n", String.Empty);
            _eid = vars_form.eid;
            wl_id = _search_id;
            id_db_obj = macros.sql_command("SELECT idObject FROM btk.Object where Object.Object_id_wl = '" + _search_id + "';");



            dateTimePicker_close_plan_date.Value = DateTime.Now.AddDays(60);


            comboBox_status_trevogi.Text = _id_status;
            this.Text = "ID Тривоги: " + _id_notif + ", " + vars_form.unit_name + ", " + _alarm_name + ": " + vars_form.zvernenya;



            get_remaynder();


            accsses();


            TreeView_zapolnyaem();


            mysql_get_hronologiya_trivog();


            mysql_get_group_alarm();


            get_close_object_data();


            get_sensor_value();


            GetUserOtvetstvenyi();

            //await Task.Run(() => arhiv_object());
            ////arhiv_object();
            //label10.Text = "9";


        }

   

        private void accsses()
        {
            DataTable users_accses = macros.GetData("SELECT idUsers, username, accsess_lvl FROM btk.Users;");
            foreach (DataRow user in users_accses.Rows)
            {
                if (user["idUsers"].ToString() == vars_form.user_login_id)
                {
                    if (Convert.ToInt32(user["accsess_lvl"]) <= 4)// wialon
                    {
                        tabControl2.TabPages.Remove(tabPage_dii_z_obectom);
                    }
                    else if (Convert.ToInt32(user["accsess_lvl"]) == 5)//service
                    {
                        if (Convert.ToInt32(user["idUsers"]) != 5)// if not service
                        {
                            tabControl2.TabPages.Remove(tabPage_dii_z_obectom);
                        }
                    }
                    else if (Convert.ToInt32(user["accsess_lvl"]) == 6)//Lozinskiy
                    {
                        tabControl2.TabPages.Remove(tabPage_dii_z_obectom);
                        comboBox_otvetstvenniy.Enabled = false;
                    }
                    else if (Convert.ToInt32(user["accsess_lvl"]) <= 7)//danilchenko
                    {
                        tabControl2.TabPages.Remove(tabPage_dii_z_obectom);
                        comboBox_otvetstvenniy.Enabled = false;
                    }
                    else if (Convert.ToInt32(user["accsess_lvl"]) == 8)//Pustovit
                    {
                        tabControl2.TabPages.Remove(tabPage_dii_z_obectom);
                    }
                    else if (Convert.ToInt32(user["accsess_lvl"]) == 9)//operators
                    {
                        if (Convert.ToInt32(user["idUsers"]) != 35 || Convert.ToInt32(user["idUsers"]) != 36 || Convert.ToInt32(user["idUsers"]) != 37 || Convert.ToInt32(user["idUsers"]) != 16)//elit
                        {
                            tabControl2.TabPages.Remove(tabPage_dii_z_obectom);
                        }
                        tabControl2.TabPages.Remove(tabPage_dii_z_obectom);
                        tabControl2.TabPages.Remove(tabPage_edit_client);
                        tabControl2.TabPages.Remove(tabPage_rouming);
                        comboBox_otvetstvenniy.Enabled = false;
                    }
                    else
                    {
                        tabControl2.TabPages.Remove(tabPage_edit_client);
                        tabControl2.TabPages.Remove(tabPage_dii_z_obectom);
                        tabControl2.TabPages.Remove(tabPage_rouming);
                    }
                }
            }
        }

        private void close_start_object()
        {

        }

        private void load_vo()
        {
            string json = macros.WialonRequest("&svc=core/search_items&params={" +
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
                    break;
                }

            }

            //load VO1
            string VO1 = macros.sql_command("select Kontakti_idKontakti from btk.VO where Object_idObject = '" + id_db_obj + "' and VOcol_num_vo = '1' ORDER BY idVO DESC limit 1;");
            vars_form.transfer_vo1_vo_form = VO1;

            string VO_falilia = macros.sql_command("SELECT Kontakti_familia FROM btk.Kontakti where idKontakti = '" + VO1.ToString() + "';");
            string VO_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,''), ' (', COALESCE (Phonebookcol_messanger, ''), ')') FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + VO1.ToString() + "';");
            string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + VO1.ToString() + "';");
            string VO_phone2_Coment = macros.sql_command("SELECT Phonebook.Phonebookcol_messanger FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + VO1.ToString() + "';");
            if (VO_phone2 == "   -   -")
            { VO_phone2 = ""; }
            if (VO_falilia == "" & VO_imya_phone == "" || VO_imya_phone.Contains("Пусто"))
            {
                textBox_vo1.Text = "";
                vars_form.transfer_vo1_vo_form = "1";
            }
            else
            {
                textBox_vo1.Text = VO_falilia.ToUpper() + " " + VO_imya_phone + ", " + VO_phone2 + " (" + VO_phone2_Coment + ")"; ;
            }

            //load VO2
            string VO2 = macros.sql_command("select Kontakti_idKontakti from btk.VO where Object_idObject = '" + id_db_obj + "' and VOcol_num_vo = '2' ORDER BY idVO DESC limit 1;");
            vars_form.transfer_vo2_vo_form = VO2;
            string VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,''), ' (', COALESCE (Phonebookcol_messanger, ''), ')') FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + VO2.ToString() + "';");
            VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + VO2.ToString() + "';");
            VO_phone2_Coment = macros.sql_command("SELECT Phonebook.Phonebookcol_messanger FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + VO2.ToString() + "';");
            if (VO_phone2 == "   -   -")
            { VO_phone2 = ""; }
            if (VO_familia_imya_phone == "" || VO_familia_imya_phone == "Пусто Пусто , ")
            {
                textBox_vo2.Text = "";
                vars_form.transfer_vo2_vo_form = "1";
            }
            else
            {
                textBox_vo2.Text = VO_familia_imya_phone + ", " + VO_phone2 + " (" + VO_phone2_Coment + ")"; ;
            }


            //load VO3
            string VO3 = macros.sql_command("select Kontakti_idKontakti from btk.VO where Object_idObject = '" + id_db_obj + "' and VOcol_num_vo = '3' ORDER BY idVO DESC limit 1;");
            vars_form.transfer_vo3_vo_form = VO3;
            VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,''), ' (', COALESCE (Phonebookcol_messanger, ''), ')') FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + VO3.ToString() + "';");
            VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + VO3.ToString() + "';");
            VO_phone2_Coment = macros.sql_command("SELECT Phonebook.Phonebookcol_messanger FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + VO3.ToString() + "';");
            if (VO_phone2 == "   -   -")
            { VO_phone2 = ""; }
            if (VO_familia_imya_phone == "" || VO_familia_imya_phone == "Пусто Пусто , ")
            {
                textBox_vo3.Text = "";
                vars_form.transfer_vo3_vo_form = "1";
            }
            else
            {
                textBox_vo3.Text = VO_familia_imya_phone + ", " + VO_phone2 + " (" + VO_phone2_Coment + ")"; ;
            }

            //load VO4
            string VO4 = macros.sql_command("select Kontakti_idKontakti from btk.VO where Object_idObject = '" + id_db_obj + "' and VOcol_num_vo = '4' ORDER BY idVO DESC limit 1;");
            vars_form.transfer_vo4_vo_form = VO4;
            VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,''), ' (', COALESCE (Phonebookcol_messanger, ''), ')') FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + VO4.ToString() + "';");
            VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + VO4.ToString() + "';");
            VO_phone2_Coment = macros.sql_command("SELECT Phonebook.Phonebookcol_messanger FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + VO4.ToString() + "';");
            if (VO_phone2 == "   -   -")
            { VO_phone2 = ""; }
            if (VO_familia_imya_phone == "" || VO_familia_imya_phone == "Пусто Пусто , ")
            {
                textBox_vo4.Text = "";
                vars_form.transfer_vo4_vo_form = "1";
            }
            else
            {
                textBox_vo4.Text = VO_familia_imya_phone + ", " + VO_phone2 + " (" + VO_phone2_Coment + ")"; ;
            }

            //load VO5
            string VO5 = macros.sql_command("select Kontakti_idKontakti from btk.VO where Object_idObject = '" + id_db_obj + "' and VOcol_num_vo = '5' ORDER BY idVO DESC limit 1;");
            vars_form.transfer_vo5_vo_form = VO5;
            VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,''), ' (', COALESCE (Phonebookcol_messanger, ''), ')') FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + VO5.ToString() + "';");
            VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + VO5.ToString() + "';");
            VO_phone2_Coment = macros.sql_command("SELECT Phonebook.Phonebookcol_messanger FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + VO5.ToString() + "';");
            if (VO_phone2 == "   -   -")
            { VO_phone2 = ""; }
            if (VO_familia_imya_phone == "" || VO_familia_imya_phone == "Пусто Пусто , ")
            {
                textBox_vo5.Text = "";
                vars_form.transfer_vo5_vo_form = "1";
            }
            else
            {
                textBox_vo5.Text = VO_familia_imya_phone + ", " + VO_phone2 + " (" + VO_phone2_Coment + ")"; ;
            }






            ////load VO
            //DataTable VO = macros.GetData("select Kontakti_idKontakti, VOcol_num_vo from btk.VO where Object_idObject = '" + id_db_obj + "';");
            //if (VO.Rows.Count >= 1)
            //{
            //    foreach (DataRow row in VO.Rows)
            //    {
            //        if (row[1].ToString() == "1")
            //        {

            //            string VO_falilia = macros.sql_command("SELECT Kontakti_familia FROM btk.Kontakti where idKontakti = '" + row[0].ToString() + "';");
            //            string VO_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,'')) FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + row[0].ToString() + "';");
            //            string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + row[0].ToString() + "';");
            //            if (VO_phone2 == "   -   -")
            //            { VO_phone2 = ""; }
            //            if (VO_falilia == "" & VO_imya_phone == "")
            //            {
            //                textBox_vo1.Text = "";
            //            }
            //            else
            //            {
            //                textBox_vo1.Text = VO_falilia.ToUpper() + " " + VO_imya_phone + ", " + VO_phone2;
            //            }
            //        }
            //        if (row[1].ToString() == "2")
            //        {
            //            string VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,'')) FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + row[0].ToString() + "';");
            //            string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + row[0].ToString() + "';");
            //            if (VO_phone2 == "   -   -")
            //            { VO_phone2 = ""; }
            //            if (VO_familia_imya_phone == "" || VO_familia_imya_phone == "Пусто Пусто , ")
            //            {
            //                textBox_vo2.Text = "";
            //            }
            //            else
            //            {
            //                textBox_vo2.Text = VO_familia_imya_phone + ", " + VO_phone2;
            //            }
            //        }
            //        if (row[1].ToString() == "3")
            //        {
            //            string VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,'')) FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + row[0].ToString() + "';");
            //            string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + row[0].ToString() + "';");
            //            if (VO_phone2 == "   -   -")
            //            { VO_phone2 = ""; }
            //            if (VO_familia_imya_phone == "" || VO_familia_imya_phone == "Пусто Пусто , ")
            //            {
            //                textBox_vo3.Text = "";
            //            }
            //            else
            //            {
            //                textBox_vo3.Text = VO_familia_imya_phone + ", " + VO_phone2;
            //            }
            //        }


            //    }
            //    if (textBox_vo1.Text == "")
            //    {
            //        vars_form.transfer_vo1_vo_form = "1";
            //    }
            //    if (textBox_vo2.Text == "")
            //    {
            //        vars_form.transfer_vo2_vo_form = "1";
            //    }
            //    if (textBox_vo3.Text == "")
            //    {
            //        vars_form.transfer_vo3_vo_form = "1";
            //    }
            //}
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
                    //Last_message();
                    Thread.Sleep(10000);
                }
            });
            task.Start();
        }

        private void TreeView_zapolnyaem()//Получаем произвольные поля из Виалона и заполняем дерево
        {
            treeView_client_info.Nodes.Clear();

            TreeNode node1 = new TreeNode("Об'єкт охорони:");
            treeView_client_info.Nodes.Add(node1);
            treeView_client_info.Nodes[0].Expand();
            TreeNode node2 = new TreeNode("Підземні паркінги:");
            treeView_client_info.Nodes.Add(node2);
            treeView_client_info.Nodes[1].Expand();
            TreeNode node3 = new TreeNode("Знаходження в геозонах:");
            treeView_client_info.Nodes.Add(node3);
            treeView_client_info.Nodes[2].Expand();
            TreeNode node4 = new TreeNode("Загальна інформація:");
            treeView_client_info.Nodes.Add(node4);
            treeView_client_info.Nodes[3].Expand();
            TreeNode node5 = new TreeNode("Адміністративні поля:");
            treeView_client_info.Nodes.Add(node5);
            treeView_client_info.Nodes[4].Expand();
            TreeNode node6 = new TreeNode("Про автомобіль:");
            treeView_client_info.Nodes.Add(node6);
            treeView_client_info.Nodes[5].Expand();
            TreeNode node7 = new TreeNode("Знаходится в групах:");
            treeView_client_info.Nodes.Add(node7);

            string json = macros.WialonRequest("&svc=core/search_items&params={" +
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

            string json5 = macros.WialonRequest("&svc=core/search_items&params={" +
                                                    "\"spec\":{" +
                                                    "\"itemsType\":\"avl_unit_group\"," +
                                                    "\"propName\":\"sys_id\"," +
                                                    "\"propValueMask\":\"*\", " +
                                                    "\"sortType\":\"sys_name\"," +
                                                    "\"or_logic\":\"1\"}," +
                                                    "\"or_logic\":\"1\"," +
                                                    "\"force\":\"1\"," +
                                                    "\"flags\":\"1\"," +
                                                    "\"from\":\"0\"," +
                                                    "\"to\":\"0\"}");
            var m1 = JsonConvert.DeserializeObject<RootObject>(json5);
            foreach (var items in m1.items)
            {
                foreach (int units in items.u)
                {
                    if (units.ToString() == _search_id)
                    {

                        treeView_client_info.Nodes[6].Nodes.Add(new TreeNode(items.nm));
                    }
                }
            }




            if (json == "")
            {
                this.BackColor = Color.Red;
                return;
            }
            else
            {
                this.BackColor = Color.Empty;
                var m = JsonConvert.DeserializeObject<RootObject>(json);

                if (m.error != 1)
                {
                    if (m.items.Count == 0)
                    {

                    }
                    else
                    {
                        //Проверяем по WLID если объект в нашей базе, если нет - содаем запись
                        string ImeiDB = macros.sql_command("SELECT Object_imei FROM btk.Object where Object_id_wl = " + wl_id + ";");
                        if (ImeiDB == "")
                        {
                            macros.sql_command("" +
                              "insert into btk.Object ( " +
                              "Object_id_wl," +
                              "Object_imei, " +
                              "Object_name, " +
                              "Object_sim_no, " +
                              "products_idproducts, " +
                              "Simcard_idSimcard," +
                              "TS_info_idTS_info, " +
                              "TS_info_TS_brend_model_idTS_brend_model, " +
                              "Kontakti_idKontakti_serviceman, " +
                              "Dogovora_idDogovora, " +
                              "Users_idUsers, " +
                              "Objectcol_puk, " +
                              "Simcard_idSimcard1" +
                              ") values (" +
                              "'"+wl_id+"', " +
                              "'"+m.items[0].uid+"', " +
                              "'"+m.items[0].nm+"', " +
                              "'"+m.items[0].ph+"', " +
                              "'1'," +
                              "'1'," +
                              "'1'," +
                              "'1'," +
                              "'1'," +
                              "'1'," +
                              "'34'," +
                              "'1', " +
                              "'1');");

                            id_db_obj = macros.sql_command("SELECT MAX(idObject) FROM btk.Object;");

                        }



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



                    if (m.items.Count != 0)
                    {
                        foreach (var keyvalue in m.items[0].flds)
                        {
                            if (keyvalue.Value.n.Contains("Паркінг 1"))
                            {
                                Parking1_textBox.Text = keyvalue.Value.v.ToString();
                                Parking1ExistInWL = keyvalue.Key;
                            }
                            if (keyvalue.Value.n.Contains("Паркінг 2"))
                            {
                                Parking2_textBox.Text = keyvalue.Value.v.ToString();
                                Parking2ExistInWL = keyvalue.Key;
                            }
                            if (keyvalue.Value.n.Contains("Паркінг 3"))
                            {
                                Parking3_textBox.Text = keyvalue.Value.v.ToString();
                                Parking3ExistInWL = keyvalue.Key;
                            }
                            if (keyvalue.Value.n.Contains("Паркінг 4"))
                            {
                                Parking4_textBox.Text = keyvalue.Value.v.ToString();
                                Parking4ExistInWL = keyvalue.Key;
                            }
                            if (keyvalue.Value.n.Contains("Паркінг 5"))
                            {
                                Parking5_textBox.Text = keyvalue.Value.v.ToString();
                                Parking5ExistInWL = keyvalue.Key;
                            }


                            if (keyvalue.Value.n.Contains("0 УВАГА") & !keyvalue.Value.n.Contains("алгоритм"))
                            {
                                textBox_Uvaga.Text = keyvalue.Value.v.ToString();
                            }

                            if (keyvalue.Value.n.Contains("Кодов"))
                            {
                                treeView_client_info.Nodes[0].Nodes.Insert(0, (new TreeNode("Кодове слово" + ": " + keyvalue.Value.v.ToString())));
                            }

                            if (keyvalue.Value.n.Contains(" І Відп"))
                            {
                                if (keyvalue.Value.v != "")
                                {
                                    treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("ВО1" + ": " + keyvalue.Value.v.ToString()));

                                    //load VO1
                                    string VO1 = macros.sql_command("select Kontakti_idKontakti from btk.VO where Object_idObject = '" + id_db_obj + "' and VOcol_num_vo = '1' ORDER BY idVO DESC limit 1;");
                                    if (VO1 != "")
                                    {
                                        foreach (TreeNode child in treeView_client_info.Nodes[0].Nodes)
                                        {
                                            if (child.Text == "ВО1" + ": " + keyvalue.Value.v.ToString())
                                            {
                                                child.BackColor = Color.LightGreen;
                                            }
                                        }
                                    }

                                }
                            }
                            if (keyvalue.Value.n.Contains(" ІІ Відп"))
                            {
                                if (keyvalue.Value.v != "")
                                {

                                    treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("ВО2" + ": " + keyvalue.Value.v.ToString()));


                                    //load VO1
                                    string VO1 = macros.sql_command("select Kontakti_idKontakti from btk.VO where Object_idObject = '" + id_db_obj + "' and VOcol_num_vo = '2' ORDER BY idVO DESC limit 1;");
                                    if (VO1 != "")
                                    {
                                        foreach (TreeNode child in treeView_client_info.Nodes[0].Nodes)
                                        {
                                            if (child.Text == "ВО2" + ": " + keyvalue.Value.v.ToString())
                                            {
                                                child.BackColor = Color.LightGreen;
                                            }
                                        }
                                    }
                                }
                            }
                            if (keyvalue.Value.n.Contains(" ІІІ Відп"))
                            {
                                if (keyvalue.Value.v != "")
                                {

                                    treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("ВО3" + ": " + keyvalue.Value.v.ToString()));

                                    //load VO1
                                    string VO1 = macros.sql_command("select Kontakti_idKontakti from btk.VO where Object_idObject = '" + id_db_obj + "' and VOcol_num_vo = '3' ORDER BY idVO DESC limit 1;");
                                    if (VO1 != "")
                                    {
                                        foreach (TreeNode child in treeView_client_info.Nodes[0].Nodes)
                                        {
                                            if (child.Text == "ВО3" + ": " + keyvalue.Value.v.ToString())
                                            {
                                                child.BackColor = Color.LightGreen;
                                            }
                                        }
                                    }
                                }
                            }
                            if (keyvalue.Value.n.Contains(" IV Відп"))
                            {
                                if (keyvalue.Value.v != "")
                                {
                                    treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("ВО4" + ": " + keyvalue.Value.v.ToString()));

                                    //load VO1
                                    string VO1 = macros.sql_command("select Kontakti_idKontakti from btk.VO where Object_idObject = '" + id_db_obj + "' and VOcol_num_vo = '4' ORDER BY idVO DESC limit 1;");
                                    if (VO1 != "")
                                    {
                                        foreach (TreeNode child in treeView_client_info.Nodes[0].Nodes)
                                        {
                                            if (child.Text == "ВО4" + ": " + keyvalue.Value.v.ToString())
                                            {
                                                child.BackColor = Color.LightGreen;
                                            }
                                        }
                                    }
                                }
                                //Групируем ВО4 рядом со всеми ВО
                                foreach (TreeNode TreeViewNode in treeView_client_info.Nodes[0].Nodes)
                                {
                                    if (TreeViewNode.Text.Contains("ВО4"))
                                    {
                                        var lastNode = TreeViewNode;
                                        treeView_client_info.Nodes[0].Nodes.Remove(lastNode);
                                        treeView_client_info.Nodes[0].Nodes.Insert(4, lastNode);
                                    }
                                }
                            }
                            if (keyvalue.Value.n.Contains(" V Відп"))
                            {
                                if (keyvalue.Value.v != "")
                                {

                                    treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("ВО5" + ": " + keyvalue.Value.v.ToString()));

                                    //load VO1
                                    string VO1 = macros.sql_command("select Kontakti_idKontakti from btk.VO where Object_idObject = '" + id_db_obj + "' and VOcol_num_vo = '5' ORDER BY idVO DESC limit 1;");
                                    if (VO1 != "")
                                    {
                                        foreach (TreeNode child in treeView_client_info.Nodes[0].Nodes)
                                        {
                                            if (child.Text == "ВО5" + ": " + keyvalue.Value.v.ToString())
                                            {
                                                child.BackColor = Color.LightGreen;
                                            }
                                        }
                                    }
                                }
                                //Групируем ВО5 рядом со всеми ВО
                                foreach (TreeNode TreeViewNode in treeView_client_info.Nodes[0].Nodes)
                                {
                                    if (TreeViewNode.Text.Contains("ВО5"))
                                    {
                                        var lastNode = TreeViewNode;
                                        treeView_client_info.Nodes[0].Nodes.Remove(lastNode);
                                        treeView_client_info.Nodes[0].Nodes.Insert(5, lastNode);
                                    }
                                }
                            }
                            if (keyvalue.Value.n.Contains("ня сервісної"))
                            {

                                treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("Сервісна кнопка" + ": " + keyvalue.Value.v.ToString()));
                            }
                            if (keyvalue.Value.n.Contains("Штатні кн"))
                            {
                                treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("Кнопки PIN" + ": " + keyvalue.Value.v.ToString()));
                            }
                            if (keyvalue.Value.n.Contains("Дата активації"))
                            {
                                treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("Дата активації" + ": " + keyvalue.Value.v.ToString()));
                            }
                            if (keyvalue.Value.n.Contains("овки тривожної"))
                            {
                                treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("Тривожна кнопка" + ": " + keyvalue.Value.v.ToString()));
                            }

                            treeView_client_info.Nodes[3].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));

                            if (keyvalue.Value.n.Contains("Парк"))
                            {
                                if (keyvalue.Value.v != "")
                                {
                                    treeView_client_info.Nodes[1].Nodes.Add(new TreeNode("Паркінг" + ": " + keyvalue.Value.v.ToString()));
                                }
                            }
                        }
                        treeView_client_info.Nodes[0].Nodes.Insert(0, (new TreeNode("Назва об'єкту: " + m.items[0].nm.ToString())));
                        try
                        {
                            IMEI_object = m.items[0].uid.ToString();
                            treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("IMEI: " + m.items[0].uid.ToString()));
                            IMEI_object = m.items[0].uid.ToString();
                            treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("SIM: " + m.items[0].ph.ToString()));
                        }
                        catch
                        { }


                        try
                        {
                            foreach (var keyvalue in m.items[0].aflds)
                            {
                                treeView_client_info.Nodes[4].Nodes.Add(new TreeNode(keyvalue.Value.n.ToString() + ": " + keyvalue.Value.v.ToString()));
                                if (keyvalue.Value.n.Contains("PUK"))
                                {
                                    treeView_client_info.Nodes[0].Nodes.Add(new TreeNode("PUK код" + ": " + keyvalue.Value.v.ToString()));
                                }

                            }
                        }
                        catch { }

                        foreach (var keyvalue in m.items[0].pflds)
                        {
                            if (keyvalue.Value.n.Contains("vehicle_type"))
                            {
                                treeView_client_info.Nodes[5].Nodes.Add(new TreeNode("Тип Т/З: " + keyvalue.Value.v.ToString()));
                            }
                            else if (keyvalue.Value.n.Contains("vin"))
                            {
                                treeView_client_info.Nodes[5].Nodes.Add(new TreeNode("VIN: " + keyvalue.Value.v.ToString()));
                                VIN_object = keyvalue.Value.v.ToString();
                            }
                            else if (keyvalue.Value.n.Contains("registration_plate"))
                            {
                                treeView_client_info.Nodes[5].Nodes.Add(new TreeNode("Державний номер: " + keyvalue.Value.v.ToString()));
                            }
                            else if (keyvalue.Value.n.Contains("brand"))
                            {
                                treeView_client_info.Nodes[5].Nodes.Add(new TreeNode("Марка: " + keyvalue.Value.v.ToString()));
                            }
                            else if (keyvalue.Value.n.Contains("model"))
                            {
                                treeView_client_info.Nodes[5].Nodes.Add(new TreeNode("Модель: " + keyvalue.Value.v.ToString()));
                            }
                            else if (keyvalue.Value.n.Contains("year"))
                            {
                                treeView_client_info.Nodes[5].Nodes.Add(new TreeNode("Рік випуску: " + keyvalue.Value.v.ToString()));
                            }
                            else if (keyvalue.Value.n.Contains("color"))
                            {
                                treeView_client_info.Nodes[5].Nodes.Add(new TreeNode("Колір: " + keyvalue.Value.v.ToString()));
                            }
                        }
                    }
                }
                treeView_client_info.Nodes[0].Expand();
                treeView_client_info.Nodes[1].Expand();
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

            macros.sql_command("update btk.notification set time_stamp='" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "', otvetstvenniy = '"+comboBox_otvetstvenniy.GetItemText(comboBox_otvetstvenniy.SelectedItem)+"' where idnotification = '" + _id_notif + "';");

            mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог

            textBox_otrabotka_trevogi.Text = "";

            if (comboBox_status_trevogi.SelectedItem.ToString() == "909" & _id_status != "909")//если изменяем статус с Обробляется на 909  - отправляем меил со всейхронологией обработки тревоги
            {
                string recipient = "<" + vars_form.user_login_email + ">," + "<d.yacenko@venbest.com.ua>,<a.oberemchuk@venbest.com.ua>,<v.bogoley@venbest.com.ua>";
                send_email(recipient);
            }

            if (comboBox_status_trevogi.SelectedItem.ToString() == "808" & _id_status != "808")//если изменяем статус с Обробляется на 808  - отправляем меил со всейхронологией обработки тревоги
            {
                string recipient = "<" + vars_form.user_login_email + ">," + "<o.pustovit@venbest.com.ua>,<d.lenik@venbest.com.ua>,<mc@venbest.com.ua>,<e.remekh@venbest.com.ua>";
                send_email(recipient);
            }
            if (comboBox_status_trevogi.SelectedItem.ToString() == "Продажи" & _id_status != "Продажи")//если изменяем статус с Обробляется на 808  - отправляем меил со всейхронологией обработки тревоги
            {
                string recipient = "<" + vars_form.user_login_email + ">," + "<a.lozinskiy@venbest.com.ua>, <y.kravchenko@venbest.com.ua>";
                send_email(recipient);
            }
            if (comboBox_status_trevogi.SelectedItem.ToString() == "Дилеры" & _id_status != "Дилеры")//если изменяем статус с Обробляется на 808  - отправляем меил со всейхронологией обработки тревоги
            {
                string recipient = "<" + vars_form.user_login_email + ">," + "<e.danilchenko@venbest.com.ua>,<a.andreasyan@venbest.com.ua>,<s.gregul@venbest.com.ua>,<o.mychka@venbest.com.ua>";
                send_email(recipient);
            }

            if (comboBox_status_trevogi.SelectedItem.ToString() == "110" & _id_status != "110")//если изменяем статус с Обробляется на 110  - отправляем меил со всейхронологией обработки тревоги
            {
                string recipient = "<" + vars_form.user_login_email + ">," + "<v.konoval@venbest.com.ua>,<v.chykhman@venbest.com.ua>";
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

            DateClosed = DateTime.Now;
            StatusClosed = comboBox_status_trevogi.Text;
            TimeSpan timeSpan = DateClosed - DateOpened;
            string t = $"{timeSpan.Hours}:{timeSpan.Minutes}:{timeSpan.Seconds}";


            // get id_object DB where id_wl
            string sql1 = string.Format("" +
                "insert into btk.ProcessingTime " +
                "(" +
                "DateOpened, " +
                "DateClosed, " +
                "notification_idnotification, " +
                "StatusOpened, " +
                "StatusClosed, " +
                "Delta, " +
                "Object_idObject," +
                "Users_idUsers" +
                ") values (" +
                "'" + Convert.ToDateTime(DateOpened).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                "'" + Convert.ToDateTime(DateClosed).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                "'"+ _id_notif + "'," +
                "'"+ StatusOpened + "'," +
                "'"+ StatusClosed + "'," +
                "'" + $"{timeSpan.Hours}:{timeSpan.Minutes}:{timeSpan.Seconds}" + "'," +
                "'" + id_db_obj + "'," +
                "'" + _user_login_id + "'" +
                ");");
            string idobject = macros.sql_command(sql1);

        }//если форма закрывается оператором - снимаем блокировку одновременного открытия


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
                mail.Body = "УВАГА: " + textBox_Uvaga.Text + System.Environment.NewLine + htmlMessageBody(dataGridView_hronologija_trivog).ToString();

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
            string json2 = macros.WialonRequest("&svc=core/search_items&params={" +
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

            //string json9 = macros.wialon_request_new("&svc=core/search_items&params={" +
            //                                         "\"spec\":{" +
            //                                         "\"itemsType\":\"avl_resource\"," +
            //                                         "\"propName\":\"sys_id\"," +
            //                                         "\"propValueMask\":\"4296\", " +
            //                                         "\"sortType\":\"sys_name\"," +
            //                                         "\"or_logic\":\"1\"}," +
            //                                         "\"or_logic\":\"1\"," +
            //                                         "\"force\":\"1\"," +
            //                                         "\"flags\":\"-1\"," +
            //                                         "\"from\":\"0\"," +
            //                                         "\"to\":\"500\"}");//получаем resource


            var m = JsonConvert.DeserializeObject<RootObject>(json2);

            if (m.items.Count <= 0)
            {
                MessageBox.Show("Для справки: Объект удален, информация не полная");
                return;
            }

            if (m.items[0].lmsg != null)
            {
                label_lmst.Text = macros.UnixTimeStampToDateTime(m.items[0].lmsg.t).ToString();
            }

            if (m.items.Count != 0)
            {
                var lat = "";
                var lon = "";
                if (m.items[0].pos != null)
                {
                    lat = m.items[0].pos["y"];
                    lon = m.items[0].pos["x"];
                }

                if (m.items[0].pos != null)
                {
                    string json3;
                    if (checkBox_geozone_all.Checked is true)
                    {
                        json3 = macros.WialonRequest("&svc=resource/get_zones_by_point&params={" +
                                     "\"spec\":{" +
                                     "\"zoneId\":{" +
                                     "\"28\":[],\"" +
                                     "4296\":[],\"" +
                                     "241\":[] }" + "," +
                                     "\"lat\":" + lat + "," +
                                     "\"lon\":" + lon + "}}");//получаем id геозон в которых находится объект, ресурс res_service id=28, 241=operator_messages, 4296=Operator_user
                    }
                    else
                    {
                        json3 = macros.WialonRequest("&svc=resource/get_zones_by_point&params={" +
                                     "\"spec\":{" +
                                     "\"zoneId\":{" +
                                     "\"4296\":[] }" + "," +
                                     "\"lat\":" + lat + "," +
                                     "\"lon\":" + lon + "}}");//получаем id геозон в которых находится объект, ресурс res_service id=28, 241=operator_messages, 4296=Operator_user
                    }
                    

                    if (!json3.Contains("error"))
                    {
                        var geozone = JsonConvert.DeserializeObject<Dictionary<int, List<dynamic>>>(json3);
                        label_geozones.Text = "Знаходиться в геозонах:";
                        treeView_client_info.Nodes[2].Nodes.Clear();
                        foreach (var key in geozone)
                        {
                            foreach (long value in key.Value)
                            {
                                string json4 = macros.WialonRequest("&svc=resource/get_zone_data&params={" +
                                                                            "\"itemId\":\""+ key.Key +"\"," +
                                                                            "\"col\":[" + value + "]," +
                                                                            "\"flags\":\"28\"}");//

                                var x = JsonConvert.DeserializeObject<List<Value_>>(json4);

                                if (label_geozones.Text == "Знаходиться в геозонах:")
                                { label_geozones.Text = "Знаходиться в геозонах:" + "\r\n" + x[0].n.ToString(); }
                                else
                                { label_geozones.Text += "\r\n" + x[0].n.ToString(); }
                                if (key.Key== 4296)
                                { 
                                    treeView_client_info.Nodes[2].Nodes.Add(new TreeNode(x[0].n.ToString())); 
                                }
                            }
                        }
                        treeView_client_info.Nodes[2].Expand();
                    }
                }//получаем текущие значения датчиков, нахождение в ГЕО-зонах объекта
            }
        }

        


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
            
            string json = macros.WialonRequest("&svc=core/search_items&params={" +
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


            string json2 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                     "\"itemId\":\"" + _search_id + "\"," + 
                                                     "\"id\":\"1\"," + 
                                                     "\"callMode\":\"update\"," +
                                                     "\"n\":\"" + m.items[0].flds[1].n + "\"," +
                                                     "\"v\":\"" + textBox_Uvaga.Text.Replace("\"", "%5C%22") + "\"}");//получаем датчики объекта

            string _text = "Поле Увага змінено: "+ textBox_Uvaga.Text.Replace("\"", "%5C%22") + ".\n" + "Змінив оператор: " + vars_form.user_login_name + "";
            int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
            int police = checkBox_vizov_police.Checked ? 1 : 0;
            macros.sql_command("insert into btk.alarm_ack(" +
                                "alarm_text, " +
                                "notification_idnotification, " +
                                "Users_chenge, time_start_ack, " +
                                "current_status_alarm, " +
                                "vizov_police, " +
                                "vizov_gmp) " +
                                "values('" + _text + "', " +
                                "'" + _id_notif + "'," +
                                "'" + _user_login_id + "', " +
                                "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                " 'Учетки', '" +
                                "" + police + "', " +
                                "'" + gmr + "');");

            mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог

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


        


        private void Arhiv_object()
        {
            //DataSet dt = new DataSet();
            //dataGridView_trivogi_objecta.AutoGenerateColumns = false;
            //dataGridView_trivogi_objecta.DataSource = macros.GetData("SELECT idnotification, type_alarm, msg_time, Status, last_location FROM btk.notification where unit_id = " + _search_id + " and group_alarm is null");
            //if (dataGridView_trivogi_objecta.InvokeRequired)
            //    dataGridView_trivogi_objecta.Invoke(new Action(() => dataGridView_trivogi_objecta.DataSource = macros.GetData("SELECT idnotification, type_alarm, msg_time, Status, last_location FROM btk.notification where unit_id = " + _search_id + " and group_alarm is null")));
            //else
            //    dataGridView_trivogi_objecta.DataSource = macros.GetData("SELECT idnotification, type_alarm, msg_time, Status, last_location FROM btk.notification where unit_id = " + _search_id + " and group_alarm is null");

            //  dataGridView_trivogi_objecta.DataSource = await macros.GetData("SELECT idnotification, type_alarm, msg_time, Status, last_location FROM btk.notification where unit_id = " + _search_id + " and group_alarm is null");

            Task.Run(() =>
            {
                dataGridView_trivogi_objecta.AutoGenerateColumns = false;
                DataTable dt = macros.GetData("SELECT idnotification, type_alarm, msg_time, Status, last_location FROM btk.notification where unit_id = " + _search_id + " and group_alarm is null order by idnotification desc");

                if (dataGridView_trivogi_objecta.InvokeRequired)
                        dataGridView_trivogi_objecta.Invoke(new Action(() => { dataGridView_trivogi_objecta.DataSource = dt; }));
                    dataGridView_trivogi_objecta.DataSource = dt;
            });
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
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                var mySelectedNode = treeView_client_info.GetNodeAt(e.X, e.Y);
                treeView_client_info.LabelEdit = true;
                mySelectedNode.BeginEdit();
            }
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

        private void GetUserOtvetstvenyi()
        {
            // Строим список тип кузова авто - Имя=кузов, Значение=айди
            DataTable dt = macros.GetData("SELECT user_id_wl, username FROM btk.Users where State = '1' and (accsess_lvl = '8' or accsess_lvl = '9') order by accsess_lvl;");
            this.comboBox_otvetstvenniy.DataSource = dt;
            this.comboBox_otvetstvenniy.DisplayMember = "username";
            this.comboBox_otvetstvenniy.ValueMember = "user_id_wl";

            DataRow row = dt.NewRow();
            row[0] = 0;
            row[1] = "";
            dt.Rows.InsertAt(row, 0);


            string t = macros.sql_command("SELECT otvetstvenniy FROM btk.notification where idnotification = '" + _id_notif + "';");
            comboBox_otvetstvenniy.SelectedIndex = comboBox_otvetstvenniy.FindStringExact(t);
        }

        private void detail_Load(object sender, EventArgs e)
        {
            DateOpened = DateTime.Now;
            StatusOpened = comboBox_status_trevogi.Text;

            



            //if opened start detail - automaticly chenge status alarm in combobox = Обробляется
            comboBox_status_trevogi.SelectedIndex = comboBox_status_trevogi.FindStringExact(_id_status);
        }

        private void button_add_vo1_Click(object sender, EventArgs e)
        {
            vars_form.kontakts_opened_from = 1;
            Kontacts Kontacts_form = new Kontacts();
            Kontacts_form.Activated += new EventHandler(form_vo_add_activated);
            Kontacts_form.FormClosed += new FormClosedEventHandler(form_vo_add_deactivated);
            Kontacts_form.Show();
        }

        private void button_add_vo2_Click(object sender, EventArgs e)
        {
            vars_form.kontakts_opened_from = 2;
            Kontacts Kontacts_form = new Kontacts();
            Kontacts_form.Activated += new EventHandler(form_vo_add_activated);
            Kontacts_form.FormClosed += new FormClosedEventHandler(form_vo_add_deactivated);
            Kontacts_form.Show();
        }

        private void button_add_vo3_Click(object sender, EventArgs e)
        {
            vars_form.kontakts_opened_from = 3;
            Kontacts Kontacts_form = new Kontacts();
            Kontacts_form.Activated += new EventHandler(form_vo_add_activated);
            Kontacts_form.FormClosed += new FormClosedEventHandler(form_vo_add_deactivated);
            Kontacts_form.Show();
        }

        private void button_add_vo4_Click(object sender, EventArgs e)
        {
            vars_form.kontakts_opened_from = 4;
            Kontacts Kontacts_form = new Kontacts();
            Kontacts_form.Activated += new EventHandler(form_vo_add_activated);
            Kontacts_form.FormClosed += new FormClosedEventHandler(form_vo_add_deactivated);
            Kontacts_form.Show();
        }

        private void button_add_vo5_Click(object sender, EventArgs e)
        {
            vars_form.kontakts_opened_from = 5;
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
            if (vars_form.kontakts_opened_from == 1)//VO1 1 familia make upper case
            {
                string VO_falilia = macros.sql_command("SELECT Kontakti_familia FROM btk.Kontakti where idKontakti = '" + vars_form.transfer_vo1_vo_form + "';");
                string VO_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,''), ' (', COALESCE (Phonebookcol_messanger, ''), ')') FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + vars_form.transfer_vo1_vo_form + "';");
                string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + vars_form.transfer_vo1_vo_form + "';");
                string VO_phone2_Coment = macros.sql_command("SELECT Phonebook.Phonebookcol_messanger FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + vars_form.transfer_vo1_vo_form + "';");
                if (VO_phone2 == "   -   -")
                { VO_phone2 = ""; }
                if (VO_falilia == "" & VO_imya_phone =="" || VO_imya_phone.Contains("Пусто"))
                {
                    textBox_vo1.Text = "";
                    vars_form.transfer_vo1_vo_form = "1";
                }
                else
                {
                    textBox_vo1.Text = VO_falilia.ToUpper() + " " + VO_imya_phone + ", " + VO_phone2 + " (" + VO_phone2_Coment + ")";
                }

            }
            if (vars_form.kontakts_opened_from == 2)
            {
                string VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,''), ' (', COALESCE (Phonebookcol_messanger, ''), ')') FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + vars_form.transfer_vo2_vo_form + "';");
                string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + vars_form.transfer_vo2_vo_form + "';");
                string VO_phone2_Coment = macros.sql_command("SELECT Phonebook.Phonebookcol_messanger FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + vars_form.transfer_vo2_vo_form + "';");
                if (VO_phone2 == "   -   -")
                { VO_phone2 = ""; }
                if (VO_familia_imya_phone == "" || VO_familia_imya_phone == "Пусто Пусто , ")
                {
                    textBox_vo2.Text = "";
                    vars_form.transfer_vo2_vo_form = "1";
                }
                else
                {
                    textBox_vo2.Text = VO_familia_imya_phone + ", " + VO_phone2 + " (" + VO_phone2_Coment + ")";
                }

            }
            if (vars_form.kontakts_opened_from == 3)
            {
                string VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,''), ' (', COALESCE (Phonebookcol_messanger, ''), ')') FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + vars_form.transfer_vo3_vo_form + "';");
                string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + vars_form.transfer_vo3_vo_form + "';");
                string VO_phone2_Coment = macros.sql_command("SELECT Phonebook.Phonebookcol_messanger FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + vars_form.transfer_vo3_vo_form + "';");
                if (VO_phone2 == "   -   -")
                { VO_phone2 = ""; }
                if (VO_familia_imya_phone == "" || VO_familia_imya_phone == "Пусто Пусто , ")
                {
                    textBox_vo3.Text = "";
                    vars_form.transfer_vo3_vo_form = "1";
                }
                else
                {
                    textBox_vo3.Text = VO_familia_imya_phone + ", " + VO_phone2 + " (" + VO_phone2_Coment + ")";
                }
            }
            if (vars_form.kontakts_opened_from == 4)
            {
                string VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,''), ' (', COALESCE (Phonebookcol_messanger, ''), ')') FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + vars_form.transfer_vo4_vo_form + "';");
                string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + vars_form.transfer_vo4_vo_form + "';");
                string VO_phone2_Coment = macros.sql_command("SELECT Phonebook.Phonebookcol_messanger FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + vars_form.transfer_vo4_vo_form + "';");
                if (VO_phone2 == "   -   -")
                { VO_phone2 = ""; }
                if (VO_familia_imya_phone == "" || VO_familia_imya_phone == "Пусто Пусто , ")
                {
                    textBox_vo4.Text = "";
                    vars_form.transfer_vo4_vo_form = "1";
                }
                else
                {
                    textBox_vo4.Text = VO_familia_imya_phone + ", " + VO_phone2 + " (" + VO_phone2_Coment + ")";
                }
            }
            if (vars_form.kontakts_opened_from == 5)
            {
                string VO_familia_imya_phone = macros.sql_command("SELECT concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,', ',  COALESCE (Phonebook.Phonebookcol_phone,''), ' (', COALESCE (Phonebookcol_messanger, ''), ')') FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + vars_form.transfer_vo5_vo_form + "';");
                string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + vars_form.transfer_vo5_vo_form + "';");
                string VO_phone2_Coment = macros.sql_command("SELECT Phonebook.Phonebookcol_messanger FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + vars_form.transfer_vo5_vo_form + "';");
                if (VO_phone2 == "   -   -")
                { VO_phone2 = ""; }
                if (VO_familia_imya_phone == "" || VO_familia_imya_phone == "Пусто Пусто , ")
                {
                    textBox_vo5.Text = "";
                    vars_form.transfer_vo5_vo_form = "1";
                }
                else
                {
                    textBox_vo5.Text = VO_familia_imya_phone + ", " + VO_phone2 + " (" + VO_phone2_Coment + ")";
                }
            }
            this.Visible = true;// разблокируем окно контрагентов кактолько закрыто окно добавления контрагента
            vars_form.kontakts_opened_from = 0;
        }

        private void textBox_vo1_DoubleClick(object sender, EventArgs e)
        {
            textBox_vo1.Text = "";
            vars_form.transfer_vo1_vo_form = "1";
        }

        private void textBox_vo2_DoubleClick(object sender, EventArgs e)
        {
            textBox_vo2.Text = "";
            vars_form.transfer_vo2_vo_form = "1";
        }

        private void textBox_vo3_DoubleClick(object sender, EventArgs e)
        {
            textBox_vo3.Text = "";
            vars_form.transfer_vo3_vo_form = "1";
        }
        private void textBox_vo4_DoubleClick(object sender, EventArgs e)
        {
            textBox_vo4.Text = "";
            vars_form.transfer_vo4_vo_form = "1";
        }

        private void textBox_vo5_DoubleClick(object sender, EventArgs e)
        {
            textBox_vo5.Text = "";
            vars_form.transfer_vo5_vo_form = "1";
        }

        private void button_vo_save_Click(object sender, EventArgs e)
        {
            //Загружаем произвольные поля объекта
            string json = macros.WialonRequest("&svc=core/search_items&params={" +
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

            //chek if exist costom feild in WL VO4, VO5
            int vo4_exist = 0;
            int vo5_exist = 0;
            foreach (var keyvalue in object_data.items[0].flds)
            {
                if (keyvalue.Value.n.Contains("2.4 ІV Від"))
                {
                    vo4_exist = 1;
                }
                if (keyvalue.Value.n.Contains("2.5 V Від"))
                {
                    vo5_exist = 1;
                }
            }
            if (vo4_exist == 0)
            { macros.create_custom_field_wl(Convert.ToInt32(wl_id), "2.4 ІV Відповідальна особа", ""); }
            if (vo5_exist == 0)
            { macros.create_custom_field_wl(Convert.ToInt32(wl_id), "2.5 V Відповідальна особа", ""); }

            //Загружаем заново произвольные поля объекта
            json = macros.WialonRequest("&svc=core/search_items&params={" +
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

            object_data = JsonConvert.DeserializeObject<RootObject>(json);


            //update costom feild in WL, Upatate VO in WL and DB
            foreach (var keyvalue in object_data.items[0].flds)
            {
                switch (keyvalue.Value.n)
                {
                    //Chenge feild Кодове слово
                    case string a when a.Contains("Кодове "):
                        string json2 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                                "\"itemId\":\"" + wl_id + "\"," +
                                                                "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                                "\"callMode\":\"update\"," +
                                                                "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                                "\"v\":\"" + kodove_slovo_textBox.Text + "\"}");
                        break;

                    //Chenge feild ВО1
                    case string a when a.Contains("2.1 І Від") & keyvalue.Value.v != textBox_vo1.Text & textBox_vo1.Text != "1":
                        macros.sql_command("insert into btk.VO (Object_idObject,Kontakti_idKontakti,VOcol_num_vo,VOcol_date_add,Users_idUsers) values('" + id_db_obj + "','" + vars_form.transfer_vo1_vo_form + "','1','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','" + vars_form.user_login_id + "');");
                        string json3 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                         "\"itemId\":\"" + wl_id + "\"," +
                                                         "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                         "\"callMode\":\"update\"," +
                                                         "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                         "\"v\":\"" + textBox_vo1.Text.Replace("\"", "%5C%22") + "\"}");
                        break;

                    //Chenge feild ВО2
                    case string a when a.Contains("2.2 ІІ Від") & keyvalue.Value.v != textBox_vo2.Text & textBox_vo2.Text != "1":
                        macros.sql_command("insert into btk.VO (Object_idObject,Kontakti_idKontakti,VOcol_num_vo,VOcol_date_add,Users_idUsers) values('" + id_db_obj + "','" + vars_form.transfer_vo2_vo_form + "','2','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','" + vars_form.user_login_id + "');");
                        string json4 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                         "\"itemId\":\"" + wl_id + "\"," +
                                                         "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                         "\"callMode\":\"update\"," +
                                                         "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                         "\"v\":\"" + textBox_vo2.Text.Replace("\"", "%5C%22") + "\"}");
                        break;

                    //Chenge feild ВО3
                    case string a when a.Contains("2.3 ІІІ Від") & keyvalue.Value.v != textBox_vo3.Text & textBox_vo3.Text != "1":
                        macros.sql_command("insert into btk.VO (Object_idObject,Kontakti_idKontakti,VOcol_num_vo,VOcol_date_add,Users_idUsers) values('" + id_db_obj + "','" + vars_form.transfer_vo3_vo_form + "','3','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','" + vars_form.user_login_id + "');");
                        string json5 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                         "\"itemId\":\"" + wl_id + "\"," +
                                                         "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                         "\"callMode\":\"update\"," +
                                                         "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                         "\"v\":\"" + textBox_vo3.Text.Replace("\"", "%5C%22") + "\"}");
                        break;

                    //Chenge feild ВО4
                    case string a when a.Contains("2.4 ІV Від") & keyvalue.Value.v != textBox_vo4.Text & textBox_vo4.Text != "1":
                        macros.sql_command("insert into btk.VO (Object_idObject,Kontakti_idKontakti,VOcol_num_vo,VOcol_date_add,Users_idUsers) values('" + id_db_obj + "','" + vars_form.transfer_vo4_vo_form + "','4','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','" + vars_form.user_login_id + "');");
                        string json6 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                         "\"itemId\":\"" + wl_id + "\"," +
                                                         "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                         "\"callMode\":\"update\"," +
                                                         "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                         "\"v\":\"" + textBox_vo4.Text.Replace("\"", "%5C%22") + "\"}");
                        break;

                    //Chenge feild ВО5
                    case string a when a.Contains("2.5 V Від") & keyvalue.Value.v != textBox_vo5.Text & textBox_vo5.Text != "1":
                        macros.sql_command("insert into btk.VO (Object_idObject,Kontakti_idKontakti,VOcol_num_vo,VOcol_date_add,Users_idUsers) values('" + id_db_obj + "','" + vars_form.transfer_vo5_vo_form + "','5','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','" + vars_form.user_login_id + "');");
                        string json7 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                         "\"itemId\":\"" + wl_id + "\"," +
                                                         "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                         "\"callMode\":\"update\"," +
                                                         "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                         "\"v\":\"" + textBox_vo5.Text.Replace("\"", "%5C%22") + "\"}");
                        break;                   
                }
            }
            TreeView_zapolnyaem();
            MessageBox.Show("Збережено");
            // Диалог закрываем заявку или нет при завершении работы с учетными записями
            on_end_account_job("Внесено зміни:\nВО1:" + textBox_vo1.Text + "\nВО2: " + textBox_vo2.Text + "\nВО3: " + textBox_vo3.Text + "\nВО4: " + textBox_vo4.Text + "\nВО5: " + textBox_vo5.Text + "\nКодове слово: " + kodove_slovo_textBox.Text);
        }



        private void email_textBoxTextChanged(object sender, EventArgs e)
        {
            if (email_textBox.Text != "")
            {
                string json = macros.WialonRequest("&svc=core/search_items&params={" +
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


                string json = macros.WialonRequest("&svc=core/check_accessors&params={" +
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


                            string json1 = macros.WialonRequest("&svc=core/search_items&params={" +
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
                            string json3 = macros.WialonRequest("&svc=user/get_items_access&params={" +
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

                            string json4 = macros.WialonRequest("&svc=core/search_items&params={" +
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
            const string valid = "abcdefghkmnpqrstuvwxyz123456789";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        private void on_end_account_job(string _text)
        {
            DialogResult dialogResult = MessageBox.Show("Закрити заявку?", "Закрити?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                int police = checkBox_vizov_police.Checked ? 1 : 0;
                macros.sql_command("insert into btk.alarm_ack(" +
                                    "alarm_text, " +
                                    "notification_idnotification, " +
                                    "Users_chenge, time_start_ack, " +
                                    "current_status_alarm, " +
                                    "vizov_police, " +
                                    "vizov_gmp) " +
                                    "values('" + _text + "', " +
                                    "'" + _id_notif + "'," +
                                    "'" + _user_login_id + "', " +
                                    "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                    " 'Закрито', '" +
                                    "" + police + "', " +
                                    "'" + gmr + "'); UPDATE btk.notification SET Status = 'Закрито', time_stamp = now(), Users_idUsers='" + _user_login_id + "' WHERE idnotification = '" + _id_notif + "'OR group_alarm = '" + _id_notif + "'; ");
                this.Close();
            }
            else 
            {
                int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                int police = checkBox_vizov_police.Checked ? 1 : 0;
                macros.sql_command("insert into btk.alarm_ack(" +
                                    "alarm_text, " +
                                    "notification_idnotification, " +
                                    "Users_chenge, time_start_ack, " +
                                    "current_status_alarm, " +
                                    "vizov_police, " +
                                    "vizov_gmp) " +
                                    "values('" + _text + "', " +
                                    "'" + _id_notif + "'," +
                                    "'" + _user_login_id + "', " +
                                    "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                    " '"+ comboBox_status_trevogi.SelectedItem.ToString() +"', '" +
                                    "" + police + "', " +
                                    "'" + gmr + "'); UPDATE btk.notification SET Status = '"+ comboBox_status_trevogi.SelectedItem.ToString() + "', time_stamp = now(), Users_idUsers='" + _user_login_id + "' WHERE idnotification = '" + _id_notif + "'OR group_alarm = '" + _id_notif + "'; ");
                mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог
            }
        }// Диалог закрываем заявку или нет при завершении работы с учетными записями

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
                    string created_user_answer = macros.WialonRequest("&svc=core/create_user&params={" +
                                                            "\"creatorId\":\"" + vars_form.wl_user_id + "\"," +
                                                            "\"name\":\"" + email_textBox.Text + "\"," +
                                                            "\"password\":\"" + pass + "\"," +
                                                            "\"dataFlags\":\"1\"}");
                    var created_user_data = JsonConvert.DeserializeObject<RootObject>(created_user_answer);

                    // create resours name: newuser
                    string created_resource_answer = macros.WialonRequest("&svc=core/create_resource&params={" +
                                                            "\"creatorId\":\"" + created_user_data.item.id + "\"," +
                                                            "\"name\":\"" + email_textBox.Text + "\"," +
                                                            "\"dataFlags\":\"1\"," +
                                                            "\"skipCreatorCheck\":\"1\"}");
                    var created_resource_data = JsonConvert.DeserializeObject<RootObject>(created_resource_answer);

                    // create resours name: newuser_user
                    string created_resource_user_answer = macros.WialonRequest("&svc=core/create_resource&params={" +
                                                             "\"creatorId\":\"" + created_user_data.item.id + "\"," +
                                                             "\"name\":\"" + email_textBox.Text + "_user" + "\"," +
                                                             "\"dataFlags\":\"1\"," +
                                                             "\"skipCreatorCheck\":\"1\"}");
                    var created_resource_user_data = JsonConvert.DeserializeObject<RootObject>(created_resource_user_answer);


                    //Доступ на объект for nwe user
                    string set_right_object_answer = macros.WialonRequest("&svc=user/update_item_access&params={" +
                                                            "\"userId\":\"" + created_user_data.item.id + "\"," +
                                                            "\"itemId\":\"" + wl_id + "\"," +
                                                            "\"accessMask\":\"550611455877\"}");
                    var set_right_object = JsonConvert.DeserializeObject<RootObject>(set_right_object_answer);

                    DataTable users = macros.GetData("SELECT idUsers, user_id_wl FROM btk.Users where (accsess_lvl = '1' or accsess_lvl = '5' or accsess_lvl = '8' or accsess_lvl = '9') and State != '0';");

                    List<DataRow> woruser_list = users.AsEnumerable().ToList();
                    foreach (DataRow workuser in woruser_list)
                    {
                        if (workuser[1].ToString() != vars_form.wl_user_id.ToString() & workuser[1].ToString() != null & workuser[1].ToString() != "" & workuser[1].ToString() != "0")
                        {

                            //set full Accsess client account for workuser 
                            string set_right_user_suport_answer = macros.WialonRequest("&svc=user/update_item_access&params={" +
                                                                     "\"userId\":\"" + workuser[1].ToString() + "\"," +
                                                                     "\"itemId\":\"" + created_user_data.item.id + "\"," +
                                                                     "\"accessMask\":\"-1\"}");
                            var set_right_user_suport_ = JsonConvert.DeserializeObject<RootObject>(set_right_user_suport_answer);

                            //set full Accsess client resourse "username" for workuser 
                            string set_right_resource_support_answer = macros.WialonRequest("&svc=user/update_item_access&params={" +
                                                                     "\"userId\":\"" + workuser[1].ToString() + "\"," +
                                                                     "\"itemId\":\"" + created_resource_data.item.id + "\"," +
                                                                     "\"accessMask\":\"-1\"}");
                            var set_right_resourc_support = JsonConvert.DeserializeObject<RootObject>(set_right_resource_support_answer);

                            //set full Accsess client resourse "username.._user" for workuser 
                            string set_right_resource_user_support_answer = macros.WialonRequest("&svc=user/update_item_access&params={" +
                                                                     "\"userId\":\"" + workuser[1].ToString() + "\"," +
                                                                     "\"itemId\":\"" + created_resource_user_data.item.id + "\"," +
                                                                     "\"accessMask\":\"-1\"}");
                            var set_right_resource_user_support = JsonConvert.DeserializeObject<RootObject>(set_right_resource_user_support_answer);
                        }
                        else
                        {
                        }

                    }

                    //Доступ на ресурс: username
                    string set_right_resource_answer = macros.WialonRequest("&svc=user/update_item_access&params={" +
                                                             "\"userId\":\"" + created_user_data.item.id + "\"," +
                                                             "\"itemId\":\"" + created_resource_data.item.id + "\"," +
                                                             "\"accessMask\":\"4648339329\"}");
                    var set_right_resource = JsonConvert.DeserializeObject<RootObject>(set_right_resource_answer);

                    //Доступ на ресурс: username_user
                    string set_right_resource_user_answer = macros.WialonRequest("&svc=user/update_item_access&params={" +
                                                             "\"userId\":\"" + created_user_data.item.id + "\"," +
                                                             "\"itemId\":\"" + created_resource_user_data.item.id + "\"," +
                                                             "\"accessMask\":\"52785134440321\"}");
                    var set_right_resource_user = JsonConvert.DeserializeObject<RootObject>(set_right_resource_user_answer);



                    //get product id from id object
                    string product_id = get_product_from_id_object(wl_id);

                    //create notif depends product id

                    if (product_id == "10" || product_id == "11" || product_id == "13" || product_id == "14" || product_id == "18" || product_id == "19" || product_id == "20" || product_id == "21")//CNTP_910, CNTK_910
                    {
                        create_notif_910(email_textBox.Text, created_user_data.item.id, wl_id, created_resource_data.item.id, created_resource_user_data.item.id);
                    }
                    else if (product_id == "2" || product_id == "3")//CNTP, CNTK
                    {
                        create_notif_730(email_textBox.Text, created_user_data.item.id, wl_id, created_resource_data.item.id, created_resource_user_data.item.id);
                    }
                    else if (product_id == "12" || product_id == "24")//Kp_n
                    {
                        create_notif_Kp_n(email_textBox.Text, created_user_data.item.id, wl_id, created_resource_data.item.id, created_resource_user_data.item.id);
                    }
                    else if (product_id == "17" || product_id == "23")//Kb_n
                    {
                        create_notif_Kp_n(email_textBox.Text, created_user_data.item.id, wl_id, created_resource_data.item.id, created_resource_user_data.item.id);
                    }
                    else if (product_id == "7" || product_id == "22")//K_n
                    {
                        create_notif_K_n(email_textBox.Text, created_user_data.item.id, wl_id, created_resource_data.item.id, created_resource_user_data.item.id);
                    }
                    else
                    {
                        MessageBox.Show("Невідомий продукт, сповіщення не створені");
                    }

                    string Body = "<p>Добрий день!</p><p></p><p>Дякуємо за ваш вибір!</p><p>Для вас було створено доступ в систему моніторингу ВЕНБЕСТ. </p><p>----------------------------------------------</p><p>Для входу в систему моніторингу за допомогою мобільного додатку:</p><p>1.Завантажте мобільний додаток <b>Navi Venbest</b>: https://venbest.ua/gps-prilozheniia/</p> <p>2.При першому вході в мобільний додаток введіть такі дані:</p><p>a. Логін: " + email_textBox.Text + "</p><p>b. Пароль: " + pass + " </p><p>Зверніть, будь ласка, увагу, що логін та пароль чутливий до регістру символів, які ви вводите.</p><p> <br></p><p>3.Якщо ви бажаєте отримувати сповіщення, увімкніть їх в налаштуваннях.</p><p>----------------------------------------------</p><p>Для входу в систему моніторингу за допомогою браузеру:</p><p>1.Перейдіть за посиланням: https://navi.venbest.com.ua/</p> <p>2.Введіть логін: " + email_textBox.Text + "</p><p>3.Введіть пароль: " + pass + "</p><p>  <br></p><p>Змініть, будь ласка, пароль в налаштуваннях користувача при вході через браузер.</p><p>----------------------------------------------</p><p>Департамент супутникових систем охорони</p><p>Група Компаній «ВЕНБЕСТ»</p><p>Т 044 501 33 77;</p><p>auto@venbest.com.ua | https://venbest.ua/ohrana-avto-i-zashchita-ot-ugona</p>";
                    macros.send_mail_auto(email_textBox.Text, "ВЕНБЕСТ. Вхід в систему моніторингу", Body);
                    macros.send_mail_auto("auto@venbest.com.ua", "ВЕНБЕСТ. Вхід в систему моніторингу", Body);
                    //Save in db client account

                    macros.GetData("insert into btk.Client_accounts (name, pass, date, reason, Object_idObject, Users_idUsers) value ('" + email_textBox.Text + "','" + pass + "','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','Create account from detail','" + id_db_obj + "','" + vars_form.user_login_id + "');");


                    //update treeView_user_accounts after making chenge
                    build_list_account();
                    email_textBoxTextChanged(email_textBox, EventArgs.Empty);


                    //через запятую перебираем все аккауты из тривив и добавляем в accounts для записи в виалон
                    string accounts = "";
                    for (int index1 = 0; index1 < treeView_user_accounts.Nodes[0].Nodes.Count; index1++)
                    {
                        accounts = accounts + treeView_user_accounts.Nodes[0].Nodes[index1].Text + ", ";
                    }



                    // Обновляем произвольное поле 4.4 Обліковий запис WL созданным записом
                    //Загружаем произвольные поля объекта
                    string json = macros.WialonRequest("&svc=core/search_items&params={" +
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


                    //chek if exist costom feild in WL VO4, VO5
                    foreach (var keyvalue in object_data.items[0].flds)
                    {
                        if (keyvalue.Value.n.Contains("4.4 Обліковий запис WL"))
                        {
                            //update Обліковий запис WL
                            string pp8_answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                                                                    + "\"itemId\":\"" + wl_id + "\","
                                                                    + "\"id\":\""+ keyvalue.Key +"\","
                                                                    + "\"callMode\":\"update\","
                                                                    + "\"n\":\"4.4 Обліковий запис WL\","
                                                                    + "\"v\":\"" + accounts.Replace("\"", "%5C%22") + "\"}");
                            break;
                        }
                    }


                    Clipboard.SetText(textBox_account_pss.Text);
                    // Диалог закрываем заявку или нет при завершении работы с учетными записями
                    on_end_account_job("Створено каінет користувача: "+ email_textBox.Text + " та встановлено пароль: " + textBox_account_pss.Text);



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
                string user_data_answer = macros.WialonRequest("&svc=core/search_items&params={" +
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
                string set_object_accsess_answer = macros.WialonRequest("&svc=user/update_item_access&params={" +
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

                if (product_id == "10" || product_id == "11" || product_id == "13" || product_id == "14" || product_id == "18" || product_id == "19" || product_id == "20" || product_id == "21")//CNTP_910, CNTK_910
                {
                    create_notif_910(email_textBox.Text, user_data.items[0].id, wl_id, get_resource_user.items[0].id, get_resource_user_user.items[0].id);
                }
                else if (product_id == "2" || product_id == "3")//CNTP, CNTK
                {
                    create_notif_730(email_textBox.Text, user_data.items[0].id, wl_id, get_resource_user.items[0].id, get_resource_user_user.items[0].id);
                }
                else if (product_id == "12")//Kp_n
                {
                    create_notif_Kp_n(email_textBox.Text, user_data.items[0].id, wl_id, get_resource_user.items[0].id, get_resource_user_user.items[0].id);
                }
                else if (product_id == "17")//Kb_n
                {
                    create_notif_Kp_n(email_textBox.Text, user_data.items[0].id, wl_id, get_resource_user.items[0].id, get_resource_user_user.items[0].id);
                }
                else if (product_id == "7")//K_n
                {
                    create_notif_K_n(email_textBox.Text, user_data.items[0].id, wl_id, get_resource_user.items[0].id, get_resource_user_user.items[0].id);
                }
                else
                {
                    MessageBox.Show("Невідомий продукт, сповіщення не створені");
                }

                

                //log user action
                macros.LogUserAction(vars_form.user_login_id, "Дозволити користувачу перегляд авто", "", "Надано доступ Account: " + email_textBox.Text + "до обєкту" + wl_id, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));

                //update treeView_user_accounts after making chenge
                build_list_account();

                //через запятую перебираем все аккауты из тривив и добавляем в accounts для записи в виалон
                string accounts = "";
                for (int index1 = 0; index1 < treeView_user_accounts.Nodes[0].Nodes.Count; index1++)
                {
                    accounts = accounts + treeView_user_accounts.Nodes[0].Nodes[index1].Text + ", ";
                }




                // Обновляем произвольное поле 4.4 Обліковий запис WL созданным записом
                //Загружаем произвольные поля объекта
                string json = macros.WialonRequest("&svc=core/search_items&params={" +
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


                //4.4 Обліковий запис WL
                foreach (var keyvalue in object_data.items[0].flds)
                {
                    if (keyvalue.Value.n.Contains("4.4 Обліковий запис WL"))
                    {
                        //update Обліковий запис WL
                        string pp8_answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + wl_id + "\","
                                                                + "\"id\":\"" + keyvalue.Key + "\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"4.4 Обліковий запис WL\","
                                                                + "\"v\":\"" + accounts.Replace("\"", "%5C%22") + "\"}");
                        break;
                    }
                }




                on_end_account_job("Дозволено перегляд для користувача : " + email_textBox.Text);
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
            string CreteNotifAnswer = macros.WialonRequest("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Cработка: Датчик удара/наклона\"," +                            /* название */
                                                            "\"txt\":\"%25UNIT%25: сработал датчик удара/наклона. Время сработки: %25MSG_TIME%25. В %25POS_TIME%25 автомобиль находился около '%25LOCATION%25'.\"," +     /* текст уведомления */
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
                                                                                    //"\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Venbest Navi" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

            //Блокировка двигателя
            string CreteNotifAnswer2 = macros.WialonRequest("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Блокировка двигателя\"," +                                 /* название */
                                                            "\"txt\":\"%25UNIT%25: Произошла блокировка двигателя. Время сработки: %25MSG_TIME%25  В %25POS_TIME%25 автомобиль двигался со скоростью %25SPEED%25 около '%25LOCATION%25'.\"," +     /* текст уведомления */
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
                                                                                    //"\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Venbest Navi" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

            //Низкое напряжения АКБ
            string CreteNotifAnswer3 = macros.WialonRequest("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Низкое напряжения АКБ\"," +                                 /* название */
                                                            "\"txt\":\"%25UNIT%25: низкое напряжение АКБ. Автомобиль находился около '%25LOCATION%25'.\"," +     /* текст уведомления */
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
                                                                                    //"\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Venbest Navi" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

            //Сработка: Датчики взлома
            string CreteNotifAnswer4 = macros.WialonRequest("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Сработка: Датчики взлома\"," +                                 /* название */
                                                            "\"txt\":\"%25UNIT%25: Несанкционированное открытие дверей, капота или багажника. Время сработки: %25MSG_TIME%25. Автомобиль находился около '%25LOCATION%25'.\"," +     /* текст уведомления */
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
                                                                                    //"\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Venbest Navi" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );



            //Сработка: Датчики взлома
            string CreteNotifAnswer5 = macros.WialonRequest("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"ТРЕВОЖНАЯ КНОПКА\"," +                                 /* название */
                                                            "\"txt\":\"%25UNIT%25: НАЖАТА ТРЕВОЖНАЯ КНОПКА!!!!! Время сработки: %25MSG_TIME%25. В %25POS_TIME%25 объект двигался со скоростью %25SPEED%25 около '%25LOCATION%25'.\"," +     /* текст уведомления */
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
                                                                                    //"\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Venbest Navi" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

            //Сработка датчика глушения
            string CreteNotifAnswer6 = macros.WialonRequest("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Сработка: датчика глушения\"," +                                 /* название */
                                                            "\"txt\":\"%25UNIT%25: Сработка датчика глушения. Время сработки: %25MSG_TIME%25. В %25POS_TIME%25 объект двигался со скоростью %25SPEED%25 около '%25LOCATION%25'.\"," +     /* текст уведомления */
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
                                                                                    //"\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Venbest Navi" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

        }

        // K_n create nitif for new user account
        private void create_notif_K_n(string user_account_name, int user_account_id, string object_id, int resours_id, int resours_user_id)
        {
            //Низкое напряжения АКБ
            string CreteNotifAnswer3 = macros.WialonRequest("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Напряжение АКБ\"," +                                 /* название */
                                                            "\"txt\":\"%25UNIT25%: низкое напряжение АКБ. Автомобиль находился около '%25LOCATION%25'.\"," +     /* текст уведомления */
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
                                                                                    //"\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Venbest Navi" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

        }

        // Kb_n create nitif for new user account
        private void create_notif_Kb_n(string user_account_name, int user_account_id, string object_id, int resours_id, int resours_user_id)
        {


            //Блокировка двигателя
            string CreteNotifAnswer2 = macros.WialonRequest("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Блокировка двигателя\"," +                                 /* название */
                                                            "\"txt\":\"%25UNIT%25: Произошла блокировка двигателя. Время сработки: %25MSG_TIME%25  В %25POS_TIME%25 автомобиль двигался со скоростью %25SPEED%25 около '%25LOCATION%25'.\"," +     /* текст уведомления */
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
                                                                                    //"\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Venbest Navi" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

            //Низкое напряжения АКБ
            string CreteNotifAnswer3 = macros.WialonRequest("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Напряжение АКБ\"," +                                 /* название */
                                                            "\"txt\":\"%25UNIT%25: низкое напряжение АКБ. Автомобиль находился около '%25LOCATION%25'.\"," +     /* текст уведомления */
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
                                                                                    //"\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Venbest Navi" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

        }
        // Kp_n_ create nitif for new user account
        private void create_notif_Kp_n(string user_account_name, int user_account_id, string object_id, int resours_id, int resours_user_id)
        {
            

            //Блокировка двигателя
            string CreteNotifAnswer2 = macros.WialonRequest("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Блокировка двигателя\"," +                                 /* название */
                                                            "\"txt\":\"%25UNIT%25: Произошла блокировка двигателя. Время сработки: %25MSG_TIME%25  В %25POS_TIME%25 автомобиль двигался со скоростью %25SPEED%25 около '%25LOCATION%25'.\"," +     /* текст уведомления */
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
                                                                                    //"\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Venbest Navi" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

            //Низкое напряжения АКБ
            string CreteNotifAnswer3 = macros.WialonRequest("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Напряжение АКБ\"," +                                 /* название */
                                                            "\"txt\":\"%25UNIT%25: низкое напряжение АКБ. Автомобиль находился около '%25LOCATION%25'.\"," +     /* текст уведомления */
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
                                                                                    //"\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Venbest Navi" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );
            

            //Сработка: ТРЕВОЖНАЯ КНОПКА
            string CreteNotifAnswer5 = macros.WialonRequest("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"ТРЕВОЖНАЯ КНОПКА\"," +                                 /* название */
                                                            "\"txt\":\"%25UNIT%25: НАЖАТА ТРЕВОЖНАЯ КНОПКА!!!!! Время сработки: %25MSG_TIME%25. В %25POS_TIME%25 объект двигался со скоростью %25SPEED%25 около '%25LOCATION%25'.\"," +     /* текст уведомления */
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
                                                                                    //"\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Venbest Navi" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
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
            string CreteNotifAnswer = macros.WialonRequest("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Cработка: Датчик удара/наклона\"," +                            /* название */
                                                            "\"txt\":\"%25UNIT%25: сработал датчик удара/наклона/буксировки. Время сработки: %25MSG_TIME%25. В %25POS_TIME%25 автомобиль находился около '%25LOCATION%25'.'.\"," +     /* текст уведомления */
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
                                                                                    //"\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Venbest Navi" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

            //Блокировка двигателя
            string CreteNotifAnswer2 = macros.WialonRequest("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Блокировка двигателя\"," +                                 /* название */
                                                            "\"txt\":\"%25UNIT%25: Произошла блокировка двигателя. Время сработки: %25MSG_TIME%25  В %25POS_TIME%25 автомобиль двигался со скоростью %25SPEED%25 около '%25LOCATION%25'.\"," +     /* текст уведомления */
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
                                                                                    //"\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Venbest Navi" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

            //Низкое напряжения АКБ
            string CreteNotifAnswer3 = macros.WialonRequest("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Напряжение АКБ\"," +                                 /* название */
                                                            "\"txt\":\"%25UNIT%25: низкое напряжение АКБ. Автомобиль находился около '%25LOCATION%25'.\"," +     /* текст уведомления */
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
                                                                                    //"\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Venbest Navi" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

            //Сработка: Датчики взлома
            string CreteNotifAnswer4 = macros.WialonRequest("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"Сработка: Датчики взлома\"," +                                 /* название */
                                                            "\"txt\":\"%25UNIT%25: Несанкционированное открытие дверей, капота или багажника. Время сработки: %25MSG_TIME%25. Автомобиль находился около '%25LOCATION%25'.\"," +     /* текст уведомления */
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
                                                                                    //"\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Venbest Navi" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                   "}" +
                                                                        "}" +
                                                                 "]" +
                                                        "}"
                                                    );

            //Сработка: ТРЕВОЖНАЯ КНОПКА
            string CreteNotifAnswer5 = macros.WialonRequest("&svc=resource/update_notification&params={" +
                                                            "\"itemId\":\"" + resours_id + "\"," +                                             /* ID ресурса */
                                                            "\"id\":\"0\"," +                                                   /* ID уведомления (0 для создания) */
                                                            "\"callMode\":\"create\"," +                                        /* режим: создание, редактирование, включение/выключение, удаление (create, update, enable, delete) */
                                                            "\"e\":\"1\"," +                                                    /* только для режима включения/выключения: 1 - включить, 0 выключить */
                                                            "\"n\":\"ТРЕВОЖНАЯ КНОПКА\"," +                                 /* название */
                                                            "\"txt\":\"%25UNIT%25: НАЖАТА ТРЕВОЖНАЯ КНОПКА!!!!! Время сработки: %25MSG_TIME%25. В %25POS_TIME%25 объект двигался со скоростью %25SPEED%25 около '%25LOCATION%25'.\"," +     /* текст уведомления */
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
                                                                                    //"\"apps\":\"{" + "\\" + "\"Wialon Local" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
                                                                                    "\"apps\":\"{" + "\\" + "\"Venbest Navi" + "\\" + "\"" + ":" + "[" + user_account_id + "]" + "}\"" +
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
                string saving_selected_account = treeView_user_accounts.SelectedNode.Text;
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

                    string json = macros.WialonRequest("&svc=core/search_items&params={" +
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

                    string json1 = macros.WialonRequest("&svc=user/update_password&params={" +
                                                             "\"userId\":\"" + m.items[0].id + "\"," +
                                                             "\"oldPassword\":\"\"," +
                                                             "\"newPassword\":\"" + pass + "\"}");
                    var m1 = JsonConvert.DeserializeObject<RootObject>(json1);

                    string Body = "<p>Добрий день!</p><p>Відповідно до Вашого запиту, для Вас було відновлено пароль для доступу в систему моніторингу ВЕНБЕСТ. </p><p>----------------------------------------------</p><p>Для входу в систему моніторингу за допомогою мобільного додатку:</p><p>1.Завантажте мобільний додаток: https://venbest.ua/gps-prilozheniia/</p> <p>2.При першому вході в мобільний додаток введіть такі дані:</p><p>a. Логін: " + treeView_user_accounts.SelectedNode.Text + "</p><p>b.Пароль: " + pass + " </p><p>Зверніть, будь ласка, увагу, що логін та пароль чутливий до регістру символів, які ви вводите.</p><p> <br></p><p>3.Якщо ви бажаєте отримувати сповіщення, увімкніть їх в налаштуваннях.</p><p>----------------------------------------------</p><p>Для входу в систему моніторингу за допомогою браузеру:</p><p>1.Перейдіть за посиланням: https://navi.venbest.com.ua/</p> <p>2.Введіть логін: " + treeView_user_accounts.SelectedNode.Text + "</p><p>3.Введіть пароль: " + pass + "</p><p>  <br></p><p>Змініть, будь ласка, пароль в налаштуваннях користувача при вході через браузер.</p><p>----------------------------------------------</p><p>Департамент супутникових систем охорони</p><p>Група Компаній «ВЕНБЕСТ»</p><p>Т 044 501 33 77;</p><p>auto@venbest.com.ua | https://venbest.ua/ohrana-avto-i-zashchita-ot-ugona</p>";
                    macros.send_mail_auto(treeView_user_accounts.SelectedNode.Text, "ВЕНБЕСТ. Вхід в систему моніторингу", Body);
                    macros.send_mail_auto("auto@venbest.com.ua", "ВЕНБЕСТ. Вхід в систему моніторингу", Body);

                    //Save in db client account
                    macros.GetData("insert into btk.Client_accounts (name, pass, date, reason, Object_idObject, Users_idUsers) value ('" + treeView_user_accounts.SelectedNode.Text + "','" + pass + "','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "','Chenge pass account','" + id_db_obj + "','" + vars_form.user_login_id + "');");
                    Clipboard.SetText(textBox_account_pss.Text);

                    // Диалог закрываем заявку или нет при завершении работы с учетными записями
                    on_end_account_job("Відновлено пароль: " + saving_selected_account);
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
                string saving_selected_account = treeView_user_accounts.SelectedNode.Text;
                DialogResult dialogResult = MessageBox.Show("Відключити користувача", "Відключити?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string json = macros.WialonRequest("&svc=core/search_items&params={" +
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

                    string json2 = macros.WialonRequest("&svc=user/update_user_flags&params={" +
                                                    "\"userId\":\"" + m.items[0].id + "\"," +
                                                    "\"flags\":\"1\"," +
                                                    "\"flagsMask\":\"1\"}");
                    var m2 = JsonConvert.DeserializeObject<RootObject>(json2);

                    //log user action
                    macros.LogUserAction(vars_form.user_login_id, "Відключити користувача", treeView_user_accounts.SelectedNode.Text, "", Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));

                    // Диалог закрываем заявку или нет при завершении работы с учетными записями
                    on_end_account_job("Вимкнено доступ в систему для користувача : " + saving_selected_account);

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
                string saving_selected_account = treeView_user_accounts.SelectedNode.Text;
                DialogResult dialogResult = MessageBox.Show("Включити користувача", "Включити?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string json = macros.WialonRequest("&svc=core/search_items&params={" +
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

                    string json2 = macros.WialonRequest("&svc=user/update_user_flags&params={" +
                                                    "\"userId\":\"" + m.items[0].id + "\"," +
                                                    "\"flags\":\"0\"," +
                                                    "\"flagsMask\":\"1\"}");
                    var m2 = JsonConvert.DeserializeObject<RootObject>(json2);

                    //log user action
                    macros.LogUserAction(vars_form.user_login_id, "Відключити користувача", treeView_user_accounts.SelectedNode.Text, "", Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));

                    // Диалог закрываем заявку или нет при завершении работы с учетными записями
                    on_end_account_job("Ввімкнено доступ в систему для користувача : " + saving_selected_account);

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
                MessageBox.Show("Вибери який кабінет забрати з доступу!");
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
                        string UserDataAnswer = macros.WialonRequest("&svc=core/search_items&params={" +
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
                        string ResourceDataAnswer = macros.WialonRequest("&svc=core/search_items&params={" +
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
                        string json4 = macros.WialonRequest("&svc=user/update_item_access&params={" +
                                                                 "\"userId\":\"" + UserData.items[0].id + "\"," +
                                                                 "\"itemId\":\"" + wl_id + "\"," +
                                                                 "\"accessMask\":\"0\"}");
                        var m4 = JsonConvert.DeserializeObject<RootObject>(json4);


                        //update treeView_user_accounts after making chenge
                        build_list_account();


                        //через запятую перебираем все аккауты из тривив и добавляем в accounts для записи в виалон
                        string accounts = "";
                        for (int index1 = 0; index1 < treeView_user_accounts.Nodes[0].Nodes.Count; index1++)
                        {
                            accounts = accounts + treeView_user_accounts.Nodes[0].Nodes[index1].Text + ", ";
                        }



                        // Обновляем произвольное поле 4.4 Обліковий запис WL созданным записом
                        //Загружаем произвольные поля объекта
                        string json = macros.WialonRequest("&svc=core/search_items&params={" +
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


                        //chek if exist costom feild in WL VO4, VO5
                        foreach (var keyvalue in object_data.items[0].flds)
                        {
                            if (keyvalue.Value.n.Contains("4.4 Обліковий запис WL"))
                            {
                                //update Обліковий запис WL
                                string pp8_answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                                                                        + "\"itemId\":\"" + wl_id + "\","
                                                                        + "\"id\":\"" + keyvalue.Key + "\","
                                                                        + "\"callMode\":\"update\","
                                                                        + "\"n\":\"4.4 Обліковий запис WL\","
                                                                        + "\"v\":\"" + accounts.Replace("\"", "%5C%22") + "\"}");
                                break;
                            }
                        }



                        //log user action
                        macros.LogUserAction(vars_form.user_login_id, "Прибрати з доступу користувача", saving_selected_account, wl_id, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));


                        

                        // Диалог закрываем заявку или нет при завершении работы с учетными записями
                        on_end_account_job("Заборонено перегляд авто для користувача : " + saving_selected_account);


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
                    string saving_selected_account = treeView_user_accounts.SelectedNode.Text;

                    DialogResult dialogResult = MessageBox.Show("Видалити кабінет: " + treeView_user_accounts.SelectedNode.Text + "", "Видалити?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string json1 = macros.WialonRequest("&svc=core/search_items&params={" +
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
                                    string json2 = macros.WialonRequest("&svc=item/delete_item&params={" +
                                                                             "\"itemId\":\"" + t.id + "\"}");
                                }
                            }//удаляем сначала все ресурсы cls=3

                            foreach (var t in m.items)
                            {
                                if (t.cls == 1)
                                {
                                    string json2 = macros.WialonRequest("&svc=item/delete_item&params={" +
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


                        //через запятую перебираем все аккауты из тривив и добавляем в accounts для записи в виалон
                        string accounts = "";
                        for (int index1 = 0; index1 < treeView_user_accounts.Nodes[0].Nodes.Count; index1++)
                        {
                            accounts = accounts + treeView_user_accounts.Nodes[0].Nodes[index1].Text + ", ";
                        }



                        // Обновляем произвольное поле 4.4 Обліковий запис WL созданным записом
                        //Загружаем произвольные поля объекта
                        string json = macros.WialonRequest("&svc=core/search_items&params={" +
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


                        //chek if exist costom feild in WL VO4, VO5
                        foreach (var keyvalue in object_data.items[0].flds)
                        {
                            if (keyvalue.Value.n.Contains("4.4 Обліковий запис WL"))
                            {
                                //update Обліковий запис WL
                                string pp8_answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                                                                        + "\"itemId\":\"" + wl_id + "\","
                                                                        + "\"id\":\"" + keyvalue.Key + "\","
                                                                        + "\"callMode\":\"update\","
                                                                        + "\"n\":\"4.4 Обліковий запис WL\","
                                                                        + "\"v\":\"" + accounts.Replace("\"", "%5C%22") + "\"}");
                                break;
                            }
                        }



                        on_end_account_job("Видалено користувача : " + saving_selected_account);
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
            string CreteNotifAnswer = macros.WialonRequest("&svc=resource/update_notification&params={" +
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
                try
                {
                    

                    build_list_account();
                    groupBox3.Visible = false;
                    groupBox3.SendToBack();
                }
                catch
                { MessageBox.Show("tabPage_edit_client"); }
            }
            else if (tabControl2.SelectedTab.Name == "tabPage_Edit_VO")
            {
                try
                {
                    load_vo();
                    groupBox3.Visible = true;
                    groupBox3.BringToFront();
                }
                catch
                { MessageBox.Show("tabPage_Edit_VO"); }
            }
            else if (tabControl2.SelectedTab.Name == "tabPage6")
            {
                try
                {
                    Arhiv_object();
                }
                catch
                { MessageBox.Show("tabPage6"); }
            }
            else if (tabControl2.SelectedTab.Name == "tabPage_rouming")
            {
                try
                {
                    groupBox3.Visible = false;
                    groupBox3.SendToBack();
                }
                catch
                { MessageBox.Show("tabPage_rouming"); }

                textBox_name.Text = _unit_name;
                textBox_IMEI.Text = IMEI_object;
                textBox_vin.Text = VIN_object;

                string sql = string.Format("SELECT Kontragenti_full_name, idKontragenti FROM btk.Kontragenti  where kontragent_type_idkontragent_type = 2;");
                var temp = macros.GetData(sql);
                comboBox_Project.DataSource = null;
                comboBox_Project.DisplayMember = "Kontragenti_full_name";
                comboBox_Project.ValueMember = "idKontragenti";
                comboBox_Project.DataSource = temp;

                //RoumingAccept = macros.sql_command("select RoumingAccept from btk.notification where idnotification = '" + _id_notif + "'");
                DataTable dt = macros.GetData("select StartDate, EndDate, RoumingAccepted from btk.RoumingZayavka where notification_idnotification = '" + _id_notif + "'");
                if(dt.Rows.Count == 0)
                {
                    macros.sql_command("insert into btk.RoumingZayavka(" +
                                    "StartDate, " +
                                    "EndDate, " +
                                    "notification_idnotification)" +
                                    "values('" + Convert.ToDateTime(DateTime.Now).Date.ToString("yyyy-MM-dd") + "', " +
                                    "'" + Convert.ToDateTime(DateTime.Now.AddDays(1)).Date.ToString("yyyy-MM-dd") + "'," +
                                    "'" + _id_notif + "' " +
                                    "); ");
                    dt = macros.GetData("select StartDate, EndDate, RoumingAccepted from btk.RoumingZayavka where notification_idnotification = '" + _id_notif + "'");
                    RoumingAccept = dt.Rows[0][2].ToString();
                    RoumingZapitStart_dateTimePicker.Value = Convert.ToDateTime(dt.Rows[0][0]).Date;
                    RoumingZapitEnd_dateTimePicker.Value = Convert.ToDateTime(dt.Rows[0][1]).Date;

                    Rouming_SIM1_End_dtp.Value = RoumingZapitEnd_dateTimePicker.Value;
                    Rouming_SIM1_Start_dtp.Value = RoumingZapitStart_dateTimePicker.Value;

                    Rouming_SIM2_End_dtp.Value = RoumingZapitEnd_dateTimePicker.Value;
                    Rouming_SIM2_Start_dtp.Value = RoumingZapitStart_dateTimePicker.Value;

                }
                else
                {
                    RoumingAccept = dt.Rows[0][2].ToString();
                    RoumingZapitStart_dateTimePicker.Value = Convert.ToDateTime(dt.Rows[0][0]).Date;
                    RoumingZapitEnd_dateTimePicker.Value = Convert.ToDateTime(dt.Rows[0][1]).Date;

                    Rouming_SIM1_End_dtp.Value = RoumingZapitEnd_dateTimePicker.Value;
                    Rouming_SIM1_Start_dtp.Value = RoumingZapitStart_dateTimePicker.Value;

                    Rouming_SIM2_End_dtp.Value = RoumingZapitEnd_dateTimePicker.Value;
                    Rouming_SIM2_Start_dtp.Value = RoumingZapitStart_dateTimePicker.Value;
                }
                if (RoumingAccept == "True")
                {
                    AcceptRouming_button.Text = "Погоджено";
                    AcceptRouming_button.BackColor = Color.Green;
                }


                string json = macros.WialonRequest("&svc=core/search_items&params={" +
                                                        "\"spec\":{" +
                                                        "\"itemsType\":\"avl_unit\"," +
                                                        "\"propName\":\"sys_id\"," +
                                                        "\"propValueMask\":\"" + wl_id + "\", " +
                                                        "\"sortType\":\"sys_name\"," +
                                                        "\"or_logic\":\"1\"}," +
                                                        "\"force\":\"1\"," +
                                                        "\"flags\":\"257\"," +
                                                        "\"from\":\"0\"," +
                                                        "\"to\":\"1\"}");
                var m = JsonConvert.DeserializeObject<RootObject>(json);
                //Заполняем SIM1
                if (m.items[0].ph != "")
                {
                    groupBox_SIM1.Enabled = true;
                    textBox_SIM1.Text = m.items[0].ph;
                    if (m.items[0].ph.Contains("+38067"))
                    {
                        idSimCard = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number like '" + m.items[0].ph.Remove(0, 4) + "';");
                        if (idSimCard != "")
                        {
                            DataTable RoumingTarifList = macros.GetData("SELECT idRouming_tarif, TarifName FROM btk.Rouming_tarif where OperatorName = 'KS';");
                            RoumingTarifSIM1_comboBox.DataSource = RoumingTarifList;
                            RoumingTarifSIM1_comboBox.ValueMember = "idRouming_tarif";
                            RoumingTarifSIM1_comboBox.DisplayMember = "TarifName";

                            

                            string tarif = macros.sql_command("SELECT Rouming_tarif_idRouming_tarif FROM btk.Simcard_rouming where Simcard_idSimcard = '" + idSimCard + "' order by idSimcard_rouming desc limit 1;");
                            if (tarif != "") 
                            { 
                                RoumingTarifSIM1_comboBox.SelectedValue = tarif; 

                            }
                        }
                    }
                    else if (m.items[0].ph.Contains("+882"))
                    {
                        idSimCard = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number like '" + m.items[0].ph.Remove(0, 1) + "';");
                        if (idSimCard != "")
                        {
                            DataTable RoumingTarifList = macros.GetData("SELECT idRouming_tarif, TarifName FROM btk.Rouming_tarif where OperatorName = 'VF';");
                            RoumingTarifSIM1_comboBox.DataSource = RoumingTarifList;
                            RoumingTarifSIM1_comboBox.ValueMember = "idRouming_tarif";
                            RoumingTarifSIM1_comboBox.DisplayMember = "TarifName";

                            string ICCID = macros.sql_command("SELECT Simcardcol_imsi FROM btk.Simcard where Simcardcol_number like '" + m.items[0].ph.Remove(0, 1) + "';");
                            if (ICCID != "")
                            {
                                string t = macros.VodafoneGetServiceProfile(ICCID);
                                var ServiceProfileData = JsonConvert.DeserializeObject<RootObject>(t);
                                RoumingTarifSIM1_comboBox.Text = ServiceProfileData.getDeviceDetailsv2Response.@return.customerServiceProfile;

                            }
                        }
                    }
                    else if (m.items[0].ph.Contains("+38050"))
                    {

                    }
                    else
                    {
                        groupBox_SIM1.Enabled = false;
                    }
                }
                else
                { groupBox_SIM1.Enabled = false; textBox_SIM1.Text = "Не встановлено"; }

                if (m.items[0].ph2 != "")
                {
                    groupBox_SIM2.Enabled = true;
                    textBox_SIM2.Text = m.items[0].ph2;
                    if (m.items[0].ph2.Contains("+38067"))
                    {
                        string rf = m.items[0].ph2.Remove(0, 4);
                        idSim2Card = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number like '" + m.items[0].ph2.Remove(0, 4) + "';");
                        if (idSim2Card != "")
                        {
                            DataTable RoumingTarifList = macros.GetData("SELECT idRouming_tarif, TarifName FROM btk.Rouming_tarif where OperatorName = 'KS';");
                            RoumingTarifSIM2_comboBox.DataSource = RoumingTarifList;
                            RoumingTarifSIM2_comboBox.ValueMember = "idRouming_tarif";
                            RoumingTarifSIM2_comboBox.DisplayMember = "TarifName";

                            string tarif = macros.sql_command("SELECT Rouming_tarif_idRouming_tarif FROM btk.Simcard_rouming where Simcard_idSimcard = '" + idSimCard + "' order by idSimcard_rouming desc limit 1;");
                            if (tarif != "")
                            {
                                RoumingTarifSIM2_comboBox.SelectedValue = tarif;

                            }
                        }
                    }
                    else if (m.items[0].ph2.Contains("+882"))
                    {
                        idSim2Card = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number like '" + m.items[0].ph2.Remove(0, 1) + "';");
                        if (idSim2Card != "")
                        {
                            DataTable RoumingTarifList = macros.GetData("SELECT idRouming_tarif, TarifName FROM btk.Rouming_tarif where OperatorName = 'VF';");
                            RoumingTarifSIM2_comboBox.DataSource = RoumingTarifList;
                            RoumingTarifSIM2_comboBox.ValueMember = "idRouming_tarif";
                            RoumingTarifSIM2_comboBox.DisplayMember = "TarifName";

                            string ICCID = macros.sql_command("SELECT Simcardcol_imsi FROM btk.Simcard where Simcardcol_number like '" + m.items[0].ph2.Remove(0, 1) + "';");
                            if (ICCID != "")
                            {
                                string t = macros.VodafoneGetServiceProfile(ICCID);
                                var ServiceProfileData = JsonConvert.DeserializeObject<RootObject>(t);
                                RoumingTarifSIM2_comboBox.Text = ServiceProfileData.getDeviceDetailsv2Response.@return.customerServiceProfile;

                            }
                        }
                    }
                    else if (m.items[0].ph2.Contains("+38050"))
                    {
                    }
                    else 
                    { 
                        groupBox_SIM2.Enabled = false;
                    }
                }
                else
                { groupBox_SIM2.Enabled = false; textBox_SIM2.Text = "Не встановлено"; }
                ReadRoumingHistory();
            }
            else if (tabControl2.SelectedTab.Name == "tabPage_locator")
            {

                //start mini map
                try
                {
                    groupBox3.Visible = true;
                    groupBox3.BringToFront();

                    //geckoWebBrowser1.Navigate("http://10.44.30.32/disp_app/HTMLPage_map.html?foo=" + _search_id);

                    string json = macros.WialonRequestSimple("&svc=token/update&params={" +
                                                        "\"callMode\":\"create\"," +
                                                        "\"app\":\"locator\"," +
                                                        "\"at\":\"0\"," +
                                                        "\"dur\":\"1800\"," +
                                                        "\"fl\":\"-1\"," +
                                                        "\"p\":\"{" + "\\" + "\"sensorMasks" + "\\" + "\"" + ":[" + "\\" + "\"*" + "\\" + "\"]," + "\\" + "\"note" + "\\" + "\"" + ":" + "\\" + "\"" + vars_form.unit_name + "" + "\\" + "\"," + "\\" + "\"zones" + "\\" + "\"" + ":" + "\\" + "\"1" + "\\" + "\"," + "\\" + "\"tracks" + "\\" + "\"" + ":" + "\\" + "\"1" + "\\" + "\"" + "}\"," +
                                                        "\"items\":[" + _search_id + "]" +
                                                        "}");

                    var m = JsonConvert.DeserializeObject<locator>(json);

                    string locator_url = "https://navi.venbest.com.ua/locator/index.html?t=" + m.h;
                    geckoWebBrowser2.Navigate(locator_url);

                }
                catch
                {
                    MessageBox.Show("tabPage_locator");
                }
            }
            else if (tabControl2.SelectedTab.Name == "tabPage3")
            {
                try
                {
                    groupBox3.Visible = true;
                    groupBox3.BringToFront();
                }
                catch
                { MessageBox.Show("tabPage3"); }
            }
            else
            {
                try
                {
                    groupBox3.Visible = false;
                    groupBox3.SendToBack();
                }
                catch
                { MessageBox.Show("else"); }
            }
        }

        private void treeView_client_info_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Text.Contains("ВО1:") )
            {
                //load VO1
                string VO1 = macros.sql_command("select Kontakti_idKontakti from btk.VO where Object_idObject = '" + id_db_obj + "' and VOcol_num_vo = '1' ORDER BY idVO DESC limit 1;");
                if (VO1 != "" || VO1 != "1")
                {
                    string VO_phone1 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + VO1.ToString() + "';");
                    string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + VO1.ToString() + "';");


                    string path = macros.GetProcessPath("microsip");
                    if (path == "")
                    { MessageBox.Show("Для виклику необхідно запустити Microsip!"); return; }
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C " + path + " " + new String(VO_phone1.Where(Char.IsDigit).ToArray());
                    process.StartInfo = startInfo;
                    process.Start();
                }
            }
            if (e.Node.Text.Contains("ВО2:"))
            {
                //load VO1
                string VO2 = macros.sql_command("select Kontakti_idKontakti from btk.VO where Object_idObject = '" + id_db_obj + "' and VOcol_num_vo = '2' ORDER BY idVO DESC limit 1;");
                if (VO2 != "" || VO2 != "1")
                {
                    string VO_phone1 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + VO2.ToString() + "';");
                    string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + VO2.ToString() + "';");


                    string path = macros.GetProcessPath("microsip");
                    if (path == "")
                    { MessageBox.Show("Для виклику необхідно запустити Microsip!"); return; }
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C " + path + " " + new String(VO_phone1.Where(Char.IsDigit).ToArray());
                    process.StartInfo = startInfo;
                    process.Start();
                }
            }
            if (e.Node.Text.Contains("ВО3:"))
            {
                //load VO1
                string VO3 = macros.sql_command("select Kontakti_idKontakti from btk.VO where Object_idObject = '" + id_db_obj + "' and VOcol_num_vo = '3' ORDER BY idVO DESC limit 1;");
                if (VO3 != "" || VO3 != "1")
                {
                    string VO_phone1 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + VO3.ToString() + "';");
                    string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + VO3.ToString() + "';");


                    string path = macros.GetProcessPath("microsip");
                    if (path == "")
                    { MessageBox.Show("Для виклику необхідно запустити Microsip!"); return; }
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C " + path + " " + new String(VO_phone1.Where(Char.IsDigit).ToArray());
                    process.StartInfo = startInfo;
                    process.Start();
                }
            }
            if (e.Node.Text.Contains("ВО4:"))
            {
                //load VO1
                string VO4 = macros.sql_command("select Kontakti_idKontakti from btk.VO where Object_idObject = '" + id_db_obj + "' and VOcol_num_vo = '4' ORDER BY idVO DESC limit 1;");
                if (VO4 != "" || VO4 != "1")
                {
                    string VO_phone1 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + VO4.ToString() + "';");
                    string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + VO4.ToString() + "';");


                    string path = macros.GetProcessPath("microsip");
                    if (path == "")
                    { MessageBox.Show("Для виклику необхідно запустити Microsip!"); return; }
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C " + path + " " + new String(VO_phone1.Where(Char.IsDigit).ToArray());
                    process.StartInfo = startInfo;
                    process.Start();
                }
            }
            if (e.Node.Text.Contains("ВО5:"))
            {
                //load VO1
                string VO5 = macros.sql_command("select Kontakti_idKontakti from btk.VO where Object_idObject = '" + id_db_obj + "' and VOcol_num_vo = '5' ORDER BY idVO DESC limit 1;");
                if (VO5 != "" || VO5 != "1")
                {
                    string VO_phone1 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and idKontakti = '" + VO5.ToString() + "';");
                    string VO_phone2 = macros.sql_command("SELECT Phonebook.Phonebookcol_phone FROM btk.Kontakti, btk.Phonebook where  Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook1 and idKontakti = '" + VO5.ToString() + "';");


                    string path = macros.GetProcessPath("microsip");
                    if (path == "")
                    { MessageBox.Show("Для виклику необхідно запустити Microsip!"); return; }
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C " + path + " " + new String(VO_phone1.Where(Char.IsDigit).ToArray());
                    process.StartInfo = startInfo;
                    process.Start();
                }
            }
        }

        private void checkBox_geozone_all_CheckedChanged(object sender, EventArgs e)
        {
            get_sensor_value();
        }

        //Rouming KS

        private void Rouming_KS_Start_dtp_ValueChanged(object sender, EventArgs e)
        {
            if (Rouming_SIM2_Start_dtp.Value.Date >= Rouming_SIM2_End_dtp.Value.Date)
            {
                MessageBox.Show("Выбран не верный период");
                return;
            }
            TimeSpan t = Rouming_SIM2_End_dtp.Value.Date - Rouming_SIM2_Start_dtp.Value.Date;
            if (t.Days <= 32)
            {  }
            if (t.Days >= 32)
            {  }
        }

        private void Rouming_KS_End_dtp_ValueChanged(object sender, EventArgs e)
        {
            if (Rouming_SIM2_Start_dtp.Value.Date >= Rouming_SIM2_End_dtp.Value.Date)
            {
                MessageBox.Show("Выбран не верный период");
                return;
            }
            TimeSpan t = Rouming_SIM2_End_dtp.Value.Date - Rouming_SIM2_Start_dtp.Value.Date;
            if (t.Days <= 32)
            { }
            if (t.Days >= 32)
            { }
        }

       


        //Rouming VF
        private void Rouming_VF_Start_dtp_ValueChanged(object sender, EventArgs e)
        {
            if (Rouming_SIM1_Start_dtp.Value.Date >= Rouming_SIM1_End_dtp.Value.Date)
            {
                MessageBox.Show("Выбран не верный период");
                return;
            }
        }

        private void Rouming_VF_End_dtp_ValueChanged(object sender, EventArgs e)
        {
            if (Rouming_SIM1_Start_dtp.Value.Date >= Rouming_SIM1_End_dtp.Value.Date)
            {
                MessageBox.Show("Выбран не верный период");
                return;
            }
        }

        

        private void button_VF_Rouming_enter_Click(object sender, EventArgs e)
        {
            //string json = macros.WialonRequest("&svc=core/search_items&params={" +
            //                                        "\"spec\":{" +
            //                                        "\"itemsType\":\"avl_unit\"," +
            //                                        "\"propName\":\"sys_id\"," +
            //                                        "\"propValueMask\":\"" + wl_id + "\", " +
            //                                        "\"sortType\":\"sys_name\"," +
            //                                        "\"or_logic\":\"1\"}," +
            //                                        "\"force\":\"1\"," +
            //                                        "\"flags\":\"257\"," +
            //                                        "\"from\":\"0\"," +
            //                                        "\"to\":\"1\"}");
            //var m = JsonConvert.DeserializeObject<RootObject>(json);

            //idSimCard = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number like '"+ textBox_SIM1.Text.Remove(0, 1) + "';");
            if (idSimCard == "")
            {
                MessageBox.Show("SIM не знайдено в базі, зверніться до 117");
                return;
            }

            if (RoumingTarifSIM1_comboBox.Text == "")
            {
                MessageBox.Show("Не вибрано тариф роумінгу");
                return;
            }

            if (Rouming_SIM1_Start_dtp.Value.Date >= Rouming_SIM1_End_dtp.Value.Date)
            {
                MessageBox.Show("Перевір дату");
                return;
            }
            

            if (textBox_SIM1.Text.Contains("+38067"))
            {
                if (RoumingTarifSIM1_comboBox.Text == "Украина (вимк.)")
                {
                    macros.sql_command("insert into btk.Simcard_rouming (" +
                   "Simcard_idSimcard, " +
                   "DateVikonano, " +
                   "Simcard_roumingcol_created, " +
                   "Users_idUsers, " +
                   "Comments, " +
                   "Rouming_tarif_idRouming_tarif" +
                   ") values(" +
                   "'" + idSimCard + "', " +
                   "'" + Convert.ToDateTime(SIM1Vikonano_dateTimePicker.Value.Date).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                   "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                   "'" + vars_form.user_login_id + "', " +
                   "'" + textBox_comments.Text + "', " +
                   "'" + RoumingTarifSIM1_comboBox.SelectedValue.ToString() + "'" +
                   ");");

                    macros.sql_command(string.Format("UPDATE btk.notification SET remayder_date ='null', remaynder_activate= 0 where idnotification=" + _id_notif + ";"));

                    DialogResult dialogResult = MessageBox.Show("Закрити заявку?", "Закрити?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                       

                        int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                        int police = checkBox_vizov_police.Checked ? 1 : 0;
                        macros.sql_command("insert into btk.alarm_ack(" +
                                            "alarm_text, " +
                                            "notification_idnotification, " +
                                            "Users_chenge, time_start_ack, " +
                                            "current_status_alarm, " +
                                            "vizov_police, " +
                                            "vizov_gmp) " +
                                            "values('Роумінг відключено', " +
                                            "'" + _id_notif + "'," +
                                            "'" + _user_login_id + "', " +
                                            "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            " 'Закрито', '" +
                                            "" + police + "', " +
                                            "'" + gmr + "'); UPDATE btk.notification SET Status = 'Закрито', time_stamp = now(), Users_idUsers='" + _user_login_id + "' WHERE idnotification = '" + _id_notif + "'OR group_alarm = '" + _id_notif + "'; ");
                        this.Close();
                    }
                    else
                    {
                        int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                        int police = checkBox_vizov_police.Checked ? 1 : 0;
                        macros.sql_command("insert into btk.alarm_ack(" +
                                            "alarm_text, " +
                                            "notification_idnotification, " +
                                            "Users_chenge, time_start_ack, " +
                                            "current_status_alarm, " +
                                            "vizov_police, " +
                                            "vizov_gmp) " +
                                            "values('Роумінг відключено', " +
                                            "'" + _id_notif + "'," +
                                            "'" + _user_login_id + "', " +
                                            "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            " 'Учетки', '" +
                                            "" + police + "', " +
                                            "'" + gmr + "'); UPDATE btk.notification SET Status = '" + comboBox_status_trevogi.SelectedItem.ToString() + "', time_stamp = now(), Users_idUsers='" + _user_login_id + "' WHERE idnotification = '" + _id_notif + "'OR group_alarm = '" + _id_notif + "'; ");
                        mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог
                    }
                }
                else
                {
                    macros.sql_command("insert into btk.Simcard_rouming (" +
                    "Simcard_idSimcard, " +
                    "Simcard_roumingcol_start, " +
                    "Simcard_roumingcol_end, " +
                    "Simcard_roumingcol_created, " +
                    "DateVikonano, " +
                    "Users_idUsers, " +
                    "Comments, " +
                    "Rouming_tarif_idRouming_tarif" +
                    ") values(" +
                    "'" + idSimCard + "', " +
                    "'" + Convert.ToDateTime(Rouming_SIM1_Start_dtp.Value).ToString("yyyy-MM-dd") + "', " +
                    "'" + Convert.ToDateTime(Rouming_SIM1_End_dtp.Value).ToString("yyyy-MM-dd") + "', " +
                    "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                    "'" + Convert.ToDateTime(SIM1Vikonano_dateTimePicker.Value.Date).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                    "'" + vars_form.user_login_id + "', " +
                    "'" + textBox_comments.Text + "', " +
                    "'" + RoumingTarifSIM1_comboBox.SelectedValue.ToString() + "'" +
                    ");");

                    macros.sql_command(string.Format("UPDATE btk.notification SET remayder_date ='" + Convert.ToDateTime(Rouming_SIM1_End_dtp.Value).ToString("yyyy-MM-dd") + "', remaynder_activate= 1 where idnotification=" + _id_notif + ";"));

                    int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                    int police = checkBox_vizov_police.Checked ? 1 : 0;
                    macros.sql_command("insert into btk.alarm_ack(" +
                                        "alarm_text, " +
                                        "notification_idnotification, " +
                                        "Users_chenge, time_start_ack, " +
                                        "current_status_alarm, " +
                                        "vizov_police, " +
                                        "vizov_gmp) " +
                                        "values('Роумінг підключено', " +
                                        "'" + _id_notif + "'," +
                                        "'" + _user_login_id + "', " +
                                        "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                        " 'Учетки', '" +
                                        "" + police + "', " +
                                        "'" + gmr + "'); UPDATE btk.notification SET Status = '" + comboBox_status_trevogi.SelectedItem.ToString() + "', time_stamp = now(), Users_idUsers='" + _user_login_id + "' WHERE idnotification = '" + _id_notif + "'OR group_alarm = '" + _id_notif + "'; ");
                    mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог
                }
            }
            else if (textBox_SIM1.Text.Contains("+882"))
            {
                // idSimCard = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number like '" + textBox_SIM1.Text.Remove(0, 4) + "';");
                if (idSimCard == "")
                {
                    MessageBox.Show("SIM не знайдено в базі, зверніться до 117");
                    return;
                }

                if (RoumingTarifSIM1_comboBox.Text == "VENBEST_UKRAINE_Basic_Kit") 
                {
                    string ICCID = macros.sql_command("SELECT Simcardcol_imsi FROM btk.Simcard where Simcardcol_number like '" + textBox_SIM1.Text.Remove(0, 1) + "';");
                    if (ICCID != "")
                    {
                        string t = macros.VodafoneSetServiceProfile(ICCID, "customerServiceProfile", RoumingTarifSIM1_comboBox.Text);
                        var respone = JsonConvert.DeserializeObject<RootObject>(t);
                        if (respone.setDeviceDetailsv4Response.@return.returnCode.majorReturnCode != "000")
                        {
                            MessageBox.Show("Error set: majorReturnCode: "+ respone.setDeviceDetailsv4Response.@return.returnCode.majorReturnCode + 
                                ", majorReturnCode: "+ respone.setDeviceDetailsv4Response.@return.returnCode.minorReturnCode + ".");
                            return;
                        }
                    }

                    macros.sql_command(string.Format("UPDATE btk.notification SET remayder_date ='null', remaynder_activate= 0 where idnotification=" + _id_notif + ";"));

                    macros.sql_command("insert into btk.Simcard_rouming (" +
                    "Simcard_idSimcard, " +
                    "Simcard_roumingcol_created, " +
                    "DateVikonano, " +
                    "Users_idUsers, " +
                    "Comments, " +
                    "Rouming_tarif_idRouming_tarif" +
                    ") values(" +
                    "'" + idSimCard + "', " +
                    "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                    "'" + Convert.ToDateTime(SIM1Vikonano_dateTimePicker.Value.Date).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                    "'" + vars_form.user_login_id + "', " +
                    "'" + textBox_comments.Text + "', " +
                    "'" + RoumingTarifSIM1_comboBox.SelectedValue.ToString() + "'" +
                    ");");

                    DialogResult dialogResult = MessageBox.Show("Закрити заявку?", "Закрити?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                        int police = checkBox_vizov_police.Checked ? 1 : 0;
                        macros.sql_command("insert into btk.alarm_ack(" +
                                            "alarm_text, " +
                                            "notification_idnotification, " +
                                            "Users_chenge, time_start_ack, " +
                                            "current_status_alarm, " +
                                            "vizov_police, " +
                                            "vizov_gmp) " +
                                            "values('Роумінг відключено', " +
                                            "'" + _id_notif + "'," +
                                            "'" + _user_login_id + "', " +
                                            "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            " 'Закрито', '" +
                                            "" + police + "', " +
                                            "'" + gmr + "'); UPDATE btk.notification SET Status = 'Закрито', time_stamp = now(), Users_idUsers='" + _user_login_id + "' WHERE idnotification = '" + _id_notif + "'OR group_alarm = '" + _id_notif + "'; ");
                        this.Close();
                    }
                    else
                    {
                        int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                        int police = checkBox_vizov_police.Checked ? 1 : 0;
                        macros.sql_command("insert into btk.alarm_ack(" +
                                            "alarm_text, " +
                                            "notification_idnotification, " +
                                            "Users_chenge, time_start_ack, " +
                                            "current_status_alarm, " +
                                            "vizov_police, " +
                                            "vizov_gmp) " +
                                            "values('Роумінг відключено', " +
                                            "'" + _id_notif + "'," +
                                            "'" + _user_login_id + "', " +
                                            "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            " 'Учетки', '" +
                                            "" + police + "', " +
                                            "'" + gmr + "'); UPDATE btk.notification SET Status = '" + comboBox_status_trevogi.SelectedItem.ToString() + "', time_stamp = now(), Users_idUsers='" + _user_login_id + "' WHERE idnotification = '" + _id_notif + "'OR group_alarm = '" + _id_notif + "'; ");
                        mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог
                    }

                }
                else
                {
                    string ICCID = macros.sql_command("SELECT Simcardcol_imsi FROM btk.Simcard where Simcardcol_number like '" + textBox_SIM1.Text.Remove(0, 1) + "';");
                    if (ICCID != "")
                    {
                        string t = macros.VodafoneSetServiceProfile(ICCID, "customerServiceProfile", RoumingTarifSIM1_comboBox.Text);
                        var respone = JsonConvert.DeserializeObject<RootObject>(t);
                        if (respone.setDeviceDetailsv4Response.@return.returnCode.majorReturnCode != "000")
                        {
                            MessageBox.Show("Error set: majorReturnCode: " + respone.setDeviceDetailsv4Response.@return.returnCode.majorReturnCode +
                                ", majorReturnCode: " + respone.setDeviceDetailsv4Response.@return.returnCode.minorReturnCode + ".");
                            return;
                        }
                    }
                    macros.sql_command("insert into btk.Simcard_rouming (" +
                    "Simcard_idSimcard, " +
                    "Simcard_roumingcol_start, " +
                    "Simcard_roumingcol_end, " +
                    "Simcard_roumingcol_created, " +
                    "DateVikonano, " +
                    "Users_idUsers, " +
                    "Comments, " +
                    "Rouming_tarif_idRouming_tarif" +
                    ") values(" +
                    "'" + idSimCard + "', " +
                    "'" + Convert.ToDateTime(Rouming_SIM1_Start_dtp.Value).ToString("yyyy-MM-dd") + "', " +
                    "'" + Convert.ToDateTime(Rouming_SIM1_End_dtp.Value).ToString("yyyy-MM-dd") + "', " +
                    "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                    "'" + Convert.ToDateTime(SIM1Vikonano_dateTimePicker.Value).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                    "'" + vars_form.user_login_id + "', " +
                    "'" + textBox_comments.Text + "', " +
                    "'" + RoumingTarifSIM1_comboBox.SelectedValue.ToString() + "'" +
                    ");");

                    macros.sql_command(string.Format("UPDATE btk.notification SET remayder_date ='" + Convert.ToDateTime(Rouming_SIM1_End_dtp.Value).ToString("yyyy-MM-dd") + "', remaynder_activate= 1 where idnotification=" + _id_notif + ";"));

                    int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                    int police = checkBox_vizov_police.Checked ? 1 : 0;
                    macros.sql_command("insert into btk.alarm_ack(" +
                                        "alarm_text, " +
                                        "notification_idnotification, " +
                                        "Users_chenge, time_start_ack, " +
                                        "current_status_alarm, " +
                                        "vizov_police, " +
                                        "vizov_gmp) " +
                                        "values('Роумінг підключено', " +
                                        "'" + _id_notif + "'," +
                                        "'" + _user_login_id + "', " +
                                        "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                        " 'Учетки', '" +
                                        "" + police + "', " +
                                        "'" + gmr + "'); UPDATE btk.notification SET Status = '" + comboBox_status_trevogi.SelectedItem.ToString() + "', time_stamp = now(), Users_idUsers='" + _user_login_id + "' WHERE idnotification = '" + _id_notif + "'OR group_alarm = '" + _id_notif + "'; ");
                    mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог
                } 
            }

            ReadRoumingHistory();
        }
        private void ReadRoumingHistory() => dataGridView_history.DataSource = macros.GetData("SELECT " +
                                                            "Simcardcol_number AS 'Номер'," +
                                                            "Simcard_roumingcol_start AS 'Початок'," +
                                                            "Simcard_roumingcol_end AS 'Кінець'," +
                                                            "TarifName as 'Тариф'," +
                                                            "OperatorName as 'Оператор', " +
                                                            "Users.username as 'Користувач', " +
                                                            "DateVikonano as 'Виконано', " +
                                                            "Simcard_roumingcol_created as 'Дата внесення' " +
                                                            "FROM " +
                                                            "btk.Simcard_rouming, " +
                                                            "btk.Simcard, " +
                                                            "btk.Users, " +
                                                            "btk.Rouming_tarif " +
                                                            //"btk.Object " +
                                                            "where " +
                                                            "(Simcard_rouming.Simcard_idSimcard = '" + idSimCard + "' " +
                                                            "or Simcard_rouming.Simcard_idSimcard = '" + idSim2Card + "') " +
                                                            "and Simcard.idSimcard = Simcard_rouming.Simcard_idSimcard " +
                                                            "and Users.idUsers = Simcard_rouming.Users_idUsers " +
                                                            "and Rouming_tarif.idRouming_tarif = Simcard_rouming.Rouming_tarif_idRouming_tarif " +
                                                            //"and Simcard_rouming.Simcard_idSimcard = Object.Simcard_idSimcard " +
                                                            //"and(Object.Objectcol_deleted != '1' OR Object.Objectcol_deleted IS NULL" +
                                                            ";");


        private void button_SIM2_Rouming_enter_Click(object sender, EventArgs e)
        {
            //string json = macros.WialonRequest("&svc=core/search_items&params={" +
            //                                        "\"spec\":{" +
            //                                        "\"itemsType\":\"avl_unit\"," +
            //                                        "\"propName\":\"sys_id\"," +
            //                                        "\"propValueMask\":\"" + wl_id + "\", " +
            //                                        "\"sortType\":\"sys_name\"," +
            //                                        "\"or_logic\":\"1\"}," +
            //                                        "\"force\":\"1\"," +
            //                                        "\"flags\":\"257\"," +
            //                                        "\"from\":\"0\"," +
            //                                        "\"to\":\"1\"}");
            //var m = JsonConvert.DeserializeObject<RootObject>(json);

            // idSim2Card = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number like '" + textBox_SIM2.Text.Remove(0, 1) + "';");
            if (idSim2Card == "")
            {
                MessageBox.Show("SIM не знайдено в базі, зверніться до 117");
                return;
            }

            if (RoumingTarifSIM2_comboBox.Text == "")
            {
                MessageBox.Show("Не вибрано тариф роумінгу");
                return;
            }

            if (Rouming_SIM2_Start_dtp.Value.Date >= Rouming_SIM2_End_dtp.Value.Date)
            {
                MessageBox.Show("Перевір дату");
                return;
            }

            if (textBox_SIM2.Text.Contains("+38067"))
            {
                if (RoumingTarifSIM2_comboBox.Text == "Украина (вимк.)")
                {
                    macros.sql_command("insert into btk.Simcard_rouming (" +
                   "Simcard_idSimcard, " +
                   "DateVikonano, " +
                   "Simcard_roumingcol_created, " +
                   "Users_idUsers, " +
                   "Comments, " +
                   "Rouming_tarif_idRouming_tarif" +
                   ") values(" +
                   "'" + idSimCard + "', " +
                   "'" + Convert.ToDateTime(SIM2Vikonano_dateTimePicker.Value.Date).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                   "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                   "'" + vars_form.user_login_id + "', " +
                   "'" + textBox_comments.Text + "', " +
                   "'" + RoumingTarifSIM2_comboBox.SelectedValue.ToString() + "'" +
                   ");");

                    macros.sql_command(string.Format("UPDATE btk.notification SET remayder_date ='null', remaynder_activate= 0 where idnotification=" + _id_notif + ";"));

                    DialogResult dialogResult = MessageBox.Show("Закрити заявку?", "Закрити?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {


                        int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                        int police = checkBox_vizov_police.Checked ? 1 : 0;
                        macros.sql_command("insert into btk.alarm_ack(" +
                                            "alarm_text, " +
                                            "notification_idnotification, " +
                                            "Users_chenge, time_start_ack, " +
                                            "current_status_alarm, " +
                                            "vizov_police, " +
                                            "vizov_gmp) " +
                                            "values('Роумінг відключено', " +
                                            "'" + _id_notif + "'," +
                                            "'" + _user_login_id + "', " +
                                            "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            " 'Закрито', '" +
                                            "" + police + "', " +
                                            "'" + gmr + "'); UPDATE btk.notification SET Status = 'Закрито', time_stamp = now(), Users_idUsers='" + _user_login_id + "' WHERE idnotification = '" + _id_notif + "'OR group_alarm = '" + _id_notif + "'; ");
                        this.Close();
                    }
                    else
                    {
                        int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                        int police = checkBox_vizov_police.Checked ? 1 : 0;
                        macros.sql_command("insert into btk.alarm_ack(" +
                                            "alarm_text, " +
                                            "notification_idnotification, " +
                                            "Users_chenge, time_start_ack, " +
                                            "current_status_alarm, " +
                                            "vizov_police, " +
                                            "vizov_gmp) " +
                                            "values('Роумінг відключено', " +
                                            "'" + _id_notif + "'," +
                                            "'" + _user_login_id + "', " +
                                            "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            " 'Учетки', '" +
                                            "" + police + "', " +
                                            "'" + gmr + "'); UPDATE btk.notification SET Status = '" + comboBox_status_trevogi.SelectedItem.ToString() + "', time_stamp = now(), Users_idUsers='" + _user_login_id + "' WHERE idnotification = '" + _id_notif + "'OR group_alarm = '" + _id_notif + "'; ");
                        mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог
                    }
                }
                else
                {
                    macros.sql_command("insert into btk.Simcard_rouming (" +
                    "Simcard_idSimcard, " +
                    "Simcard_roumingcol_start, " +
                    "Simcard_roumingcol_end, " +
                    "Simcard_roumingcol_created, " +
                    "DateVikonano, " +
                    "Users_idUsers, " +
                    "Comments, " +
                    "Rouming_tarif_idRouming_tarif" +
                    ") values(" +
                    "'" + idSimCard + "', " +
                    "'" + Convert.ToDateTime(Rouming_SIM2_Start_dtp.Value).ToString("yyyy-MM-dd") + "', " +
                    "'" + Convert.ToDateTime(Rouming_SIM2_End_dtp.Value).ToString("yyyy-MM-dd") + "', " +
                    "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                    "'" + Convert.ToDateTime(SIM2Vikonano_dateTimePicker.Value.Date).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                    "'" + vars_form.user_login_id + "', " +
                    "'" + textBox_comments.Text + "', " +
                    "'" + RoumingTarifSIM2_comboBox.SelectedValue.ToString() + "'" +
                    ");");

                    macros.sql_command(string.Format("UPDATE btk.notification SET remayder_date ='" + Convert.ToDateTime(Rouming_SIM2_End_dtp.Value).ToString("yyyy-MM-dd") + "', remaynder_activate= 1 where idnotification=" + _id_notif + ";"));

                    int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                    int police = checkBox_vizov_police.Checked ? 1 : 0;
                    macros.sql_command("insert into btk.alarm_ack(" +
                                        "alarm_text, " +
                                        "notification_idnotification, " +
                                        "Users_chenge, time_start_ack, " +
                                        "current_status_alarm, " +
                                        "vizov_police, " +
                                        "vizov_gmp) " +
                                        "values('Роумінг підключено', " +
                                        "'" + _id_notif + "'," +
                                        "'" + _user_login_id + "', " +
                                        "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                        " 'Учетки', '" +
                                        "" + police + "', " +
                                        "'" + gmr + "'); UPDATE btk.notification SET Status = '" + comboBox_status_trevogi.SelectedItem.ToString() + "', time_stamp = now(), Users_idUsers='" + _user_login_id + "' WHERE idnotification = '" + _id_notif + "'OR group_alarm = '" + _id_notif + "'; ");
                    mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог
                }
            }
            else if (textBox_SIM2.Text.Contains("+882"))
            {
                if (RoumingTarifSIM2_comboBox.Text == "VENBEST_UKRAINE_Basic_Kit")
                {
                    string ICCID = macros.sql_command("SELECT Simcardcol_imsi FROM btk.Simcard where Simcardcol_number like '" + textBox_SIM2.Text.Remove(0, 1) + "';");
                    if (ICCID != "")
                    {
                        string t = macros.VodafoneSetServiceProfile(ICCID, "customerServiceProfile", RoumingTarifSIM2_comboBox.Text);
                        var respone = JsonConvert.DeserializeObject<RootObject>(t);
                        if (respone.setDeviceDetailsv4Response.@return.returnCode.majorReturnCode != "000")
                        {
                            MessageBox.Show("Error set: majorReturnCode: " + respone.setDeviceDetailsv4Response.@return.returnCode.majorReturnCode +
                                ", majorReturnCode: " + respone.setDeviceDetailsv4Response.@return.returnCode.minorReturnCode + ".");
                            return;
                        }
                    }
                    
                    macros.sql_command(string.Format("UPDATE btk.notification SET remayder_date ='null', remaynder_activate= 0 where idnotification=" + _id_notif + ";"));

                    macros.sql_command("insert into btk.Simcard_rouming (" +
                    "Simcard_idSimcard, " +
                    "Simcard_roumingcol_created, " +
                    "DateVikonano, " +
                    "Users_idUsers, " +
                    "Comments, " +
                    "Rouming_tarif_idRouming_tarif" +
                    ") values(" +
                    "'" + idSimCard + "', " +
                    "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                    "'" + Convert.ToDateTime(SIM2Vikonano_dateTimePicker.Value.Date).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                    "'" + vars_form.user_login_id + "', " +
                    "'" + textBox_comments.Text + "', " +
                    "'" + RoumingTarifSIM2_comboBox.SelectedValue.ToString() + "'" +
                    ");");

                    DialogResult dialogResult = MessageBox.Show("Закрити заявку?", "Закрити?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                        int police = checkBox_vizov_police.Checked ? 1 : 0;
                        macros.sql_command("insert into btk.alarm_ack(" +
                                            "alarm_text, " +
                                            "notification_idnotification, " +
                                            "Users_chenge, time_start_ack, " +
                                            "current_status_alarm, " +
                                            "vizov_police, " +
                                            "vizov_gmp) " +
                                            "values('Роумінг відключено', " +
                                            "'" + _id_notif + "'," +
                                            "'" + _user_login_id + "', " +
                                            "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            " 'Закрито', '" +
                                            "" + police + "', " +
                                            "'" + gmr + "'); UPDATE btk.notification SET Status = 'Закрито', time_stamp = now(), Users_idUsers='" + _user_login_id + "' WHERE idnotification = '" + _id_notif + "'OR group_alarm = '" + _id_notif + "'; ");
                        this.Close();
                    }
                    else
                    {
                        int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                        int police = checkBox_vizov_police.Checked ? 1 : 0;
                        macros.sql_command("insert into btk.alarm_ack(" +
                                            "alarm_text, " +
                                            "notification_idnotification, " +
                                            "Users_chenge, time_start_ack, " +
                                            "current_status_alarm, " +
                                            "vizov_police, " +
                                            "vizov_gmp) " +
                                            "values('Роумінг відключено', " +
                                            "'" + _id_notif + "'," +
                                            "'" + _user_login_id + "', " +
                                            "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                            " 'Учетки', '" +
                                            "" + police + "', " +
                                            "'" + gmr + "'); UPDATE btk.notification SET Status = '" + comboBox_status_trevogi.SelectedItem.ToString() + "', time_stamp = now(), Users_idUsers='" + _user_login_id + "' WHERE idnotification = '" + _id_notif + "'OR group_alarm = '" + _id_notif + "'; ");
                        mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог
                    }
                }
                else
                {
                    string ICCID = macros.sql_command("SELECT Simcardcol_imsi FROM btk.Simcard where Simcardcol_number like '" + textBox_SIM2.Text.Remove(0, 1) + "';");
                    if (ICCID != "")
                    {
                        string t = macros.VodafoneSetServiceProfile(ICCID, "customerServiceProfile", RoumingTarifSIM2_comboBox.Text);
                        var respone = JsonConvert.DeserializeObject<RootObject>(t);
                        if (respone.setDeviceDetailsv4Response.@return.returnCode.majorReturnCode != "000")
                        {
                            MessageBox.Show("Error set: majorReturnCode: " + respone.setDeviceDetailsv4Response.@return.returnCode.majorReturnCode +
                                ", majorReturnCode: " + respone.setDeviceDetailsv4Response.@return.returnCode.minorReturnCode + ".");
                            return;
                        }
                    }
                    macros.sql_command("insert into btk.Simcard_rouming (" +
                    "Simcard_idSimcard, " +
                    "Simcard_roumingcol_start, " +
                    "Simcard_roumingcol_end, " +
                    "Simcard_roumingcol_created, " +
                    "DateVikonano, " +
                    "Users_idUsers, " +
                    "Comments, " +
                    "Rouming_tarif_idRouming_tarif" +
                    ") values(" +
                    "'" + idSimCard + "', " +
                    "'" + Convert.ToDateTime(Rouming_SIM2_Start_dtp.Value).ToString("yyyy-MM-dd") + "', " +
                    "'" + Convert.ToDateTime(Rouming_SIM2_End_dtp.Value).ToString("yyyy-MM-dd") + "', " +
                    "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                    "'" + Convert.ToDateTime(SIM2Vikonano_dateTimePicker.Value).ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                    "'" + vars_form.user_login_id + "', " +
                    "'" + textBox_comments.Text + "', " +
                    "'" + RoumingTarifSIM2_comboBox.SelectedValue.ToString() + "'" +
                    ");");

                    macros.sql_command(string.Format("UPDATE btk.notification SET remayder_date ='" + Convert.ToDateTime(Rouming_SIM2_End_dtp.Value).ToString("yyyy-MM-dd") + "', remaynder_activate= 1 where idnotification=" + _id_notif + ";"));

                    int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                    int police = checkBox_vizov_police.Checked ? 1 : 0;
                    macros.sql_command("insert into btk.alarm_ack(" +
                                        "alarm_text, " +
                                        "notification_idnotification, " +
                                        "Users_chenge, time_start_ack, " +
                                        "current_status_alarm, " +
                                        "vizov_police, " +
                                        "vizov_gmp) " +
                                        "values('Роумінг підключено', " +
                                        "'" + _id_notif + "'," +
                                        "'" + _user_login_id + "', " +
                                        "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                        " 'Учетки', '" +
                                        "" + police + "', " +
                                        "'" + gmr + "'); UPDATE btk.notification SET Status = '" + comboBox_status_trevogi.SelectedItem.ToString() + "', time_stamp = now(), Users_idUsers='" + _user_login_id + "' WHERE idnotification = '" + _id_notif + "'OR group_alarm = '" + _id_notif + "'; ");
                    mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог
                }
            }
            ReadRoumingHistory();
        }

        private void button_block_man_Click(object sender, EventArgs e)
        {
            if (StateBlockButton is true)
            {
                block_groupBox.Visible = false;
                StateBlockButton = false;
            }
            else
            {
                block_groupBox.Visible = true;
                StateBlockButton = true;
            }
        }

        private void service_button_Click(object sender, EventArgs e)
        {
            if (StateServiceButton is true)
            {
                Service_groupBox.Visible = false;
                StateServiceButton = false;
            }
            else
            {
                Service_groupBox.Visible = true;
                StateServiceButton = true;
            }
        }

        private void Autostart_button_Click(object sender, EventArgs e)
        {
            if (StateAutostartButton is true)
            {
                Autostart_groupBox.Visible = false;
                StateAutostartButton = false;
            }
            else
            {
                Autostart_groupBox.Visible = true;
                StateAutostartButton = true;
            }
        }


        private void block_eng_button_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Заблокувати?", "Блокування двигуна", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string json = macros.WialonRequest("&svc=core/search_item&params={"
                                                         + "\"id\":\"" + wl_id + "\","
                                                         + "\"flags\":\"2098177\"}");
                var test_out = JsonConvert.DeserializeObject<RootObject>(json);

                if (test_out != null)
                {
                    if (test_out.item.netconn == 0)
                    {
                        MessageBox.Show("Немає звязку, зачекай та спробуй ще");
                        return;
                    }
                    //get product of testing device
                    string get_produt_testing_device = macros.sql_command("select " +
                                                                           "products_has_Tarif.products_idproducts " +
                                                                           "from " +
                                                                           "btk.object_subscr, btk.Subscription, btk.products_has_Tarif, btk.Object " +
                                                                           "where " +
                                                                           "Object.Object_id_wl = " + wl_id + " and " +
                                                                           "Object.idObject=Object_idObject and " +
                                                                           "Subscription_idSubscr=idSubscr and " +
                                                                           "products_has_Tarif_idproducts_has_Tarif=idproducts_has_Tarif;");

                    if (get_produt_testing_device == "10" || get_produt_testing_device == "11" || get_produt_testing_device == "13" || get_produt_testing_device == "14" || get_produt_testing_device == "18" || get_produt_testing_device == "19" || get_produt_testing_device == "20" || get_produt_testing_device == "21")
                    {
                        string cmd = macros.WialonRequest("&svc=unit/exec_cmd&params={" +
                                                                    "\"itemId\":\"" + wl_id + "\"," +
                                                                    "\"commandName\":\"3 - СТОП двигатель\"," +
                                                                    "\"linkType\":\"tcp\"," +
                                                                    "\"param\":\"\"," +
                                                                    "\"timeout\":\"0\"," +
                                                                    "\"flags\":\"0\"}");
                        if (cmd.Contains("rror"))
                        { MessageBox.Show("Помилка, зачекай та спробуй ще"); return; }
                    }
                    else if (get_produt_testing_device == "2" || get_produt_testing_device == "3" || get_produt_testing_device == "6" || get_produt_testing_device == "12" || get_produt_testing_device == "17")
                    {
                        string cmd = macros.WialonRequest("&svc=unit/exec_cmd&params={" +
                                                                    "\"itemId\":\"" + wl_id + "\"," +
                                                                    "\"commandName\":\"2. СТОП Двигатель\"," +
                                                                    "\"linkType\":\"tcp\"," +
                                                                    "\"param\":\"\"," +
                                                                    "\"timeout\":\"0\"," +
                                                                    "\"flags\":\"0\"}");
                        if (cmd.Contains("rror"))
                        { MessageBox.Show("Помилка, зачекай та спробуй ще"); return; }
                    }
                    else
                    {
                        MessageBox.Show("Упс, ошибка, сообщить 117");
                    }
                    string _text = "Команду заблокувати відправлено. Оператор: "+ vars_form.user_login_name + "";
                    int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                    int police = checkBox_vizov_police.Checked ? 1 : 0;
                    macros.sql_command("insert into btk.alarm_ack(" +
                                        "alarm_text, " +
                                        "notification_idnotification, " +
                                        "Users_chenge, time_start_ack, " +
                                        "current_status_alarm, " +
                                        "vizov_police, " +
                                        "vizov_gmp) " +
                                        "values('" + _text + "', " +
                                        "'" + _id_notif + "'," +
                                        "'" + _user_login_id + "', " +
                                        "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                        " '" + comboBox_status_trevogi.Text + "', '" +
                                        "" + police + "', " +
                                        "'" + gmr + "');");

                    mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог

                    MessageBox.Show("Команду заблокувати відправлено!");
                }
                else 
                {
                    MessageBox.Show("Немає звязку, зачекай та спробуй ще");
                    return;
                }

                
            }
            else if (dialogResult == DialogResult.No)
            {
                
            }
            
        }

        private void unblock_eng_button_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Розблокувати?", "Блокування двигуна", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string json = macros.WialonRequest("&svc=core/search_item&params={"
                                                         + "\"id\":\"" + wl_id.ToString() + "\","
                                                         + "\"flags\":\"2098177\"}"); //
                var test_out = JsonConvert.DeserializeObject<RootObject>(json);

                if (test_out != null)
                {
                    if (test_out.item.netconn == 0)
                    {
                        MessageBox.Show("Немає звязку, зачекай та спробуй ще");
                        return;
                    }

                    //get product of testing device
                    string get_produt_testing_device = macros.sql_command("select " +
                                                                           "products_has_Tarif.products_idproducts " +
                                                                           "from " +
                                                                           "btk.object_subscr, btk.Subscription, btk.products_has_Tarif, btk.Object " +
                                                                           "where " +
                                                                           "Object.Object_id_wl = " + wl_id + " and " +
                                                                           "Object.idObject=Object_idObject and " +
                                                                           "Subscription_idSubscr=idSubscr and " +
                                                                           "products_has_Tarif_idproducts_has_Tarif=idproducts_has_Tarif;");

                    if (get_produt_testing_device == "10" || get_produt_testing_device == "11" || get_produt_testing_device == "13" || get_produt_testing_device == "14" || get_produt_testing_device == "18" || get_produt_testing_device == "19" || get_produt_testing_device == "20" || get_produt_testing_device == "21")
                    {
                        string cmd = macros.WialonRequest("&svc=unit/exec_cmd&params={" +
                                                                    "\"itemId\":\"" + wl_id + "\"," +
                                                                    "\"commandName\":\"3 - СТАРТ двигатель\"," +
                                                                    "\"linkType\":\"tcp\"," +
                                                                    "\"param\":\"\"," +
                                                                    "\"timeout\":\"0\"," +
                                                                    "\"flags\":\"0\"}");
                        if (cmd.Contains("rror"))
                        { MessageBox.Show("Помилка, зачекай та спробуй ще"); return; }
                    }
                    else if (get_produt_testing_device == "2" || get_produt_testing_device == "3" || get_produt_testing_device == "6" || get_produt_testing_device == "12" || get_produt_testing_device == "17")
                    {
                        string cmd = macros.WialonRequest("&svc=unit/exec_cmd&params={" +
                                                                    "\"itemId\":\"" + wl_id + "\"," +
                                                                    "\"commandName\":\"1. СТАРТ Двигатель\"," +
                                                                    "\"linkType\":\"tcp\"," +
                                                                    "\"param\":\"\"," +
                                                                    "\"timeout\":\"0\"," +
                                                                    "\"flags\":\"0\"}");
                        if (cmd.Contains("rror"))
                        { MessageBox.Show("Помилка, зачекай та спробуй ще"); return; }
                    }
                    else
                    {
                        MessageBox.Show("Упс, ошибка, сообщить 117");
                    }
                    string _text = "Команду розблокувати відправлено. Оператор: " + vars_form.user_login_name + "";
                    int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                    int police = checkBox_vizov_police.Checked ? 1 : 0;
                    macros.sql_command("insert into btk.alarm_ack(" +
                                        "alarm_text, " +
                                        "notification_idnotification, " +
                                        "Users_chenge, time_start_ack, " +
                                        "current_status_alarm, " +
                                        "vizov_police, " +
                                        "vizov_gmp) " +
                                        "values('" + _text + "', " +
                                        "'" + _id_notif + "'," +
                                        "'" + _user_login_id + "', " +
                                        "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                        " '" + comboBox_status_trevogi.Text + "', '" +
                                        "" + police + "', " +
                                        "'" + gmr + "');");

                    mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог
                    MessageBox.Show("Команду розблокувати відправлено!");
                }
                else
                {
                    MessageBox.Show("Немає звязку, зачекай та спробуй ще");
                    return;
                }


                
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }

            
        }

        private void ServiceOn_button_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Увімкнути?", "Сервісний режим", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string json = macros.WialonRequest("&svc=core/search_item&params={"
                                                         + "\"id\":\"" + wl_id.ToString() + "\","
                                                         + "\"flags\":\"2098177\"}"); //
                var test_out = JsonConvert.DeserializeObject<RootObject>(json);

                if (test_out != null)
                {
                    if (test_out.item.netconn == 0)
                    {
                        MessageBox.Show("Немає звязку, зачекай та спробуй ще");
                        return;
                    }

                    //get product of testing device
                    string get_produt_testing_device = macros.sql_command("select " +
                                                                           "products_has_Tarif.products_idproducts " +
                                                                           "from " +
                                                                           "btk.object_subscr, btk.Subscription, btk.products_has_Tarif, btk.Object " +
                                                                           "where " +
                                                                           "Object.Object_id_wl = " + wl_id + " and " +
                                                                           "Object.idObject=Object_idObject and " +
                                                                           "Subscription_idSubscr=idSubscr and " +
                                                                           "products_has_Tarif_idproducts_has_Tarif=idproducts_has_Tarif;");

                    if (get_produt_testing_device == "10" || get_produt_testing_device == "11" || get_produt_testing_device == "13" || get_produt_testing_device == "14" || get_produt_testing_device == "18" || get_produt_testing_device == "19" || get_produt_testing_device == "20" || get_produt_testing_device == "21")
                    {
                        string cmd = macros.WialonRequest("&svc=unit/exec_cmd&params={" +
                                                                    "\"itemId\":\"" + wl_id + "\"," +
                                                                    "\"commandName\":\"6 - Включить сервисный режим\"," +
                                                                    "\"linkType\":\"tcp\"," +
                                                                    "\"param\":\"\"," +
                                                                    "\"timeout\":\"0\"," +
                                                                    "\"flags\":\"0\"}");
                        if (cmd.Contains("rror"))
                        { MessageBox.Show("Помилка, зачекай та спробуй ще"); return; }
                    }
                    else if (get_produt_testing_device == "2" || get_produt_testing_device == "3" || get_produt_testing_device == "6" || get_produt_testing_device == "12" || get_produt_testing_device == "17")
                    {
                        MessageBox.Show("Обладнення не підтримує команду сервісний");
                    }
                    else
                    {
                        MessageBox.Show("Упс, ошибка, сообщить 117");
                    }

                    string _text = "Команду увімкнення сервісного режиму відправлено. Оператор: " + vars_form.user_login_name + "";
                    int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                    int police = checkBox_vizov_police.Checked ? 1 : 0;
                    macros.sql_command("insert into btk.alarm_ack(" +
                                        "alarm_text, " +
                                        "notification_idnotification, " +
                                        "Users_chenge, time_start_ack, " +
                                        "current_status_alarm, " +
                                        "vizov_police, " +
                                        "vizov_gmp) " +
                                        "values('" + _text + "', " +
                                        "'" + _id_notif + "'," +
                                        "'" + _user_login_id + "', " +
                                        "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                        " '" + comboBox_status_trevogi.Text + "', '" +
                                        "" + police + "', " +
                                        "'" + gmr + "');");

                    mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог
                    MessageBox.Show("Команду увімкнення сервісного режиму відправлено!");
                }
                else
                {
                    MessageBox.Show("Немає звязку, зачекай та спробуй ще");
                    return;
                }



            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        private void ServiceOff_button_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Вимкнути?", "Сервісний режим", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string json = macros.WialonRequest("&svc=core/search_item&params={"
                                                         + "\"id\":\"" + wl_id.ToString() + "\","
                                                         + "\"flags\":\"2098177\"}"); //
                var test_out = JsonConvert.DeserializeObject<RootObject>(json);

                if (test_out != null)
                {
                    if (test_out.item.netconn == 0)
                    {
                        MessageBox.Show("Немає звязку, зачекай та спробуй ще");
                        return;
                    }

                    //get product of testing device
                    string get_produt_testing_device = macros.sql_command("select " +
                                                                           "products_has_Tarif.products_idproducts " +
                                                                           "from " +
                                                                           "btk.object_subscr, btk.Subscription, btk.products_has_Tarif, btk.Object " +
                                                                           "where " +
                                                                           "Object.Object_id_wl = " + wl_id + " and " +
                                                                           "Object.idObject=Object_idObject and " +
                                                                           "Subscription_idSubscr=idSubscr and " +
                                                                           "products_has_Tarif_idproducts_has_Tarif=idproducts_has_Tarif;");

                    if (get_produt_testing_device == "10" || get_produt_testing_device == "11" || get_produt_testing_device == "13" || get_produt_testing_device == "14" || get_produt_testing_device == "18" || get_produt_testing_device == "19" || get_produt_testing_device == "20" || get_produt_testing_device == "21")
                    {
                        string cmd = macros.WialonRequest("&svc=unit/exec_cmd&params={" +
                                                                    "\"itemId\":\"" + wl_id + "\"," +
                                                                    "\"commandName\":\"6 - Выключить сервисный режиме\"," +
                                                                    "\"linkType\":\"tcp\"," +
                                                                    "\"param\":\"\"," +
                                                                    "\"timeout\":\"0\"," +
                                                                    "\"flags\":\"0\"}");
                        if (cmd.Contains("rror"))
                        { MessageBox.Show("Помилка, зачекай та спробуй ще"); return; }
                    }
                    else if (get_produt_testing_device == "2" || get_produt_testing_device == "3" || get_produt_testing_device == "6" || get_produt_testing_device == "12" || get_produt_testing_device == "17")
                    {
                        MessageBox.Show("Обладнення не підтримує команду сервісний");
                    }
                    else
                    {
                        MessageBox.Show("Упс, ошибка, сообщить 117");
                    }

                    string _text = "Команду вимкнення сервісного режиму відправлено. Оператор: " + vars_form.user_login_name + "";
                    int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                    int police = checkBox_vizov_police.Checked ? 1 : 0;
                    macros.sql_command("insert into btk.alarm_ack(" +
                                        "alarm_text, " +
                                        "notification_idnotification, " +
                                        "Users_chenge, time_start_ack, " +
                                        "current_status_alarm, " +
                                        "vizov_police, " +
                                        "vizov_gmp) " +
                                        "values('" + _text + "', " +
                                        "'" + _id_notif + "'," +
                                        "'" + _user_login_id + "', " +
                                        "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                        " '" + comboBox_status_trevogi.Text + "', '" +
                                        "" + police + "', " +
                                        "'" + gmr + "');");

                    mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог
                    MessageBox.Show("Команду вимкнення сервісного режиму відправлено!");
                }
                else
                {
                    MessageBox.Show("Немає звязку, зачекай та спробуй ще");
                    return;
                }



            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        private void AutostartOn_button_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Увімкнути?", "Автозапуск", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string json = macros.WialonRequest("&svc=core/search_item&params={"
                                                         + "\"id\":\"" + wl_id.ToString() + "\","
                                                         + "\"flags\":\"2098177\"}"); //
                var test_out = JsonConvert.DeserializeObject<RootObject>(json);

                if (test_out != null)
                {
                    if (test_out.item.netconn == 0)
                    {
                        MessageBox.Show("Немає звязку, зачекай та спробуй ще");
                        return;
                    }

                    //get product of testing device
                    string get_produt_testing_device = macros.sql_command("select " +
                                                                           "products_has_Tarif.products_idproducts " +
                                                                           "from " +
                                                                           "btk.object_subscr, btk.Subscription, btk.products_has_Tarif, btk.Object " +
                                                                           "where " +
                                                                           "Object.Object_id_wl = " + wl_id + " and " +
                                                                           "Object.idObject=Object_idObject and " +
                                                                           "Subscription_idSubscr=idSubscr and " +
                                                                           "products_has_Tarif_idproducts_has_Tarif=idproducts_has_Tarif;");

                    if (get_produt_testing_device == "10" || get_produt_testing_device == "11" || get_produt_testing_device == "13" || get_produt_testing_device == "14" || get_produt_testing_device == "18" || get_produt_testing_device == "19" || get_produt_testing_device == "20" || get_produt_testing_device == "21")
                    {
                        string cmd = macros.WialonRequest("&svc=unit/exec_cmd&params={" +
                                                                    "\"itemId\":\"" + wl_id + "\"," +
                                                                    "\"commandName\":\"2 - Автозапуск старт\"," +
                                                                    "\"linkType\":\"tcp\"," +
                                                                    "\"param\":\"\"," +
                                                                    "\"timeout\":\"0\"," +
                                                                    "\"flags\":\"0\"}");
                        if (cmd.Contains("rror"))
                        { MessageBox.Show("Помилка, зачекай та спробуй ще"); return; }
                    }
                    else if (get_produt_testing_device == "2" || get_produt_testing_device == "3" || get_produt_testing_device == "6" || get_produt_testing_device == "12" || get_produt_testing_device == "17")
                    {
                        MessageBox.Show("Обладнення не підтримує команду Автозапуск");
                    }
                    else
                    {
                        MessageBox.Show("Упс, ошибка, сообщить 117");
                    }

                    string _text = "Команду увімкнення автозапуску відправлено. Оператор: " + vars_form.user_login_name + "";
                    int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                    int police = checkBox_vizov_police.Checked ? 1 : 0;
                    macros.sql_command("insert into btk.alarm_ack(" +
                                        "alarm_text, " +
                                        "notification_idnotification, " +
                                        "Users_chenge, time_start_ack, " +
                                        "current_status_alarm, " +
                                        "vizov_police, " +
                                        "vizov_gmp) " +
                                        "values('" + _text + "', " +
                                        "'" + _id_notif + "'," +
                                        "'" + _user_login_id + "', " +
                                        "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                        " '" + comboBox_status_trevogi.Text + "', '" +
                                        "" + police + "', " +
                                        "'" + gmr + "');");

                    mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог
                    MessageBox.Show("Команду увімкнення автозапуску відправлено!");
                }
                else
                {
                    MessageBox.Show("Немає звязку, зачекай та спробуй ще");
                    return;
                }



            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        private void AutostartOff_button_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Вимкнути?", "Автозапуск", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string json = macros.WialonRequest("&svc=core/search_item&params={"
                                                         + "\"id\":\"" + wl_id.ToString() + "\","
                                                         + "\"flags\":\"2098177\"}"); //
                var test_out = JsonConvert.DeserializeObject<RootObject>(json);

                if (test_out != null)
                {
                    if (test_out.item.netconn == 0)
                    {
                        MessageBox.Show("Немає звязку, зачекай та спробуй ще");
                        return;
                    }

                    //get product of testing device
                    string get_produt_testing_device = macros.sql_command("select " +
                                                                           "products_has_Tarif.products_idproducts " +
                                                                           "from " +
                                                                           "btk.object_subscr, btk.Subscription, btk.products_has_Tarif, btk.Object " +
                                                                           "where " +
                                                                           "Object.Object_id_wl = " + wl_id + " and " +
                                                                           "Object.idObject=Object_idObject and " +
                                                                           "Subscription_idSubscr=idSubscr and " +
                                                                           "products_has_Tarif_idproducts_has_Tarif=idproducts_has_Tarif;");

                    if (get_produt_testing_device == "10" || get_produt_testing_device == "11" || get_produt_testing_device == "13" || get_produt_testing_device == "14" || get_produt_testing_device == "18" || get_produt_testing_device == "19" || get_produt_testing_device == "20" || get_produt_testing_device == "21")
                    {
                        string cmd = macros.WialonRequest("&svc=unit/exec_cmd&params={" +
                                                                    "\"itemId\":\"" + wl_id + "\"," +
                                                                    "\"commandName\":\"2 - Автозапуск стоп\"," +
                                                                    "\"linkType\":\"tcp\"," +
                                                                    "\"param\":\"\"," +
                                                                    "\"timeout\":\"0\"," +
                                                                    "\"flags\":\"0\"}");
                        if (cmd.Contains("rror"))
                        { MessageBox.Show("Помилка, зачекай та спробуй ще"); return; }
                    }
                    else if (get_produt_testing_device == "2" || get_produt_testing_device == "3" || get_produt_testing_device == "6" || get_produt_testing_device == "12" || get_produt_testing_device == "17")
                    {
                        MessageBox.Show("Обладнення не підтримує команду Автозапуск");
                    }
                    else
                    {
                        MessageBox.Show("Упс, ошибка, сообщить 117");
                    }

                    string _text = "Команду вимкнення автозапуску відправлено. Оператор: " + vars_form.user_login_name + "";
                    int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                    int police = checkBox_vizov_police.Checked ? 1 : 0;
                    macros.sql_command("insert into btk.alarm_ack(" +
                                        "alarm_text, " +
                                        "notification_idnotification, " +
                                        "Users_chenge, time_start_ack, " +
                                        "current_status_alarm, " +
                                        "vizov_police, " +
                                        "vizov_gmp) " +
                                        "values('" + _text + "', " +
                                        "'" + _id_notif + "'," +
                                        "'" + _user_login_id + "', " +
                                        "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                        " '" + comboBox_status_trevogi.Text + "', '" +
                                        "" + police + "', " +
                                        "'" + gmr + "');");

                    mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог
                    MessageBox.Show("Команду вимкнення автозапуску відправлено!");
                }
                else
                {
                    MessageBox.Show("Немає звязку, зачекай та спробуй ще");
                    return;
                }



            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }


        }
        private void ArmOn_button_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Закрити?", "Охорона", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string json = macros.WialonRequest("&svc=core/search_item&params={"
                                                         + "\"id\":\"" + wl_id.ToString() + "\","
                                                         + "\"flags\":\"2098177\"}"); //
                var test_out = JsonConvert.DeserializeObject<RootObject>(json);

                if (test_out != null)
                {
                    if (test_out.item.netconn == 0)
                    {
                        MessageBox.Show("Немає звязку, зачекай та спробуй ще");
                        return;
                    }

                    //get product of testing device
                    string get_produt_testing_device = macros.sql_command("select " +
                                                                           "products_has_Tarif.products_idproducts " +
                                                                           "from " +
                                                                           "btk.object_subscr, btk.Subscription, btk.products_has_Tarif, btk.Object " +
                                                                           "where " +
                                                                           "Object.Object_id_wl = " + wl_id + " and " +
                                                                           "Object.idObject=Object_idObject and " +
                                                                           "Subscription_idSubscr=idSubscr and " +
                                                                           "products_has_Tarif_idproducts_has_Tarif=idproducts_has_Tarif;");

                    if (get_produt_testing_device == "10" || get_produt_testing_device == "11" || get_produt_testing_device == "13" || get_produt_testing_device == "14" || get_produt_testing_device == "18" || get_produt_testing_device == "19" || get_produt_testing_device == "20" || get_produt_testing_device == "21")
                    {
                        string cmd = macros.WialonRequest("&svc=unit/exec_cmd&params={" +
                                                                    "\"itemId\":\"" + wl_id + "\"," +
                                                                    "\"commandName\":\"1 - Закрыть автомобиль\"," +
                                                                    "\"linkType\":\"tcp\"," +
                                                                    "\"param\":\"\"," +
                                                                    "\"timeout\":\"0\"," +
                                                                    "\"flags\":\"0\"}");
                        if (cmd.Contains("rror"))
                        { MessageBox.Show("Помилка, зачекай та спробуй ще"); return; }
                    }
                    else if (get_produt_testing_device == "2" || get_produt_testing_device == "3" || get_produt_testing_device == "6" || get_produt_testing_device == "12" || get_produt_testing_device == "17")
                    {
                        MessageBox.Show("Обладнення не підтримує команду Автозапуск");
                    }
                    else
                    {
                        MessageBox.Show("Упс, ошибка, сообщить 117");
                    }

                    string _text = "Команду закриття автомобіля відправлено. Оператор: " + vars_form.user_login_name + "";
                    int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                    int police = checkBox_vizov_police.Checked ? 1 : 0;
                    macros.sql_command("insert into btk.alarm_ack(" +
                                        "alarm_text, " +
                                        "notification_idnotification, " +
                                        "Users_chenge, time_start_ack, " +
                                        "current_status_alarm, " +
                                        "vizov_police, " +
                                        "vizov_gmp) " +
                                        "values('" + _text + "', " +
                                        "'" + _id_notif + "'," +
                                        "'" + _user_login_id + "', " +
                                        "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                        " '" + comboBox_status_trevogi.Text + "', '" +
                                        "" + police + "', " +
                                        "'" + gmr + "');");

                    mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог
                    MessageBox.Show("Команду закриття автомобіля відправлено!");
                }
                else
                {
                    MessageBox.Show("Немає звязку, зачекай та спробуй ще");
                    return;
                }



            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        private void ArmOff_button_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Відкрити?", "Охорона", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string json = macros.WialonRequest("&svc=core/search_item&params={"
                                                         + "\"id\":\"" + wl_id.ToString() + "\","
                                                         + "\"flags\":\"2098177\"}"); //
                var test_out = JsonConvert.DeserializeObject<RootObject>(json);

                if (test_out != null)
                {
                    if (test_out.item.netconn == 0)
                    {
                        MessageBox.Show("Немає звязку, зачекай та спробуй ще");
                        return;
                    }

                    //get product of testing device
                    string get_produt_testing_device = macros.sql_command("select " +
                                                                           "products_has_Tarif.products_idproducts " +
                                                                           "from " +
                                                                           "btk.object_subscr, btk.Subscription, btk.products_has_Tarif, btk.Object " +
                                                                           "where " +
                                                                           "Object.Object_id_wl = " + wl_id + " and " +
                                                                           "Object.idObject=Object_idObject and " +
                                                                           "Subscription_idSubscr=idSubscr and " +
                                                                           "products_has_Tarif_idproducts_has_Tarif=idproducts_has_Tarif;");

                    if (get_produt_testing_device == "10" || get_produt_testing_device == "11" || get_produt_testing_device == "13" || get_produt_testing_device == "14" || get_produt_testing_device == "18" || get_produt_testing_device == "19" || get_produt_testing_device == "20" || get_produt_testing_device == "21")
                    {
                        string cmd = macros.WialonRequest("&svc=unit/exec_cmd&params={" +
                                                                    "\"itemId\":\"" + wl_id + "\"," +
                                                                    "\"commandName\":\"1 - Открыть автомобиль\"," +
                                                                    "\"linkType\":\"tcp\"," +
                                                                    "\"param\":\"\"," +
                                                                    "\"timeout\":\"0\"," +
                                                                    "\"flags\":\"0\"}");
                        if (cmd.Contains("rror"))
                        { MessageBox.Show("Помилка, зачекай та спробуй ще"); return; }
                    }
                    else if (get_produt_testing_device == "2" || get_produt_testing_device == "3" || get_produt_testing_device == "6" || get_produt_testing_device == "12" || get_produt_testing_device == "17")
                    {
                        MessageBox.Show("Обладнення не підтримує команду Автозапуск");
                    }
                    else
                    {
                        MessageBox.Show("Упс, ошибка, сообщить 117");
                    }

                    string _text = "Команду відкриття автомобіля відправлено. Оператор: " + vars_form.user_login_name + "";
                    int gmr = checkBox_vizov_gmr.Checked ? 1 : 0;
                    int police = checkBox_vizov_police.Checked ? 1 : 0;
                    macros.sql_command("insert into btk.alarm_ack(" +
                                        "alarm_text, " +
                                        "notification_idnotification, " +
                                        "Users_chenge, time_start_ack, " +
                                        "current_status_alarm, " +
                                        "vizov_police, " +
                                        "vizov_gmp) " +
                                        "values('" + _text + "', " +
                                        "'" + _id_notif + "'," +
                                        "'" + _user_login_id + "', " +
                                        "'" + Convert.ToDateTime(dateTimePicker_nachalo_dejstvia.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                        " '" + comboBox_status_trevogi.Text + "', '" +
                                        "" + police + "', " +
                                        "'" + gmr + "');");

                    mysql_get_hronologiya_trivog();//Обновляем таблицу хронология обработки тревог
                    MessageBox.Show("Команду відкриття автомобіля відправлено!");
                }
                else
                {
                    MessageBox.Show("Немає звязку, зачекай та спробуй ще");
                    return;
                }



            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        private void detail_Activated(object sender, EventArgs e)
        {
            vars_form.MapFormIdObject = _search_id;
            vars_form.MapFormName = _unit_name;
        }

        private void Map_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Map_checkBox.Checked is true)
            {
                Map map = new Map();
                map.Show();
                vars_form.MapFormIdObject = _search_id;
                vars_form.MapFormName = _unit_name;
            }
            else
            {
                Map f = new Map();
                f.Close();
            }
        }

        private void arm_button_Click(object sender, EventArgs e)
        {
            if (StateArmtButton is true)
            {
                Arm_groupBox.Visible = false;
                StateArmtButton = false;
            }
            else
            {
                Arm_groupBox.Visible = true;
                StateArmtButton = true;
            }
        }

        private void AcceptRouming_button_Click(object sender, EventArgs e)
        {
            if (RoumingAccept == "True")
            {
                macros.sql_command("update " +
                    "btk.RoumingZayavka " +
                    "set " +
                    "RoumingZayavka.RoumingAccepted = 0 " +
                    " where " +
                    "notification_idnotification = '" + _id_notif + "'");

                macros.sql_command("update btk.notification set notification.RoumingAccept = 0 where idnotification = '" + _id_notif + "'");
                AcceptRouming_button.Text = "Погодити";
                AcceptRouming_button.BackColor = Color.Empty;
                RoumingAccept = "False";
                macros.sql_command("insert into btk.alarm_ack(" +
                                    "alarm_text, " +
                                    "notification_idnotification, " +
                                    "Users_chenge, time_start_ack, " +
                                    "current_status_alarm, " +
                                    "vizov_police, " +
                                    "vizov_gmp) " +
                                    "values('Роумінг скасовано', " +
                                    "'" + _id_notif + "'," +
                                    "'" + vars_form.user_login_id + "', " +
                                    "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                    " '" + "Роумінг" + "', " +
                                    "'0', " +
                                    "'0'); ");
            }
            else
            {
                macros.sql_command("update " +
                    "btk.RoumingZayavka " +
                    "set " +
                    "RoumingZayavka.RoumingAccepted = 1, " +
                    "StartDate = '" + Convert.ToDateTime(RoumingZapitStart_dateTimePicker.Value.Date).ToString("yyyy-MM-dd") + "'," +
                    "EndDate = '" + Convert.ToDateTime(RoumingZapitEnd_dateTimePicker.Value.Date).ToString("yyyy-MM-dd") + "'" +
                    " where " +
                    "notification_idnotification = '" + _id_notif + "'");

                //macros.sql_command("update btk.RoumingZayavka set RoumingZayavka.RoumingAccepted = 1 where notification_idnotification = '" + _id_notif + "'");
                macros.sql_command("update btk.notification set notification.RoumingAccept = 1 where idnotification = '" + _id_notif + "'");
                AcceptRouming_button.Text = "Погоджено";
                AcceptRouming_button.BackColor = Color.Green;
                RoumingAccept = "True";
                macros.sql_command("insert into btk.alarm_ack(" +
                                    "alarm_text, " +
                                    "notification_idnotification, " +
                                    "Users_chenge, time_start_ack, " +
                                    "current_status_alarm, " +
                                    "vizov_police, " +
                                    "vizov_gmp) " +
                                    "values('Роумінг погоджено', " +
                                    "'" + _id_notif + "'," +
                                    "'" + vars_form.user_login_id + "', " +
                                    "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                    " '" + "Роумінг" + "', " +
                                    "'0', " +
                                    "'0'); ");

                string json = macros.WialonRequest("&svc=core/search_item&params={"
                                                         + "\"id\":\"" + _search_id + "\","
                                                         + "\"flags\":\"8388873\"}"); //
                var test_out = JsonConvert.DeserializeObject<RootObject>(json);

                string project = "";
                if (test_out.item != null)
                {
                    foreach (var keyvalue in test_out.item.flds)
                    {
                        if (keyvalue.Value.n.Contains("Проект"))
                        {
                            project = keyvalue.Value.v.ToString();
                            break;
                        }
                    }
                }

                string Body = 
                    "<p>Добрий день!</p>" +
                    "<p>Роумінг погоджено!</p>" +
                    "<p>Project: " + project + "</p>" +
                    "<p>Name: " + _unit_name + "</p>" +
                    "<p>ID: " + textBox_IMEI.Text + "</p>" +
                    "<p>SIM1: " + textBox_SIM1.Text + "</p>" +
                    "<p>SIM2: " + textBox_SIM1.Text + "</p>" +
                    "<p>FROM: " + RoumingZapitStart_dateTimePicker.Value + "</p>" +
                    "<p>TO: " + RoumingZapitEnd_dateTimePicker.Value + "</p>";

                //Stroim spisok Komu otpravlayem
                DataTable GroupRecipient = macros.GetData("SELECT user_mail FROM btk.Users where dept_user = '113' and State = '1';");
                string Recipient = "";
                int Count = GroupRecipient.Rows.Count;
                foreach (DataRow row in GroupRecipient.Rows)
                {
                    if (Count == 1)
                    {
                        Recipient += "<" + row["user_mail"].ToString() + ">";
                    }
                    else if (Count > 1)
                    {
                        Recipient += "<" + row["user_mail"].ToString() + ">,";
                        Count--;
                    }
                    else if (Count == 0)
                    {
                        MessageBox.Show("Ups, Net Adresatov, naberi 117");
                    }
                }

                if (Recipient != "")
                { macros.send_mail_auto(Recipient, "Disp. Активацыя роумінгу. Обєкт: "+ _unit_name + " ID: " + textBox_IMEI.Text, Body); }

            }
            mysql_get_hronologiya_trivog();

        }

        private void Test_button_Click(object sender, EventArgs e)
        {
            string ee = macros.WialonRequest("&svc=core/get_hw_types&params={" +
                                                     "\"filterType\":\"id\"," +
                                                     "\"filterValue\":\"*250*\"," +
                                                     "\"includeType\":\"true\"," +
                                                     "\"ignoreRename\":\"true\"}");// Проверям существует ли данный номер в системе

            return;
            string json2 = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_resource\"," +
                                                     "\"propName\":\"sys_name\"," +
                                                     "\"propValueMask\":\"wialon\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"-1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"5\"}");//получаем текущее местоположение объекта

            string json3 = macros.WialonRequest("&svc=resource/get_notification_data&params={" +
                                                     "\"itemId\":\"12\"," +
                                                     "\"col\":\"13\"}");//получаем текущее местоположение объекта

            var test_out = JsonConvert.DeserializeObject<RootObject>(json3);
            var t = test_out.act[3].p.apps;
            List<int> TagIds = t.Split(',').Select(int.Parse).ToList();

            string outt = "";
            string Err = "";
            int count = 0;
            foreach (int elm in TagIds)
            {
                string json4 = macros.WialonRequest("&svc=core/search_item&params={" +
                                                     "\"id\":\"" + elm + "\"," +
                                                     "\"flags\":\"1\"}");//получаем текущее местоположение объекта
               
                
                if (json4.Contains("error"))
                { 
                    Err += elm + ", ";
                }
                else
                {
                    var test_out1 = JsonConvert.DeserializeObject<RootObject>(json4);
                    if (test_out1.item.nm.Contains("@"))
                    {
                        outt += test_out1.item.nm + ", ";
                        count++;
                    }
                }

            }

        }

        private void ParkingSave_button_Click(object sender, EventArgs e)
        {
            if (Parking1ExistInWL == 0)
            {
                //Создание Произвольного поля Паркинг 1
                string answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                                + "\"itemId\":\"" + _search_id + "\","
                                + "\"id\":\"0\","
                                + "\"callMode\":\"create\","
                                + "\"n\":\"8.1 Паркінг 1\","
                                + "\"v\":\"" + Parking1_textBox.Text.Replace("\"", "%5C%22") + "\"}");
            }
            else
            {
                //Обновление Произвольного поля Паркинг 1
                string json2 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                         "\"itemId\":\"" + _search_id + "\"," +
                                                         "\"id\":\"" + Parking1ExistInWL + "\"," +
                                                         "\"callMode\":\"update\"," +
                                                         "\"n\":\"8.1 Паркінг 1\"," +
                                                         "\"v\":\"" + Parking1_textBox.Text.Replace("\"", "%5C%22") + "\"}");//получаем датчики объекта
            }

            if (Parking2ExistInWL == 0)
            {
                //Создание Произвольного поля Паркинг 2
                string answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                                + "\"itemId\":\"" + _search_id + "\","
                                + "\"id\":\"0\","
                                + "\"callMode\":\"create\","
                                + "\"n\":\"8.2 Паркінг 2\","
                                + "\"v\":\"" + Parking2_textBox.Text.Replace("\"", "%5C%22") + "\"}");
            }
            else
            {
                //Обновление Произвольного поля Паркинг 2
                string json2 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                         "\"itemId\":\"" + _search_id + "\"," +
                                                         "\"id\":\"" + Parking2ExistInWL + "\"," +
                                                         "\"callMode\":\"update\"," +
                                                         "\"n\":\"8.2 Паркінг 2\"," +
                                                         "\"v\":\"" + Parking2_textBox.Text.Replace("\"", "%5C%22") + "\"}");//получаем датчики объекта
            }

            if (Parking3ExistInWL == 0)
            {
                //Создание Произвольного поля Паркинг 3
                string answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                                + "\"itemId\":\"" + _search_id + "\","
                                + "\"id\":\"0\","
                                + "\"callMode\":\"create\","
                                + "\"n\":\"8.3 Паркінг 3\","
                                + "\"v\":\"" + Parking3_textBox.Text.Replace("\"", "%5C%22") + "\"}");
            }
            else
            {
                //Обновление Произвольного поля Паркинг 3
                string json2 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                         "\"itemId\":\"" + _search_id + "\"," +
                                                         "\"id\":\"" + Parking3ExistInWL + "\"," +
                                                         "\"callMode\":\"update\"," +
                                                         "\"n\":\"8.3 Паркінг 3\"," +
                                                         "\"v\":\"" + Parking3_textBox.Text.Replace("\"", "%5C%22") + "\"}");//получаем датчики объекта
            }

            if (Parking4ExistInWL == 0)
            {
                //Создание Произвольного поля Паркинг 4
                string answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                                + "\"itemId\":\"" + _search_id + "\","
                                + "\"id\":\"0\","
                                + "\"callMode\":\"create\","
                                + "\"n\":\"8.4 Паркінг 4\","
                                + "\"v\":\"" + Parking4_textBox.Text.Replace("\"", "%5C%22") + "\"}");
            }
            else
            {
                //Обновление Произвольного поля Паркинг 4
                string json2 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                         "\"itemId\":\"" + _search_id + "\"," +
                                                         "\"id\":\"" + Parking4ExistInWL + "\"," +
                                                         "\"callMode\":\"update\"," +
                                                         "\"n\":\"8.4 Паркінг 4\"," +
                                                         "\"v\":\"" + Parking4_textBox.Text.Replace("\"", "%5C%22") + "\"}");//получаем датчики объекта
            }

            if (Parking5ExistInWL == 0)
            {
                //Создание Произвольного поля Паркинг 5
                string answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                                + "\"itemId\":\"" + _search_id + "\","
                                + "\"id\":\"0\","
                                + "\"callMode\":\"create\","
                                + "\"n\":\"8.5 Паркінг 5\","
                                + "\"v\":\"" + Parking5_textBox.Text.Replace("\"", "%5C%22") + "\"}");
            }
            else
            {
                //Обновление Произвольного поля Паркинг 5
                string json2 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                         "\"itemId\":\"" + _search_id + "\"," +
                                                         "\"id\":\"" + Parking5ExistInWL + "\"," +
                                                         "\"callMode\":\"update\"," +
                                                         "\"n\":\"8.4 Паркінг 5\"," +
                                                         "\"v\":\"" + Parking5_textBox.Text.Replace("\"", "%5C%22") + "\"}");//получаем датчики объекта
            }

            
            // Диалог закрываем заявку или нет при завершении работы с учетными записями
            on_end_account_job("Внесено зміни:\nПаркінг1:" + Parking1_textBox.Text + "\nПаркінг2: " + Parking2_textBox.Text + "\nПаркінг3: " + Parking3_textBox.Text + "\nПаркінг4: " + Parking4_textBox.Text + "\nПаркінг5: " + Parking5_textBox.Text);
            TreeView_zapolnyaem();
            MessageBox.Show("Збережено!");

        }

        private void listBox_activation_list_search_DoubleClick(object sender, EventArgs e)
        {
            if (listBox_activation_list_search.SelectedItem != null)
            {
                email_textBox.Text = listBox_activation_list_search.SelectedItem.ToString();
            }

        }
    }
    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
