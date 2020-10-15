using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Disp_WinForm
{
    public partial class Activation_Form : Form
    {
        private Macros macros = new Macros();
        private string id_new_user;
        private string date_chenge_pin;

        public Activation_Form()
        {
            InitializeComponent();
            Read_data();
            get_remaynder();
        }

        private void get_remaynder()
        {
            string sql2 = string.Format("select remaynder_activate from btk.Activation_object where idActivation_object=" + vars_form.id_db_object_for_activation + ";");
            string remaynder_activate = macros.sql_command(sql2);
            if (remaynder_activate == "True")
            {
                remaynder_checkBox.Checked = true;
                string sql = string.Format("select remayder_date from btk.Activation_object where idActivation_object=" + vars_form.id_db_activation_for_activation + ";");
                string remayder_date = macros.sql_command(sql);
                DateTime rem = Convert.ToDateTime(remayder_date);
                remaynder_dateTimePicker.Value = rem;
            }
        }

        private void Read_data()
        {
            if (vars_form.if_open_created_activation == 1)
            {
                load_form_for_zayavka();
            }
            else if (vars_form.if_open_created_activation == 0)
            {
                load_form_for_sengl_activation();
            }
        }

        private void load_form_for_sengl_activation()
        {
            string json = macros.WialonRequest("&svc=core/search_items&params={" +
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
                        if (VO_falilia == "" & VO_imya_phone == "")
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

        }

        //init command on load form
        private void load_form_for_zayavka()
        {
            string json = macros.WialonRequest("&svc=core/search_items&params={" +
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
                        if (VO_falilia == "" & VO_imya_phone == "")
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
            else
            {
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
            string tk_tested = macros.sql_command("SELECT alarm_button FROM btk.Activation_object where idActivation_object = '" + vars_form.id_db_activation_for_activation + "'");
            if (tk_tested == "1")
            { checkBox_tk_tested.Checked = true; }
            else if (tk_tested == "0" || tk_tested == "")
            { checkBox_tk_tested.Checked = false; }

            string pin_chenged = macros.sql_command("SELECT pin_chenged FROM btk.Activation_object where idActivation_object = '" + vars_form.id_db_activation_for_activation + "'");
            if (pin_chenged == "1")
            { 
                checkBox_pin_chenged.Checked = true;
                textBox_who_chenge_pin.ReadOnly = true;
                textBox_who_chenge_pin.Text = macros.sql_command("SELECT Who_chenge_pin FROM btk.Activation_object where idActivation_object = '" + vars_form.id_db_activation_for_activation + "'");
                date_chenge_pin = macros.sql_command("SELECT Date_chenge_pin FROM btk.Activation_object where idActivation_object = '" + vars_form.id_db_activation_for_activation + "'");
            }
            else if (pin_chenged == "0" || pin_chenged == "")
            { 
                checkBox_pin_chenged.Checked = false;
                textBox_who_chenge_pin.ReadOnly = false;
            }



            //load data from zayavki Zayavki.Zayavkicol_comment
            DataTable table2 = new DataTable();
            table2 = macros.GetData("SELECT " +
                "Sobstvennik_avto_neme ," +
                "Kontakt_name_avto_1 ," +
                "Kontakt_phone_avto_1 ," +
                "Kontakt_name_avto_2 ," +
                "Kontakt_phone_avto_2 ," +
                "email ," +
                "products.product_name ," +
                "Kontragenti.Kontragenti_short_name, " +
                "TS_info.TS_infocol_licence_plate, " +
                "Zayavki.Zayavkicol_VIN, " +
                "Zayavki.Zayavkicol_license_plate, " +
                "TS_info.TS_infocol_vin, " +
                "Kontragenti.kontragent_type_idkontragent_type, " +
                "Zayavki.Zayavkicol_comment " +
                "from " +
                "btk.Zayavki, btk.products, btk.Kontragenti, btk.TS_info, btk.testing_object, btk.Object " +
                "where " +
                "Zayavki.products_idproducts = products.idproducts " +
                "and Kontragenti.idKontragenti = Zayavki.Kontragenti_idKontragenti_zakazchik " +
                "and Zayavki.idZayavki = '" + vars_form.id_db_zayavki_for_activation + "' " +
                "and Zayavki.testing_object_idtesting_object = testing_object.idtesting_object " +
                "and testing_object.Object_idObject = Object.idObject " +
                "and Object.TS_info_idTS_info = TS_info.idTS_info " +
                ";");

            textBox_vlasnik.Text = table2.Rows[0][0].ToString();
            textBox_kont1.Text = table2.Rows[0][1].ToString();
            maskedTextBox_kont_phone1.Text = table2.Rows[0][2].ToString();
            textBox_kont2.Text = table2.Rows[0][3].ToString();
            maskedTextBox_cont_phone2.Text = table2.Rows[0][4].ToString();
            textBox_email.Text = table2.Rows[0][5].ToString();
            textBox_product.Text = table2.Rows[0][6].ToString();
            textBox_zayavka_comments.Text = table2.Rows[0][13].ToString();
            // Формируем название проекта в зависимости от типа контрагента и названии контрагента
            if (table2.Rows[0][12].ToString() == "1")
            { textBox_zamovnik.Text = "Дилер (" + table2.Rows[0][7].ToString() + ")"; }
            else if (table2.Rows[0][12].ToString() == "2")
            { textBox_zamovnik.Text = "СК (" + table2.Rows[0][7].ToString() + ")"; }
            else if (table2.Rows[0][12].ToString() == "3")
            { textBox_zamovnik.Text = "Роздріб (" + table2.Rows[0][7].ToString() + ")"; }
            else { textBox_zamovnik.Text = table2.Rows[0][7].ToString(); }

            
            // если номер не указан в завке , то смотрим в тестирование, если и там не указан то ставим пусто
            if (table2.Rows[0][10].ToString() == "")
            {
                if (table2.Rows[0][8].ToString() == "")
                { textBox_licence_plate.Text = ""; }
                else
                { textBox_licence_plate.Text = table2.Rows[0][8].ToString(); }
            }
            else { textBox_licence_plate.Text = table2.Rows[0][10].ToString(); }

            // если номер не указан в завке , то смотрим в тестирование, если и там не указан то ставим пусто
            if (table2.Rows[0][9].ToString() == "")
            {
                if (table2.Rows[0][11].ToString() == "")
                { textBox_vin_zayavka.Text = ""; }
                else
                { textBox_vin_zayavka.Text = table2.Rows[0][11].ToString(); }
            }
            else { textBox_vin_zayavka.Text = table2.Rows[0][9].ToString(); }

            ReadActivationChenges(vars_form.id_db_activation_for_activation);
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Закрити без збереження?", "Закрити?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        private void button_chenge_uvaga_Click(object sender, EventArgs e)
        {
            string json = macros.WialonRequest("&svc=item/update_name&params={" +
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
                    Read_data();

                    //log user action
                    macros.LogUserAction(vars_form.user_login_id, "Зміна поля Увага", name_object_current_textBox.Text, vars_form.id_db_object_for_activation, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));

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
                if (VO_falilia == "" & VO_imya_phone == "")
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
            vars_form.num_vo = 0;
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

        private void button_add_vo4_Click(object sender, EventArgs e)
        {
            vars_form.num_vo = 4;
            Kontacts Kontacts_form = new Kontacts();
            Kontacts_form.Activated += new EventHandler(form_vo_add_activated);
            Kontacts_form.FormClosed += new FormClosedEventHandler(form_vo_add_deactivated);
            Kontacts_form.Show();
        }

        private void button_add_vo5_Click(object sender, EventArgs e)
        {
            vars_form.num_vo = 5;
            Kontacts Kontacts_form = new Kontacts();
            Kontacts_form.Activated += new EventHandler(form_vo_add_activated);
            Kontacts_form.FormClosed += new FormClosedEventHandler(form_vo_add_deactivated);
            Kontacts_form.Show();
        }


        private void save_button_Click(object sender, EventArgs e)
        {
            if (name_obj_new_textBox.Text.Length >= 51)
            {
                MessageBox.Show("Відкорегуй нову назву обекта");
                return;
            }

            if (comboBox_activation_result.SelectedIndex == -1)
            {
                MessageBox.Show("Оберіть результат тестування");
                return;
            }
            else if ((comboBox_activation_result.SelectedIndex == 1 | comboBox_activation_result.SelectedIndex == 2) & textBox_comments.Text == "")
            {
                MessageBox.Show("Опишіть ситуацію в коментрі.");
                return;
            }
            // 
            //if ((comboBox_activation_result.SelectedIndex == 1 | comboBox_activation_result.SelectedIndex == 2) | textBox_comments.Text != "")
            //{
            //    macros.sql_command("insert into btk.activation_comments (" +
            //        "coments," +
            //        "date_insert," +
            //        "Activation_object_idActivation_object," +
            //        "Users_idUsers" +
            //        ") values (" +
            //        "'" + MySqlHelper.EscapeString(textBox_comments.Text) + "', " +
            //        "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
            //        "'" + vars_form.id_db_activation_for_activation + "'," +
            //        "'" + vars_form.user_login_id + "'" +
            //        "); ");
            //}

            //Загружаем произвольные поля объекта
            string json = macros.WialonRequest("&svc=core/search_items&params={" +
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
                string name_answer = macros.WialonRequest("&svc=item/update_name&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\","
                                                                + "\"name\":\"" + name_obj_new_textBox.Text + "\"}");

                macros.sql_command("UPDATE btk.Object " +
                               "SET " +
                               "Object_name = '" + name_obj_new_textBox.Text + "' " +
                               "WHERE " +
                               "idObject = '" + vars_form.id_db_object_for_activation + "';");
            }

            foreach (var keyvalue in object_data.items[0].flds)
            {
                if (keyvalue.Value.n.Contains("УВАГА"))
                {
                    if (keyvalue.Value.v != uvaga_textBox.Text)
                    {
                        //Chenge feild Uvaga
                        string json2 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                                 "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\"," +
                                                                 "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                                 "\"callMode\":\"update\"," +
                                                                 "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                                 "\"v\":\"" + uvaga_textBox.Text.Replace("\"", "%5C%22") + "\"}");
                    }
                }
                if (keyvalue.Value.n.Contains("Кодове "))
                {
                    if (keyvalue.Value.v != kodove_slovo_textBox.Text)
                    {
                        //Chenge feild Кодове слово
                        string json2 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
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
                            string json2 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                             "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\"," +
                                                             "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                             "\"callMode\":\"update\"," +
                                                             "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                             "\"v\":\"" + textBox_vo1.Text + "\"}");
                        }
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
                            string json2 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                             "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\"," +
                                                             "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                             "\"callMode\":\"update\"," +
                                                             "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                             "\"v\":\"" + textBox_vo2.Text + "\"}");
                        }
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
                            string json2 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                             "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\"," +
                                                             "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                             "\"callMode\":\"update\"," +
                                                             "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                             "\"v\":\"" + textBox_vo3.Text + "\"}");
                        }
                    }
                }


                if (keyvalue.Value.n.Contains("02 Проект"))
                {
                    string json2 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                    "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\"," +
                                                    "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                    "\"callMode\":\"update\"," +
                                                    "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                    "\"v\":\"" + textBox_zamovnik.Text + "\"}");
                }

                if (textBox_who_chenge_pin.ReadOnly is false)
                {
                    if (keyvalue.Value.n.Contains("4.2 Дата"))
                    {
                        string json2 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                        "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\"," +
                                                        "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                        "\"callMode\":\"update\"," +
                                                        "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                        "\"v\":\"" + DateTime.Now.Date.ToString() + "\"}");
                    }
                }
                if (textBox_who_chenge_pin.ReadOnly is false)
                {
                    if (keyvalue.Value.n.Contains("4.3 PIN-код"))
                    {
                        string json2 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                        "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\"," +
                                                        "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                        "\"callMode\":\"update\"," +
                                                        "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                        "\"v\":\"" + textBox_who_chenge_pin.Text + "\"}");
                    }
                }
                if (keyvalue.Value.n.Contains("9.1 Техпаспор"))
                {
                    string json2 = macros.WialonRequest("&svc=item/update_custom_field&params={" +
                                                    "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\"," +
                                                    "\"id\":\"" + keyvalue.Value.id + "\"," +
                                                    "\"callMode\":\"update\"," +
                                                    "\"n\":\"" + keyvalue.Value.n + "\"," +
                                                    "\"v\":\"" + textBox_vlasnik.Text + "\"}");
                }


                ReadActivationChenges(vars_form.id_db_activation_for_activation);
            }

            // insert/update activation

            if (vars_form.if_open_created_activation == 1)
            {
                if (textBox_who_chenge_pin.ReadOnly is true)
                {
                    macros.sql_command("update btk.Activation_object " +
                        "set " +
                        "Activation_date = '" + Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy-MM-dd") + "', " +
                        "Users_idUsers = '" + vars_form.user_login_id + "'," +
                        "Object_idObject = '" + vars_form.id_db_object_for_activation + "'," +
                        "Activation_objectcol_result = '" + comboBox_activation_result.GetItemText(comboBox_activation_result.SelectedItem) + "'," +
                        "new_name_obj = '" + MySqlHelper.EscapeString(name_obj_new_textBox.Text) + "'," +
                        "new_pole_uvaga = '" + MySqlHelper.EscapeString(uvaga_textBox.Text) + "'," +
                        "vo1 = '" + vars_form.transfer_vo1_vo_form + "'," +
                        "vo2 = '" + vars_form.transfer_vo2_vo_form + "'," +
                        "vo3 = '" + vars_form.transfer_vo3_vo_form + "'," +
                        "vo4 = '" + vars_form.transfer_vo4_vo_form + "'," +
                        "vo5 = '" + vars_form.transfer_vo5_vo_form + "'," +
                        "kodove_slovo = '" + MySqlHelper.EscapeString(kodove_slovo_textBox.Text) + "'," +
                        "alarm_button = '" + (checkBox_tk_tested.Checked ? "1" : "0") + "'," +
                        "pin_chenged = '" + (checkBox_pin_chenged.Checked ? "1" : "0") + "'," +
                        "comment = '" + textBox_comments.Text + "' " +
                        "where " +
                        "idActivation_object = '" + vars_form.id_db_activation_for_activation + "'" +
                        ";");
                }
                else
                {
                    macros.sql_command("update btk.Activation_object " +
                        "set " +
                        "Activation_date = '" + Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy-MM-dd") + "', " +
                        "Users_idUsers = '" + vars_form.user_login_id + "'," +
                        "Object_idObject = '" + vars_form.id_db_object_for_activation + "'," +
                        "Activation_objectcol_result = '" + comboBox_activation_result.GetItemText(comboBox_activation_result.SelectedItem) + "'," +
                        "new_name_obj = '" + MySqlHelper.EscapeString(name_obj_new_textBox.Text) + "'," +
                        "new_pole_uvaga = '" + MySqlHelper.EscapeString(uvaga_textBox.Text) + "'," +
                        "vo1 = '" + vars_form.transfer_vo1_vo_form + "'," +
                        "vo2 = '" + vars_form.transfer_vo2_vo_form + "'," +
                        "vo3 = '" + vars_form.transfer_vo3_vo_form + "'," +
                        "vo4 = '" + vars_form.transfer_vo4_vo_form + "'," +
                        "vo5 = '" + vars_form.transfer_vo5_vo_form + "'," +
                        "kodove_slovo = '" + MySqlHelper.EscapeString(kodove_slovo_textBox.Text) + "'," +
                        "alarm_button = '" + (checkBox_tk_tested.Checked ? "1" : "0") + "'," +
                        "pin_chenged = '" + (checkBox_pin_chenged.Checked ? "1" : "0") + "'," +
                        "Who_chenge_pin = '" + textBox_who_chenge_pin.Text + "'," +
                        "Date_chenge_pin = '" + Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy-MM-dd") + "'," +
                        "comment = '" + textBox_comments.Text + "' " +
                        "where " +
                        "idActivation_object = '" + vars_form.id_db_activation_for_activation + "'" +
                        ";");
                }

                string id_ts_info_fo_object_activation = macros.sql_command("select TS_info_idTS_info from btk.Object where idObject = '" + vars_form.id_db_object_for_activation + "'");

                if (textBox_licence_plate.Text != "")
                {
                    //update lic plate in db
                    macros.sql_command("update btk.TS_info set TS_infocol_licence_plate = '" + textBox_licence_plate.Text + "' where idTS_info = '" + id_ts_info_fo_object_activation + "';");

                    //update lic plate in WL
                    string pp25_answer = macros.WialonRequest("&svc=item/update_profile_field&params={"
                                                                   + "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\","
                                                                   + "\"n\":\"registration_plate\","
                                                                   + "\"v\":\"" + textBox_licence_plate.Text.Replace("\"", "%5C%22") + "\"}");
                }

                if (textBox_vin_zayavka.Text != "")
                {
                    //update VIN plate in WL
                    string pp28_answer = macros.WialonRequest("&svc=item/update_profile_field&params={"
                                                                   + "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\","
                                                                   + "\"n\":\"vin\","
                                                                   + "\"v\":\"" + textBox_vin_zayavka.Text.Replace("\"", "%5C%22") + "\"}");
                    //update VIN plate in db
                    macros.sql_command("update btk.TS_info set TS_infocol_vin = '" + textBox_vin_zayavka.Text + "' where idTS_info = '" + id_ts_info_fo_object_activation + "';");
                }

                //update хто тестував in WL
                string pp6_answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\","
                                                                + "\"id\":\"22\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"4.1.1 Оператор, що активував\","
                                                                + "\"v\":\"" + vars_form.user_login_name + "\"}");

                //update коли активації in WL
                string pp7_answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\","
                                                                + "\"id\":\"21\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"4.1 Дата активації\","
                                                                + "\"v\":\"" + DateTime.Now.Date + "\"}");



                ////через запятую перебираем все аккауты из тривив и добавляем в accounts для записи в виалон
                //string accounts = "";
                //for (int index1 = 0; index1 < treeView_user_accounts.Nodes[0].Nodes.Count; index1++)
                //{
                //    accounts = accounts + treeView_user_accounts.Nodes[0].Nodes[index1].Text + ", ";
                //}

                ////update коли тестував in WL
                //string pp8_answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                //                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\","
                //                                                + "\"id\":\"25\","
                //                                                + "\"callMode\":\"update\","
                //                                                + "\"n\":\"4.4 Обліковий запис WL\","
                //                                                + "\"v\":\"" + accounts.Replace("\"", "%5C%22") + "\"}");
                //Insert chenges
                InsertActivationChenges(
                    vars_form.id_db_activation_for_activation, 
                    vars_form.user_login_id,
                    Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"),
                    comboBox_activation_result.GetItemText(comboBox_activation_result.SelectedItem), 
                    name_obj_new_textBox.Text,
                    uvaga_textBox.Text,
                    textBox_vo1.Text,
                    textBox_vo2.Text,
                    textBox_vo3.Text,
                    textBox_vo4.Text,
                    kodove_slovo_textBox.Text,
                    checkBox_tk_tested.Checked ? 1 : 0,
                    checkBox_pin_chenged.Checked ? 1 : 0,
                    textBox_comments.Text,
                    textBox_who_chenge_pin.Text,
                    DateTime.Now.Date
                    );

                if (comboBox_activation_result.GetItemText(comboBox_activation_result.SelectedItem).Contains("Успішно"))
                {
                    string Subject = "707 Активовано успішно! VIN: " + textBox_vin_zayavka.Text + ", Обєкт: " + name_obj_new_textBox.Text + ", Проект: " + textBox_zamovnik.Text;
                    string recip = "<" + vars_form.user_login_email + ">," + "<o.pustovit@venbest.com.ua>,<d.lenik@venbest.com.ua>,<s.gregul@venbest.com.ua>,<a.lozinskiy@venbest.com.ua>,<mc@venbest.com.ua>,<e.remekh@venbest.com.ua>,<e.danilchenko@venbest.com.ua>,<a.andreasyan@venbest.com.ua>,<y.kravchenko@venbest.com.ua>,<a.andreasyan@venbest.com.ua>,<n.kovalenko@venbest.com.ua>";
                    DataTable dt = new DataTable();

                    dt.Columns.Add("Параметр");
                    dt.Columns.Add("Значення");
                    object[] row = { "VIN", textBox_vin_zayavka.Text };
                    dt.Rows.Add(row);
                    object[] row1 = { "Обєкт", name_obj_new_textBox.Text };
                    dt.Rows.Add(row1);
                    object[] row2 = { "IMEI", imei_object_textBox.Text };
                    dt.Rows.Add(row2);

                    object[] row11 = { "Дата дата активації", DateTime.Now.ToString() };
                    dt.Rows.Add(row11);
                    object[] row13 = { "Оператор що активував", vars_form.user_login_name };
                    dt.Rows.Add(row13);
                    object[] row3 = { "Статс активації", comboBox_activation_result.GetItemText(comboBox_activation_result.SelectedItem) };
                    dt.Rows.Add(row3);
                    object[] row14 = { "Коментар", textBox_comments.Text };
                    dt.Rows.Add(row14);

                    string Body = macros.ConvertDataTableToHTML(dt);

                    macros.send_mail(recip, Subject, Body);
                }
            }
            else if (vars_form.if_open_created_activation == 0)
            {
                if (textBox_who_chenge_pin.ReadOnly is true)
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
                                                                    "comment " +
                                                                    ") " +
                                                                    "values (" +
                                                                    "'" + Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy-MM-dd") + "'," +
                                                                    "'" + vars_form.user_login_id + "'," +
                                                                    "'" + vars_form.id_db_object_for_activation + "'," +
                                                                    "'" + comboBox_activation_result.GetItemText(comboBox_activation_result.SelectedItem) + "'," +
                                                                    "'" + MySqlHelper.EscapeString(name_obj_new_textBox.Text) + "'," +
                                                                    "'" + MySqlHelper.EscapeString(uvaga_textBox.Text) + "'," +
                                                                    "'" + vars_form.transfer_vo1_vo_form + "'," +
                                                                    "'" + vars_form.transfer_vo2_vo_form + "'," +
                                                                    "'" + vars_form.transfer_vo3_vo_form + "'," +
                                                                    "'" + vars_form.transfer_vo4_vo_form + "'," +
                                                                    "'" + vars_form.transfer_vo5_vo_form + "'," +
                                                                    "'" + MySqlHelper.EscapeString(kodove_slovo_textBox.Text) + "'," +
                                                                    "'" + (checkBox_tk_tested.Checked ? "1" : "0") + "'," +
                                                                    "'" + textBox_comments.Text + "'" +
                                                                    ");");
                }
                else
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
                                                                    "pin_chenged," +
                                                                    "Who_chenge_pin," +
                                                                    "Date_chenge_pin," +
                                                                    "comment " +
                                                                    ") " +
                                                                    "values (" +
                                                                    "'" + Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy-MM-dd") + "'," +
                                                                    "'" + vars_form.user_login_id + "'," +
                                                                    "'" + vars_form.id_db_object_for_activation + "'," +
                                                                    "'" + comboBox_activation_result.GetItemText(comboBox_activation_result.SelectedItem) + "'," +
                                                                    "'" + MySqlHelper.EscapeString(name_obj_new_textBox.Text) + "'," +
                                                                    "'" + MySqlHelper.EscapeString(uvaga_textBox.Text) + "'," +
                                                                    "'" + vars_form.transfer_vo1_vo_form + "'," +
                                                                    "'" + vars_form.transfer_vo2_vo_form + "'," +
                                                                    "'" + vars_form.transfer_vo3_vo_form + "'," +
                                                                    "'" + vars_form.transfer_vo4_vo_form + "'," +
                                                                    "'" + vars_form.transfer_vo5_vo_form + "'," +
                                                                    "'" + MySqlHelper.EscapeString(kodove_slovo_textBox.Text) + "'," +
                                                                    "'" + (checkBox_tk_tested.Checked ? "1" : "0") + "'," +
                                                                    "'" + (checkBox_pin_chenged.Checked ? "1" : "0") + "'," +
                                                                    "'" + textBox_who_chenge_pin.Text + "'," +
                                                                    "'" + Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy-MM-dd") + "'," +
                                                                    "'" + textBox_comments.Text + "'" +
                                                                    ");");
                }

                vars_form.id_db_activation_for_activation = macros.sql_command("SELECT MAX(idActivation_object) FROM btk.Activation_object;");

                string id_ts_info_fo_object_activation = macros.sql_command("select TS_info_idTS_info from btk.Object where idObject = '" + vars_form.id_db_object_for_activation + "'");
                if (textBox_licence_plate.Text != "")
                {
                    //update lic plate in db
                    macros.sql_command("update btk.TS_info set TS_infocol_licence_plate = '" + textBox_licence_plate.Text + "' where idTS_info = '" + id_ts_info_fo_object_activation + "';");

                    //update lic plate in WL
                    //Характеристики licence plate
                    string pp25_answer = macros.WialonRequest("&svc=item/update_profile_field&params={"
                                                                   + "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\","
                                                                   + "\"n\":\"registration_plate\","
                                                                   + "\"v\":\"" + textBox_licence_plate.Text.Replace("\"", "%5C%22") + "\"}");
                }

                if (textBox_vin_zayavka.Text != "")
                {
                    //update VIN in db
                    macros.sql_command("update btk.TS_info set TS_infocol_vin = '" + textBox_vin_zayavka.Text + "' where idTS_info = '" + id_ts_info_fo_object_activation + "';");

                    //update VIN in WL
                    string pp28_answer = macros.WialonRequest("&svc=item/update_profile_field&params={"
                                                                   + "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\","
                                                                   + "\"n\":\"vin\","
                                                                   + "\"v\":\"" + textBox_vin_zayavka.Text.Replace("\"", "%5C%22") + "\"}");

                    //update хто активації in WL
                    string pp6_answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                                                                    + "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\","
                                                                    + "\"id\":\"22\","
                                                                    + "\"callMode\":\"update\","
                                                                    + "\"n\":\"4.1.1 Оператор, що активував\","
                                                                    + "\"v\":\"" + vars_form.user_login_name + "\"}");
                }

                //update коли активації in WL
                string pp7_answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\","
                                                                + "\"id\":\"21\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"4.1 Дата активації\","
                                                                + "\"v\":\"" + DateTime.Now.Date + "\"}");

                ////через запятую перебираем все аккауты из тривив и добавляем в accounts для записи в виалон
                //string accounts = "";
                //for (int index1 = 0; index1 < treeView_user_accounts.Nodes[0].Nodes.Count; index1++)
                //{
                //    accounts = accounts + treeView_user_accounts.Nodes[0].Nodes[index1].Text + ", ";
                //}

                ////update коли тестував in WL
                //string pp8_answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                //                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_activation + "\","
                //                                                + "\"id\":\"25\","
                //                                                + "\"callMode\":\"update\","
                //                                                + "\"n\":\"4.4 Обліковий запис WL\","
                //                                                + "\"v\":\"" + accounts.Replace("\"", "%5C%22") + "\"}");

                //Insert chenges
                InsertActivationChenges(
                    vars_form.id_db_activation_for_activation,
                    vars_form.user_login_id,
                    Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"),
                    comboBox_activation_result.GetItemText(comboBox_activation_result.SelectedItem),
                    name_obj_new_textBox.Text,
                    uvaga_textBox.Text,
                    textBox_vo1.Text,
                    textBox_vo2.Text,
                    textBox_vo3.Text,
                    textBox_vo4.Text,
                    kodove_slovo_textBox.Text,
                    checkBox_tk_tested.Checked ? 1 : 0,
                    checkBox_pin_chenged.Checked ? 1 : 0,
                    textBox_comments.Text,
                    textBox_who_chenge_pin.Text,
                    DateTime.Now.Date
                    );

                

                if (comboBox_activation_result.GetItemText(comboBox_activation_result.SelectedItem) == "Успішно" || comboBox_activation_result.GetItemText(comboBox_activation_result.SelectedItem) == "Успішно(PIN)")
                {
                    string Subject = "707 Активовано успішно! VIN: " + textBox_vin_zayavka.Text + ", Обєкт: " + name_obj_new_textBox.Text + ", Проект: " + textBox_zamovnik.Text;
                    string recip = "<" + vars_form.user_login_email + ">," + "<o.pustovit@venbest.com.ua>,<d.lenik@venbest.com.ua>,<s.gregul@venbest.com.ua>,<a.lozinskiy@venbest.com.ua>,<mc@venbest.com.ua>,<e.remekh@venbest.com.ua>,<e.danilchenko@venbest.com.ua>,<a.andreasyan@venbest.com.ua>,<y.kravchenko@venbest.com.ua>,<a.andreasyan@venbest.com.ua>,<n.kovalenko@venbest.com.ua>";
                    DataTable dt = new DataTable();

                    dt.Columns.Add("Параметр");
                    dt.Columns.Add("Значення");
                    object[] row = { "VIN", textBox_vin_zayavka.Text };
                    dt.Rows.Add(row);
                    object[] row1 = { "Обєкт", name_obj_new_textBox.Text };
                    dt.Rows.Add(row1);
                    object[] row2 = { "IMEI", imei_object_textBox.Text };
                    dt.Rows.Add(row2);

                    object[] row11 = { "Дата дата активації", DateTime.Now.ToString() };
                    dt.Rows.Add(row11);
                    object[] row13 = { "Оператор що активував", vars_form.user_login_name };
                    dt.Rows.Add(row13);
                    object[] row3 = { "Статс активації", comboBox_activation_result.GetItemText(comboBox_activation_result.SelectedItem) };
                    dt.Rows.Add(row3);
                    object[] row14 = { "Коментар", textBox_comments.Text };
                    dt.Rows.Add(row14);

                    string Body = macros.ConvertDataTableToHTML(dt);

                    macros.send_mail(recip, Subject, Body);
                }
            }

            if (remaynder_checkBox.Checked == true)
            {
                string sql = string.Format("UPDATE btk.Activation_object SET remayder_date ='" + Convert.ToDateTime(remaynder_dateTimePicker.Value).ToString("yyyy-MM-dd HH:mm:ss") + "', remaynder_activate= 1 where idActivation_object=" + vars_form.id_db_activation_for_activation + ";");
                macros.sql_command(sql);
            }
            if (remaynder_checkBox.Checked == false)
            {
                string sql = string.Format("UPDATE btk.Activation_object SET remayder_date = null, remaynder_activate= 0 where idActivation_object=" + vars_form.id_db_activation_for_activation + ";");
                macros.sql_command(sql);
            }

            this.Close();
        }

        private void InsertActivationChenges(string idActivation_object, string idUsers, string date, string Activation_objectcol_result, string new_name_obj, string new_pole_uvaga, string vo1, string vo2, string vo3, string vo4, string kodove_slovo, int alarm_button, int pin_chenged, string comment, string _Who_chenge_pin, DateTime _Date_chenge_pin)
        {
            DataTable chenges = macros.GetData("insert into btk.Activation_chenges (" +
                "Activation_object_idActivation_object," +
                "Users_idUsers, " +
                "Date,Activation_objectcol_result, " +
                "new_name_obj, " +
                "new_pole_uvaga, " +
                "vo1," +
                "vo2," +
                "vo3," +
                "vo4," +
                "kodove_slovo," +
                "alarm_button," +
                "pin_chenged, " +
                "Who_chenge_pin, " +
                "Date_chenge_pin, " +
                "comment) " +
                "values('"+ 
                idActivation_object + "', '"+ 
                idUsers + "', '" + date + "', '" + 
                Activation_objectcol_result + "', '"+ 
                new_name_obj + "', '"+
                MySqlHelper.EscapeString(new_pole_uvaga) + "', '"+ 
                vo1 + "', '"+ 
                vo2 + "', '"+ 
                vo3 + "', '"+ 
                vo4 + "', '"+ 
                kodove_slovo + "', '"+ 
                alarm_button + "', '"+ 
                pin_chenged + "', '" +
                _Who_chenge_pin + "', '" +
                Convert.ToDateTime(_Date_chenge_pin).ToString("yyyy-MM-dd") + "', '" +
                comment + "'); ");
        }//insert chenges activation

        private void ReadActivationChenges(string idActivation_object)
        {
            DataTable RreadChenges = macros.GetData("SELECT Users.username as 'Змінив', Date as 'Дата', Activation_objectcol_result as 'Результат', comment as 'Коментар', new_name_obj as 'Назва', new_pole_uvaga as 'Увага', vo1 as 'ВО1', vo2 as 'ВО2', vo3 as 'ВО3', kodove_slovo as 'Кодове слово', alarm_button as 'ТК', pin_chenged as 'PIN' FROM btk.Activation_chenges, btk.Users where Users.idUsers=Activation_chenges.Users_idUsers and Activation_object_idActivation_object = '" + idActivation_object + "';");
            dataGridView_activation_chenges.DataSource = null;
            dataGridView_activation_chenges.DataSource = RreadChenges;

            //dataGridView_activation_chenges.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView_activation_chenges.Columns["PIN"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_activation_chenges.Columns["ТК"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_activation_chenges.Columns["Кодове слово"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_activation_chenges.Columns["ВО3"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_activation_chenges.Columns["ВО2"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_activation_chenges.Columns["ВО1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_activation_chenges.Columns["Увага"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_activation_chenges.Columns["Назва"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_activation_chenges.Columns["Коментар"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_activation_chenges.Columns["Коментар"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView_activation_chenges.Columns["Результат"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_activation_chenges.Columns["Дата"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_activation_chenges.Columns["Змінив"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void button_call_kont1_Click(object sender, EventArgs e)
        {
            string path = macros.GetProcessPath("microsip");
            if (path == "")
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

        private void button_name_obj_generate_Click(object sender, EventArgs e)
        {
            string id_db_object = macros.sql_command("select Object_idObject from btk.Activation_object where idActivation_object = '" + vars_form.id_db_activation_for_activation + "'");

            DataTable name = macros.GetData("select " +
            "TS_model.TS_modelcol_name_short," +
            "TS_brand.TS_brandcol_brand_short," +
            "TS_info.TS_infocol_licence_plate," +
            "Zayavki.Sobstvennik_avto_neme," +
            "products.product_name, " +
            "Kontragenti.Kontragenti_short_name, " +
            "Kontragenti.kontragent_type_idkontragent_type " +
            "from " +
            "btk.TS_model," +
            "btk.TS_brand," +
            "btk.TS_info," +
            "btk.Object," +
            "btk.products," +
            "btk.Zayavki," +
            "btk.Activation_object," +
            "btk.Kontragenti " +
            "where " +
            "Object.idObject = '" + id_db_object + "' " +
            "and Object.TS_info_idTS_info = TS_info.idTS_info " +
            "and TS_model.idTS_model = TS_info.TS_model_idTS_model " +
            "and TS_brand.idTS_brand = TS_info.TS_brand_idTS_brand " +
            "and Object.products_idproducts = products.idproducts " +
            "and Zayavki.Activation_object_idActivation_object = Activation_object.idActivation_object " +
            "and Activation_object.Object_idObject = Object.idObject " +
            "and Zayavki.Kontragenti_idKontragenti_zakazchik = Kontragenti.idKontragenti" +
            ";");
            string model = name.Rows[0][0].ToString();
            string brand = name.Rows[0][1].ToString();
            //string lic_pl = name.Rows[0][2].ToString();
            string zakazchik = "";
            if (name.Rows[0][6].ToString() == "2")
            { zakazchik = " (" + name.Rows[0][5].ToString() + ")"; }
            string sobstv = " (" + name.Rows[0][3].ToString() + ")";
            string product = " (" + name.Rows[0][4].ToString() + ")";
            name_obj_new_textBox.Text = brand + " " + model + " " + textBox_licence_plate.Text + sobstv + product + zakazchik;
        }

        private void name_obj_new_textBox_TextChanged(object sender, EventArgs e)
        {
            textBox_lenth_name.Text = (50 - name_obj_new_textBox.Text.Length).ToString();
            if (name_obj_new_textBox.Text.Length >= 51)
            {
                name_obj_new_textBox.BackColor = Color.LightPink;
            }
            else
            {
                name_obj_new_textBox.BackColor = Color.White;
            }
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

        private void textBox_who_chenge_pin_DoubleClick(object sender, EventArgs e)
        {
            textBox_who_chenge_pin.ReadOnly = false;
            checkBox_pin_chenged.Checked = false;
        }

        private void uvaga_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }
    }
}