using MySql.Data.MySqlClient;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;


namespace Disp_WinForm
{

    public partial class Main_window : Form
    {
        Macros macros = new Macros();
        private static System.Timers.Timer aTimer;

        delegate void UpdateGridHandler(DataTable table);
        delegate void UpdateGridThreadHandler(DataTable table);


        public Main_window()
        {
            this.Font = new System.Drawing.Font("Arial", vars_form.setting_font_size);


            InitializeComponent();
            this.Text = "Disp v." + vars_form.version;
            comboBox_activovani_select.SelectedIndex = 0;
            comboBox_prikripleno_select.SelectedIndex = 0;
            comboBox_activation_filter.SelectedIndex = 0;
            comboBox_testing_filter.SelectedIndex = 0;



            aTimer = new System.Timers.Timer();

            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            TimeSpan ts1 = new TimeSpan(00, 00, 0);

            dateTimePicker_testing_date.Value = DateTime.Now.Date + ts1;
            dateTimePicker_for_zayavki_na_activation_W2.Value = DateTime.Now.Date + ts1;
            dateTimePicker_activation_filter_start.Value = DateTime.Now.Date;
            dateTimePicker_activation_filter_end.Value = DateTime.Now.Date;

            TimeSpan ts2 = new TimeSpan(23, 59, 59);

            dateTime_rep_from.Value = DateTime.Now.Date + ts1;
            dateTime_rep_to.Value = DateTime.Now.Date + ts2;


            //dataGridView_for_activation.DefaultCellStyle.SelectionBackColor = Color.White;
            //dataGridView_for_activation.DefaultCellStyle.SelectionForeColor = Color.Black;


        }

        private void init()
        {
            //// Set the DataGridView control's border.
            //dataGridView_for_activation.BorderStyle = BorderStyle.Fixed3D;
            //// Put the cells in edit mode when user enters them.
            //dataGridView_for_activation.EditMode = DataGridViewEditMode.EditOnEnter;

            dataGridView_808_n.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView_808_n.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            // Set the DataGridView control's border.
            dataGridView_808_n.BorderStyle = BorderStyle.Fixed3D;
            // Put the cells in edit mode when user enters them.
            dataGridView_808_n.EditMode = DataGridViewEditMode.EditOnEnter;

            dataGridView_lost.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView_lost.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            // Set the DataGridView control's border.
            dataGridView_lost.BorderStyle = BorderStyle.Fixed3D;
            // Put the cells in edit mode when user enters them.
            dataGridView_lost.EditMode = DataGridViewEditMode.EditOnEnter;

            dataGridView_909_n.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView_909_n.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            // Set the DataGridView control's border.
            dataGridView_909_n.BorderStyle = BorderStyle.Fixed3D;
            // Put the cells in edit mode when user enters them.
            dataGridView_909_n.EditMode = DataGridViewEditMode.EditOnEnter;

            dataGridView_dilery.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView_dilery.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            // Set the DataGridView control's border.
            dataGridView_dilery.BorderStyle = BorderStyle.Fixed3D;
            // Put the cells in edit mode when user enters them.
            dataGridView_dilery.EditMode = DataGridViewEditMode.EditOnEnter;

            dataGridView_sales.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView_sales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            // Set the DataGridView control's border.
            dataGridView_sales.BorderStyle = BorderStyle.Fixed3D;
            // Put the cells in edit mode when user enters them.
            dataGridView_sales.EditMode = DataGridViewEditMode.EditOnEnter;

            dataGridView_open_alarm.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView_open_alarm.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            // Set the DataGridView control's border.
            dataGridView_open_alarm.BorderStyle = BorderStyle.Fixed3D;
            // Put the cells in edit mode when user enters them.
            dataGridView_open_alarm.EditMode = DataGridViewEditMode.EditOnEnter;

            dataGridView_testing.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView_testing.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            // Set the DataGridView control's border.
            dataGridView_testing.BorderStyle = BorderStyle.Fixed3D;
            // Put the cells in edit mode when user enters them.
            dataGridView_testing.EditMode = DataGridViewEditMode.EditOnEnter;

            vars_form.order_sort = "desc";
            vars_form.hide_group_alarm = " and group_alarm is null";
            comboBox_sort_column.SelectedIndexChanged -= new System.EventHandler(this.comboBox_sort_column_SelectedIndexChanged);
            comboBox_sort_column.SelectedItem = "Час тривоги";
            comboBox_sort_column.SelectedIndexChanged += new System.EventHandler(this.comboBox_sort_column_SelectedIndexChanged);
            vars_form.sort = "msg_time";
            vars_form.sort_close = "msg_time";

            
        }

        private void accsses()
        {
            DataTable users_accses = macros.GetData("SELECT idUsers, username, accsess_lvl FROM btk.Users;");
            foreach (DataRow user in users_accses.Rows)
            {
                if (user["idUsers"].ToString() == vars_form.user_login_id)
                {
                    if (Convert.ToInt32(user["accsess_lvl"]) <= 5)
                    {
                    }
                    else if (Convert.ToInt32(user["accsess_lvl"]) == 8)
                    {
                        tabControl_testing.TabPages.Remove(tabPage3);
                    }
                    else 
                    {
                        tabControl_testing.TabPages.Remove(tabPage3);
                        tabControl_testing.TabPages.Remove(tabPage_zvit);
                        textBox_activation_search.Enabled = false;
                    }
                }
            }

            //if (vars_form.user_login_id == "5" || vars_form.user_login_id == "9" || vars_form.user_login_id == "34")//разрешаем Ленику, Пустовит доступ к вкладке Звит
            //{

            //}
            //else
            //{
            //    tabControl_testing.TabPages.Remove(tabPage3);
            //    tabControl_testing.TabPages.Remove(tabPage_zvit);
            //    textBox_activation_search.Enabled = false;
            //}

            //if (vars_form.user_login_id == "6" || vars_form.user_login_id == "2")//разрешаем Лозинскоу доступ к вкладке зупинки обслуговування
            //{

            //}
            //else
            //{
            //    tabControl_testing.TabPages.Remove(tabPage3);
            //}





        }// Прячм вкладки если ты не из касты

        private void SetTimer()
        {
            update_open_dgv();
            aTimer.Interval = 3000;
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent_open);
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }//Запускаем таймер на 2 сек 

        private void OnTimedEvent_808(object sender, EventArgs e)
        {
            update_808_dgv();
        }
        private void OnTimedEvent_lost(object sender, EventArgs e)
        {
            update_lost_dgv();
        }
        private void OnTimedEvent_909(object sender, EventArgs e)
        {
            update_909_dgv();
        }
        private void OnTimedEvent_open(object sender, EventArgs e)
        {
            update_open_dgv();
        }
        private void OnTimedEvent_sale(object sender, EventArgs e)
        {
            update_sale_dgv();
        }
        private void OnTimedEvent_dilery(object sender, EventArgs e)
        {
            update_dilery_dgv();
        }
        private void OnTimedEvent_testing(object sender, EventArgs e)
        {
            update_testing_dgv();
        }
        private void OnTimedEvent_activation(object sender, EventArgs e)
        {
            update_actication_dgv();
        }
        private void OnTimedEvent_zayavki_na_aktivation(object sender, EventArgs e)
        {
            update_zayavki_na_aktivation_2W();
        }

