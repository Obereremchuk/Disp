using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;



namespace Disp_WinForm
{     
    public partial class Testing : Form
    {
        private static System.Timers.Timer aTimer3;
        Macros macros = new Macros();

        public Testing()
        {
            InitializeComponent();
            Read_data();
            aTimer3 = new System.Timers.Timer();
            comboBox_ustanoshik_poisk_DropDown();
            build_list_color();
            get_data_testing_obj();
            timer();
            textBox_licence_plate.CharacterCasing = CharacterCasing.Upper;
            adaptation_UI_for_product();

            group_debug();
            
        }

        private void group_debug()
        {
            


        }

        private void Read_data()
        {
            if (vars_form.if_open_created_testing == 1)
            {
                button_write.Text = "Оновити";
                DataTable table = new DataTable();
                table = macros.GetData("SELECT * FROM btk.testing_object where idtesting_object = '" + vars_form.id_db_openning_testing + "';");
                
                if (table.Rows[0]["testing_objectcol_block"].ToString() == "1")
                { checkBox_test_zablocovano.Checked = true; }

                if (table.Rows[0]["testing_objectcol_alarm_door"].ToString() == "1")
                { checkBox__test_vzlom.Checked = true; }

                if (table.Rows[0]["testing_objectcol_udar"].ToString() == "1")
                { checkBox_test_du.Checked = true; }

                if (table.Rows[0]["testing_objectcol_tk"].ToString() == "1")
                { checkBox_test_tk.Checked = true; }

                if (table.Rows[0]["testing_objectcol_adress"].ToString() == "1")
                { checkBox_test_koordinati.Checked = true; }

                if (table.Rows[0]["testing_objectcol_arm_hood"].ToString() == "1")
                { checkBox_test_hood_in_arm.Checked = true; }

                if (table.Rows[0]["testing_objectcol_dop1"].ToString() == "1")
                { checkBox_dop_1.Checked = true; }

                if (table.Rows[0]["testing_objectcol_dop1"].ToString() == "1")
                { checkBox_dop_1.Checked = true; }

                if (table.Rows[0]["testing_objectcol_dop2"].ToString() == "1")
                { checkBox_test_dop_2.Checked = true; }

                if (table.Rows[0]["testing_objectcol_autostart"].ToString() == "1")
                { checkBox_test_autostart.Checked = true; }



            }
        }

        

        private void adaptation_UI_for_product()
        {
            //get product of testing device
            string get_produt_testing_device = macros.sql_command("select " +
                                                                   "products_has_Tarif.products_idproducts " +
                                                                   "from " +
                                                                   "btk.object_subscr, btk.Subscription, btk.products_has_Tarif, btk.Object " +
                                                                   "where " +
                                                                   "Object.Object_id_wl = " + vars_form.id_wl_object_for_test + " and " +
                                                                   "Object.idObject=Object_idObject and " +
                                                                   "Subscription_idSubscr=idSubscr and " +
                                                                   "products_has_Tarif_idproducts_has_Tarif=idproducts_has_Tarif;");

            if (get_produt_testing_device == "10" || get_produt_testing_device == "11")
            {
                textBox_device1.Enabled = false;
                checkBox_test_relay_plus.Enabled = false;
                label3.Enabled = false;
            }
            else if (get_produt_testing_device == "2" || get_produt_testing_device == "3")
            {
                checkBox_test_autostart.Enabled = false;
                button_autostart.Enabled = false;
                label27.Enabled = false;
                label26.Enabled = false;
                label29.Enabled = false;
                label30.Enabled = false;
                label_test_hood_arm.Enabled = false;
                label_auth.Enabled = false;
                label_ign.Enabled = false;
                label9.Enabled = false;
                label_test_dop_1.Enabled = false;
                label11.Enabled = false;
                label_test_dop_2.Enabled = false;
                label_autstart.Enabled = false;
            }
        }

        private void timer()
        {
            
            aTimer3.Interval = 2000;
            aTimer3.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer3.AutoReset = true;
            aTimer3.Enabled = true;
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new System.Timers.ElapsedEventHandler(OnTimedEvent)
                    , new object[] { sender, e });

