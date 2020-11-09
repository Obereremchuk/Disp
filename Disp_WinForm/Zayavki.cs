using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Disp_WinForm
{
    public partial class Zayavki : Form
    {
        Macros macros = new Macros();
        string idZayavki = "";
        string idtesting_object = "";
        public Zayavki()
        {
            //TopMost = true;
            InitializeComponent();
            comboBox_filter.SelectedIndex = 0;
            // Строим список Продуктов
            TimeSpan ts2 = new TimeSpan(00, 00, 00);
            dateTimePicker_filter_tested_zayavki.Value = DateTime.Now.Date + ts2;
            comboBox_reason_zayavki.SelectedIndex = 0;
            checkBox_for_all_time.Checked = true;


        }

        private void Zayavki_Load(object sender, EventArgs e)
        {
            build_list_color();
            buid_datagreed_tested_object();
            Read_data();

        }

        private void Read_data()
        {
            if (vars_form.if_open_created_zayavka == 1)
            {
                button_create.Text = "Оновити";
                DataTable table = new DataTable();
                table = macros.GetData("SELECT " +
                                            "idZayavki," +
                                            "Zayavkicol_name," +
                                            "Zayavkicol_plan_date," +
                                            "Zayavkicol_reason," +
                                            "Zayavkicol_VIN," +
                                            "TS_model_idTS_model," +
                                            "TS_brand_idTS_brand," +
                                            "products_idproducts," +
                                            "Kontragenti_idKontragenti_sto," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where idKontragenti=Kontragenti_idKontragenti_sto) as 'kontragent_sto'," +
                                            "Zayavkicol_license_plate," +
                                            "Kontragenti_idKontragenti_zakazchik," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where idKontragenti=Kontragenti_idKontragenti_zakazchik) as 'kontragent_zakazchik'," +
                                            "Zayavkicol_data_vipuska," +
                                            "Zayavkicol_comment," +
                                            "Zayavkicol_edit_timestamp," +
                                            "Sobstvennik_avto_neme," +
                                            "Kontakt_name_avto_1," +
                                            "Kontakt_phone_avto_1," +
                                            "Kontakt_name_avto_2," +
                                            "Kontakt_phone_avto_2," +
                                            "Users_idUsers," +
                                            "Activation_object_idActivation_object," +
                                            "testing_object_idtesting_object," +
                                            "email " +
                                            "FROM btk.Zayavki " +
                                            "where idZayavki = '" + vars_form.id_db_zayavki_for_activation + "';");

                textBox_name_zayavka.Text = table.Rows[0][1].ToString();
                dateTimePicker_plan_date_zayavki.Value = Convert.ToDateTime(table.Rows[0][2].ToString());
                if (table.Rows[0][3].ToString() == "Монтаж")
                {
                    comboBox_reason_zayavki.SelectedIndex = 0;
                }
                else if (table.Rows[0][3].ToString() == "Демонтаж")
                {
                    comboBox_reason_zayavki.SelectedIndex = 1;
                }
                else if (table.Rows[0][3].ToString() == "Інше")
                {
                    comboBox_reason_zayavki.SelectedIndex = 2;
                }
                textBox_vin_zayavki.Text = table.Rows[0][4].ToString();

                int Selected = -1;
                int count = comboBox_brand_zayavki.Items.Count;
                for (int i = 0; (i <= (count - 1)); i++)
                {
                    comboBox_brand_zayavki.SelectedIndex = i;
                    if ((int)(comboBox_brand_zayavki.SelectedValue) == Convert.ToInt32(table.Rows[0][6].ToString()))
                    {
                        Selected = i;
                        break;
                    }
                }

                Selected = -1;
                count = comboBox_model_zayavki.Items.Count;

                for (int i = 0; (i <= (count - 1)); i++)
                {
                    comboBox_model_zayavki.SelectedIndex = i;
                    if ((int)(comboBox_model_zayavki.SelectedValue) == Convert.ToInt32(table.Rows[0][5].ToString()))
                    {
                        Selected = i;
                        break;
                    }
                }
                comboBox_model_zayavki.SelectedIndex = Selected;


                Selected = -1;
                count = comboBox_product_zayavki.Items.Count;

                for (int i = 0; (i <= (count - 1)); i++)
                {
                    comboBox_product_zayavki.SelectedIndex = i;
                    if ((int)(comboBox_product_zayavki.SelectedValue) == Convert.ToInt32(table.Rows[0][7].ToString()))
                    {
                        Selected = i;
                        break;
                    }
                }
                comboBox_product_zayavki.SelectedIndex = Selected;





                vars_form.id_kontragent_sto_for_zayavki = table.Rows[0][8].ToString();
                textBox_kontragent_sto_zayavki.Text = macros.sql_command("SELECT Kontragenti_full_name FROM btk.Kontragenti where idKontragenti ='" + vars_form.id_kontragent_sto_for_zayavki + "';");

                textBox_license_plate_zayavki.Text = table.Rows[0][10].ToString();

                vars_form.id_kontragent_zakazchik_for_zayavki = table.Rows[0][11].ToString();
                textBox_kontragent_zakazchik.Text = macros.sql_command("SELECT Kontragenti_full_name FROM btk.Kontragenti where idKontragenti ='" + vars_form.id_kontragent_zakazchik_for_zayavki + "';");


                comboBox_date_vipuska_zayavki.SelectedIndex = comboBox_date_vipuska_zayavki.FindStringExact(table.Rows[0][13].ToString());
                textBox_sobstvennik_avto.Text = table.Rows[0][16].ToString();
                textBox_Coments.Text = table.Rows[0][14].ToString();



                textBox_kont_osoba1.Text = table.Rows[0][17].ToString();
                maskedTextBox_tel1.Text = table.Rows[0][18].ToString();
                textBox_kont_osoba2.Text = table.Rows[0][19].ToString();
                maskedTextBox_tel2.Text = table.Rows[0][20].ToString();
                idZayavki = table.Rows[0][0].ToString();
                textBox_email.Text = table.Rows[0][24].ToString();

                //string name_testing_added = macros.sql_command("SELECT Object_idObject from btk.testing_object where idtesting_object = '" + table.Rows[0][23].ToString() + "'");
                //textBox_selected_object.Text = macros.sql_command("SELECT Object_name FROM btk.Object where idObject = '" + name_testing_added + "'");
                idtesting_object = macros.sql_command("SELECT idtesting_object from btk.testing_object where idtesting_object = '" + table.Rows[0][23].ToString() + "'");
                if (idtesting_object != "1")
                {
                    try
                    {
                        //textBox_selected_object.Text = dataGridView_tested_objects_zayavki.Rows[e.RowIndex].Cells[3].Value.ToString(); //ID об"єкту(8)
                        //idtesting_object = dataGridView_tested_objects_zayavki.Rows[e.RowIndex].Cells[0].Value.ToString();
                        //textBox_id_testing.Text = dataGridView_tested_objects_zayavki.Rows[e.RowIndex].Cells[2].Value.ToString();

                        //string SelectetRowCellVin = dataGridView_tested_objects_zayavki.Rows[e.RowIndex].Cells[4].Value.ToString();
                        DataTable table2 = new DataTable();
                        table2 = macros.GetData("SELECT " +
                                                    "idtesting_object as Id, " +
                                                    "Object_idObject as idWl," +
                                                    "testing_object.idtesting_object as 'Тест'," +
                                                    "Object.Object_name as Назва," +
                                                    "TS_info.TS_infocol_vin as VIN," +
                                                    "Object.Object_imei as IMEI," +
                                                    "Kontragenti.Kontragenti_full_name as Установник," +
                                                    "testing_objectcol_edit_timestamp as Дата_тестування," +
                                                    "Users.username as Тестував " +
                                                    "FROM btk.testing_object, btk.Object, btk.TS_info, btk.Kontragenti, btk.Users " +
                                                    "WHERE " +
                                                    "testing_object.Users_idUsers=Users.idUsers and " +
                                                    "testing_object.idtesting_object =  '" + vars_form.id_testing_for_zayavki + "' and " +                                                    
                                                    "Kontragenti.idKontragenti=TS_info.Kontragenti_idKontragenti and " +
                                                    "Object.TS_info_idTS_info=TS_info.idTS_info and " +
                                                    "Object.idObject = testing_object.Object_idObject ;");
                        dataGridView_tested_objects_zayavki.DataSource = table2;
                        dataGridView_tested_objects_zayavki.DefaultCellStyle.BackColor = Color.LightGreen;
                        idtesting_object = dataGridView_tested_objects_zayavki.Rows[dataGridView_tested_objects_zayavki.CurrentCell.RowIndex].Cells[0].Value.ToString();
                        dataGridView_tested_objects_zayavki.ClearSelection();
                        dataGridView_tested_objects_zayavki.Columns["Id"].Visible = false;
                        dataGridView_tested_objects_zayavki.Columns["idWl"].Visible = false;
                    }
                    catch { }

                    dataGridView_tested_objects_zayavki.DefaultCellStyle.BackColor = Color.LightGreen;
                    dataGridView_tested_objects_zayavki.ClearSelection();
                }

            }
        }

        private void buid_datagreed_tested_object()
        {
            string searchbydate = "";
            if (checkBox_for_all_time.Checked == true)
            {
                searchbydate = " order by testing_objectcol_edit_timestamp DESC Limit 10;";
            }
            else
            {
                searchbydate = "and(testing_objectcol_edit_timestamp between '" + Convert.ToDateTime(dateTimePicker_filter_tested_zayavki.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "') order by testing_objectcol_edit_timestamp DESC;";
            }
            //ПОлучим последний созданный айди тестирование, и возмем из него дату для верного отображения которую покладем в даттаймпикер
            DataTable table = new DataTable();
            if (comboBox_filter.SelectedIndex==0)
            {
                table = macros.GetData("SELECT " +
                                            "idtesting_object as Id, " +
                                            "Object_idObject as idWl," +
                                            "testing_object.idtesting_object as 'Тест'," +
                                            "Object.Object_name as Назва," +
                                            "TS_info.TS_infocol_vin as VIN," +
                                            "Object.Object_imei as IMEI," +
                                            "Kontragenti.Kontragenti_full_name as Установник," +
                                            "testing_objectcol_edit_timestamp as Дата_тестування," +
                                            "Users.username as Тестував " +
                                            "FROM btk.testing_object, btk.Object, btk.TS_info, btk.Kontragenti, btk.Users " +
                                            "WHERE " +
                                            "testing_object.Users_idUsers=Users.idUsers and " +
                                            "TS_info.TS_infocol_vin LIKE  '%" + textBox_search_by_vin_testing.Text + "%' and " +
                                            "testing_object.idtesting_object NOT IN (select testing_object_idtesting_object from btk.Zayavki) and " +
                                            "Kontragenti.idKontragenti=TS_info.Kontragenti_idKontragenti and " +
                                            "Object.TS_info_idTS_info=TS_info.idTS_info and " +
                                            "Object.idObject = testing_object.Object_idObject " + searchbydate);

            }
            else if (comboBox_filter.SelectedIndex == 2)
            {
                table = macros.GetData("SELECT " +
                                            "idtesting_object as Id, " +
                                            "Object_idObject as idWl," +
                                            "testing_object.idtesting_object as 'Тест'," +
                                            "Object.Object_name as Назва," +
                                            "TS_info.TS_infocol_vin as VIN," +
                                            "Object.Object_imei as IMEI," +
                                            "Kontragenti.Kontragenti_full_name as Установник," +
                                            "testing_objectcol_edit_timestamp as Дата_тестування," +
                                            "Users.username as Тестував " +
                                            "FROM btk.testing_object, btk.Object, btk.TS_info, btk.Kontragenti, btk.Users " +
                                            "where " +
                                            "testing_object.Users_idUsers=Users.idUsers and " +
                                            "TS_info.TS_infocol_vin LIKE  '%" + textBox_search_by_vin_testing.Text + "%' and " +
                                            "Kontragenti.idKontragenti=TS_info.Kontragenti_idKontragenti and " +
                                            "Object.TS_info_idTS_info=TS_info.idTS_info and " +
                                            "Object.idObject = testing_object.Object_idObject " + searchbydate);
                                            
            }
            else if (comboBox_filter.SelectedIndex == 1)
            {
                table = macros.GetData("SELECT " +
                                            "idtesting_object as Id, " +
                                            "Object_idObject as idWl," +
                                            "testing_object.idtesting_object as 'Тест'," +
                                            "Object.Object_name as Назва," +
                                            "TS_info.TS_infocol_vin as VIN," +
                                            "Object.Object_imei as IMEI," +
                                            "Kontragenti.Kontragenti_full_name as Установник," +
                                            "testing_objectcol_edit_timestamp as Дата_тестування," +
                                            "Users.username as Тестував " +
                                            "FROM btk.testing_object, btk.Object, btk.TS_info, btk.Kontragenti, btk.Users " +
                                            "where " +
                                            "testing_object.Users_idUsers=Users.idUsers and " +
                                            "TS_info.TS_infocol_vin LIKE  '%" + textBox_search_by_vin_testing.Text + "%' and " +
                                            "testing_object.idtesting_object IN (select testing_object_idtesting_object from btk.Zayavki) and " +
                                            "Kontragenti.idKontragenti=TS_info.Kontragenti_idKontragenti and " +
                                            "Object.TS_info_idTS_info=TS_info.idTS_info and " +
                                            "Object.idObject = testing_object.Object_idObject " + searchbydate);
            }
            dataGridView_tested_objects_zayavki.DataSource = table;
            dataGridView_tested_objects_zayavki.DefaultCellStyle.BackColor = Color.White;
            this.dataGridView_tested_objects_zayavki.Columns["Id"].Visible = false;
            this.dataGridView_tested_objects_zayavki.Columns["idWl"].Visible = false;

        }

        private void build_list_color()
        {
            // Строим список Продуктов
            this.comboBox_product_zayavki.DataSource = macros.GetData("SELECT idproducts, full_name FROM btk.products order by full_name;");
            this.comboBox_product_zayavki.DisplayMember = "full_name";
            this.comboBox_product_zayavki.ValueMember = "idproducts";

            // Строим список брен авто - Имя=бренд, Значение=айди
            this.comboBox_brand_zayavki.DataSource = macros.GetData("SELECT idTS_brand, TS_brandcol_brand FROM btk.TS_brand order by TS_brandcol_brand;");
            this.comboBox_brand_zayavki.DisplayMember = "TS_brandcol_brand";
            this.comboBox_brand_zayavki.ValueMember = "idTS_brand";

            // Строим список модель авто - Имя=модель, Значение=айди
            this.comboBox_model_zayavki.DataSource = macros.GetData("SELECT idTS_model, TS_modelcol_name FROM btk.TS_model order by TS_modelcol_name;");
            this.comboBox_model_zayavki.DisplayMember = "TS_modelcol_name";
            this.comboBox_model_zayavki.ValueMember = "idTS_model";

            // Строим список год выпуска авто - Имя=год, Значение=айди
            this.comboBox_date_vipuska_zayavki.DataSource = macros.GetData("SELECT idProduction_date, Production_datecol_date FROM btk.Production_date ORDER BY Production_datecol_date DESC;");
            this.comboBox_date_vipuska_zayavki.DisplayMember = "Production_datecol_date";
            this.comboBox_date_vipuska_zayavki.ValueMember = "idProduction_date";

        }// Строим список цветов авто - Имя=цвет, Значение=айди цвета

        private void comboBox_brand_zayavki_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("SELECT TS_brandcol_brand, idTS_brand FROM btk.TS_brand order by TS_brandcol_brand;");
            var temp = macros.GetData(sql);
            comboBox_brand_zayavki.DataSource = null;
            comboBox_brand_zayavki.DisplayMember = "TS_brandcol_brand";
            comboBox_brand_zayavki.ValueMember = "idTS_brand";
            comboBox_brand_zayavki.DataSource = temp;
        }// При открытии комбобокса запрашиваем и отображаем перечень всех брендов

        private void comboBox_brand_zayavki_DropDownClosed(object sender, EventArgs e)
        {
            string sql = string.Format("SELECT TS_modelcol_name, idTS_model FROM btk.TS_model where TS_brand_idTS_brand =" + comboBox_brand_zayavki.SelectedValue.ToString() + ";");
            var temp = macros.GetData(sql);
            comboBox_model_zayavki.DataSource = null;
            comboBox_model_zayavki.DisplayMember = "TS_modelcol_name";
            comboBox_model_zayavki.ValueMember = "idTS_model";
            comboBox_model_zayavki.DataSource = temp;
        }//После выбора из списка необходимый бренд, по выбранному бренду запрашиваем и заполняем комбобокс моделями этого бренда

        private void button_select_kontragent_sto_zayavki_Click(object sender, EventArgs e)
        {
            vars_form.select_sto_or_zakazchik_for_zayavki = 1;
            Kontragents form_Kontragents = new Kontragents();
            form_Kontragents.Activated += new EventHandler(form_Kontragents_sto_activated);
            form_Kontragents.FormClosed += new FormClosedEventHandler(form_Kontragents_sto_deactivated);
            form_Kontragents.Show();

        }
        private void form_Kontragents_sto_activated(object sender, EventArgs e)
        {
            this.Visible = false;// блокируем окно контрагентов пока открыто окно добавления контрагента


        }
        private void form_Kontragents_sto_deactivated(object sender, FormClosedEventArgs e)
        {
            this.Visible = true;// разблокируем окно контрагентов кактолько закрыто окно добавления контрагента
            if (vars_form.select_sto_or_zakazchik_for_zayavki == 1)
            {
                textBox_kontragent_sto_zayavki.Text = macros.sql_command("SELECT Kontragenti_full_name FROM btk.Kontragenti where idKontragenti ='" + vars_form.id_kontragent_sto_for_zayavki + "';");
            }
        }

        private void button_select_kontraget_zakazchik_zayavki_Click(object sender, EventArgs e)
        {
            vars_form.select_sto_or_zakazchik_for_zayavki = 0;
            Kontragents form_Kontragents = new Kontragents();
            form_Kontragents.Activated += new EventHandler(form_Kontragents_zakazchik_activated);
            form_Kontragents.FormClosed += new FormClosedEventHandler(form_Kontragents_zakazchik_deactivated);
            form_Kontragents.Show();
            
        }
        private void form_Kontragents_zakazchik_activated(object sender, EventArgs e)
        {
            this.Visible = false;// блокируем окно контрагентов пока открыто окно добавления контрагента

        }
        private void form_Kontragents_zakazchik_deactivated(object sender, FormClosedEventArgs e)
        {
            this.Visible = true;// разблокируем окно контрагентов кактолько закрыто окно добавления контрагента
            //если окно было вызвано из заполнения поля заказчик то получаем выбранного контрагента
            if (vars_form.select_sto_or_zakazchik_for_zayavki == 0)
            {
                textBox_kontragent_zakazchik.Text = macros.sql_command("SELECT Kontragenti_full_name FROM btk.Kontragenti where idKontragenti ='" + vars_form.id_kontragent_zakazchik_for_zayavki + "';");
            }
        }

        private void button_create_Click(object sender, EventArgs e)
        {
            if (textBox_kontragent_zakazchik.Text == "" 
                || textBox_kontragent_sto_zayavki.Text == "" 
                || comboBox_product_zayavki.Text == ""
                || textBox_kontragent_sto_zayavki.Text == "" )
            {
                MessageBox.Show("Не заповнені обовязкові поля");
                return;
            }
            //если открыта созданная заявка выполняем этот сценалий
            if (vars_form.if_open_created_zayavka == 1)
            {
                // insert activation

                if (idtesting_object == "")
                {
                    idtesting_object = "1";
                }

                if (vars_form.id_kontragent_sto_for_zayavki == "")
                {
                    DialogResult dialogResult = MessageBox.Show("Не вибрано встановника", "Зберегти з пустим полем?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        vars_form.id_kontragent_sto_for_zayavki = "1";

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                if (vars_form.id_kontragent_zakazchik_for_zayavki == "")
                {
                    DialogResult dialogResult = MessageBox.Show("Не вибрано замовника", "Зберегти з пустим полем?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        vars_form.id_kontragent_zakazchik_for_zayavki = "1";

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                string phone1 = maskedTextBox_tel1.Text.Replace(" ", string.Empty);
                phone1 = phone1.Replace("-", string.Empty);
                phone1 = phone1.Replace("(", string.Empty);
                phone1 = phone1.Replace(")", string.Empty);

                string phone2 = maskedTextBox_tel2.Text.Replace(" ", string.Empty);
                phone2 = phone2.Replace("-", string.Empty);
                phone2 = phone2.Replace("(", string.Empty);
                phone2 = phone2.Replace(")", string.Empty);

                macros.sql_command("UPDATE btk.Zayavki set " +
                                                        "Zayavkicol_name = '" + MySqlHelper.EscapeString(textBox_name_zayavka.Text) + "'," +
                                                        "Zayavkicol_plan_date = '" + Convert.ToDateTime(dateTimePicker_plan_date_zayavki.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                        "Zayavkicol_reason = '" + comboBox_reason_zayavki.GetItemText(comboBox_reason_zayavki.SelectedItem) + "'," +
                                                        "Zayavkicol_VIN = '" + textBox_vin_zayavki.Text + "'," +
                                                        "TS_model_idTS_model = '" + comboBox_model_zayavki.SelectedValue + "'," +
                                                        "TS_brand_idTS_brand = '" + comboBox_brand_zayavki.SelectedValue + "'," +
                                                        "products_idproducts = '" + comboBox_product_zayavki.SelectedValue + "'," +
                                                        "Kontragenti_idKontragenti_sto = '" + vars_form.id_kontragent_sto_for_zayavki + "'," +
                                                        "Zayavkicol_license_plate = '" + textBox_license_plate_zayavki.Text + "'," +
                                                        "Kontragenti_idKontragenti_zakazchik = '" + vars_form.id_kontragent_zakazchik_for_zayavki + "'," +
                                                        "Zayavkicol_data_vipuska = '" + comboBox_date_vipuska_zayavki.GetItemText(comboBox_date_vipuska_zayavki.SelectedItem) + "'," +
                                                        "Zayavkicol_comment = '" + MySqlHelper.EscapeString(textBox_Coments.Text) + "'," +
                                                        "Zayavkicol_edit_timestamp = '" + Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                        "testing_object_idtesting_object = '" + idtesting_object + "'," +
                                                        "Sobstvennik_avto_neme = '" + textBox_sobstvennik_avto.Text + "'," +
                                                        "Kontakt_name_avto_1 = '" + MySqlHelper.EscapeString(textBox_kont_osoba1.Text) + "'," +
                                                        "Kontakt_phone_avto_1 = '" + phone1 + "'," +
                                                        "Kontakt_name_avto_2 = '" + MySqlHelper.EscapeString(textBox_kont_osoba2.Text) + "'," +
                                                        "Kontakt_phone_avto_2 = '" + phone2 + "'," +
                                                        "Users_idUsers = '" + vars_form.user_login_id + "'," +
                                                        "email = '" + MySqlHelper.EscapeString(textBox_email.Text) + "'" +
                                                        "WHERE idZayavki = '" + idZayavki + "'");


                string idActivation_object = macros.sql_command("SELECT Activation_object_idActivation_object FROM btk.Zayavki_has_Activation_object where Zayavki_idZayavki = '" + idZayavki + "';");

                string idObject = macros.sql_command("select Object_idObject from btk.testing_object where idtesting_object = '" + idtesting_object + "';");
                macros.sql_command("update btk.Activation_object set Object_idObject = " + idObject + " where idActivation_object = '" + idActivation_object + "';");


            }
            //если если создается новая заявка выполняем этот сценалий
            else
            {   // insert activation

                if (idtesting_object == "")
                {
                    idtesting_object = "1";
                }

                //if (textBox_id_testing.Text == "") 
                //{ textBox_id_testing.Text = "10"; }

                string id_object_from_testing = macros.sql_command("select Object_idObject from btk.testing_object where idtesting_object = '" + idtesting_object + "';");

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
                                                                    "comment" +
                                                                    ") " +
                                                                    "values (" +
                                                                    "'" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                    "'" + vars_form.user_login_id + "'," +
                                                                    "'"+ id_object_from_testing+ "'," +
                                                                    "'Не проводилось'," +
                                                                    "''," +
                                                                    "''," +
                                                                    "'1'," +
                                                                    "'1'," +
                                                                    "'1'," +
                                                                    "'1'," +
                                                                    "'1'," +
                                                                    "''," +
                                                                    "''," +
                                                                    "''" +
                                                                    ");");//Добавляем активацию для объекта и прикрепляем ее к заявке

                string id_created_activation = macros.sql_command("SELECT max(idActivation_object) FROM btk.Activation_object;");

                macros.sql_command("insert into btk.Zayavki (" +
                                                                        "Zayavkicol_name, " +
                                                                        "Zayavkicol_plan_date, " +
                                                                        "Zayavkicol_reason," +
                                                                        "Zayavkicol_VIN," +
                                                                        "TS_model_idTS_model," +
                                                                        "TS_brand_idTS_brand," +
                                                                        "products_idproducts," +
                                                                        "Kontragenti_idKontragenti_sto," +
                                                                        "Zayavkicol_license_plate," +
                                                                        "Kontragenti_idKontragenti_zakazchik," +
                                                                        "Zayavkicol_data_vipuska, " +
                                                                        "Zayavkicol_comment," +
                                                                        "Zayavkicol_edit_timestamp," +
                                                                        "testing_object_idtesting_object," +
                                                                        "Sobstvennik_avto_neme," +
                                                                        "Kontakt_name_avto_1," +
                                                                        "Kontakt_phone_avto_1," +
                                                                        "Kontakt_name_avto_2," +
                                                                        "Kontakt_phone_avto_2," +
                                                                        "Users_idUsers," +
                                                                        "email," +
                                                                        "Activation_object_idActivation_object" +
                                                                        ") " +
                                                                        "values (" +
                                                                        "'" + MySqlHelper.EscapeString(textBox_name_zayavka.Text) + "'," +
                                                                        "'" + Convert.ToDateTime(dateTimePicker_filter_tested_zayavki.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                        "'" + comboBox_reason_zayavki.GetItemText(comboBox_reason_zayavki.SelectedItem) + "'," +
                                                                        "'" + textBox_vin_zayavki.Text + "'," +
                                                                        "'" + comboBox_model_zayavki.SelectedValue + "'," +
                                                                        "'" + comboBox_brand_zayavki.SelectedValue + "'," +
                                                                        "'" + comboBox_product_zayavki.SelectedValue + "'," +
                                                                        "'" + vars_form.id_kontragent_sto_for_zayavki + "'," +
                                                                        "'" + textBox_license_plate_zayavki.Text + "'," +
                                                                        "'" + vars_form.id_kontragent_zakazchik_for_zayavki + "'," +
                                                                        "'" + comboBox_date_vipuska_zayavki.GetItemText(comboBox_date_vipuska_zayavki.SelectedItem) + "'," +
                                                                        "'" + MySqlHelper.EscapeString(textBox_Coments.Text) + "'," +
                                                                        "'" + Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                        "'" + idtesting_object + "'," +
                                                                        "'" + textBox_sobstvennik_avto.Text + "'," +
                                                                        "'" + MySqlHelper.EscapeString(textBox_kont_osoba1.Text) + "'," +
                                                                        "'" + maskedTextBox_tel1.Text + "'," +
                                                                        "'" + MySqlHelper.EscapeString(textBox_kont_osoba2.Text) + "'," +
                                                                        "'" + maskedTextBox_tel2.Text + "'," +
                                                                        "'" + vars_form.user_login_id + "'," +
                                                                        "'" + textBox_email.Text + "'," +
                                                                        "'" + id_created_activation + "'" +
                                                                        ");");

                string id_created_zayavka = macros.sql_command("SELECT max(idZayavki) FROM btk.Zayavki;");

                macros.sql_command("insert into btk.Zayavki_has_Activation_object (" +
                                                                        "Zayavki_idZayavki, " +
                                                                        "Activation_object_idActivation_object" +
                                                                        ") " +
                                                                        "values (" +
                                                                        "'" + id_created_zayavka + "'," +
                                                                        "'" + id_created_activation + "'" +
                                                                        ");");


            }
            this.Close();
        }

        private void dataGridView_tested_objects_zayavki_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            try
            {
                //textBox_selected_object.Text = dataGridView_tested_objects_zayavki.Rows[e.RowIndex].Cells[3].Value.ToString(); //ID об"єкту(8)
                //idtesting_object = dataGridView_tested_objects_zayavki.Rows[e.RowIndex].Cells[0].Value.ToString();
                //textBox_id_testing.Text = dataGridView_tested_objects_zayavki.Rows[e.RowIndex].Cells[2].Value.ToString();

                string SelectetRowCellVin = dataGridView_tested_objects_zayavki.Rows[e.RowIndex].Cells[4].Value.ToString();
                DataTable table = new DataTable();
                table = macros.GetData("SELECT " +
                                            "idtesting_object as Id, " +
                                            "Object_idObject as idWl," +
                                            "testing_object.idtesting_object as 'Тест'," +
                                            "Object.Object_name as Назва," +
                                            "TS_info.TS_infocol_vin as VIN," +
                                            "Object.Object_imei as IMEI," +
                                            "Kontragenti.Kontragenti_full_name as Установник," +
                                            "testing_objectcol_edit_timestamp as Дата_тестування," +
                                            "Users.username as Тестував " +
                                            "FROM btk.testing_object, btk.Object, btk.TS_info, btk.Kontragenti, btk.Users " +
                                            "WHERE " +
                                            "testing_object.Users_idUsers=Users.idUsers and " +
                                            "testing_object.idtesting_object =  '" + dataGridView_tested_objects_zayavki.Rows[e.RowIndex].Cells[0].Value.ToString() + "' and " +
                                            "testing_object.idtesting_object NOT IN (select testing_object_idtesting_object from btk.Zayavki) and " +
                                            "Kontragenti.idKontragenti=TS_info.Kontragenti_idKontragenti and " +
                                            "Object.TS_info_idTS_info=TS_info.idTS_info and " +
                                            "Object.idObject = testing_object.Object_idObject ;");
                dataGridView_tested_objects_zayavki.DataSource = table;
                dataGridView_tested_objects_zayavki.DefaultCellStyle.BackColor = Color.LightGreen;
                idtesting_object = dataGridView_tested_objects_zayavki.Rows[dataGridView_tested_objects_zayavki.CurrentCell.RowIndex].Cells[0].Value.ToString();
                dataGridView_tested_objects_zayavki.ClearSelection();
                this.dataGridView_tested_objects_zayavki.Columns["Id"].Visible = false;
                this.dataGridView_tested_objects_zayavki.Columns["idWl"].Visible = false;
            }
            catch { }


        }

        private void comboBox_filter_DropDownClosed(object sender, EventArgs e)
        {
            buid_datagreed_tested_object();
        }

        private void dateTimePicker_filter_tested_zayavki_CloseUp(object sender, EventArgs e)
        {
            buid_datagreed_tested_object();
        }

        private void button_vidkripyty_Click(object sender, EventArgs e)
        {
            //textBox_selected_object.Text = ""; //ID об"єкту(8)
            idtesting_object = "";
            textBox_search_by_vin_testing.Text = "";
            buid_datagreed_tested_object();
            //textBox_id_testing.Text = "";
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

        private void textBox_search_by_vin_testing_TextChanged(object sender, EventArgs e)
        {
            buid_datagreed_tested_object();
        }

        private void checkBox_for_all_time_CheckedChanged(object sender, EventArgs e)
        {
            buid_datagreed_tested_object();
        }
    }
}