        private void tabControl_testing_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (tabControl_testing.SelectedTab.Name == "tabPage_808")
            {
                update_808_dgv();
                aTimer.Interval = 2000;
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage_lost")
            {
                update_lost_dgv();
                aTimer.Interval = 2000;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage_909_n")
            {
                update_909_dgv();
                aTimer.Interval = 2000;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage_sales")
            {
                update_sale_dgv();
                aTimer.Interval = 2000;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage_p")
            {
                update_dilery_dgv();
                aTimer.Interval = 2000;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage1")
            {
                update_open_dgv();
                aTimer.Interval = 2000;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage_testing")
            {
                
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.Enabled = false;
                update_testing_dgv();


            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage2")
            {
                mysql_close_alarm();
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.Enabled = false;
            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage_activation")
            {
                update_actication_dgv();
                //aTimer.Interval = 2000;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                
                //aTimer.AutoReset = true;
                aTimer.Enabled = false;
                

            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage_zayavki_activation")
            {
                update_zayavki_na_aktivation_2W();
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.Enabled = false;
            }


        }

        /// Обновляем вкладку zayavki Activation W2 
        /// 
        private void update_zayavki_na_aktivation_2W()
        {
            //ПОлучим последний созданный айди тестирование, и возмем из него дату для верного отображения которую покладем в даттаймпикер
            
            DataTable table = new DataTable();

            if (comboBox_activovani_select.SelectedIndex == 0 & comboBox_prikripleno_select.SelectedIndex == 0)// Отбор в заявках всі/всі
            {
                table = macros.GetData("SELECT " +
                                            "Zayavkicol_name as 'Назва'," +
                                            "Zayavkicol_plan_date as 'План дата'," +
                                            "Zayavkicol_reason as 'Причина'," +
                                            "Zayavkicol_VIN as'VIN'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti) as 'Встановник'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_zakazchik=Kontragenti.idKontragenti) as 'Замовник'," +
                                            "Users.username as 'Створив'," +
                                            "Zayavki.testing_object_idtesting_object as 'Привязаний до тестування'," +
                                            "Activation_object.Activation_objectcol_result as 'Статус активації'," +
                                            "Activation_object.Activation_date as 'Дата активації'" +
                                            "FROM btk.Zayavki, btk.Kontragenti, btk.Users, btk.Activation_object, btk.testing_object " +
                                            "where Zayavki.Activation_object_idActivation_object=Activation_object.idActivation_object " +
                                            "AND testing_object.idtesting_object=Zayavki.testing_object_idtesting_object " +
                                            "AND Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti " +
                                            "AND Zayavki.Users_idUsers=Users.idUsers " +
                                            "AND (Zayavkicol_edit_timestamp between '" + Convert.ToDateTime(dateTimePicker_for_zayavki_na_activation_W2.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "');");
            }
            else if (comboBox_activovani_select.SelectedIndex == 1 & comboBox_prikripleno_select.SelectedIndex == 1)//Отбор активованы /Прикриплени
            {
                table = macros.GetData("SELECT " +
                                            "Zayavkicol_name as 'Назва', " +
                                            "Zayavkicol_plan_date as 'План дата'," +
                                            "Zayavkicol_reason as 'Причина'," +
                                            "Zayavkicol_VIN as'VIN'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti) as 'Встановник'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_zakazchik=Kontragenti.idKontragenti) as 'Замовник'," +
                                            "Users.username as 'Створив'," +
                                            "Zayavki.testing_object_idtesting_object as 'Привязаний до тестування'," +
                                            "Activation_object.Activation_objectcol_result as 'Статус активації'," +
                                            "Activation_object.Activation_date as 'Дата активації'" +
                                            "FROM btk.Zayavki, btk.Kontragenti, btk.Users, btk.Activation_object, btk.testing_object " +
                                            "where Zayavki.Activation_object_idActivation_object=Activation_object.idActivation_object " +
                                            "AND testing_object.idtesting_object=Zayavki.testing_object_idtesting_object " +
                                            "AND Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti " +
                                            "AND Zayavki.Users_idUsers=Users.idUsers " +
                                            "AND Activation_object.Activation_objectcol_result = 'Успінно' " +
                                            "AND (Zayavkicol_edit_timestamp between '" + Convert.ToDateTime(dateTimePicker_for_zayavki_na_activation_W2.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "') " +
                                            "and testing_object_idtesting_object != 1;");
            }
            else if (comboBox_activovani_select.SelectedIndex == 2 & comboBox_prikripleno_select.SelectedIndex == 2)//не активованы/не прикриплени
            {
                table = macros.GetData("SELECT " +
                                            "Zayavkicol_name as 'Назва', " +
                                            "Zayavkicol_plan_date as 'План дата'," +
                                            "Zayavkicol_reason as 'Причина'," +
                                            "Zayavkicol_VIN as'VIN'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti) as 'Встановник'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_zakazchik=Kontragenti.idKontragenti) as 'Замовник'," +
                                            "Users.username as 'Створив'," +
                                            "Zayavki.testing_object_idtesting_object as 'Привязаний до тестування'," +
                                            "Activation_object.Activation_objectcol_result as 'Статус активації'," +
                                            "Activation_object.Activation_date as 'Дата активації'" +
                                            "FROM btk.Zayavki, btk.Kontragenti, btk.Users, btk.Activation_object, btk.testing_object " +
                                            "where Zayavki.Activation_object_idActivation_object=Activation_object.idActivation_object " +
                                            "AND testing_object.idtesting_object=Zayavki.testing_object_idtesting_object " +
                                            "AND Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti " +
                                            "AND Zayavki.Users_idUsers=Users.idUsers " +
                                            "AND Activation_object.Activation_objectcol_result != 'Успінно' " +
                                            "AND (Zayavkicol_edit_timestamp between '" + Convert.ToDateTime(dateTimePicker_for_zayavki_na_activation_W2.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "') " +
                                            "and testing_object_idtesting_object = 1;");
            }
            else if (comboBox_activovani_select.SelectedIndex == 1 & comboBox_prikripleno_select.SelectedIndex == 2) //активовані /непрікріплені
            {
                table = macros.GetData("SELECT " +
                                            "Zayavkicol_name as 'Назва', " +
                                            "Zayavkicol_plan_date as 'План дата'," +
                                            "Zayavkicol_reason as 'Причина'," +
                                            "Zayavkicol_VIN as'VIN'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti) as 'Встановник'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_zakazchik=Kontragenti.idKontragenti) as 'Замовник'," +
                                            "Users.username as 'Створив'," +
                                            "Zayavki.testing_object_idtesting_object as 'Привязаний до тестування'," +
                                            "Activation_object.Activation_objectcol_result as 'Статус активації'," +
                                            "Activation_object.Activation_date as 'Дата активації'" +
                                            "FROM btk.Zayavki, btk.Kontragenti, btk.Users, btk.Activation_object, btk.testing_object " +
                                            "where Zayavki.Activation_object_idActivation_object=Activation_object.idActivation_object " +
                                            "AND testing_object.idtesting_object=Zayavki.testing_object_idtesting_object " +
                                            "AND Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti " +
                                            "AND Zayavki.Users_idUsers=Users.idUsers " +
                                            "AND Activation_object.Object_idObject != '10' " +
                                            "AND (Zayavkicol_edit_timestamp between '" + Convert.ToDateTime(dateTimePicker_for_zayavki_na_activation_W2.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "') " +
                                            "and testing_object_idtesting_object = 1;");

            }
            else if (comboBox_activovani_select.SelectedIndex == 2 & comboBox_prikripleno_select.SelectedIndex == 1)//неактивовані/прікріплені
            {
                table = macros.GetData("SELECT " +
                                            "Zayavkicol_name as 'Назва', " +
                                            "Zayavkicol_plan_date as 'План дата'," +
                                            "Zayavkicol_reason as 'Причина'," +
                                            "Zayavkicol_VIN as'VIN'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti) as 'Встановник'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_zakazchik=Kontragenti.idKontragenti) as 'Замовник'," +
                                            "Users.username as 'Створив'," +
                                            "Zayavki.testing_object_idtesting_object as 'Привязаний до тестування'," +
                                            "Activation_object.Activation_objectcol_result as 'Статус активації'," +
                                            "Activation_object.Activation_date as 'Дата активації'" +
                                            "FROM btk.Zayavki, btk.Kontragenti, btk.Users, btk.Activation_object, btk.testing_object " +
                                            "where Zayavki.Activation_object_idActivation_object=Activation_object.idActivation_object " +
                                            "AND testing_object.idtesting_object=Zayavki.testing_object_idtesting_object " +
                                            "AND Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti " +
                                            "AND Zayavki.Users_idUsers=Users.idUsers " +
                                            "AND Activation_object.Activation_objectcol_result != 'Успінно' " +
                                            "AND (Zayavkicol_edit_timestamp between '" + Convert.ToDateTime(dateTimePicker_for_zayavki_na_activation_W2.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "') " +
                                            "and testing_object_idtesting_object != 1;");
            }
            else if (comboBox_activovani_select.SelectedIndex == 0 & comboBox_prikripleno_select.SelectedIndex == 1) //всі/прікріплені
            {
                table = macros.GetData("SELECT " +
                                            "Zayavkicol_name as 'Назва', " +
                                            "Zayavkicol_plan_date as 'План дата'," +
                                            "Zayavkicol_reason as 'Причина'," +
                                            "Zayavkicol_VIN as'VIN'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti) as 'Встановник'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_zakazchik=Kontragenti.idKontragenti) as 'Замовник'," +
                                            "Users.username as 'Створив'," +
                                            "Zayavki.testing_object_idtesting_object as 'Привязаний до тестування'," +
                                            "Activation_object.Activation_objectcol_result as 'Статус активації'," +
                                            "Activation_object.Activation_date as 'Дата активації'" +
                                            "FROM btk.Zayavki, btk.Kontragenti, btk.Users, btk.Activation_object, btk.testing_object " +
                                            "where Zayavki.Activation_object_idActivation_object=Activation_object.idActivation_object " +
                                            "AND testing_object.idtesting_object=Zayavki.testing_object_idtesting_object " +
                                            "AND Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti " +
                                            "AND Zayavki.Users_idUsers=Users.idUsers " +
                                            "AND (Zayavkicol_edit_timestamp between '" + Convert.ToDateTime(dateTimePicker_for_zayavki_na_activation_W2.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "') " +
                                            "and testing_object_idtesting_object != 1;");
            }
            else if (comboBox_activovani_select.SelectedIndex == 1 & comboBox_prikripleno_select.SelectedIndex == 0)//активовані/всі
            {
                table = macros.GetData("SELECT " +
                                            "Zayavkicol_name as 'Назва', " +
                                            "Zayavkicol_plan_date as 'План дата'," +
                                            "Zayavkicol_reason as 'Причина'," +
                                            "Zayavkicol_VIN as'VIN'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti) as 'Встановник'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_zakazchik=Kontragenti.idKontragenti) as 'Замовник'," +
                                            "Users.username as 'Створив'," +
                                            "Zayavki.testing_object_idtesting_object as 'Привязаний до тестування'," +
                                            "Activation_object.Activation_objectcol_result as 'Статус активації'," +
                                            "Activation_object.Activation_date as 'Дата активації'" +
                                            "FROM btk.Zayavki, btk.Kontragenti, btk.Users, btk.Activation_object, btk.testing_object " +
                                            "where Zayavki.Activation_object_idActivation_object=Activation_object.idActivation_object " +
                                            "AND testing_object.idtesting_object=Zayavki.testing_object_idtesting_object " +
                                            "AND Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti " +
                                            "AND Zayavki.Users_idUsers=Users.idUsers " +
                                            "AND Activation_object.Activation_objectcol_result = 'Успінно' " +
                                            "AND (Zayavkicol_edit_timestamp between '" + Convert.ToDateTime(dateTimePicker_for_zayavki_na_activation_W2.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "');");
            }
            else if (comboBox_activovani_select.SelectedIndex == 0 & comboBox_prikripleno_select.SelectedIndex == 2)//всі/непрікріплено
            {
                table = macros.GetData("SELECT " +
                                            "Zayavkicol_name as 'Назва', " +
                                            "Zayavkicol_plan_date as 'План дата'," +
                                            "Zayavkicol_reason as 'Причина'," +
                                            "Zayavkicol_VIN as'VIN'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti) as 'Встановник'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_zakazchik=Kontragenti.idKontragenti) as 'Замовник'," +
                                            "Users.username as 'Створив'," +
                                            "Zayavki.testing_object_idtesting_object as 'Привязаний до тестування'," +
                                            "Activation_object.Activation_objectcol_result as 'Статус активації'," +
                                            "Activation_object.Activation_date as 'Дата активації'" +
                                            "FROM btk.Zayavki, btk.Kontragenti, btk.Users, btk.Activation_object, btk.testing_object " +
                                            "where Zayavki.Activation_object_idActivation_object=Activation_object.idActivation_object " +
                                            "AND testing_object.idtesting_object=Zayavki.testing_object_idtesting_object " +
                                            "AND Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti " +
                                            "AND Zayavki.Users_idUsers=Users.idUsers " +
                                            "AND (Zayavkicol_edit_timestamp between '" + Convert.ToDateTime(dateTimePicker_for_zayavki_na_activation_W2.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "') " +
                                            "and testing_object_idtesting_object = 1;");
            }
            else if (comboBox_activovani_select.SelectedIndex == 2 & comboBox_prikripleno_select.SelectedIndex == 0)//Неактивовано /всі
            {
                table = macros.GetData("SELECT " +
                                            "Zayavkicol_name as 'Назва', " +
                                            "Zayavkicol_plan_date as 'План дата'," +
                                            "Zayavkicol_reason as 'Причина'," +
                                            "Zayavkicol_VIN as'VIN'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti) as 'Встановник'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_zakazchik=Kontragenti.idKontragenti) as 'Замовник'," +
                                            "Users.username as 'Створив'," +
                                            "Zayavki.testing_object_idtesting_object as 'Привязаний до тестування'," +
                                            "Activation_object.Activation_objectcol_result as 'Статус активації'," +
                                            "Activation_object.Activation_date as 'Дата активації'" +
                                            "FROM btk.Zayavki, btk.Kontragenti, btk.Users, btk.Activation_object, btk.testing_object " +
                                            "where Zayavki.Activation_object_idActivation_object=Activation_object.idActivation_object " +
                                            "AND testing_object.idtesting_object=Zayavki.testing_object_idtesting_object " +
                                            "AND Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti " +
                                            "AND Zayavki.Users_idUsers=Users.idUsers " +
                                            "AND Activation_object.Activation_objectcol_result != 'Успінно' " +
                                            "AND (Zayavkicol_edit_timestamp between '" + Convert.ToDateTime(dateTimePicker_for_zayavki_na_activation_W2.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "') ;");
            }



            int scrollPosition = dataGridView_zayavki_na_activation.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
            dataGridView_zayavki_na_activation.DataSource = table;
            if (dataGridView_zayavki_na_activation.Rows.Count >= 1)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
            {
                if (scrollPosition == -1)
                {
                    scrollPosition = 0;
                }
                dataGridView_zayavki_na_activation.FirstDisplayedScrollingRowIndex = scrollPosition;
            }


        }



        /// Обновляем вкладку For Activation  
        private void update_actication_dgv()
        {
            //ПОлучим последний созданный айди тестирование, и возмем из него дату для верного отображения которую покладем в даттаймпикер
            DataTable table = new DataTable();
            if (comboBox_activation_filter.SelectedIndex == 0)
            {
                table = macros.GetData(
                    "SELECT " +
                    "Zayavki.idZayavki as 'Заявка №', " +
                    "Activation_object.Object_idObject, " +
                    "Activation_object.idActivation_object, " +
                    "Zayavki.Zayavkicol_name as 'Назва заявки', " +
                    "products.product_name as 'Продукт', " +
                    "TS_brand.TS_brandcol_brand as 'Бренд'," +
                    "TS_model.TS_modelcol_name as 'Модель'," +
                    "Zayavki.Zayavkicol_VIN as 'VIN'," +
                    "Activation_object.Activation_objectcol_result as 'Результат', " +
                    "Activation_object.new_name_obj as 'Назва обєкту', " +
                    "Activation_object.comment as 'Коментар', " +
                    "remaynder_activate as 'Нагадати', " +
                    "remayder_date as 'Дата нагадування' " +
                    "FROM " +
                    "btk.products," +
                    "btk.Activation_object," +
                    "btk.Zayavki," +
                    "btk.TS_brand," +
                    "btk.TS_model " +
                    "where " +
                    "Activation_object.Activation_objectcol_result != 'Успішно' " +
                    "and Activation_object.new_name_obj like '%" + textBox_search_object_name_activation.Text + "%' " +
                    "and Activation_object.Object_idObject != '10' " +
                    "and TS_brand.idTS_brand = Zayavki.TS_brand_idTS_brand " +
                    "and TS_model.idTS_model = Zayavki.TS_model_idTS_model " +
                    "and products.idproducts = Zayavki.products_idproducts " +
                    "and Zayavki.Activation_object_idActivation_object = Activation_object.idActivation_object " +
                    "order by idZayavki desc" +
                    "; ");
            }
            else if (comboBox_activation_filter.SelectedIndex == 1)
            {
                table = macros.GetData(
                    "SELECT " +
                    "Zayavki.idZayavki as 'Заявка №', " +
                    "Activation_object.Object_idObject, " +
                    "Activation_object.idActivation_object, " +
                    "Zayavki.Zayavkicol_name as 'Назва заявки', " +
                    "products.product_name as 'Продукт', " +
                    "TS_brand.TS_brandcol_brand as 'Бренд'," +
                    "TS_model.TS_modelcol_name as 'Модель'," +
                    "Zayavki.Zayavkicol_VIN as 'VIN'," +
                    "Activation_object.Activation_objectcol_result as 'Результат', " +
                    "Activation_object.new_name_obj as 'Назва обєкту', " +
                    "Activation_object.comment as 'Коментар', " +
                    "remaynder_activate as 'Нагадати', " +
                    "remayder_date as 'Дата нагадування' " +
                    "FROM " +
                    "btk.products," +
                    "btk.Activation_object," +
                    "btk.Zayavki," +
                    "btk.TS_brand," +
                    "btk.TS_model " +
                    "where " +
                    "Activation_object.Activation_objectcol_result = 'Успішно' " +
                    "and Activation_object.Activation_date between '" + Convert.ToDateTime(dateTimePicker_activation_filter_start.Value).Date.ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(dateTimePicker_activation_filter_end.Value).Date.ToString("yyyy-MM-dd") + "' " +
                    "and Activation_object.new_name_obj like '%" + textBox_search_object_name_activation.Text + "%' " +
                    "and Activation_object.Object_idObject != '10' " +
                    "and TS_brand.idTS_brand = Zayavki.TS_brand_idTS_brand " +
                    "and TS_model.idTS_model = Zayavki.TS_model_idTS_model " +
                    "and products.idproducts = Zayavki.products_idproducts " +
                    "and Zayavki.Activation_object_idActivation_object = Activation_object.idActivation_object " +
                    "order by idZayavki desc" +
                    "; ");
            }
            else if (comboBox_activation_filter.SelectedIndex == 2)
            {
                table = macros.GetData(
                    "SELECT " +
                    "Zayavki.idZayavki as 'Заявка №', " +
                    "Activation_object.Object_idObject, " +
                    "Activation_object.idActivation_object, " +
                    "Zayavki.Zayavkicol_name as 'Назва заявки', " +
                    "products.product_name as 'Продукт', " +
                    "TS_brand.TS_brandcol_brand as 'Бренд'," +
                    "TS_model.TS_modelcol_name as 'Модель'," +
                    "Zayavki.Zayavkicol_VIN as 'VIN'," +
                    "Activation_object.Activation_objectcol_result as 'Результат', " +
                    "Activation_object.new_name_obj as 'Назва обєкту', " +
                    "Activation_object.comment as 'Коментар', " +
                    "remaynder_activate as 'Нагадати', " +
                    "remayder_date as 'Дата нагадування' " +
                    "FROM " +
                    "btk.products," +
                    "btk.Activation_object," +
                    "btk.Zayavki," +
                    "btk.TS_brand," +
                    "btk.TS_model " +
                    "where " +
                    "Activation_object.Object_idObject != '10' " +
                    "and Activation_object.Activation_date between '" + Convert.ToDateTime(dateTimePicker_activation_filter_start.Value).Date.ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(dateTimePicker_activation_filter_end.Value).Date.ToString("yyyy-MM-dd") + "' " +
                    "and Activation_object.new_name_obj like '%"+ textBox_search_object_name_activation.Text + "%' " +
                    "and TS_brand.idTS_brand = Zayavki.TS_brand_idTS_brand " +
                    "and TS_model.idTS_model = Zayavki.TS_model_idTS_model " +
                    "and products.idproducts = Zayavki.products_idproducts " +
                    "and Zayavki.Activation_object_idActivation_object = Activation_object.idActivation_object " +
                    "order by idZayavki desc" +
                    "; ");

            }

            int scrollPosition = dataGridView_for_activation.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
            dataGridView_for_activation.DataSource = table;

            dataGridView_for_activation.Columns["Object_idObject"].Visible = false;
            dataGridView_for_activation.Columns["idActivation_object"].Visible = false;
            dataGridView_for_activation.Columns["Коментар"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView_for_activation.Columns["Нагадати"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_for_activation.Columns["Дата нагадування"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_for_activation.Columns["Заявка №"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_for_activation.Columns["Назва заявки"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_for_activation.Columns["Продукт"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_for_activation.Columns["Бренд"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_for_activation.Columns["Модель"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_for_activation.Columns["VIN"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_for_activation.Columns["Результат"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_for_activation.Columns["Назва обєкту"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_for_activation.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            if (dataGridView_for_activation.Rows.Count >= 1)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
            {
                if (scrollPosition == -1)
                {
                    scrollPosition = 0;
                }
                dataGridView_for_activation.FirstDisplayedScrollingRowIndex = scrollPosition;
            }
        }

        /// Обновляем вкладку Testing 
        private void update_testing_dgv()
        {
            DataTable table = new DataTable();

            if (comboBox_testing_filter.SelectedIndex == 1)
            {
                table = macros.GetData("SELECT " +
                                   "testing_object.idtesting_object as '№ тестування', " +
                                   "Object.Object_name as 'Назва обекту', " +
                                   "Object.Object_imei as 'IMEI', " +
                                   "Users.username as 'Змінив', " +
                                   "testing_object.testing_objectcol_result as 'Результат', " +
                                   "testing_object.testing_objectcol_edit_timestamp as 'Змінено', " +
                                   "testing_object.testing_objectcol_comments as 'Коментар' " +
                                   "FROM " +
                                   "btk.testing_object, " +
                                   "btk.Object, " +
                                   "btk.Users " +
                                   "where " +
                                   "testing_object.testing_objectcol_result='Успішно' and " +
                                   "testing_object.Object_idObject=Object.idObject and " +
                                   "testing_object.Users_idUsers=Users.idUsers and " +
                                   "(testing_object.testing_objectcol_edit_timestamp  between '" + Convert.ToDateTime(dateTimePicker_testing_date.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') and " +
                                   "Object.Object_imei like '%" + textBox_search_testing.Text + "%' ;");
            }
            else if (comboBox_testing_filter.SelectedIndex == 0)
            {
                table = macros.GetData("SELECT " +
                                   "testing_object.idtesting_object as '№ тестування', " +
                                   "Object.Object_name as 'Назва обекту', " +
                                   "Object.Object_imei as 'IMEI', " +
                                   "Users.username as 'Змінив', " +
                                   "testing_object.testing_objectcol_result as 'Результат', " +
                                   "testing_object.testing_objectcol_edit_timestamp as 'Змінено', " +
                                   "testing_object.testing_objectcol_comments as 'Коментар' " +
                                   "FROM " +
                                   "btk.testing_object, " +
                                   "btk.Object, " +
                                   "btk.Users " +
                                   "where " +
                                   "testing_object.testing_objectcol_result ='Не завершено' and " +
                                   "testing_object.Object_idObject=Object.idObject and " +
                                   "testing_object.Users_idUsers=Users.idUsers and " +
                                   "(testing_object.testing_objectcol_edit_timestamp  between '" + Convert.ToDateTime(dateTimePicker_testing_date.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') and " +
                                   "Object.Object_imei like '%" + textBox_search_testing.Text + "%' ;");
            }
            else if (comboBox_testing_filter.SelectedIndex == 2)
            {
                table = macros.GetData("SELECT " +
                                   "testing_object.idtesting_object as '№ тестування', " +
                                   "Object.Object_name as 'Назва обекту', " +
                                   "Object.Object_imei as 'IMEI', " +
                                   "Users.username as 'Змінив', " +
                                   "testing_object.testing_objectcol_result as 'Результат', " +
                                   "testing_object.testing_objectcol_edit_timestamp as 'Змінено', " +
                                   "testing_object.testing_objectcol_comments as 'Коментар' " +
                                   "FROM " +
                                   "btk.testing_object, " +
                                   "btk.Object, " +
                                   "btk.Users " +
                                   "where " +
                                   "testing_object.Object_idObject=Object.idObject and " +
                                   "testing_object.Users_idUsers=Users.idUsers and " +
                                   "(testing_object.testing_objectcol_edit_timestamp  between '" + Convert.ToDateTime(dateTimePicker_testing_date.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') and " +
                                   "Object.Object_imei like '%" + textBox_search_testing.Text + "%' ;");
            }

            int scrollPosition = dataGridView_testing.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
            dataGridView_testing.DataSource = table;


            dataGridView_testing.Columns["№ тестування"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_testing.Columns["Назва обекту"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_testing.Columns["IMEI"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_testing.Columns["Змінив"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_testing.Columns["Результат"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_testing.Columns["Змінено"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_testing.Columns["Коментар"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dataGridView_testing.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;


            if (dataGridView_testing.Rows.Count >= 1)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
            {
                if (scrollPosition == -1)
                {
                    scrollPosition = 0;
                }
                dataGridView_testing.FirstDisplayedScrollingRowIndex = scrollPosition;
            }

        }

        private void dataGridView_testing_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dataGridView_testing.SuspendLayout();
            if (dataGridView_testing.Rows.Count >= 1)
            {
                //if (dataGridView_testing.Rows[e.RowIndex].Cells[3].Value != null)
                //{
                //    if (dataGridView_testing.Rows[e.RowIndex].Cells[3].Value.ToString() == "1")
                //    {
                //        //dataGridView_testing.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                //        //dataGridView_testing.Rows[e.RowIndex].Cells[3].Value = 3;
                //        e.CellStyle.BackColor = Color.Empty;
                //    }
                //    else
                //    {
                //        dataGridView_testing.Rows[e.RowIndex].Cells[2].Value = "Не успішно";
                //        e.CellStyle.BackColor = Color.Pink;
                //    }
                //}


            }
            dataGridView_testing.ResumeLayout();
        }



        /// Обновляем вкладку Диллеры
        /// 
        private void update_dilery_dgv()
        {
            DataTable table = new DataTable();
            table = macros.GetData("SELECT idnotification as 'ID тривоги'," +
                                   "product as 'Продукт'," +
                                   "unit_name as 'Назва об’єкту'," +
                                   "type_alarm as 'Тип тривоги'," +
                                   "unit_id as 'ID'," +
                                   "Status as 'Статус'," +
                                   "alarm_locked_user as 'Обробляє'," +
                                   "notification.time_stamp as 'Дата зміни'," +
                                   "group_alarm as 'Згруповано до', " +
                                   "Users.username as 'Створив'," +
                                   "speed, " +
                                   "remaynder_activate as 'Нагадати', " +
                                   "remayder_date as 'Дата нагадування'  FROM btk.notification, btk.Users WHERE Users.idUsers=notification.Users_idUsers and Status = 'Дилеры' " + vars_form.hide_group_alarm + ";"); //order by btk.notification." + vars_form.sort.ToString() + " " + vars_form.order_sort + "
            //table = vars_form.table_dilery;
            UpdateGridHandler ug = UpdateGrid_dilery;
            ug.BeginInvoke(table, cb_dilery, null);
        }
        private void cb_dilery(IAsyncResult res)
        {

        }
        private void UpdateGrid1_dilery(DataTable table)
        {
            //save sort
            DataGridViewColumn oldColumn = dataGridView_dilery.SortedColumn;
            ListSortDirection direction;
            if (dataGridView_dilery.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
            else direction = ListSortDirection.Descending;

            //save scrol and selected row
            int scrollPosition = 0;
            int selectpozition = 0;
            if (dataGridView_dilery.DataSource != null)
            {
                scrollPosition = dataGridView_dilery.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
                
                try
                {
                    selectpozition = dataGridView_dilery.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                }
                catch (Exception)
                {
                    selectpozition = 0;
                }
            }

            DataView dv = table.DefaultView;
            dv.Sort = "Дата зміни desc";
            DataTable sortedDT = dv.ToTable();

            dataGridView_dilery.DataSource = table;

            //restote sort
            if (oldColumn != null)
            {
                DataGridViewColumn newColumn = dataGridView_dilery.Columns[oldColumn.Name.ToString()];
                dataGridView_dilery.Sort(newColumn, direction);
                newColumn.HeaderCell.SortGlyphDirection =
                                 direction == ListSortDirection.Ascending ?
                                 SortOrder.Ascending : SortOrder.Descending;
            }

            if (dataGridView_dilery.Rows.Count >= 1)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
            {
                if (scrollPosition == -1)
                {
                    scrollPosition = 0;
                }
                dataGridView_dilery.FirstDisplayedScrollingRowIndex = scrollPosition;
                dataGridView_dilery.ClearSelection();
                dataGridView_dilery.Rows[selectpozition].Selected = true;

                
            }
        }
        private void UpdateGrid_dilery(DataTable table)
        {
            if (dataGridView_dilery.InvokeRequired)
            {
                UpdateGridThreadHandler handler = UpdateGrid1_dilery;
                dataGridView_dilery.BeginInvoke(handler, table);
            }
            else
            {
                //save sort
                DataGridViewColumn oldColumn = dataGridView_dilery.SortedColumn;
                ListSortDirection direction;
                if (dataGridView_dilery.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
                else direction = ListSortDirection.Descending;

                //save scrol and selected row
                int scrollPosition = 0;
                int selectpozition = 0;
                if (dataGridView_dilery.DataSource != null)
                {
                    scrollPosition = dataGridView_dilery.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
                   
                    try
                    {
                        selectpozition = dataGridView_dilery.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                    }
                    catch (Exception)
                    {
                        selectpozition = 0;
                    }
                }

                DataView dv = table.DefaultView;
                dv.Sort = "Дата зміни desc";
                DataTable sortedDT = dv.ToTable();

                dataGridView_dilery.DataSource = table;

                //restote sort
                if (oldColumn != null)
                {
                    DataGridViewColumn newColumn = dataGridView_dilery.Columns[oldColumn.Name.ToString()];
                    dataGridView_dilery.Sort(newColumn, direction);
                    newColumn.HeaderCell.SortGlyphDirection =
                                     direction == ListSortDirection.Ascending ?
                                     SortOrder.Ascending : SortOrder.Descending;
                }

                if (dataGridView_dilery.Rows.Count >= 0)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
                {
                    if (scrollPosition == -1)
                    {
                        scrollPosition = 0;
                    }
                    dataGridView_dilery.FirstDisplayedScrollingRowIndex = scrollPosition;
                    dataGridView_dilery.ClearSelection();
                    dataGridView_dilery.Rows[selectpozition].Selected = true;

                    
                }
            }
        }
        private void dataGridView_dilery_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            if (dataGridView_dilery.Rows[e.RowIndex].Cells[9].Value.ToString() == "")//Згруповано до ID тривоги(9)
            {
                vars_form.search_id = dataGridView_dilery.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_dilery.Rows[e.RowIndex].Cells[0].Value.ToString();//ID Тривоги (0)
                vars_form.id_status = dataGridView_dilery.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_dilery.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_dilery.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_dilery.Rows[e.RowIndex].Cells[11].Value.ToString();//Звернення(4)
            }
            else
            {

                vars_form.search_id = dataGridView_dilery.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_dilery.Rows[e.RowIndex].Cells[0].Value.ToString();//Згруповано до ID тривоги(9)
                vars_form.id_status = dataGridView_dilery.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_dilery.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_dilery.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_dilery.Rows[e.RowIndex].Cells[10].Value.ToString();//Звернення(4)
                //var dataIndexNo = dataGridView1.Rows[e.RowIndex].Index.ToString();
                //string id_notif = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                // MessageBox.Show(cellValue);
            }


            DataTable results1 = macros.GetData("SELECT " +
                                                 "alarm_locked, " +
                                                 "alarm_locked_user " +
                                                 "FROM btk.notification " +
                                                 "WHERE " +
                                                 "idnotification = '" + vars_form.id_notif + "';");

            if (vars_form.user_login_name != "admin" & vars_form.user_login_name != results1.Rows[0][1].ToString())
            {
                if (results1.Rows[0][0].ToString() == "1")
                {
                    MessageBox.Show("Користувач: " + results1.Rows[0][0] + " вже опрацовуе тривогу.");
                    return;
                }
            }

            macros.GetData("UPDATE btk.notification " +
                           "SET " +
                           "alarm_locked = '1', " +
                           "alarm_locked_user = '" + vars_form.user_login_name + "' " +
                           "WHERE " +
                           "idnotification = '" + vars_form.id_notif + "';");


            detail subwindow = new detail();
            subwindow.Show();
        }
        private void dataGridView_dilery_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dataGridView_dilery.SuspendLayout();

            if (dataGridView_dilery.Rows[e.RowIndex].Cells[11].Value is true)
            {
                if (Convert.ToDateTime(dataGridView_dilery.Rows[e.RowIndex].Cells[12].Value) == DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#bcccf2");
                }
                else if (Convert.ToDateTime(dataGridView_dilery.Rows[e.RowIndex].Cells[12].Value) <= DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F9A780");
                }
                else if (Convert.ToDateTime(dataGridView_dilery.Rows[e.RowIndex].Cells[12].Value) >= DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#C8FAB7");
                }
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }

            dataGridView_dilery.Columns[10].Visible = false;
            dataGridView_dilery.Columns[8].Visible = false;
            dataGridView_dilery.Columns[4].Visible = false;
            dataGridView_dilery.ResumeLayout();
        }



        /// Обновляем вкладку Продажи
        /// 
        private void update_sale_dgv()
        {
            DataTable table = new DataTable();
            table = macros.GetData("SELECT idnotification as 'ID тривоги'," +
                                   "product as 'Продукт'," +
                                   "unit_name as 'Назва об’єкту'," +
                                   "type_alarm as 'Тип тривоги'," +
                                   "unit_id as 'ID'," +
                                   "Status as 'Статус'," +
                                   "alarm_locked_user as 'Обробляє'," +
                                   "notification.time_stamp as 'Дата зміни'," +
                                   "group_alarm as 'Згруповано до', " +
                                   "Users.username as 'Створив'," +
                                   "speed, " +
                                   "remaynder_activate as 'Нагадати', " +
                                   "remayder_date as 'Дата нагадування' " +
                                   "FROM btk.notification, btk.Users WHERE Users.idUsers=notification.Users_idUsers and Status = 'Продажи' " + vars_form.hide_group_alarm + "  ;");//order by btk.notification." + vars_form.sort.ToString() + " " + vars_form.order_sort + "
            //table = vars_form.table_sales;
            UpdateGridHandler ug = UpdateGrid_sale;
            ug.BeginInvoke(table, cb_sale, null);
        }
        private void cb_sale(IAsyncResult res)
        {

        }
        private void UpdateGrid1_sale(DataTable table)
        {
            //save sort
            DataGridViewColumn oldColumn = dataGridView_sales.SortedColumn;
            ListSortDirection direction;
            if (dataGridView_sales.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
            else direction = ListSortDirection.Descending;

            //save scrol and selected row
            int scrollPosition = 0;
            int selectpozition = 0;
            if (dataGridView_sales.DataSource != null)
            {
                scrollPosition = dataGridView_sales.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
                
                try
                {
                    selectpozition = dataGridView_sales.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                }
                catch (Exception)
                {
                    selectpozition = 0;
                }
            }

            DataView dv = table.DefaultView;
            dv.Sort = "Дата зміни desc";
            DataTable sortedDT = dv.ToTable();

            dataGridView_sales.DataSource = table;

            //restote sort
            if (oldColumn != null)
            {
                DataGridViewColumn newColumn = dataGridView_sales.Columns[oldColumn.Name.ToString()];
                dataGridView_sales.Sort(newColumn, direction);
                newColumn.HeaderCell.SortGlyphDirection =
                                 direction == ListSortDirection.Ascending ?
                                 SortOrder.Ascending : SortOrder.Descending;
            }

            if (dataGridView_sales.Rows.Count >= 0)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
            {
                if (scrollPosition == -1)
                {
                    scrollPosition = 0;
                }
                dataGridView_sales.FirstDisplayedScrollingRowIndex = scrollPosition;
                dataGridView_sales.ClearSelection();
                dataGridView_sales.Rows[selectpozition].Selected = true;

                
            }
        }
        private void UpdateGrid_sale(DataTable table)
        {
            if (dataGridView_sales.InvokeRequired)
            {
                UpdateGridThreadHandler handler = UpdateGrid1_sale;
                dataGridView_sales.BeginInvoke(handler, table);
            }
            else
            {
                //save sort
                DataGridViewColumn oldColumn = dataGridView_sales.SortedColumn;
                ListSortDirection direction;
                if (dataGridView_sales.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
                else direction = ListSortDirection.Descending;

                //save scrol and selected row
                int scrollPosition = 0;
                int selectpozition = 0;
                if (dataGridView_sales.DataSource != null)
                {
                    scrollPosition = dataGridView_sales.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
                    
                    try
                    {
                        selectpozition = dataGridView_sales.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                    }
                    catch (Exception)
                    {
                        selectpozition = 0;
                    }
                }

                DataView dv = table.DefaultView;
                dv.Sort = "Дата зміни desc";
                DataTable sortedDT = dv.ToTable();

                dataGridView_sales.DataSource = table;
                
                //restote sort
                if (oldColumn != null)
                {
                    DataGridViewColumn newColumn = dataGridView_sales.Columns[oldColumn.Name.ToString()];
                    dataGridView_sales.Sort(newColumn, direction);
                    newColumn.HeaderCell.SortGlyphDirection =
                                     direction == ListSortDirection.Ascending ?
                                     SortOrder.Ascending : SortOrder.Descending;
                }

                if (dataGridView_sales.Rows.Count >= 0)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
                {
                    if (scrollPosition == -1)
                    {
                        scrollPosition = 0;
                    }
                    dataGridView_sales.FirstDisplayedScrollingRowIndex = scrollPosition;
                    dataGridView_sales.ClearSelection();
                    dataGridView_sales.Rows[selectpozition].Selected = true;

                    
                }
            }
        }
        private void dataGridView_sales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            if (dataGridView_sales.Rows[e.RowIndex].Cells[9].Value.ToString() == "")//Згруповано до ID тривоги(9)
            {
                vars_form.search_id = dataGridView_sales.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_sales.Rows[e.RowIndex].Cells[0].Value.ToString();//ID Тривоги (0)
                vars_form.id_status = dataGridView_sales.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_sales.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_sales.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_sales.Rows[e.RowIndex].Cells[10].Value.ToString();//Звернення(4)
            }
            else
            {

                vars_form.search_id = dataGridView_sales.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_sales.Rows[e.RowIndex].Cells[0].Value.ToString();//Згруповано до ID тривоги(9)
                vars_form.id_status = dataGridView_sales.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_sales.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_sales.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_sales.Rows[e.RowIndex].Cells[10].Value.ToString();//Звернення(4)
                //var dataIndexNo = dataGridView1.Rows[e.RowIndex].Index.ToString();
                //string id_notif = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                // MessageBox.Show(cellValue);
            }

            // Блокируем обработку тревоги однвременно двумя операторами, и вносим информацию кто открыл тревогу
            DataTable results1 = macros.GetData("SELECT " +
                                                "alarm_locked, " +
                                                "alarm_locked_user " +
                                                "FROM btk.notification " +
                                                "WHERE " +
                                                "idnotification = '" + vars_form.id_notif + "';");

            if (vars_form.user_login_name != "admin" & vars_form.user_login_name != results1.Rows[0][1].ToString())
            {
                if (results1.Rows[0][0].ToString() == "1")
                {
                    MessageBox.Show("Користувач: " + results1.Rows[0][0] + " вже опрацовуе тривогу.");
                    return;
                }
            }

            macros.GetData("UPDATE btk.notification " +
                           "SET " +
                           "alarm_locked = '1', " +
                           "alarm_locked_user = '" + vars_form.user_login_name + "' " +
                           "WHERE " +
                           "idnotification = '" + vars_form.id_notif + "';");



            detail subwindow = new detail();
            subwindow.Show();
        }
        private void dataGridView_sales_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dataGridView_sales.SuspendLayout();

            if (dataGridView_sales.Rows[e.RowIndex].Cells[11].Value is true)
            {
                if (Convert.ToDateTime(dataGridView_sales.Rows[e.RowIndex].Cells[12].Value) == DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#bcccf2");
                }
                else if (Convert.ToDateTime(dataGridView_sales.Rows[e.RowIndex].Cells[12].Value) <= DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F9A780");
                }
                else if (Convert.ToDateTime(dataGridView_sales.Rows[e.RowIndex].Cells[12].Value) >= DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#C8FAB7");
                }
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }

            dataGridView_sales.Columns[10].Visible = false;
            dataGridView_sales.Columns[8].Visible = false;
            dataGridView_sales.Columns[4].Visible = false;
            dataGridView_sales.ResumeLayout();
        }



        /// Обновляем вкладку 808
        /// 
        private void update_808_dgv()
        {
            DataTable table = new DataTable();
            table = macros.GetData("SELECT idnotification as 'ID тривоги'," +
                                   "product as 'Продукт'," +
                                   "unit_name as 'Назва об’єкту'," +
                                   "type_alarm as 'Тип тривоги'," +
                                   "unit_id as 'ID'," +
                                   "Status as 'Статус'," +
                                   "alarm_locked_user as 'Обробляє'," +
                                   "notification.time_stamp as 'Дата зміни'," +
                                   "group_alarm as 'Згруповано до', " +
                                   "Users.username as 'Створив'," +
                                   "speed, " +
                                   "remaynder_activate as 'Нагадати', " +
                                   "remayder_date as 'Дата нагадування'  FROM btk.notification, btk.Users WHERE Users.idUsers=notification.Users_idUsers and Status = '808' " + vars_form.hide_group_alarm + "  ;");//order by btk.notification." + vars_form.sort.ToString() + " " + vars_form.order_sort + "
            //table = vars_form.table_808;
            UpdateGridHandler ug = UpdateGrid_808;
            ug.BeginInvoke(table, cb_808, null);
        }
        private void cb_808(IAsyncResult res)
        {

        }
        private void UpdateGrid1_808(DataTable table)
        {
            //save sort
            DataGridViewColumn oldColumn = dataGridView_808_n.SortedColumn;
            ListSortDirection direction;
            if (dataGridView_808_n.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
            else direction = ListSortDirection.Descending;

            //save scrol and selected row
            int scrollPosition = 0;
            int selectpozition = 0;
            if (dataGridView_808_n.DataSource != null)
            {
                scrollPosition = dataGridView_808_n.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
                
                try
                {
                    selectpozition = dataGridView_808_n.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                }
                catch (Exception)
                {
                    selectpozition = 0;
                }
            }

            DataView dv = table.DefaultView;
            dv.Sort = "Дата зміни desc";
            DataTable sortedDT = dv.ToTable();

            dataGridView_808_n.DataSource = table;
            ////------------------------------------------
            //if (dataGridView_808_n.DataSource == null)
            //{
            //    dataGridView_808_n.DataSource = table;
            //}
            //else
            //{
            //    dataGridView_808_n.Refresh();
            //}
            ////----------------------------------------------------------------------

            //restote sort
            if (oldColumn != null)
            {
                DataGridViewColumn newColumn = dataGridView_808_n.Columns[oldColumn.Name.ToString()];
                dataGridView_808_n.Sort(newColumn, direction);
                newColumn.HeaderCell.SortGlyphDirection =
                                 direction == ListSortDirection.Ascending ?
                                 SortOrder.Ascending : SortOrder.Descending;
            }

            if (dataGridView_808_n.Rows.Count >= 0)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
            {
                if (scrollPosition == -1)
                {
                    scrollPosition = 0;
                }
                dataGridView_808_n.FirstDisplayedScrollingRowIndex = scrollPosition;
                dataGridView_808_n.ClearSelection();
                dataGridView_808_n.Rows[selectpozition].Selected = true;

                
            }
        }
        private void UpdateGrid_808(DataTable table)
        {
            if (dataGridView_808_n.InvokeRequired)
            {
                UpdateGridThreadHandler handler = UpdateGrid1_808;
                dataGridView_808_n.BeginInvoke(handler, table);
            }
            else
            {
                //save sort
                DataGridViewColumn oldColumn = dataGridView_808_n.SortedColumn;
                ListSortDirection direction;
                if (dataGridView_808_n.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
                else direction = ListSortDirection.Descending;

                //save scrol and selected row
                int scrollPosition = 0;
                int selectpozition = 0;
                if (dataGridView_808_n.DataSource != null)
                {
                    scrollPosition = dataGridView_808_n.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
                    
                    try
                    {
                        selectpozition = dataGridView_808_n.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                    }
                    catch (Exception)
                    {
                        selectpozition = 0;
                    }
                }

                DataView dv = table.DefaultView;
                dv.Sort = "Дата зміни desc";
                DataTable sortedDT = dv.ToTable();

                dataGridView_808_n.DataSource = table;

                //restote sort
                if (oldColumn != null)
                {
                    DataGridViewColumn newColumn = dataGridView_808_n.Columns[oldColumn.Name.ToString()];
                    dataGridView_808_n.Sort(newColumn, direction);
                    newColumn.HeaderCell.SortGlyphDirection =
                                     direction == ListSortDirection.Ascending ?
                                     SortOrder.Ascending : SortOrder.Descending;
                }
                ////------------------------------------------
                //if (dataGridView_808_n.DataSource == null)
                //{
                //    dataGridView_808_n.DataSource = table;
                //}
                //else
                //{
                //    dataGridView_808_n.Refresh();
                //}
                ////----------------------------------------------------------------------
                if (dataGridView_808_n.Rows.Count >= 0)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
                {
                    if (scrollPosition == -1)
                    {
                        scrollPosition = 0;
                    }
                    dataGridView_808_n.FirstDisplayedScrollingRowIndex = scrollPosition;
                    dataGridView_808_n.ClearSelection();
                    dataGridView_808_n.Rows[selectpozition].Selected = true;

                    
                }
            }
        }
        private void dataGridView_808_n_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dataGridView_808_n.SuspendLayout();

            if (dataGridView_808_n.Rows[e.RowIndex].Cells[11].Value is true)
            {
                if (Convert.ToDateTime(dataGridView_808_n.Rows[e.RowIndex].Cells[12].Value) == DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#bcccf2");
                }
                else if (Convert.ToDateTime(dataGridView_808_n.Rows[e.RowIndex].Cells[12].Value) <= DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F9A780");
                }
                else if (Convert.ToDateTime(dataGridView_808_n.Rows[e.RowIndex].Cells[12].Value) >= DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#C8FAB7");
                }
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }

            dataGridView_808_n.Columns[10].Visible = false;
            dataGridView_808_n.Columns[8].Visible = false;
            dataGridView_808_n.Columns[4].Visible = false;
            dataGridView_808_n.ResumeLayout();
        }
        private void dataGridView_808_n_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            if (dataGridView_808_n.Rows[e.RowIndex].Cells[9].Value.ToString() == "")//Згруповано до ID тривоги(9)
            {
                vars_form.search_id = dataGridView_808_n.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_808_n.Rows[e.RowIndex].Cells[0].Value.ToString();//ID Тривоги (0)
                vars_form.id_status = dataGridView_808_n.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_808_n.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_808_n.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_808_n.Rows[e.RowIndex].Cells[11].Value.ToString();//Звернення(4)
            }
            else
            {

                vars_form.search_id = dataGridView_808_n.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_808_n.Rows[e.RowIndex].Cells[0].Value.ToString();//Згруповано до ID тривоги(9)
                vars_form.id_status = dataGridView_808_n.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_808_n.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_808_n.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_808_n.Rows[e.RowIndex].Cells[10].Value.ToString();//Звернення(4)
            }

            // Блокируем обработку тревоги однвременно двумя операторами, и вносим информацию кто открыл тревогу
            DataTable results1 = macros.GetData("SELECT " +
                                                "alarm_locked, " +
                                                "alarm_locked_user " +
                                                "FROM btk.notification " +
                                                "WHERE " +
                                                "idnotification = '" + vars_form.id_notif + "';");

            if (vars_form.user_login_name != "admin" & vars_form.user_login_name != results1.Rows[0][1].ToString())
            {
                if (results1.Rows[0][0].ToString() == "1")
                {
                    MessageBox.Show("Користувач: " + results1.Rows[0][0] + " вже опрацовуе тривогу.");
                    return;
                }
            }

            macros.GetData("UPDATE btk.notification " +
                           "SET " +
                           "alarm_locked = '1', " +
                           "alarm_locked_user = '" + vars_form.user_login_name + "' " +
                           "WHERE " +
                           "idnotification = '" + vars_form.id_notif + "';");

            detail subwindow = new detail();
            subwindow.Show();
        }


        /// Обновляем вкладку lost
        /// 
        private void update_lost_dgv()
        {
            DataTable table = new DataTable();
            table = macros.GetData("SELECT idnotification as 'ID тривоги'," +
                                   "product as 'Продукт'," +
                                   "unit_name as 'Назва об’єкту'," +
                                   "type_alarm as 'Тип тривоги'," +
                                   "unit_id as 'ID'," +
                                   "Status as 'Статус'," +
                                   "alarm_locked_user as 'Обробляє'," +
                                   "notification.time_stamp as 'Дата зміни'," +
                                   "group_alarm as 'Згруповано до', " +
                                   "Users.username as 'Створив'," +
                                   "speed, " +
                                   "remaynder_activate as 'Нагадати', " +
                                   "remayder_date as 'Дата нагадування'  FROM btk.notification, btk.Users WHERE Users.idUsers=notification.Users_idUsers and Status = '808_звязок' " + vars_form.hide_group_alarm + "  ;");//order by btk.notification." + vars_form.sort.ToString() + " " + vars_form.order_sort + "
            UpdateGridHandler ug = UpdateGrid_lost;
            ug.BeginInvoke(table, cb_lost, null);
        }
        private void cb_lost(IAsyncResult res)
        {

        }
        private void UpdateGrid1_lost(DataTable table)
        {
            //save sort
            DataGridViewColumn oldColumn = dataGridView_lost.SortedColumn;
            ListSortDirection direction;
            if (dataGridView_lost.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
            else direction = ListSortDirection.Descending;

            //save scrol and selected row
            int scrollPosition = 0;
            int selectpozition = 0;
            if (dataGridView_lost.DataSource != null)
            {
                scrollPosition = dataGridView_lost.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
                
                try
                {
                    selectpozition = dataGridView_lost.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                }
                catch (Exception)
                {
                    selectpozition = 0;
                }
            }


            DataView dv = table.DefaultView;
            dv.Sort = "Дата зміни desc";
            DataTable sortedDT = dv.ToTable();

            dataGridView_lost.DataSource = table;

            //restote sort
            if (oldColumn != null)
            {
                DataGridViewColumn newColumn = dataGridView_lost.Columns[oldColumn.Name.ToString()];
                dataGridView_lost.Sort(newColumn, direction);
                newColumn.HeaderCell.SortGlyphDirection =
                                 direction == ListSortDirection.Ascending ?
                                 SortOrder.Ascending : SortOrder.Descending;
            }

            if (dataGridView_lost.Rows.Count >= 1)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
            {
                if (scrollPosition == -1)
                {
                    scrollPosition = 0;
                }
                dataGridView_lost.FirstDisplayedScrollingRowIndex = scrollPosition;
                dataGridView_lost.ClearSelection();
                dataGridView_lost.Rows[selectpozition].Selected = true;

                
            }
        }
        private void UpdateGrid_lost(DataTable table)
        {
            if (dataGridView_lost.InvokeRequired)
            {
                UpdateGridThreadHandler handler = UpdateGrid1_lost;
                dataGridView_lost.BeginInvoke(handler, table);
            }
            else
            {
                //save sort
                DataGridViewColumn oldColumn = dataGridView_lost.SortedColumn;
                ListSortDirection direction;
                if (dataGridView_lost.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
                else direction = ListSortDirection.Descending;

                //save scrol and selected row
                int scrollPosition = 0;
                int selectpozition = 0;
                if (dataGridView_lost.DataSource != null)
                {
                    scrollPosition = dataGridView_lost.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
                    
                    try
                    {
                        selectpozition = dataGridView_lost.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                    }
                    catch (Exception)
                    {
                        selectpozition = 0;
                    }
                }

                DataView dv = table.DefaultView;
                dv.Sort = "Дата зміни desc";
                DataTable sortedDT = dv.ToTable();

                dataGridView_lost.DataSource = table;

                //restote sort
                if (oldColumn != null)
                {
                    DataGridViewColumn newColumn = dataGridView_lost.Columns[oldColumn.Name.ToString()];
                    dataGridView_lost.Sort(newColumn, direction);
                    newColumn.HeaderCell.SortGlyphDirection =
                                     direction == ListSortDirection.Ascending ?
                                     SortOrder.Ascending : SortOrder.Descending;
                }

                if (dataGridView_lost.Rows.Count >= 0)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
                {
                    if (scrollPosition == -1)
                    {
                        scrollPosition = 0;
                    }
                    dataGridView_lost.FirstDisplayedScrollingRowIndex = scrollPosition;
                    dataGridView_lost.ClearSelection();
                    dataGridView_lost.Rows[selectpozition].Selected = true;

                    
                }
            }
        }
        private void dataGridView_lost_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //return;
            dataGridView_lost.SuspendLayout();

            if (dataGridView_lost.Rows[e.RowIndex].Cells[11].Value is true)
            {
                if (Convert.ToDateTime(dataGridView_lost.Rows[e.RowIndex].Cells[12].Value) == DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#bcccf2");
                }
                else if (Convert.ToDateTime(dataGridView_lost.Rows[e.RowIndex].Cells[12].Value) <= DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F9A780");
                }
                else if (Convert.ToDateTime(dataGridView_lost.Rows[e.RowIndex].Cells[12].Value) >= DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#C8FAB7");
                }
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }

            dataGridView_lost.Columns[10].Visible = false;
            dataGridView_lost.Columns[8].Visible = false;
            dataGridView_lost.Columns[4].Visible = false;
            dataGridView_lost.ResumeLayout();
        }
        private void dataGridView_lost_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            if (dataGridView_lost.Rows[e.RowIndex].Cells[9].Value.ToString() == "")//Згруповано до ID тривоги(9)
            {
                vars_form.search_id = dataGridView_lost.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_lost.Rows[e.RowIndex].Cells[0].Value.ToString();//ID Тривоги (0)
                vars_form.id_status = dataGridView_lost.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_lost.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_lost.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_lost.Rows[e.RowIndex].Cells[11].Value.ToString();//Звернення(4)
            }
            else
            {

                vars_form.search_id = dataGridView_lost.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_lost.Rows[e.RowIndex].Cells[0].Value.ToString();//Згруповано до ID тривоги(9)
                vars_form.id_status = dataGridView_lost.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_lost.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_lost.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_lost.Rows[e.RowIndex].Cells[10].Value.ToString();//Звернення(4)
            }

            // Блокируем обработку тревоги однвременно двумя операторами, и вносим информацию кто открыл тревогу
            DataTable results1 = macros.GetData("SELECT " +
                                                "alarm_locked, " +
                                                "alarm_locked_user " +
                                                "FROM btk.notification " +
                                                "WHERE " +
                                                "idnotification = '" + vars_form.id_notif + "';");

            if (vars_form.user_login_name != "admin" & vars_form.user_login_name != results1.Rows[0][1].ToString())
            {
                if (results1.Rows[0][0].ToString() == "1")
                {
                    MessageBox.Show("Користувач: " + results1.Rows[0][0] + " вже опрацовуе тривогу.");
                    return;
                }
            }

            macros.GetData("UPDATE btk.notification " +
                           "SET " +
                           "alarm_locked = '1', " +
                           "alarm_locked_user = '" + vars_form.user_login_name + "' " +
                           "WHERE " +
                           "idnotification = '" + vars_form.id_notif + "';");

            detail subwindow = new detail();
            subwindow.Show();
        }


        /// Обновляем вкладку 909
        /// 
        private void update_909_dgv()
        {
            DataTable table = new DataTable();
            table = macros.GetData("SELECT idnotification as 'ID тривоги'," +
                                   "notification.product as 'Продукт'," +
                                   "notification.unit_name as 'Назва об’єкту'," +
                                   "notification.type_alarm as 'Тип тривоги'," +
                                   "notification.unit_id as 'ID'," +
                                   "notification.Status as 'Статус'," +
                                   "notification.alarm_locked_user as 'Обробляє'," +
                                   "notification.time_stamp as 'Дата зміни'," +
                                   "notification.group_alarm as 'Згруповано до', " +
                                   "Users.username as 'Створив'," +
                                   "notification.speed, " +
                                   "notification.remaynder_activate as 'Нагадати', " +
                                   "notification.remayder_date as 'Дата нагадування'  " +
                                   "FROM btk.notification, btk.Users " +
                                   "WHERE Users.idUsers=notification.Users_idUsers " +
                                   "and notification.Status = '909' " + vars_form.hide_group_alarm + "  ;");//order by btk.notification." + vars_form.sort.ToString() + " " + vars_form.order_sort + "
            //table = vars_form.table_909;
            UpdateGridHandler ug = UpdateGrid_909;
            ug.BeginInvoke(table, cb_909, null);
        }
        private void cb_909(IAsyncResult res)
        {

        }
        private void UpdateGrid1_909(DataTable table)
        {
            //save sort
            DataGridViewColumn oldColumn = dataGridView_909_n.SortedColumn;
            ListSortDirection direction;
            if (dataGridView_909_n.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
            else direction = ListSortDirection.Descending;
            
            //save scrol and selected row
            int scrollPosition=0;
            int selectpozition=0;
            if (dataGridView_909_n.DataSource != null)
            {
                scrollPosition = dataGridView_909_n.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
                
                try
                {
                    selectpozition = dataGridView_909_n.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                }
                catch (Exception)
                {
                    selectpozition = 0;
                }
            }
            
            DataView dv = table.DefaultView;
            dv.Sort = "Дата зміни desc";
            DataTable sortedDT = dv.ToTable();

            dataGridView_909_n.DataSource = table;
            ////------------------------------------------
            //if (dataGridView_909_n.DataSource == null)
            //{
            //    dataGridView_909_n.DataSource = table;
            //}
            //else
            //{
            //    dataGridView_909_n.Refresh();
            //}
            ////----------------------------------------------------------------------

            //restote sort
            if (oldColumn != null)
            {
                DataGridViewColumn newColumn = dataGridView_909_n.Columns[oldColumn.Name.ToString()];
                dataGridView_909_n.Sort(newColumn, direction);
                newColumn.HeaderCell.SortGlyphDirection =
                                 direction == ListSortDirection.Ascending ?
                                 SortOrder.Ascending : SortOrder.Descending;
            }

            if (dataGridView_909_n.Rows.Count >= 0)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
            {
                if (scrollPosition == -1)
                {
                    scrollPosition = 0;
                }
                dataGridView_909_n.FirstDisplayedScrollingRowIndex = scrollPosition;
                dataGridView_909_n.ClearSelection();
                dataGridView_909_n.Rows[selectpozition].Selected = true;

                
            }

        }
        private void UpdateGrid_909(DataTable table)
        {
            if (dataGridView_909_n.InvokeRequired)
            {
                UpdateGridThreadHandler handler = UpdateGrid1_909;
                dataGridView_909_n.BeginInvoke(handler, table);
            }
            else
            {
                //save sort
                DataGridViewColumn oldColumn = dataGridView_909_n.SortedColumn;
                ListSortDirection direction;
                if (dataGridView_909_n.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
                else direction = ListSortDirection.Descending;

                //save scrol and selected row
                int scrollPosition =0;
                int selectpozition=0;
                if (dataGridView_909_n.DataSource != null)
                {
                    scrollPosition = dataGridView_909_n.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
                    try
                    {
                        selectpozition = dataGridView_909_n.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                    }
                    catch (Exception)
                    {
                        selectpozition = 0;
                    }
                }

                DataView dv = table.DefaultView;
                dv.Sort = "Дата зміни desc";
                DataTable sortedDT = dv.ToTable();

                dataGridView_909_n.DataSource = table;
                ////------------------------------------------
                //if (dataGridView_909_n.DataSource == null)
                //{
                //    dataGridView_909_n.DataSource = table;
                //}
                //else
                //{
                //    dataGridView_909_n.Refresh();
                //}
                ////----------------------------------------------------------------------
                
                //restote sort
                if (oldColumn != null)
                {
                    DataGridViewColumn newColumn = dataGridView_909_n.Columns[oldColumn.Name.ToString()];
                    dataGridView_909_n.Sort(newColumn, direction);
                    newColumn.HeaderCell.SortGlyphDirection =
                                     direction == ListSortDirection.Ascending ?
                                     SortOrder.Ascending : SortOrder.Descending;
                }

                if (dataGridView_909_n.Rows.Count >= 0)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
                {
                    if (scrollPosition == -1)
                    {
                        scrollPosition = 0;
                    }
                    dataGridView_909_n.FirstDisplayedScrollingRowIndex = scrollPosition;
                }
                dataGridView_909_n.ClearSelection();
                dataGridView_909_n.Rows[selectpozition].Selected = true;

                
            }
        }
        private void dataGridView_909_n_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            if (dataGridView_909_n.Rows[e.RowIndex].Cells[9].Value.ToString() == "")//Згруповано до ID тривоги(9)
            {
                vars_form.search_id = dataGridView_909_n.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_909_n.Rows[e.RowIndex].Cells[0].Value.ToString();//ID Тривоги (0)
                vars_form.id_status = dataGridView_909_n.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_909_n.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_909_n.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_909_n.Rows[e.RowIndex].Cells[11].Value.ToString();//Звернення(4)
            }
            else
            {

                vars_form.search_id = dataGridView_909_n.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_909_n.Rows[e.RowIndex].Cells[0].Value.ToString();//Згруповано до ID тривоги(9)
                vars_form.id_status = dataGridView_909_n.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_909_n.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_909_n.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_909_n.Rows[e.RowIndex].Cells[10].Value.ToString();//Звернення(4)
            }

            DataTable results1 = macros.GetData("SELECT " +
                                         "alarm_locked, " +
                                         "alarm_locked_user " +
                                         "FROM btk.notification " +
                                         "WHERE " +
                                         "idnotification = '" + vars_form.id_notif + "';");

            if (vars_form.user_login_name != "admin" & vars_form.user_login_name != results1.Rows[0][1].ToString())
            {
                if (results1.Rows[0][0].ToString() == "1")
                {
                    MessageBox.Show("Користувач: " + results1.Rows[0][0] + " вже опрацовуе тривогу.");
                    return;
                }
            }

            macros.GetData("UPDATE btk.notification " +
                                "SET " +
                                "alarm_locked = '1', " +
                                "alarm_locked_user = '" + vars_form.user_login_name + "' " +
                                "WHERE " +
                                "idnotification = '" + vars_form.id_notif + "';");


            detail subwindow = new detail();
            subwindow.Show();
        }
        private void dataGridView_909_n_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dataGridView_909_n.SuspendLayout();

            if (dataGridView_909_n.Rows[e.RowIndex].Cells[11].Value is true)
            {
                if (Convert.ToDateTime(dataGridView_909_n.Rows[e.RowIndex].Cells[12].Value) == DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#bcccf2");
                }
                else if (Convert.ToDateTime(dataGridView_909_n.Rows[e.RowIndex].Cells[12].Value) <= DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F9A780");
                }
                else if (Convert.ToDateTime(dataGridView_909_n.Rows[e.RowIndex].Cells[12].Value) >= DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#C8FAB7");
                }
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }

            dataGridView_909_n.Columns[10].Visible = false;
            dataGridView_909_n.Columns[8].Visible = false;
            dataGridView_909_n.Columns[4].Visible = false;
            dataGridView_909_n.ResumeLayout();
        }



        /// Обновляем вкладку Открытые тревоги
        /// 
        private void update_open_dgv()
        {
            DataTable table = new DataTable();
            table = macros.GetData("SELECT idnotification as 'ID тривоги'," +
                                           "product as 'Продукт'," +
                                           "unit_name as 'Назва об’єкту'," +
                                           "type_alarm as 'Тип тривоги'," +
                                           "Status as 'Статус'," +
                                           "alarm_locked_user as 'Обробляє'," +
                                           "msg_time as 'Час створення'," +
                                           "curr_time as 'Час отримання тривоги'," +
                                           "unit_id as 'ID обєкту'," +
                                           "group_alarm as 'Згруповано до' FROM btk.notification WHERE Status = 'Відкрито' OR Status = 'Обробляется' " + vars_form.hide_group_alarm + "  order by btk.notification." + vars_form.sort.ToString() + " " + vars_form.order_sort + ";");
            //table = vars_form.table_open;
            UpdateGridHandler ug = UpdateGrid_open;
            ug.BeginInvoke(table, cb_open, null);
        }
        private void cb_open(IAsyncResult res)
        {

        }
        private void UpdateGrid1_open(DataTable table)
        {
            //save sort
            DataGridViewColumn oldColumn = dataGridView_open_alarm.SortedColumn;
            ListSortDirection direction;
            if (dataGridView_open_alarm.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
            else direction = ListSortDirection.Descending;

            //save scrol and selected row
            int scrollPosition = 0;
            int selectpozition = 0;
            if (dataGridView_open_alarm.DataSource != null)
            {
                scrollPosition = dataGridView_open_alarm.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
                
                try
                {
                    selectpozition = dataGridView_open_alarm.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                }
                catch (Exception)
                {
                    selectpozition = 0;
                }
            }


            dataGridView_open_alarm.DataSource = table;
            ////------------------------------------------
            //if (dataGridView_open_alarm.DataSource == null)
            //{
            //    dataGridView_open_alarm.DataSource = table;
            //}
            //else
            //{
            //    dataGridView_open_alarm.Refresh();
            //}
            ////----------------------------------------------------------------------

            //restote sort
            if (oldColumn != null)
            {
                DataGridViewColumn newColumn = dataGridView_open_alarm.Columns[oldColumn.Name.ToString()];
                dataGridView_open_alarm.Sort(newColumn, direction);
                newColumn.HeaderCell.SortGlyphDirection =
                                 direction == ListSortDirection.Ascending ?
                                 SortOrder.Ascending : SortOrder.Descending;
            }

            if (dataGridView_open_alarm.Rows.Count >= 0)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
            {
                if (scrollPosition == -1)
                {
                    scrollPosition = 0;
                }
                dataGridView_open_alarm.FirstDisplayedScrollingRowIndex = scrollPosition;
                dataGridView_open_alarm.ClearSelection();
                dataGridView_open_alarm.Rows[selectpozition].Selected = true;

                
            }
        }
        private void UpdateGrid_open(DataTable table)
        {
            if (dataGridView_open_alarm.InvokeRequired)
            {
                UpdateGridThreadHandler handler = UpdateGrid1_open;
                dataGridView_open_alarm.BeginInvoke(handler, table);
            }
            else
            {
                //save sort
                DataGridViewColumn oldColumn = dataGridView_open_alarm.SortedColumn;
                ListSortDirection direction;
                if (dataGridView_open_alarm.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
                else direction = ListSortDirection.Descending;

                //save scrol and selected row
                int scrollPosition = 0;
                int selectpozition = 0;
                if (dataGridView_open_alarm.DataSource != null)
                {
                    scrollPosition = dataGridView_open_alarm.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
                    
                    try
                    {
                        selectpozition = dataGridView_open_alarm.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                    }
                    catch (Exception)
                    {
                        selectpozition = 0;
                    }
                }


                dataGridView_open_alarm.DataSource = table;
                ////------------------------------------------
                //if (dataGridView_open_alarm.DataSource == null)
                //{
                //    dataGridView_open_alarm.DataSource = table;
                //}
                //else
                //{
                //    dataGridView_open_alarm.Refresh();
                //}
                ////----------------------------------------------------------------------
                ///

                //restote sort
                if (oldColumn != null)
                {
                    DataGridViewColumn newColumn = dataGridView_open_alarm.Columns[oldColumn.Name.ToString()];
                    dataGridView_open_alarm.Sort(newColumn, direction);
                    newColumn.HeaderCell.SortGlyphDirection =
                                     direction == ListSortDirection.Ascending ?
                                     SortOrder.Ascending : SortOrder.Descending;
                }

                if (dataGridView_open_alarm.Rows.Count >= 0)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
                {
                    if (scrollPosition == -1)
                    {
                        scrollPosition = 0;
                    }
                    dataGridView_open_alarm.FirstDisplayedScrollingRowIndex = scrollPosition;
                    dataGridView_open_alarm.ClearSelection();
                    dataGridView_open_alarm.Rows[selectpozition].Selected = true;

                    
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            if (dataGridView_open_alarm.Rows[e.RowIndex].Cells[9].Value.ToString() == "")//Згруповано до ID тривоги(9)
            {
                vars_form.search_id = dataGridView_open_alarm.Rows[e.RowIndex].Cells[8].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_open_alarm.Rows[e.RowIndex].Cells[0].Value.ToString();//ID Тривоги (0)
                vars_form.id_status = dataGridView_open_alarm.Rows[e.RowIndex].Cells[4].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_open_alarm.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_open_alarm.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                //vars_form.zvernenya = dataGridView_open_alarm.Rows[e.RowIndex].Cells[8].Value.ToString();//Звернення(4)
            }
            else
            {

                vars_form.search_id = dataGridView_open_alarm.Rows[e.RowIndex].Cells[8].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_open_alarm.Rows[e.RowIndex].Cells[9].Value.ToString();//Згруповано до ID тривоги(9)
                vars_form.id_status = dataGridView_open_alarm.Rows[e.RowIndex].Cells[4].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_open_alarm.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_open_alarm.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                //vars_form.zvernenya = dataGridView_open_alarm.Rows[e.RowIndex].Cells[8].Value.ToString();//Звернення(4)
                //var dataIndexNo = dataGridView1.Rows[e.RowIndex].Index.ToString();
                //string id_notif = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                // MessageBox.Show(cellValue);
            }

            // Блокируем обработку тревоги однвременно двумя операторами, и вносим информацию кто открыл тревогу
            DataTable results1 = macros.GetData("SELECT " +
                                                "alarm_locked, " +
                                                "alarm_locked_user " +
                                                "FROM btk.notification " +
                                                "WHERE " +
                                                "idnotification = '" + vars_form.id_notif + "';");

            if (vars_form.user_login_name != "admin" & vars_form.user_login_name != results1.Rows[0][1].ToString())
            {
                if (results1.Rows[0][0].ToString() == "1")
                {
                    MessageBox.Show("Користувач: " + results1.Rows[0][0] + " вже опрацовуе тривогу.");
                    return;
                }
            }

            macros.GetData("UPDATE btk.notification " +
                           "SET " +
                           "alarm_locked = '1', " +
                           "alarm_locked_user = '" + vars_form.user_login_name + "' " +
                           "WHERE " +
                           "idnotification = '" + vars_form.id_notif + "';");

            detail subwindow = new detail();
            subwindow.Show();
        }
        void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dataGridView_open_alarm.SuspendLayout();

            if (dataGridView_open_alarm.Rows[e.RowIndex].Cells[4].Value.ToString() == "Обробляется")
            {
                e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#6ace23");
            }

            if (dataGridView_open_alarm.Rows[e.RowIndex].Cells[4].Value.ToString() == "909")
            {
                e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#f4bf42");
            }

            if (dataGridView_open_alarm.Rows[e.RowIndex].Cells[4].Value.ToString() == "808")
            {
                e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#bcccf2");
            }

            if (dataGridView_open_alarm.Rows[e.RowIndex].Cells[9].Value.ToString() != "")
            {
                e.CellStyle.BackColor = Color.Gray;
            }
            dataGridView_open_alarm.ResumeLayout();
        }



        public void mysql_close_alarm()
        {

            DataTable ds_close_alarm = new DataTable();
            //ds_close_alarm = macros.GetData("SELECT " +
            //                                        "idnotification, " +
            //                                        "unit_id, " +
            //                                        "unit_name, " +
            //                                        "type_alarm, " +
            //                                        "product, " +
            //                                        "speed, " +
            //                                        "curr_time, " +
            //                                        "msg_time, " +
            //                                        "location, " +
            //                                        "last_location, " +
            //                                        "btk.notification.time_stamp, " +
            //                                        "group_alarm, Status, " +
            //                                        "Users_idUsers, " +
            //                                        "alarm_locked_user, (" +
            //                                        "SELECT " +
            //                                        "username " +
            //                                        "FROM btk.Users " +
            //                                        "WHERE " +
            //                                        "idUsers = btk.notification.Users_idUsers) " +
            //                                        "FROM btk.notification " +
            //                                        "WHERE Status = 'Закрито' " +
            //                                vars_form.hide_group_alarm + "  and btk.notification.unit_name LIKE '%" +
            //                                search_close_alarm.Text.ToString() + "%' and btk.notification.type_alarm like '%" +
            //                                comboBox_close_alarm_type.Text.ToString() +
            //                                "%' and (SELECT username FROM btk.Users WHERE idUsers = btk.notification.Users_idUsers) like '%" +
            //                                textBox_close_user_chenge.Text.ToString() + "%' AND (btk.notification.time_stamp BETWEEN '" +
            //                                Convert.ToDateTime(dateTimePicker1.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" +
            //                                Convert.ToDateTime(dateTimePicker2.Value).ToString("yyyy-MM-dd HH:mm:ss") +
            //                                "') order by btk.notification." + vars_form.sort_close + " " + " " +
            //                                vars_form.order_close_sort + " limit " + textBox_limit_close.Text.ToString() + " ");

            ds_close_alarm = macros.GetData("SELECT " +
                                                    "idnotification, " +
                                                    "unit_id, " +
                                                    "unit_name, " +
                                                    "type_alarm, " +
                                                    "product, " +
                                                    "speed, " +
                                                    "curr_time, " +
                                                    "msg_time, " +
                                                    "location, " +
                                                    "last_location, " +
                                                    "btk.notification.time_stamp, " +
                                                    "group_alarm, Status, " +
                                                    "Users_idUsers, " +
                                                    "alarm_locked_user," +
                                                    "Users.username " +
                                                    "FROM btk.notification, btk.Users " +
                                                    "WHERE " +
                                                    "Users.idUsers = notification.Users_idUsers " +
                                                    "and group_alarm is null " +
                                                    "and btk.notification.unit_name LIKE '%" + search_close_alarm.Text + "%' " +
                                                    "and btk.notification.type_alarm like '%" + comboBox_close_alarm_type.Text + "%' " +
                                                    "and Users.username like '%" + textBox_close_user_chenge.Text + "%' " +
                                                    "order by notification.idnotification DESC " +
                                                    "limit " + textBox_limit_close.Text.ToString() + " ");



            dataGridView_close_alarm.AutoGenerateColumns = false;
            dataGridView_close_alarm.RowHeadersVisible = false;

            int scrollPosition =
                this.dataGridView_close_alarm
                    .FirstDisplayedScrollingRowIndex; //сохраняем позицию скрола перед обновлением таблицы

            dataGridView_close_alarm.DataSource = ds_close_alarm;



            if (ds_close_alarm.Rows.Count > 0) // если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
            {
                if (scrollPosition == -1)
                {
                    scrollPosition = 0;
                }

                this.dataGridView_close_alarm.FirstDisplayedScrollingRowIndex = scrollPosition;
            }

        }
        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_close_alarm.Rows[e.RowIndex].Cells[9].Value.ToString() == "")
            {
                vars_form.search_id = dataGridView_close_alarm.Rows[e.RowIndex].Cells[8].Value.ToString();
                vars_form.id_notif = dataGridView_close_alarm.Rows[e.RowIndex].Cells[0].Value.ToString();
                vars_form.id_status = dataGridView_close_alarm.Rows[e.RowIndex].Cells[7].Value.ToString();
                vars_form.unit_name = dataGridView_close_alarm.Rows[e.RowIndex].Cells[2].Value.ToString();
                vars_form.alarm_name = dataGridView_close_alarm.Rows[e.RowIndex].Cells[3].Value.ToString();
                vars_form.restrict_un_group = true;
                detail subwindow = new detail();
                subwindow.Show();
            }
            else
            {

                vars_form.search_id = dataGridView_close_alarm.Rows[e.RowIndex].Cells[8].Value.ToString(); ;
                vars_form.id_notif = dataGridView_close_alarm.Rows[e.RowIndex].Cells[9].Value.ToString();
                vars_form.id_status = dataGridView_close_alarm.Rows[e.RowIndex].Cells[7].Value.ToString();
                vars_form.unit_name = dataGridView_close_alarm.Rows[e.RowIndex].Cells[2].Value.ToString();
                vars_form.alarm_name = dataGridView_close_alarm.Rows[e.RowIndex].Cells[3].Value.ToString();
                vars_form.restrict_un_group = true;

                detail subwindow = new detail();
                subwindow.Show();
            }


        }
        private void dataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView_close_alarm.Rows[e.RowIndex].Cells[9].Value.ToString() != "")
            {
                e.CellStyle.BackColor = Color.Gray;
            }
        }



        private void Main_window_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button_add_alarm_Click(object sender, EventArgs e)
        {
            Add_alarm subwindow2 = new Add_alarm();
            subwindow2.Show();
        }

        private void comboBox_sort_column_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_sort_column.SelectedItem.ToString() == "ID Тривоги")
            {
                vars_form.sort = "idnotification";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "Продукт")
            {
                vars_form.sort = "product";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "Назва обєкту")
            {
                vars_form.sort = "unit_name";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "Тип тривоги")
            {
                vars_form.sort = "type_alarm";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "Статус")
            {
                vars_form.sort = "Status";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "Обробляется")
            {
                vars_form.sort = "alarm_locked_user";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "Час тривоги")
            {
                vars_form.sort = "msg_time";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "Час отримання тривоги")
            {
                vars_form.sort = "curr_time";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "Звернення")
            {
                vars_form.sort = "speed";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "ID обєкту")
            {
                vars_form.sort = "Unit_id";
            }
            if (comboBox_sort_column.SelectedItem.ToString() == "Згруповано до ID тривоги")
            {
                vars_form.sort = "group_alarm";
            }
            //mysql_open_alarm();
        }

        private void checkBox_sort_order_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_sort_order.Checked)
            {
                vars_form.order_sort = "desc";
            }
            else
            {
                vars_form.order_sort = "asc";
            }
            //mysql_open_alarm();
        }

        private void search_close_alarm_TextChanged(object sender, EventArgs e)
        {
            mysql_close_alarm();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            mysql_close_alarm();
        }

        private void comboBox_close_alarm_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_close_alarm_type.Text.ToString() == "Всі")
            {
                comboBox_close_alarm_type.Text = "";
            }
            mysql_close_alarm();
        }

        private void textBox_close_user_chenge_TextChanged(object sender, EventArgs e)
        {
            mysql_close_alarm();
        }

        private void textBox_limit_close_TextChanged(object sender, EventArgs e)
        {

            if (textBox_limit_close.Text.ToString() == "")
            { textBox_limit_close.Text = "1"; }
            mysql_close_alarm();
        }

        private void button_test_sel_obj_yes_Click(object sender, EventArgs e)
        {
            
            if (listBox_test_search_result.SelectedItems.Count <= 0)
            {
                return;
            }

            vars_form.id_wl_object_for_test = listBox_test_search_result.SelectedValue.ToString();
            vars_form.id_db_object_for_test = macros.sql_command("SELECT idObject FROM btk.Object where Object_id_wl=" + vars_form.id_wl_object_for_test + ";");


            Testing form_Testing = new Testing();
            form_Testing.Activated += new EventHandler(form_Testing_activated);
            form_Testing.FormClosed += new FormClosedEventHandler(form_Testing_deactivated);
            form_Testing.Show();
        }

        private void dataGridView_testing_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            vars_form.id_db_object_for_test = macros.sql_command("SELECT Object_idObject FROM btk.testing_object where idtesting_object ='" + dataGridView_testing.Rows[e.RowIndex].Cells[0].Value.ToString() + "';"); 
            vars_form.id_wl_object_for_test = macros.sql_command("SELECT Object_id_wl FROM btk.Object where idObject ='" + vars_form.id_db_object_for_test + "';");
            vars_form.id_db_openning_testing = dataGridView_testing.Rows[e.RowIndex].Cells[0].Value.ToString();
            vars_form.if_open_created_testing = 1;


            Testing form_Testing = new Testing();
            form_Testing.Activated += new EventHandler(form_Testing_activated);
            form_Testing.FormClosed += new FormClosedEventHandler(form_Testing_deactivated);
            textBox_test_search.Text = "";
            form_Testing.Show();

        }

        private void form_Testing_activated(object sender, EventArgs e)
        {
            this.Visible = false;// блокируем окно контрагентов пока открыто окно добавления контрагента
        }

        private void form_Testing_deactivated(object sender, FormClosedEventArgs e)
        {
            vars_form.if_open_created_testing = 0;
            update_testing_dgv();
            this.Visible = true;// разблокируем окно контрагентов кактолько закрыто окно добавления контрагента
        }

        private void label_status_ohoroni_Paint(object sender, PaintEventArgs e)
        {
            // label_status_ohoroni.BackColor = Color.Red;
        }

        private void textBox_test_search_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //// если все поля поиска пустыет - выводим полный перечень. Думаю это не рационально, может быть дохера записей...
                string sql = string.Format("SELECT Object_id_wl, concat(Object_name, ' (',Object_imei, ')') as name_id  FROM btk.Object where Object_name like '%" + textBox_test_search.Text.ToString() + "%'  or Object_imei like '%" + textBox_test_search.Text.ToString() + "%';");
                listBox_test_search_result.DataSource = macros.GetData(sql);
                listBox_test_search_result.DisplayMember = "name_id";
                listBox_test_search_result.ValueMember = "Object_id_wl";

                if (textBox_test_search.Text == "")
                {
                    listBox_test_search_result.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "textBox_filter_object_ohrani_TextChanged");
            }
        }

        private void listBox_test_search_result_DoubleClick(object sender, EventArgs e)
        {
            if (listBox_test_search_result.SelectedItems.Count <= 0)
            {
                return;
            }

            vars_form.id_wl_object_for_test = listBox_test_search_result.SelectedValue.ToString();
            vars_form.id_db_object_for_test = macros.sql_command("SELECT idObject FROM btk.Object where Object_id_wl=" + vars_form.id_wl_object_for_test + ";");


            Testing form_Testing = new Testing();
            form_Testing.Activated += new EventHandler(form_Testing_activated);
            form_Testing.FormClosed += new FormClosedEventHandler(form_Testing_deactivated);
            form_Testing.Show();
        }

        private void checkBox_hide_groupe_alarm_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_hide_groupe_alarm.Checked == true)
            {
                vars_form.hide_group_alarm = " and group_alarm is null";
            }
            else
            {
                vars_form.hide_group_alarm = "";
            }
        }


        private void button_start_activation_Click(object sender, EventArgs e)
        {
            if (listBox_activation_result_search.SelectedItems.Count <= 0)
            {
                return;
            }

            vars_form.id_wl_object_for_activation = listBox_activation_result_search.SelectedValue.ToString();
            vars_form.id_db_object_for_activation = macros.sql_command("SELECT idObject FROM btk.Object where Object_id_wl=" + vars_form.id_wl_object_for_activation + ";");


            vars_form.if_open_created_activation = 0;
            Activation_Form activation_form = new Activation_Form();
            activation_form.Activated += new EventHandler(activation_form_activated);
            activation_form.FormClosed += new FormClosedEventHandler(activation_form_deactivated);
            activation_form.Show();
        }

        private void activation_form_activated(object sender, EventArgs e)
        {
            this.Visible = false;// блокируем окно  пока открыто окно добавления 
        }

        private void activation_form_deactivated(object sender, FormClosedEventArgs e)
        {
            this.Visible = true;// разблокируем окно  кактолько закрыто окно добавления 
            vars_form.if_open_created_activation = 0;
            update_actication_dgv();
        }



        private void Main_window_Shown(object sender, EventArgs e)
        {
            init();
            //get_mqsql_data();


            SetTimer();

            //mysql_close_alarm();
            accsses();

            comboBox_sort_column.SelectedIndexChanged -= checkBox_sort_order_CheckedChanged;

            checkBox_sort_order.Checked = true;

            checkBox_hide_groupe_alarm.Checked = true;

            comboBox_sort_column.SelectedIndexChanged += checkBox_sort_order_CheckedChanged;

        }

        private void button_lenik_get_rep_vidpra_Click(object sender, EventArgs e)
        {
            int gmr_police = 0;
            if (checkBox_vidpra_gmr_police.Checked is true)
            {
                gmr_police = 1;
            }
            else
            {
                gmr_police = 0;
            }




            DataSet data = new DataSet();

            
            data = macros.GetData_dataset("SELECT " +
                                  "notification.idnotification, " +
                                  "notification.unit_name, " +
                                  "Users.username, " +
                                  "notification.type_alarm, " +
                                  "notification.curr_time, " +
                                  "alarm_ack.time_start_ack, " +
                                  "notification.time_stamp, " +
                                  "alarm_ack.alarm_text, " +
                                  "alarm_ack.vizov_gmp, " +
                                  "alarm_ack.vizov_police " +
                                  "FROM " +
                                  "btk.notification, btk.Users, btk.alarm_ack " +
                                  "where(" +
                                  "alarm_ack.vizov_gmp = '" + gmr_police + "' or " +
                                  "alarm_ack.vizov_police = '" + gmr_police + "') and " +
                                  "alarm_ack.notification_idnotification = notification.idnotification and " +
                                  "Users.idUsers = notification.Users_idUsers and " +
                                  "notification.Status = 'Закрито' and " +
                                  "notification.time_stamp BETWEEN '" + Convert.ToDateTime(dateTime_rep_from.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + Convert.ToDateTime(dateTime_rep_to.Value).ToString("yyyy-MM-dd HH:mm:ss") + "';");

            macros.ExportDataSet(data);
        }

        private void textBox_activation_search_TextChanged(object sender, EventArgs e)
        {
            //// если все поля поиска пустыет - выводим полный перечень. Думаю это не рационально, может быть дохера записей...
            string sql = string.Format("SELECT Object_id_wl, concat(Object_name, ' (',Object_imei, ')') as name_id  FROM btk.Object where Object_name like '%" + textBox_activation_search.Text.ToString() + "%'  or Object_imei like '%" + textBox_activation_search.Text.ToString() + "%' limit 6;");
            listBox_activation_result_search.DataSource = macros.GetData(sql);
            listBox_activation_result_search.DisplayMember = "name_id";
            listBox_activation_result_search.ValueMember = "Object_id_wl";

            //if (textBox_test_search.Text == "")
            //{
            //    listBox_test_search_result.DataSource = null;
            //}
        }

        private void listBox_activation_result_search_DoubleClick(object sender, EventArgs e)
        {
            if (listBox_activation_result_search.SelectedItems.Count <= 0)
            {
                return;
            }

            vars_form.id_wl_object_for_activation = listBox_activation_result_search.SelectedValue.ToString();
            vars_form.id_db_object_for_activation = macros.sql_command("SELECT idObject FROM btk.Object where Object_id_wl=" + vars_form.id_wl_object_for_activation + ";");

            vars_form.if_open_created_activation = 0;

            Activation_Form activation_form = new Activation_Form();
            activation_form.Activated += new EventHandler(activation_form_activated);
            activation_form.FormClosed += new FormClosedEventHandler(activation_form_deactivated);
            activation_form.Show();
        }

        private void Main_window_Load(object sender, EventArgs e)
        {
            

        }


        private void form_form_Zayavki_activated(object sender, EventArgs e)
        {
            //this.Visible = false;// блокируем окно контрагентов пока открыто окно добавления контрагента
        }

        private void form_form_Zayavki_deactivated(object sender, FormClosedEventArgs e)
        {
            vars_form.if_open_created_zayavka = 0;
            //this.Visible = true;// разблокируем окно контрагентов кактолько закрыто окно добавления контрагента
            update_zayavki_na_aktivation_2W();

        }

        private void button_create_zayavka_Click(object sender, EventArgs e)
        {

            Zayavki form_Zayavki = new Zayavki();
            form_Zayavki.Activated += new EventHandler(form_form_Zayavki_activated);
            form_Zayavki.FormClosed += new FormClosedEventHandler(form_form_Zayavki_deactivated);
            form_Zayavki.Show();
        }

        private void dataGridView_zayavki_na_activation_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            vars_form.id_testing_for_zayavki = dataGridView_zayavki_na_activation.Rows[e.RowIndex].Cells[0].Value.ToString();
            vars_form.if_open_created_zayavka = 1;

            Zayavki form_Zayavki = new Zayavki();
            form_Zayavki.Activated += new EventHandler(form_form_Zayavki_activated);
            form_Zayavki.FormClosed += new FormClosedEventHandler(form_form_Zayavki_deactivated);
            form_Zayavki.Show();
        }


        private void dataGridView_for_activation_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }
            vars_form.id_db_zayavki_for_activation = dataGridView_for_activation.Rows[e.RowIndex].Cells[0].Value.ToString();
            vars_form.id_db_object_for_activation = dataGridView_for_activation.Rows[e.RowIndex].Cells[1].Value.ToString();
            vars_form.id_db_activation_for_activation = dataGridView_for_activation.Rows[e.RowIndex].Cells[2].Value.ToString();
            vars_form.id_wl_object_for_activation = macros.sql_command("SELECT Object_id_wl FROM btk.Object where idObject = '" + vars_form.id_db_object_for_activation + "';");
            vars_form.if_open_created_activation = 1;

            Activation_Form activation_form = new Activation_Form();
            activation_form.Activated += new EventHandler(activation_form_activated);
            activation_form.FormClosed += new FormClosedEventHandler(activation_form_deactivated);
            activation_form.Show();
        }

        private void comboBox_activation_filter_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBox_activation_filter.SelectedIndex == 0)
            {
                dateTimePicker_activation_filter_end.Enabled = false;
                dateTimePicker_activation_filter_start.Enabled = false;
            }
            else
            {
                dateTimePicker_activation_filter_end.Enabled = true;
                dateTimePicker_activation_filter_start.Enabled = true;
            }
            update_actication_dgv();
        }

        private void comboBox_testing_filter_DropDownClosed(object sender, EventArgs e)
        {
            update_testing_dgv();
        }

        private void dateTimePicker_testing_date_CloseUp(object sender, EventArgs e)
        {
            update_testing_dgv();
        }

        private void comboBox_activovani_select_DropDownClosed(object sender, EventArgs e)
        {
            update_zayavki_na_aktivation_2W();
        }

        private void dateTimePicker_for_zayavki_na_activation_W2_CloseUp(object sender, EventArgs e)
        {
            update_zayavki_na_aktivation_2W();
        }

        private void comboBox_prikripleno_select_DropDownClosed(object sender, EventArgs e)
        {
            update_zayavki_na_aktivation_2W();
        }

        private void dataGridView_for_activation_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow row = dataGridView_for_activation.Rows[e.RowIndex];

            if (dataGridView_for_activation.Rows[e.RowIndex].Cells[11].Value is true)
            {
                if (Convert.ToDateTime(dataGridView_for_activation.Rows[e.RowIndex].Cells[12].Value) == DateTime.Now.Date)
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#bcccf2");
                }
                else if (Convert.ToDateTime(dataGridView_for_activation.Rows[e.RowIndex].Cells[12].Value) <= DateTime.Now.Date)
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F9A780");
                }
                else if (Convert.ToDateTime(dataGridView_for_activation.Rows[e.RowIndex].Cells[12].Value) >= DateTime.Now.Date)
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#C8FAB7");
                }
            }
            else
            {

                row.DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void dateTimePicker_activation_filter_start_CloseUp(object sender, EventArgs e)
        {
            update_actication_dgv();
        }

        private void dateTimePicker_activation_filter_end_CloseUp(object sender, EventArgs e)
        {
            update_actication_dgv();
        }

        private void textBox_search_object_name_activation_TextChanged(object sender, EventArgs e)
        {
            update_actication_dgv();
        }

    }

    internal class List_add_alarm
    {
        public List_add_alarm()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }

    public static class vars_form
    {
        public static string id_notif { get; set; }//айди уведомления
        public static string id_status { get; set; }
        public static int id_status_old { get; set; }
        public static string user_login_id { get; set; }
        public static string search_id { get; set; }
        public static string user_login_name { get; set; }
        public static string user_login_email { get; set; }
        public static int user_accsess_lvl { get; set; }
        public static string unit_name { get; set; }
        public static bool restrict_un_group { get; set; }
        public static string alarm_name { get; set; }
        public static string add_alarm_unit_id { get; set; }
        public static string zvernenya { get; set; }
        public static string sort { get; set; }
        public static string sort_close { get; set; }
        public static string type_alarm_sort_close { get; set; }
        public static string order_sort { get; set; }
        public static string order_close_sort { get; set; }
        public static int setting_font_size { get; set; }
        public static Pen c_color { get; set; }
        public static Brush b_color { get; set; }
        public static string test_id { get; set; }
        public static string hide_group_alarm { get; set; }
        public static string btk_idkontragents { get; set; }
        public static string id_wl_object_for_test { get; set; }
        public static string id_db_object_for_test { get; set; }
        public static string id_db_openning_testing { get; set; }
        public static string id_wl_object_for_activation { get; set; }
        public static string id_db_object_for_activation { get; set; }
        public static string id_db_zayavki_for_activation { get; set; }
        public static string id_db_activation_for_activation { get; set; }
        public static string eid { get; set; }//Храним eid ссесии для Wialon
        public static string wl_user_nm { get; set; }//Храним имя пользователя в Wialon который прошел авторизацию по токену
        public static int wl_user_id { get; set; }//Храним id пользователя в Wialon который прошел авторизацию по токену
        public static string user_token { get; set; }//Храним token авторизации для Wialon из БД
        public static object rootobject { get; set; }//
        public static string error { get; set; }//Храним token авторизации для Wialon из БД
        public static DataTable table_808 { get; set; }//data from mysql for 808
        public static DataTable table_lost { get; set; }//data from mysql for 808
        public static DataTable table_909 { get; set; }//data from mysql for 909
        public static DataTable table_open { get; set; }//data from mysql for open
        public static DataTable table_dilery { get; set; }//data from mysql for dilery
        public static DataTable table_sales { get; set; }//data from mysql for sales
        public static DataTable table_close { get; set; }//data from mysql for close
        public static string transfer_vo1_vo_form { get; set; }//
        public static string transfer_vo2_vo_form { get; set; }//
        public static string transfer_vo3_vo_form { get; set; }//
        public static string transfer_vo4_vo_form { get; set; }//
        public static string transfer_vo5_vo_form { get; set; }//
        public static int num_vo { get; set; }//
        public static string version { get; set; }
        public static string id_testing_for_zayavki { get; set; }
        public static string id_kontragent_sto_for_zayavki { get; set; }
        public static string id_kontragent_zakazchik_for_zayavki { get; set; }
        public static int select_sto_or_zakazchik_for_zayavki { get; set; }//if 1 ->STO, if 0-> zakazchik
        public static int if_open_created_zayavka { get; set; }
        public static int if_open_created_testing { get; set; }
        public static int if_open_created_activation { get; set; }
    }
}
