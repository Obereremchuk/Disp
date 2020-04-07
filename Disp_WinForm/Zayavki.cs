using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            // Строим список Продуктов
            TimeSpan ts2 = new TimeSpan(23, 59, 59);
            dateTimePicker_filter_tested_zayavki.Value = DateTime.Now.Date + ts2;
            
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
                                            "testing_object_idtesting_object " +
                                            "FROM btk.Zayavki " +
                                            "where Zayavkicol_name = '" + vars_form.id_testing_for_zayavki+"';");

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
                textBox_vin_zayavki.Text= table.Rows[0][4].ToString();

                int Selected = -1;
                int count = comboBox_brand_zayavki.Items.Count;
                for (int i = 0; (i <= (count - 1)); i++)
                {
                    comboBox_brand_zayavki.SelectedIndex = i;
                    if ((int)(comboBox_brand_zayavki.SelectedValue) == Convert.ToInt32( table.Rows[0][6].ToString()))
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


                comboBox_date_vipuska_zayavki.SelectedIndex=comboBox_date_vipuska_zayavki.FindStringExact(table.Rows[0][13].ToString());
                textBox_sobstvennik_avto.Text = table.Rows[0][16].ToString();
                textBox_Coments.Text= table.Rows[0][14].ToString();


                
                textBox_kont_osoba1.Text= table.Rows[0][17].ToString();
                textBox_tel1.Text= table.Rows[0][18].ToString();
                textBox_kont_osoba2.Text= table.Rows[0][19].ToString();
                textBox_tel2.Text= table.Rows[0][20].ToString();
                idZayavki= table.Rows[0][0].ToString();
                
                string name_testing_added = macros.sql_command("SELECT Object_idObject from btk.testing_object where idtesting_object = '" + table.Rows[0][23].ToString() + "'");
                textBox_selected_object.Text = macros.sql_command("SELECT Object_name FROM btk.Object where idObject = '" + name_testing_added + "'");
                textBox_id_testing.Text = macros.sql_command("SELECT idtesting_object from btk.testing_object where idtesting_object = '" + table.Rows[0][23].ToString() + "'");

            }
        }

        private void buid_datagreed_tested_object()
        {
            //ПОлучим последний созданный айди тестирование, и возмем из него дату для верного отображения которую покладем в даттаймпикер
            DataTable table = new DataTable();
            if (checkBox_createt_zayavka.Checked == false)
            {
                table = macros.GetData("SELECT " +
                                            "idtesting_object as Id, " +
                                            "Object_idObject as idWl," +
                                            "testing_object.idtesting_object as '№Тестування'," +
                                            "Object.Object_name as Назва," +
                                            "TS_info.TS_infocol_vin as VIN," +
                                            "Object.Object_imei as IMEI," +
                                            "Kontragenti.Kontragenti_full_name as Установник," +
                                            "testing_objectcol_edit_timestamp as Дата_тестування," +
                                            "Users.username as Користувач " +
                                            "FROM btk.testing_object, btk.Object, btk.TS_info, btk.Kontragenti, btk.Users " +
                                            "WHERE " +
                                            "testing_object.Users_idUsers=Users.idUsers and " +
                                            "testing_object.idtesting_object NOT IN (select testing_object_idtesting_object from btk.Zayavki) and " +
                                            "Kontragenti.idKontragenti=TS_info.Kontragenti_idKontragenti and " +
                                            "Object.TS_info_idTS_info=TS_info.idTS_info and " +
                                            "Object.idObject = testing_object.Object_idObject and " +
                                            "(testing_objectcol_edit_timestamp between '" + Convert.ToDateTime(dateTimePicker_filter_tested_zayavki.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "');");
                
            }
            else if (checkBox_createt_zayavka.Checked == true)
            {
                table = macros.GetData("SELECT " +
                                            "idtesting_object as Id, " +
                                            "Object_idObject as idWl," +
                                            "testing_object.idtesting_object as '№Тестування'," +
                                            "Object.Object_name as Назва," +
                                            "TS_info.TS_infocol_vin as VIN," +
                                            "Object.Object_imei as IMEI," +
                                            "Kontragenti.Kontragenti_full_name as Установник," +
                                            "testing_objectcol_edit_timestamp as Дата_тестування," +
                                            "Users.username as Користувач " +
                                            "FROM btk.testing_object, btk.Object, btk.TS_info, btk.Kontragenti, btk.Users " +
                                            "where " +
                                            "testing_object.Users_idUsers=Users.idUsers and " +
                                            "Kontragenti.idKontragenti=TS_info.Kontragenti_idKontragenti and " +
                                            "Object.TS_info_idTS_info=TS_info.idTS_info and " +
                                            "Object.idObject = testing_object.Object_idObject and " +
                                            "(testing_objectcol_edit_timestamp between '" + Convert.ToDateTime(dateTimePicker_filter_tested_zayavki.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "');");
                

                
            }
            dataGridView_tested_objects_zayavki.DataSource = table;
            this.dataGridView_tested_objects_zayavki.Columns["Id"].Visible = false;
            this.dataGridView_tested_objects_zayavki.Columns["idWl"].Visible = false;

        }

        private void build_list_color()
        {
            // Строим список Продуктов
            this.comboBox_product_zayavki.DataSource = macros.GetData("SELECT idproducts, full_name FROM btk.products;");
            this.comboBox_product_zayavki.DisplayMember = "full_name";
            this.comboBox_product_zayavki.ValueMember = "idproducts";

            // Строим список брен авто - Имя=бренд, Значение=айди
            this.comboBox_brand_zayavki.DataSource = macros.GetData("SELECT idTS_brand, TS_brandcol_brand FROM btk.TS_brand;");
            this.comboBox_brand_zayavki.DisplayMember = "TS_brandcol_brand";
            this.comboBox_brand_zayavki.ValueMember = "idTS_brand";

            // Строим список модель авто - Имя=модель, Значение=айди
            this.comboBox_model_zayavki.DataSource = macros.GetData("SELECT idTS_model, TS_modelcol_name FROM btk.TS_model;");
            this.comboBox_model_zayavki.DisplayMember = "TS_modelcol_name";
            this.comboBox_model_zayavki.ValueMember = "idTS_model";

            // Строим список год выпуска авто - Имя=год, Значение=айди
            this.comboBox_date_vipuska_zayavki.DataSource = macros.GetData("SELECT idProduction_date, Production_datecol_date FROM btk.Production_date ORDER BY Production_datecol_date DESC;");
            this.comboBox_date_vipuska_zayavki.DisplayMember = "Production_datecol_date";
            this.comboBox_date_vipuska_zayavki.ValueMember = "idProduction_date";

        }// Строим список цветов авто - Имя=цвет, Значение=айди цвета

        private void comboBox_brand_zayavki_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("SELECT TS_brandcol_brand, idTS_brand FROM btk.TS_brand;");
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
            Kontragents form_Kontragents = new Kontragents();
            form_Kontragents.Activated += new EventHandler(form_Kontragents_zakazchik_activated);
            form_Kontragents.FormClosed += new FormClosedEventHandler(form_Kontragents_zakazchik_deactivated);
            form_Kontragents.Show();
            vars_form.select_sto_or_zakazchik_for_zayavki = 0;
        }
        private void form_Kontragents_zakazchik_activated(object sender, EventArgs e)
        {
            this.Visible = false;// блокируем окно контрагентов пока открыто окно добавления контрагента
            
        }
        private void form_Kontragents_zakazchik_deactivated(object sender, FormClosedEventArgs e)
        {
            this.Visible = true;// разблокируем окно контрагентов кактолько закрыто окно добавления контрагента
            if (vars_form.select_sto_or_zakazchik_for_zayavki == 0)
            {
                textBox_kontragent_zakazchik.Text = macros.sql_command("SELECT Kontragenti_full_name FROM btk.Kontragenti where idKontragenti ='" + vars_form.id_kontragent_zakazchik_for_zayavki + "';");
            }
        }

        private void dateTimePicker_filter_tested_zayavki_ValueChanged(object sender, EventArgs e)
        {
            buid_datagreed_tested_object();
        }

        private void checkBox_createt_zayavka_CheckedChanged(object sender, EventArgs e)
        {
            buid_datagreed_tested_object();
        }

        private void checkBox_activovano_CheckedChanged(object sender, EventArgs e)
        {
            buid_datagreed_tested_object();
        }

        private void button_create_Click(object sender, EventArgs e)
        {
            if (vars_form.if_open_created_zayavka == 1)
            {
                // insert activation

                if (textBox_selected_object.Text == "")
                {
                    idtesting_object = "1";
                }

                macros.sql_command("UPDATE btk.Zayavki set " +
                                                        "Zayavkicol_name = '" + textBox_name_zayavka.Text + "'," +
                                                        "Zayavkicol_plan_date = '" + Convert.ToDateTime(dateTimePicker_filter_tested_zayavki.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                        "Zayavkicol_reason = '" + comboBox_reason_zayavki.GetItemText(comboBox_reason_zayavki.SelectedItem) + "'," +
                                                        "Zayavkicol_VIN = '" + textBox_vin_zayavki.Text + "'," +
                                                        "TS_model_idTS_model = '" + comboBox_model_zayavki.SelectedValue + "'," +
                                                        "TS_brand_idTS_brand = '" + comboBox_brand_zayavki.SelectedValue + "'," +
                                                        "products_idproducts = '" + comboBox_product_zayavki.SelectedValue + "'," +
                                                        "Kontragenti_idKontragenti_sto = '" + vars_form.id_kontragent_sto_for_zayavki + "'," +
                                                        "Zayavkicol_license_plate = '" + textBox_license_plate_zayavki.Text + "'," +
                                                        "Kontragenti_idKontragenti_zakazchik = '" + vars_form.id_kontragent_zakazchik_for_zayavki + "'," +
                                                        "Zayavkicol_data_vipuska = '" + comboBox_date_vipuska_zayavki.GetItemText(comboBox_date_vipuska_zayavki.SelectedItem) + "'," +
                                                        "Zayavkicol_comment = '" + textBox_Coments.Text + "'," +
                                                        "Zayavkicol_edit_timestamp = '" + Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                        "testing_object_idtesting_object = '" + idtesting_object + "'," +
                                                        "Sobstvennik_avto_neme = '" + textBox_sobstvennik_avto.Text + "'," +
                                                        "Kontakt_name_avto_1 = '" + textBox_kont_osoba1.Text + "'," +
                                                        "Kontakt_phone_avto_1 = '" + textBox_tel1.Text + "'," +
                                                        "Kontakt_name_avto_2 = '" + textBox_kont_osoba2.Text + "'," +
                                                        "Kontakt_phone_avto_2 = '" + textBox_tel2.Text + "'," +
                                                        "Users_idUsers = '" + vars_form.user_login_id + "'" +
                                                        "WHERE idZayavki = '"+ idZayavki + "'");

            }
            else
            {   // insert activation

                if (textBox_selected_object.Text == "")
                {
                    idtesting_object = "1";
                }
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
                                                                        "Activation_object_idActivation_object" +
                                                                        ") " +
                                                                        "values (" +
                                                                        "'" + textBox_name_zayavka.Text + "'," +
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
                                                                        "'" + textBox_Coments.Text + "'," +
                                                                        "'" + Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                                                                        "'" + idtesting_object + "'," +
                                                                        "'" + textBox_sobstvennik_avto.Text + "'," +
                                                                        "'" + textBox_kont_osoba1.Text + "'," +
                                                                        "'" + textBox_tel1.Text + "'," +
                                                                        "'" + textBox_kont_osoba2.Text + "'," +
                                                                        "'" + textBox_tel2.Text + "'," +
                                                                        "'" + vars_form.user_login_id + "'," +
                                                                        "'1'" +
                                                                        ");");
                this.Close();
            }
        }

        private void dataGridView_tested_objects_zayavki_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            textBox_selected_object.Text = dataGridView_tested_objects_zayavki.Rows[e.RowIndex].Cells[3].Value.ToString(); //ID об"єкту(8)
            idtesting_object = dataGridView_tested_objects_zayavki.Rows[e.RowIndex].Cells[0].Value.ToString();
            textBox_id_testing.Text= dataGridView_tested_objects_zayavki.Rows[e.RowIndex].Cells[2].Value.ToString();
        }
    }
}