                return; // в этом побочном потоке ничего не делаем больше
            }
            getobjwl();
        }

        private void comboBox_test_brand_DropDownClosed(object sender, EventArgs e)
        {
            string sql = string.Format("SELECT TS_modelcol_name, idTS_model FROM btk.TS_model where TS_brand_idTS_brand =" + comboBox_test_brand.SelectedValue.ToString() + ";");
            var temp = macros.GetData(sql);
            comboBox_test_model.DataSource = null;
            comboBox_test_model.DisplayMember = "TS_modelcol_name";
            comboBox_test_model.ValueMember = "idTS_model";
            comboBox_test_model.DataSource = temp;
        }//После выбора из списка необходимый бренд, по выбранному бренду запрашиваем и заполняем комбобокс моделями этого бренда

        private void comboBox_test_brand_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("SELECT TS_brandcol_brand, idTS_brand FROM btk.TS_brand;");
            var temp = macros.GetData(sql);
            comboBox_test_brand.DataSource = null;
            comboBox_test_brand.DisplayMember = "TS_brandcol_brand";
            comboBox_test_brand.ValueMember = "idTS_brand";
            comboBox_test_brand.DataSource = temp;
        }// При открытии комбобокса запрашиваем и отображаем перечень всех брендов

        //private void comboBox_test_production_date_DropDown(object sender, EventArgs e)
        //{
        //    comboBox_test_production_date.DataSource = Enumerable.Range(2006, 26).ToList();
        //    comboBox_test_production_date.SelectedIndex = comboBox_test_production_date.Items.IndexOf(DateTime.Now.Year);
        //}//При открытии комбобокса строим список годов с 2006+26, ивыбираем по умолчанию текущий год

        private void comboBox_ustanovshik_poisk_TextUpdate(object sender, EventArgs e)
        {
            string from_textbox = comboBox_ustanoshik_poisk.Text.ToString();
            //string sql = string.Format("SELECT concat(btk.Kontakti.Kontakti_imya, ' ' ,btk.Kontakti.Kontakti_familia) AS familia_imya, Phonebook_idPhonebook, Phonebook_idPhonebook1, Kontakti.idKontakti FROM btk.Kontakti where Kontakti.Kontakti_familia like '%" + comboBox_ustanoshik_poisk.Text.ToString() + "%' or Kontakti.Kontakti_imya like '%" + comboBox_ustanoshik_poisk.Text.ToString() + "%';");
            string sql = string.Format("SELECT concat(btk.Kontakti.Kontakti_imya, ' ' ,btk.Kontakti.Kontakti_familia, '  ', Phonebook.Phonebookcol_phone ) AS familia_imya, Kontakti.idKontakti FROM btk.Kontakti, btk.Phonebook where Kontakti.Phonebook_idPhonebook=Phonebook.idPhonebook and (Kontakti.Kontakti_familia like '%" + comboBox_ustanoshik_poisk.Text.ToString() + "%' or Kontakti.Kontakti_imya like '%" + comboBox_ustanoshik_poisk.Text.ToString() + "%' or Phonebook.Phonebookcol_phone like '%" + comboBox_ustanoshik_poisk.Text.ToString() + "%');");

            comboBox_ustanoshik_poisk.DataSource = macros.GetData(sql);
            comboBox_ustanoshik_poisk.DisplayMember = "familia_imya";
            comboBox_ustanoshik_poisk.ValueMember = "idKontakti";
            comboBox_ustanoshik_poisk.Text = from_textbox;
            comboBox_ustanoshik_poisk.SelectionStart = comboBox_ustanoshik_poisk.Text.Length;
            comboBox_ustanoshik_poisk.DroppedDown = true;

        }//Контекстный поиск при вводе в поле комбобокс ищем по совпадению имени или фамилии в базе контактов

        private void comboBox_ustanoshik_poisk_DropDown()//(object sender, EventArgs e)
        {
            comboBox_ustanoshik_poisk.Text = "";
            //string sql = string.Format("SELECT concat(btk.Kontakti.Kontakti_imya, ' ' ,btk.Kontakti.Kontakti_familia) AS familia_imya, Phonebook_idPhonebook, Phonebook_idPhonebook1, Kontakti.idKontakti FROM btk.Kontakti where Kontakti.Kontakti_familia like '%" + comboBox_ustanoshik_poisk.Text.ToString() + "%' or Kontakti.Kontakti_imya like '%" + comboBox_ustanoshik_poisk.Text.ToString() + "%';");
            string sql = string.Format("SELECT concat(btk.Kontakti.Kontakti_imya, ' ' ,btk.Kontakti.Kontakti_familia, '  ', Phonebook.Phonebookcol_phone ) AS familia_imya, Kontakti.idKontakti FROM btk.Kontakti, btk.Phonebook where Kontakti.Phonebook_idPhonebook=Phonebook.idPhonebook and (Kontakti.Kontakti_familia like '%" + comboBox_ustanoshik_poisk.Text.ToString() + "%' or Kontakti.Kontakti_imya like '%" + comboBox_ustanoshik_poisk.Text.ToString() + "%' or Phonebook.Phonebookcol_phone like '%" + comboBox_ustanoshik_poisk.Text.ToString() + "%');");
            comboBox_ustanoshik_poisk.DisplayMember = "familia_imya";
            comboBox_ustanoshik_poisk.ValueMember = "idKontakti";
            comboBox_ustanoshik_poisk.DataSource = macros.GetData(sql);


            string sql2 = string.Format("select Kontragenti.idKontragenti, concat(btk.Kontragenti.Kontragenti_short_name, '(',btk.Kontragenti.Kontragenti_full_name, ')') AS full_short  FROM btk.Kontragenti  join btk.Kontakti_has_Kontragenti on Kontakti_has_Kontragenti.Kontragenti_idKontragenti= Kontragenti.idKontragenti AND Kontakti_has_Kontragenti.Kontakti_idKontakti = " + comboBox_ustanoshik_poisk.SelectedValue.ToString() + "");
            comboBox_test_sto.DisplayMember = "full_short";
            comboBox_test_sto.ValueMember = "idKontragenti";
            comboBox_test_sto.DataSource = macros.GetData(sql2);
        }//При открытии комбобокса запрашиваются и отображаются все контакты

        private void comboBox_ustanoshik_poisk_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBox_ustanoshik_poisk.SelectedValue == null)
            {
                comboBox_test_sto.Text = "Вибрати або створити установника";
                return;
            }
            string sql = string.Format("select Kontragenti.idKontragenti, concat(btk.Kontragenti.Kontragenti_short_name, '(',btk.Kontragenti.Kontragenti_full_name, ')') AS full_short  FROM btk.Kontragenti  join btk.Kontakti_has_Kontragenti on Kontakti_has_Kontragenti.Kontragenti_idKontragenti= Kontragenti.idKontragenti AND Kontakti_has_Kontragenti.Kontakti_idKontakti = " + comboBox_ustanoshik_poisk.SelectedValue.ToString() + "");
            comboBox_test_sto.DisplayMember = "full_short";
            comboBox_test_sto.ValueMember = "idKontragenti";
            comboBox_test_sto.DataSource = macros.GetData(sql);
        }//Выбрав необходимый контакт запрашиваем контрагента к кому он относится и предлагаем выбор в комбобоксе

        private void button_Kontakts_Click(object sender, EventArgs e)
        {
            Kontacts form_Kontakts = new Kontacts();
            form_Kontakts.Activated += new EventHandler(form_Kontakts_activated);
            form_Kontakts.FormClosed += new FormClosedEventHandler(form_Kontakts_deactivated);
            form_Kontakts.Show();
        }
        private void form_Kontakts_activated(object sender, EventArgs e)
        {
            this.Visible = false;// блокируем окно контрагентов пока открыто окно добавления контрагента
        }
        private void form_Kontakts_deactivated(object sender, FormClosedEventArgs e)
        {
            comboBox_ustanoshik_poisk_DropDown();
            this.Visible = true;// разблокируем окно контрагентов кактолько закрыто окно добавления контрагента
        }

        private void button_Kontagents_Click(object sender, EventArgs e)
        {
            Kontragents form_Kontragents = new Kontragents();
            form_Kontragents.Activated += new EventHandler(form_Kontragents_activated);
            form_Kontragents.FormClosed += new FormClosedEventHandler(form_Kontragents_deactivated);
            form_Kontragents.Show();
        }
        private void form_Kontragents_activated(object sender, EventArgs e)
        {
            this.Visible = false;// блокируем окно контрагентов пока открыто окно добавления контрагента
        }
        private void form_Kontragents_deactivated(object sender, FormClosedEventArgs e)
        {
            comboBox_ustanoshik_poisk_DropDown();
            this.Visible = true;// разблокируем окно контрагентов кактолько закрыто окно добавления контрагента
        }

        private void build_list_color()
        {
            // Строим список цветов авто - Имя=цвет, Значение=айди цвета
            this.comboBox_color.DataSource = macros.GetData("SELECT idColor, Colorcol_name FROM btk.Color;");
            this.comboBox_color.DisplayMember = "Colorcol_name";
            this.comboBox_color.ValueMember = "idColor";

            // Строим список брен авто - Имя=бренд, Значение=айди
            this.comboBox_test_brand.DataSource = macros.GetData("SELECT idTS_brand, TS_brandcol_brand FROM btk.TS_brand;");
            this.comboBox_test_brand.DisplayMember = "TS_brandcol_brand";
            this.comboBox_test_brand.ValueMember = "idTS_brand";

            // Строим список модель авто - Имя=модель, Значение=айди
            this.comboBox_test_model.DataSource = macros.GetData("SELECT idTS_model, TS_modelcol_name FROM btk.TS_model;");
            this.comboBox_test_model.DisplayMember = "TS_modelcol_name";
            this.comboBox_test_model.ValueMember = "idTS_model";

            // Строим список тип кузова авто - Имя=кузов, Значение=айди
            this.comboBox_kuzov_type.DataSource = macros.GetData("SELECT idKuzov_type, Kuzov_typecol_name FROM btk.Kuzov_type;");
            this.comboBox_kuzov_type.DisplayMember = "Kuzov_typecol_name";
            this.comboBox_kuzov_type.ValueMember = "idKuzov_type";

            // Строим список год выпуска авто - Имя=год, Значение=айди
            this.comboBox_test_production_date.DataSource = macros.GetData("SELECT idProduction_date, Production_datecol_date FROM btk.Production_date ORDER BY Production_datecol_date DESC;");
            this.comboBox_test_production_date.DisplayMember = "Production_datecol_date";
            this.comboBox_test_production_date.ValueMember = "idProduction_date";

        }// Строим список цветов авто - Имя=цвет, Значение=айди цвета

        private void get_data_testing_obj()
        {
            string name_obj = macros.sql_command("SELECT Object_name FROM btk.Object where idObject = " + vars_form.id_db_object_for_test + ";");
            name_obj_textBox.Text = name_obj;
            imei_obj_textBox.Text = macros.sql_command("SELECT Object_imei FROM btk.Object where idObject = " + vars_form.id_db_object_for_test + ";");

            string id_db_TS_info = macros.sql_command("SELECT TS_info_idTS_info FROM btk.Object where Object_id_wl = " + vars_form.id_wl_object_for_test + ";");

            


            if (id_db_TS_info == "1")
            {
                macros.sql_command("insert into btk.TS_info(Kuzov_type_idKuzov_type, Color_idColor, Production_date_idProduction_date, TS_brand_idTS_brand, TS_model_idTS_model, Kontakti_idKontakti, Kontragenti_idKontragenti)values('1', '1', '1', '1', '1', '1', '1');");
                string TS_infoID = macros.sql_command("SELECT MAX(idTS_info) FROM btk.TS_info;");

                macros.sql_command("update btk.Object set TS_info_idTS_info=" + TS_infoID + " where Object_id_wl=" + vars_form.id_wl_object_for_test + "");
            }//Если не установленно значения в информации об автомобиле - устанавливаем дефоллтніе значения

            DataTable db_TS_info = macros.GetData("SELECT * FROM btk.TS_info where idTS_info = "+ id_db_TS_info +"; ");


            textBox_device1.Text = db_TS_info.Rows[0]["TS_infocol_place_treker"].ToString();
            TextBox_device2.Text = db_TS_info.Rows[0]["TS_infocol_place_alarm"].ToString();
            textBox_place_relay.Text = db_TS_info.Rows[0]["TS_infocol_place_relay"].ToString();
            textBox_wire_cut.Text = db_TS_info.Rows[0]["TS_infocol_wire_cut"].ToString();
            textBox_service_button.Text = db_TS_info.Rows[0]["TS_infocol_place_service_button"].ToString();
            textBox_buttons_for_pin.Text = db_TS_info.Rows[0]["TS_infocol_button_for_pin"].ToString();
            textBox_other_alarm.Text = db_TS_info.Rows[0]["TS_infocol_other_alarm"].ToString();
            textBox_licence_plate.Text = db_TS_info.Rows[0]["TS_infocol_licence_plate"].ToString();
            textBox_vin.Text = db_TS_info.Rows[0]["TS_infocol_vin"].ToString();
            textBox_arm_bagagnik.Text = db_TS_info.Rows[0]["TS_infocol_arm_bagagnik"].ToString();
            textBox_instaled_sensor.Text = db_TS_info.Rows[0]["TS_infocol_other_sensor"].ToString();
            textBox_uvaga.Text = db_TS_info.Rows[0]["TS_infocol_uvaga"].ToString();
            comboBox__tk_wirreless_or_wire.Text = db_TS_info.Rows[0]["TS_infocol_wireless_tk"].ToString();
            textBox_pin_button_external.Text = db_TS_info.Rows[0]["TS_infocol_pin_button_external"].ToString();

            int m = Convert.ToInt16(db_TS_info.Rows[0]["TS_model_idTS_model"]);
            comboBox_test_model.SelectedValue = m;


            int b = Convert.ToInt16(db_TS_info.Rows[0]["TS_brand_idTS_brand"]);
            comboBox_test_brand.SelectedValue = b;

            int k = Convert.ToInt16(db_TS_info.Rows[0]["Kuzov_type_idKuzov_type"]);
            comboBox_kuzov_type.SelectedValue = k;

            int p = Convert.ToInt16(db_TS_info.Rows[0]["Production_date_idProduction_date"]);
            comboBox_test_production_date.SelectedValue = p;

            
            //DataTable dt = (DataTable)comboBox_ustanoshik_poisk.DataSource;
            DataTable dt = comboBox_ustanoshik_poisk.DataSource as DataTable;

            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["idKontakti"].ToString() == db_TS_info.Rows[0]["Kontakti_idKontakti"].ToString())
                {
                    comboBox_ustanoshik_poisk.SelectedIndex = i;
                    comboBox_ustanoshik_poisk_SelectionChangeCommitted(comboBox_ustanoshik_poisk, new EventArgs());
                }
                i++;
            }
            i = 0;
            //DataTable dtt = (DataTable)comboBox_test_sto.DataSource;
            DataTable dtt = comboBox_test_sto.DataSource as DataTable;

            if (dtt.Rows.Count >= 1)
            {
                foreach (DataRow dr in dtt.Rows)
                {
                    if (dr["idKontragenti"].ToString() == db_TS_info.Rows[0]["Kontragenti_idKontragenti"].ToString())
                    {
                        comboBox_test_sto.SelectedIndex = i;
                        break;
                    }
                i++;
                }
            }
            

            //textBox_commets.Text =
            textBox_place_tk.Text = db_TS_info.Rows[0]["TS_infocol_place_tk"].ToString();
            textBox_current_pin.Text = db_TS_info.Rows[0]["TS_infocol_new_pin"].ToString();
            textBox_komfort.Text = db_TS_info.Rows[0]["TS_infocol_comfort_accsess"].ToString();
            int c = Convert.ToInt16(db_TS_info.Rows[0]["Color_idColor"]);
            comboBox_color.SelectedIndex = c - 1;

            if (db_TS_info.Rows[0]["TS_infocol_relay_on_plus"].ToString() == "1")
            {
                checkBox_test_relay_plus.Checked = true;
            }

            ////Временное код. Для получения данніх про обїект из виалона

            //string item_id_in = "&svc=core/search_item&params={"
            //                    + "\"id\":\"" + vars_form.id_object_for_test + "\","
            //                    + "\"flags\":\"8388745\"}";

            //string json2 = macros.wialon_request_new(item_id_in);
            //RootObject rootobject = JsonConvert.DeserializeObject<RootObject>(json2);

        }
        private void button_write_Click(object sender, EventArgs e)
        {
            if (comboBox_kuzov_type.SelectedIndex == -1
                || comboBox_color.SelectedIndex == -1
                || comboBox_test_production_date.SelectedIndex == -1
                || comboBox_test_brand.SelectedIndex == -1
                || comboBox_test_model.SelectedIndex == -1
                || comboBox_ustanoshik_poisk.SelectedIndex == -1
                || comboBox_test_sto.SelectedIndex == -1)
            {
                MessageBox.Show("Перевірь щось, можливо зір..");
                return;
            }


            commands_fill_anketa();
            commands_add_testing();
        }

        private void commands_fill_anketa()
        {
            

            string id_db_TS_info = macros.sql_command("SELECT TS_info_idTS_info FROM btk.Object where Object_id_wl = " + vars_form.id_wl_object_for_test + ";");

            /////////////////
            ///Заполняем произвольные поля в БД
            /////////////////
            ///

            

            macros.sql_command("update btk.TS_info set " +
                               "TS_infocol_place_treker='" + textBox_device1.Text + "', " +
                               "TS_infocol_place_alarm='" + TextBox_device2.Text + "', " +
                               "TS_infocol_place_relay='" + textBox_place_relay.Text + "', " +
                               "TS_infocol_wire_cut='" + textBox_wire_cut.Text + "', " +
                               "TS_infocol_place_tk='" + textBox_place_tk.Text + "', " +
                               "TS_infocol_wireless_tk='" + comboBox__tk_wirreless_or_wire.GetItemText(comboBox__tk_wirreless_or_wire.SelectedItem).ToString() + "', " +
                               "TS_infocol_place_service_button='" + textBox_service_button.Text + "', " +
                               "TS_infocol_button_for_pin='" + textBox_buttons_for_pin.Text + "', " +
                               "TS_infocol_set_pin='" + textBox_current_pin.Text + "', " +
                               "TS_infocol_pin_button_external='" + textBox_pin_button_external.Text + "', " +
                               "TS_infocol_other_alarm='" + textBox_other_alarm.Text + "', " +
                               "TS_infocol_other_sensor='" + textBox_instaled_sensor.Text + "', " +
                               "TS_infocol_arm_bagagnik='" + textBox_arm_bagagnik.Text + "', " +
                               "TS_infocol_uvaga='" + textBox_uvaga.Text + "', " +
                               "Kuzov_type_idKuzov_type='" + comboBox_kuzov_type.SelectedValue.ToString() + "', " +
                               "Color_idColor='" + comboBox_color.SelectedValue.ToString() + "', " +
                               "Production_date_idProduction_date='" + comboBox_test_production_date.SelectedValue.ToString() + "', " +
                               "TS_infocol_licence_plate='" + textBox_licence_plate.Text + "', " +
                               "TS_brand_idTS_brand='" + comboBox_test_brand.SelectedValue.ToString() + "', " +
                               "TS_model_idTS_model='" + comboBox_test_model.SelectedValue.ToString() + "', " +
                               "TS_infocol_vin='" + textBox_vin.Text.ToString() + "', " +
                               "Kontakti_idKontakti='" + comboBox_ustanoshik_poisk.SelectedValue.ToString() + "', " +
                               "Kontragenti_idKontragenti='" + comboBox_test_sto.SelectedValue.ToString() + "', " +
                               "TS_infocol_relay_on_plus='" + (checkBox_test_relay_plus.Checked ? "1" : "0") + "', " +
                               "TS_infocol_comfort_accsess='" + textBox_komfort.Text.ToString() + "', " +
                               "TS_infocol_new_pin='" + textBox_current_pin.Text.ToString() + "' " +
                               "where idTS_info=" + id_db_TS_info + ";");

            macros.sql_command("update btk.Object set " +
                               "Object_name='" + name_obj_textBox.Text + "'" +
                               "where idObject =" + vars_form.id_db_object_for_test + ";");

            //get product of testing device
            string get_produt_testing_device = macros.sql_command("select " +
                                                                   "products_has_Tarif.products_idproducts " +
                                                                   "from " +
                                                                   "btk.object_subscr, btk.Subscription, btk.products_has_Tarif, btk.Object " +
                                                                   "where " +
                                                                   "Object.Object_id_wl = " + vars_form.id_wl_object_for_test + " and " +
                                                                   "Object.idObject=Object_idObject and " +
                                                                   "Subscription_idSubscr=idSubscr and " +
                                                                   "products_has_Tarif_idproducts_has_Tarif=idproducts_has_Tarif;");

            if (get_produt_testing_device=="10" || get_produt_testing_device=="11" || get_produt_testing_device == "13" || get_produt_testing_device == "14")
            {

                string json = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                         + "\"id\":\"" + vars_form.id_wl_object_for_test + "\","
                                                         + "\"flags\":\"‭‭‭‭5257‬‬‬‬\"}"); //
                var test_out = JsonConvert.DeserializeObject<RootObject>(json);


                //Меняем имя об"екта
                string name_answer = macros.wialon_request_lite("&svc=item/update_name&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"name\":\"" + name_obj_textBox.Text.Replace("\"", "%5C%22") + "\"}");



                //Произвольное поле  name operator
                string pp6_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"6\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"3.1.1 Оператор, що тестував\","
                                                                + "\"v\":\"" + vars_form.user_login_name + "\"}");

                //Произвольное поле 0 УВАГА
                string pp1_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"1\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"0 УВАГА\","
                                                                + "\"v\":\"" + textBox_uvaga.Text.Replace("\"", "%5C%22") + "\"}");

                //Произвольное поле  other alarm
                string pp7_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                               + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                               + "\"id\":\"7\","
                                                               + "\"callMode\":\"update\","
                                                               + "\"n\":\"3.11 Додатково встановлені сигналізації\","
                                                               + "\"v\":\"" + textBox_other_alarm.Text.Replace("\"", "%5C%22") + "\"}");

                //Произвольное поле  в охрану с багажника
                string pp8_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                               + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                               + "\"id\":\"8\","
                                                               + "\"callMode\":\"update\","
                                                               + "\"n\":\"3.12 Постановка авто под охрану через багажник?\","
                                                               + "\"v\":\"" + textBox_arm_bagagnik.Text.Replace("\"", "%5C%22") + "\"}");

                //Произвольное поле  в охрану с багажника
                string pp9_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"9\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"3.15 Додатково встановлені датчики\","
                                                                + "\"v\":\"" + textBox_instaled_sensor.Text.Replace("\"", "%5C%22") + "\"}");


                //Произвольное поле Установщик
                string pp10_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"10\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"3.2.2 Установник - монтажник: ПІБ, №тел.\","
                                                                  + "\"v\":\"" + comboBox_ustanoshik_poisk.GetItemText(this.comboBox_ustanoshik_poisk.SelectedItem).ToString() + "\"}");

                //Произвольное поле СТО
                string pp11_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"11\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"3.2.1 Установник: назва, адреса\","
                                                                + "\"v\":\"" + comboBox_test_sto.GetItemText(this.comboBox_test_sto.SelectedItem).ToString() + "\"}");

                //Произвольное поле дата установки = дата тестирования
                string pp12_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"12\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"3.3 Дата установки\","
                                                                + "\"v\":\"" + DateTime.Now.Date + "\"}");

                //Произвольное поле место установки сигналызації
                string pp13_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"13\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"3.4 Місце установки пристрою ВЕНБЕСТ\","
                                                                + "\"v\":\"" + TextBox_device2.Text.Replace("\"", "%5C%22") + "\"}");
                
                //Произвольное поле place relay
                string pp14_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                              + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                              + "\"id\":\"14\","
                                                              + "\"callMode\":\"update\","
                                                              + "\"n\":\"3.6.1  Реле блокування: місце встановлення\","
                                                              + "\"v\":\"" + textBox_place_relay.Text.Replace("\"", "%5C%22") + "\"}");
                //Произвольное поле wire cut
                string pp15_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                              + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                              + "\"id\":\"15\","
                                                              + "\"callMode\":\"update\","
                                                              + "\"n\":\"3.6.2 Реле блокування: елемент блокування\","
                                                              + "\"v\":\"" + textBox_wire_cut.Text.Replace("\"", "%5C%22") + "\"}");

                //Произвольное поле service button
                string pp16_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                               + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                               + "\"id\":\"16\","
                                                               + "\"callMode\":\"update\","
                                                               + "\"n\":\"3.7 Місце встановлення сервісної кнопки\","
                                                               + "\"v\":\"" + textBox_service_button.Text.Replace("\"", "%5C%22") + "\"}");

                    // Произвольное поле place wireles tk
                string pp17_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                               + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                               + "\"id\":\"17\","
                                                               + "\"callMode\":\"update\","
                                                               + "\"n\":\"3.8.1 Тривожна кнопка: провідна, безпровідна\","
                                                               + "\"v\":\"\"}");
                //Произвольное поле place tk
                string pp18_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                              + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                              + "\"id\":\"18\","
                                                              + "\"callMode\":\"update\","
                                                              + "\"n\":\"3.8.2 Місце установки тривожної кнопки\","
                                                              + "\"v\":\"" + textBox_place_tk.Text.Replace("\"", "%5C%22") + "\"}");
                
                
                //Произвольное поле  button fo pin
                string pp20_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                              + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                              + "\"id\":\"20\","
                                                              + "\"callMode\":\"update\","
                                                              + "\"n\":\"3.9.2 Штатні кнопки введення PIN-коду\","
                                                              + "\"v\":\"" + textBox_buttons_for_pin.Text.Replace("\"", "%5C%22") + "\"}");

                //Произвольное поле новій ПИН
                string pp21_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"24\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"4.3 PIN-код встановлено особою(клієнт / установлник)\","
                                                                + "\"v\":\"" + textBox_current_pin.Text.Replace("\"", "%5C%22") + "\"}");

                //Произвольное поле Гарантія до
                string pp211_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"28\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"5.3 Гарантія до\","
                                                                + "\"v\":\"" + (DateTime.Now.Date.AddYears(1)).AddDays(-1).ToString() + "\"}");

                //Характеристики kuzov type
                string pp22_answer = macros.wialon_request_lite("&svc=item/update_profile_field&params={"
                                                              + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                              + "\"n\":\"vehicle_type\","
                                                              + "\"v\":\"" + comboBox_kuzov_type.GetItemText(this.comboBox_kuzov_type.SelectedItem).ToString() + "\"}");
                //Характеристики color
                string pp23_answer = macros.wialon_request_lite("&svc=item/update_profile_field&params={"
                                                              + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                              + "\"n\":\"color\","
                                                              + "\"v\":\"" + comboBox_color.GetItemText(this.comboBox_color.SelectedItem).ToString() + "\"}");
                //Характеристики prod date
                string pp24_answer = macros.wialon_request_lite("&svc=item/update_profile_field&params={"
                                                               + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                               + "\"n\":\"year\","
                                                               + "\"v\":\"" + comboBox_test_production_date.GetItemText(this.comboBox_test_production_date.SelectedItem).ToString() + "\"}");
                //Характеристики licence plate
                string pp25_answer = macros.wialon_request_lite("&svc=item/update_profile_field&params={"
                                                               + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                               + "\"n\":\"registration_plate\","
                                                               + "\"v\":\"" + textBox_licence_plate.Text.Replace("\"", "%5C%22") + "\"}");
                //Характеристики brend
                string pp26_answer = macros.wialon_request_lite("&svc=item/update_profile_field&params={"
                                                               + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                               + "\"n\":\"brand\","
                                                               + "\"v\":\"" + comboBox_test_brand.GetItemText(this.comboBox_test_brand.SelectedItem).ToString() + "\"}");
                //Характеристики model
                string pp27_answer = macros.wialon_request_lite("&svc=item/update_profile_field&params={"
                                                               + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                               + "\"n\":\"model\","
                                                               + "\"v\":\"" + comboBox_test_model.GetItemText(this.comboBox_test_model.SelectedItem).ToString() + "\"}");
                //Характеристики vin
                string pp28_answer = macros.wialon_request_lite("&svc=item/update_profile_field&params={"
                                                               + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                               + "\"n\":\"vin\","
                                                               + "\"v\":\"" + textBox_vin.Text.Replace("\"", "%5C%22") + "\"}");

                //3.8.1 Тривожна кнопка: провідна, безпровідна
                string pp224_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"17\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"3.8.1 Тривожна кнопка: провідна, безпровідна\","
                                                                + "\"v\":\"" + comboBox__tk_wirreless_or_wire.GetItemText(comboBox__tk_wirreless_or_wire.SelectedItem).ToString().Replace("\"", "%5C%22") + "\"}");

                //3.9.1 Кнопки введення PIN коду: штатні, додатково встановленні
                string pp234_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"19\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"3.9.1 Кнопки введення PIN коду: штатні, додатково встановленні\","
                                                                + "\"v\":\"" + textBox_pin_button_external.Text.Replace("\"", "%5C%22") + "\"}");




            }///Создаем произвольные поля, меняем имя обекта new 710/730 CNTP, CNTK

            else if (get_produt_testing_device == "2" || get_produt_testing_device == "3" )
            {
                string json = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                         + "\"id\":\"" + vars_form.id_wl_object_for_test + "\","
                                                         + "\"flags\":\"‭‭‭‭5257‬‬‬‬\"}"); //
                var test_out = JsonConvert.DeserializeObject<RootObject>(json);

                //Меняем имя об"екта
                string name_answer = macros.wialon_request_lite("&svc=item/update_name&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"name\":\"" + name_obj_textBox.Text.Replace("\"", "%5C%22") + "\"}");

                //Произвольное поле  name operator
                string pp6_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"7\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"3.1.1 Оператор, що тестував\","
                                                                + "\"v\":\"" + vars_form.user_login_name + "\"}");



                //Произвольное поле 0 УВАГА
                string pp1_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"1\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"0 УВАГА\","
                                                                + "\"v\":\"" + textBox_uvaga.Text.Replace("\"", "%5C%22") + "\"}");
                //Произвольное поле device1
                string pp100_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"14\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"3.4 Місце установки пристрою ВЕНБЕСТ\","
                                                                + "\"v\":\"" + textBox_device1.Text.Replace("\"", "%5C%22") + "\"}");
                //Произвольное поле device2
                string pp2_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                              + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                              + "\"id\":\"15\","
                                                              + "\"callMode\":\"update\","
                                                              + "\"n\":\"3.5 Назва та місце установки сигналізації\","
                                                              + "\"v\":\"" + TextBox_device2.Text.Replace("\"", "%5C%22") + "\"}");
                //Произвольное поле place relay
                string pp3_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                              + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                              + "\"id\":\"16\","
                                                              + "\"callMode\":\"update\","
                                                              + "\"n\":\"3.6.1  Реле блокування: місце встановлення\","
                                                              + "\"v\":\"" + textBox_place_relay.Text.Replace("\"", "%5C%22") + "\"}");
                //Произвольное поле wire cut
                string pp4_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                              + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                              + "\"id\":\"17\","
                                                              + "\"callMode\":\"update\","
                                                              + "\"n\":\"3.6.2 Реле блокування: елемент блокування\","
                                                              + "\"v\":\"" + textBox_wire_cut.Text.Replace("\"", "%5C%22") + "\"}");

                
                //Произвольное поле place tk
                string pp5_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                              + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                              + "\"id\":\"20\","
                                                              + "\"callMode\":\"update\","
                                                              + "\"n\":\"3.8.2 Місце установки тривожної кнопки\","
                                                              + "\"v\":\"" + textBox_place_tk.Text.Replace("\"", "%5C%22") + "\"}");
                //Произвольное поле place wireles tk
                string pp60_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                              + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                              + "\"id\":\"19\","
                                                              + "\"callMode\":\"update\","
                                                              + "\"n\":\"3.8.1 Тривожна кнопка: провідна, безпровідна\","
                                                              + "\"v\":\"\"}");
                //Произвольное поле service button
                string pp7_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                              + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                              + "\"id\":\"18\","
                                                              + "\"callMode\":\"update\","
                                                              + "\"n\":\"3.7 Місце встановлення сервісної кнопки\","
                                                              + "\"v\":\"" + textBox_service_button.Text.Replace("\"", "%5C%22") + "\"}");
                //Произвольное поле  button fo pin
                string pp8_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                              + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                              + "\"id\":\"22\","
                                                              + "\"callMode\":\"update\","
                                                              + "\"n\":\"3.9.2 Штатні кнопки введення PIN-коду\","
                                                              + "\"v\":\"" + textBox_buttons_for_pin.Text.Replace("\"", "%5C%22") + "\"}");
                
                //Додатково встановлені сигналізації\
                string pp9_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                              + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                              + "\"id\":\"8\","
                                                              + "\"callMode\":\"update\","
                                                              + "\"n\":\"3.11 Додатково встановлені сигналізації\","
                                                              + "\"v\":\"" + textBox_other_alarm.Text.Replace("\"", "%5C%22") + "\"}");

                //3.15 Додатково встановлені датчики
                string pp99_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                              + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                              + "\"id\":\"10\","
                                                              + "\"callMode\":\"update\","
                                                              + "\"n\":\"3.15 Додатково встановлені датчики\","
                                                              + "\"v\":\"" + textBox_instaled_sensor.Text.Replace("\"", "%5C%22") + "\"}");

                //Постановка авто под охрану через багажник?
                string pp90_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                              + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                              + "\"id\":\"9\","
                                                              + "\"callMode\":\"update\","
                                                              + "\"n\":\"3.12 Постановка авто под охрану через багажник?\","
                                                              + "\"v\":\"" + textBox_arm_bagagnik.Text.Replace("\"", "%5C%22") + "\"}");

                //Характеристики kuzov type
                string pp10_answer = macros.wialon_request_lite("&svc=item/update_profile_field&params={"
                                                              + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                              + "\"n\":\"vehicle_type\","
                                                              + "\"v\":\"" + comboBox_kuzov_type.GetItemText(this.comboBox_kuzov_type.SelectedItem).ToString() + "\"}");
                //Характеристики color
                string pp11_answer = macros.wialon_request_lite("&svc=item/update_profile_field&params={"
                                                              + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                              + "\"n\":\"color\","
                                                              + "\"v\":\"" + comboBox_color.GetItemText(this.comboBox_color.SelectedItem).ToString() + "\"}");
                //Характеристики prod date
                string pp12_answer = macros.wialon_request_lite("&svc=item/update_profile_field&params={"
                                                               + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                               + "\"n\":\"year\","
                                                               + "\"v\":\"" + comboBox_test_production_date.GetItemText(this.comboBox_test_production_date.SelectedItem).ToString() + "\"}");
                //Характеристики licence plate
                string pp13_answer = macros.wialon_request_lite("&svc=item/update_profile_field&params={"
                                                               + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                               + "\"n\":\"registration_plate\","
                                                               + "\"v\":\"" + textBox_licence_plate.Text.Replace("\"", "%5C%22") + "\"}");
                //Характеристики brend
                string pp14_answer = macros.wialon_request_lite("&svc=item/update_profile_field&params={"
                                                               + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                               + "\"n\":\"brand\","
                                                               + "\"v\":\"" + comboBox_test_brand.GetItemText(this.comboBox_test_brand.SelectedItem).ToString() + "\"}");
                //Характеристики model
                string pp15_answer = macros.wialon_request_lite("&svc=item/update_profile_field&params={"
                                                               + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                               + "\"n\":\"model\","
                                                               + "\"v\":\"" + comboBox_test_model.GetItemText(this.comboBox_test_model.SelectedItem).ToString() + "\"}");
                //Характеристики vin
                string pp16_answer = macros.wialon_request_lite("&svc=item/update_profile_field&params={"
                                                               + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                               + "\"n\":\"vin\","
                                                               + "\"v\":\"" + textBox_vin.Text.Replace("\"", "%5C%22") + "\"}");
                //Произвольное поле  relay plus?
                //string pp17_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                //                                              + "\"itemId\":\"" + vars_form.id_object_for_test + "\","
                //                                              + "\"id\":\"22\","
                //                                              + "\"callMode\":\"update\","
                //                                              + "\"n\":\"3.9.2 Штатні кнопки введення PIN-коду\","
                //                                              + "\"v\":\"" + textBox_buttons_for_pin.Text + "\"}");
                
                //Произвольное поле  relay plus?
                string pp17_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                               + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                               + "\"id\":\"7\","
                                                               + "\"callMode\":\"update\","
                                                               + "\"n\":\"3.1.1 Оператор, що тестував\","
                                                               + "\"v\":\"" + vars_form.user_login_name + "\"}");
                //Произвольное поле СТО
                
                string pp18_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"11\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"3.2.1 Установник: назва, адреса\","
                                                                + "\"v\":\"" + comboBox_test_sto.GetItemText(this.comboBox_test_sto.SelectedItem).ToString().Replace("\"", "%5C%22") + "\"}");
                //Произвольное поле Установщик
                
                    string pp19_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"12\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"3.2.2 Установник - монтажник: ПІБ, №тел.\","
                                                                  + "\"v\":\"" + comboBox_ustanoshik_poisk.GetItemText(this.comboBox_ustanoshik_poisk.SelectedItem).ToString().Replace("\"", "%5C%22") + "\"}");
                //Произвольное поле дата установки = дата тестирования
                string pp20_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"13\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"3.3 Дата установки\","
                                                                + "\"v\":\"" + DateTime.Now.Date + "\"}");
                //Произвольное поле новій ПИН
                string pp21_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"26\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"4.3 PIN-код встановлено особою(клієнт/установлник)\","
                                                                + "\"v\":\"" + textBox_current_pin.Text.Replace("\"", "%5C%22") + "\"}");

                //3.8.1 Тривожна кнопка: провідна, безпровідна
                string pp22_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"19\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"3.8.1 Тривожна кнопка: провідна, безпровідна\","
                                                                + "\"v\":\"" + comboBox__tk_wirreless_or_wire.GetItemText(comboBox__tk_wirreless_or_wire.SelectedItem).ToString().Replace("\"", "%5C%22") + "\"}");

                //3.9.1 Кнопки введення PIN коду: штатні, додатково встановленні
                string pp23_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"21\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"3.9.1 Кнопки введення PIN коду: штатні, додатково встановленні\","
                                                                + "\"v\":\"" + textBox_pin_button_external.Text.Replace("\"", "%5C%22") + "\"}");

                //Произвольное поле Гарантія до
                string pp211_answer = macros.wialon_request_lite("&svc=item/update_custom_field&params={"
                                                                + "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                                + "\"id\":\"30\","
                                                                + "\"callMode\":\"update\","
                                                                + "\"n\":\"5.3 Гарантія до\","
                                                                + "\"v\":\"" + (DateTime.Now.Date.AddYears(1)).AddDays(-1).ToString() + "\"}");
            }//Создаем произвольные поля, меняем имя обекта old CNTP, CNTK

            else
            {
                MessageBox.Show("Пока не готово сохранение произволных полй по этому продукту");
            }
        }

        private void commands_add_testing()
        {
            if (vars_form.if_open_created_testing == 1)// выполняем если открываем незаконченное тестирование 
            {
                if (checkBox_test_tk.Checked is false || checkBox_test_zablocovano.Checked is false ||
                    checkBox_test_du.Checked is false || checkBox__test_vzlom.Checked is false ||
                    checkBox_test_koordinati.Checked is false)// выполняем если завершается тестирование неуспехом
                {
                    if (textBox_commets.Text == "")
                    {
                        MessageBox.Show("тестування не успішне? Додай коментар");
                        return;
                    }

                    macros.sql_command("update btk.testing_object set " +
                        "testing_objectcol_edit_timestamp = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                        "testing_objectcol_block = '" + (checkBox_test_zablocovano.Checked ? "1" : "0") + "', " +
                        "testing_objectcol_alarm_door = '" + (checkBox__test_vzlom.Checked ? "1" : "0") + "', " +
                        "testing_objectcol_udar = '" + (checkBox_test_du.Checked ? "1" : "0") + "', " +
                        "testing_objectcol_tk = '" + (checkBox_test_tk.Checked ? "1" : "0") + "', " +
                        "testing_objectcol_adress = '" + (checkBox_test_koordinati.Checked ? "1" : "0") + "', " +
                        "testing_objectcol_arm_hood = '" + (checkBox_test_hood_in_arm.Checked ? "1" : "0") + "', " +
                        "testing_objectcol_dop1 = '" + (checkBox_dop_1.Checked ? "1" : "0") + "', " +
                        "testing_objectcol_dop2 = '" + (checkBox_test_dop_2.Checked ? "1" : "0") + "', " +
                        "testing_objectcol_autostart = '" + (checkBox_test_autostart.Checked ? "1" : "0") + "', " +
                        "testing_objectcol_comments = '" + textBox_commets.Text.ToString() + "', " +
                        "Users_idUsers = '" + vars_form.user_login_id + "', " +
                        "testing_objectcol_result = 'Не завершено' " +
                        "where " +
                        "idtesting_object = '" + vars_form.id_db_openning_testing + "' " +
                        ";");


                    //macros.sql_command("insert into btk.testing_object (" +
                    //                       "Object_idObject, " +
                    //                       "testing_objectcol_edit_timestamp, " +
                    //                       "testing_objectcol_block, " +
                    //                       "testing_objectcol_alarm_door, " +
                    //                       "testing_objectcol_udar, " +
                    //                       "testing_objectcol_tk, " +
                    //                       "testing_objectcol_adress, " +
                    //                       "testing_objectcol_arm_hood, " +
                    //                       "testing_objectcol_dop1, " +
                    //                       "testing_objectcol_dop2, " +
                    //                       "testing_objectcol_autostart, " +
                    //                       "testing_objectcol_comments, " +
                    //                       "Users_idUsers, " +
                    //                       "testing_objectcol_result) " +
                    //                       "values(" +
                    //                       "'" + vars_form.id_db_object_for_test + "', " +
                    //                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                    //                       "'" + (checkBox_test_zablocovano.Checked ? "1" : "0") + "', " +
                    //                       "'" + (checkBox__test_vzlom.Checked ? "1" : "0") + "', " +
                    //                       "'" + (checkBox_test_du.Checked ? "1" : "0") + "', " +
                    //                       "'" + (checkBox_test_tk.Checked ? "1" : "0") + "', " +
                    //                       "'" + (checkBox_test_koordinati.Checked ? "1" : "0") + "', " +
                    //                       "'" + (checkBox_test_hood_in_arm.Checked ? "1" : "0") + "', " +
                    //                       "'" + (checkBox_dop_1.Checked ? "1" : "0") + "', " +
                    //                       "'" + (checkBox_test_dop_2.Checked ? "1" : "0") + "', " +
                    //                       "'" + (checkBox_test_autostart.Checked ? "1" : "0") + "', " +
                    //                       "'" + textBox_commets.Text.ToString() + "', " +
                    //                       "'" + vars_form.user_login_id + "', " +
                    //                       "'" + result + "'" +
                    //                       ");");

                    macros.sql_command("update btk.Object set Objectcol_testing_ok = 0 where idObject = '" +
                                       vars_form.id_db_object_for_test + "';");
                }
                else // // выполняем если завершается тестирование успехом
                {
                    if (wl_group_activ_checkBox.Checked is true)
                    {
                        /////////////////
                        ///Добавляем в группу Объекты All
                        /////////////////
                        ///
                        ///
                        ///
                        //string ev = macros.wialon_request_lite("&svc=core/search_items&params={" +
                        //                                       "\"spec\":{"
                        //                                       + "\"itemsType\":\"avl_unit_group\","
                        //                                       + "\"propName\":\"sys_name\","
                        //                                       + "\"propValueMask\":\"*актив*\", " 
                        //                                       + "\"sortType\":\"sys_name\"," 
                        //                                       + "\"or_logic\":\"1\"}," 
                        //                                       + "\"or_logic\":\"1\"," 
                        //                                       + "\"force\":\"1\"," 
                        //                                       + "\"flags\":\"1\"," 
                        //                                       + "\"from\":\"0\"," 
                        //                                       + "\"to\":\"0\"}");//получаем текущее местоположение объекта

                        //Запрашиваем айди продукта тестируемого обїекта
                        string get_produt_testing_device = macros.sql_command("select " +
                                                                               "products_has_Tarif.products_idproducts " +
                                                                               "from " +
                                                                               "btk.object_subscr, btk.Subscription, btk.products_has_Tarif, btk.Object " +
                                                                               "where " +
                                                                               "Object.Object_id_wl = " + vars_form.id_wl_object_for_test + " and " +
                                                                               "Object.idObject=Object_idObject and " +
                                                                               "Subscription_idSubscr=idSubscr and " +
                                                                               "products_has_Tarif_idproducts_has_Tarif=idproducts_has_Tarif;");

                        //Установка Групп в зависимсти от продукта
                        if (get_produt_testing_device == "11" || get_produt_testing_device == "3")//Добавляем в группы для CNTP_910, CNTP_730
                        {

                            // ADD to groupe: ^ CONNECT - KEYLESS - PLUS - КОНДОР+: активные
                            string get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"1759\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            string gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"1759\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый



                            // ADD to groupe: ^ Активні (all)
                            get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"46\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"46\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый


                            // ADD to groupe: ^ Активні (all)
                            get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"6409\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"6409\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый


                            // ADD to groupe: srv_CNTP
                            get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"7669\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"7669\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

                        }
                        else if (get_produt_testing_device == "10" || get_produt_testing_device == "2")//Добавляем в группы для CNTK_910, CNTK_710
                        {

                            // ADD to groupe: ^ CONNECT - KEYLESS - PLUS - КОНДОР+: активные
                            string get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"1759\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            string gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"1759\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый



                            // ADD to groupe: ^ Активні (all)
                            get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"46\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"46\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый


                            // ADD to groupe: ^ Активні (all)
                            get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"6409\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"6409\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый


                            // ADD to groupe: srv_CNTP
                            get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"7668\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"7668\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

                        }
                        else if (get_produt_testing_device == "4" || get_produt_testing_device == "13" || get_produt_testing_device == "14")//Добавляем в группы для CNTP_910_SE_N, CNTP_910_SE_P, CNTP_SE
                        {

                            // ADD to groupe: ^ CONNECT - KEYLESS - PLUS - КОНДОР+: активные
                            string get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"1759\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            string gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"1759\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый



                            // ADD to groupe: ^ Активні (all)
                            get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"46\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"46\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый


                            // ADD to groupe: ^ Активні (all)
                            get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"6409\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"6409\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый


                            // ADD to groupe: srv_CNTP
                            get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"7668\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"7668\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

                        }
                    }

                    macros.sql_command("update btk.testing_object set " +
                        "testing_objectcol_edit_timestamp = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                        "testing_objectcol_block = '" + (checkBox_test_zablocovano.Checked ? "1" : "0") + "', " +
                        "testing_objectcol_alarm_door = '" + (checkBox__test_vzlom.Checked ? "1" : "0") + "', " +
                        "testing_objectcol_udar = '" + (checkBox_test_du.Checked ? "1" : "0") + "', " +
                        "testing_objectcol_tk = '" + (checkBox_test_tk.Checked ? "1" : "0") + "', " +
                        "testing_objectcol_adress = '" + (checkBox_test_koordinati.Checked ? "1" : "0") + "', " +
                        "testing_objectcol_arm_hood = '" + (checkBox_test_hood_in_arm.Checked ? "1" : "0") + "', " +
                        "testing_objectcol_dop1 = '" + (checkBox_dop_1.Checked ? "1" : "0") + "', " +
                        "testing_objectcol_dop2 = '" + (checkBox_test_dop_2.Checked ? "1" : "0") + "', " +
                        "testing_objectcol_autostart = '" + (checkBox_test_autostart.Checked ? "1" : "0") + "', " +
                        "testing_objectcol_comments = '" + textBox_commets.Text.ToString() + "', " +
                        "Users_idUsers = '" + vars_form.user_login_id + "', " +
                        "testing_objectcol_result = 'Успішно' " +
                        "where " +
                        "idtesting_object = '" + vars_form.id_db_openning_testing + "' " +
                        ";");

                    


                    //macros.sql_command("insert into btk.testing_object (" +
                    //                       "Object_idObject, " +
                    //                       "testing_objectcol_edit_timestamp, " +
                    //                       "testing_objectcol_block, " +
                    //                       "testing_objectcol_alarm_door, " +
                    //                       "testing_objectcol_udar, " +
                    //                       "testing_objectcol_tk, " +
                    //                       "testing_objectcol_adress, " +
                    //                       "testing_objectcol_arm_hood, " +
                    //                       "testing_objectcol_dop1, " +
                    //                       "testing_objectcol_dop2, " +
                    //                       "testing_objectcol_autostart, " +
                    //                       "testing_objectcol_comments, " +
                    //                       "Users_idUsers, " +
                    //                       "testing_objectcol_result) " +
                    //                       "values(" +
                    //                       "'" + vars_form.id_db_object_for_test + "', " +
                    //                       "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                    //                       "'" + (checkBox_test_zablocovano.Checked ? "1" : "0") + "', " +
                    //                       "'" + (checkBox__test_vzlom.Checked ? "1" : "0") + "', " +
                    //                       "'" + (checkBox_test_du.Checked ? "1" : "0") + "', " +
                    //                       "'" + (checkBox_test_tk.Checked ? "1" : "0") + "', " +
                    //                       "'" + (checkBox_test_koordinati.Checked ? "1" : "0") + "', " +
                    //                       "'" + (checkBox_test_hood_in_arm.Checked ? "1" : "0") + "', " +
                    //                       "'" + (checkBox_dop_1.Checked ? "1" : "0") + "', " +
                    //                       "'" + (checkBox_test_dop_2.Checked ? "1" : "0") + "', " +
                    //                       "'" + (checkBox_test_autostart.Checked ? "1" : "0") + "', " +
                    //                       "'" + textBox_commets.Text.ToString() + "', " +
                    //                       "'" + vars_form.user_login_id + "', " +
                    //                       "'" + comboBox_result.GetItemText(comboBox_result.SelectedItem).ToString() + "'" +
                    //                       ");");

                    macros.sql_command("update btk.Object set Objectcol_testing_ok = 'Не завершено' where idObject = '" + vars_form.id_db_object_for_test + "';");

                    string Subject = "505 Протестовано успішно! VIN: " + textBox_vin.Text + ", Обєкт: " + name_obj_textBox.Text;
                    string recip = "<" + vars_form.user_login_email + ">," + "<o.pustovit@venbest.com.ua>,<d.lenik@venbest.com.ua>,<s.gregul@venbest.com.ua>,<a.lozinskiy@venbest.com.ua>,<mc@venbest.com.ua>,<e.remekh@venbest.com.ua><e.danilchenko@venbest.com.ua>,<a.andreasyan@venbest.com.ua>";
                    DataTable dt = new DataTable();

                    dt.Columns.Add("Параметр");
                    dt.Columns.Add("Значення");
                    object[] row = { "VIN", textBox_vin.Text };
                    dt.Rows.Add(row);
                    object[] row1 = { "Обєкт", name_obj_textBox.Text };
                    dt.Rows.Add(row1);
                    object[] row2 = { "IMEI", imei_obj_textBox.Text };
                    dt.Rows.Add(row2);
                    if (checkBox_test_autostart.Checked is true)
                    {
                        object[] row3 = { "Автозапуск", "Встановлено" };
                        dt.Rows.Add(row3);
                    }
                    if (checkBox_dop_1.Checked is true)
                    {
                        object[] row12 = { "Додатковий датчик 1", textBox_instaled_sensor.Text };
                        dt.Rows.Add(row12);
                    }
                    object[] row4 = { "Марка", comboBox_test_brand.GetItemText(this.comboBox_test_brand.SelectedItem) };
                    dt.Rows.Add(row4);
                    object[] row5 = { "Модель", comboBox_test_model.GetItemText(this.comboBox_test_model.SelectedItem) };
                    dt.Rows.Add(row5);
                    object[] row6 = { "Колір", comboBox_color.GetItemText(this.comboBox_color.SelectedItem) };
                    dt.Rows.Add(row6);
                    object[] row7 = { "Рік випуску", comboBox_test_production_date.GetItemText(this.comboBox_test_production_date.SelectedItem) };
                    dt.Rows.Add(row7);
                    object[] row8 = { "Держ. Номер", textBox_licence_plate.Text };
                    dt.Rows.Add(row8);
                    object[] row9 = { "СТО", comboBox_test_sto.GetItemText(this.comboBox_test_sto.SelectedItem) };
                    dt.Rows.Add(row9);
                    object[] row10 = { "Установник", comboBox_ustanoshik_poisk.GetItemText(this.comboBox_ustanoshik_poisk.SelectedItem) };
                    dt.Rows.Add(row10);
                    object[] row11 = { "Дата тестування", DateTime.Now.ToString() };
                    dt.Rows.Add(row11);

                    string Body = macros.ConvertDataTableToHTML(dt);

                    macros.send_mail(recip, Subject, Body);
                }
            }
            else
            {
                
                if (checkBox_test_tk.Checked is false || checkBox_test_zablocovano.Checked is false ||
                    checkBox_test_du.Checked is false || checkBox__test_vzlom.Checked is false ||
                    checkBox_test_koordinati.Checked is false)
                {
                    if (textBox_commets.Text == "")
                    {
                        MessageBox.Show("тестування не успішне? Додай коментар");
                        return;
                    }

                    

                    macros.sql_command("insert into btk.testing_object (" +
                                           "Object_idObject, " +
                                           "testing_objectcol_edit_timestamp, " +
                                           "testing_objectcol_block, " +
                                           "testing_objectcol_alarm_door, " +
                                           "testing_objectcol_udar, " +
                                           "testing_objectcol_tk, " +
                                           "testing_objectcol_adress, " +
                                           "testing_objectcol_arm_hood, " +
                                           "testing_objectcol_dop1, " +
                                           "testing_objectcol_dop2, " +
                                           "testing_objectcol_autostart, " +
                                           "testing_objectcol_comments, " +
                                           "Users_idUsers, " +
                                           "testing_objectcol_result) " +
                                           "values(" +
                                           "'" + vars_form.id_db_object_for_test + "', " +
                                           "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                           "'" + (checkBox_test_zablocovano.Checked ? "1" : "0") + "', " +
                                           "'" + (checkBox__test_vzlom.Checked ? "1" : "0") + "', " +
                                           "'" + (checkBox_test_du.Checked ? "1" : "0") + "', " +
                                           "'" + (checkBox_test_tk.Checked ? "1" : "0") + "', " +
                                           "'" + (checkBox_test_koordinati.Checked ? "1" : "0") + "', " +
                                           "'" + (checkBox_test_hood_in_arm.Checked ? "1" : "0") + "', " +
                                           "'" + (checkBox_dop_1.Checked ? "1" : "0") + "', " +
                                           "'" + (checkBox_test_dop_2.Checked ? "1" : "0") + "', " +
                                           "'" + (checkBox_test_autostart.Checked ? "1" : "0") + "', " +
                                           "'" + textBox_commets.Text.ToString() + "', " +
                                           "'" + vars_form.user_login_id + "', " +
                                           "'Не завершено'" +
                                           ");");
                    macros.sql_command("update btk.Object set Objectcol_testing_ok = 0 where idObject = '" +
                                       vars_form.id_db_object_for_test + "';");
                }
                else
                {
                    

                    if (wl_group_activ_checkBox.Checked is true)
                    {
                        //Запрашиваем айди продукта тестируемого обїекта
                        string get_produt_testing_device = macros.sql_command("select " +
                                                                               "products_has_Tarif.products_idproducts " +
                                                                               "from " +
                                                                               "btk.object_subscr, btk.Subscription, btk.products_has_Tarif, btk.Object " +
                                                                               "where " +
                                                                               "Object.Object_id_wl = " + vars_form.id_wl_object_for_test + " and " +
                                                                               "Object.idObject=Object_idObject and " +
                                                                               "Subscription_idSubscr=idSubscr and " +
                                                                               "products_has_Tarif_idproducts_has_Tarif=idproducts_has_Tarif;");

                        //string ev = macros.wialon_request_lite("&svc=core/search_items&params={" +
                        //                                        "\"spec\":{"
                        //                                        + "\"itemsType\":\"avl_unit_group\","
                        //                                        + "\"propName\":\"sys_name\","
                        //                                        + "\"propValueMask\":\"*актив*\", "
                        //                                        + "\"sortType\":\"sys_name\","
                        //                                        + "\"or_logic\":\"1\"},"
                        //                                        + "\"or_logic\":\"1\","
                        //                                        + "\"force\":\"1\","
                        //                                        + "\"flags\":\"1\","
                        //                                        + "\"from\":\"0\","
                        //                                        + "\"to\":\"0\"}");//получаем текущее местоположение объекта

                        //string ev2 = macros.wialon_request_lite("&svc=core/search_items&params={" +
                        //                                        "\"spec\":{"
                        //                                        + "\"itemsType\":\"avl_unit_group\","
                        //                                        + "\"propName\":\"sys_name\","
                        //                                        + "\"propValueMask\":\"*srv*\", "
                        //                                        + "\"sortType\":\"sys_name\","
                        //                                        + "\"or_logic\":\"1\"},"
                        //                                        + "\"or_logic\":\"1\","
                        //                                        + "\"force\":\"1\","
                        //                                        + "\"flags\":\"1\","
                        //                                        + "\"from\":\"0\","
                        //                                        + "\"to\":\"0\"}");//получаем текущее местоположение объекта



                        //Установка Групп в зависимсти от продукта
                        if (get_produt_testing_device == "11" || get_produt_testing_device == "3")//Добавляем в группы для CNTP_910, CNTP_730
                        {

                            // ADD to groupe: ^ CONNECT - KEYLESS - PLUS - КОНДОР+: активные
                            string get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"1759\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            string gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"1759\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый



                            // ADD to groupe: ^ Активні (all)
                            get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"46\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"46\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый


                            // ADD to groupe: ^ Активні (all)
                            get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"6409\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"6409\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый


                            // ADD to groupe: srv_CNTP
                            get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"7669\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"7669\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

                        }
                        else if (get_produt_testing_device == "10" || get_produt_testing_device == "2")//Добавляем в группы для CNTK_910, CNTK_710
                        {

                            // ADD to groupe: ^ CONNECT - KEYLESS - PLUS - КОНДОР+: активные
                            string get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"1759\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            string gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"1759\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый



                            // ADD to groupe: ^ Активні (all)
                            get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"46\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"46\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый


                            // ADD to groupe: ^ Активні (all)
                            get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"6409\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"6409\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый


                            // ADD to groupe: srv_CNTP
                            get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"7668\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"7668\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

                        }
                        else if (get_produt_testing_device == "4" || get_produt_testing_device == "13" || get_produt_testing_device == "14")//Добавляем в группы для CNTP_910_SE_N, CNTP_910_SE_P, CNTP_SE
                        {

                            // ADD to groupe: ^ CONNECT - KEYLESS - PLUS - КОНДОР+: активные
                            string get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"1759\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            string gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"1759\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый



                            // ADD to groupe: ^ Активні (all)
                            get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"46\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"46\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый


                            // ADD to groupe: ^ Активні (all)
                            get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"6409\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"6409\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый


                            // ADD to groupe: srv_CNTP
                            get_units_on_group = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                                                                                                + "\"id\":\"7668\","
                                                                                                                                + "\"flags\":\"1\"}");//получаем все объекты группы

                            list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

                            list_get_units_on_group.item.u.Add(Convert.ToInt32(vars_form.id_wl_object_for_test));//Доповляем в список новый объект
                            units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

                            gr_answer = macros.wialon_request_new("&svc=unit_group/update_units&params={"
                                                                                                             + "\"itemId\":\"7668\","
                                                                                                             + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

                        }
                    }
                    macros.sql_command("insert into btk.testing_object (" +
                                           "Object_idObject, " +
                                           "testing_objectcol_edit_timestamp, " +
                                           "testing_objectcol_block, " +
                                           "testing_objectcol_alarm_door, " +
                                           "testing_objectcol_udar, " +
                                           "testing_objectcol_tk, " +
                                           "testing_objectcol_adress, " +
                                           "testing_objectcol_arm_hood, " +
                                           "testing_objectcol_dop1, " +
                                           "testing_objectcol_dop2, " +
                                           "testing_objectcol_autostart, " +
                                           "testing_objectcol_comments, " +
                                           "Users_idUsers, " +
                                           "testing_objectcol_result) " +
                                           "values(" +
                                           "'" + vars_form.id_db_object_for_test + "', " +
                                           "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                                           "'" + (checkBox_test_zablocovano.Checked ? "1" : "0") + "', " +
                                           "'" + (checkBox__test_vzlom.Checked ? "1" : "0") + "', " +
                                           "'" + (checkBox_test_du.Checked ? "1" : "0") + "', " +
                                           "'" + (checkBox_test_tk.Checked ? "1" : "0") + "', " +
                                           "'" + (checkBox_test_koordinati.Checked ? "1" : "0") + "', " +
                                           "'" + (checkBox_test_hood_in_arm.Checked ? "1" : "0") + "', " +
                                           "'" + (checkBox_dop_1.Checked ? "1" : "0") + "', " +
                                           "'" + (checkBox_test_dop_2.Checked ? "1" : "0") + "', " +
                                           "'" + (checkBox_test_autostart.Checked ? "1" : "0") + "', " +
                                           "'" + textBox_commets.Text.ToString() + "', " +
                                           "'" + vars_form.user_login_id + "', " +
                                           "'Успішно'" +
                                           ");");
                    macros.sql_command("update btk.Object set Objectcol_testing_ok = 'Успішно' where idObject = '" + vars_form.id_db_object_for_test + "';");

                    string Subject = "505 Протестовано успішно! VIN: " + textBox_vin.Text + ", Обєкт: " + name_obj_textBox.Text;
                    string recip = "<" + vars_form.user_login_email + ">," + "<o.pustovit@venbest.com.ua>,<d.lenik@venbest.com.ua>,<s.gregul@venbest.com.ua>,<a.lozinskiy@venbest.com.ua>,<mc@venbest.com.ua>,<e.remekh@venbest.com.ua><e.danilchenko@venbest.com.ua>,<a.andreasyan@venbest.com.ua>";
                    DataTable dt = new DataTable();

                    dt.Columns.Add("Параметр");
                    dt.Columns.Add("Значення");
                    object[] row = { "VIN", textBox_vin.Text };
                    dt.Rows.Add(row);
                    object[] row1 = { "Обєкт", name_obj_textBox.Text };
                    dt.Rows.Add(row1);
                    object[] row2 = { "IMEI", imei_obj_textBox.Text };
                    dt.Rows.Add(row2);
                    if (checkBox_test_autostart.Checked is true)
                    {
                        object[] row3 = { "Автозапуск", "Встановлено" };
                        dt.Rows.Add(row3);
                    }
                    if (checkBox_dop_1.Checked is true)
                    {
                        object[] row12 = { "Додатковий датчик 1", textBox_instaled_sensor.Text };
                        dt.Rows.Add(row12);
                    }
                    object[] row4 = { "Марка", comboBox_test_brand.GetItemText(this.comboBox_test_brand.SelectedItem) };
                    dt.Rows.Add(row4);
                    object[] row5 = { "Модель", comboBox_test_model.GetItemText(this.comboBox_test_model.SelectedItem) };
                    dt.Rows.Add(row5);
                    object[] row6 = { "Колір", comboBox_color.GetItemText(this.comboBox_color.SelectedItem) };
                    dt.Rows.Add(row6);
                    object[] row7 = { "Рік випуску", comboBox_test_production_date.GetItemText(this.comboBox_test_production_date.SelectedItem) };
                    dt.Rows.Add(row7);
                    object[] row8 = { "Держ. Номер", textBox_licence_plate.Text };
                    dt.Rows.Add(row8);
                    object[] row9 = { "СТО", comboBox_test_sto.GetItemText(this.comboBox_test_sto.SelectedItem) };
                    dt.Rows.Add(row9);
                    object[] row10 = { "Установник", comboBox_ustanoshik_poisk.GetItemText(this.comboBox_ustanoshik_poisk.SelectedItem) };
                    dt.Rows.Add(row10);
                    object[] row11 = { "Дата тестування", DateTime.Now.ToString() };
                    dt.Rows.Add(row11);

                    string Body = macros.ConvertDataTableToHTML(dt);

                    macros.send_mail(recip, Subject, Body);
                }
            }
            MessageBox.Show("Збережено!");
            this.Close();
        }

        //Запрос параметров с виалоноа и отображение статусов в тестировании
        private void getobjwl()
        {
            //get product of testing device
            string get_produt_testing_device = macros.sql_command("select " +
                                                                   "products_has_Tarif.products_idproducts " +
                                                                   "from " +
                                                                   "btk.object_subscr, btk.Subscription, btk.products_has_Tarif, btk.Object " +
                                                                   "where " +
                                                                   "Object.Object_id_wl = " + vars_form.id_wl_object_for_test + " and " +
                                                                   "Object.idObject=Object_idObject and " +
                                                                   "Subscription_idSubscr=idSubscr and " +
                                                                   "products_has_Tarif_idproducts_has_Tarif=idproducts_has_Tarif;");

            if (get_produt_testing_device == "2" || get_produt_testing_device == "3")
            {
                string json = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                         + "\"id\":\"" + vars_form.id_wl_object_for_test + "\","
                                                         + "\"flags\":\"‭‭‭‭2098177‬‬‬‬\"}"); //
                var test_out = JsonConvert.DeserializeObject<RootObject>(json);

                if (test_out.item.pos != null)
                {
                    //Запрашиваем по координатам фактический адрес
                    string get_adress = "http://navi.venbest.com.ua/gis_geocode?coords="
                                        + "[{\"lon\":\"" + test_out.item.pos["x"] + "\""
                                        + ",\"lat\":\"" + test_out.item.pos["y"] + "\"}]&flags=1255211008&uid=" +
                                        vars_form.wl_user_id + "";
                    MyWebRequest myRequest = new MyWebRequest(get_adress);
                    string json3 = myRequest.GetResponse();
                    var get_adress_out = JsonConvert.DeserializeObject<string[]>(json3);
                    label_test_address.Text = get_adress_out[0].ToString();
                }

                if (test_out.item.lmsg is null)
                {

                    return;
                }

                //Статус звязку
                if (test_out.item.netconn >= 1)
                {
                    label_netconn.Text = ":)";
                    label_netconn.BackColor = Color.Green;
                }
                else
                {
                    label_netconn.Text = ":(";
                    label_netconn.BackColor = Color.Red;
                }

                //Статус Блокування
                if (test_out.item.lmsg.p.AIN1 >= 4)
                {
                    label_test_zablocovano.Text = "Розблоковано";
                    label_test_zablocovano.BackColor = Color.Empty;
                }
                else
                {
                    label_test_zablocovano.Text = "Заблоковано";
                    label_test_zablocovano.BackColor = Color.YellowGreen;
                }

                //Статус Двері в охороні
                if (test_out.item.lmsg.p.par154 >= 1 & test_out.item.lmsg.p.par1 >= 1)
                {
                    label__test_vzlom.Text = "Відкрито";
                    label__test_vzlom.BackColor = Color.YellowGreen;
                }
                else
                {
                    label__test_vzlom.Text = "Закрито";
                    label__test_vzlom.BackColor = Color.Empty;
                }

                //Статус Сирени
                if (test_out.item.lmsg.p.par154 >= 1)
                {
                    label_test_du.Text = "Ввімкнено";
                    label_test_du.BackColor = Color.YellowGreen;
                }
                else
                {
                    label_test_du.Text = "Вимкнуто";
                    label_test_du.BackColor = Color.Empty;
                }

                //Статус TK
                if (test_out.item.lmsg.p.AIN2 >= 4)
                {
                    label_test_tk.Text = "Вимкнено";
                    label_test_tk.BackColor = Color.Empty;
                }
                else
                {
                    label_test_tk.Text = "Ввімкнено";
                    label_test_tk.BackColor = Color.YellowGreen;
                }

                //Статус В охороні
                if (test_out.item.lmsg.p.par6 >= 1)
                {
                    label_arm.Text = "B охороні";
                    label_arm.BackColor = Color.YellowGreen;
                }
                else
                {
                    label_arm.Text = "Знято з охорони";
                    label_arm.BackColor = Color.Empty;
                }

                //Статус all door
                if (test_out.item.lmsg.p.par1 >= 1)
                {
                    label_all_door.Text = "Відкрито";
                    label_all_door.BackColor = Color.YellowGreen;
                }
                else
                {
                    label_all_door.Text = "Закрито";
                    label_all_door.BackColor = Color.Empty;
                }




                ////Статус Доп_2
                //if (test_out.item.lmsg.p.par1 >= 1)
                //{
                //    label_test_dop_2.Text = "Відкрито";
                //}
                //else
                //{
                //    label_test_dop_2.Text = "Закрито";
                //}



                ////Статус Запалення в охороні
                //if (test_out.item.lmsg.p.AIN1 <= 4 & test_out.item.lmsg.p.par154 >= 1)
                //{
                //    label_test_dop_2.Text = "Ввімкнено";
                //}
                //else
                //{
                //    label_test_dop_2.Text = "Вимкнено";
                //}

            }
            else if (get_produt_testing_device == "10" || get_produt_testing_device == "11")
            {
                string json2 = macros.wialon_request_lite("&svc=core/search_items&params={" +
                                                          "\"spec\":{"
                                                          + "\"itemsType\":\"avl_unit\","
                                                          + "\"propName\":\"sys_id\","
                                                          + "\"propValueMask\":\"" + vars_form.id_wl_object_for_test + "\", "
                                                          + "\"sortType\":\"sys_name\"," 
                                                          + "\"or_logic\":\"1\"}," 
                                                          + "\"or_logic\":\"1\"," 
                                                          + "\"force\":\"1\"," 
                                                          + "\"flags\":\"4097\","
                                                          + "\"from\":\"0\"," 
                                                          + "\"to\":\"5\"}");

                


                string json = macros.wialon_request_lite("&svc=unit/calc_last_message&params={"
                                                         + "\"unitId\":\"" + vars_form.id_wl_object_for_test + "\","
                                                         + "\"sensors\":\"\","
                                                         + "\"flags\":\"1\"}"); //

                string json1 = macros.wialon_request_lite("&svc=core/search_item&params={"
                                                          + "\"id\":\"" + vars_form.id_wl_object_for_test + "\","
                                                          + "\"flags\":\"‭‭‭‭2098177‬‬‬‬\"}"); //



                Dictionary<string, string> sens_910 = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                var test_out = JsonConvert.DeserializeObject<RootObject>(json1);

                if (test_out.item.pos != null)
                {
                    //Запрашиваем по координатам фактический адрес
                    string get_adress = "http://navi.venbest.com.ua/gis_geocode?coords="
                                        + "[{\"lon\":\"" + test_out.item.pos["x"] + "\""
                                        + ",\"lat\":\"" + test_out.item.pos["y"] + "\"}]&flags=‭1254096896‬&uid=" +
                                        vars_form.wl_user_id + "";
                    MyWebRequest myRequest = new MyWebRequest(get_adress);
                    string json3 = myRequest.GetResponse();
                    var get_adress_out = JsonConvert.DeserializeObject<string[]>(json3);
                    label_test_address.Text = get_adress_out[0].ToString();
                }
                else
                {
                    label_test_address.Text = "Не визначено";
                }

                if (test_out.item.lmsg is null)
                {
                    label_test_address.Text = "Нет Координат";
                        //return;
                }

                //Статус звязку
                if (test_out.item.netconn == 1)
                {
                    label_netconn.Text = ":)";
                    label_netconn.BackColor = Color.YellowGreen;
                }
                else
                {
                    label_netconn.Text = ":(";
                    label_netconn.BackColor = Color.Red;
                }

                //Статус Блокування
                if (sens_910["1"] == "1"|| sens_910["3"] == "1")
                {
                    label_test_zablocovano.Text = "Заблоковано";
                    label_test_zablocovano.BackColor = Color.YellowGreen;
                }
                else
                {
                    label_test_zablocovano.Text = "Розблоковано";
                    label_test_zablocovano.BackColor = Color.Empty;
                }

                //Статус Двері в охороні
                if (sens_910["7"] == "1" & sens_910["2"] == "1")
                {
                    label__test_vzlom.Text = "Відкрито";
                    label__test_vzlom.BackColor = Color.YellowGreen;
                }
                else
                {
                    label__test_vzlom.Text = "Закрито";
                    label__test_vzlom.BackColor = Color.Empty;
                }

                //Статус ДУ
                if (sens_910["12"] == "1")
                {
                    label_test_du.Text = "Ввімкнено";
                    label_test_du.BackColor = Color.YellowGreen;
                }
                else
                {
                    label_test_du.Text = "Вимкнуто";
                    label_test_du.BackColor = Color.Empty;
                }

                //Статус TK
                if (sens_910["5"] == "1")
                {
                    label_test_tk.Text = "Ввімкнено";
                    label_test_tk.BackColor = Color.YellowGreen;
                }
                else
                {
                    label_test_tk.Text = "Вимкнено";
                    label_test_tk.BackColor = Color.Empty;
                }

                //Статус Dop 1
                if (sens_910["14"] == "1")
                {
                    label_test_dop_1.Text = "Ввімкнено";
                    label_test_dop_1.BackColor = Color.YellowGreen;
                }
                else
                {
                    label_test_dop_1.Text = "Вимкнено";
                    label_test_dop_1.BackColor = Color.Empty;
                }

                //Статус Dop_2
                if (sens_910["16"] == "1")
                {
                    label_test_dop_2.Text = "Ввімкнено";
                    label_test_dop_2.BackColor = Color.YellowGreen;
                }
                else
                {
                    label_test_dop_2.Text = "Вимкнено";
                    label_test_dop_2.BackColor = Color.Empty;
                }

                //Статус Охрани
                if (sens_910["2"] == "1")
                {
                    label_arm.Text = "Ввімкнено";
                    label_arm.BackColor = Color.YellowGreen;
                }
                else
                {
                    label_arm.Text = "Вимкнено";
                    label_arm.BackColor = Color.Empty;
                }

                //Статус all door
                if (sens_910["7"] == "1")
                {
                    label_all_door.Text = "Відкрито";
                    label_all_door.BackColor = Color.YellowGreen;
                }
                else
                {
                    label_all_door.Text = "Закрито";
                    label_all_door.BackColor = Color.Empty;
                }

                //Статус Авторизация
                if (sens_910["4"] == "1")
                {
                    label_auth.Text = "Пройдено";
                    label_auth.BackColor = Color.YellowGreen;
                }
                else
                {
                    label_auth.Text = "Немає";
                    label_auth.BackColor = Color.Empty;
                }

                //Статус Запалення
                if (sens_910["19"] == "1")
                {
                    label_ign.Text = "Ввімкнено";
                    label_ign.BackColor = Color.YellowGreen;
                }
                else
                {
                    label_ign.Text = "Вимкнено";
                    label_ign.BackColor = Color.Empty;
                }

                //Статус AutoStart
                if (sens_910["21"] == "1")
                {
                    label_autstart.Text = "Працює";
                    label_autstart.BackColor = Color.YellowGreen;
                }
                else
                {
                    label_autstart.Text = "Вимкненний";
                    label_autstart.BackColor = Color.Empty;
                }

                //Статус hood in arm
                if (sens_910["27"] == "1" & sens_910["27"] == "2")
                {
                    label_test_hood_arm.Text = "Відкрито";
                    label_test_hood_arm.BackColor = Color.YellowGreen;
                }
                else
                {
                    label_test_hood_arm.Text = "Закрито";
                    label_test_hood_arm.BackColor = Color.Empty;
                }
            }
        }

        private void checkBox_block_engine_CheckedChanged(object sender, EventArgs e)
        {
            //get product of testing device
            string get_produt_testing_device = macros.sql_command("select " +
                                                                   "products_has_Tarif.products_idproducts " +
                                                                   "from " +
                                                                   "btk.object_subscr, btk.Subscription, btk.products_has_Tarif, btk.Object " +
                                                                   "where " +
                                                                   "Object.Object_id_wl = " + vars_form.id_wl_object_for_test + " and " +
                                                                   "Object.idObject=Object_idObject and " +
                                                                   "Subscription_idSubscr=idSubscr and " +
                                                                   "products_has_Tarif_idproducts_has_Tarif=idproducts_has_Tarif;");

            if (get_produt_testing_device == "10" || get_produt_testing_device == "11")
            {
                if (checkBox_block_engine.Checked == true)
                {
                    string cmd = macros.wialon_request_lite("&svc=unit/exec_cmd&params={" +
                                                            "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\"," +
                                                            "\"commandName\":\"3 - СТОП двигатель\"," +
                                                            "\"linkType\":\"tcp\"," +
                                                            "\"param\":\"\"," +
                                                            "\"timeout\":\"0\"," +
                                                            "\"flags\":\"0\"}");
                }
                else
                {
                    string cmd = macros.wialon_request_lite("&svc=unit/exec_cmd&params={" +
                                                            "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\"," +
                                                            "\"commandName\":\"3 - СТАРТ двигатель\"," +
                                                            "\"linkType\":\"tcp\"," +
                                                            "\"param\":\"\"," +
                                                            "\"timeout\":\"0\"," +
                                                            "\"flags\":\"0\"}");
                }
            }
            else if (get_produt_testing_device == "2" || get_produt_testing_device == "3")
            {
                if (checkBox_block_engine.Checked == true)
                {
                    string cmd = macros.wialon_request_lite("&svc=unit/exec_cmd&params={" +
                                                            "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\"," +
                                                            "\"commandName\":\"2. СТОП Двигатель\"," +
                                                            "\"linkType\":\"tcp\"," +
                                                            "\"param\":\"\"," +
                                                            "\"timeout\":\"0\"," +
                                                            "\"flags\":\"0\"}");
                }
                else
                {
                    string cmd = macros.wialon_request_lite("&svc=unit/exec_cmd&params={" +
                                                            "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\"," +
                                                            "\"commandName\":\"1. СТАРТ Двигатель\"," +
                                                            "\"linkType\":\"tcp\"," +
                                                            "\"param\":\"\"," +
                                                            "\"timeout\":\"0\"," +
                                                            "\"flags\":\"0\"}");
                }
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
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

        private void checkBox_test_zablocovano_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_test_tk.Checked is true & checkBox_test_zablocovano.Checked is true & 
                 checkBox_test_du.Checked is true & checkBox__test_vzlom.Checked is true & checkBox_test_koordinati.Checked is true )
            {
                wl_group_activ_checkBox.Checked = true;
                panel_move_active.BackColor = Color.YellowGreen;
            }
            else
            {
                wl_group_activ_checkBox.Checked = false;
                panel_move_active.BackColor = Color.Empty;
            }
        }

        private void checkBox_test_sirene_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_test_tk.Checked is true & checkBox_test_zablocovano.Checked is true  & 
                checkBox_test_du.Checked is true & checkBox__test_vzlom.Checked is true & checkBox_test_koordinati.Checked is true )
            {
                wl_group_activ_checkBox.Checked = true;
                panel_move_active.BackColor = Color.YellowGreen;
                comboBox_result.SelectedIndex = 0;
            }
            else
            {
                wl_group_activ_checkBox.Checked = false;
                panel_move_active.BackColor = Color.Empty;
                comboBox_result.SelectedIndex = 1;
            }
        }

        private void checkBox__test_vzlom_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_test_tk.Checked is true & checkBox_test_zablocovano.Checked is true  & 
                 checkBox_test_du.Checked is true & checkBox__test_vzlom.Checked is true & checkBox_test_koordinati.Checked is true)
            {
                wl_group_activ_checkBox.Checked = true;
                panel_move_active.BackColor = Color.YellowGreen;
                comboBox_result.SelectedIndex = 0;
            }
            else
            {
                wl_group_activ_checkBox.Checked = false;
                panel_move_active.BackColor = Color.Empty;
                comboBox_result.SelectedIndex = 1;
            }
        }

        private void checkBox_test_tk_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_test_tk.Checked is true & checkBox_test_zablocovano.Checked is true  & 
                checkBox_test_du.Checked is true & checkBox__test_vzlom.Checked is true & checkBox_test_koordinati.Checked is true )
            {
                wl_group_activ_checkBox.Checked = true;
                panel_move_active.BackColor = Color.YellowGreen;
                comboBox_result.SelectedIndex = 0;
            }
            else
            {
                wl_group_activ_checkBox.Checked = false;
                panel_move_active.BackColor = Color.Empty;
                comboBox_result.SelectedIndex = 1;
            }
        }

        private void checkBox_test_koordinati_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_test_tk.Checked is true & checkBox_test_zablocovano.Checked is true & 
                 checkBox_test_du.Checked is true & checkBox__test_vzlom.Checked is true & checkBox_test_koordinati.Checked is true )
            {
                wl_group_activ_checkBox.Checked = true;
                panel_move_active.BackColor = Color.YellowGreen;
                comboBox_result.SelectedIndex = 0;
            }
            else
            {
                wl_group_activ_checkBox.Checked = false;
                panel_move_active.BackColor = Color.Empty;
                comboBox_result.SelectedIndex = 1;
            }
        }

        private void wl_group_activ_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (wl_group_activ_checkBox.Checked is true)
            {
                panel_move_active.BackColor = Color.YellowGreen;
            }
            else
            {
                panel_move_active.BackColor = Color.Empty;
            }
        }

        // escape non utf charter
        private void textBox_licence_plate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (System.Text.Encoding.UTF8.GetByteCount(new char[] { e.KeyChar }) > 1)
            {
                e.Handled = true;
            }
        }

        
        private void button_autostart_Click(object sender, EventArgs e)
        {
            //get product of testing device
            string get_produt_testing_device = macros.sql_command("select " +
                                                                   "products_has_Tarif.products_idproducts " +
                                                                   "from " +
                                                                   "btk.object_subscr, btk.Subscription, btk.products_has_Tarif, btk.Object " +
                                                                   "where " +
                                                                   "Object.Object_id_wl = " + vars_form.id_wl_object_for_test + " and " +
                                                                   "Object.idObject=Object_idObject and " +
                                                                   "Subscription_idSubscr=idSubscr and " +
                                                                   "products_has_Tarif_idproducts_has_Tarif=idproducts_has_Tarif;");

            if (get_produt_testing_device == "10" || get_produt_testing_device == "11")
            {
                string cmd = macros.wialon_request_lite("&svc=unit/exec_cmd&params={" +
                                                        "\"itemId\":\"" + vars_form.id_wl_object_for_test + "\"," +
                                                        "\"commandName\":\"2 - Автозапуск старт\"," +
                                                        "\"linkType\":\"tcp\"," +
                                                        "\"param\":\"\"," +
                                                        "\"timeout\":\"0\"," +
                                                        "\"flags\":\"0\"}");
            }
        }

        private void Testing_FormClosing(object sender, FormClosingEventArgs e)
        {
            aTimer3.Enabled = false;
        }
    }
}
    