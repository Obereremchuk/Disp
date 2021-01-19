using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using ZXing;
using CefSharp;
using CefSharp.WinForms;

namespace Disp_WinForm
{
    public partial class Main_window : Form
    {
        private Macros macros = new Macros();
        private static System.Timers.Timer aTimer;

        private delegate void UpdateGridHandler(DataTable table);

        private delegate void UpdateGridThreadHandler(DataTable table);

        private string streamToPrint = Directory.GetCurrentDirectory() + "\\barcode.png";
        private int LoadedGoogleMessage = 0;

        public Main_window()
        {
            this.Font = new System.Drawing.Font("Arial", vars_form.setting_font_size);

            InitializeComponent();

            this.Text = "Disp v." + vars_form.version;
            comboBox_testing_filter.SelectedIndex = 0;

            aTimer = new System.Timers.Timer();

            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            TimeSpan ts1 = new TimeSpan(00, 00, 0);
            TimeSpan ts2 = new TimeSpan(23, 59, 59);

            dateTimePicker_testing_date.Value = DateTime.Now.Date + ts1;
            dateTimePicker_for_zayavki_na_activation_W2.Value = DateTime.Now.Date + ts2;
            dateTimePicker_from_for_zayavki_na_activation_W2.Value = DateTime.Now.Date + ts1;
            dateTimePicker_activation_filter_start.Value = DateTime.Now.Date;
            dateTimePicker_activation_filter_end.Value = DateTime.Now.Date + ts2;

            dateTimePicker_From_close_alarm.Value = DateTime.Now.Date.AddDays(-1) + ts1;
            dateTimePicker_To_close_alarm.Value = DateTime.Now.Date + ts2;


            this.dateTimePicker_testig_filter_start.ValueChanged -= new System.EventHandler(this.dateTimePicker_testig_filter_start_ValueChanged);
            this.dateTimePicker_testing_filter_end.ValueChanged -= new System.EventHandler(this.dateTimePicker_testing_filter_end_ValueChanged);

            dateTimePicker_testig_filter_start.Value = DateTime.Now.Date + ts1;
            dateTimePicker_testing_filter_end.Value = DateTime.Now.Date + ts2;

            this.dateTimePicker_testig_filter_start.ValueChanged += new System.EventHandler(this.dateTimePicker_testig_filter_start_ValueChanged);
            this.dateTimePicker_testing_filter_end.ValueChanged += new System.EventHandler(this.dateTimePicker_testing_filter_end_ValueChanged);



            dateTime_rep_from.Value = DateTime.Now.Date + ts1;
            dateTime_rep_to.Value = DateTime.Now.Date + ts2;
            //add number of rows to table created objects of user
            dataGridView_CreatedObjects.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_CreatedObjects_RowPostPaint);

            //dataGridView_for_activation.DefaultCellStyle.SelectionBackColor = Color.White;
            //dataGridView_for_activation.DefaultCellStyle.SelectionForeColor = Color.Black;

            string sql = string.Format("SELECT idUsers, username FROM btk.Users where accsess_lvl BETWEEN '4' AND '5';");
            var temp = macros.GetData(sql);
            comboBox_user_to_crate_obj.DataSource = null;
            comboBox_user_to_crate_obj.DisplayMember = "username";
            comboBox_user_to_crate_obj.ValueMember = "idUsers";
            comboBox_user_to_crate_obj.DataSource = temp;
            try
            {
                comboBox_user_to_crate_obj.SelectedValue = vars_form.user_login_id;
            }
            catch
            { }

            

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

            dataGridView_accounts.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView_accounts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            // Set the DataGridView control's border.
            dataGridView_accounts.BorderStyle = BorderStyle.Fixed3D;
            // Put the cells in edit mode when user enters them.
            dataGridView_accounts.EditMode = DataGridViewEditMode.EditOnEnter;

            dataGridView_Rouming.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView_Rouming.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            // Set the DataGridView control's border.
            dataGridView_Rouming.BorderStyle = BorderStyle.Fixed3D;
            // Put the cells in edit mode when user enters them.
            dataGridView_Rouming.EditMode = DataGridViewEditMode.EditOnEnter;

            dataGridView_CM.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView_CM.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            // Set the DataGridView control's border.
            dataGridView_CM.BorderStyle = BorderStyle.Fixed3D;
            // Put the cells in edit mode when user enters them.
            dataGridView_CM.EditMode = DataGridViewEditMode.EditOnEnter;

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
            DataTable users_accses = macros.GetData("SELECT idUsers, username, accsess_lvl FROM btk.Users where State = '1';");
            foreach (DataRow user in users_accses.Rows)
            {
                if (user["idUsers"].ToString() == vars_form.user_login_id)
                {
                    if (Convert.ToInt32(user["accsess_lvl"]) <= 5)
                    {
                    }
                    else if (Convert.ToInt32(user["accsess_lvl"]) >= 6 && Convert.ToInt32(user["accsess_lvl"]) <= 8)
                    {
                        tabControl_testing.TabPages.Remove(tabPage3);
                        tabControl_testing.TabPages.Remove(tab_create_object);
                        Delet_Testing_button.Visible = false;
                    }
                    else
                    {
                        tabControl_testing.TabPages.Remove(tabPage3);
                        tabControl_testing.TabPages.Remove(tabPage_zvit);
                        //textBox_activation_search.Enabled = false;
                        tabControl_testing.TabPages.Remove(tab_create_object);
                        tabControl_testing.TabPages.Remove(tabPage_zayavki_activation);
                        tabControl_testing.TabPages.Remove(tabPage6);
                        tabControl_testing.TabPages.Remove(tabPage_work_whith_obj);
                        tabControl_testing.TabPages.Remove(tabPage4);
                        Delet_Testing_button.Visible = false;


                    }
                }
            }
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

        private void OnTimedEvent_accounts(object sender, EventArgs e)
        {
            update_accounts_dgv();
        }

        private void OnTimedEvent_Rouming(object sender, EventArgs e)
        {
            update_Rouming_dgv();
        }

        private void OnTimedEvent_CM(object sender, EventArgs e)
        {
            update_CM_dgv();
        }

        




        private void tabControl_testing_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (tabControl_testing.SelectedTab.Name == "tabPage_808")
            {
                aTimer.Enabled = false;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_accounts);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_Rouming);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_CM);
                update_808_dgv();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Interval = 2000;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage_lost")
            {
                aTimer.Enabled = false;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_accounts);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_Rouming);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_CM);
                update_lost_dgv();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Interval = 2000;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage_909_n")
            { 
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_accounts);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_Rouming);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_CM);
                update_909_dgv();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Interval = 2000;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage_sales")
            {
                aTimer.Enabled = false;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_accounts);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_Rouming);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_CM);
                update_sale_dgv();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Interval = 2000;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage_p")
            {
                aTimer.Enabled = false;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_accounts);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_Rouming);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_CM);
                update_dilery_dgv();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Interval = 2000;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage1")
            {
                aTimer.Enabled = false;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_accounts);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_Rouming);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_CM);
                update_open_dgv();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Interval = 2000;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage_testing")
            {
                aTimer.Enabled = false;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_accounts);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_Rouming);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_CM);
                update_testing_dgv();
            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage2")
            {
                aTimer.Enabled = false;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_accounts);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_Rouming);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_CM);
                mysql_close_alarm();
            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage_activation")
            {
                aTimer.Enabled = false;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_accounts);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_Rouming);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_CM);
                update_actication_dgv();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Interval = 3000;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;

            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage_zayavki_activation")
            {
                aTimer.Enabled = false;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_accounts);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_Rouming);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_CM);
                update_zayavki_na_aktivation_2W();
            }
            else if (tabControl_testing.SelectedTab.Name == "tab_create_object")
            {
                aTimer.Enabled = false;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_accounts);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_Rouming);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_CM);
                build_list_products();
                Google_masseges();
                GetPrinters();
                UpdateCreatedObjectsByUser(DateTime.Now.Date);
                search_tovar_comboBox.Enabled = false;
            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage_accounts")
            {
                aTimer.Enabled = false;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_Rouming);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_CM);
                update_accounts_dgv();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent_accounts);
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage_Rouming")
            {
                aTimer.Enabled = false;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_accounts);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_CM);
                update_Rouming_dgv();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent_Rouming);
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
            else if (tabControl_testing.SelectedTab.Name == "tabPage_CM")
            {
                aTimer.Enabled = false;
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_808);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_909);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_open);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_sale);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_dilery);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_testing);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_activation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_lost);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_zayavki_na_aktivation);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_accounts);
                aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent_Rouming);
                update_CM_dgv();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent_CM);
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
            }
        }

        //
        private void UpdateCreatedObjectsByUser(DateTime date)
        {
            string sql = "SELECT " +
                "(SELECT product_name FROM btk.products where idproducts = products_idproducts) as 'Продукт', " +
                "Object_imei as 'IMEI', " +
                "date_cteate as 'Создано', " +
                "idObject, " +
                "(SELECT Simcardcol_imsi FROM btk.Simcard where idSimcard = Simcard_idSimcard) as 'Simcardcol_imsi', " +
                "(SELECT Simcardcol_number FROM btk.Simcard where idSimcard = Simcard_idSimcard) as 'Simcardcol_number', " +
                "Objectcol_puk, " +
                "Objectcol_gsm_code, " +
                "Objectcol_ble_code, " +
                "Objectcol_bt_enable, " +
                "products_idproducts " +
                "FROM btk.Object " +
                "where Users_idUsers = '" + comboBox_user_to_crate_obj.SelectedValue + "' " +
                "and date_cteate BETWEEN '" + Convert.ToDateTime(date + new TimeSpan(00, 00, 0)).ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                "and '" + Convert.ToDateTime(date + new TimeSpan(23, 59, 59)).ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                "order by Objectcol_create_date desc;";
            DataTable table = macros.GetData(sql);


            if (table.Rows.Count == 0)
            { dataGridView_CreatedObjects.DataSource = null; }
            else
            {
                dataGridView_CreatedObjects.DataSource = table;
                dataGridView_CreatedObjects.Columns["idObject"].Visible = false;
                dataGridView_CreatedObjects.Columns["Simcardcol_imsi"].Visible = false;
                dataGridView_CreatedObjects.Columns["Simcardcol_number"].Visible = false;
                dataGridView_CreatedObjects.Columns["Objectcol_puk"].Visible = false;
                dataGridView_CreatedObjects.Columns["Objectcol_gsm_code"].Visible = false;
                dataGridView_CreatedObjects.Columns["Objectcol_ble_code"].Visible = false;
                dataGridView_CreatedObjects.Columns["Objectcol_bt_enable"].Visible = false;
                dataGridView_CreatedObjects.Columns["products_idproducts"].Visible = false;
            }



        }

        private void dateTimePicker_date_created_by_user_ValueChanged(object sender, EventArgs e)
        {
            UpdateCreatedObjectsByUser(dateTimePicker_date_created_by_user.Value.Date);
        }

        private void dataGridView_CreatedObjects_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView_CreatedObjects.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        /// Обновляем вкладку zayavki Activation W2
        ///
        private void update_zayavki_na_aktivation_2W()
        {
            //ПОлучим последний созданный айди тестирование, и возмем из него дату для верного отображения которую покладем в даттаймпикер

            DataTable table = new DataTable();

            if (checkBox_zayavki_za_ves_chas.Checked)
            {
                table = macros.GetData("SELECT " +
                                            "Zayavki.idZayavki as 'Заявка №', " +
                                            "Zayavkicol_name as 'Назва'," +
                                            "Zayavkicol_plan_date as 'План дата'," +
                                            "Zayavkicol_reason as 'Причина'," +
                                            "Zayavkicol_VIN as'VIN'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti) as 'Встановник'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_zakazchik=Kontragenti.idKontragenti) as 'Замовник'," +
                                            "Users.username as 'Створив'," +
                                            "Zayavki.testing_object_idtesting_object as 'Привязаний до тестування'," +
                                            "Activation_object.Activation_objectcol_result as 'Статус активації'," +
                                            "Activation_object.Activation_date as 'Дата активації'," +
                                            "Object.Object_imei as 'IMEI'," +
                                            "idZayavki " +
                                            "FROM btk.Zayavki, btk.Kontragenti, btk.Users, btk.Activation_object, btk.testing_object, btk.Object " +
                                            "where Zayavki.Activation_object_idActivation_object=Activation_object.idActivation_object " +
                                            "AND testing_object.idtesting_object=Zayavki.testing_object_idtesting_object " +
                                            "AND Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti " +
                                            "AND Zayavki.Users_idUsers=Users.idUsers " +
                                            "AND testing_object.Object_idObject=Object.idObject " +
                                            "AND Zayavkicol_reason like '%" + comboBox_reason_zayavki.Text + "%' " +
                                            "AND (Zayavkicol_name like '%" + textBox_search_zayavki.Text + "%' " +
                                            "OR idZayavki like '%" + textBox_search_zayavki.Text + "%' " +
                                            "OR Zayavkicol_VIN like '%" + textBox_search_zayavki.Text + "%' " +
                                            "OR username like '%" + textBox_search_zayavki.Text + "%' " +
                                            "OR Object.Object_imei like '%" + textBox_search_zayavki.Text + "%' " +
                                            "OR Activation_objectcol_result like '%" + textBox_search_zayavki.Text + "%') " +
                                            ";");
            }
            else
            {
                table = macros.GetData("SELECT " +
                                            "Zayavki.idZayavki as 'Заявка №', " +
                                            "Zayavkicol_name as 'Назва'," +
                                            "Zayavkicol_plan_date as 'План дата'," +
                                            "Zayavkicol_reason as 'Причина'," +
                                            "Zayavkicol_VIN as'VIN'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti) as 'Встановник'," +
                                            "(select Kontragenti_full_name from btk.Kontragenti where Zayavki.Kontragenti_idKontragenti_zakazchik=Kontragenti.idKontragenti) as 'Замовник'," +
                                            "Users.username as 'Створив'," +
                                            "Zayavki.testing_object_idtesting_object as 'Привязаний до тестування'," +
                                            "Activation_object.Activation_objectcol_result as 'Статус активації'," +
                                            "Activation_object.Activation_date as 'Дата активації'," +
                                            "Object.Object_imei as 'IMEI'," +
                                            "idZayavki " +
                                            "FROM btk.Zayavki, btk.Kontragenti, btk.Users, btk.Activation_object, btk.testing_object, btk.Object " +
                                            "where Zayavki.Activation_object_idActivation_object=Activation_object.idActivation_object " +
                                            "AND testing_object.idtesting_object=Zayavki.testing_object_idtesting_object " +
                                            "AND Zayavki.Kontragenti_idKontragenti_sto=Kontragenti.idKontragenti " +
                                            "AND Zayavki.Users_idUsers=Users.idUsers " +
                                            "AND testing_object.Object_idObject=Object.idObject " +
                                            "AND Zayavkicol_reason like '%" + comboBox_reason_zayavki.Text + "%' " +
                                            "AND (Zayavkicol_edit_timestamp between '" + Convert.ToDateTime(dateTimePicker_from_for_zayavki_na_activation_W2.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + Convert.ToDateTime(dateTimePicker_for_zayavki_na_activation_W2.Value).ToString("yyyy-MM-dd HH:mm:ss") + "')" +
                                            "AND (Zayavkicol_name like '%" + textBox_search_zayavki.Text + "%' " +
                                            "OR idZayavki like '%" + textBox_search_zayavki.Text + "%' " +
                                            "OR Zayavkicol_VIN like '%" + textBox_search_zayavki.Text + "%' " +
                                            "OR username like '%" + textBox_search_zayavki.Text + "%' " +
                                            "OR Object.Object_imei like '%" + textBox_search_zayavki.Text + "%' " +
                                            "OR Activation_objectcol_result like '%" + textBox_search_zayavki.Text + "%') " +
                                            ";");
            }


            int scrollPosition = dataGridView_zayavki_na_activation.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
            dataGridView_zayavki_na_activation.DataSource = table;


            dataGridView_zayavki_na_activation.Columns["Заявка №"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_zayavki_na_activation.Columns["Назва"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_zayavki_na_activation.Columns["План дата"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_zayavki_na_activation.Columns["Причина"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_zayavki_na_activation.Columns["VIN"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_zayavki_na_activation.Columns["Встановник"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_zayavki_na_activation.Columns["Замовник"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_zayavki_na_activation.Columns["Створив"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_zayavki_na_activation.Columns["Привязаний до тестування"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_zayavki_na_activation.Columns["Статус активації"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_zayavki_na_activation.Columns["IMEI"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_zayavki_na_activation.Columns["Дата активації"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_zayavki_na_activation.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            if (dataGridView_zayavki_na_activation.Rows.Count >= 1)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
            {
                if (scrollPosition == -1)
                {
                    scrollPosition = 0;
                }
                if (scrollPosition > dataGridView_zayavki_na_activation.Rows.Count)
                {
                    scrollPosition = 0;
                }
                dataGridView_zayavki_na_activation.FirstDisplayedScrollingRowIndex = scrollPosition;
            }
            dataGridView_zayavki_na_activation.Columns["idZayavki"].Visible = false;
        }




        /// Обновляем вкладку For Activation
        private void _update_actication_dgv()
        {
            //ПОлучим последний созданный айди тестирование, и возмем из него дату для верного отображения которую покладем в даттаймпикер
            DataTable table = new DataTable();
            
            string uspishno = "";
            if (checkBox_activation_uspishno.Checked is true)
            { uspishno = checkBox_activation_uspishno.Checked ? "or Activation_objectcol_result like 'Успішно' " : "or Activation_objectcol_result like '%" + textBox_search_object_name_activation.Text + "%' "; }
            else if (checkBox_activation_uspishno.Checked is true)
            { uspishno = checkBox_activation_uspishno_pin.Checked ? "or Activation_objectcol_result like 'Успішно (PIN)' " : "or Activation_objectcol_result like '%" + textBox_search_object_name_activation.Text + "%' "; }
            else if (checkBox_activation_neuspishno.Checked is true)
            { uspishno = checkBox_activation_uspishno.Checked ? "or Activation_objectcol_result like 'Не проводилось' " : "or Activation_objectcol_result like '%" + textBox_search_object_name_activation.Text + "%' "; }


            if (checkBox_activation_za_ves_chas_search.Checked)
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
                    //"(SELECT coments FROM btk.activation_comments where Activation_object_idActivation_object = Activation_object.idActivation_object order by date_insert desc limit 1) as 'Коментар', " +
                    "Activation_object.comment as 'Коментар', " +
                    "remaynder_activate as 'Нагадати', " +
                    "remayder_date as 'Дата нагадування', " +
                    "Object.Object_imei as 'IMEI'," +
                    "Activation_object.Locked_user as 'Обробляє' " +
                    "FROM " +
                    "btk.products," +
                    "btk.Activation_object," +
                    "btk.Zayavki," +
                    "btk.TS_brand," +
                    "btk.TS_model, " +
                    "btk.Object " +
                    "where " +
                    "Activation_object.Object_idObject != '10' " +
                    "and(idZayavki like '%" + textBox_search_object_name_activation.Text + "%' " +
                    "or Activation_object.new_name_obj like '%" + textBox_search_object_name_activation.Text + "%' " +
                    "or Zayavkicol_name like '%" + textBox_search_object_name_activation.Text + "%' " +
                    "OR Object.Object_imei like '%" + textBox_search_object_name_activation.Text + "%' " +
                    uspishno +
                    "or Zayavkicol_VIN like '%" + textBox_search_object_name_activation.Text + "%') " +
                    "and TS_brand.idTS_brand = Zayavki.TS_brand_idTS_brand " +
                    "and TS_model.idTS_model = Zayavki.TS_model_idTS_model " +
                    "and products.idproducts = Zayavki.products_idproducts " +
                    "AND Activation_object.Object_idObject=Object.idObject " +
                    "and Zayavki.Activation_object_idActivation_object = Activation_object.idActivation_object " +
                    "order by idZayavki desc" +
                    "; ");
            }
            else
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
                    //"(SELECT coments FROM btk.activation_comments where Activation_object_idActivation_object = Activation_object.idActivation_object order by date_insert desc limit 1) as 'Коментар', " +
                    "Activation_object.comment as 'Коментар', " +
                    "remaynder_activate as 'Нагадати', " +
                    "remayder_date as 'Дата нагадування', " +
                    "Object.Object_imei as 'IMEI'," +
                    "Activation_object.Locked_user as 'Обробляє' " +
                    "FROM " +
                    "btk.products," +
                    "btk.Activation_object," +
                    "btk.Zayavki," +
                    "btk.TS_brand," +
                    "btk.TS_model, " +
                    "btk.Object " +
                    "where " +
                    "Activation_object.Object_idObject != '10' " +
                    "and Activation_object.Activation_date between '" + Convert.ToDateTime(dateTimePicker_activation_filter_start.Value).Date.ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(dateTimePicker_activation_filter_end.Value).Date.ToString("yyyy-MM-dd") + "' " +
                    "and(idZayavki like '%" + textBox_search_object_name_activation.Text + "%' " +
                    "or Activation_object.new_name_obj like '%" + textBox_search_object_name_activation.Text + "%' " +
                    "OR Object.Object_imei like '%" + textBox_search_object_name_activation.Text + "%' " +
                    "or Zayavkicol_name like '%" + textBox_search_object_name_activation.Text + "%' " +
                    uspishno +
                    "or Zayavkicol_VIN like '%" + textBox_search_object_name_activation.Text + "%') " +
                    "and TS_brand.idTS_brand = Zayavki.TS_brand_idTS_brand " +
                    "and TS_model.idTS_model = Zayavki.TS_model_idTS_model " +
                    "and products.idproducts = Zayavki.products_idproducts " +
                    "AND Activation_object.Object_idObject=Object.idObject " +
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
            dataGridView_for_activation.Columns["IMEI"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_for_activation.Columns["Обробляє"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_for_activation.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            if (dataGridView_for_activation.Rows.Count >= 1)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
            {
                if (scrollPosition == -1)
                {
                    scrollPosition = 0;
                }
                if (scrollPosition > dataGridView_for_activation.Rows.Count)
                {
                    scrollPosition = 0;
                }
                dataGridView_for_activation.FirstDisplayedScrollingRowIndex = scrollPosition;
            }
        }

        private void update_actication_dgv()
        {
            //ПОлучим последний созданный айди тестирование, и возмем из него дату для верного отображения которую покладем в даттаймпикер 
            DataTable table = new DataTable();


            if (checkBox_activation_za_ves_chas_search.Checked)
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
                    //"(SELECT coments FROM btk.activation_comments where Activation_object_idActivation_object = Activation_object.idActivation_object order by date_insert desc limit 1) as 'Коментар', " +
                    "Activation_object.comment as 'Коментар', " +
                    "remaynder_activate as 'Нагадати', " +
                    "Object.Object_imei as 'IMEI'," +
                    "remayder_date as 'Дата нагадування', " +
                    "Activation_object.Locked_user as 'Обробляє' " +
                    "FROM " +
                    "btk.products," +
                    "btk.Activation_object," +
                    "btk.Zayavki," +
                    "btk.TS_brand," +
                    "btk.TS_model, " +
                    "btk.Object " +
                    "where " +
                    "Activation_object.Object_idObject != '10' " +
                    "and(idZayavki like '%" + textBox_search_object_name_activation.Text + "%' " +
                    "or Activation_object.new_name_obj like '%" + textBox_search_object_name_activation.Text + "%' " +
                    "or Zayavkicol_name like '%" + textBox_search_object_name_activation.Text + "%' " +
                    "OR Object.Object_imei like '%" + textBox_search_object_name_activation.Text + "%' " +
                    " or Activation_objectcol_result like '" + textBox_search_object_name_activation.Text + "' " +
                    "or Zayavkicol_VIN like '%" + textBox_search_object_name_activation.Text + "%') " +
                    "and TS_brand.idTS_brand = Zayavki.TS_brand_idTS_brand " +
                    "and TS_model.idTS_model = Zayavki.TS_model_idTS_model " +
                    "and products.idproducts = Zayavki.products_idproducts " +
                    "AND Activation_object.Object_idObject=Object.idObject " +
                    "and Zayavki.Activation_object_idActivation_object = Activation_object.idActivation_object " +
                    "order by idZayavki desc" +
                    "; ");
            }
            else
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
                    //"(SELECT coments FROM btk.activation_comments where Activation_object_idActivation_object = Activation_object.idActivation_object order by date_insert desc limit 1) as 'Коментар', " +
                    "Activation_object.comment as 'Коментар', " +
                    "remaynder_activate as 'Нагадати', " +
                    "remayder_date as 'Дата нагадування', " +
                     "Object.Object_imei as 'IMEI'," +
                    "Activation_object.Locked_user as 'Обробляє' " +
                    "FROM " +
                    "btk.products," +
                    "btk.Activation_object," +
                    "btk.Zayavki," +
                    "btk.TS_brand," +
                    "btk.TS_model, " +
                    "btk.Object " +
                    "where " +
                    "Activation_object.Object_idObject != '10' " +
                    "and Activation_object.Activation_date between '" + Convert.ToDateTime(dateTimePicker_activation_filter_start.Value).Date.ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(dateTimePicker_activation_filter_end.Value).Date.ToString("yyyy-MM-dd") + "' " +
                    "and(idZayavki like '%" + textBox_search_object_name_activation.Text + "%' " +
                    "or Activation_object.new_name_obj like '%" + textBox_search_object_name_activation.Text + "%' " +
                    "or Zayavkicol_name like '%" + textBox_search_object_name_activation.Text + "%' " +
                    "or Activation_objectcol_result like '" + textBox_search_object_name_activation.Text + "' " +
                    "OR Object.Object_imei like '%" + textBox_search_object_name_activation.Text + "%' " +
                    "or Zayavkicol_VIN like '%" + textBox_search_object_name_activation.Text + "%') " +
                    "and TS_brand.idTS_brand = Zayavki.TS_brand_idTS_brand " +
                    "and TS_model.idTS_model = Zayavki.TS_model_idTS_model " +
                    "and products.idproducts = Zayavki.products_idproducts " +
                    "AND Activation_object.Object_idObject=Object.idObject " +
                    "and Zayavki.Activation_object_idActivation_object = Activation_object.idActivation_object " +
                    "order by idZayavki desc" +
                    "; ");
            }
            //table = vars_form.table_dilery;
            UpdateGridHandler ug = UpdateGrid_actication;
            ug.BeginInvoke(table, cb_actication, null);
        }

        private void cb_actication(IAsyncResult res)
        {
        }

        private void UpdateGrid1_actication(DataTable table)
        {
            //save sort
            DataGridViewColumn oldColumn = dataGridView_for_activation.SortedColumn;
            ListSortDirection direction;
            if (dataGridView_for_activation.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
            else direction = ListSortDirection.Descending;

            //save scrol and selected row
            int scrollPosition = 0;
            int selectpozition = 0;
            if (dataGridView_for_activation.Rows.Count >= 1)
            {
                scrollPosition = dataGridView_for_activation.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы

                try
                {
                    selectpozition = dataGridView_for_activation.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                }
                catch (Exception)
                {
                    selectpozition = 0;
                }
            }

            //DataView dv = table.DefaultView;
            //dv.Sort = "Дата зміни desc";
            //DataTable sortedDT = dv.ToTable();

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
            dataGridView_for_activation.Columns["Обробляє"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView_for_activation.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            //restote sort
            if (oldColumn != null)
            {
                DataGridViewColumn newColumn = dataGridView_for_activation.Columns[oldColumn.Name.ToString()];
                dataGridView_for_activation.Sort(newColumn, direction);
                newColumn.HeaderCell.SortGlyphDirection =
                                 direction == ListSortDirection.Ascending ?
                                 SortOrder.Ascending : SortOrder.Descending;
            }

            if (dataGridView_for_activation.Rows.Count >= 1 & dataGridView_for_activation.Rows.Count > scrollPosition)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
            {
                if (scrollPosition == -1)
                {
                    scrollPosition = 0;
                }
                dataGridView_for_activation.FirstDisplayedScrollingRowIndex = scrollPosition;
            }
            if (dataGridView_for_activation.Rows.Count >= 1 & dataGridView_for_activation.Rows.Count > selectpozition)
            {
                dataGridView_for_activation.ClearSelection();

                try
                {
                    dataGridView_for_activation.Rows[selectpozition].Selected = true;
                }
                catch (Exception)
                {
                    //dataGridView_for_activation.Rows[0].Selected = true;
                }
            }
        }

        private void UpdateGrid_actication(DataTable table)
        {
            if (dataGridView_for_activation.InvokeRequired)
            {
                UpdateGridThreadHandler handler = UpdateGrid1_actication;
                dataGridView_for_activation.BeginInvoke(handler, table);
            }
            else
            {
                //save sort
                DataGridViewColumn oldColumn = dataGridView_for_activation.SortedColumn;
                ListSortDirection direction;
                if (dataGridView_for_activation.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
                else direction = ListSortDirection.Descending;

                //save scrol and selected row
                int scrollPosition = 0;
                int selectpozition = 0;
                if (dataGridView_for_activation.Rows.Count >= 1)
                {
                    scrollPosition = dataGridView_for_activation.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы

                    try
                    {
                        selectpozition = dataGridView_for_activation.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                    }
                    catch (Exception)
                    {
                        selectpozition = 0;
                    }
                }

                //DataView dv = table.DefaultView;
                //dv.Sort = "Дата зміни desc";
                //DataTable sortedDT = dv.ToTable();

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
                dataGridView_for_activation.Columns["Обробляє"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView_for_activation.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                //restote sort
                if (oldColumn != null)
                {
                    DataGridViewColumn newColumn = dataGridView_for_activation.Columns[oldColumn.Name.ToString()];
                    dataGridView_for_activation.Sort(newColumn, direction);
                    newColumn.HeaderCell.SortGlyphDirection =
                                     direction == ListSortDirection.Ascending ?
                                     SortOrder.Ascending : SortOrder.Descending;
                }

                if (dataGridView_for_activation.Rows.Count >= 0 & dataGridView_for_activation.Rows.Count > scrollPosition)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
                {
                    if (scrollPosition == -1)
                    {
                        scrollPosition = 0;
                    }
                    dataGridView_for_activation.FirstDisplayedScrollingRowIndex = scrollPosition;
                }
                if (dataGridView_for_activation.Rows.Count >= selectpozition)
                {
                    dataGridView_for_activation.ClearSelection();
                    try
                    {
                        dataGridView_for_activation.Rows[selectpozition].Selected = true;
                    }
                    catch (Exception)
                    {
                        //dataGridView_for_activation.Rows[0].Selected = true;
                    }
                }
            }
        }





        /// Обновляем вкладку Testing
        private void update_testing_dgv()
        {
            DataTable table = new DataTable();


            if (checkBox_testing_za_ves_chas_search.Checked)
            {

                table = macros.GetData(
                    "SELECT " +
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
                    "testing_object.Object_idObject=Object.idObject " +
                    "and testing_object.Users_idUsers=Users.idUsers " +
                    "and (Object.Object_imei like '%" + textBox_search_object_name_testing.Text + "%' " +
                    "OR testing_object.idtesting_object like '%" + textBox_search_object_name_testing.Text + "%' " +
                    "OR Object.Object_name like '%" + textBox_search_object_name_testing.Text + "%' " +
                    "OR Users.username like '%" + textBox_search_object_name_testing.Text + "%' " +
                    "OR testing_object.testing_objectcol_result like '%" + textBox_search_object_name_testing.Text + "%') " +
                    "order by idtesting_object desc" +
                    "; ");

            }
            else
            {
                table = macros.GetData(
                    "SELECT " +
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
                    "testing_object.Object_idObject=Object.idObject " +
                    "and testing_object.Users_idUsers=Users.idUsers " +
                    "and testing_object.testing_objectcol_edit_timestamp between '" + Convert.ToDateTime(dateTimePicker_testig_filter_start.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + Convert.ToDateTime(dateTimePicker_testing_filter_end.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                    "and (Object.Object_imei like '%" + textBox_search_object_name_testing.Text + "%' " +
                    "OR testing_object.idtesting_object like '%" + textBox_search_object_name_testing.Text + "%' " +
                    "OR Object.Object_name like '%" + textBox_search_object_name_testing.Text + "%' " +
                    "OR Users.username like '%" + textBox_search_object_name_testing.Text + "%' " +
                    "OR testing_object.testing_objectcol_result like '%" + textBox_search_object_name_testing.Text + "%') " +
                    "order by idtesting_object desc" +
                    "; ");
            }







            //if (comboBox_testing_filter.SelectedIndex == 1)
            //{
            //    table = macros.GetData("SELECT " +
            //                       "testing_object.idtesting_object as '№ тестування', " +
            //                       "Object.Object_name as 'Назва обекту', " +
            //                       "Object.Object_imei as 'IMEI', " +
            //                       "Users.username as 'Змінив', " +
            //                       "testing_object.testing_objectcol_result as 'Результат', " +
            //                       "testing_object.testing_objectcol_edit_timestamp as 'Змінено', " +
            //                       "testing_object.testing_objectcol_comments as 'Коментар' " +
            //                       "FROM " +
            //                       "btk.testing_object, " +
            //                       "btk.Object, " +
            //                       "btk.Users " +
            //                       "where " +
            //                       "testing_object.testing_objectcol_result='Успішно' and " +
            //                       "testing_object.Object_idObject=Object.idObject and " +
            //                       "testing_object.Users_idUsers=Users.idUsers and " +
            //                       "(testing_object.testing_objectcol_edit_timestamp  between '" + Convert.ToDateTime(dateTimePicker_testing_date.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') and " +
            //                       "Object.Object_imei like '%" + textBox_search_testing.Text + "%' ;");
            //}
            //else if (comboBox_testing_filter.SelectedIndex == 0)
            //{
            //    table = macros.GetData("SELECT " +
            //                       "testing_object.idtesting_object as '№ тестування', " +
            //                       "Object.Object_name as 'Назва обекту', " +
            //                       "Object.Object_imei as 'IMEI', " +
            //                       "Users.username as 'Змінив', " +
            //                       "testing_object.testing_objectcol_result as 'Результат', " +
            //                       "testing_object.testing_objectcol_edit_timestamp as 'Змінено', " +
            //                       "testing_object.testing_objectcol_comments as 'Коментар' " +
            //                       "FROM " +
            //                       "btk.testing_object, " +
            //                       "btk.Object, " +
            //                       "btk.Users " +
            //                       "where " +
            //                       "testing_object.testing_objectcol_result ='Не завершено' and " +
            //                       "testing_object.Object_idObject=Object.idObject and " +
            //                       "testing_object.Users_idUsers=Users.idUsers and " +
            //                       "(testing_object.testing_objectcol_edit_timestamp  between '" + Convert.ToDateTime(dateTimePicker_testing_date.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') and " +
            //                       "Object.Object_imei like '%" + textBox_search_testing.Text + "%' ;");
            //}
            //else if (comboBox_testing_filter.SelectedIndex == 2)
            //{
            //    table = macros.GetData("SELECT " +
            //                       "testing_object.idtesting_object as '№ тестування', " +
            //                       "Object.Object_name as 'Назва обекту', " +
            //                       "Object.Object_imei as 'IMEI', " +
            //                       "Users.username as 'Змінив', " +
            //                       "testing_object.testing_objectcol_result as 'Результат', " +
            //                       "testing_object.testing_objectcol_edit_timestamp as 'Змінено', " +
            //                       "testing_object.testing_objectcol_comments as 'Коментар' " +
            //                       "FROM " +
            //                       "btk.testing_object, " +
            //                       "btk.Object, " +
            //                       "btk.Users " +
            //                       "where " +
            //                       "testing_object.Object_idObject=Object.idObject and " +
            //                       "testing_object.Users_idUsers=Users.idUsers and " +
            //                       "(testing_object.testing_objectcol_edit_timestamp  between '" + Convert.ToDateTime(dateTimePicker_testing_date.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') and " +
            //                       "Object.Object_imei like '%" + textBox_search_testing.Text + "%' ;");
            //}

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
            }
            if (dataGridView_dilery.Rows.Count >= selectpozition)
            {
                dataGridView_dilery.ClearSelection();

                try
                {
                    dataGridView_dilery.Rows[selectpozition].Selected = true;
                }
                catch (Exception)
                {
                    dataGridView_dilery.Rows[0].Selected = true;
                }
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
                }
                if (dataGridView_dilery.Rows.Count >= selectpozition)
                {
                    dataGridView_dilery.ClearSelection();
                    try
                    {
                        dataGridView_dilery.Rows[selectpozition].Selected = true;
                    }
                    catch (Exception)
                    {
                        dataGridView_dilery.Rows[0].Selected = true;
                    }
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
            }
            if (dataGridView_sales.Rows.Count >= selectpozition)
            {
                dataGridView_sales.ClearSelection();

                try
                {
                    dataGridView_sales.Rows[selectpozition].Selected = true;
                }
                catch (Exception)
                {
                    dataGridView_sales.Rows[0].Selected = true;
                }

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
                }
                if (dataGridView_sales.Rows.Count >= selectpozition)
                {
                    dataGridView_sales.ClearSelection();
                    try
                    {
                        dataGridView_sales.Rows[selectpozition].Selected = true;
                    }
                    catch (Exception)
                    {
                        dataGridView_sales.Rows[0].Selected = true;
                    }
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



        /// Обновляем вкладку СМ 
        ///
        private void update_CM_dgv()
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
                                   "and notification.Status = '110' " + vars_form.hide_group_alarm + "  ;");//order by btk.notification." + vars_form.sort.ToString() + " " + vars_form.order_sort + "
            //table = vars_form.table_909;
            UpdateGridHandler ug = UpdateGrid_CM;
            ug.BeginInvoke(table, cb_CM, null);
        }

        private void cb_CM(IAsyncResult res)
        {
        }

        private void UpdateGrid1_CM(DataTable table)
        {
            //save sort
            DataGridViewColumn oldColumn = dataGridView_CM.SortedColumn;
            ListSortDirection direction;
            if (dataGridView_CM.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
            else direction = ListSortDirection.Descending;

            //save scrol and selected row
            int scrollPosition = 0;
            int selectpozition = 0;
            if (dataGridView_CM.DataSource != null)
            {
                scrollPosition = dataGridView_CM.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы

                if (dataGridView_CM.Rows.Count >= 1)
                {
                    try
                    {
                        selectpozition = dataGridView_CM.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                    }
                    catch (Exception)
                    {
                        selectpozition = 0;
                    }
                }
            }

            DataView dv = table.DefaultView;
            dv.Sort = "Дата зміни desc";
            DataTable sortedDT = dv.ToTable();

            dataGridView_CM.DataSource = table;


            //restote sort
            if (oldColumn != null)
            {
                DataGridViewColumn newColumn = dataGridView_CM.Columns[oldColumn.Name.ToString()];
                dataGridView_CM.Sort(newColumn, direction);
                newColumn.HeaderCell.SortGlyphDirection =
                                 direction == ListSortDirection.Ascending ?
                                 SortOrder.Ascending : SortOrder.Descending;
            }

            if (dataGridView_CM.Rows.Count >= 1)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
            {
                if (scrollPosition == -1)
                {
                    scrollPosition = 0;
                }
                dataGridView_CM.FirstDisplayedScrollingRowIndex = scrollPosition;
            }
            else { scrollPosition = -1; }
            if (dataGridView_CM.Rows.Count >= 1)
            {
                dataGridView_CM.ClearSelection();
                try
                {
                    dataGridView_CM.Rows[selectpozition].Selected = true;
                }
                catch (Exception)
                {
                    dataGridView_CM.Rows[0].Selected = true;
                }
            }
        }

        private void UpdateGrid_CM(DataTable table)
        {
            if (dataGridView_CM.InvokeRequired)
            {
                UpdateGridThreadHandler handler = UpdateGrid1_CM;
                dataGridView_CM.BeginInvoke(handler, table);
            }
            else
            {
                //save sort
                DataGridViewColumn oldColumn = dataGridView_CM.SortedColumn;
                ListSortDirection direction;
                if (dataGridView_CM.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
                else direction = ListSortDirection.Descending;

                //save scrol and selected row
                int scrollPosition = 0;
                int selectpozition = 0;
                if (dataGridView_CM.DataSource != null)
                {
                    scrollPosition = dataGridView_CM.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
                    try
                    {
                        selectpozition = dataGridView_CM.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                    }
                    catch (Exception)
                    {
                        selectpozition = 0;
                    }
                }

                DataView dv = table.DefaultView;
                dv.Sort = "Дата зміни desc";
                DataTable sortedDT = dv.ToTable();

                dataGridView_CM.DataSource = table;


                //restote sort
                if (oldColumn != null)
                {
                    DataGridViewColumn newColumn = dataGridView_CM.Columns[oldColumn.Name.ToString()];
                    dataGridView_CM.Sort(newColumn, direction);
                    newColumn.HeaderCell.SortGlyphDirection =
                                     direction == ListSortDirection.Ascending ?
                                     SortOrder.Ascending : SortOrder.Descending;
                }

                if (dataGridView_CM.Rows.Count >= 0)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
                {
                    if (scrollPosition == -1)
                    {
                        scrollPosition = 0;
                    }
                    dataGridView_CM.FirstDisplayedScrollingRowIndex = scrollPosition;
                }
                if (dataGridView_CM.Rows.Count >= selectpozition)
                {
                    dataGridView_CM.ClearSelection();

                    try
                    {
                        dataGridView_CM.Rows[selectpozition].Selected = true;
                    }
                    catch (Exception)
                    {
                        dataGridView_CM.Rows[0].Selected = true;
                    }
                }
            }
        }

        private void dataGridView_CM_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            if (dataGridView_CM.Rows[e.RowIndex].Cells[9].Value.ToString() == "")//Згруповано до ID тривоги(9)
            {
                vars_form.search_id = dataGridView_CM.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_CM.Rows[e.RowIndex].Cells[0].Value.ToString();//ID Тривоги (0)
                vars_form.id_status = dataGridView_CM.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_CM.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_CM.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_CM.Rows[e.RowIndex].Cells[11].Value.ToString();//Звернення(4)
            }
            else
            {
                vars_form.search_id = dataGridView_CM.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_CM.Rows[e.RowIndex].Cells[0].Value.ToString();//Згруповано до ID тривоги(9)
                vars_form.id_status = dataGridView_CM.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_CM.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_CM.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_CM.Rows[e.RowIndex].Cells[10].Value.ToString();//Звернення(4)
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

        private void dataGridView_CM_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dataGridView_CM.SuspendLayout();

            if (dataGridView_CM.Rows[e.RowIndex].Cells[11].Value is true)
            {
                //if (Convert.ToDateTime(dataGridView_Rouming.Rows[e.RowIndex].Cells[12].Value) == DateTime.Now.Date)
                //{
                //    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#bcccf2");
                //}
                //else if (Convert.ToDateTime(dataGridView_Rouming.Rows[e.RowIndex].Cells[12].Value) <= DateTime.Now.Date)
                //{
                //    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F9A780");
                //}
                //else if (Convert.ToDateTime(dataGridView_Rouming.Rows[e.RowIndex].Cells[12].Value) >= DateTime.Now.Date)
                //{
                //    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#C8FAB7");
                //}
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }


            dataGridView_CM.ResumeLayout();
        }



        /// Обновляем вкладку Rouming 
        ///
        private void update_Rouming_dgv()
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
                                   "and notification.Status = 'Роумінг' " + vars_form.hide_group_alarm + "  ;");//order by btk.notification." + vars_form.sort.ToString() + " " + vars_form.order_sort + "
            //table = vars_form.table_909;
            UpdateGridHandler ug = UpdateGrid_Rouming;
            ug.BeginInvoke(table, cb_Rouming, null);
        }

        private void cb_Rouming(IAsyncResult res)
        {
        }

        private void UpdateGrid1_Rouming(DataTable table)
        {
            //save sort
            DataGridViewColumn oldColumn = dataGridView_Rouming.SortedColumn;
            ListSortDirection direction;
            if (dataGridView_Rouming.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
            else direction = ListSortDirection.Descending;

            //save scrol and selected row
            int scrollPosition = 0;
            int selectpozition = 0;
            if (dataGridView_Rouming.DataSource != null)
            {
                scrollPosition = dataGridView_Rouming.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
                if (dataGridView_Rouming.Rows.Count >= 1)
                {
                    try
                    {
                        selectpozition = dataGridView_Rouming.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                    }
                    catch (Exception)
                    {
                        selectpozition = 0;
                    }
                }
            }

            DataView dv = table.DefaultView;
            dv.Sort = "Дата зміни desc";
            DataTable sortedDT = dv.ToTable();

            dataGridView_Rouming.DataSource = table;
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
                DataGridViewColumn newColumn = dataGridView_Rouming.Columns[oldColumn.Name.ToString()];
                dataGridView_Rouming.Sort(newColumn, direction);
                newColumn.HeaderCell.SortGlyphDirection =
                                 direction == ListSortDirection.Ascending ?
                                 SortOrder.Ascending : SortOrder.Descending;
            }

            if (dataGridView_Rouming.Rows.Count >= 1)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
            {
                if (scrollPosition == -1)
                {
                    scrollPosition = 0;
                }
                dataGridView_Rouming.FirstDisplayedScrollingRowIndex = scrollPosition;
            }
            if (dataGridView_Rouming.Rows.Count >= 1)
            {
                dataGridView_Rouming.ClearSelection();
                try
                {
                    dataGridView_Rouming.Rows[selectpozition].Selected = true;
                }
                catch (Exception)
                {
                    dataGridView_Rouming.Rows[0].Selected = true;
                }
            }
        }

        private void UpdateGrid_Rouming(DataTable table)
        {
            if (dataGridView_Rouming.InvokeRequired)
            {
                UpdateGridThreadHandler handler = UpdateGrid1_Rouming;
                dataGridView_Rouming.BeginInvoke(handler, table);
            }
            else
            {
                //save sort
                DataGridViewColumn oldColumn = dataGridView_Rouming.SortedColumn;
                ListSortDirection direction;
                if (dataGridView_Rouming.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
                else direction = ListSortDirection.Descending;

                //save scrol and selected row
                int scrollPosition = 0;
                int selectpozition = 0;
                if (dataGridView_Rouming.DataSource != null)
                {
                    scrollPosition = dataGridView_Rouming.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы
                    try
                    {
                        selectpozition = dataGridView_Rouming.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                    }
                    catch (Exception)
                    {
                        selectpozition = 0;
                    }
                }

                DataView dv = table.DefaultView;
                dv.Sort = "Дата зміни desc";
                DataTable sortedDT = dv.ToTable();

                dataGridView_Rouming.DataSource = table;
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
                    DataGridViewColumn newColumn = dataGridView_Rouming.Columns[oldColumn.Name.ToString()];
                    dataGridView_Rouming.Sort(newColumn, direction);
                    newColumn.HeaderCell.SortGlyphDirection =
                                     direction == ListSortDirection.Ascending ?
                                     SortOrder.Ascending : SortOrder.Descending;
                }

                if (dataGridView_Rouming.Rows.Count >= 0)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
                {
                    if (scrollPosition == -1)
                    {
                        scrollPosition = 0;
                    }
                    dataGridView_Rouming.FirstDisplayedScrollingRowIndex = scrollPosition;
                }
                if (dataGridView_Rouming.Rows.Count >= selectpozition)
                {
                    dataGridView_Rouming.ClearSelection();

                    try
                    {
                        dataGridView_Rouming.Rows[selectpozition].Selected = true;
                    }
                    catch (Exception)
                    {
                        dataGridView_Rouming.Rows[0].Selected = true;
                    }
                }
            }
        }

        private void dataGridView_Rouming_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            if (dataGridView_Rouming.Rows[e.RowIndex].Cells[9].Value.ToString() == "")//Згруповано до ID тривоги(9)
            {
                vars_form.search_id = dataGridView_Rouming.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_Rouming.Rows[e.RowIndex].Cells[0].Value.ToString();//ID Тривоги (0)
                vars_form.id_status = dataGridView_Rouming.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_Rouming.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_Rouming.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_Rouming.Rows[e.RowIndex].Cells[11].Value.ToString();//Звернення(4)
            }
            else
            {
                vars_form.search_id = dataGridView_Rouming.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_Rouming.Rows[e.RowIndex].Cells[0].Value.ToString();//Згруповано до ID тривоги(9)
                vars_form.id_status = dataGridView_Rouming.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_Rouming.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_Rouming.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_Rouming.Rows[e.RowIndex].Cells[10].Value.ToString();//Звернення(4)
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

        private void dataGridView_Rouming_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dataGridView_Rouming.SuspendLayout();

            if (dataGridView_Rouming.Rows[e.RowIndex].Cells[11].Value is true)
            {
                //if (Convert.ToDateTime(dataGridView_Rouming.Rows[e.RowIndex].Cells[12].Value) == DateTime.Now.Date)
                //{
                //    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#bcccf2");
                //}
                //else if (Convert.ToDateTime(dataGridView_Rouming.Rows[e.RowIndex].Cells[12].Value) <= DateTime.Now.Date)
                //{
                //    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F9A780");
                //}
                //else if (Convert.ToDateTime(dataGridView_Rouming.Rows[e.RowIndex].Cells[12].Value) >= DateTime.Now.Date)
                //{
                //    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#C8FAB7");
                //}
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }

            
            dataGridView_Rouming.ResumeLayout();
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
                                   "remayder_date as 'Дата нагадування', " +
                                   "otvetstvenniy as 'Відповідальний'  FROM btk.notification, btk.Users WHERE Users.idUsers=notification.Users_idUsers and Status = '808' " + vars_form.hide_group_alarm + "  ;");//order by btk.notification." + vars_form.sort.ToString() + " " + vars_form.order_sort + "
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
            }
            if (dataGridView_808_n.Rows.Count >= selectpozition)
            {
                dataGridView_808_n.ClearSelection();

                try
                {
                    dataGridView_808_n.Rows[selectpozition].Selected = true;
                }
                catch (Exception)
                {
                    dataGridView_808_n.Rows[0].Selected = true;
                }

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

                if (dataGridView_808_n.Rows.Count >= 0)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
                {
                    if (scrollPosition == -1)
                    {
                        scrollPosition = 0;
                    }
                    dataGridView_808_n.FirstDisplayedScrollingRowIndex = scrollPosition;
                }
                if (dataGridView_808_n.Rows.Count >= selectpozition)
                {
                    dataGridView_808_n.ClearSelection();
                    try
                    {
                        dataGridView_808_n.Rows[selectpozition].Selected = true;
                    }
                    catch (Exception)
                    {
                        dataGridView_808_n.Rows[0].Selected = true;
                    }
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
            }
            if (dataGridView_lost.Rows.Count >= selectpozition)
            {
                dataGridView_lost.ClearSelection();
                try
                {
                    dataGridView_lost.Rows[selectpozition].Selected = true;
                }
                catch (Exception)
                {
                    dataGridView_lost.Rows[0].Selected = true;
                }

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
                }
                if (dataGridView_lost.Rows.Count >= selectpozition)
                {
                    dataGridView_lost.ClearSelection();

                    try
                    {
                        dataGridView_lost.Rows[selectpozition].Selected = true;
                    }
                    catch (Exception)
                    {
                        dataGridView_lost.Rows[0].Selected = true;
                    }
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
            int scrollPosition = 0;
            int selectpozition = 0;
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
            if (dataGridView_909_n.Rows.Count >= selectpozition)
            {
                dataGridView_909_n.ClearSelection();
                try
                {
                    dataGridView_909_n.Rows[selectpozition].Selected = true;
                }
                catch (Exception)
                {
                    dataGridView_909_n.Rows[0].Selected = true;
                }
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
                int scrollPosition = 0;
                int selectpozition = 0;
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
                if (dataGridView_909_n.Rows.Count >= selectpozition)
                {
                    dataGridView_909_n.ClearSelection();

                    try
                    {
                        dataGridView_909_n.Rows[selectpozition].Selected = true;
                    }
                    catch (Exception)
                    {
                        dataGridView_909_n.Rows[0].Selected = true;
                    }
                }
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


        /// Обновляем вкладку accounts
        ///
        private void update_accounts_dgv()
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
                                   "remayder_date as 'Дата нагадування', " +
                                   "otvetstvenniy as 'Відповідальний'  FROM btk.notification, btk.Users WHERE Users.idUsers=notification.Users_idUsers and Status = 'Учетки' " + vars_form.hide_group_alarm + "  ;");//order by btk.notification." + vars_form.sort.ToString() + " " + vars_form.order_sort + "
            //table = vars_form.table_808;
            UpdateGridHandler ug = UpdateGrid_accounts;
            ug.BeginInvoke(table, cb_accounts, null);
        }

        private void cb_accounts(IAsyncResult res)
        {
        }

        private void UpdateGrid1_accounts(DataTable table)
        {
            //save sort
            DataGridViewColumn oldColumn = dataGridView_accounts.SortedColumn;
            ListSortDirection direction;
            if (dataGridView_accounts.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
            else direction = ListSortDirection.Descending;

            //save scrol and selected row
            int scrollPosition = 0;
            int selectpozition = 0;
            if (dataGridView_accounts.DataSource != null)
            {
                scrollPosition = dataGridView_accounts.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы

                try
                {
                    if (dataGridView_accounts.Rows.Count > 0)
                    {
                        selectpozition = dataGridView_accounts.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                    }
                }
                catch (Exception)
                {
                    selectpozition = 0;
                }
            }

            DataView dv = table.DefaultView;
            dv.Sort = "Дата зміни desc";
            DataTable sortedDT = dv.ToTable();

            dataGridView_accounts.DataSource = table;

            if (oldColumn != null)
            {
                DataGridViewColumn newColumn = dataGridView_accounts.Columns[oldColumn.Name.ToString()];
                dataGridView_accounts.Sort(newColumn, direction);
                newColumn.HeaderCell.SortGlyphDirection =
                                 direction == ListSortDirection.Ascending ?
                                 SortOrder.Ascending : SortOrder.Descending;
            }

            if (dataGridView_accounts.Rows.Count > 0)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
            {
                if (scrollPosition == -1)
                {
                    scrollPosition = 0;
                }
                dataGridView_accounts.FirstDisplayedScrollingRowIndex = scrollPosition;
            }
            if (dataGridView_accounts.Rows.Count > selectpozition)
            {
                dataGridView_accounts.ClearSelection();

                try
                {
                    dataGridView_accounts.Rows[selectpozition].Selected = true;
                }
                catch (Exception)
                {
                    dataGridView_accounts.Rows[0].Selected = true;
                }
            }
        }

        private void UpdateGrid_accounts(DataTable table)
        {
            if (dataGridView_accounts.InvokeRequired)
            {
                UpdateGridThreadHandler handler = UpdateGrid1_accounts;
                dataGridView_accounts.BeginInvoke(handler, table);
            }
            else
            {
                //save sort
                DataGridViewColumn oldColumn = dataGridView_accounts.SortedColumn;
                ListSortDirection direction;
                if (dataGridView_accounts.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
                else direction = ListSortDirection.Descending;

                //save scrol and selected row
                int scrollPosition = 0;
                int selectpozition = 0;
                if (dataGridView_accounts.DataSource != null)
                {
                    scrollPosition = dataGridView_accounts.FirstDisplayedScrollingRowIndex;//сохраняем позицию скрола перед обновлением таблицы

                    try
                    {
                        selectpozition = dataGridView_accounts.SelectedRows[0].Index; //dataGridView_909_n.CurrentCell.RowIndex;
                    }
                    catch (Exception)
                    {
                        selectpozition = 0;
                    }
                }

                DataView dv = table.DefaultView;
                dv.Sort = "Дата зміни desc";
                DataTable sortedDT = dv.ToTable();

                dataGridView_accounts.DataSource = table;

                //restote sort
                if (oldColumn != null)
                {
                    DataGridViewColumn newColumn = dataGridView_accounts.Columns[oldColumn.Name.ToString()];
                    dataGridView_accounts.Sort(newColumn, direction);
                    newColumn.HeaderCell.SortGlyphDirection =
                                     direction == ListSortDirection.Ascending ?
                                     SortOrder.Ascending : SortOrder.Descending;
                }

                if (dataGridView_accounts.Rows.Count >= 0)// если позиция скрола -1 то не меняем положенеие скрола (для случаем когда скрола нет)
                {
                    if (scrollPosition == -1)
                    {
                        scrollPosition = 0;
                    }
                    dataGridView_accounts.FirstDisplayedScrollingRowIndex = scrollPosition;
                }
                if (dataGridView_accounts.Rows.Count >= selectpozition)
                {
                    dataGridView_accounts.ClearSelection();

                    try
                    {
                        dataGridView_accounts.Rows[selectpozition].Selected = true;
                    }
                    catch (Exception)
                    {
                        dataGridView_accounts.Rows[0].Selected = true;
                    }
                }
            }
        }

        private void dataGridView_accounts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dataGridView_accounts.SuspendLayout();

            if (dataGridView_accounts.Rows[e.RowIndex].Cells[11].Value is true)
            {
                if (Convert.ToDateTime(dataGridView_accounts.Rows[e.RowIndex].Cells[12].Value) == DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#bcccf2");
                }
                else if (Convert.ToDateTime(dataGridView_accounts.Rows[e.RowIndex].Cells[12].Value) <= DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F9A780");
                }
                else if (Convert.ToDateTime(dataGridView_accounts.Rows[e.RowIndex].Cells[12].Value) >= DateTime.Now.Date)
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#C8FAB7");
                }
            }
            else
            {
                e.CellStyle.BackColor = Color.White;
            }

            dataGridView_accounts.Columns[10].Visible = false;
            dataGridView_accounts.Columns[8].Visible = false;
            dataGridView_accounts.Columns[4].Visible = false;
            dataGridView_accounts.ResumeLayout();
        }

        private void dataGridView_accounts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            if (dataGridView_accounts.Rows[e.RowIndex].Cells[9].Value.ToString() == "")//Згруповано до ID тривоги(9)
            {
                vars_form.search_id = dataGridView_accounts.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_accounts.Rows[e.RowIndex].Cells[0].Value.ToString();//ID Тривоги (0)
                vars_form.id_status = dataGridView_accounts.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_accounts.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_accounts.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_accounts.Rows[e.RowIndex].Cells[11].Value.ToString();//Звернення(4)
            }
            else
            {
                vars_form.search_id = dataGridView_accounts.Rows[e.RowIndex].Cells[4].Value.ToString(); //ID об"єкту(8)
                vars_form.id_notif = dataGridView_accounts.Rows[e.RowIndex].Cells[0].Value.ToString();//Згруповано до ID тривоги(9)
                vars_form.id_status = dataGridView_accounts.Rows[e.RowIndex].Cells[5].Value.ToString();//Статус(7)
                vars_form.unit_name = dataGridView_accounts.Rows[e.RowIndex].Cells[2].Value.ToString();//Назва об"єкту(2)
                vars_form.alarm_name = dataGridView_accounts.Rows[e.RowIndex].Cells[3].Value.ToString();//Тип тривоги(3)
                vars_form.restrict_un_group = false;
                vars_form.zvernenya = dataGridView_accounts.Rows[e.RowIndex].Cells[10].Value.ToString();//Звернення(4)
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
            }
            if (dataGridView_open_alarm.Rows.Count >= selectpozition)
            {
                dataGridView_open_alarm.ClearSelection();

                try
                {
                    dataGridView_open_alarm.Rows[selectpozition].Selected = true;
                }
                catch (Exception)
                {
                    dataGridView_open_alarm.Rows[0].Selected = true;
                }
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
                }
                if (dataGridView_open_alarm.Rows.Count >= selectpozition)
                {
                    dataGridView_open_alarm.ClearSelection();

                    try
                    {
                        dataGridView_open_alarm.Rows[selectpozition].Selected = true;
                    }
                    catch (Exception)
                    {
                        dataGridView_open_alarm.Rows[0].Selected = true;
                    }
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

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

        private void dateTimePicker_From_close_alarm_ValueChanged(object sender, EventArgs e)
        {
            mysql_close_alarm();
        }

        private void dateTimePicker_To_close_alarm_ValueChanged(object sender, EventArgs e)
        {
            mysql_close_alarm();
        }

        public void mysql_close_alarm()
        {
            //create random name for table
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var stringChars = new char[5];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var name_table = new String(stringChars);
            //-------------------

            string st1 = "CREATE TEMPORARY TABLE " +
                "btk." + name_table + " as (" +
                "SELECT " +
                "idnotification, " +
                "unit_id, " +
                "unit_name, " +
                "type_alarm, " +
                "product, " +
                "speed, " +
                "curr_time, " +
                "msg_time, location, " +
                "last_location, " +
                "notification.time_stamp, " +
                "group_alarm, " +
                "Status, " +
                "Users_idUsers, " +
                "alarm_locked_user " +
                "FROM btk.notification " +
                "WHERE " +
                "group_alarm is null " +
                "and msg_time between '" + Convert.ToDateTime(dateTimePicker_From_close_alarm.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + Convert.ToDateTime(dateTimePicker_To_close_alarm.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                "order by notification.idnotification DESC limit " + textBox_limit_close.Text + ");";

            string st2 = "SELECT " +
                "idnotification, " +
                "unit_id, unit_name, " +
                "type_alarm, product, " +
                "speed, " +
                "curr_time, " +
                "msg_time, " +
                "location, " +
                "last_location, " +
                "" + name_table + ".time_stamp, " +
                "group_alarm, Status, " +
                "Users_idUsers, " +
                "alarm_locked_user, " +
                "Users.username " +
                "FROM btk." + name_table + ", btk.Users " +
                "WHERE " +
                "Users.idUsers = " + name_table + ".Users_idUsers " +
                "and group_alarm is null " +
                "and btk." + name_table + ".unit_name LIKE '%" + search_close_alarm.Text + "%' " +
                "and btk." + name_table + ".type_alarm like '%" + comboBox_close_alarm_type.Text + "%' " +
                "and Users.username like '%" + textBox_close_user_chenge.Text + "%'";

            string st3 = "DROP TABLE IF EXISTS " + name_table + ";";



            string connectionString = "server = 10.44.30.32; " +
                                      "user id=lozik;" +
                                      "password=lozik;" +
                                      "database=btk;" +
                                      "pooling=true;" +
                                      "SslMode=none;" +
                                      "Convert Zero Datetime = True;" +
                                      "charset=utf8;";

            DataTable ds_close_alarm = new DataTable();
            using (MySqlConnection northwindConnection = new MySqlConnection(connectionString))
            {
                northwindConnection.Open();

                //MySqlCommand command = new MySqlCommand(st3, northwindConnection);
                //MySqlDataReader reader = command.ExecuteReader();
                //reader.Close();

                MySqlCommand command = new MySqlCommand(st1, northwindConnection);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Close();

                command = new MySqlCommand(st2, northwindConnection);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = command;
                ds_close_alarm.Locale = System.Globalization.CultureInfo.InvariantCulture;
                adapter.Fill(ds_close_alarm);

                command = new MySqlCommand(st3, northwindConnection);
                reader = command.ExecuteReader();
                reader.Close();

                northwindConnection.Close();
            }

            dataGridView_close_alarm.AutoGenerateColumns = false;
            dataGridView_close_alarm.RowHeadersVisible = false;

            int scrollPosition =
                this.dataGridView_close_alarm
                    .FirstDisplayedScrollingRowIndex; //сохраняем позицию скрола перед обновлением таблицы

            //save sort
            DataGridViewColumn oldColumn = dataGridView_close_alarm.SortedColumn;
            ListSortDirection direction;
            if (dataGridView_close_alarm.SortOrder == SortOrder.Ascending) direction = ListSortDirection.Ascending;
            else direction = ListSortDirection.Descending;

            dataGridView_close_alarm.DataSource = ds_close_alarm;

            if (oldColumn != null)
            {
                DataGridViewColumn newColumn = dataGridView_close_alarm.Columns[oldColumn.Name.ToString()];
                dataGridView_close_alarm.Sort(newColumn, direction);
                newColumn.HeaderCell.SortGlyphDirection =
                                 direction == ListSortDirection.Ascending ?
                                 SortOrder.Ascending : SortOrder.Descending;
            }

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
            if (e.RowIndex != -1)
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
            //this.Visible = false;// блокируем окно  пока открыто окно добавления
        }

        private void activation_form_deactivated(object sender, FormClosedEventArgs e)
        {
            //this.Visible = true;// разблокируем окно  кактолько закрыто окно добавления
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
            this.Enabled = false;// блокируем окно контрагентов пока открыто окно добавления контрагента
        }

        private void form_form_Zayavki_deactivated(object sender, FormClosedEventArgs e)
        {
            vars_form.if_open_created_zayavka = 0;
            this.Enabled = true;// разблокируем окно контрагентов кактолько закрыто окно добавления контрагента
            update_zayavki_na_aktivation_2W();
        }

        private void button_create_zayavka_Click(object sender, EventArgs e)
        {
            vars_form.if_open_created_zayavka = 0;
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

            vars_form.id_testing_for_zayavki = dataGridView_zayavki_na_activation.Rows[e.RowIndex].Cells[8].Value.ToString();
            vars_form.id_db_zayavki_for_activation = dataGridView_zayavki_na_activation.Rows[e.RowIndex].Cells[11].Value.ToString();
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
            vars_form.id_wl_object_for_activation = macros.sql_command("select t1.Object_id_wl from btk.Object as t1 inner join btk.testing_object as t2 where t2.Object_idObject = '" + vars_form.id_db_object_for_activation + "' and t1.idObject = t2.Object_idObject;");
            vars_form.if_open_created_activation = 1;


            DataTable results1 = macros.GetData("SELECT " +
                                         "Locked, " +
                                         "Locked_user " +
                                         "FROM btk.Activation_object " +
                                         "WHERE " +
                                         "idActivation_object = '" + vars_form.id_db_activation_for_activation + "';");

            if (vars_form.user_login_name != "admin" & vars_form.user_login_name != results1.Rows[0][1].ToString())
            {
                if (results1.Rows[0][0].ToString() == "1")
                {
                    MessageBox.Show("Користувач: " + results1.Rows[0][0] + " вже опрацовуе тривогу.");
                    return;
                }
            }

            macros.GetData("UPDATE btk.Activation_object " +
                                "SET " +
                                "Locked = '1', " +
                                "Locked_user = '" + vars_form.user_login_name + "' " +
                                "WHERE " +
                                "idActivation_object = '" + vars_form.id_db_activation_for_activation + "';");


            Activation_Form activation_form = new Activation_Form();
            activation_form.Activated += new EventHandler(activation_form_activated);
            activation_form.FormClosed += new FormClosedEventHandler(activation_form_deactivated);
            activation_form.Show();
        }

        //private void comboBox_activation_filter_DropDownClosed(object sender, EventArgs e)
        //{
        //    if (comboBox_activation_filter.SelectedIndex == 0)
        //    {
        //        dateTimePicker_activation_filter_end.Enabled = false;
        //        dateTimePicker_activation_filter_start.Enabled = false;
        //    }
        //    else
        //    {
        //        dateTimePicker_activation_filter_end.Enabled = true;
        //        dateTimePicker_activation_filter_start.Enabled = true;
        //    }
        //    update_actication_dgv();
        //}

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

        private void dataGridView_for_activation_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewRow row = dataGridView_for_activation.Rows[e.RowIndex];

            if (dataGridView_for_activation.Rows[e.RowIndex].Cells[11].Value is true)
            {
                if (Convert.ToDateTime(dataGridView_for_activation.Rows[e.RowIndex].Cells[12].Value) == DateTime.Now)
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#bcccf2");
                }
                else if (Convert.ToDateTime(dataGridView_for_activation.Rows[e.RowIndex].Cells[12].Value) <= DateTime.Now)
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F9A780");
                }
                else if (Convert.ToDateTime(dataGridView_for_activation.Rows[e.RowIndex].Cells[12].Value) >= DateTime.Now)
                {
                    row.DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#C8FAB7");
                }
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void dataGridView_for_activation_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

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
            if (textBox_search_object_name_activation.Text == "")
            {
                checkBox_activation_za_ves_chas_search.Enabled = false;
                checkBox_activation_za_ves_chas_search.Checked = false; 
            }
            else { checkBox_activation_za_ves_chas_search.Enabled = true;  }
            update_actication_dgv();
        }

        /// <Вкладка add_object>
        /// ////////
        /// </summary>

        ///

        private void Google_masseges()
        {
            if (LoadedGoogleMessage == 0)
            {
                LoadedGoogleMessage = 1;
                //GeckoPreferences.User["browser.xul.error_pages.enabled"] = true;
                geckoWebBrowser_GoogleMaseges.Navigate("https://messages.google.com/web/authentication");
            }

        }

        private void build_list_products()
        {
            string sql = string.Format("SELECT idproducts, product_name, productscol_hwid FROM btk.products order by ocherednost;");
            var dataSource = macros.GetData(sql);
            this.comboBox_list_poructs.DataSource = dataSource;
            //Setup data binding
            this.comboBox_list_poructs.DisplayMember = "product_name";
            this.comboBox_list_poructs.ValueMember = "idproducts";
            maskedTextBox_GSM_CODE.Enabled = false;
            maskedTextBox_BLE_CODE.Enabled = false;
            textBox_id_to_create.Enabled = false;
            maskedTextBox_PUK.Enabled = false;
            maskedTextBox_sim_no_to_create.Enabled = false;
            comboBox_tel_select.Enabled = false;
        }// Строим список продуктов - Имя=Название продукта, Значение=краткое название

        private void comboBox_list_poructs_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox_list_poructs.SelectedValue.ToString())
            {
                case "1"://None
                    //HW_id = 0;
                    maskedTextBox_GSM_CODE.Enabled = false;
                    maskedTextBox_BLE_CODE.Enabled = false;
                    textBox_id_to_create.Enabled = false;
                    maskedTextBox_PUK.Enabled = false;
                    maskedTextBox_PUK.ReadOnly = false;
                    maskedTextBox_sim_no_to_create.Enabled = false;
                    search_tovar_comboBox.Enabled = false;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = "";
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = false;

                    comboBox_tel2_select.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Text = "";
                    break;

                case "2"://CNTK
                    //HW_id = 9;
                    maskedTextBox_GSM_CODE.Enabled = false;
                    maskedTextBox_BLE_CODE.Enabled = false;
                    textBox_id_to_create.Enabled = true;
                    maskedTextBox_PUK.Enabled = true;
                    maskedTextBox_PUK.ReadOnly = false;
                    maskedTextBox_sim_no_to_create.Enabled = true;
                    search_tovar_comboBox.Enabled = false;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = ""; ;
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = true;

                    comboBox_tel2_select.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Text = "";
                    break;

                case "3"://CNTP
                    //int HW_id = "9";
                    maskedTextBox_GSM_CODE.Enabled = false;
                    maskedTextBox_BLE_CODE.Enabled = false;
                    textBox_id_to_create.Enabled = true;
                    maskedTextBox_PUK.Enabled = true;
                    maskedTextBox_PUK.ReadOnly = false;
                    maskedTextBox_sim_no_to_create.Enabled = true;
                    search_tovar_comboBox.Enabled = false;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = "";
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = true;

                    comboBox_tel2_select.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Text = "";
                    break;

                case "4"://CNTP-SE
                    //int HW_id = "38";
                    maskedTextBox_GSM_CODE.Enabled = false;
                    maskedTextBox_BLE_CODE.Enabled = false;
                    textBox_id_to_create.Enabled = true;
                    maskedTextBox_PUK.Enabled = true;
                    maskedTextBox_PUK.ReadOnly = false;
                    maskedTextBox_sim_no_to_create.Enabled = true;
                    search_tovar_comboBox.Enabled = false;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = "";
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = true;

                    comboBox_tel2_select.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Text = "";
                    break;

                case "5"://SLED
                    //selectet_product = "37";
                    maskedTextBox_GSM_CODE.Enabled = false;
                    maskedTextBox_BLE_CODE.Enabled = false;
                    textBox_id_to_create.Enabled = true;
                    maskedTextBox_PUK.Enabled = false;
                    maskedTextBox_sim_no_to_create.Enabled = true;
                    search_tovar_comboBox.Enabled = false;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = "";
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = true;

                    comboBox_tel2_select.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Text = "";
                    break;

                case "6"://C_n
                    //selectet_product = "9";
                    maskedTextBox_GSM_CODE.Enabled = false;
                    maskedTextBox_BLE_CODE.Enabled = false;
                    textBox_id_to_create.Enabled = true;
                    maskedTextBox_PUK.Enabled = false;
                    maskedTextBox_sim_no_to_create.Enabled = true;
                    search_tovar_comboBox.Enabled = false;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = "";
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = true;

                    comboBox_tel2_select.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Text = "";
                    break;

                case "7"://K_n
                    //selectet_product = "9";
                    maskedTextBox_GSM_CODE.Enabled = false;
                    maskedTextBox_BLE_CODE.Enabled = false;
                    textBox_id_to_create.Enabled = true;
                    maskedTextBox_PUK.Enabled = false;
                    maskedTextBox_sim_no_to_create.Enabled = true;
                    search_tovar_comboBox.Enabled = false;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = "";
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = true;

                    comboBox_tel2_select.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Text = "";
                    break;

                case "17"://KB_n
                    //selectet_product = "9";
                    maskedTextBox_GSM_CODE.Enabled = false;
                    maskedTextBox_BLE_CODE.Enabled = false;
                    textBox_id_to_create.Enabled = true;
                    maskedTextBox_PUK.Enabled = false;
                    maskedTextBox_sim_no_to_create.Enabled = true;
                    search_tovar_comboBox.Enabled = false;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = "";
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = true;

                    comboBox_tel2_select.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Text = "";
                    break;

                case "12"://KP_n
                    //selectet_product = "9";
                    maskedTextBox_GSM_CODE.Enabled = false;
                    maskedTextBox_BLE_CODE.Enabled = false;
                    textBox_id_to_create.Enabled = true;
                    maskedTextBox_PUK.Enabled = false;
                    maskedTextBox_sim_no_to_create.Enabled = true;
                    search_tovar_comboBox.Enabled = false;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = "";
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = true;

                    comboBox_tel2_select.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Text = "";

                    comboBox_tel2_select.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Text = "";
                    break;

                case "8"://W
                    //selectet_product = "456";
                    maskedTextBox_GSM_CODE.Enabled = false;
                    maskedTextBox_BLE_CODE.Enabled = false;
                    textBox_id_to_create.Enabled = true;
                    maskedTextBox_PUK.Enabled = false;
                    maskedTextBox_sim_no_to_create.Enabled = true;
                    search_tovar_comboBox.Enabled = false;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = "";
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = true;

                    comboBox_tel2_select.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Text = "";
                    break;

                case "9"://S
                    //selectet_product = "8";
                    maskedTextBox_GSM_CODE.Enabled = false;
                    maskedTextBox_BLE_CODE.Enabled = false;
                    textBox_id_to_create.Enabled = true;
                    maskedTextBox_PUK.Enabled = false;
                    maskedTextBox_sim_no_to_create.Enabled = false;
                    comboBox_tel_select.Enabled = false;
                    search_tovar_comboBox.Enabled = false;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = "";
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = false;

                    comboBox_tel2_select.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Text = "";
                    break;

                case "10"://CNTK_910
                    //selectet_product = "9";
                    maskedTextBox_GSM_CODE.Enabled = true;
                    maskedTextBox_BLE_CODE.Enabled = true;
                    textBox_id_to_create.Enabled = true;
                    maskedTextBox_PUK.Enabled = true;
                    maskedTextBox_PUK.ReadOnly = true;
                    maskedTextBox_sim_no_to_create.Enabled = true;
                    search_tovar_comboBox.Enabled = true;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = "";
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = true;

                    comboBox_tel2_select.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Text = "";
                    break;

                case "11"://CNTP_910
                    //selectet_product = "9";
                    maskedTextBox_GSM_CODE.Enabled = true;
                    maskedTextBox_BLE_CODE.Enabled = true;
                    textBox_id_to_create.Enabled = true;
                    maskedTextBox_PUK.Enabled = true;
                    maskedTextBox_PUK.ReadOnly = true;
                    maskedTextBox_sim_no_to_create.Enabled = true;
                    search_tovar_comboBox.Enabled = true;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = "";
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = true;

                    comboBox_tel2_select.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Text = "";
                    break;

                case "13"://CNTP_910_SE_N
                    //selectet_product = "9";
                    maskedTextBox_GSM_CODE.Enabled = true;
                    maskedTextBox_BLE_CODE.Enabled = true;
                    textBox_id_to_create.Enabled = true;
                    maskedTextBox_PUK.Enabled = true;
                    maskedTextBox_PUK.ReadOnly = true;
                    maskedTextBox_sim_no_to_create.Enabled = true;
                    search_tovar_comboBox.Enabled = true;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = "";
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = true;

                    comboBox_tel2_select.Enabled = true;
                    maskedTextBox_sim2_no_to_create.Enabled = true;
                    maskedTextBox_sim2_no_to_create.Text = "";

                    break;

                case "14"://CNTP_910_SE_P
                    //selectet_product = "9";
                    maskedTextBox_GSM_CODE.Enabled = true;
                    maskedTextBox_BLE_CODE.Enabled = true;
                    textBox_id_to_create.Enabled = true;
                    maskedTextBox_PUK.Enabled = true;
                    maskedTextBox_PUK.ReadOnly = true;
                    maskedTextBox_sim_no_to_create.Enabled = true;
                    search_tovar_comboBox.Enabled = true;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = "";
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = true;

                    comboBox_tel2_select.Enabled = true;
                    maskedTextBox_sim2_no_to_create.Enabled = true;
                    maskedTextBox_sim2_no_to_create.Text = "";
                    break;

                case "18"://CNTK_910
                    //selectet_product = "9";
                    maskedTextBox_GSM_CODE.Enabled = true;
                    maskedTextBox_BLE_CODE.Enabled = true;
                    textBox_id_to_create.Enabled = true;
                    maskedTextBox_PUK.Enabled = true;
                    maskedTextBox_PUK.ReadOnly = true;
                    maskedTextBox_sim_no_to_create.Enabled = true;
                    search_tovar_comboBox.Enabled = true;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = "";
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = true;

                    comboBox_tel2_select.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Text = "";
                    break;

                case "19"://CNTP_910
                    //selectet_product = "9";
                    maskedTextBox_GSM_CODE.Enabled = true;
                    maskedTextBox_BLE_CODE.Enabled = true;
                    textBox_id_to_create.Enabled = true;
                    maskedTextBox_PUK.Enabled = true;
                    maskedTextBox_PUK.ReadOnly = true;
                    maskedTextBox_sim_no_to_create.Enabled = true;
                    search_tovar_comboBox.Enabled = true;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = "";
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = true;

                    comboBox_tel2_select.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Enabled = false;
                    maskedTextBox_sim2_no_to_create.Text = "";
                    break;

                case "20"://CNTP_910_SE_N
                    //selectet_product = "9";
                    maskedTextBox_GSM_CODE.Enabled = true;
                    maskedTextBox_BLE_CODE.Enabled = true;
                    textBox_id_to_create.Enabled = true;
                    maskedTextBox_PUK.Enabled = true;
                    maskedTextBox_PUK.ReadOnly = true;
                    maskedTextBox_sim_no_to_create.Enabled = true;
                    search_tovar_comboBox.Enabled = true;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = "";
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = true;

                    comboBox_tel2_select.Enabled = true;
                    maskedTextBox_sim2_no_to_create.Enabled = true;
                    maskedTextBox_sim2_no_to_create.Text = "";

                    break;

                case "21"://CNTP_910_SE_P
                    //selectet_product = "9";
                    maskedTextBox_GSM_CODE.Enabled = true;
                    maskedTextBox_BLE_CODE.Enabled = true;
                    textBox_id_to_create.Enabled = true;
                    maskedTextBox_PUK.Enabled = true;
                    maskedTextBox_PUK.ReadOnly = true;
                    maskedTextBox_sim_no_to_create.Enabled = true;
                    search_tovar_comboBox.Enabled = true;
                    maskedTextBox_BLE_CODE.Text = "";
                    maskedTextBox_GSM_CODE.Text = "";
                    textBox_id_to_create.Text = "";
                    maskedTextBox_PUK.Text = "";
                    maskedTextBox_sim_no_to_create.Text = "";
                    textBox_bt_enable.Text = "";
                    comboBox_tel_select.Enabled = true;

                    comboBox_tel2_select.Enabled = true;
                    maskedTextBox_sim2_no_to_create.Enabled = true;
                    maskedTextBox_sim2_no_to_create.Text = "";
                    break;
            }
        }

        private void search_tovar_comboBox_DropDownClosed(object sender, EventArgs e)
        {
            //DataTable t = new DataTable();
            //t = macros.GetData("SELECT CN, IMEI, BtKey, Puk, gsm_code, tag_access_code, SN FROM btk.Invice_Tovar where SN='" + search_tovar_comboBox.Text + "';");
            //if (t != null & t.Rows.Count > 0)
            //{
            //    textBox_id_to_create.Text = t.Rows[0][0].ToString();
            //    maskedTextBox_GSM_CODE.Text = t.Rows[0][4].ToString();
            //    maskedTextBox_PUK.Text = t.Rows[0][3].ToString();
            //    maskedTextBox_BLE_CODE.Text = t.Rows[0][2].ToString();
            //    textBox_bt_enable.Text = t.Rows[0][5].ToString();
            //    Clipboard.SetText(textBox_id_to_create.Text);
            //}
        }

        private void button_copy_tel_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(maskedTextBox_sim_no_to_create.Text);
        }

        private void button_copy_sms_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(maskedTextBox_GSM_CODE.Text + "*27183#" + textBox_bt_enable.Text);
        }

        private void button_create_object_Click(object sender, EventArgs e)
        {

            ///////
            ///В зависимости от выбранного продукта в комбоксе запускаем необходимый
            ///
            switch (comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem))
            {
                case "Пусто":
                    MessageBox.Show("Выбери продукт");
                    break;

                case "CNTK":
                    CNTK();
                    break;

                case "CNTP":
                    CNTP();
                    break;

                case "CNTP-SE":
                    MessageBox.Show("В разработке, скоро будет..");
                    break;

                case "SLED":
                    SLED();
                    break;

                case "C_n":
                    C_N();
                    break;

                case "K_n":
                    K_n();
                    break;

                case "KP_n":
                    KP_n();
                    break;

                case "KB_n":
                    KB_n();
                    break;

                case "W":
                    MessageBox.Show("В разработке, скоро будет..");
                    break;

                case "S":
                    S();
                    break;

                case "CNTK_910":
                    CNTK_910();
                    break;

                case "CNTP_910":
                    CNTP_910();
                    break;

                case "CNTK_910BT":
                    CNTK_910BT();
                    break;

                case "CNTP_910BT":
                    CNTP_910BT();
                    break;

                case "CNTP_910_SE_N":
                    CNTP_910_N();
                    break;

                case "CNTP_910_SE_P":
                    CNTP_910_P();
                    break;

                case "CNTP_910BT_SE_N":
                    CNTP_910BT_N();
                    break;

                case "CNTP_910BT_SE_P":
                    CNTP_910BT_P();
                    break;
            }
            UpdateCreatedObjectsByUser(DateTime.Now.Date);
        }

        private void check_err_create_obj()
        {

        }

        private void CNTK_910()
        {
            /////////////////////
            //////Проверяем корректность введенных данных
            ///

            if (textBox_id_to_create.Text.Length <= 3)
            {
                textBox_id_to_create.BackColor = Color.Red;//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
                return;
            }

            if (maskedTextBox_sim_no_to_create.Text.Length <= 11)//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Red;
                return;
            }
            if (maskedTextBox_PUK.Text.Length <= 3)//Если PUK короче 4х символов останавливается и подсвкечиваем желтым
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Yellow;
                DialogResult result = MessageBox.Show(
                    "Вопрос",
                    "PUK Верный?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.No)
                    return;
                maskedTextBox_PUK.BackColor = Color.White;
            }
            // Проверям существует ли данный номер в системе
            string unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли указанный ИМЕЙ в системе
            string unswer2 = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_unique_id\"," +
                                                     "\"propValueMask\":\"" + textBox_id_to_create.Text + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m2 = JsonConvert.DeserializeObject<RootObject>(unswer2);
            if (m2.items.Count > 0)
            {
                MessageBox.Show("Указанный IMEI существует в WL");
                return;
            }

            /////////////////
            ///Создаем объект
            /////////////////
            string cr_obj_in = "&svc=core/create_unit&params={\"creatorId\":\"" + vars_form.wl_user_id + "\",\"name\":\"" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "\",\"hwTypeId\":\"9\",\"dataFlags\":\"1\"}";
            string json = macros.WialonRequest(cr_obj_in);
            var cr_obj_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (cr_obj_out.error != 0)
            {
                string text = macros.Get_wl_text_error(cr_obj_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Создаем пароль доступа к объекту
            /////////////////

            string accsess_pass_answer = macros.WialonRequest(
                "&svc=unit/update_access_password&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"accessPassword\":\"" + maskedTextBox_GSM_CODE.Text.ToString() + "\"}");
            var accsess_pass_answer_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (accsess_pass_answer_out.error != 0)
            {
                string text = macros.Get_wl_text_error(accsess_pass_answer_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Установливаем имя объекта и ID оборудования и
            /////////////////
            string item_id_in =
                "&svc=unit/update_device_type&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id
                + "\",\"deviceTypeId\":\"" + "9"
                + "\",\"uniqueId\":\"" + textBox_id_to_create.Text.ToString() + "\"}";
            string json2 = macros.WialonRequest(item_id_in);
            var item_id_out = JsonConvert.DeserializeObject<RootObject>(json2);
            if (item_id_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_id_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Устанавливаем номер СИМ
            /////////////////
            string item_phone_in = "&svc=unit/update_phone&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim_no_to_create.Text) + "\"}";
            string json3 = macros.WialonRequest(item_phone_in);
            var item_phone_out = JsonConvert.DeserializeObject<RootObject>(json3);
            if (item_phone_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Создаем датчики
            /////////////////

            //1. Автоматическая блокировка двигателя
            string auto_block_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Автоматическая блокировка двигателя", "digital", "Вкл/Выкл", "auto_block", 1, "1", 0, "");

            //2. Охрана
            string protection_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Охрана", "digital", "Вкл/Выкл", "protection", 2, "1", 0, "");

            //3. Принудительная блокировка двигателя
            string man_block_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Принудительная блокировка двигателя", "digital", "Вкл/Выкл", "man_block", 3, "1", 0, "");

            //4. Авторизация
            string auth_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Авторизация", "digital", "Вкл/Выкл", "auth", 4, "1", 0, "");

            //5. Тревожная кнопка
            string alarm_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Тревожная кнопка_", "digital", "Вкл/Выкл", "alarm", 5, "1", 0, "");

            //6. Сервисный режим
            string valet_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сервисный режим", "digital", "Вкл/Выкл", "valet", 6, "1", 0, "");

            //7. Двери статус
            string all_door_status_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Двери статус", "digital", "Вкл/Выкл", "trunk%2Bhood%2Bpass_door%2Bdriver_door%2Bl_rear_door%2Br_rear_door", 21, "1", 0, "");

            //8. Сработка открытие дверей в охране
            string Door_in_protection_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка открытие дверей в охране", "digital", "Вкл/Выкл", "alarm_act", 7, "1", 7, "");

            //9. Сработка сирены
            string alarm_act_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка сирены", "digital", "Вкл/Выкл", "alarm_act", 8, "1", 0, "");

            //10. Сработка датчика наклона
            string tilt_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика наклона", "digital", "Вкл/Выкл", "tilt_trig", 9, "1", 0, "");

            //11. Напряжение АКБ
            string ext_voltage_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Напряжение АКБ", "voltage", "В", "ext_voltage", 10, "1", 0, "");

            //12. Сработка датчика ударов
            string sh_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика ударов", "digital", "Вкл/Выкл", "sh_trig", 11, "1", 0, "");

            //13. Предупреждение датчика ударов
            string sh_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение датчика ударов", "digital", "Вкл/Выкл", "sh_warn", 12, "1", 0, "");

            //14. Сработка доп. датчика 1
            string ext1_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка доп. датчика 1", "digital", "Вкл/Выкл", "ext1_trig", 13, "1", 0, "");

            //15. Предупреждение доп. датчика 1
            string ext1_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение доп. датчика 1", "digital", "Вкл/Выкл", "ext1_warn", 14, "1", 0, "");

            //16. Сработка доп. датчика 2
            string ext2_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка доп. датчика 2", "digital", "Вкл/Выкл", "ext2_trig", 15, "1", 0, "");

            //17. Предупреждение доп. датчика 2
            string ext2_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение доп. датчика 2", "digital", "Вкл/Выкл", "ext2_warn", 16, "1", 0, "");

            //18. РАКБ
            string acc_voltage_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "РАКБ", "voltage", "В", "acc_voltage", 17, "1", 0, "");

            //19. Зажигание
            string ign_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Зажигание", "engine operation", "Вкл/Выкл", "ign", 18, "1", 0, "");

            //20. Центральный замок
            string central_lock_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Центральный замок", "digital", "Открыто/Закрыто", "central_lock", 19, "1", 0, "");

            //21. Двигатель заведен
            string engine_is_on_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Двигатель заведен", "digital", "Вкл/Выкл", "engine_is_on", 20, "1", 0, "");

            //22. Капот
            string hood_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Капот", "digital", "Откр/Закр", "hood", 22, "1", 0, "");

            //23. Передняя левая дверь (водителя)
            string driver_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Передняя левая дверь (водителя)", "digital", "Откр/Закр", "driver_door", 23, "1", 0, "");

            //24. Передняя правая дверь (пасс.)
            string pass_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Передняя правая дверь (пасс.)", "digital", "Откр/Закр", "pass_door", 24, "1", 0, "");

            //25. Задняя левая дверь
            string l_rear_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Задняя левая дверь", "digital", "Откр/Закр", "l_rear_door", 25, "1", 0, "");

            //26. Правая задняя дверь
            string r_rear_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Правая задняя дверь", "digital", "Откр/Закр", "r_rear_door", 26, "1", 0, "");

            //27. Багажник
            string trunk_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Багажник", "digital", "Откр/Закр", "trunk", 27, "1", 0, "");

            //28. Пробег
            string can_odo_km_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Пробег", "mileage", "КМ", "can_odo_km", 28, "1", 0, "");

            //29. Топливо
            string fuel_lev_l_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Топливо", "fuel level", "л", "fuel_lev_l", 29, "1", 0, "");

            //30. Ручной тормоз
            string hand_break_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Ручной тормоз", "digital", "Вкл/Выкл", "hand_break", 30, "1", 0, "");

            //31. Габаритные огни
            string marker_lights_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Габаритные огни", "digital", "Вкл/Выкл", "marker_lights", 31, "1", 0, "");

            //32. АКПП в P
            string parking_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "АКПП в P", "digital", "Вкл/Выкл", "parking", 32, "1", 0, "");

            //33. Ближний свет
            string dipped_beam_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Ближний свет", "digital", "Вкл/Выкл", "dipped_beam", 33, "1", 0, "");

            //34. Низкий уровень омывающей жидкости
            string washer_alert_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Низкий уровень омывающей жидкости", "digital", "Вкл/Выкл", "washer_alert", 34, "1", 0, "");

            //35. Температура двигателя
            string eng_temp_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Температура двигателя", "temperature", "C", "eng_temp", 35, "1", 0, "");

            //36. Сработка датчика глушения
            string aux_zone_1_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика глушения", "digital", "Вкл/Выкл", "aux_zone_1", 36, "1", 0, "");

            /////////////////
            ///Создаем произвольные поля
            /////////////////

            //Произвольное поле 1
            string answer = macros.create_custom_field_wl(cr_obj_out.item.id, "0 УВАГА", "");

            //Произвольное поле 2
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "02 Проект", "");

            //Произвольное поле 3
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.1 І Відповідальна особа (основна)", "");

            //Произвольное поле 4
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.2 ІІ Відповідальна особа", "");

            //Произвольное поле 5
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.3 ІІІ Відповідальна особа", "");

            //Произвольное поле 6
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.4 ІV Відповідальна особа", "");

            //Произвольное поле 7
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.5 V Відповідальна особа", "");

            //Произвольное поле 8
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.1.1 Оператор, що тестував", "");

            //Произвольное поле 9
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.11 Додатково встановлені сигналізації", "");

            //Произвольное поле 10
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.12 Постановка авто под охрану через багажник?", "");

            //Произвольное поле 11
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.15 Додатково встановлені датчики", "");

            //Произвольное поле 12
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.2.1 Установник: назва, адреса", "");

            //Произвольное поле 13
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.2.2 Установник-монтажник: ПІБ, №тел.", "");

            //Произвольное поле 14
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.3 Дата установки", "");

            //Произвольное поле 15
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.4 Місце установки пристрою ВЕНБЕСТ", "");

            //Произвольное поле 16
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.1 CAN-реле", "");

            //Произвольное поле 17
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.2 Звичайне реле", "");

            //Произвольное поле 18
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.7 Місце встановлення сервісної кнопки", "");

            //Произвольное поле 19
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.8.1 Дротова тривожна кнопка", "");

            //Произвольное поле 20
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.8.2 Бездротова тривожна кнопка", "");

            //Произвольное поле 21
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.3 Блокує Prizrak по CAN", "");

            //Произвольное поле 22
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.9.2 Штатні кнопки введення PIN-коду", "");

            //Административное поле 1
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.3 GSM-код", maskedTextBox_GSM_CODE.Text.ToString());

            //Административное поле 2
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.4 PUK-код", maskedTextBox_PUK.Text.ToString());

            //Административное поле 3
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.5 Bluetuth-код", maskedTextBox_BLE_CODE.Text.ToString());

            //Произвольное поле 21
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.1 Дата активації", "");

            //Произвольное поле 22
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.1.1 Оператор, що активував", "");

            //Произвольное поле 23
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.2 Дата встановлення PIN-коду", "");

            //Произвольное поле 24
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.3 PIN-код встановлено особою(клієнт/установлник)", "");

            //Произвольное поле 25
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.4 Обліковий запис WL", "");

            //Произвольное поле 26
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.1 Менеджер", "");

            //Произвольное поле 27
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.2 Договір обслуговування", "");

            //Произвольное поле 28
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.3 Гарантія до", "");

            //Произвольное поле 29
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.4 Дата закінчення договору страхування", "");

            //Произвольное поле 30
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "8.1 Паркінг 1", "");

            //Произвольное поле 31
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "8.2 Паркінг 2", "");

            //Произвольное поле 33
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.0 Примітки", "");

            //Произвольное поле 33
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.1 Техпаспорт", "");

            //Произвольное поле 34
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.2.1 Дата перевірки картки", "ДД.ММ.РРРР");

            //Произвольное поле 35
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.2.2 Оператор перевірки картки", "Прізвище");

            //Произвольное поле 36
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "10 Кодове слово", "");

            //Админитстративное поле 40
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "13 Prizrak 910 SN", search_tovar_comboBox.Text);

            /////////////////
            ///Добавляем в группу Объекты All
            /////////////////

            string get_units_on_group = macros.WialonRequest(
                "&svc=core/search_item&params={"
                + "\"id\":\"2612\","
                + "\"flags\":\"1\"}");//получаем все объекты группы

            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

            list_get_units_on_group.item.u.Add(cr_obj_out.item.id);//Доповляем в список новый объект
            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

            string gr_answer = macros.WialonRequest(
                "&svc=unit_group/update_units&params={"
                + "\"itemId\":\"2612\","
                + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

            /////////////////
            ///Создаем команды
            /////////////////
            ///http://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/unit/update_command_definition
            /// 83886080- закрыта команда  для клиента
            /// 16777216- открыта команда для клиента

            //0 - Запросить текущее состояние
            string cmd_refresh = macros.create_commads_wl(cr_obj_out.item.id, "0 - Запросить текущее состояние", "%23refresh%23", 83886080);

            //0 - Перезагрузить систему
            string cmd_reboot = macros.create_commads_wl(cr_obj_out.item.id, "0 - Перезагрузить систему", "%23reboot%23", 83886080);

            //1 - Закрыть автомобиль
            string cmd_protection_on = macros.create_commads_wl(cr_obj_out.item.id, "1 - Закрыть автомобиль", "%23protection=1%23", 83886080);

            //1 - Открыть автомобиль
            string cmd_protection_off = macros.create_commads_wl(cr_obj_out.item.id, "1 - Открыть автомобиль", "%23protection=0%23", 83886080);

            //2 - Автозапуск старт
            string cmd_autostart_on = macros.create_commads_wl(cr_obj_out.item.id, "2 - Автозапуск старт", "%23autostart=1%23", 83886080);

            //2 - Автозапуск стоп
            string cmd_autostart_off = macros.create_commads_wl(cr_obj_out.item.id, "2 - Автозапуск стоп", "%23autostart=0%23", 83886080);

            //3 - СТАРТ двигатель
            string cmd_man_block_off = macros.create_commads_wl(cr_obj_out.item.id, "3 - СТАРТ двигатель", "%23man_block=0%23", 83886080);

            //3 - СТОП двигатель
            string cmd_man_block_on = macros.create_commads_wl(cr_obj_out.item.id, "3 - СТОП двигатель", "%23man_block=1%23", 83886080);

            //4 - Включить сирену
            string cmd_alarm_on = macros.create_commads_wl(cr_obj_out.item.id, "4 - Включить сирену", "%23alarm=1%23", 83886080);

            //4 - Выключить сирену
            string cmd_alarm_off = macros.create_commads_wl(cr_obj_out.item.id, "4 - Выключить сирену", "%23alarm=0%23", 83886080);

            //5 - Включить поиск на парковке
            string cmd_search_on = macros.create_commads_wl(cr_obj_out.item.id, "5 - Включить поиск на парковке", "%23search=1%23", 83886080);

            //5 - Выключить поиск на парковке
            string cmd_search_off = macros.create_commads_wl(cr_obj_out.item.id, "5 - Выключить поиск на парковке", "%23search=0%23", 83886080);

            //6 - Включить сервисный режим
            string cmd_valet_on = macros.create_commads_wl(cr_obj_out.item.id, "6 - Включить сервисный режим", "%23valet=1%23", 16777216);

            //6 - Выключить сервисный режиме
            string cmd_valet_off = macros.create_commads_wl(cr_obj_out.item.id, "6 - Выключить сервисный режиме", "%23valet=0%23", 16777216);

            string id_sim = "";

            DataTable t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel_select.Text + "';");
            if (t.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(4) + "';");
            }

            string sql2 = string.Format("insert into btk.Object(Object_id_wl, Object_imei, Object_name, Object_sim_no, products_idproducts, Simcard_idSimcard, TS_info_idTS_info, TS_info_TS_brend_model_idTS_brend_model, Kontakti_idKontakti_serviceman, Dogovora_idDogovora, Objectcol_edit_date, Users_idUsers, Objectcol_sn_puk_card, Objectcol_puk, Objectcol_gsm_code, Objectcol_ble_code, Objectcol_bt_enable, Objectcol_sn_prizrak) "
                + "values('" + cr_obj_out.item.id
                + "', '" + textBox_id_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem)
                + " " + textBox_id_to_create.Text.ToString() + "','"
                + maskedTextBox_sim_no_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.SelectedValue.ToString() + "'," +
                " '" + id_sim + "'," +
                " '1', '1', '1','1', null, '" + vars_form.user_login_id + "', '"
                + textBox_id_to_create.Text.ToString() + "', '"
                + maskedTextBox_PUK.Text.ToString() + "', '"
                + maskedTextBox_GSM_CODE.Text.ToString() + "', '"
                + maskedTextBox_BLE_CODE.Text.ToString() + "', '"
                + textBox_bt_enable.Text.ToString() + "', '"
                + search_tovar_comboBox.GetItemText(search_tovar_comboBox.SelectedItem) + "');");
            macros.sql_command(sql2);

            //получаем айди созданного объекта
            string id_object = macros.sql_command("SELECT MAX(idObject) FROM btk.Object;");

            // set datetime create
            macros.sql_command("update btk.Object set date_cteate = now() where idobject = '" + id_object + "';");

            //Создаем подписку для объекта
            string sql3 = string.Format("INSERT INTO btk.Subscription(products_has_Tarif_idproducts_has_Tarif, idobject) values('20'," + id_object + ");");
            macros.sql_command(sql3);

            //Получаем айди созданной подписки
            string sql4 = macros.sql_command("select max(idSubscr) from btk.Subscription;");

            //привязываем айди созданного объекта с айди созданной подписки
            string sql5 = string.Format("insert into btk.object_subscr (Object_idObject, Subscription_idSubscr) values (" + id_object + "," + sql4 + ");");
            macros.sql_command(sql5);

            ///////////////////////
            //Если все прошло успешно - завечиваем зеленым кнопку и затирает текстбоксы
            //////////////////////////

            button_create_object.Text = "Створено: " + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text + "\n Дата та час: " + DateTime.Now.ToString();
            maskedTextBox_BLE_CODE.Text = "";
            maskedTextBox_GSM_CODE.Text = "";
            textBox_id_to_create.Text = "";
            maskedTextBox_PUK.Text = "";
            maskedTextBox_sim_no_to_create.Text = "";
            textBox_bt_enable.Text = "";
            button_create_object.BackColor = Color.Green;
            comboBox_tel_select.Text = "";
            search_tovar_comboBox.Text = "";

        }

        private void CNTP_910()
        {
            /////////////////////
            //////Проверяем корректность введенных данных
            ///

            if (textBox_id_to_create.Text.Length <= 3)
            {
                textBox_id_to_create.BackColor = Color.Red;//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
                return;
            }
            if (maskedTextBox_sim_no_to_create.Text.Length <= 11)//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Red;
                return;
            }
            if (maskedTextBox_PUK.Text.Length <= 3)//Если PUK короче 4х символов останавливается и подсвкечиваем желтым
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Yellow;
                DialogResult result = MessageBox.Show(
                    "Вопрос",
                    "PUK Верный?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.No)
                    return;
                maskedTextBox_PUK.BackColor = Color.White;
            }

            // Проверям существует ли данный номер в системе
            string unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли указанный ИМЕЙ в системе
            string unswer2 = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_unique_id\"," +
                                                     "\"propValueMask\":\"" + textBox_id_to_create.Text + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m2 = JsonConvert.DeserializeObject<RootObject>(unswer2);
            if (m2.items.Count > 0)
            {
                MessageBox.Show("Указанный IMEI существует в WL");
                return;
            }

            /////////////////
            ///Создаем объект
            /////////////////
            string cr_obj_in = "&svc=core/create_unit&params={\"creatorId\":\"" + vars_form.wl_user_id + "\",\"name\":\"" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "\",\"hwTypeId\":\"9\",\"dataFlags\":\"1\"}";
            string json = macros.WialonRequest(cr_obj_in);
            var cr_obj_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (cr_obj_out.error != 0)
            {
                string text = macros.Get_wl_text_error(cr_obj_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Создаем пароль доступа к объекту
            /////////////////

            string accsess_pass_answer = macros.WialonRequest(
                "&svc=unit/update_access_password&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"accessPassword\":\"" + maskedTextBox_GSM_CODE.Text.ToString() + "\"}");
            var accsess_pass_answer_out = JsonConvert.DeserializeObject<RootObject>(accsess_pass_answer);
            if (accsess_pass_answer_out.error != 0)
            {
                string text = macros.Get_wl_text_error(accsess_pass_answer_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Установливаем имя объекта и ID оборудования и
            /////////////////
            string item_id_in =
                "&svc=unit/update_device_type&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id
                + "\",\"deviceTypeId\":\"" + "9"
                + "\",\"uniqueId\":\"" + textBox_id_to_create.Text.ToString() + "\"}";
            string json2 = macros.WialonRequest(item_id_in);
            var item_id_out = JsonConvert.DeserializeObject<RootObject>(json2);
            if (item_id_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_id_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Устанавливаем номер СИМ
            /////////////////
            string item_phone_in = "&svc=unit/update_phone&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim_no_to_create.Text) + "\"}";
            string json3 = macros.WialonRequest(item_phone_in);
            var item_phone_out = JsonConvert.DeserializeObject<RootObject>(json3);
            if (item_phone_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Создаем датчики
            /////////////////

            //1. Автоматическая блокировка двигателя
            string auto_block_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Автоматическая блокировка двигателя", "digital", "Вкл/Выкл", "auto_block", 1, "1", 0, "");

            //2. Охрана
            string protection_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Охрана", "digital", "Вкл/Выкл", "protection", 2, "1", 0, "");

            //3. Принудительная блокировка двигателя
            string man_block_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Принудительная блокировка двигателя", "digital", "Вкл/Выкл", "man_block", 3, "1", 0, "");

            //4. Авторизация
            string auth_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Авторизация", "digital", "Вкл/Выкл", "auth", 4, "1", 0, "");

            //5. Тревожная кнопка
            string alarm_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Тревожная кнопка_", "digital", "Вкл/Выкл", "alarm", 5, "1", 0, "");

            //6. Сервисный режим
            string valet_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сервисный режим", "digital", "Вкл/Выкл", "valet", 6, "1", 0, "");

            //7. Двери статус
            string all_door_status_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Двери статус", "digital", "Вкл/Выкл", "trunk%2Bhood+%2Bpass_door%2Bdriver_door%2Bl_rear_door%2Br_rear_door", 21, "1", 0, "");

            //8. Сработка открытие дверей в охране
            string Door_in_protection_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка открытие дверей в охране", "digital", "Вкл/Выкл", "alarm_act", 7, "1", 7, "");

            //9. Сработка сирены
            string alarm_act_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка сирены", "digital", "Вкл/Выкл", "alarm_act", 8, "1", 0, "");

            //10. Сработка датчика наклона
            string tilt_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика наклона", "digital", "Вкл/Выкл", "tilt_trig", 9, "1", 0, "");

            //11. Напряжение АКБ
            string ext_voltage_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Напряжение АКБ", "voltage", "В", "ext_voltage", 10, "1", 0, "");

            //12. Сработка датчика ударов
            string sh_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика ударов", "digital", "Вкл/Выкл", "sh_trig", 11, "1", 0, "");

            //13. Предупреждение датчика ударов
            string sh_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение датчика ударов", "digital", "Вкл/Выкл", "sh_warn", 12, "1", 0, "");

            //14. Сработка доп. датчика 1
            string ext1_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка доп. датчика 1", "digital", "Вкл/Выкл", "ext1_trig", 13, "1", 0, "");

            //15. Предупреждение доп. датчика 1
            string ext1_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение доп. датчика 1", "digital", "Вкл/Выкл", "ext1_warn", 14, "1", 0, "");

            //16. Сработка доп. датчика 2
            string ext2_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка доп. датчика 2", "digital", "Вкл/Выкл", "ext2_trig", 15, "1", 0, "");

            //17. Предупреждение доп. датчика 2
            string ext2_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение доп. датчика 2", "digital", "Вкл/Выкл", "ext2_warn", 16, "1", 0, "");

            //18. РАКБ
            string acc_voltage_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "РАКБ", "voltage", "В", "acc_voltage", 17, "1", 0, "");

            //19. Зажигание
            string ign_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Зажигание", "engine operation", "Вкл/Выкл", "ign", 18, "1", 0, "");

            //20. Центральный замок
            string central_lock_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Центральный замок", "digital", "Открыто/Закрыто", "central_lock", 19, "1", 0, "");

            //21. Двигатель заведен
            string engine_is_on_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Двигатель заведен", "digital", "Вкл/Выкл", "engine_is_on", 20, "1", 0, "");

            //22. Капот
            string hood_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Капот", "digital", "Откр/Закр", "hood", 22, "1", 0, "");

            //23. Передняя левая дверь (водителя)
            string driver_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Передняя левая дверь (водителя)", "digital", "Откр/Закр", "driver_door", 23, "1", 0, "");

            //24. Передняя правая дверь (пасс.)
            string pass_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Передняя правая дверь (пасс.)", "digital", "Откр/Закр", "pass_door", 24, "1", 0, "");

            //25. Задняя левая дверь
            string l_rear_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Задняя левая дверь", "digital", "Откр/Закр", "l_rear_door", 25, "1", 0, "");

            //26. Правая задняя дверь
            string r_rear_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Правая задняя дверь", "digital", "Откр/Закр", "r_rear_door", 26, "1", 0, "");

            //27. Багажник
            string trunk_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Багажник", "digital", "Откр/Закр", "trunk", 27, "1", 0, "");

            //28. Пробег
            string can_odo_km_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Пробег", "mileage", "КМ", "can_odo_km", 28, "1", 0, "");

            //29. Топливо
            string fuel_lev_l_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Топливо", "fuel level", "л", "fuel_lev_l", 29, "1", 0, "");

            //30. Ручной тормоз
            string hand_break_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Ручной тормоз", "digital", "Вкл/Выкл", "hand_break", 30, "1", 0, "");

            //31. Габаритные огни
            string marker_lights_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Габаритные огни", "digital", "Вкл/Выкл", "marker_lights", 31, "1", 0, "");

            //32. АКПП в P
            string parking_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "АКПП в P", "digital", "Вкл/Выкл", "parking", 32, "1", 0, "");

            //33. Ближний свет
            string dipped_beam_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Ближний свет", "digital", "Вкл/Выкл", "dipped_beam", 33, "1", 0, "");

            //34. Низкий уровень омывающей жидкости
            string washer_alert_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Низкий уровень омывающей жидкости", "digital", "Вкл/Выкл", "washer_alert", 34, "1", 0, "");

            //35. Температура двигателя
            string eng_temp_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Температура двигателя", "temperature", "C", "eng_temp", 35, "1", 0, "");

            //36. Сработка датчика глушения
            string aux_zone_1_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика глушения", "digital", "Вкл/Выкл", "aux_zone_1", 36, "1", 0, "");

            /////////////////
            ///Создаем произвольные поля
            /////////////////

            //Произвольное поле 1
            string answer = macros.create_custom_field_wl(cr_obj_out.item.id, "0 УВАГА", "");

            //Произвольное поле 2
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "02 Проект", "");

            //Произвольное поле 3
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.1 І Відповідальна особа (основна)", "");

            //Произвольное поле 4
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.2 ІІ Відповідальна особа", "");

            //Произвольное поле 5
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.3 ІІІ Відповідальна особа", "");

            //Произвольное поле 6
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.4 ІV Відповідальна особа", "");

            //Произвольное поле 7
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.5 V Відповідальна особа", "");

            //Произвольное поле 8
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.1.1 Оператор, що тестував", "");

            //Произвольное поле 9
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.11 Додатково встановлені сигналізації", "");

            //Произвольное поле 10
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.12 Постановка авто под охрану через багажник?", "");

            //Произвольное поле 11
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.15 Додатково встановлені датчики", "");

            //Произвольное поле 12
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.2.1 Установник: назва, адреса", "");

            //Произвольное поле 13
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.2.2 Установник-монтажник: ПІБ, №тел.", "");

            //Произвольное поле 14
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.3 Дата установки", "");

            //Произвольное поле 15
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.4 Місце установки пристрою ВЕНБЕСТ", "");

            //Произвольное поле 16
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.1 CAN-реле", "");

            //Произвольное поле 17
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.2 Звичайне реле", "");

            //Произвольное поле 18
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.7 Місце встановлення сервісної кнопки", "");

            //Произвольное поле 19
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.8.1 Дротова тривожна кнопка", "");

            //Произвольное поле 20
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.8.2 Бездротова тривожна кнопка", "");

            //Произвольное поле 21
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.3 Блокує Prizrak по CAN", "");

            //Произвольное поле 22
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.9.2 Штатні кнопки введення PIN-коду", "");

            //Административное поле 1
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.3 GSM-код", maskedTextBox_GSM_CODE.Text.ToString());

            //Административное поле 2
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.4 PUK-код", maskedTextBox_PUK.Text.ToString());

            //Административное поле 3
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.5 Bluetuth-код", maskedTextBox_BLE_CODE.Text.ToString());

            //Произвольное поле 24
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.1 Дата активації", "");

            //Произвольное поле 25
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.1.1 Оператор, що активував", "");

            //Произвольное поле 26
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.2 Дата встановлення PIN-коду", "");

            //Произвольное поле 27
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.3 PIN-код встановлено особою(клієнт/установлник)", "");

            //Произвольное поле 28
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.4 Обліковий запис WL", "");

            //Произвольное поле 29
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.1 Менеджер", "");

            //Произвольное поле 30
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.2 Договір обслуговування", "");

            //Произвольное поле 31
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.3 Гарантія до", "");

            //Произвольное поле 32
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.4 Дата закінчення договору страхування", "");

            //Произвольное поле 33
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "8.1 Паркінг 1", "");

            //Произвольное поле 34
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "8.2 Паркінг 2", "");

            //Произвольное поле 35
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.0 Примітки", "");

            //Произвольное поле 36
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.1 Техпаспорт", "");

            //Произвольное поле 37
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.2.1 Дата перевірки картки", "ДД.ММ.РРРР");

            //Произвольное поле 38
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.2.2 Оператор перевірки картки", "Прізвище");

            //Произвольное поле 39
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "10 Кодове слово", "");

            //Админитстративное поле 40
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "13 Prizrak 910 SN", search_tovar_comboBox.Text);

            /////////////////
            ///Добавляем в группу Объекты All
            /////////////////

            string get_units_on_group = macros.WialonRequest(
                "&svc=core/search_item&params={"
                + "\"id\":\"2612\","
                + "\"flags\":\"1\"}");//получаем все объекты группы

            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

            list_get_units_on_group.item.u.Add(cr_obj_out.item.id);//Доповляем в список новый объект
            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

            string gr_answer = macros.WialonRequest(
                "&svc=unit_group/update_units&params={"
                + "\"itemId\":\"2612\","
                + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

            /////////////////
            ///Создаем команды
            /////////////////
            ///http://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/unit/update_command_definition
            /// 83886080- закрыта команда  для клиента
            /// 16777216- открыта команда для клиента

            //0 - Запросить текущее состояние
            string cmd_refresh = macros.create_commads_wl(cr_obj_out.item.id, "0 - Запросить текущее состояние", "%23refresh%23", 83886080);

            //0 - Перезагрузить систему
            string cmd_reboot = macros.create_commads_wl(cr_obj_out.item.id, "0 - Перезагрузить систему", "%23reboot%23", 83886080);

            //1 - Закрыть автомобиль
            string cmd_protection_on = macros.create_commads_wl(cr_obj_out.item.id, "1 - Закрыть автомобиль", "%23protection=1%23", 16777216);

            //1 - Открыть автомобиль
            string cmd_protection_off = macros.create_commads_wl(cr_obj_out.item.id, "1 - Открыть автомобиль", "%23protection=0%23", 16777216);

            //2 - Автозапуск старт
            string cmd_autostart_on = macros.create_commads_wl(cr_obj_out.item.id, "2 - Автозапуск старт", "%23autostart=1%23", 83886080);

            //2 - Автозапуск стоп
            string cmd_autostart_off = macros.create_commads_wl(cr_obj_out.item.id, "2 - Автозапуск стоп", "%23autostart=0%23", 83886080);

            //3 - СТАРТ двигатель
            string cmd_man_block_off = macros.create_commads_wl(cr_obj_out.item.id, "3 - СТАРТ двигатель", "%23man_block=0%23", 83886080);

            //3 - СТОП двигатель
            string cmd_man_block_on = macros.create_commads_wl(cr_obj_out.item.id, "3 - СТОП двигатель", "%23man_block=1%23", 83886080);

            //4 - Включить сирену
            string cmd_alarm_on = macros.create_commads_wl(cr_obj_out.item.id, "4 - Включить сирену", "%23alarm=1%23", 16777216);

            //4 - Выключить сирену
            string cmd_alarm_off = macros.create_commads_wl(cr_obj_out.item.id, "4 - Выключить сирену", "%23alarm=0%23", 16777216);

            //5 - Включить поиск на парковке
            string cmd_search_on = macros.create_commads_wl(cr_obj_out.item.id, "5 - Включить поиск на парковке", "%23search=1%23", 16777216);

            //5 - Выключить поиск на парковке
            string cmd_search_off = macros.create_commads_wl(cr_obj_out.item.id, "5 - Выключить поиск на парковке", "%23search=0%23", 16777216);

            //6 - Включить сервисный режим
            string cmd_valet_on = macros.create_commads_wl(cr_obj_out.item.id, "6 - Включить сервисный режим", "%23valet=1%23", 16777216);

            //6 - Выключить сервисный режиме
            string cmd_valet_off = macros.create_commads_wl(cr_obj_out.item.id, "6 - Выключить сервисный режиме", "%23valet=0%23", 16777216);

            string id_sim = "";

            DataTable t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel_select.Text + "';");
            if (t.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(4) + "';");
            }


            string sql2 = string.Format("insert into btk.Object(Object_id_wl, Object_imei, Object_name, Object_sim_no, products_idproducts, Simcard_idSimcard, TS_info_idTS_info, TS_info_TS_brend_model_idTS_brend_model, Kontakti_idKontakti_serviceman, Dogovora_idDogovora, Objectcol_edit_date, Users_idUsers, Objectcol_sn_puk_card, Objectcol_puk, Objectcol_gsm_code, Objectcol_ble_code, Objectcol_bt_enable, Objectcol_sn_prizrak) "
                                        + "values('" + cr_obj_out.item.id
                                        + "', '" + textBox_id_to_create.Text.ToString()
                                        + "', '" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem)
                                        + " " + textBox_id_to_create.Text.ToString() + "','"
                                        + maskedTextBox_sim_no_to_create.Text.ToString()
                                        + "', '" + comboBox_list_poructs.SelectedValue.ToString() + "', " +
                                        "'" + id_sim + "'," +
                                        " '1', '1', '1','1', null, '" + vars_form.user_login_id + "', '"
                                        + textBox_id_to_create.Text.ToString() + "', '"
                                        + maskedTextBox_PUK.Text.ToString() + "', '"
                                        + maskedTextBox_GSM_CODE.Text.ToString() + "', '"
                                        + maskedTextBox_BLE_CODE.Text.ToString() + "', '"
                                        + textBox_bt_enable.Text.ToString() + "', '"
                                        + search_tovar_comboBox.GetItemText(search_tovar_comboBox.SelectedItem) + "');");
            macros.sql_command(sql2);

            //получаем айди созданного объекта
            string id_object = macros.sql_command("SELECT MAX(idObject) FROM btk.Object;");

            // set datetime create
            macros.sql_command("update btk.Object set date_cteate = now() where idobject = '" + id_object + "';");

            //Создаем подписку для объекта
            string sql3 = string.Format("INSERT INTO btk.Subscription(products_has_Tarif_idproducts_has_Tarif, idobject) values('11'," + id_object + ");");
            macros.sql_command(sql3);

            //Получаем айди созданной подписки
            string sql4 = macros.sql_command("select max(idSubscr) from btk.Subscription;");

            //привязываем айди созданного объекта с айди созданной подписки
            string sql5 = string.Format("insert into btk.object_subscr (Object_idObject, Subscription_idSubscr) values (" + id_object + "," + sql4 + ");");
            macros.sql_command(sql5);

            ///////////////////////
            //Если все прошло успешно - завечиваем зеленым кнопку и затирает текстбоксы
            //////////////////////////
            button_create_object.Text = "Створено: " + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text + "\n Дата та час: " + DateTime.Now.ToString();
            maskedTextBox_BLE_CODE.Text = "";
            maskedTextBox_GSM_CODE.Text = "";
            textBox_id_to_create.Text = "";
            maskedTextBox_PUK.Text = "";
            maskedTextBox_sim_no_to_create.Text = "";
            textBox_bt_enable.Text = "";
            comboBox_tel_select.Text = "";
            search_tovar_comboBox.Text = "";
            button_create_object.BackColor = Color.Green;
        }

        private void CNTK_910BT()
        {
            /////////////////////
            //////Проверяем корректность введенных данных
            ///

            if (textBox_id_to_create.Text.Length <= 3)
            {
                textBox_id_to_create.BackColor = Color.Red;//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
                return;
            }

            if (maskedTextBox_sim_no_to_create.Text.Length <= 11)//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Red;
                return;
            }
            if (maskedTextBox_PUK.Text.Length <= 3)//Если PUK короче 4х символов останавливается и подсвкечиваем желтым
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Yellow;
                DialogResult result = MessageBox.Show(
                    "Вопрос",
                    "PUK Верный?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.No)
                    return;
                maskedTextBox_PUK.BackColor = Color.White;
            }
            // Проверям существует ли данный номер в системе
            string unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли указанный ИМЕЙ в системе
            string unswer2 = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_unique_id\"," +
                                                     "\"propValueMask\":\"" + textBox_id_to_create.Text + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m2 = JsonConvert.DeserializeObject<RootObject>(unswer2);
            if (m2.items.Count > 0)
            {
                MessageBox.Show("Указанный IMEI существует в WL");
                return;
            }

            /////////////////
            ///Создаем объект
            /////////////////
            string cr_obj_in = "&svc=core/create_unit&params={\"creatorId\":\"" + vars_form.wl_user_id + "\",\"name\":\"" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "\",\"hwTypeId\":\"9\",\"dataFlags\":\"1\"}";
            string json = macros.WialonRequest(cr_obj_in);
            var cr_obj_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (cr_obj_out.error != 0)
            {
                string text = macros.Get_wl_text_error(cr_obj_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Создаем пароль доступа к объекту
            /////////////////

            string accsess_pass_answer = macros.WialonRequest(
                "&svc=unit/update_access_password&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"accessPassword\":\"" + maskedTextBox_GSM_CODE.Text.ToString() + "\"}");
            var accsess_pass_answer_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (accsess_pass_answer_out.error != 0)
            {
                string text = macros.Get_wl_text_error(accsess_pass_answer_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Установливаем имя объекта и ID оборудования и
            /////////////////
            string item_id_in =
                "&svc=unit/update_device_type&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id
                + "\",\"deviceTypeId\":\"" + "9"
                + "\",\"uniqueId\":\"" + textBox_id_to_create.Text.ToString() + "\"}";
            string json2 = macros.WialonRequest(item_id_in);
            var item_id_out = JsonConvert.DeserializeObject<RootObject>(json2);
            if (item_id_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_id_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Устанавливаем номер СИМ
            /////////////////
            string item_phone_in = "&svc=unit/update_phone&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim_no_to_create.Text) + "\"}";
            string json3 = macros.WialonRequest(item_phone_in);
            var item_phone_out = JsonConvert.DeserializeObject<RootObject>(json3);
            if (item_phone_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Создаем датчики
            /////////////////

            //1. Автоматическая блокировка двигателя
            string auto_block_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Автоматическая блокировка двигателя", "digital", "Вкл/Выкл", "auto_block", 1, "1", 0, "");

            //2. Охрана
            string protection_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Охрана", "digital", "Вкл/Выкл", "protection", 2, "1", 0, "");

            //3. Принудительная блокировка двигателя
            string man_block_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Принудительная блокировка двигателя", "digital", "Вкл/Выкл", "man_block", 3, "1", 0, "");

            //4. Авторизация
            string auth_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Авторизация", "digital", "Вкл/Выкл", "auth", 4, "1", 0, "");

            //5. Тревожная кнопка
            string alarm_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Тревожная кнопка_", "digital", "Вкл/Выкл", "alarm", 5, "1", 0, "");

            //6. Сервисный режим
            string valet_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сервисный режим", "digital", "Вкл/Выкл", "valet", 6, "1", 0, "");

            //7. Двери статус
            string all_door_status_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Двери статус", "digital", "Вкл/Выкл", "trunk%2Bhood%2Bpass_door%2Bdriver_door%2Bl_rear_door%2Br_rear_door", 21, "1", 0, "");

            //8. Сработка открытие дверей в охране
            string Door_in_protection_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка открытие дверей в охране", "digital", "Вкл/Выкл", "alarm_act", 7, "1", 7, "");

            //9. Сработка сирены
            string alarm_act_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка сирены", "digital", "Вкл/Выкл", "alarm_act", 8, "1", 0, "");

            //10. Сработка датчика наклона
            string tilt_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика наклона", "digital", "Вкл/Выкл", "tilt_trig", 9, "1", 0, "");

            //11. Напряжение АКБ
            string ext_voltage_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Напряжение АКБ", "voltage", "В", "ext_voltage", 10, "1", 0, "");

            //12. Сработка датчика ударов
            string sh_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика ударов", "digital", "Вкл/Выкл", "sh_trig", 11, "1", 0, "");

            //13. Предупреждение датчика ударов
            string sh_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение датчика ударов", "digital", "Вкл/Выкл", "sh_warn", 12, "1", 0, "");

            //14. Сработка доп. датчика 1
            string ext1_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка доп. датчика 1", "digital", "Вкл/Выкл", "ext1_trig", 13, "1", 0, "");

            //15. Предупреждение доп. датчика 1
            string ext1_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение доп. датчика 1", "digital", "Вкл/Выкл", "ext1_warn", 14, "1", 0, "");

            //16. Сработка доп. датчика 2
            string ext2_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка доп. датчика 2", "digital", "Вкл/Выкл", "ext2_trig", 15, "1", 0, "");

            //17. Предупреждение доп. датчика 2
            string ext2_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение доп. датчика 2", "digital", "Вкл/Выкл", "ext2_warn", 16, "1", 0, "");

            //18. РАКБ
            string acc_voltage_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "РАКБ", "voltage", "В", "acc_voltage", 17, "1", 0, "");

            //19. Зажигание
            string ign_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Зажигание", "engine operation", "Вкл/Выкл", "ign", 18, "1", 0, "");

            //20. Центральный замок
            string central_lock_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Центральный замок", "digital", "Открыто/Закрыто", "central_lock", 19, "1", 0, "");

            //21. Двигатель заведен
            string engine_is_on_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Двигатель заведен", "digital", "Вкл/Выкл", "engine_is_on", 20, "1", 0, "");

            //22. Капот
            string hood_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Капот", "digital", "Откр/Закр", "hood", 22, "1", 0, "");

            //23. Передняя левая дверь (водителя)
            string driver_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Передняя левая дверь (водителя)", "digital", "Откр/Закр", "driver_door", 23, "1", 0, "");

            //24. Передняя правая дверь (пасс.)
            string pass_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Передняя правая дверь (пасс.)", "digital", "Откр/Закр", "pass_door", 24, "1", 0, "");

            //25. Задняя левая дверь
            string l_rear_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Задняя левая дверь", "digital", "Откр/Закр", "l_rear_door", 25, "1", 0, "");

            //26. Правая задняя дверь
            string r_rear_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Правая задняя дверь", "digital", "Откр/Закр", "r_rear_door", 26, "1", 0, "");

            //27. Багажник
            string trunk_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Багажник", "digital", "Откр/Закр", "trunk", 27, "1", 0, "");

            //28. Пробег
            string can_odo_km_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Пробег", "mileage", "КМ", "can_odo_km", 28, "1", 0, "");

            //29. Топливо
            string fuel_lev_l_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Топливо", "fuel level", "л", "fuel_lev_l", 29, "1", 0, "");

            //30. Ручной тормоз
            string hand_break_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Ручной тормоз", "digital", "Вкл/Выкл", "hand_break", 30, "1", 0, "");

            //31. Габаритные огни
            string marker_lights_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Габаритные огни", "digital", "Вкл/Выкл", "marker_lights", 31, "1", 0, "");

            //32. АКПП в P
            string parking_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "АКПП в P", "digital", "Вкл/Выкл", "parking", 32, "1", 0, "");

            //33. Ближний свет
            string dipped_beam_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Ближний свет", "digital", "Вкл/Выкл", "dipped_beam", 33, "1", 0, "");

            //34. Низкий уровень омывающей жидкости
            string washer_alert_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Низкий уровень омывающей жидкости", "digital", "Вкл/Выкл", "washer_alert", 34, "1", 0, "");

            //35. Температура двигателя
            string eng_temp_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Температура двигателя", "temperature", "C", "eng_temp", 35, "1", 0, "");

            //36. Сработка датчика глушения
            string aux_zone_1_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика глушения", "digital", "Вкл/Выкл", "aux_zone_1", 36, "1", 0, "");

            /////////////////
            ///Создаем произвольные поля
            /////////////////

            //Произвольное поле 1
            string answer = macros.create_custom_field_wl(cr_obj_out.item.id, "0 УВАГА", "");

            //Произвольное поле 2
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "02 Проект", "");

            //Произвольное поле 3
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.1 І Відповідальна особа (основна)", "");

            //Произвольное поле 4
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.2 ІІ Відповідальна особа", "");

            //Произвольное поле 5
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.3 ІІІ Відповідальна особа", "");

            //Произвольное поле 6
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.4 ІV Відповідальна особа", "");

            //Произвольное поле 7
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.5 V Відповідальна особа", "");

            //Произвольное поле 8
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.1.1 Оператор, що тестував", "");

            //Произвольное поле 9
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.11 Додатково встановлені сигналізації", "");

            //Произвольное поле 10
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.12 Постановка авто под охрану через багажник?", "");

            //Произвольное поле 11
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.15 Додатково встановлені датчики", "");

            //Произвольное поле 12
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.2.1 Установник: назва, адреса", "");

            //Произвольное поле 13
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.2.2 Установник-монтажник: ПІБ, №тел.", "");

            //Произвольное поле 14
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.3 Дата установки", "");

            //Произвольное поле 15
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.4 Місце установки пристрою ВЕНБЕСТ", "");

            //Произвольное поле 16
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.1 CAN-реле", "");

            //Произвольное поле 17
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.2 Звичайне реле", "");

            //Произвольное поле 18
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.7 Місце встановлення сервісної кнопки", "");

            //Произвольное поле 19
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.8.1 Дротова тривожна кнопка", "");

            //Произвольное поле 20
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.8.2 Бездротова тривожна кнопка", "");

            //Произвольное поле 21
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.3 Блокує Prizrak по CAN", "");

            //Произвольное поле 22
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.9.2 Штатні кнопки введення PIN-коду", "");

            //Административное поле 1
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.3 GSM-код", maskedTextBox_GSM_CODE.Text.ToString());

            //Административное поле 2
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.4 PUK-код", maskedTextBox_PUK.Text.ToString());

            //Административное поле 3
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.5 Bluetuth-код", maskedTextBox_BLE_CODE.Text.ToString());

            //Произвольное поле 21
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.1 Дата активації", "");

            //Произвольное поле 22
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.1.1 Оператор, що активував", "");

            //Произвольное поле 23
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.2 Дата встановлення PIN-коду", "");

            //Произвольное поле 24
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.3 PIN-код встановлено особою(клієнт/установлник)", "");

            //Произвольное поле 25
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.4 Обліковий запис WL", "");

            //Произвольное поле 26
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.1 Менеджер", "");

            //Произвольное поле 27
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.2 Договір обслуговування", "");

            //Произвольное поле 28
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.3 Гарантія до", "");

            //Произвольное поле 29
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.4 Дата закінчення договору страхування", "");

            //Произвольное поле 30
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "8.1 Паркінг 1", "");

            //Произвольное поле 31
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "8.2 Паркінг 2", "");

            //Произвольное поле 33
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.0 Примітки", "");

            //Произвольное поле 33
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.1 Техпаспорт", "");

            //Произвольное поле 34
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.2.1 Дата перевірки картки", "ДД.ММ.РРРР");

            //Произвольное поле 35
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.2.2 Оператор перевірки картки", "Прізвище");

            //Произвольное поле 36
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "10 Кодове слово", "");

            //Админитстративное поле 40
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "13 Prizrak 910 SN", search_tovar_comboBox.Text);

            /////////////////
            ///Добавляем в группу Объекты All
            /////////////////

            string get_units_on_group = macros.WialonRequest(
                "&svc=core/search_item&params={"
                + "\"id\":\"2612\","
                + "\"flags\":\"1\"}");//получаем все объекты группы

            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

            list_get_units_on_group.item.u.Add(cr_obj_out.item.id);//Доповляем в список новый объект
            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

            string gr_answer = macros.WialonRequest(
                "&svc=unit_group/update_units&params={"
                + "\"itemId\":\"2612\","
                + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

            /////////////////
            ///Создаем команды
            /////////////////
            ///http://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/unit/update_command_definition
            /// 83886080- закрыта команда  для клиента
            /// 16777216- открыта команда для клиента

            //0 - Запросить текущее состояние
            string cmd_refresh = macros.create_commads_wl(cr_obj_out.item.id, "0 - Запросить текущее состояние", "%23refresh%23", 83886080);

            //0 - Перезагрузить систему
            string cmd_reboot = macros.create_commads_wl(cr_obj_out.item.id, "0 - Перезагрузить систему", "%23reboot%23", 83886080);

            //1 - Закрыть автомобиль
            string cmd_protection_on = macros.create_commads_wl(cr_obj_out.item.id, "1 - Закрыть автомобиль", "%23protection=1%23", 83886080);

            //1 - Открыть автомобиль
            string cmd_protection_off = macros.create_commads_wl(cr_obj_out.item.id, "1 - Открыть автомобиль", "%23protection=0%23", 83886080);

            //2 - Автозапуск старт
            string cmd_autostart_on = macros.create_commads_wl(cr_obj_out.item.id, "2 - Автозапуск старт", "%23autostart=1%23", 83886080);

            //2 - Автозапуск стоп
            string cmd_autostart_off = macros.create_commads_wl(cr_obj_out.item.id, "2 - Автозапуск стоп", "%23autostart=0%23", 83886080);

            //3 - СТАРТ двигатель
            string cmd_man_block_off = macros.create_commads_wl(cr_obj_out.item.id, "3 - СТАРТ двигатель", "%23man_block=0%23", 83886080);

            //3 - СТОП двигатель
            string cmd_man_block_on = macros.create_commads_wl(cr_obj_out.item.id, "3 - СТОП двигатель", "%23man_block=1%23", 83886080);

            //4 - Включить сирену
            string cmd_alarm_on = macros.create_commads_wl(cr_obj_out.item.id, "4 - Включить сирену", "%23alarm=1%23", 83886080);

            //4 - Выключить сирену
            string cmd_alarm_off = macros.create_commads_wl(cr_obj_out.item.id, "4 - Выключить сирену", "%23alarm=0%23", 83886080);

            //5 - Включить поиск на парковке
            string cmd_search_on = macros.create_commads_wl(cr_obj_out.item.id, "5 - Включить поиск на парковке", "%23search=1%23", 83886080);

            //5 - Выключить поиск на парковке
            string cmd_search_off = macros.create_commads_wl(cr_obj_out.item.id, "5 - Выключить поиск на парковке", "%23search=0%23", 83886080);

            //6 - Включить сервисный режим
            string cmd_valet_on = macros.create_commads_wl(cr_obj_out.item.id, "6 - Включить сервисный режим", "%23valet=1%23", 16777216);

            //6 - Выключить сервисный режиме
            string cmd_valet_off = macros.create_commads_wl(cr_obj_out.item.id, "6 - Выключить сервисный режиме", "%23valet=0%23", 16777216);

            string id_sim = "";

            DataTable t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel_select.Text + "';");
            if (t.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(4) + "';");
            }

            string sql2 = string.Format("insert into btk.Object(Object_id_wl, Object_imei, Object_name, Object_sim_no, products_idproducts, Simcard_idSimcard, TS_info_idTS_info, TS_info_TS_brend_model_idTS_brend_model, Kontakti_idKontakti_serviceman, Dogovora_idDogovora, Objectcol_edit_date, Users_idUsers, Objectcol_sn_puk_card, Objectcol_puk, Objectcol_gsm_code, Objectcol_ble_code, Objectcol_bt_enable, Objectcol_sn_prizrak) "
                + "values('" + cr_obj_out.item.id
                + "', '" + textBox_id_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem)
                + " " + textBox_id_to_create.Text.ToString() + "','"
                + maskedTextBox_sim_no_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.SelectedValue.ToString() + "'," +
                " '" + id_sim + "'," +
                " '1', '1', '1','1', null, '" + vars_form.user_login_id + "', '"
                + textBox_id_to_create.Text.ToString() + "', '"
                + maskedTextBox_PUK.Text.ToString() + "', '"
                + maskedTextBox_GSM_CODE.Text.ToString() + "', '"
                + maskedTextBox_BLE_CODE.Text.ToString() + "', '"
                + textBox_bt_enable.Text.ToString() + "', '"
                + search_tovar_comboBox.GetItemText(search_tovar_comboBox.SelectedItem) + "');");
            macros.sql_command(sql2);

            //получаем айди созданного объекта
            string id_object = macros.sql_command("SELECT MAX(idObject) FROM btk.Object;");

            // set datetime create
            macros.sql_command("update btk.Object set date_cteate = now() where idobject = '" + id_object + "';");

            //Создаем подписку для объекта
            string sql3 = string.Format("INSERT INTO btk.Subscription(products_has_Tarif_idproducts_has_Tarif, idobject) values('40'," + id_object + ");");
            macros.sql_command(sql3);

            //Получаем айди созданной подписки
            string sql4 = macros.sql_command("select max(idSubscr) from btk.Subscription;");

            //привязываем айди созданного объекта с айди созданной подписки
            string sql5 = string.Format("insert into btk.object_subscr (Object_idObject, Subscription_idSubscr) values (" + id_object + "," + sql4 + ");");
            macros.sql_command(sql5);

            ///////////////////////
            //Если все прошло успешно - завечиваем зеленым кнопку и затирает текстбоксы
            //////////////////////////

            button_create_object.Text = "Створено: " + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text + "\n Дата та час: " + DateTime.Now.ToString();
            maskedTextBox_BLE_CODE.Text = "";
            maskedTextBox_GSM_CODE.Text = "";
            textBox_id_to_create.Text = "";
            maskedTextBox_PUK.Text = "";
            maskedTextBox_sim_no_to_create.Text = "";
            textBox_bt_enable.Text = "";
            button_create_object.BackColor = Color.Green;
            comboBox_tel_select.Text = "";
            search_tovar_comboBox.Text = "";

        }

        private void CNTP_910BT()
        {
            /////////////////////
            //////Проверяем корректность введенных данных
            ///

            if (textBox_id_to_create.Text.Length <= 3)
            {
                textBox_id_to_create.BackColor = Color.Red;//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
                return;
            }
            if (maskedTextBox_sim_no_to_create.Text.Length <= 11)//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Red;
                return;
            }
            if (maskedTextBox_PUK.Text.Length <= 3)//Если PUK короче 4х символов останавливается и подсвкечиваем желтым
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Yellow;
                DialogResult result = MessageBox.Show(
                    "Вопрос",
                    "PUK Верный?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.No)
                    return;
                maskedTextBox_PUK.BackColor = Color.White;
            }

            // Проверям существует ли данный номер в системе
            string unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли указанный ИМЕЙ в системе
            string unswer2 = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_unique_id\"," +
                                                     "\"propValueMask\":\"" + textBox_id_to_create.Text + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m2 = JsonConvert.DeserializeObject<RootObject>(unswer2);
            if (m2.items.Count > 0)
            {
                MessageBox.Show("Указанный IMEI существует в WL");
                return;
            }

            /////////////////
            ///Создаем объект
            /////////////////
            string cr_obj_in = "&svc=core/create_unit&params={\"creatorId\":\"" + vars_form.wl_user_id + "\",\"name\":\"" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "\",\"hwTypeId\":\"9\",\"dataFlags\":\"1\"}";
            string json = macros.WialonRequest(cr_obj_in);
            var cr_obj_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (cr_obj_out.error != 0)
            {
                string text = macros.Get_wl_text_error(cr_obj_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Создаем пароль доступа к объекту
            /////////////////

            string accsess_pass_answer = macros.WialonRequest(
                "&svc=unit/update_access_password&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"accessPassword\":\"" + maskedTextBox_GSM_CODE.Text.ToString() + "\"}");
            var accsess_pass_answer_out = JsonConvert.DeserializeObject<RootObject>(accsess_pass_answer);
            if (accsess_pass_answer_out.error != 0)
            {
                string text = macros.Get_wl_text_error(accsess_pass_answer_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Установливаем имя объекта и ID оборудования и
            /////////////////
            string item_id_in =
                "&svc=unit/update_device_type&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id
                + "\",\"deviceTypeId\":\"" + "9"
                + "\",\"uniqueId\":\"" + textBox_id_to_create.Text.ToString() + "\"}";
            string json2 = macros.WialonRequest(item_id_in);
            var item_id_out = JsonConvert.DeserializeObject<RootObject>(json2);
            if (item_id_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_id_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Устанавливаем номер СИМ
            /////////////////
            string item_phone_in = "&svc=unit/update_phone&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim_no_to_create.Text) + "\"}";
            string json3 = macros.WialonRequest(item_phone_in);
            var item_phone_out = JsonConvert.DeserializeObject<RootObject>(json3);
            if (item_phone_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Создаем датчики
            /////////////////

            //1. Автоматическая блокировка двигателя
            string auto_block_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Автоматическая блокировка двигателя", "digital", "Вкл/Выкл", "auto_block", 1, "1", 0, "");

            //2. Охрана
            string protection_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Охрана", "digital", "Вкл/Выкл", "protection", 2, "1", 0, "");

            //3. Принудительная блокировка двигателя
            string man_block_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Принудительная блокировка двигателя", "digital", "Вкл/Выкл", "man_block", 3, "1", 0, "");

            //4. Авторизация
            string auth_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Авторизация", "digital", "Вкл/Выкл", "auth", 4, "1", 0, "");

            //5. Тревожная кнопка
            string alarm_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Тревожная кнопка_", "digital", "Вкл/Выкл", "alarm", 5, "1", 0, "");

            //6. Сервисный режим
            string valet_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сервисный режим", "digital", "Вкл/Выкл", "valet", 6, "1", 0, "");

            //7. Двери статус
            string all_door_status_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Двери статус", "digital", "Вкл/Выкл", "trunk%2Bhood+%2Bpass_door%2Bdriver_door%2Bl_rear_door%2Br_rear_door", 21, "1", 0, "");

            //8. Сработка открытие дверей в охране
            string Door_in_protection_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка открытие дверей в охране", "digital", "Вкл/Выкл", "alarm_act", 7, "1", 7, "");

            //9. Сработка сирены
            string alarm_act_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка сирены", "digital", "Вкл/Выкл", "alarm_act", 8, "1", 0, "");

            //10. Сработка датчика наклона
            string tilt_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика наклона", "digital", "Вкл/Выкл", "tilt_trig", 9, "1", 0, "");

            //11. Напряжение АКБ
            string ext_voltage_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Напряжение АКБ", "voltage", "В", "ext_voltage", 10, "1", 0, "");

            //12. Сработка датчика ударов
            string sh_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика ударов", "digital", "Вкл/Выкл", "sh_trig", 11, "1", 0, "");

            //13. Предупреждение датчика ударов
            string sh_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение датчика ударов", "digital", "Вкл/Выкл", "sh_warn", 12, "1", 0, "");

            //14. Сработка доп. датчика 1
            string ext1_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка доп. датчика 1", "digital", "Вкл/Выкл", "ext1_trig", 13, "1", 0, "");

            //15. Предупреждение доп. датчика 1
            string ext1_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение доп. датчика 1", "digital", "Вкл/Выкл", "ext1_warn", 14, "1", 0, "");

            //16. Сработка доп. датчика 2
            string ext2_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка доп. датчика 2", "digital", "Вкл/Выкл", "ext2_trig", 15, "1", 0, "");

            //17. Предупреждение доп. датчика 2
            string ext2_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение доп. датчика 2", "digital", "Вкл/Выкл", "ext2_warn", 16, "1", 0, "");

            //18. РАКБ
            string acc_voltage_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "РАКБ", "voltage", "В", "acc_voltage", 17, "1", 0, "");

            //19. Зажигание
            string ign_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Зажигание", "engine operation", "Вкл/Выкл", "ign", 18, "1", 0, "");

            //20. Центральный замок
            string central_lock_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Центральный замок", "digital", "Открыто/Закрыто", "central_lock", 19, "1", 0, "");

            //21. Двигатель заведен
            string engine_is_on_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Двигатель заведен", "digital", "Вкл/Выкл", "engine_is_on", 20, "1", 0, "");

            //22. Капот
            string hood_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Капот", "digital", "Откр/Закр", "hood", 22, "1", 0, "");

            //23. Передняя левая дверь (водителя)
            string driver_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Передняя левая дверь (водителя)", "digital", "Откр/Закр", "driver_door", 23, "1", 0, "");

            //24. Передняя правая дверь (пасс.)
            string pass_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Передняя правая дверь (пасс.)", "digital", "Откр/Закр", "pass_door", 24, "1", 0, "");

            //25. Задняя левая дверь
            string l_rear_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Задняя левая дверь", "digital", "Откр/Закр", "l_rear_door", 25, "1", 0, "");

            //26. Правая задняя дверь
            string r_rear_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Правая задняя дверь", "digital", "Откр/Закр", "r_rear_door", 26, "1", 0, "");

            //27. Багажник
            string trunk_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Багажник", "digital", "Откр/Закр", "trunk", 27, "1", 0, "");

            //28. Пробег
            string can_odo_km_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Пробег", "mileage", "КМ", "can_odo_km", 28, "1", 0, "");

            //29. Топливо
            string fuel_lev_l_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Топливо", "fuel level", "л", "fuel_lev_l", 29, "1", 0, "");

            //30. Ручной тормоз
            string hand_break_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Ручной тормоз", "digital", "Вкл/Выкл", "hand_break", 30, "1", 0, "");

            //31. Габаритные огни
            string marker_lights_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Габаритные огни", "digital", "Вкл/Выкл", "marker_lights", 31, "1", 0, "");

            //32. АКПП в P
            string parking_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "АКПП в P", "digital", "Вкл/Выкл", "parking", 32, "1", 0, "");

            //33. Ближний свет
            string dipped_beam_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Ближний свет", "digital", "Вкл/Выкл", "dipped_beam", 33, "1", 0, "");

            //34. Низкий уровень омывающей жидкости
            string washer_alert_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Низкий уровень омывающей жидкости", "digital", "Вкл/Выкл", "washer_alert", 34, "1", 0, "");

            //35. Температура двигателя
            string eng_temp_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Температура двигателя", "temperature", "C", "eng_temp", 35, "1", 0, "");

            //36. Сработка датчика глушения
            string aux_zone_1_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика глушения", "digital", "Вкл/Выкл", "aux_zone_1", 36, "1", 0, "");

            /////////////////
            ///Создаем произвольные поля
            /////////////////

            //Произвольное поле 1
            string answer = macros.create_custom_field_wl(cr_obj_out.item.id, "0 УВАГА", "");

            //Произвольное поле 2
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "02 Проект", "");

            //Произвольное поле 3
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.1 І Відповідальна особа (основна)", "");

            //Произвольное поле 4
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.2 ІІ Відповідальна особа", "");

            //Произвольное поле 5
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.3 ІІІ Відповідальна особа", "");

            //Произвольное поле 6
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.4 ІV Відповідальна особа", "");

            //Произвольное поле 7
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.5 V Відповідальна особа", "");

            //Произвольное поле 8
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.1.1 Оператор, що тестував", "");

            //Произвольное поле 9
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.11 Додатково встановлені сигналізації", "");

            //Произвольное поле 10
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.12 Постановка авто под охрану через багажник?", "");

            //Произвольное поле 11
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.15 Додатково встановлені датчики", "");

            //Произвольное поле 12
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.2.1 Установник: назва, адреса", "");

            //Произвольное поле 13
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.2.2 Установник-монтажник: ПІБ, №тел.", "");

            //Произвольное поле 14
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.3 Дата установки", "");

            //Произвольное поле 15
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.4 Місце установки пристрою ВЕНБЕСТ", "");

            //Произвольное поле 16
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.1 CAN-реле", "");

            //Произвольное поле 17
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.2 Звичайне реле", "");

            //Произвольное поле 18
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.7 Місце встановлення сервісної кнопки", "");

            //Произвольное поле 19
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.8.1 Дротова тривожна кнопка", "");

            //Произвольное поле 20
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.8.2 Бездротова тривожна кнопка", "");

            //Произвольное поле 21
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.3 Блокує Prizrak по CAN", "");

            //Произвольное поле 22
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.9.2 Штатні кнопки введення PIN-коду", "");

            //Административное поле 1
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.3 GSM-код", maskedTextBox_GSM_CODE.Text.ToString());

            //Административное поле 2
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.4 PUK-код", maskedTextBox_PUK.Text.ToString());

            //Административное поле 3
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.5 Bluetuth-код", maskedTextBox_BLE_CODE.Text.ToString());

            //Произвольное поле 24
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.1 Дата активації", "");

            //Произвольное поле 25
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.1.1 Оператор, що активував", "");

            //Произвольное поле 26
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.2 Дата встановлення PIN-коду", "");

            //Произвольное поле 27
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.3 PIN-код встановлено особою(клієнт/установлник)", "");

            //Произвольное поле 28
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.4 Обліковий запис WL", "");

            //Произвольное поле 29
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.1 Менеджер", "");

            //Произвольное поле 30
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.2 Договір обслуговування", "");

            //Произвольное поле 31
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.3 Гарантія до", "");

            //Произвольное поле 32
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.4 Дата закінчення договору страхування", "");

            //Произвольное поле 33
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "8.1 Паркінг 1", "");

            //Произвольное поле 34
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "8.2 Паркінг 2", "");

            //Произвольное поле 35
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.0 Примітки", "");

            //Произвольное поле 36
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.1 Техпаспорт", "");

            //Произвольное поле 37
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.2.1 Дата перевірки картки", "ДД.ММ.РРРР");

            //Произвольное поле 38
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.2.2 Оператор перевірки картки", "Прізвище");

            //Произвольное поле 39
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "10 Кодове слово", "");

            //Админитстративное поле 40
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "13 Prizrak 910 SN", search_tovar_comboBox.Text);

            /////////////////
            ///Добавляем в группу Объекты All
            /////////////////

            string get_units_on_group = macros.WialonRequest(
                "&svc=core/search_item&params={"
                + "\"id\":\"2612\","
                + "\"flags\":\"1\"}");//получаем все объекты группы

            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

            list_get_units_on_group.item.u.Add(cr_obj_out.item.id);//Доповляем в список новый объект
            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

            string gr_answer = macros.WialonRequest(
                "&svc=unit_group/update_units&params={"
                + "\"itemId\":\"2612\","
                + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

            /////////////////
            ///Создаем команды
            /////////////////
            ///http://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/unit/update_command_definition
            /// 83886080- закрыта команда  для клиента
            /// 16777216- открыта команда для клиента

            //0 - Запросить текущее состояние
            string cmd_refresh = macros.create_commads_wl(cr_obj_out.item.id, "0 - Запросить текущее состояние", "%23refresh%23", 83886080);

            //0 - Перезагрузить систему
            string cmd_reboot = macros.create_commads_wl(cr_obj_out.item.id, "0 - Перезагрузить систему", "%23reboot%23", 83886080);

            //1 - Закрыть автомобиль
            string cmd_protection_on = macros.create_commads_wl(cr_obj_out.item.id, "1 - Закрыть автомобиль", "%23protection=1%23", 16777216);

            //1 - Открыть автомобиль
            string cmd_protection_off = macros.create_commads_wl(cr_obj_out.item.id, "1 - Открыть автомобиль", "%23protection=0%23", 16777216);

            //2 - Автозапуск старт
            string cmd_autostart_on = macros.create_commads_wl(cr_obj_out.item.id, "2 - Автозапуск старт", "%23autostart=1%23", 83886080);

            //2 - Автозапуск стоп
            string cmd_autostart_off = macros.create_commads_wl(cr_obj_out.item.id, "2 - Автозапуск стоп", "%23autostart=0%23", 83886080);

            //3 - СТАРТ двигатель
            string cmd_man_block_off = macros.create_commads_wl(cr_obj_out.item.id, "3 - СТАРТ двигатель", "%23man_block=0%23", 83886080);

            //3 - СТОП двигатель
            string cmd_man_block_on = macros.create_commads_wl(cr_obj_out.item.id, "3 - СТОП двигатель", "%23man_block=1%23", 83886080);

            //4 - Включить сирену
            string cmd_alarm_on = macros.create_commads_wl(cr_obj_out.item.id, "4 - Включить сирену", "%23alarm=1%23", 16777216);

            //4 - Выключить сирену
            string cmd_alarm_off = macros.create_commads_wl(cr_obj_out.item.id, "4 - Выключить сирену", "%23alarm=0%23", 16777216);

            //5 - Включить поиск на парковке
            string cmd_search_on = macros.create_commads_wl(cr_obj_out.item.id, "5 - Включить поиск на парковке", "%23search=1%23", 16777216);

            //5 - Выключить поиск на парковке
            string cmd_search_off = macros.create_commads_wl(cr_obj_out.item.id, "5 - Выключить поиск на парковке", "%23search=0%23", 16777216);

            //6 - Включить сервисный режим
            string cmd_valet_on = macros.create_commads_wl(cr_obj_out.item.id, "6 - Включить сервисный режим", "%23valet=1%23", 16777216);

            //6 - Выключить сервисный режиме
            string cmd_valet_off = macros.create_commads_wl(cr_obj_out.item.id, "6 - Выключить сервисный режиме", "%23valet=0%23", 16777216);

            string id_sim = "";

            DataTable t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel_select.Text + "';");
            if (t.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(4) + "';");
            }


            string sql2 = string.Format("insert into btk.Object(Object_id_wl, Object_imei, Object_name, Object_sim_no, products_idproducts, Simcard_idSimcard, TS_info_idTS_info, TS_info_TS_brend_model_idTS_brend_model, Kontakti_idKontakti_serviceman, Dogovora_idDogovora, Objectcol_edit_date, Users_idUsers, Objectcol_sn_puk_card, Objectcol_puk, Objectcol_gsm_code, Objectcol_ble_code, Objectcol_bt_enable, Objectcol_sn_prizrak) "
                                        + "values('" + cr_obj_out.item.id
                                        + "', '" + textBox_id_to_create.Text.ToString()
                                        + "', '" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem)
                                        + " " + textBox_id_to_create.Text.ToString() + "','"
                                        + maskedTextBox_sim_no_to_create.Text.ToString()
                                        + "', '" + comboBox_list_poructs.SelectedValue.ToString() + "', " +
                                        "'" + id_sim + "'," +
                                        " '1', '1', '1','1', null, '" + vars_form.user_login_id + "', '"
                                        + textBox_id_to_create.Text.ToString() + "', '"
                                        + maskedTextBox_PUK.Text.ToString() + "', '"
                                        + maskedTextBox_GSM_CODE.Text.ToString() + "', '"
                                        + maskedTextBox_BLE_CODE.Text.ToString() + "', '"
                                        + textBox_bt_enable.Text.ToString() + "', '"
                                        + search_tovar_comboBox.GetItemText(search_tovar_comboBox.SelectedItem) + "');");
            macros.sql_command(sql2);

            //получаем айди созданного объекта
            string id_object = macros.sql_command("SELECT MAX(idObject) FROM btk.Object;");

            // set datetime create
            macros.sql_command("update btk.Object set date_cteate = now() where idobject = '" + id_object + "';");

            //Создаем подписку для объекта
            string sql3 = string.Format("INSERT INTO btk.Subscription(products_has_Tarif_idproducts_has_Tarif, idobject) values('39'," + id_object + ");");
            macros.sql_command(sql3);

            //Получаем айди созданной подписки
            string sql4 = macros.sql_command("select max(idSubscr) from btk.Subscription;");

            //привязываем айди созданного объекта с айди созданной подписки
            string sql5 = string.Format("insert into btk.object_subscr (Object_idObject, Subscription_idSubscr) values (" + id_object + "," + sql4 + ");");
            macros.sql_command(sql5);

            ///////////////////////
            //Если все прошло успешно - завечиваем зеленым кнопку и затирает текстбоксы
            //////////////////////////
            button_create_object.Text = "Створено: " + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text + "\n Дата та час: " + DateTime.Now.ToString();
            maskedTextBox_BLE_CODE.Text = "";
            maskedTextBox_GSM_CODE.Text = "";
            textBox_id_to_create.Text = "";
            maskedTextBox_PUK.Text = "";
            maskedTextBox_sim_no_to_create.Text = "";
            textBox_bt_enable.Text = "";
            comboBox_tel_select.Text = "";
            search_tovar_comboBox.Text = "";
            button_create_object.BackColor = Color.Green;
        }

        private void CNTP_910_P()
        {
            /////////////////////
            //////Проверяем корректность введенных данных
            ///

            if (textBox_id_to_create.Text.Length <= 3)
            {
                textBox_id_to_create.BackColor = Color.Red;//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
                return;
            }
            if (maskedTextBox_sim_no_to_create.Text.Length <= 11)//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Red;
                return;
            }
            if (maskedTextBox_PUK.Text.Length <= 3)//Если PUK короче 4х символов останавливается и подсвкечиваем желтым
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Yellow;
                DialogResult result = MessageBox.Show(
                    "Вопрос",
                    "PUK Верный?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.No)
                    return;
                maskedTextBox_PUK.BackColor = Color.White;
            }

            // Проверям существует ли данный номер в системе
            string unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли данный номер в системе
            unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim2_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли указанный ИМЕЙ в системе
            string unswer2 = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_unique_id\"," +
                                                     "\"propValueMask\":\"" + textBox_id_to_create.Text + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m2 = JsonConvert.DeserializeObject<RootObject>(unswer2);
            if (m2.items.Count > 0)
            {
                MessageBox.Show("Указанный IMEI существует в WL");
                return;
            }

            /////////////////
            ///Создаем объект
            /////////////////
            string cr_obj_in = "&svc=core/create_unit&params={\"creatorId\":\"" + vars_form.wl_user_id + "\",\"name\":\"" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "\",\"hwTypeId\":\"9\",\"dataFlags\":\"1\"}";
            string json = macros.WialonRequest(cr_obj_in);
            var cr_obj_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (cr_obj_out.error != 0)
            {
                string text = macros.Get_wl_text_error(cr_obj_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Создаем пароль доступа к объекту
            /////////////////

            string accsess_pass_answer = macros.WialonRequest(
                "&svc=unit/update_access_password&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"accessPassword\":\"" + maskedTextBox_GSM_CODE.Text.ToString() + "\"}");
            var accsess_pass_answer_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (accsess_pass_answer_out.error != 0)
            {
                string text = macros.Get_wl_text_error(accsess_pass_answer_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Установливаем имя объекта и ID оборудования и
            /////////////////
            string item_id_in =
                "&svc=unit/update_device_type&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id
                + "\",\"deviceTypeId\":\"" + "9"
                + "\",\"uniqueId\":\"" + textBox_id_to_create.Text.ToString() + "\"}";
            string json2 = macros.WialonRequest(item_id_in);
            var item_id_out = JsonConvert.DeserializeObject<RootObject>(json2);
            if (item_id_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_id_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Устанавливаем номер СИМ
            /////////////////
            string item_phone_in = "&svc=unit/update_phone&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim_no_to_create.Text) + "\"}";
            string json3 = macros.WialonRequest(item_phone_in);
            var item_phone_out = JsonConvert.DeserializeObject<RootObject>(json3);
            if (item_phone_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Устанавливаем номер СИМ2
            /////////////////
            string item_phone2_in = "&svc=unit/update_phone2&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim2_no_to_create.Text) + "\"}";
            string json32 = macros.WialonRequest(item_phone2_in);
            var item_phone_out2 = JsonConvert.DeserializeObject<RootObject>(json32);
            if (item_phone_out2.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out2.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Создаем датчики
            /////////////////

            //1. Автоматическая блокировка двигателя
            string auto_block_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Автоматическая блокировка двигателя", "digital", "Вкл/Выкл", "auto_block", 1, "1", 0, "");

            //2. Охрана
            string protection_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Охрана", "digital", "Вкл/Выкл", "protection", 2, "1", 0, "");

            //3. Принудительная блокировка двигателя
            string man_block_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Принудительная блокировка двигателя", "digital", "Вкл/Выкл", "man_block", 3, "1", 0, "");

            //4. Авторизация
            string auth_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Авторизация", "digital", "Вкл/Выкл", "auth", 4, "1", 0, "");

            //5. Тревожная кнопка
            string alarm_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Тревожная кнопка_", "digital", "Вкл/Выкл", "alarm", 5, "1", 0, "");

            //6. Сервисный режим
            string valet_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сервисный режим", "digital", "Вкл/Выкл", "valet", 6, "1", 0, "");

            //7. Двери статус
            string all_door_status_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Двери статус", "digital", "Вкл/Выкл", "trunk%2Bhood+%2Bpass_door%2Bdriver_door%2Bl_rear_door%2Br_rear_door", 21, "1", 0, "");

            //8. Сработка открытие дверей в охране
            string Door_in_protection_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка открытие дверей в охране", "digital", "Вкл/Выкл", "alarm_act", 7, "1", 7, "");

            //9. Сработка сирены
            string alarm_act_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка сирены", "digital", "Вкл/Выкл", "alarm_act", 8, "1", 0, "");

            //10. Сработка датчика наклона
            string tilt_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика наклона", "digital", "Вкл/Выкл", "tilt_trig", 9, "1", 0, "");

            //11. Напряжение АКБ
            string ext_voltage_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Напряжение АКБ", "voltage", "В", "ext_voltage", 10, "1", 0, "");

            //12. Сработка датчика ударов
            string sh_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика ударов", "digital", "Вкл/Выкл", "sh_trig", 11, "1", 0, "");

            //13. Предупреждение датчика ударов
            string sh_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение датчика ударов", "digital", "Вкл/Выкл", "sh_warn", 12, "1", 0, "");

            //14. Сработка доп. датчика 1
            string ext1_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка доп. датчика 1", "digital", "Вкл/Выкл", "ext1_trig", 13, "1", 0, "");

            //15. Предупреждение доп. датчика 1
            string ext1_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение доп. датчика 1", "digital", "Вкл/Выкл", "ext1_warn", 14, "1", 0, "");

            //16. Сработка доп. датчика 2
            string ext2_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка доп. датчика 2", "digital", "Вкл/Выкл", "ext2_trig", 15, "1", 0, "");

            //17. Предупреждение доп. датчика 2
            string ext2_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение доп. датчика 2", "digital", "Вкл/Выкл", "ext2_warn", 16, "1", 0, "");

            //18. РАКБ
            string acc_voltage_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "РАКБ", "voltage", "В", "acc_voltage", 17, "1", 0, "");

            //19. Зажигание
            string ign_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Зажигание", "engine operation", "Вкл/Выкл", "ign", 18, "1", 0, "");

            //20. Центральный замок
            string central_lock_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Центральный замок", "digital", "Открыто/Закрыто", "central_lock", 19, "1", 0, "");

            //21. Двигатель заведен
            string engine_is_on_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Двигатель заведен", "digital", "Вкл/Выкл", "engine_is_on", 20, "1", 0, "");

            //22. Капот
            string hood_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Капот", "digital", "Откр/Закр", "hood", 22, "1", 0, "");

            //23. Передняя левая дверь (водителя)
            string driver_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Передняя левая дверь (водителя)", "digital", "Откр/Закр", "driver_door", 23, "1", 0, "");

            //24. Передняя правая дверь (пасс.)
            string pass_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Передняя правая дверь (пасс.)", "digital", "Откр/Закр", "pass_door", 24, "1", 0, "");

            //25. Задняя левая дверь
            string l_rear_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Задняя левая дверь", "digital", "Откр/Закр", "l_rear_door", 25, "1", 0, "");

            //26. Правая задняя дверь
            string r_rear_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Правая задняя дверь", "digital", "Откр/Закр", "r_rear_door", 26, "1", 0, "");

            //27. Багажник
            string trunk_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Багажник", "digital", "Откр/Закр", "trunk", 27, "1", 0, "");

            //28. Пробег
            string can_odo_km_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Пробег", "mileage", "КМ", "can_odo_km", 28, "1", 0, "");

            //29. Топливо
            string fuel_lev_l_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Топливо", "fuel level", "л", "fuel_lev_l", 29, "1", 0, "");

            //30. Ручной тормоз
            string hand_break_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Ручной тормоз", "digital", "Вкл/Выкл", "hand_break", 30, "1", 0, "");

            //31. Габаритные огни
            string marker_lights_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Габаритные огни", "digital", "Вкл/Выкл", "marker_lights", 31, "1", 0, "");

            //32. АКПП в P
            string parking_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "АКПП в P", "digital", "Вкл/Выкл", "parking", 32, "1", 0, "");

            //33. Ближний свет
            string dipped_beam_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Ближний свет", "digital", "Вкл/Выкл", "dipped_beam", 33, "1", 0, "");

            //34. Низкий уровень омывающей жидкости
            string washer_alert_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Низкий уровень омывающей жидкости", "digital", "Вкл/Выкл", "washer_alert", 34, "1", 0, "");

            //35. Температура двигателя
            string eng_temp_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Температура двигателя", "temperature", "C", "eng_temp", 35, "1", 0, "");

            //36. Сработка датчика глушения
            string aux_zone_1_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика глушения", "digital", "Вкл/Выкл", "aux_zone_1", 36, "1", 0, "");

            /////////////////
            ///Создаем произвольные поля
            /////////////////

            //Произвольное поле 1
            string answer = macros.create_custom_field_wl(cr_obj_out.item.id, "0 УВАГА", "");

            //Произвольное поле 2
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "02 Проект", "");

            //Произвольное поле 3
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.1 І Відповідальна особа (основна)", "");

            //Произвольное поле 4
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.2 ІІ Відповідальна особа", "");

            //Произвольное поле 5
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.3 ІІІ Відповідальна особа", "");

            //Произвольное поле 6
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.4 ІV Відповідальна особа", "");

            //Произвольное поле 7
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.5 V Відповідальна особа", "");

            //Произвольное поле 8
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.1.1 Оператор, що тестував", "");

            //Произвольное поле 9
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.11 Додатково встановлені сигналізації", "");

            //Произвольное поле 10
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.12 Постановка авто под охрану через багажник?", "");

            //Произвольное поле 11
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.15 Додатково встановлені датчики", "");

            //Произвольное поле 12
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.2.1 Установник: назва, адреса", "");

            //Произвольное поле 13
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.2.2 Установник-монтажник: ПІБ, №тел.", "");

            //Произвольное поле 14
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.3 Дата установки", "");

            //Произвольное поле 15
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.4 Місце установки пристрою ВЕНБЕСТ", "");

            //Произвольное поле 16
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.1 CAN-реле", "");

            //Произвольное поле 17
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.2 Звичайне реле", "");

            //Произвольное поле 18
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.7 Місце встановлення сервісної кнопки", "");

            //Произвольное поле 19
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.8.1 Дротова тривожна кнопка", "");

            //Произвольное поле 20
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.8.2 Бездротова тривожна кнопка", "");

            //Произвольное поле 21
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.3 Блокує Prizrak по CAN", "");

            //Произвольное поле 22
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.9.2 Штатні кнопки введення PIN-коду", "");

            //Административное поле 1
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.3 GSM-код", maskedTextBox_GSM_CODE.Text.ToString());

            //Административное поле 2
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.4 PUK-код", maskedTextBox_PUK.Text.ToString());

            //Административное поле 3
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.5 Bluetuth-код", maskedTextBox_BLE_CODE.Text.ToString());

            //Произвольное поле 3
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.1 Дата активації", "");

            //Произвольное поле 4
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.1.1 Оператор, що активував", "");

            //Произвольное поле 5
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.2 Дата встановлення PIN-коду", "");

            //Произвольное поле 6
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.3 PIN-код встановлено особою(клієнт/установлник)", "");

            //Произвольное поле 28
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.4 Обліковий запис WL", "");

            //Произвольное поле 29
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.1 Менеджер", "");

            //Произвольное поле 30
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.2 Договір обслуговування", "");

            //Произвольное поле 31
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.3 Гарантія до", "");

            //Произвольное поле 32
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.4 Дата закінчення договору страхування", "");

            //Произвольное поле 33
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "8.1 Паркінг 1", "");

            //Произвольное поле 34
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "8.2 Паркінг 2", "");

            //Произвольное поле 35
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.0 Примітки", "");

            //Произвольное поле 36
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.1 Техпаспорт", "");

            //Произвольное поле 37
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.2.1 Дата перевірки картки", "ДД.ММ.РРРР");

            //Произвольное поле 38
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.2.2 Оператор перевірки картки", "Прізвище");

            //Произвольное поле 39
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "10 Кодове слово", "");

            //Админитстративное поле 40
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "13 Prizrak 910 SN", search_tovar_comboBox.Text);

            /////////////////
            ///Добавляем в группу Объекты All
            /////////////////

            string get_units_on_group = macros.WialonRequest(
                "&svc=core/search_item&params={"
                + "\"id\":\"2612\","
                + "\"flags\":\"1\"}");//получаем все объекты группы

            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

            list_get_units_on_group.item.u.Add(cr_obj_out.item.id);//Доповляем в список новый объект
            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

            string gr_answer = macros.WialonRequest(
                "&svc=unit_group/update_units&params={"
                + "\"itemId\":\"2612\","
                + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

            /////////////////
            ///Создаем команды
            /////////////////
            ///http://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/unit/update_command_definition
            /// 83886080- закрыта команда  для клиента
            /// 16777216- открыта команда для клиента

            //0 - Запросить текущее состояние
            string cmd_refresh = macros.create_commads_wl(cr_obj_out.item.id, "0 - Запросить текущее состояние", "%23refresh%23", 83886080);

            //0 - Перезагрузить систему
            string cmd_reboot = macros.create_commads_wl(cr_obj_out.item.id, "0 - Перезагрузить систему", "%23reboot%23", 83886080);

            //1 - Закрыть автомобиль
            string cmd_protection_on = macros.create_commads_wl(cr_obj_out.item.id, "1 - Закрыть автомобиль", "%23protection=1%23", 16777216);

            //1 - Открыть автомобиль
            string cmd_protection_off = macros.create_commads_wl(cr_obj_out.item.id, "1 - Открыть автомобиль", "%23protection=0%23", 16777216);

            //2 - Автозапуск старт
            string cmd_autostart_on = macros.create_commads_wl(cr_obj_out.item.id, "2 - Автозапуск старт", "%23autostart=1%23", 83886080);

            //2 - Автозапуск стоп
            string cmd_autostart_off = macros.create_commads_wl(cr_obj_out.item.id, "2 - Автозапуск стоп", "%23autostart=0%23", 83886080);

            //3 - СТАРТ двигатель
            string cmd_man_block_off = macros.create_commads_wl(cr_obj_out.item.id, "3 - СТАРТ двигатель", "%23man_block=0%23", 83886080);

            //3 - СТОП двигатель
            string cmd_man_block_on = macros.create_commads_wl(cr_obj_out.item.id, "3 - СТОП двигатель", "%23man_block=1%23", 83886080);

            //4 - Включить сирену
            string cmd_alarm_on = macros.create_commads_wl(cr_obj_out.item.id, "4 - Включить сирену", "%23alarm=1%23", 16777216);

            //4 - Выключить сирену
            string cmd_alarm_off = macros.create_commads_wl(cr_obj_out.item.id, "4 - Выключить сирену", "%23alarm=0%23", 16777216);

            //5 - Включить поиск на парковке
            string cmd_search_on = macros.create_commads_wl(cr_obj_out.item.id, "5 - Включить поиск на парковке", "%23search=1%23", 16777216);

            //5 - Выключить поиск на парковке
            string cmd_search_off = macros.create_commads_wl(cr_obj_out.item.id, "5 - Выключить поиск на парковке", "%23search=0%23", 16777216);

            //6 - Включить сервисный режим
            string cmd_valet_on = macros.create_commads_wl(cr_obj_out.item.id, "6 - Включить сервисный режим", "%23valet=1%23", 16777216);

            //6 - Выключить сервисный режиме
            string cmd_valet_off = macros.create_commads_wl(cr_obj_out.item.id, "6 - Выключить сервисный режиме", "%23valet=0%23", 16777216);

            string id_sim = "";
            

            DataTable t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel_select.Text + "';");
            if (t.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(4) + "';");
            }

            string id_sim2 = "";
            DataTable t1 = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel2_select.Text + "';");
            if (t1.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim2 = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim2_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim2 = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim2_no_to_create.Text.Substring(4) + "';");
            }

            string sql2 = string.Format("insert into btk.Object(Object_id_wl, Object_imei, Object_name, Object_sim_no, Object_sim2_no, products_idproducts, Simcard_idSimcard, Simcard_idSimcard1, TS_info_idTS_info, TS_info_TS_brend_model_idTS_brend_model, Kontakti_idKontakti_serviceman, Dogovora_idDogovora, Objectcol_edit_date, Users_idUsers, Objectcol_sn_puk_card, Objectcol_puk, Objectcol_gsm_code, Objectcol_ble_code, Objectcol_bt_enable, Objectcol_sn_prizrak) "
                                        + "values('" + cr_obj_out.item.id
                                        + "', '" + textBox_id_to_create.Text.ToString()
                                        + "', '" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem)
                                        + " " + textBox_id_to_create.Text.ToString() + "','"
                                        + maskedTextBox_sim_no_to_create.Text.ToString() + "','"    
                                        + maskedTextBox_sim2_no_to_create.Text.ToString()
                                        + "', '" + comboBox_list_poructs.SelectedValue.ToString() + "', "
                                        + "'" + id_sim + "'," 
                                        +"'" + id_sim2 + "'," +
                                        "'1', '1', '1','1', null, '" + vars_form.user_login_id + "', '"
                                        + textBox_id_to_create.Text.ToString() + "', '"
                                        + maskedTextBox_PUK.Text.ToString() + "', '"
                                        + maskedTextBox_GSM_CODE.Text.ToString() + "', '"
                                        + maskedTextBox_BLE_CODE.Text.ToString() + "', '"
                                        + textBox_bt_enable.Text.ToString() + "', '"
                                        + search_tovar_comboBox.GetItemText(search_tovar_comboBox.SelectedItem) + "');");
            macros.sql_command(sql2);

            //получаем айди созданного объекта
            string id_object = macros.sql_command("SELECT MAX(idObject) FROM btk.Object;");

            // set datetime create
            macros.sql_command("update btk.Object set date_cteate = now() where idobject = '" + id_object + "';");

            //Создаем подписку для объекта
            string sql3 = string.Format("INSERT INTO btk.Subscription(products_has_Tarif_idproducts_has_Tarif, idobject) values('35','" + id_object + "');");
            macros.sql_command(sql3);

            //Получаем айди созданной подписки
            string sql4 = macros.sql_command("select max(idSubscr) from btk.Subscription;");

            //привязываем айди созданного объекта с айди созданной подписки
            string sql5 = string.Format("insert into btk.object_subscr (Object_idObject, Subscription_idSubscr) values (" + id_object + "," + sql4 + ");");
            macros.sql_command(sql5);

            ///////////////////////
            //Если все прошло успешно - завечиваем зеленым кнопку и затирает текстбоксы
            //////////////////////////
            button_create_object.Text = "Створено: " + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text + "\n Дата та час: " + DateTime.Now.ToString();
            maskedTextBox_BLE_CODE.Text = "";
            maskedTextBox_GSM_CODE.Text = "";
            textBox_id_to_create.Text = "";
            maskedTextBox_PUK.Text = "";
            maskedTextBox_sim_no_to_create.Text = "";
            textBox_bt_enable.Text = "";
            comboBox_tel_select.Text = "";
            search_tovar_comboBox.Text = "";
            button_create_object.BackColor = Color.Green;
        }

        private void CNTP_910_N()
        {
            /////////////////////
            //////Проверяем корректность введенных данных
            ///

            if (textBox_id_to_create.Text.Length <= 3)
            {
                textBox_id_to_create.BackColor = Color.Red;//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
                return;
            }
            if (maskedTextBox_sim_no_to_create.Text.Length <= 11)//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Red;
                return;
            }
            if (maskedTextBox_PUK.Text.Length <= 3)//Если PUK короче 4х символов останавливается и подсвкечиваем желтым
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Yellow;
                DialogResult result = MessageBox.Show(
                    "Вопрос",
                    "PUK Верный?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.No)
                    return;
                maskedTextBox_PUK.BackColor = Color.White;
            }

            // Проверям существует ли данный номер в системе
            string unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли данный номер в системе
            unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim2_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли указанный ИМЕЙ в системе
            string unswer2 = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_unique_id\"," +
                                                     "\"propValueMask\":\"" + textBox_id_to_create.Text + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m2 = JsonConvert.DeserializeObject<RootObject>(unswer2);
            if (m2.items.Count > 0)
            {
                MessageBox.Show("Указанный IMEI существует в WL");
                return;
            }
            
            /////////////////
            ///Создаем объект
            /////////////////
            string cr_obj_in = "&svc=core/create_unit&params={\"creatorId\":\"" + vars_form.wl_user_id + "\",\"name\":\"" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "\",\"hwTypeId\":\"9\",\"dataFlags\":\"1\"}";
            string json = macros.WialonRequest(cr_obj_in);
            var cr_obj_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (cr_obj_out.error != 0)
            {
                string text = macros.Get_wl_text_error(cr_obj_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Создаем пароль доступа к объекту
            /////////////////

            string accsess_pass_answer = macros.WialonRequest(
                "&svc=unit/update_access_password&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"accessPassword\":\"" + maskedTextBox_GSM_CODE.Text.ToString() + "\"}");
            var accsess_pass_answer_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (accsess_pass_answer_out.error != 0)
            {
                string text = macros.Get_wl_text_error(accsess_pass_answer_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Установливаем имя объекта и ID оборудования и
            /////////////////
            string item_id_in =
                "&svc=unit/update_device_type&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id
                + "\",\"deviceTypeId\":\"" + "9"
                + "\",\"uniqueId\":\"" + textBox_id_to_create.Text.ToString() + "\"}";
            string json2 = macros.WialonRequest(item_id_in);
            var item_id_out = JsonConvert.DeserializeObject<RootObject>(json2);
            if (item_id_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_id_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Устанавливаем номер СИМ
            /////////////////
            string item_phone_in = "&svc=unit/update_phone&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim_no_to_create.Text) + "\"}";
            string json3 = macros.WialonRequest(item_phone_in);
            var item_phone_out = JsonConvert.DeserializeObject<RootObject>(json3);
            if (item_phone_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }


            /////////////////
            ///Устанавливаем номер СИМ2
            /////////////////
            string item_phone2_in = "&svc=unit/update_phone2&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim2_no_to_create.Text) + "\"}";
            string json32 = macros.WialonRequest(item_phone2_in);
            var item_phone_out2 = JsonConvert.DeserializeObject<RootObject>(json32);
            if (item_phone_out2.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out2.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Создаем датчики
            /////////////////

            //1. Автоматическая блокировка двигателя
            string auto_block_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Автоматическая блокировка двигателя", "digital", "Вкл/Выкл", "auto_block", 1, "1", 0, "");

            //2. Охрана
            string protection_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Охрана", "digital", "Вкл/Выкл", "protection", 2, "1", 0, "");

            //3. Принудительная блокировка двигателя
            string man_block_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Принудительная блокировка двигателя", "digital", "Вкл/Выкл", "man_block", 3, "1", 0, "");

            //4. Авторизация
            string auth_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Авторизация", "digital", "Вкл/Выкл", "auth", 4, "1", 0, "");

            //5. Тревожная кнопка
            string alarm_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Тревожная кнопка_", "digital", "Вкл/Выкл", "alarm", 5, "1", 0, "");

            //6. Сервисный режим
            string valet_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сервисный режим", "digital", "Вкл/Выкл", "valet", 6, "1", 0, "");

            //7. Двери статус
            string all_door_status_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Двери статус", "digital", "Вкл/Выкл", "trunk%2Bhood+%2Bpass_door%2Bdriver_door%2Bl_rear_door%2Br_rear_door", 21, "1", 0, "");

            //8. Сработка открытие дверей в охране
            string Door_in_protection_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка открытие дверей в охране", "digital", "Вкл/Выкл", "alarm_act", 7, "1", 7, "");

            //9. Сработка сирены
            string alarm_act_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка сирены", "digital", "Вкл/Выкл", "alarm_act", 8, "1", 0, "");

            //10. Сработка датчика наклона
            string tilt_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика наклона", "digital", "Вкл/Выкл", "tilt_trig", 9, "1", 0, "");

            //11. Напряжение АКБ
            string ext_voltage_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Напряжение АКБ", "voltage", "В", "ext_voltage", 10, "1", 0, "");

            //12. Сработка датчика ударов
            string sh_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика ударов", "digital", "Вкл/Выкл", "sh_trig", 11, "1", 0, "");

            //13. Предупреждение датчика ударов
            string sh_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение датчика ударов", "digital", "Вкл/Выкл", "sh_warn", 12, "1", 0, "");

            //14. Сработка доп. датчика 1
            string ext1_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка доп. датчика 1", "digital", "Вкл/Выкл", "ext1_trig", 13, "1", 0, "");

            //15. Предупреждение доп. датчика 1
            string ext1_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение доп. датчика 1", "digital", "Вкл/Выкл", "ext1_warn", 14, "1", 0, "");

            //16. Сработка доп. датчика 2
            string ext2_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка доп. датчика 2", "digital", "Вкл/Выкл", "ext2_trig", 15, "1", 0, "");

            //17. Предупреждение доп. датчика 2
            string ext2_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение доп. датчика 2", "digital", "Вкл/Выкл", "ext2_warn", 16, "1", 0, "");

            //18. РАКБ
            string acc_voltage_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "РАКБ", "voltage", "В", "acc_voltage", 17, "1", 0, "");

            //19. Зажигание
            string ign_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Зажигание", "engine operation", "Вкл/Выкл", "ign", 18, "1", 0, "");

            //20. Центральный замок
            string central_lock_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Центральный замок", "digital", "Открыто/Закрыто", "central_lock", 19, "1", 0, "");

            //21. Двигатель заведен
            string engine_is_on_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Двигатель заведен", "digital", "Вкл/Выкл", "engine_is_on", 20, "1", 0, "");

            //22. Капот
            string hood_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Капот", "digital", "Откр/Закр", "hood", 22, "1", 0, "");

            //23. Передняя левая дверь (водителя)
            string driver_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Передняя левая дверь (водителя)", "digital", "Откр/Закр", "driver_door", 23, "1", 0, "");

            //24. Передняя правая дверь (пасс.)
            string pass_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Передняя правая дверь (пасс.)", "digital", "Откр/Закр", "pass_door", 24, "1", 0, "");

            //25. Задняя левая дверь
            string l_rear_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Задняя левая дверь", "digital", "Откр/Закр", "l_rear_door", 25, "1", 0, "");

            //26. Правая задняя дверь
            string r_rear_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Правая задняя дверь", "digital", "Откр/Закр", "r_rear_door", 26, "1", 0, "");

            //27. Багажник
            string trunk_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Багажник", "digital", "Откр/Закр", "trunk", 27, "1", 0, "");

            //28. Пробег
            string can_odo_km_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Пробег", "mileage", "КМ", "can_odo_km", 28, "1", 0, "");

            //29. Топливо
            string fuel_lev_l_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Топливо", "fuel level", "л", "fuel_lev_l", 29, "1", 0, "");

            //30. Ручной тормоз
            string hand_break_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Ручной тормоз", "digital", "Вкл/Выкл", "hand_break", 30, "1", 0, "");

            //31. Габаритные огни
            string marker_lights_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Габаритные огни", "digital", "Вкл/Выкл", "marker_lights", 31, "1", 0, "");

            //32. АКПП в P
            string parking_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "АКПП в P", "digital", "Вкл/Выкл", "parking", 32, "1", 0, "");

            //33. Ближний свет
            string dipped_beam_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Ближний свет", "digital", "Вкл/Выкл", "dipped_beam", 33, "1", 0, "");

            //34. Низкий уровень омывающей жидкости
            string washer_alert_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Низкий уровень омывающей жидкости", "digital", "Вкл/Выкл", "washer_alert", 34, "1", 0, "");

            //35. Температура двигателя
            string eng_temp_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Температура двигателя", "temperature", "C", "eng_temp", 35, "1", 0, "");

            //36. Сработка датчика глушения
            string aux_zone_1_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика глушения", "digital", "Вкл/Выкл", "aux_zone_1", 36, "1", 0, "");

            /////////////////
            ///Создаем произвольные поля
            /////////////////

            //Произвольное поле 1
            string answer = macros.create_custom_field_wl(cr_obj_out.item.id, "0 УВАГА", "");

            //Произвольное поле 2
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "02 Проект", "");

            //Произвольное поле 3
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.1 І Відповідальна особа (основна)", "");

            //Произвольное поле 4
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.2 ІІ Відповідальна особа", "");

            //Произвольное поле 5
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.3 ІІІ Відповідальна особа", "");

            //Произвольное поле 6
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.4 ІV Відповідальна особа", "");

            //Произвольное поле 7
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.5 V Відповідальна особа", "");

            //Произвольное поле 8
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.1.1 Оператор, що тестував", "");

            //Произвольное поле 9
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.11 Додатково встановлені сигналізації", "");

            //Произвольное поле 10
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.12 Постановка авто под охрану через багажник?", "");

            //Произвольное поле 11
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.15 Додатково встановлені датчики", "");

            //Произвольное поле 12
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.2.1 Установник: назва, адреса", "");

            //Произвольное поле 13
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.2.2 Установник-монтажник: ПІБ, №тел.", "");

            //Произвольное поле 14
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.3 Дата установки", "");

            //Произвольное поле 15
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.4 Місце установки пристрою ВЕНБЕСТ", "");

            //Произвольное поле 16
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.1 CAN-реле", "");

            //Произвольное поле 17
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.2 Звичайне реле", "");

            //Произвольное поле 18
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.7 Місце встановлення сервісної кнопки", "");

            //Произвольное поле 19
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.8.1 Дротова тривожна кнопка", "");

            //Произвольное поле 20
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.8.2 Бездротова тривожна кнопка", "");

            //Произвольное поле 21
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.9.1 Кнопки введення PIN коду: штатні, додатково встановленні", "");

            //Произвольное поле 22
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.9.2 Штатні кнопки введення PIN-коду", "");

            //Административное поле 21
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.3 GSM-код", maskedTextBox_GSM_CODE.Text.ToString());

            //Административное поле 22
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.4 PUK-код", maskedTextBox_PUK.Text.ToString());

            //Административное поле 23
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.5 Bluetuth-код", maskedTextBox_BLE_CODE.Text.ToString());

            //Произвольное поле 24
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.1 Дата активації", "");

            //Произвольное поле 25
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.1.1 Оператор, що активував", "");

            //Произвольное поле 26
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.2 Дата встановлення PIN-коду", "");

            //Произвольное поле 27
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.3 PIN-код встановлено особою(клієнт/установлник)", "");

            //Произвольное поле 28
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.4 Обліковий запис WL", "");

            //Произвольное поле 29
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.1 Менеджер", "");

            //Произвольное поле 30
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.2 Договір обслуговування", "");

            //Произвольное поле 31
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.3 Гарантія до", "");

            //Произвольное поле 32
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.4 Дата закінчення договору страхування", "");

            //Произвольное поле 33
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "8.1 Паркінг 1", "");

            //Произвольное поле 34
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "8.2 Паркінг 2", "");

            //Произвольное поле 35
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.0 Примітки", "");

            //Произвольное поле 36
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.1 Техпаспорт", "");

            //Произвольное поле 37
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.2.1 Дата перевірки картки", "ДД.ММ.РРРР");

            //Произвольное поле 38
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.2.2 Оператор перевірки картки", "Прізвище");

            //Произвольное поле 39
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "10 Кодове слово", "");

            //Админитстративное поле 40
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "13 Prizrak 910 SN", search_tovar_comboBox.Text);

            /////////////////
            ///Добавляем в группу Объекты All
            /////////////////

            string get_units_on_group = macros.WialonRequest(
                "&svc=core/search_item&params={"
                + "\"id\":\"2612\","
                + "\"flags\":\"1\"}");//получаем все объекты группы

            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

            list_get_units_on_group.item.u.Add(cr_obj_out.item.id);//Доповляем в список новый объект
            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

            string gr_answer = macros.WialonRequest(
                "&svc=unit_group/update_units&params={"
                + "\"itemId\":\"2612\","
                + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

            /////////////////
            ///Создаем команды
            /////////////////
            ///http://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/unit/update_command_definition
            /// 83886080- закрыта команда  для клиента
            /// 16777216- открыта команда для клиента

            //0 - Запросить текущее состояние
            string cmd_refresh = macros.create_commads_wl(cr_obj_out.item.id, "0 - Запросить текущее состояние", "%23refresh%23", 83886080);

            //0 - Перезагрузить систему
            string cmd_reboot = macros.create_commads_wl(cr_obj_out.item.id, "0 - Перезагрузить систему", "%23reboot%23", 83886080);

            //1 - Закрыть автомобиль
            string cmd_protection_on = macros.create_commads_wl(cr_obj_out.item.id, "1 - Закрыть автомобиль", "%23protection=1%23", 16777216);

            //1 - Открыть автомобиль
            string cmd_protection_off = macros.create_commads_wl(cr_obj_out.item.id, "1 - Открыть автомобиль", "%23protection=0%23", 16777216);

            //2 - Автозапуск старт
            string cmd_autostart_on = macros.create_commads_wl(cr_obj_out.item.id, "2 - Автозапуск старт", "%23autostart=1%23", 83886080);

            //2 - Автозапуск стоп
            string cmd_autostart_off = macros.create_commads_wl(cr_obj_out.item.id, "2 - Автозапуск стоп", "%23autostart=0%23", 83886080);

            //3 - СТАРТ двигатель
            string cmd_man_block_off = macros.create_commads_wl(cr_obj_out.item.id, "3 - СТАРТ двигатель", "%23man_block=0%23", 83886080);

            //3 - СТОП двигатель
            string cmd_man_block_on = macros.create_commads_wl(cr_obj_out.item.id, "3 - СТОП двигатель", "%23man_block=1%23", 83886080);

            //4 - Включить сирену
            string cmd_alarm_on = macros.create_commads_wl(cr_obj_out.item.id, "4 - Включить сирену", "%23alarm=1%23", 16777216);

            //4 - Выключить сирену
            string cmd_alarm_off = macros.create_commads_wl(cr_obj_out.item.id, "4 - Выключить сирену", "%23alarm=0%23", 16777216);

            //5 - Включить поиск на парковке
            string cmd_search_on = macros.create_commads_wl(cr_obj_out.item.id, "5 - Включить поиск на парковке", "%23search=1%23", 16777216);

            //5 - Выключить поиск на парковке
            string cmd_search_off = macros.create_commads_wl(cr_obj_out.item.id, "5 - Выключить поиск на парковке", "%23search=0%23", 16777216);

            //6 - Включить сервисный режим
            string cmd_valet_on = macros.create_commads_wl(cr_obj_out.item.id, "6 - Включить сервисный режим", "%23valet=1%23", 16777216);

            //6 - Выключить сервисный режиме
            string cmd_valet_off = macros.create_commads_wl(cr_obj_out.item.id, "6 - Выключить сервисный режиме", "%23valet=0%23", 16777216);

            string id_sim = "";


            DataTable t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel_select.Text + "';");
            if (t.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(4) + "';");
            }

            string id_sim2 = "";
            DataTable t1 = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel2_select.Text + "';");
            if (t1.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim2 = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim2_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim2 = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim2_no_to_create.Text.Substring(4) + "';");
            }

            string sql2 = string.Format("insert into btk.Object(Object_id_wl, Object_imei, Object_name, Object_sim_no, Object_sim2_no, products_idproducts, Simcard_idSimcard, Simcard_idSimcard1, TS_info_idTS_info, TS_info_TS_brend_model_idTS_brend_model, Kontakti_idKontakti_serviceman, Dogovora_idDogovora, Objectcol_edit_date, Users_idUsers, Objectcol_sn_puk_card, Objectcol_puk, Objectcol_gsm_code, Objectcol_ble_code, Objectcol_bt_enable, Objectcol_sn_prizrak) "
                                        + "values('" + cr_obj_out.item.id
                                        + "', '" + textBox_id_to_create.Text.ToString()
                                        + "', '" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem)
                                        + " " + textBox_id_to_create.Text.ToString() + "','"
                                        + maskedTextBox_sim_no_to_create.Text.ToString() + "','"
                                        + maskedTextBox_sim2_no_to_create.Text.ToString()
                                        + "', '" + comboBox_list_poructs.SelectedValue.ToString() + "', "
                                        + "'" + id_sim + "',"
                                        + "'" + id_sim2 + "'," +
                                        "'1', '1', '1','1', null, '" + vars_form.user_login_id + "', '"
                                        + textBox_id_to_create.Text.ToString() + "', '"
                                        + maskedTextBox_PUK.Text.ToString() + "', '"
                                        + maskedTextBox_GSM_CODE.Text.ToString() + "', '"
                                        + maskedTextBox_BLE_CODE.Text.ToString() + "', '"
                                        + textBox_bt_enable.Text.ToString() + "', '"
                                        + search_tovar_comboBox.GetItemText(search_tovar_comboBox.SelectedItem) + "');");
            macros.sql_command(sql2);

            //получаем айди созданного объекта
            string id_object = macros.sql_command("SELECT MAX(idObject) FROM btk.Object;");

            //Создаем подписку для объекта
            string sql3 = string.Format("INSERT INTO btk.Subscription(products_has_Tarif_idproducts_has_Tarif, idobject) values('34','" + id_object + "');");
            macros.sql_command(sql3);

            // set datetime create
            macros.sql_command("update btk.Object set date_cteate = now() where idobject = '" + id_object + "';");

            //Получаем айди созданной подписки
            string sql4 = macros.sql_command("select max(idSubscr) from btk.Subscription;");

            //привязываем айди созданного объекта с айди созданной подписки
            string sql5 = string.Format("insert into btk.object_subscr (Object_idObject, Subscription_idSubscr) values (" + id_object + "," + sql4 + ");");
            macros.sql_command(sql5);

            ///////////////////////
            //Если все прошло успешно - завечиваем зеленым кнопку и затирает текстбоксы
            //////////////////////////
            button_create_object.Text = "Створено: " + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text + "\n Дата та час: " + DateTime.Now.ToString();
            maskedTextBox_BLE_CODE.Text = "";
            maskedTextBox_GSM_CODE.Text = "";
            textBox_id_to_create.Text = "";
            maskedTextBox_PUK.Text = "";
            maskedTextBox_sim_no_to_create.Text = "";
            textBox_bt_enable.Text = "";
            comboBox_tel_select.Text = "";
            search_tovar_comboBox.Text = "";
            button_create_object.BackColor = Color.Green;
        }

        private void CNTP_910BT_P()
        {
            /////////////////////
            //////Проверяем корректность введенных данных
            ///

            if (textBox_id_to_create.Text.Length <= 3)
            {
                textBox_id_to_create.BackColor = Color.Red;//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
                return;
            }
            if (maskedTextBox_sim_no_to_create.Text.Length <= 11)//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Red;
                return;
            }
            if (maskedTextBox_PUK.Text.Length <= 3)//Если PUK короче 4х символов останавливается и подсвкечиваем желтым
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Yellow;
                DialogResult result = MessageBox.Show(
                    "Вопрос",
                    "PUK Верный?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.No)
                    return;
                maskedTextBox_PUK.BackColor = Color.White;
            }

            // Проверям существует ли данный номер в системе
            string unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли данный номер в системе
            unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim2_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли указанный ИМЕЙ в системе
            string unswer2 = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_unique_id\"," +
                                                     "\"propValueMask\":\"" + textBox_id_to_create.Text + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m2 = JsonConvert.DeserializeObject<RootObject>(unswer2);
            if (m2.items.Count > 0)
            {
                MessageBox.Show("Указанный IMEI существует в WL");
                return;
            }

            /////////////////
            ///Создаем объект
            /////////////////
            string cr_obj_in = "&svc=core/create_unit&params={\"creatorId\":\"" + vars_form.wl_user_id + "\",\"name\":\"" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "\",\"hwTypeId\":\"9\",\"dataFlags\":\"1\"}";
            string json = macros.WialonRequest(cr_obj_in);
            var cr_obj_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (cr_obj_out.error != 0)
            {
                string text = macros.Get_wl_text_error(cr_obj_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Создаем пароль доступа к объекту
            /////////////////

            string accsess_pass_answer = macros.WialonRequest(
                "&svc=unit/update_access_password&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"accessPassword\":\"" + maskedTextBox_GSM_CODE.Text.ToString() + "\"}");
            var accsess_pass_answer_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (accsess_pass_answer_out.error != 0)
            {
                string text = macros.Get_wl_text_error(accsess_pass_answer_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Установливаем имя объекта и ID оборудования и
            /////////////////
            string item_id_in =
                "&svc=unit/update_device_type&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id
                + "\",\"deviceTypeId\":\"" + "9"
                + "\",\"uniqueId\":\"" + textBox_id_to_create.Text.ToString() + "\"}";
            string json2 = macros.WialonRequest(item_id_in);
            var item_id_out = JsonConvert.DeserializeObject<RootObject>(json2);
            if (item_id_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_id_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Устанавливаем номер СИМ
            /////////////////
            string item_phone_in = "&svc=unit/update_phone&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim_no_to_create.Text) + "\"}";
            string json3 = macros.WialonRequest(item_phone_in);
            var item_phone_out = JsonConvert.DeserializeObject<RootObject>(json3);
            if (item_phone_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Устанавливаем номер СИМ2
            /////////////////
            string item_phone2_in = "&svc=unit/update_phone2&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim2_no_to_create.Text) + "\"}";
            string json32 = macros.WialonRequest(item_phone2_in);
            var item_phone_out2 = JsonConvert.DeserializeObject<RootObject>(json32);
            if (item_phone_out2.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out2.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Создаем датчики
            /////////////////

            //1. Автоматическая блокировка двигателя
            string auto_block_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Автоматическая блокировка двигателя", "digital", "Вкл/Выкл", "auto_block", 1, "1", 0, "");

            //2. Охрана
            string protection_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Охрана", "digital", "Вкл/Выкл", "protection", 2, "1", 0, "");

            //3. Принудительная блокировка двигателя
            string man_block_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Принудительная блокировка двигателя", "digital", "Вкл/Выкл", "man_block", 3, "1", 0, "");

            //4. Авторизация
            string auth_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Авторизация", "digital", "Вкл/Выкл", "auth", 4, "1", 0, "");

            //5. Тревожная кнопка
            string alarm_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Тревожная кнопка_", "digital", "Вкл/Выкл", "alarm", 5, "1", 0, "");

            //6. Сервисный режим
            string valet_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сервисный режим", "digital", "Вкл/Выкл", "valet", 6, "1", 0, "");

            //7. Двери статус
            string all_door_status_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Двери статус", "digital", "Вкл/Выкл", "trunk%2Bhood+%2Bpass_door%2Bdriver_door%2Bl_rear_door%2Br_rear_door", 21, "1", 0, "");

            //8. Сработка открытие дверей в охране
            string Door_in_protection_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка открытие дверей в охране", "digital", "Вкл/Выкл", "alarm_act", 7, "1", 7, "");

            //9. Сработка сирены
            string alarm_act_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка сирены", "digital", "Вкл/Выкл", "alarm_act", 8, "1", 0, "");

            //10. Сработка датчика наклона
            string tilt_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика наклона", "digital", "Вкл/Выкл", "tilt_trig", 9, "1", 0, "");

            //11. Напряжение АКБ
            string ext_voltage_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Напряжение АКБ", "voltage", "В", "ext_voltage", 10, "1", 0, "");

            //12. Сработка датчика ударов
            string sh_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика ударов", "digital", "Вкл/Выкл", "sh_trig", 11, "1", 0, "");

            //13. Предупреждение датчика ударов
            string sh_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение датчика ударов", "digital", "Вкл/Выкл", "sh_warn", 12, "1", 0, "");

            //14. Сработка доп. датчика 1
            string ext1_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка доп. датчика 1", "digital", "Вкл/Выкл", "ext1_trig", 13, "1", 0, "");

            //15. Предупреждение доп. датчика 1
            string ext1_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение доп. датчика 1", "digital", "Вкл/Выкл", "ext1_warn", 14, "1", 0, "");

            //16. Сработка доп. датчика 2
            string ext2_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка доп. датчика 2", "digital", "Вкл/Выкл", "ext2_trig", 15, "1", 0, "");

            //17. Предупреждение доп. датчика 2
            string ext2_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение доп. датчика 2", "digital", "Вкл/Выкл", "ext2_warn", 16, "1", 0, "");

            //18. РАКБ
            string acc_voltage_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "РАКБ", "voltage", "В", "acc_voltage", 17, "1", 0, "");

            //19. Зажигание
            string ign_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Зажигание", "engine operation", "Вкл/Выкл", "ign", 18, "1", 0, "");

            //20. Центральный замок
            string central_lock_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Центральный замок", "digital", "Открыто/Закрыто", "central_lock", 19, "1", 0, "");

            //21. Двигатель заведен
            string engine_is_on_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Двигатель заведен", "digital", "Вкл/Выкл", "engine_is_on", 20, "1", 0, "");

            //22. Капот
            string hood_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Капот", "digital", "Откр/Закр", "hood", 22, "1", 0, "");

            //23. Передняя левая дверь (водителя)
            string driver_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Передняя левая дверь (водителя)", "digital", "Откр/Закр", "driver_door", 23, "1", 0, "");

            //24. Передняя правая дверь (пасс.)
            string pass_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Передняя правая дверь (пасс.)", "digital", "Откр/Закр", "pass_door", 24, "1", 0, "");

            //25. Задняя левая дверь
            string l_rear_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Задняя левая дверь", "digital", "Откр/Закр", "l_rear_door", 25, "1", 0, "");

            //26. Правая задняя дверь
            string r_rear_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Правая задняя дверь", "digital", "Откр/Закр", "r_rear_door", 26, "1", 0, "");

            //27. Багажник
            string trunk_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Багажник", "digital", "Откр/Закр", "trunk", 27, "1", 0, "");

            //28. Пробег
            string can_odo_km_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Пробег", "mileage", "КМ", "can_odo_km", 28, "1", 0, "");

            //29. Топливо
            string fuel_lev_l_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Топливо", "fuel level", "л", "fuel_lev_l", 29, "1", 0, "");

            //30. Ручной тормоз
            string hand_break_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Ручной тормоз", "digital", "Вкл/Выкл", "hand_break", 30, "1", 0, "");

            //31. Габаритные огни
            string marker_lights_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Габаритные огни", "digital", "Вкл/Выкл", "marker_lights", 31, "1", 0, "");

            //32. АКПП в P
            string parking_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "АКПП в P", "digital", "Вкл/Выкл", "parking", 32, "1", 0, "");

            //33. Ближний свет
            string dipped_beam_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Ближний свет", "digital", "Вкл/Выкл", "dipped_beam", 33, "1", 0, "");

            //34. Низкий уровень омывающей жидкости
            string washer_alert_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Низкий уровень омывающей жидкости", "digital", "Вкл/Выкл", "washer_alert", 34, "1", 0, "");

            //35. Температура двигателя
            string eng_temp_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Температура двигателя", "temperature", "C", "eng_temp", 35, "1", 0, "");

            //36. Сработка датчика глушения
            string aux_zone_1_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика глушения", "digital", "Вкл/Выкл", "aux_zone_1", 36, "1", 0, "");

            /////////////////
            ///Создаем произвольные поля
            /////////////////

            //Произвольное поле 1
            string answer = macros.create_custom_field_wl(cr_obj_out.item.id, "0 УВАГА", "");

            //Произвольное поле 2
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "02 Проект", "");

            //Произвольное поле 3
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.1 І Відповідальна особа (основна)", "");

            //Произвольное поле 4
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.2 ІІ Відповідальна особа", "");

            //Произвольное поле 5
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.3 ІІІ Відповідальна особа", "");

            //Произвольное поле 6
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.4 ІV Відповідальна особа", "");

            //Произвольное поле 7
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.5 V Відповідальна особа", "");

            //Произвольное поле 8
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.1.1 Оператор, що тестував", "");

            //Произвольное поле 9
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.11 Додатково встановлені сигналізації", "");

            //Произвольное поле 10
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.12 Постановка авто под охрану через багажник?", "");

            //Произвольное поле 11
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.15 Додатково встановлені датчики", "");

            //Произвольное поле 12
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.2.1 Установник: назва, адреса", "");

            //Произвольное поле 13
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.2.2 Установник-монтажник: ПІБ, №тел.", "");

            //Произвольное поле 14
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.3 Дата установки", "");

            //Произвольное поле 15
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.4 Місце установки пристрою ВЕНБЕСТ", "");

            //Произвольное поле 16
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.1 CAN-реле", "");

            //Произвольное поле 17
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.2 Звичайне реле", "");

            //Произвольное поле 18
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.7 Місце встановлення сервісної кнопки", "");

            //Произвольное поле 19
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.8.1 Дротова тривожна кнопка", "");

            //Произвольное поле 20
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.8.2 Бездротова тривожна кнопка", "");

            //Произвольное поле 21
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.3 Блокує Prizrak по CAN", "");

            //Произвольное поле 22
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.9.2 Штатні кнопки введення PIN-коду", "");

            //Административное поле 1
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.3 GSM-код", maskedTextBox_GSM_CODE.Text.ToString());

            //Административное поле 2
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.4 PUK-код", maskedTextBox_PUK.Text.ToString());

            //Административное поле 3
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.5 Bluetuth-код", maskedTextBox_BLE_CODE.Text.ToString());

            //Произвольное поле 3
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.1 Дата активації", "");

            //Произвольное поле 4
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.1.1 Оператор, що активував", "");

            //Произвольное поле 5
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.2 Дата встановлення PIN-коду", "");

            //Произвольное поле 6
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.3 PIN-код встановлено особою(клієнт/установлник)", "");

            //Произвольное поле 28
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.4 Обліковий запис WL", "");

            //Произвольное поле 29
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.1 Менеджер", "");

            //Произвольное поле 30
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.2 Договір обслуговування", "");

            //Произвольное поле 31
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.3 Гарантія до", "");

            //Произвольное поле 32
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.4 Дата закінчення договору страхування", "");

            //Произвольное поле 33
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "8.1 Паркінг 1", "");

            //Произвольное поле 34
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "8.2 Паркінг 2", "");

            //Произвольное поле 35
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.0 Примітки", "");

            //Произвольное поле 36
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.1 Техпаспорт", "");

            //Произвольное поле 37
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.2.1 Дата перевірки картки", "ДД.ММ.РРРР");

            //Произвольное поле 38
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.2.2 Оператор перевірки картки", "Прізвище");

            //Произвольное поле 39
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "10 Кодове слово", "");

            //Админитстративное поле 40
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "13 Prizrak 910 SN", search_tovar_comboBox.Text);

            /////////////////
            ///Добавляем в группу Объекты All
            /////////////////

            string get_units_on_group = macros.WialonRequest(
                "&svc=core/search_item&params={"
                + "\"id\":\"2612\","
                + "\"flags\":\"1\"}");//получаем все объекты группы

            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

            list_get_units_on_group.item.u.Add(cr_obj_out.item.id);//Доповляем в список новый объект
            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

            string gr_answer = macros.WialonRequest(
                "&svc=unit_group/update_units&params={"
                + "\"itemId\":\"2612\","
                + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

            /////////////////
            ///Создаем команды
            /////////////////
            ///http://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/unit/update_command_definition
            /// 83886080- закрыта команда  для клиента
            /// 16777216- открыта команда для клиента

            //0 - Запросить текущее состояние
            string cmd_refresh = macros.create_commads_wl(cr_obj_out.item.id, "0 - Запросить текущее состояние", "%23refresh%23", 83886080);

            //0 - Перезагрузить систему
            string cmd_reboot = macros.create_commads_wl(cr_obj_out.item.id, "0 - Перезагрузить систему", "%23reboot%23", 83886080);

            //1 - Закрыть автомобиль
            string cmd_protection_on = macros.create_commads_wl(cr_obj_out.item.id, "1 - Закрыть автомобиль", "%23protection=1%23", 16777216);

            //1 - Открыть автомобиль
            string cmd_protection_off = macros.create_commads_wl(cr_obj_out.item.id, "1 - Открыть автомобиль", "%23protection=0%23", 16777216);

            //2 - Автозапуск старт
            string cmd_autostart_on = macros.create_commads_wl(cr_obj_out.item.id, "2 - Автозапуск старт", "%23autostart=1%23", 83886080);

            //2 - Автозапуск стоп
            string cmd_autostart_off = macros.create_commads_wl(cr_obj_out.item.id, "2 - Автозапуск стоп", "%23autostart=0%23", 83886080);

            //3 - СТАРТ двигатель
            string cmd_man_block_off = macros.create_commads_wl(cr_obj_out.item.id, "3 - СТАРТ двигатель", "%23man_block=0%23", 83886080);

            //3 - СТОП двигатель
            string cmd_man_block_on = macros.create_commads_wl(cr_obj_out.item.id, "3 - СТОП двигатель", "%23man_block=1%23", 83886080);

            //4 - Включить сирену
            string cmd_alarm_on = macros.create_commads_wl(cr_obj_out.item.id, "4 - Включить сирену", "%23alarm=1%23", 16777216);

            //4 - Выключить сирену
            string cmd_alarm_off = macros.create_commads_wl(cr_obj_out.item.id, "4 - Выключить сирену", "%23alarm=0%23", 16777216);

            //5 - Включить поиск на парковке
            string cmd_search_on = macros.create_commads_wl(cr_obj_out.item.id, "5 - Включить поиск на парковке", "%23search=1%23", 16777216);

            //5 - Выключить поиск на парковке
            string cmd_search_off = macros.create_commads_wl(cr_obj_out.item.id, "5 - Выключить поиск на парковке", "%23search=0%23", 16777216);

            //6 - Включить сервисный режим
            string cmd_valet_on = macros.create_commads_wl(cr_obj_out.item.id, "6 - Включить сервисный режим", "%23valet=1%23", 16777216);

            //6 - Выключить сервисный режиме
            string cmd_valet_off = macros.create_commads_wl(cr_obj_out.item.id, "6 - Выключить сервисный режиме", "%23valet=0%23", 16777216);

            string id_sim = "";


            DataTable t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel_select.Text + "';");
            if (t.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(4) + "';");
            }

            string id_sim2 = "";
            DataTable t1 = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel2_select.Text + "';");
            if (t1.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim2 = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim2_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim2 = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim2_no_to_create.Text.Substring(4) + "';");
            }

            string sql2 = string.Format("insert into btk.Object(Object_id_wl, Object_imei, Object_name, Object_sim_no, Object_sim2_no, products_idproducts, Simcard_idSimcard, Simcard_idSimcard1, TS_info_idTS_info, TS_info_TS_brend_model_idTS_brend_model, Kontakti_idKontakti_serviceman, Dogovora_idDogovora, Objectcol_edit_date, Users_idUsers, Objectcol_sn_puk_card, Objectcol_puk, Objectcol_gsm_code, Objectcol_ble_code, Objectcol_bt_enable, Objectcol_sn_prizrak) "
                                        + "values('" + cr_obj_out.item.id
                                        + "', '" + textBox_id_to_create.Text.ToString()
                                        + "', '" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem)
                                        + " " + textBox_id_to_create.Text.ToString() + "','"
                                        + maskedTextBox_sim_no_to_create.Text.ToString() + "','"
                                        + maskedTextBox_sim2_no_to_create.Text.ToString()
                                        + "', '" + comboBox_list_poructs.SelectedValue.ToString() + "', "
                                        + "'" + id_sim + "',"
                                        + "'" + id_sim2 + "'," +
                                        "'1', '1', '1','1', null, '" + vars_form.user_login_id + "', '"
                                        + textBox_id_to_create.Text.ToString() + "', '"
                                        + maskedTextBox_PUK.Text.ToString() + "', '"
                                        + maskedTextBox_GSM_CODE.Text.ToString() + "', '"
                                        + maskedTextBox_BLE_CODE.Text.ToString() + "', '"
                                        + textBox_bt_enable.Text.ToString() + "', '"
                                        + search_tovar_comboBox.GetItemText(search_tovar_comboBox.SelectedItem) + "');");
            macros.sql_command(sql2);

            //получаем айди созданного объекта
            string id_object = macros.sql_command("SELECT MAX(idObject) FROM btk.Object;");

            // set datetime create
            macros.sql_command("update btk.Object set date_cteate = now() where idobject = '" + id_object + "';");

            //Создаем подписку для объекта
            string sql3 = string.Format("INSERT INTO btk.Subscription(products_has_Tarif_idproducts_has_Tarif, idobject) values('42','" + id_object + "');");
            macros.sql_command(sql3);

            //Получаем айди созданной подписки
            string sql4 = macros.sql_command("select max(idSubscr) from btk.Subscription;");

            //привязываем айди созданного объекта с айди созданной подписки
            string sql5 = string.Format("insert into btk.object_subscr (Object_idObject, Subscription_idSubscr) values (" + id_object + "," + sql4 + ");");
            macros.sql_command(sql5);

            ///////////////////////
            //Если все прошло успешно - завечиваем зеленым кнопку и затирает текстбоксы
            //////////////////////////
            button_create_object.Text = "Створено: " + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text + "\n Дата та час: " + DateTime.Now.ToString();
            maskedTextBox_BLE_CODE.Text = "";
            maskedTextBox_GSM_CODE.Text = "";
            textBox_id_to_create.Text = "";
            maskedTextBox_PUK.Text = "";
            maskedTextBox_sim_no_to_create.Text = "";
            textBox_bt_enable.Text = "";
            comboBox_tel_select.Text = "";
            search_tovar_comboBox.Text = "";
            button_create_object.BackColor = Color.Green;
        }

        private void CNTP_910BT_N()
        {
            /////////////////////
            //////Проверяем корректность введенных данных
            ///

            if (textBox_id_to_create.Text.Length <= 3)
            {
                textBox_id_to_create.BackColor = Color.Red;//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
                return;
            }
            if (maskedTextBox_sim_no_to_create.Text.Length <= 11)//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Red;
                return;
            }
            if (maskedTextBox_PUK.Text.Length <= 3)//Если PUK короче 4х символов останавливается и подсвкечиваем желтым
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Yellow;
                DialogResult result = MessageBox.Show(
                    "Вопрос",
                    "PUK Верный?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.No)
                    return;
                maskedTextBox_PUK.BackColor = Color.White;
            }

            // Проверям существует ли данный номер в системе
            string unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли данный номер в системе
            unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim2_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли указанный ИМЕЙ в системе
            string unswer2 = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_unique_id\"," +
                                                     "\"propValueMask\":\"" + textBox_id_to_create.Text + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m2 = JsonConvert.DeserializeObject<RootObject>(unswer2);
            if (m2.items.Count > 0)
            {
                MessageBox.Show("Указанный IMEI существует в WL");
                return;
            }

            /////////////////
            ///Создаем объект
            /////////////////
            string cr_obj_in = "&svc=core/create_unit&params={\"creatorId\":\"" + vars_form.wl_user_id + "\",\"name\":\"" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "\",\"hwTypeId\":\"9\",\"dataFlags\":\"1\"}";
            string json = macros.WialonRequest(cr_obj_in);
            var cr_obj_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (cr_obj_out.error != 0)
            {
                string text = macros.Get_wl_text_error(cr_obj_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Создаем пароль доступа к объекту
            /////////////////

            string accsess_pass_answer = macros.WialonRequest(
                "&svc=unit/update_access_password&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"accessPassword\":\"" + maskedTextBox_GSM_CODE.Text.ToString() + "\"}");
            var accsess_pass_answer_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (accsess_pass_answer_out.error != 0)
            {
                string text = macros.Get_wl_text_error(accsess_pass_answer_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Установливаем имя объекта и ID оборудования и
            /////////////////
            string item_id_in =
                "&svc=unit/update_device_type&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id
                + "\",\"deviceTypeId\":\"" + "9"
                + "\",\"uniqueId\":\"" + textBox_id_to_create.Text.ToString() + "\"}";
            string json2 = macros.WialonRequest(item_id_in);
            var item_id_out = JsonConvert.DeserializeObject<RootObject>(json2);
            if (item_id_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_id_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Устанавливаем номер СИМ
            /////////////////
            string item_phone_in = "&svc=unit/update_phone&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim_no_to_create.Text) + "\"}";
            string json3 = macros.WialonRequest(item_phone_in);
            var item_phone_out = JsonConvert.DeserializeObject<RootObject>(json3);
            if (item_phone_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }


            /////////////////
            ///Устанавливаем номер СИМ2
            /////////////////
            string item_phone2_in = "&svc=unit/update_phone2&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim2_no_to_create.Text) + "\"}";
            string json32 = macros.WialonRequest(item_phone2_in);
            var item_phone_out2 = JsonConvert.DeserializeObject<RootObject>(json32);
            if (item_phone_out2.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out2.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Создаем датчики
            /////////////////

            //1. Автоматическая блокировка двигателя
            string auto_block_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Автоматическая блокировка двигателя", "digital", "Вкл/Выкл", "auto_block", 1, "1", 0, "");

            //2. Охрана
            string protection_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Охрана", "digital", "Вкл/Выкл", "protection", 2, "1", 0, "");

            //3. Принудительная блокировка двигателя
            string man_block_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Принудительная блокировка двигателя", "digital", "Вкл/Выкл", "man_block", 3, "1", 0, "");

            //4. Авторизация
            string auth_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Авторизация", "digital", "Вкл/Выкл", "auth", 4, "1", 0, "");

            //5. Тревожная кнопка
            string alarm_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Тревожная кнопка_", "digital", "Вкл/Выкл", "alarm", 5, "1", 0, "");

            //6. Сервисный режим
            string valet_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сервисный режим", "digital", "Вкл/Выкл", "valet", 6, "1", 0, "");

            //7. Двери статус
            string all_door_status_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Двери статус", "digital", "Вкл/Выкл", "trunk%2Bhood+%2Bpass_door%2Bdriver_door%2Bl_rear_door%2Br_rear_door", 21, "1", 0, "");

            //8. Сработка открытие дверей в охране
            string Door_in_protection_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка открытие дверей в охране", "digital", "Вкл/Выкл", "alarm_act", 7, "1", 7, "");

            //9. Сработка сирены
            string alarm_act_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка сирены", "digital", "Вкл/Выкл", "alarm_act", 8, "1", 0, "");

            //10. Сработка датчика наклона
            string tilt_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика наклона", "digital", "Вкл/Выкл", "tilt_trig", 9, "1", 0, "");

            //11. Напряжение АКБ
            string ext_voltage_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Напряжение АКБ", "voltage", "В", "ext_voltage", 10, "1", 0, "");

            //12. Сработка датчика ударов
            string sh_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика ударов", "digital", "Вкл/Выкл", "sh_trig", 11, "1", 0, "");

            //13. Предупреждение датчика ударов
            string sh_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение датчика ударов", "digital", "Вкл/Выкл", "sh_warn", 12, "1", 0, "");

            //14. Сработка доп. датчика 1
            string ext1_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка доп. датчика 1", "digital", "Вкл/Выкл", "ext1_trig", 13, "1", 0, "");

            //15. Предупреждение доп. датчика 1
            string ext1_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение доп. датчика 1", "digital", "Вкл/Выкл", "ext1_warn", 14, "1", 0, "");

            //16. Сработка доп. датчика 2
            string ext2_trig_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка доп. датчика 2", "digital", "Вкл/Выкл", "ext2_trig", 15, "1", 0, "");

            //17. Предупреждение доп. датчика 2
            string ext2_warn_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Предупреждение доп. датчика 2", "digital", "Вкл/Выкл", "ext2_warn", 16, "1", 0, "");

            //18. РАКБ
            string acc_voltage_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "РАКБ", "voltage", "В", "acc_voltage", 17, "1", 0, "");

            //19. Зажигание
            string ign_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Зажигание", "engine operation", "Вкл/Выкл", "ign", 18, "1", 0, "");

            //20. Центральный замок
            string central_lock_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Центральный замок", "digital", "Открыто/Закрыто", "central_lock", 19, "1", 0, "");

            //21. Двигатель заведен
            string engine_is_on_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Двигатель заведен", "digital", "Вкл/Выкл", "engine_is_on", 20, "1", 0, "");

            //22. Капот
            string hood_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Капот", "digital", "Откр/Закр", "hood", 22, "1", 0, "");

            //23. Передняя левая дверь (водителя)
            string driver_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Передняя левая дверь (водителя)", "digital", "Откр/Закр", "driver_door", 23, "1", 0, "");

            //24. Передняя правая дверь (пасс.)
            string pass_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Передняя правая дверь (пасс.)", "digital", "Откр/Закр", "pass_door", 24, "1", 0, "");

            //25. Задняя левая дверь
            string l_rear_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Задняя левая дверь", "digital", "Откр/Закр", "l_rear_door", 25, "1", 0, "");

            //26. Правая задняя дверь
            string r_rear_door_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Правая задняя дверь", "digital", "Откр/Закр", "r_rear_door", 26, "1", 0, "");

            //27. Багажник
            string trunk_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Багажник", "digital", "Откр/Закр", "trunk", 27, "1", 0, "");

            //28. Пробег
            string can_odo_km_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Пробег", "mileage", "КМ", "can_odo_km", 28, "1", 0, "");

            //29. Топливо
            string fuel_lev_l_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Топливо", "fuel level", "л", "fuel_lev_l", 29, "1", 0, "");

            //30. Ручной тормоз
            string hand_break_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Ручной тормоз", "digital", "Вкл/Выкл", "hand_break", 30, "1", 0, "");

            //31. Габаритные огни
            string marker_lights_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Габаритные огни", "digital", "Вкл/Выкл", "marker_lights", 31, "1", 0, "");

            //32. АКПП в P
            string parking_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "АКПП в P", "digital", "Вкл/Выкл", "parking", 32, "1", 0, "");

            //33. Ближний свет
            string dipped_beam_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Ближний свет", "digital", "Вкл/Выкл", "dipped_beam", 33, "1", 0, "");

            //34. Низкий уровень омывающей жидкости
            string washer_alert_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Низкий уровень омывающей жидкости", "digital", "Вкл/Выкл", "washer_alert", 34, "1", 0, "");

            //35. Температура двигателя
            string eng_temp_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Температура двигателя", "temperature", "C", "eng_temp", 35, "1", 0, "");

            //36. Сработка датчика глушения
            string aux_zone_1_sensor = macros.create_sensor_wl(cr_obj_out.item.id, "Сработка датчика глушения", "digital", "Вкл/Выкл", "aux_zone_1", 36, "1", 0, "");

            /////////////////
            ///Создаем произвольные поля
            /////////////////

            //Произвольное поле 1
            string answer = macros.create_custom_field_wl(cr_obj_out.item.id, "0 УВАГА", "");

            //Произвольное поле 2
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "02 Проект", "");

            //Произвольное поле 3
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.1 І Відповідальна особа (основна)", "");

            //Произвольное поле 4
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.2 ІІ Відповідальна особа", "");

            //Произвольное поле 5
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.3 ІІІ Відповідальна особа", "");

            //Произвольное поле 6
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.4 ІV Відповідальна особа", "");

            //Произвольное поле 7
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "2.5 V Відповідальна особа", "");

            //Произвольное поле 8
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.1.1 Оператор, що тестував", "");

            //Произвольное поле 9
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.11 Додатково встановлені сигналізації", "");

            //Произвольное поле 10
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.12 Постановка авто под охрану через багажник?", "");

            //Произвольное поле 11
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.15 Додатково встановлені датчики", "");

            //Произвольное поле 12
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.2.1 Установник: назва, адреса", "");

            //Произвольное поле 13
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.2.2 Установник-монтажник: ПІБ, №тел.", "");

            //Произвольное поле 14
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.3 Дата установки", "");

            //Произвольное поле 15
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.4 Місце установки пристрою ВЕНБЕСТ", "");

            //Произвольное поле 16
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.1 CAN-реле", "");

            //Произвольное поле 17
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.6.2 Звичайне реле", "");

            //Произвольное поле 18
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.7 Місце встановлення сервісної кнопки", "");

            //Произвольное поле 19
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.8.1 Дротова тривожна кнопка", "");

            //Произвольное поле 20
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.8.2 Бездротова тривожна кнопка", "");

            //Произвольное поле 21
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.9.1 Кнопки введення PIN коду: штатні, додатково встановленні", "");

            //Произвольное поле 22
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "3.9.2 Штатні кнопки введення PIN-коду", "");

            //Административное поле 21
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.3 GSM-код", maskedTextBox_GSM_CODE.Text.ToString());

            //Административное поле 22
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.4 PUK-код", maskedTextBox_PUK.Text.ToString());

            //Административное поле 23
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "3.9.5 Bluetuth-код", maskedTextBox_BLE_CODE.Text.ToString());

            //Произвольное поле 24
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.1 Дата активації", "");

            //Произвольное поле 25
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.1.1 Оператор, що активував", "");

            //Произвольное поле 26
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.2 Дата встановлення PIN-коду", "");

            //Произвольное поле 27
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.3 PIN-код встановлено особою(клієнт/установлник)", "");

            //Произвольное поле 28
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "4.4 Обліковий запис WL", "");

            //Произвольное поле 29
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.1 Менеджер", "");

            //Произвольное поле 30
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.2 Договір обслуговування", "");

            //Произвольное поле 31
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.3 Гарантія до", "");

            //Произвольное поле 32
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "5.4 Дата закінчення договору страхування", "");

            //Произвольное поле 33
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "8.1 Паркінг 1", "");

            //Произвольное поле 34
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "8.2 Паркінг 2", "");

            //Произвольное поле 35
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.0 Примітки", "");

            //Произвольное поле 36
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.1 Техпаспорт", "");

            //Произвольное поле 37
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.2.1 Дата перевірки картки", "ДД.ММ.РРРР");

            //Произвольное поле 38
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "9.2.2 Оператор перевірки картки", "Прізвище");

            //Произвольное поле 39
            answer = macros.create_custom_field_wl(cr_obj_out.item.id, "10 Кодове слово", "");

            //Админитстративное поле 40
            answer = macros.create_admin_field_wl(cr_obj_out.item.id, "13 Prizrak 910 SN", search_tovar_comboBox.Text);

            /////////////////
            ///Добавляем в группу Объекты All
            /////////////////

            string get_units_on_group = macros.WialonRequest(
                "&svc=core/search_item&params={"
                + "\"id\":\"2612\","
                + "\"flags\":\"1\"}");//получаем все объекты группы

            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

            list_get_units_on_group.item.u.Add(cr_obj_out.item.id);//Доповляем в список новый объект
            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

            string gr_answer = macros.WialonRequest(
                "&svc=unit_group/update_units&params={"
                + "\"itemId\":\"2612\","
                + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

            /////////////////
            ///Создаем команды
            /////////////////
            ///http://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/unit/update_command_definition
            /// 83886080- закрыта команда  для клиента
            /// 16777216- открыта команда для клиента

            //0 - Запросить текущее состояние
            string cmd_refresh = macros.create_commads_wl(cr_obj_out.item.id, "0 - Запросить текущее состояние", "%23refresh%23", 83886080);

            //0 - Перезагрузить систему
            string cmd_reboot = macros.create_commads_wl(cr_obj_out.item.id, "0 - Перезагрузить систему", "%23reboot%23", 83886080);

            //1 - Закрыть автомобиль
            string cmd_protection_on = macros.create_commads_wl(cr_obj_out.item.id, "1 - Закрыть автомобиль", "%23protection=1%23", 16777216);

            //1 - Открыть автомобиль
            string cmd_protection_off = macros.create_commads_wl(cr_obj_out.item.id, "1 - Открыть автомобиль", "%23protection=0%23", 16777216);

            //2 - Автозапуск старт
            string cmd_autostart_on = macros.create_commads_wl(cr_obj_out.item.id, "2 - Автозапуск старт", "%23autostart=1%23", 83886080);

            //2 - Автозапуск стоп
            string cmd_autostart_off = macros.create_commads_wl(cr_obj_out.item.id, "2 - Автозапуск стоп", "%23autostart=0%23", 83886080);

            //3 - СТАРТ двигатель
            string cmd_man_block_off = macros.create_commads_wl(cr_obj_out.item.id, "3 - СТАРТ двигатель", "%23man_block=0%23", 83886080);

            //3 - СТОП двигатель
            string cmd_man_block_on = macros.create_commads_wl(cr_obj_out.item.id, "3 - СТОП двигатель", "%23man_block=1%23", 83886080);

            //4 - Включить сирену
            string cmd_alarm_on = macros.create_commads_wl(cr_obj_out.item.id, "4 - Включить сирену", "%23alarm=1%23", 16777216);

            //4 - Выключить сирену
            string cmd_alarm_off = macros.create_commads_wl(cr_obj_out.item.id, "4 - Выключить сирену", "%23alarm=0%23", 16777216);

            //5 - Включить поиск на парковке
            string cmd_search_on = macros.create_commads_wl(cr_obj_out.item.id, "5 - Включить поиск на парковке", "%23search=1%23", 16777216);

            //5 - Выключить поиск на парковке
            string cmd_search_off = macros.create_commads_wl(cr_obj_out.item.id, "5 - Выключить поиск на парковке", "%23search=0%23", 16777216);

            //6 - Включить сервисный режим
            string cmd_valet_on = macros.create_commads_wl(cr_obj_out.item.id, "6 - Включить сервисный режим", "%23valet=1%23", 16777216);

            //6 - Выключить сервисный режиме
            string cmd_valet_off = macros.create_commads_wl(cr_obj_out.item.id, "6 - Выключить сервисный режиме", "%23valet=0%23", 16777216);

            string id_sim = "";


            DataTable t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel_select.Text + "';");
            if (t.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(4) + "';");
            }

            string id_sim2 = "";
            DataTable t1 = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel2_select.Text + "';");
            if (t1.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim2 = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim2_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim2 = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim2_no_to_create.Text.Substring(4) + "';");
            }

            string sql2 = string.Format("insert into btk.Object(Object_id_wl, Object_imei, Object_name, Object_sim_no, Object_sim2_no, products_idproducts, Simcard_idSimcard, Simcard_idSimcard1, TS_info_idTS_info, TS_info_TS_brend_model_idTS_brend_model, Kontakti_idKontakti_serviceman, Dogovora_idDogovora, Objectcol_edit_date, Users_idUsers, Objectcol_sn_puk_card, Objectcol_puk, Objectcol_gsm_code, Objectcol_ble_code, Objectcol_bt_enable, Objectcol_sn_prizrak) "
                                        + "values('" + cr_obj_out.item.id
                                        + "', '" + textBox_id_to_create.Text.ToString()
                                        + "', '" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem)
                                        + " " + textBox_id_to_create.Text.ToString() + "','"
                                        + maskedTextBox_sim_no_to_create.Text.ToString() + "','"
                                        + maskedTextBox_sim2_no_to_create.Text.ToString()
                                        + "', '" + comboBox_list_poructs.SelectedValue.ToString() + "', "
                                        + "'" + id_sim + "',"
                                        + "'" + id_sim2 + "'," +
                                        "'1', '1', '1','1', null, '" + vars_form.user_login_id + "', '"
                                        + textBox_id_to_create.Text.ToString() + "', '"
                                        + maskedTextBox_PUK.Text.ToString() + "', '"
                                        + maskedTextBox_GSM_CODE.Text.ToString() + "', '"
                                        + maskedTextBox_BLE_CODE.Text.ToString() + "', '"
                                        + textBox_bt_enable.Text.ToString() + "', '"
                                        + search_tovar_comboBox.GetItemText(search_tovar_comboBox.SelectedItem) + "');");
            macros.sql_command(sql2);

            //получаем айди созданного объекта
            string id_object = macros.sql_command("SELECT MAX(idObject) FROM btk.Object;");

            //Создаем подписку для объекта
            string sql3 = string.Format("INSERT INTO btk.Subscription(products_has_Tarif_idproducts_has_Tarif, idobject) values('41','" + id_object + "');");
            macros.sql_command(sql3);

            // set datetime create
            macros.sql_command("update btk.Object set date_cteate = now() where idobject = '" + id_object + "';");

            //Получаем айди созданной подписки
            string sql4 = macros.sql_command("select max(idSubscr) from btk.Subscription;");

            //привязываем айди созданного объекта с айди созданной подписки
            string sql5 = string.Format("insert into btk.object_subscr (Object_idObject, Subscription_idSubscr) values (" + id_object + "," + sql4 + ");");
            macros.sql_command(sql5);

            ///////////////////////
            //Если все прошло успешно - завечиваем зеленым кнопку и затирает текстбоксы
            //////////////////////////
            button_create_object.Text = "Створено: " + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text + "\n Дата та час: " + DateTime.Now.ToString();
            maskedTextBox_BLE_CODE.Text = "";
            maskedTextBox_GSM_CODE.Text = "";
            textBox_id_to_create.Text = "";
            maskedTextBox_PUK.Text = "";
            maskedTextBox_sim_no_to_create.Text = "";
            textBox_bt_enable.Text = "";
            comboBox_tel_select.Text = "";
            search_tovar_comboBox.Text = "";
            button_create_object.BackColor = Color.Green;
        }

        private void CNTP()
        {
            /////////////////////
            //////Проверяем корректность введенных данных
            ///

            if (textBox_id_to_create.Text.Length <= 3)
            {
                textBox_id_to_create.BackColor = Color.Red;//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
                return;
            }
            if (maskedTextBox_sim_no_to_create.Text.Length <= 11)//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Red;
                return;
            }
            if (maskedTextBox_PUK.Text.Length <= 3)//Если PUK короче 4х символов останавливается и подсвкечиваем желтым
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Yellow;
                DialogResult result = MessageBox.Show(
                    "Вопрос",
                    "PUK Верный?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.No)
                    return;
                maskedTextBox_PUK.BackColor = Color.White;
            }

            // Проверям существует ли данный номер в системе
            string unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли указанный ИМЕЙ в системе
            string unswer2 = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_unique_id\"," +
                                                     "\"propValueMask\":\"" + textBox_id_to_create.Text + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m2 = JsonConvert.DeserializeObject<RootObject>(unswer2);
            if (m2.items.Count > 0)
            {
                MessageBox.Show("Указанный IMEI существует в WL");
                return;
            }

            /////////////////
            ///Создаем объект
            /////////////////
            string cr_obj_in = "&svc=core/create_unit&params={\"creatorId\":\"" + vars_form.wl_user_id + "\",\"name\":\"" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "\",\"hwTypeId\":\"9\",\"dataFlags\":\"1\"}";
            string json = macros.WialonRequest(cr_obj_in);
            var cr_obj_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (cr_obj_out.error != 0)
            {
                string text = macros.Get_wl_text_error(cr_obj_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Создаем пароль доступа к объекту
            /////////////////

            string accsess_pass_answer = macros.WialonRequest(
                "&svc=unit/update_access_password&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"accessPassword\":\"11111\"}");
            var accsess_pass_answer_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (accsess_pass_answer_out.error != 0)
            {
                string text = macros.Get_wl_text_error(accsess_pass_answer_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание

                return;
            }

            /////////////////
            ///Установливаем имя объекта и ID оборудования и
            /////////////////
            string item_id_in =
                "&svc=unit/update_device_type&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id
                + "\",\"deviceTypeId\":\"" + "9"
                + "\",\"uniqueId\":\"" + textBox_id_to_create.Text.ToString() + "\"}";
            string json2 = macros.WialonRequest(item_id_in);
            var item_id_out = JsonConvert.DeserializeObject<RootObject>(json2);
            if (item_id_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_id_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                MessageBox.Show(text);

                return;
            }
            /////////////////
            ///Устанавливаем номер СИМ
            /////////////////
            string item_phone_in = "&svc=unit/update_phone&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim_no_to_create.Text) + "\"}";
            string json3 = macros.WialonRequest(item_phone_in);
            var item_phone_out = JsonConvert.DeserializeObject<RootObject>(json3);
            if (item_phone_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Создаем датчики
            /////////////////

            //1. Статус храны
            string arm_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Статус охраны\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"В охране/Не в охране\","
                + "\"p\":\"par6\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");
            //var arm_answer_out = JsonConvert.DeserializeObject<RootObject>(arm_answer);
            //if (arm_answer_out.error != 0)
            //{
            //    string text = macros.Get_wl_text_error(arm_answer_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
            //    return;
            //}

            //2. Статус двери
            string door_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Статус дверей\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Открыты/Закрыты\","
                + "\"p\":\"par1\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //3. Напряжение АКБ
            string akb_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Напряжение АКБ\","
                + "\"t\":\"voltage\","
                + "\"d\":\"\","
                + "\"m\":\"В\","
                + "\"p\":\"VPWR\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //4. Сработка сигнализации
            string siren_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Сработка сигнализации\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Да/%2D\","
                + "\"p\":\"par154\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"1\","
                + "\"tbl\":[]}");

            //5. Датчик удара
            string d_udara_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Датчик удара\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Да/%2D\","
                + "\"p\":\"par154%2bpar1\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"4\","
                + "\"tbl\":[{\"x\":0,\"a\":0,\"b\":0},{\"x\":1,\"a\":0,\"b\":1},{\"x\":2,\"a\":0,\"b\":0},{\"x\":3,\"a\":0,\"b\":0},{\"x\":4,\"a\":0,\"b\":0},{\"x\":5,\"a\":0,\"b\":0},{\"x\":6,\"a\":0,\"b\":0}]}");

            //6. Напряжение внутреннего АКБ
            string v_akb__answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Напряжение внутреннего АКБ\","
                + "\"t\":\"voltage\","
                + "\"d\":\"\","
                + "\"m\":\"В\","
                + "\"p\":\"VBAT\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //7. Качество связи GSM
            string GMS_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Качество связи GSM\","
                + "\"t\":\"custom\","
                + "\"d\":\"\","
                + "\"m\":\"%25\","
                + "\"p\":\"par21\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[{\"x\":0,\"a\":20,\"b\":0},{\"x\":1,\"a\":20,\"b\":0},{\"x\":2,\"a\":20,\"b\":0},{\"x\":3,\"a\":20,\"b\":0},{\"x\":4,\"a\":20,\"b\":0},{\"x\":5,\"a\":20,\"b\":0}]}");

            //8. Сработка сигнализации: двери
            string siren_door_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Сработка сигнализации: двери\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Сработка/%2D\","
                + "\"p\":\"par1\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"4\","
                + "\"tbl\":[]}");

            //9. Тревожная кнопка
            string tk_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Тревожная кнопка\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Вкл/Выкл\","
                + "\"p\":\"AIN2\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[{\"x\":0,\"a\":0,\"b\":1},{\"x\":5.9,\"a\":0,\"b\":1},{\"x\":6,\"a\":0,\"b\":0},{\"x\":13,\"a\":0,\"b\":0}]}");

            //10. Блокировка иммобилайзера
            string imob_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Блокировка иммобилайзера\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Вкл/Выкл\","
                + "\"p\":\"AIN1\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[{\"x\":0,\"a\":0,\"b\":1},{\"x\":5.9,\"a\":0,\"b\":1},{\"x\":6,\"a\":0,\"b\":0},{\"x\":13,\"a\":0,\"b\":0}]}");

            //11. Зажигание
            string ign_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Зажигание\","
                + "\"t\":\"engine operation\","
                + "\"d\":\"\","
                + "\"m\":\"Вкл/Выкл\","
                + "\"p\":\"VPWR\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[{\"x\":0,\"a\":0,\"b\":0},{\"x\":12,\"a\":0,\"b\":0},{\"x\":12.9,\"a\":0,\"b\":0},{\"x\":13,\"a\":0,\"b\":1},{\"x\":14,\"a\":0,\"b\":1}]}");

            //12. Статус GPS
            string gps_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Статус GPS\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Да/%2D\","
                + "\"p\":\"par69\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            /////////////////
            ///Создаем произвольные поля
            /////////////////

            //Произвольное поле 1
            string pp1_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"0 УВАГА\","
                + "\"v\":\"\"}");

            //Произвольное поле 2
            string pp2_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"02 Проект\","
                + "\"v\":\"\"}");

            //Произвольное поле 3
            string pp3_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.1 І Відповідальна особа (основна)\","
                + "\"v\":\"\"}");

            //Произвольное поле 4
            string pp4_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.2 ІІ Відповідальна особа\","
                + "\"v\":\"\"}");

            //Произвольное поле 5
            string pp5_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.3 ІІІ Відповідальна особа\","
                + "\"v\":\"\"}");

            //Произвольное поле 6
            macros.create_custom_field_wl(cr_obj_out.item.id, "2.4 ІV Відповідальна особа", "");

            //Произвольное поле 7
            macros.create_custom_field_wl(cr_obj_out.item.id, "2.5 V Відповідальна особа", "");

            //Произвольное поле 6
            string pp6_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.10.2 Версія ПЗ Keyless\","
                + "\"v\":\"\"}");

            //Произвольное поле 7
            string pp7_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.1.1 Оператор, що тестував\","
                + "\"v\":\"\"}");

            //Произвольное поле 8
            string pp8_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.11 Додатково встановлені сигналізації\","
                + "\"v\":\"\"}");

            //Произвольное поле 9
            string pp9_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.12 Постановка авто под охрану через багажник?\","
                + "\"v\":\"\"}");

            //Произвольное поле 10
            string pp10_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.15 Додатково встановлені датчики\","
                + "\"v\":\"\"}");

            //Произвольное поле 11
            string pp11_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.2.1 Установник: назва, адреса\","
                + "\"v\":\"\"}");

            //Произвольное поле 12
            string pp12_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.2.2 Установник-монтажник: ПІБ, №тел.\","
                + "\"v\":\"\"}");

            //Произвольное поле 13
            string pp13_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.3 Дата установки\","
                + "\"v\":\"\"}");

            //Произвольное поле 14
            string pp14_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.4 Місце установки пристрою ВЕНБЕСТ\","
                + "\"v\":\"\"}");

            //Произвольное поле 15
            string pp15_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.5 Назва та місце установки сигналізації\","
                + "\"v\":\"\"}");

            //Произвольное поле 16
            string pp16_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.6.1  Реле блокування: місце встановлення\","
                + "\"v\":\"\"}");

            //Произвольное поле 17
            string pp17_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.6.2 Реле блокування: елемент блокування\","
                + "\"v\":\"\"}");

            //Произвольное поле 18
            string pp18_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.7 Місце встановлення сервісної кнопки\","
                + "\"v\":\"\"}");

            //Произвольное поле 19
            string pp19_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.8.1 Дротова тривожна кнопка\","
                + "\"v\":\"\"}");

            //Произвольное поле 20
            string pp20_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.8.2 Бездротова тривожна кнопка\","
                + "\"v\":\"\"}");

            //Произвольное поле 21
            string pp21_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.9.1 Кнопки введення PIN коду: штатні, додатково встановленні\","
                + "\"v\":\"\"}");

            //Произвольное поле 22
            string pp22_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.9.2 Штатні кнопки введення PIN-коду\","
                + "\"v\":\"\"}");

            //Административное поле 23
            string pp23_answer = macros.WialonRequest(
                "&svc=item/update_admin_field&params={"
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"n\":\"3.9.3 PUK-код\","
                + "\"v\":\"" + maskedTextBox_PUK.Text.ToString() + "\"}");

            //Произвольное поле 24
            string pp24_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.1 Дата активації\","
                + "\"v\":\"\"}");

            //Произвольное поле 25
            string pp25_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.1.1 Оператор, що активував\","
                + "\"v\":\"\"}");

            //Произвольное поле 26
            string pp26_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.2 Дата встановлення PIN-коду\","
                + "\"v\":\"\"}");

            //Произвольное поле 27
            string pp27_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.3 PIN-код встановлено особою(клієнт/установлник)\","
                + "\"v\":\"\"}");

            //Произвольное поле 28
            string pp28_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.4 Обліковий запис WL\","
                + "\"v\":\"\"}");

            //Произвольное поле 29
            string pp29_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.1 Менеджер\","
                + "\"v\":\"\"}");

            //Произвольное поле 30
            string pp30_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.2 Договір обслуговування\","
                + "\"v\":\"\"}");

            //Произвольное поле 31
            string pp31_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.3 Гарантія до\","
                + "\"v\":\"\"}");

            //Произвольное поле 32
            string pp32_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"8.1 Паркінг 1\","
                + "\"v\":\"\"}");

            //Произвольное поле 33
            string pp33_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"8.2 Паркінг 2\","
                + "\"v\":\"\"}");

            //Произвольное поле 34
            string pp34_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.0 Примітки\","
                + "\"v\":\"\"}");

            //Произвольное поле 35
            string pp35_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.1 Техпаспорт\","
                + "\"v\":\"\"}");

            //Произвольное поле 36
            string pp36_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.2.1 Дата перевірки картки\","
                + "\"v\":\"\"}");

            //Произвольное поле 37
            string pp37_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.2.2 Оператор перевірки картки\","
                + "\"v\":\"\"}");

            //Произвольное поле 38
            string pp38_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"10 Кодове слово\","
                + "\"v\":\"\"}");

            //Произвольное поле 39
            string pp39_answer = macros.WialonRequest(
                "&svc=item/update_admin_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"11 Серійний номер CNTK (Keyless)\","
                + "\"v\":\"" + textBox_id_to_create.Text.ToString() + "\"}");

            //Произвольное поле 40
            string pp40_answer = macros.WialonRequest(
                "&svc=item/update_admin_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"12 Версия ПО Prizrak\","
                + "\"v\":\"\"}");

            //Произвольное поле 41
            string pp41_answer = macros.WialonRequest(
                "&svc=item/update_admin_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"13 Модель Prizrak\","
                + "\"v\":\"Prizrak 730\"}");

            /////////////////
            ///Добавляем в группу Объекты All
            /////////////////

            string get_units_on_group = macros.WialonRequest(
                "&svc=core/search_item&params={"
                + "\"id\":\"2612\","
                + "\"flags\":\"1\"}");//получаем все объекты группы

            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

            list_get_units_on_group.item.u.Add(cr_obj_out.item.id);//Доповляем в список новый объект
            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

            string gr_answer = macros.WialonRequest(
                "&svc=unit_group/update_units&params={"
                + "\"itemId\":\"2612\","
                + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

            /////////////////
            ///Создаем команды
            /////////////////
            ///

            //Start engine
            string cmd_start_answer = macros.WialonRequest(
                "&svc=unit/update_command_definition&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"1. СТАРТ Двигатель\","
                + "\"c\":\"driver_msg\","
                + "\"l\":\"tcp\","
                + "\"p\":\"TPASS: 081983; setdigout 00;\","
                + "\"a\":\"1\"}");//обновляем в Виалоне группу все объекты + новый

            //Stop engine
            string cmd_stop_answer = macros.WialonRequest(
                "&svc=unit/update_command_definition&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2. СТОП Двигатель\","
                + "\"c\":\"driver_msg\","
                + "\"l\":\"tcp\","
                + "\"p\":\"TPASS: 081983; setdigout 01;\","
                + "\"a\":\"1\"}");//обновляем в Виалоне группу все объекты + новый

            //Service
            string cmd_service_answer = macros.WialonRequest(
                "&svc=unit/update_command_definition&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"Service\","
                + "\"c\":\"driver_msg\","
                + "\"l\":\"tcp\","
                + "\"p\":\"TPASS: 081983;\","
                + "\"a\":\"1\"}");//обновляем в Виалоне группу все объекты + новый

            string id_sim = "";

            DataTable t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel_select.Text + "';");
            if (t.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(4) + "';");
            }

            string sql2 = string.Format("insert into btk.Object(Object_id_wl, Object_imei, Object_name, Object_sim_no, products_idproducts, Simcard_idSimcard, TS_info_idTS_info, TS_info_TS_brend_model_idTS_brend_model, Kontakti_idKontakti_serviceman, Dogovora_idDogovora, Objectcol_edit_date, Users_idUsers, Objectcol_sn_puk_card, Objectcol_puk) "
                + "values('" + cr_obj_out.item.id
                + "', '" + textBox_id_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "','"
                + maskedTextBox_sim_no_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.SelectedValue.ToString() + "', " +
                "'" + id_sim + "'," +
                " '1', '1', '1','1', null, '" + vars_form.user_login_id + "', '" + textBox_id_to_create.Text.ToString() + "', '" + maskedTextBox_PUK.Text.ToString() + "');");
            macros.sql_command(sql2);

            //получаем айди созданного объекта
            string id_object = macros.sql_command("SELECT MAX(idObject) FROM btk.Object;");

            // set datetime create
            macros.sql_command("update btk.Object set date_cteate = now() where idobject = '" + id_object + "';");

            //Создаем подписку для объекта
            string sql3 = string.Format("INSERT INTO btk.Subscription(products_has_Tarif_idproducts_has_Tarif, idobject) values(" + comboBox_list_poructs.SelectedValue.ToString() + "," + id_object + ");");
            macros.sql_command(sql3);

            //Получаем айди созданной подписки
            string sql4 = macros.sql_command("select max(idSubscr) from btk.Subscription;");

            //привязываем айди созданного объекта с айди созданной подписки
            string sql5 = string.Format("insert into btk.object_subscr (Object_idObject, Subscription_idSubscr) values (" + id_object + "," + sql4 + ");");
            macros.sql_command(sql5);

            ///////////////////////
            //Если все прошло успешно - завечиваем зеленым кнопку и затирает текстбоксы
            //////////////////////////
            button_create_object.Text = "Створено: " + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text + "\n Дата та час: " + DateTime.Now.ToString();
            maskedTextBox_BLE_CODE.Text = "";
            maskedTextBox_GSM_CODE.Text = "";
            textBox_id_to_create.Text = "";
            maskedTextBox_PUK.Text = "";
            maskedTextBox_sim_no_to_create.Text = "";
            comboBox_tel_select.Text = "";
            search_tovar_comboBox.Text = "";
            button_create_object.BackColor = Color.Green;
        }

        private void CNTK()
        {
            /////////////////////
            //////Проверяем корректность введенных данных
            ///

            if (textBox_id_to_create.Text.Length <= 3)
            {
                textBox_id_to_create.BackColor = Color.Red;//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
                return;
            }
            if (maskedTextBox_sim_no_to_create.Text.Length <= 11)//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Red;
                return;
            }
            if (maskedTextBox_PUK.Text.Length <= 3)//Если PUK короче 4х символов останавливается и подсвкечиваем желтым
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Yellow;
                DialogResult result = MessageBox.Show(
                    "Вопрос",
                    "PUK Верный?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2,
                    MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.No)
                    return;
                maskedTextBox_PUK.BackColor = Color.White;
            }

            // Проверям существует ли данный номер в системе
            string unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли указанный ИМЕЙ в системе
            string unswer2 = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_unique_id\"," +
                                                     "\"propValueMask\":\"" + textBox_id_to_create.Text + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m2 = JsonConvert.DeserializeObject<RootObject>(unswer2);
            if (m2.items.Count > 0)
            {
                MessageBox.Show("Указанный IMEI существует в WL");
                return;
            }

            /////////////////
            ///Создаем объект
            /////////////////
            string cr_obj_in = "&svc=core/create_unit&params={\"creatorId\":\"" + vars_form.wl_user_id + "\",\"name\":\"" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "\",\"hwTypeId\":\"9\",\"dataFlags\":\"1\"}";
            string json = macros.WialonRequest(cr_obj_in);
            var cr_obj_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (cr_obj_out.error != 0)
            {
                string text = macros.Get_wl_text_error(cr_obj_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Создаем пароль доступа к объекту
            /////////////////

            string accsess_pass_answer = macros.WialonRequest(
                "&svc=unit/update_access_password&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"accessPassword\":\"11111\"}");
            var accsess_pass_answer_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (accsess_pass_answer_out.error != 0)
            {
                string text = macros.Get_wl_text_error(accsess_pass_answer_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Установливаем имя объекта и ID оборудования и
            /////////////////
            string item_id_in =
                "&svc=unit/update_device_type&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id
                + "\",\"deviceTypeId\":\"" + "9"
                + "\",\"uniqueId\":\"" + textBox_id_to_create.Text.ToString() + "\"}";
            string json2 = macros.WialonRequest(item_id_in);
            var item_id_out = JsonConvert.DeserializeObject<RootObject>(json2);
            if (item_id_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_id_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Устанавливаем номер СИМ
            /////////////////
            string item_phone_in = "&svc=unit/update_phone&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim_no_to_create.Text) + "\"}";
            string json3 = macros.WialonRequest(item_phone_in);
            var item_phone_out = JsonConvert.DeserializeObject<RootObject>(json3);
            if (item_phone_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Создаем датчики
            /////////////////

            //1. Статус храны
            string arm_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Статус охраны\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"В охране/Не в охране\","
                + "\"p\":\"par6\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");
            //var arm_answer_out = JsonConvert.DeserializeObject<RootObject>(arm_answer);
            //if (arm_answer_out.error != 0)
            //{
            //    string text = macros.Get_wl_text_error(arm_answer_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
            //    return;
            //}

            //2. Статус двери
            string door_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Статус дверей\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Открыты/Закрыты\","
                + "\"p\":\"par1\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //3. Напряжение АКБ
            string akb_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Напряжение АКБ\","
                + "\"t\":\"voltage\","
                + "\"d\":\"\","
                + "\"m\":\"В\","
                + "\"p\":\"VPWR\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //4. Сработка сигнализации
            string siren_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Сработка сигнализации\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Да/%2D\","
                + "\"p\":\"par154\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"1\","
                + "\"tbl\":[]}");

            //5. Датчик удара
            string d_udara_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Датчик удара\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Да/%2D\","
                + "\"p\":\"par154%2bpar1\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"4\","
                + "\"tbl\":[{\"x\":0,\"a\":0,\"b\":0},{\"x\":1,\"a\":0,\"b\":1},{\"x\":2,\"a\":0,\"b\":0},{\"x\":3,\"a\":0,\"b\":0},{\"x\":4,\"a\":0,\"b\":0},{\"x\":5,\"a\":0,\"b\":0},{\"x\":6,\"a\":0,\"b\":0}]}");

            //6. Напряжение внутреннего АКБ
            string v_akb__answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Напряжение внутреннего АКБ\","
                + "\"t\":\"voltage\","
                + "\"d\":\"\","
                + "\"m\":\"В\","
                + "\"p\":\"VBAT\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //7. Качество связи GSM
            string GMS_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Качество связи GSM\","
                + "\"t\":\"custom\","
                + "\"d\":\"\","
                + "\"m\":\"%25\","
                + "\"p\":\"par21\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[{\"x\":0,\"a\":20,\"b\":0},{\"x\":1,\"a\":20,\"b\":0},{\"x\":2,\"a\":20,\"b\":0},{\"x\":3,\"a\":20,\"b\":0},{\"x\":4,\"a\":20,\"b\":0},{\"x\":5,\"a\":20,\"b\":0}]}");

            //8. Сработка сигнализации: двери
            string siren_door_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Сработка сигнализации: двери\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Сработка/%2D\","
                + "\"p\":\"par1\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"4\","
                + "\"tbl\":[]}");

            //9. Тревожная кнопка
            string tk_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Тревожная кнопка\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Вкл/Выкл\","
                + "\"p\":\"AIN2\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[{\"x\":0,\"a\":0,\"b\":1},{\"x\":5.9,\"a\":0,\"b\":1},{\"x\":6,\"a\":0,\"b\":0},{\"x\":13,\"a\":0,\"b\":0}]}");

            //10. Блокировка иммобилайзера
            string imob_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Блокировка иммобилайзера\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Вкл/Выкл\","
                + "\"p\":\"AIN1\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[{\"x\":0,\"a\":0,\"b\":1},{\"x\":5.9,\"a\":0,\"b\":1},{\"x\":6,\"a\":0,\"b\":0},{\"x\":13,\"a\":0,\"b\":0}]}");

            //11. Зажигание
            string ign_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Зажигание\","
                + "\"t\":\"engine operation\","
                + "\"d\":\"\","
                + "\"m\":\"Вкл/Выкл\","
                + "\"p\":\"VPWR\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[{\"x\":0,\"a\":0,\"b\":0},{\"x\":12,\"a\":0,\"b\":0},{\"x\":12.9,\"a\":0,\"b\":0},{\"x\":13,\"a\":0,\"b\":1},{\"x\":14,\"a\":0,\"b\":1}]}");
            //12. Статус GPS
            string gps_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Статус GPS\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Да/%2D\","
                + "\"p\":\"par69\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            /////////////////
            ///Создаем произвольные поля
            /////////////////

            //Произвольное поле 1
            string pp1_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"0 УВАГА\","
                + "\"v\":\"\"}");

            //Произвольное поле 2
            string pp2_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"02 Проект\","
                + "\"v\":\"\"}");

            //Произвольное поле 3
            string pp3_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.1 І Відповідальна особа (основна)\","
                + "\"v\":\"\"}");

            //Произвольное поле 4
            string pp4_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.2 ІІ Відповідальна особа\","
                + "\"v\":\"\"}");

            //Произвольное поле 5
            string pp5_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.3 ІІІ Відповідальна особа\","
                + "\"v\":\"\"}");


            //Произвольное поле 6
            macros.create_custom_field_wl(cr_obj_out.item.id, "2.4 ІV Відповідальна особа", "");

            //Произвольное поле 7
            macros.create_custom_field_wl(cr_obj_out.item.id, "2.5 V Відповідальна особа", "");


            //Произвольное поле 6
            string pp6_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.10.2 Версія ПЗ Keyless\","
                + "\"v\":\"\"}");

            //Произвольное поле 7
            string pp7_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.1.1 Оператор, що тестував\","
                + "\"v\":\"\"}");

            //Произвольное поле 8
            string pp8_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.11 Додатково встановлені сигналізації\","
                + "\"v\":\"\"}");

            //Произвольное поле 9
            string pp9_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.12 Постановка авто под охрану через багажник?\","
                + "\"v\":\"\"}");

            //Произвольное поле 10
            string pp10_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.15 Додатково встановлені датчики\","
                + "\"v\":\"\"}");

            //Произвольное поле 11
            string pp11_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.2.1 Установник: назва, адреса\","
                + "\"v\":\"\"}");

            //Произвольное поле 12
            string pp12_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.2.2 Установник-монтажник: ПІБ, №тел.\","
                + "\"v\":\"\"}");

            //Произвольное поле 13
            string pp13_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.3 Дата установки\","
                + "\"v\":\"\"}");

            //Произвольное поле 14
            string pp14_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.4 Місце установки пристрою ВЕНБЕСТ\","
                + "\"v\":\"\"}");

            //Произвольное поле 15
            string pp15_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.5 Назва та місце установки сигналізації\","
                + "\"v\":\"\"}");

            //Произвольное поле 16
            string pp16_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.6.1  Реле блокування: місце встановлення\","
                + "\"v\":\"\"}");

            //Произвольное поле 17
            string pp17_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.6.2 Реле блокування: елемент блокування\","
                + "\"v\":\"\"}");

            //Произвольное поле 18
            string pp18_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.7 Місце встановлення сервісної кнопки\","
                + "\"v\":\"\"}");

            //Произвольное поле 19
            string pp19_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.8.1 Дротова тривожна кнопка\","
                + "\"v\":\"\"}");

            //Произвольное поле 20
            string pp20_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.8.2 Бездротова тривожна кнопка\","
                + "\"v\":\"\"}");

            //Произвольное поле 21
            string pp21_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.9.1 Кнопки введення PIN коду: штатні, додатково встановленні\","
                + "\"v\":\"\"}");

            //Произвольное поле 22
            string pp22_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.9.2 Штатні кнопки введення PIN-коду\","
                + "\"v\":\"\"}");

            //Административное поле 23
            string pp23_answer = macros.WialonRequest(
                "&svc=item/update_admin_field&params={"
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"n\":\"3.9.3 PUK-код\","
                + "\"v\":\"" + maskedTextBox_PUK.Text.ToString() + "\"}");

            //Произвольное поле 24
            string pp24_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.1 Дата активації\","
                + "\"v\":\"\"}");

            //Произвольное поле 25
            string pp25_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.1.1 Оператор, що активував\","
                + "\"v\":\"\"}");

            //Произвольное поле 26
            string pp26_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.2 Дата встановлення PIN-коду\","
                + "\"v\":\"\"}");

            //Произвольное поле 27
            string pp27_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.3 PIN-код встановлено особою(клієнт/установлник)\","
                + "\"v\":\"\"}");

            //Произвольное поле 28
            string pp28_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.4 Обліковий запис WL\","
                + "\"v\":\"\"}");

            //Произвольное поле 29
            string pp29_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.1 Менеджер\","
                + "\"v\":\"\"}");

            //Произвольное поле 30
            string pp30_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.2 Договір обслуговування\","
                + "\"v\":\"\"}");

            //Произвольное поле 31
            string pp31_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.3 Гарантія до\","
                + "\"v\":\"\"}");

            //Произвольное поле 32
            string pp32_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"8.1 Паркінг 1\","
                + "\"v\":\"\"}");

            //Произвольное поле 33
            string pp33_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"8.2 Паркінг 2\","
                + "\"v\":\"\"}");

            //Произвольное поле 34
            string pp34_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.0 Примітки\","
                + "\"v\":\"\"}");

            //Произвольное поле 35
            string pp35_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.1 Техпаспорт\","
                + "\"v\":\"\"}");

            //Произвольное поле 36
            string pp36_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.2.1 Дата перевірки картки\","
                + "\"v\":\"\"}");

            //Произвольное поле 37
            string pp37_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.2.2 Оператор перевірки картки\","
                + "\"v\":\"\"}");

            //Произвольное поле 38
            string pp38_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"10 Кодове слово\","
                + "\"v\":\"\"}");

            //Произвольное поле 39
            string pp39_answer = macros.WialonRequest(
                "&svc=item/update_admin_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"11 Серійний номер CNTK (Keyless)\","
                + "\"v\":\"" + textBox_id_to_create.Text.ToString() + "\"}");

            //Произвольное поле 40
            string pp40_answer = macros.WialonRequest(
                "&svc=item/update_admin_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"12 Версия ПО Prizrak\","
                + "\"v\":\"\"}");

            //Произвольное поле 41
            string pp41_answer = macros.WialonRequest(
                "&svc=item/update_admin_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"13 Модель Prizrak\","
                + "\"v\":\"Prizrak 710\"}");

            /////////////////
            ///Добавляем в группу Объекты All
            /////////////////

            string get_units_on_group = macros.WialonRequest(
                "&svc=core/search_item&params={"
                + "\"id\":\"2612\","
                + "\"flags\":\"1\"}");//получаем все объекты группы

            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

            list_get_units_on_group.item.u.Add(cr_obj_out.item.id);//Доповляем в список новый объект
            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

            string gr_answer = macros.WialonRequest(
                "&svc=unit_group/update_units&params={"
                + "\"itemId\":\"2612\","
                + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

            /////////////////
            ///Создаем команды
            /////////////////
            ///

            //Start engine
            string cmd_start_answer = macros.WialonRequest(
                "&svc=unit/update_command_definition&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"1. СТАРТ Двигатель\","
                + "\"c\":\"driver_msg\","
                + "\"l\":\"tcp\","
                + "\"p\":\"TPASS: 081983; setdigout 00;\","
                + "\"a\":\"1\"}");//обновляем в Виалоне группу все объекты + новый

            //Stop engine
            string cmd_stop_answer = macros.WialonRequest(
                "&svc=unit/update_command_definition&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2. СТОП Двигатель\","
                + "\"c\":\"driver_msg\","
                + "\"l\":\"tcp\","
                + "\"p\":\"TPASS: 081983; setdigout 01;\","
                + "\"a\":\"1\"}");//обновляем в Виалоне группу все объекты + новый

            //Service
            string cmd_service_answer = macros.WialonRequest(
                "&svc=unit/update_command_definition&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"Service\","
                + "\"c\":\"driver_msg\","
                + "\"l\":\"tcp\","
                + "\"p\":\"TPASS: 081983;\","
                + "\"a\":\"1\"}");//обновляем в Виалоне группу все объекты + новый

            string id_sim = "";

            DataTable t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel_select.Text + "';");
            if (t.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(4) + "';");
            }

            string sql2 = string.Format("insert into btk.Object(Object_id_wl, Object_imei, Object_name, Object_sim_no, products_idproducts, Simcard_idSimcard, TS_info_idTS_info, TS_info_TS_brend_model_idTS_brend_model, Kontakti_idKontakti_serviceman, Dogovora_idDogovora, Objectcol_edit_date, Users_idUsers, Objectcol_sn_puk_card, Objectcol_puk) "
                + "values('" + cr_obj_out.item.id
                + "', '" + textBox_id_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "','"
                + maskedTextBox_sim_no_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.SelectedValue.ToString() + "', " +
                "'" + id_sim + "'," +
                " '1', '1', '1','1', null, '" + vars_form.user_login_id + "', '" + textBox_id_to_create.Text.ToString() + "', '" + maskedTextBox_PUK.Text.ToString() + "');");
            macros.sql_command(sql2);

            //получаем айди созданного объекта
            string id_object = macros.sql_command("SELECT MAX(idObject) FROM btk.Object;");

            // set datetime create
            macros.sql_command("update btk.Object set date_cteate = now() where idobject = '" + id_object + "';");

            //Создаем подписку для объекта
            string sql3 = string.Format("INSERT INTO btk.Subscription(products_has_Tarif_idproducts_has_Tarif, idobject) values(" + comboBox_list_poructs.SelectedValue.ToString() + "," + id_object + ");");
            macros.sql_command(sql3);

            //Получаем айди созданной подписки
            string sql4 = macros.sql_command("select max(idSubscr) from btk.Subscription;");

            //привязываем айди созданного объекта с айди созданной подписки
            string sql5 = string.Format("insert into btk.object_subscr (Object_idObject, Subscription_idSubscr) values (" + id_object + "," + sql4 + ");");
            macros.sql_command(sql5);

            ///////////////////////
            //Если все прошло успешно - завечиваем зеленым кнопку и затирает текстбоксы
            //////////////////////////
            button_create_object.Text = "Створено: " + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text + "\n Дата та час: " + DateTime.Now.ToString();
            maskedTextBox_BLE_CODE.Text = "";
            maskedTextBox_GSM_CODE.Text = "";
            textBox_id_to_create.Text = "";
            maskedTextBox_PUK.Text = "";
            maskedTextBox_sim_no_to_create.Text = "";
            comboBox_tel_select.Text = "";
            search_tovar_comboBox.Text = "";
            button_create_object.BackColor = Color.Green;
        }

        private void C_N()
        {
            /////////////////////
            //////Проверяем корректность введенных данных
            ///

            if (textBox_id_to_create.Text.Length <= 3)
            {
                textBox_id_to_create.BackColor = Color.Red;//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
                return;
            }
            if (maskedTextBox_sim_no_to_create.Text.Length <= 11)//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Red;
                return;
            }

            // Проверям существует ли данный номер в системе
            string unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли указанный ИМЕЙ в системе
            string unswer2 = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_unique_id\"," +
                                                     "\"propValueMask\":\"" + textBox_id_to_create.Text + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m2 = JsonConvert.DeserializeObject<RootObject>(unswer2);
            if (m2.items.Count > 0)
            {
                MessageBox.Show("Указанный IMEI существует в WL");
                return;
            }

            /////////////////
            ///Создаем объект
            /////////////////
            string cr_obj_in = "&svc=core/create_unit&params={\"creatorId\":\"" + vars_form.wl_user_id + "\",\"name\":\"" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "\",\"hwTypeId\":\"9\",\"dataFlags\":\"1\"}";
            string json = macros.WialonRequest(cr_obj_in);
            var cr_obj_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (cr_obj_out.error != 0)
            {
                string text = macros.Get_wl_text_error(cr_obj_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Создаем пароль доступа к объекту
            /////////////////

            string accsess_pass_answer = macros.WialonRequest(
                "&svc=unit/update_access_password&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"accessPassword\":\"11111\"}");
            var accsess_pass_answer_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (accsess_pass_answer_out.error != 0)
            {
                string text = macros.Get_wl_text_error(accsess_pass_answer_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Установливаем имя объекта и ID оборудования и
            /////////////////
            string item_id_in =
                "&svc=unit/update_device_type&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id
                + "\",\"deviceTypeId\":\"" + "9"
                + "\",\"uniqueId\":\"" + textBox_id_to_create.Text.ToString() + "\"}";
            string json2 = macros.WialonRequest(item_id_in);
            var item_id_out = JsonConvert.DeserializeObject<RootObject>(json2);
            if (item_id_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_id_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Устанавливаем номер СИМ
            /////////////////
            string item_phone_in = "&svc=unit/update_phone&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim_no_to_create.Text) + "\"}";
            string json3 = macros.WialonRequest(item_phone_in);
            var item_phone_out = JsonConvert.DeserializeObject<RootObject>(json3);
            if (item_phone_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Создаем датчики
            /////////////////

            //1. Статус храны
            string arm_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Статус охраны\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"В охране/Не в охране\","
                + "\"p\":\"par6\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");
            //var arm_answer_out = JsonConvert.DeserializeObject<RootObject>(arm_answer);
            //if (arm_answer_out.error != 0)
            //{
            //    string text = macros.Get_wl_text_error(arm_answer_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
            //    return;
            //}

            //2. Статус двери
            string door_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Статус дверей\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Открыты/Закрыты\","
                + "\"p\":\"par1\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //3. Напряжение АКБ
            string akb_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Напряжение АКБ\","
                + "\"t\":\"voltage\","
                + "\"d\":\"\","
                + "\"m\":\"В\","
                + "\"p\":\"VPWR\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //4. Сработка сигнализации
            string siren_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Сработка сигнализации\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Да/%2D\","
                + "\"p\":\"par154\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"1\","
                + "\"tbl\":[]}");

            //5. Датчик удара
            string d_udara_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Датчик удара\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Да/%2D\","
                + "\"p\":\"par154%2bpar1\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"4\","
                + "\"tbl\":[{\"x\":0,\"a\":0,\"b\":0},{\"x\":1,\"a\":0,\"b\":1},{\"x\":2,\"a\":0,\"b\":0},{\"x\":3,\"a\":0,\"b\":0},{\"x\":4,\"a\":0,\"b\":0},{\"x\":5,\"a\":0,\"b\":0},{\"x\":6,\"a\":0,\"b\":0}]}");

            //6. Напряжение внутреннего АКБ
            string v_akb__answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Напряжение внутреннего АКБ\","
                + "\"t\":\"voltage\","
                + "\"d\":\"\","
                + "\"m\":\"В\","
                + "\"p\":\"VBAT\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //7. Качество связи GSM
            string GMS_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Качество связи GSM\","
                + "\"t\":\"custom\","
                + "\"d\":\"\","
                + "\"m\":\"%25\","
                + "\"p\":\"par21\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[{\"x\":0,\"a\":20,\"b\":0},{\"x\":1,\"a\":20,\"b\":0},{\"x\":2,\"a\":20,\"b\":0},{\"x\":3,\"a\":20,\"b\":0},{\"x\":4,\"a\":20,\"b\":0},{\"x\":5,\"a\":20,\"b\":0}]}");

            //8. Сработка сигнализации: двери
            string siren_door_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Сработка сигнализации: двери\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Сработка/%2D\","
                + "\"p\":\"par1\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"4\","
                + "\"tbl\":[]}");

            //9. Тревожная кнопка
            string tk_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Тревожная кнопка\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Вкл/Выкл\","
                + "\"p\":\"AIN2\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[{\"x\":0,\"a\":0,\"b\":1},{\"x\":5.9,\"a\":0,\"b\":1},{\"x\":6,\"a\":0,\"b\":0},{\"x\":13,\"a\":0,\"b\":0}]}");

            //10. Блокировка иммобилайзера
            string imob_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Блокировка иммобилайзера\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Вкл/Выкл\","
                + "\"p\":\"AIN1\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[{\"x\":0,\"a\":0,\"b\":1},{\"x\":5.9,\"a\":0,\"b\":1},{\"x\":6,\"a\":0,\"b\":0},{\"x\":13,\"a\":0,\"b\":0}]}");

            //11. Зажигание
            string ign_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Зажигание\","
                + "\"t\":\"engine operation\","
                + "\"d\":\"\","
                + "\"m\":\"Вкл/Выкл\","
                + "\"p\":\"VPWR\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[{\"x\":0,\"a\":0,\"b\":0},{\"x\":12,\"a\":0,\"b\":0},{\"x\":12.9,\"a\":0,\"b\":0},{\"x\":13,\"a\":0,\"b\":1},{\"x\":14,\"a\":0,\"b\":1}]}");
            //12. Статус GPS
            string gps_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Статус GPS\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Да/%2D\","
                + "\"p\":\"par69\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            /////////////////
            ///Создаем произвольные поля
            /////////////////

            //Произвольное поле 1
            string pp1_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"0 УВАГА\","
                + "\"v\":\"\"}");

            //Произвольное поле 2
            string pp2_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"02 Проект\","
                + "\"v\":\"\"}");

            //Произвольное поле 3
            string pp3_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.1 І Відповідальна особа (основна)\","
                + "\"v\":\"\"}");

            //Произвольное поле 4
            string pp4_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.2 ІІ Відповідальна особа\","
                + "\"v\":\"\"}");

            //Произвольное поле 5
            string pp5_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.3 ІІІ Відповідальна особа\","
                + "\"v\":\"\"}");


            //Произвольное поле 6
            macros.create_custom_field_wl(cr_obj_out.item.id, "2.4 ІV Відповідальна особа", "");

            //Произвольное поле 7
            macros.create_custom_field_wl(cr_obj_out.item.id, "2.5 V Відповідальна особа", "");

            //Произвольное поле 6
            string pp6_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.10.2 Версія ПЗ Keyless\","
                + "\"v\":\"\"}");

            //Произвольное поле 7
            string pp7_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.1.1 Оператор, що тестував\","
                + "\"v\":\"\"}");

            //Произвольное поле 8
            string pp8_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.11 Додатково встановлені сигналізації\","
                + "\"v\":\"\"}");

            //Произвольное поле 9
            string pp9_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.12 Постановка авто под охрану через багажник?\","
                + "\"v\":\"\"}");

            //Произвольное поле 10
            string pp10_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.15 Додатково встановлені датчики\","
                + "\"v\":\"\"}");

            //Произвольное поле 11
            string pp11_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.2.1 Установник: назва, адреса\","
                + "\"v\":\"\"}");

            //Произвольное поле 12
            string pp12_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.2.2 Установник-монтажник: ПІБ, №тел.\","
                + "\"v\":\"\"}");

            //Произвольное поле 13
            string pp13_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.3 Дата установки\","
                + "\"v\":\"\"}");

            //Произвольное поле 14
            string pp14_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.4 Місце установки пристрою ВЕНБЕСТ\","
                + "\"v\":\"\"}");

            //Произвольное поле 15
            string pp15_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.5 Назва та місце установки сигналізації\","
                + "\"v\":\"\"}");

            //Произвольное поле 16
            string pp16_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.6.1  Реле блокування: місце встановлення\","
                + "\"v\":\"\"}");

            //Произвольное поле 17
            string pp17_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.6.2 Реле блокування: елемент блокування\","
                + "\"v\":\"\"}");

            //Произвольное поле 18
            string pp18_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.7 Місце встановлення сервісної кнопки\","
                + "\"v\":\"\"}");

            //Произвольное поле 19
            string pp19_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.8.1 Дротова тривожна кнопка\","
                + "\"v\":\"\"}");

            //Произвольное поле 20
            string pp20_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.8.2 Бездротова тривожна кнопка\","
                + "\"v\":\"\"}");

            //Произвольное поле 21
            string pp21_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.9.1 Кнопки введення PIN коду: штатні, додатково встановленні\","
                + "\"v\":\"\"}");

            //Произвольное поле 22
            string pp22_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.9.2 Штатні кнопки введення PIN-коду\","
                + "\"v\":\"\"}");

            //Административное поле 23
            string pp23_answer = macros.WialonRequest(
                "&svc=item/update_admin_field&params={"
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"n\":\"3.9.3 PUK-код\","
                + "\"v\":\"" + maskedTextBox_PUK.Text.ToString() + "\"}");

            //Произвольное поле 24
            string pp24_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.1 Дата активації\","
                + "\"v\":\"\"}");

            //Произвольное поле 25
            string pp25_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.1.1 Оператор, що активував\","
                + "\"v\":\"\"}");

            //Произвольное поле 26
            string pp26_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.2 Дата встановлення PIN-коду\","
                + "\"v\":\"\"}");

            //Произвольное поле 27
            string pp27_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.3 PIN-код встановлено особою(клієнт/установлник)\","
                + "\"v\":\"\"}");

            //Произвольное поле 28
            string pp28_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.4 Обліковий запис WL\","
                + "\"v\":\"\"}");

            //Произвольное поле 29
            string pp29_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.1 Менеджер\","
                + "\"v\":\"\"}");

            //Произвольное поле 30
            string pp30_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.2 Договір обслуговування\","
                + "\"v\":\"\"}");

            //Произвольное поле 31
            string pp31_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.3 Гарантія до\","
                + "\"v\":\"\"}");

            //Произвольное поле 32
            string pp32_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"8.1 Паркінг 1\","
                + "\"v\":\"\"}");

            //Произвольное поле 33
            string pp33_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"8.2 Паркінг 2\","
                + "\"v\":\"\"}");

            //Произвольное поле 34
            string pp34_answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.0 Примітки\","
                + "\"v\":\"\"}");

            //Произвольное поле 35
            string pp35_answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.1 Техпаспорт\","
                + "\"v\":\"\"}");

            //Произвольное поле 36
            string pp36_answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.2.1 Дата перевірки картки\","
                + "\"v\":\"\"}");

            //Произвольное поле 37
            string pp37_answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.2.2 Оператор перевірки картки\","
                + "\"v\":\"\"}");

            //Произвольное поле 38
            string pp38_answer = macros.WialonRequest("&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"10 Кодове слово\","
                + "\"v\":\"\"}");

            //Произвольное поле 39
            string pp39_answer = macros.WialonRequest("&svc=item/update_admin_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"11 Серійний номер CNTK (Keyless)\","
                + "\"v\":\"" + textBox_id_to_create.Text.ToString() + "\"}");

            //Произвольное поле 40
            string pp40_answer = macros.WialonRequest("&svc=item/update_admin_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"12 Версия ПО Prizrak\","
                + "\"v\":\"\"}");

            //Произвольное поле 41
            string pp41_answer = macros.WialonRequest("&svc=item/update_admin_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"13 Модель Prizrak\","
                + "\"v\":\"Prizrak 710\"}");

            /////////////////
            ///Добавляем в группу Объекты All
            /////////////////

            string get_units_on_group = macros.WialonRequest("&svc=core/search_item&params={"
                + "\"id\":\"2612\","
                + "\"flags\":\"1\"}");//получаем все объекты группы

            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

            list_get_units_on_group.item.u.Add(cr_obj_out.item.id);//Доповляем в список новый объект
            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

            string gr_answer = macros.WialonRequest("&svc=unit_group/update_units&params={"
                + "\"itemId\":\"2612\","
                + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

            /////////////////
            ///Создаем команды
            /////////////////
            ///

            //Start engine
            string cmd_start_answer = macros.WialonRequest("&svc=unit/update_command_definition&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"1. СТАРТ Двигатель\","
                + "\"c\":\"driver_msg\","
                + "\"l\":\"tcp\","
                + "\"p\":\"TPASS: 081983; setdigout 00;\","
                + "\"a\":\"1\"}");//обновляем в Виалоне группу все объекты + новый

            //Stop engine
            string cmd_stop_answer = macros.WialonRequest("&svc=unit/update_command_definition&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2. СТОП Двигатель\","
                + "\"c\":\"driver_msg\","
                + "\"l\":\"tcp\","
                + "\"p\":\"TPASS: 081983; setdigout 01;\","
                + "\"a\":\"1\"}");//обновляем в Виалоне группу все объекты + новый

            //Service
            string cmd_service_answer = macros.WialonRequest("&svc=unit/update_command_definition&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"Service\","
                + "\"c\":\"driver_msg\","
                + "\"l\":\"tcp\","
                + "\"p\":\"TPASS: 081983;\","
                + "\"a\":\"1\"}");//обновляем в Виалоне группу все объекты + новый

            string id_sim = "";

            DataTable t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel_select.Text + "';");
            if (t.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(4) + "';");
            }

            string sql2 = string.Format("insert into btk.Object(Object_id_wl, Object_imei, Object_name, Object_sim_no, products_idproducts, Simcard_idSimcard, TS_info_idTS_info, TS_info_TS_brend_model_idTS_brend_model, Kontakti_idKontakti_serviceman, Dogovora_idDogovora, Objectcol_edit_date, Users_idUsers) "
                + "values('" + cr_obj_out.item.id
                + "', '" + textBox_id_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "','"
                + maskedTextBox_sim_no_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.SelectedValue.ToString() + "', " +
                "'" + id_sim + "'," +
                "'1', '1', '1','1', null, '" + vars_form.user_login_id + "');");
            macros.sql_command(sql2);

            //получаем айди созданного объекта
            string id_object = macros.sql_command("SELECT MAX(idObject) FROM btk.Object;");

            // set datetime create
            macros.sql_command("update btk.Object set date_cteate = now() where idobject = '" + id_object + "';");

            //Создаем подписку для объекта
            string sql3 = string.Format("INSERT INTO btk.Subscription(products_has_Tarif_idproducts_has_Tarif, idobject) values(" + comboBox_list_poructs.SelectedValue.ToString() + "," + id_object + ");");
            macros.sql_command(sql3);

            //Получаем айди созданной подписки
            string sql4 = macros.sql_command("select max(idSubscr) from btk.Subscription;");

            //привязываем айди созданного объекта с айди созданной подписки
            string sql5 = string.Format("insert into btk.object_subscr (Object_idObject, Subscription_idSubscr) values (" + id_object + "," + sql4 + ");");
            macros.sql_command(sql5);

            ///////////////////////
            //Если все прошло успешно - завечиваем зеленым кнопку и затирает текстбоксы
            //////////////////////////
            button_create_object.Text = "Створено: " + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text + "\n Дата та час: " + DateTime.Now.ToString();
            maskedTextBox_BLE_CODE.Text = "";
            maskedTextBox_GSM_CODE.Text = "";
            textBox_id_to_create.Text = "";
            maskedTextBox_PUK.Text = "";
            maskedTextBox_sim_no_to_create.Text = "";
            comboBox_tel_select.Text = "";
            search_tovar_comboBox.Text = "";
            button_create_object.BackColor = Color.Green;
        }

        private void K_n()
        {
            /////////////////////
            //////Проверяем корректность введенных данных
            ///

            if (textBox_id_to_create.Text.Length <= 3)
            {
                textBox_id_to_create.BackColor = Color.Red;//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
                return;
            }
            if (maskedTextBox_sim_no_to_create.Text.Length <= 11)//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Red;
                return;
            }

            // Проверям существует ли данный номер в системе
            string unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли указанный ИМЕЙ в системе
            string unswer2 = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_unique_id\"," +
                                                     "\"propValueMask\":\"" + textBox_id_to_create.Text + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m2 = JsonConvert.DeserializeObject<RootObject>(unswer2);
            if (m2.items.Count > 0)
            {
                MessageBox.Show("Указанный IMEI существует в WL");
                return;
            }

            /////////////////
            ///Создаем объект
            /////////////////
            string cr_obj_in = "&svc=core/create_unit&params={\"creatorId\":\"" + vars_form.wl_user_id + "\",\"name\":\"" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "\",\"hwTypeId\":\"9\",\"dataFlags\":\"1\"}";
            string json = macros.WialonRequest(cr_obj_in);
            var cr_obj_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (cr_obj_out.error != 0)
            {
                string text = macros.Get_wl_text_error(cr_obj_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Создаем пароль доступа к объекту
            /////////////////

            string accsess_pass_answer = macros.WialonRequest("&svc=unit/update_access_password&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"accessPassword\":\"11111\"}");
            var accsess_pass_answer_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (accsess_pass_answer_out.error != 0)
            {
                string text = macros.Get_wl_text_error(accsess_pass_answer_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Установливаем имя объекта и ID оборудования и
            /////////////////
            string item_id_in = "&svc=unit/update_device_type&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id
                + "\",\"deviceTypeId\":\"" + "9"
                + "\",\"uniqueId\":\"" + textBox_id_to_create.Text.ToString() + "\"}";
            string json2 = macros.WialonRequest(item_id_in);
            var item_id_out = JsonConvert.DeserializeObject<RootObject>(json2);
            if (item_id_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_id_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Устанавливаем номер СИМ
            /////////////////
            string item_phone_in = "&svc=unit/update_phone&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim_no_to_create.Text) + "\"}";
            string json3 = macros.WialonRequest(item_phone_in);
            var item_phone_out = JsonConvert.DeserializeObject<RootObject>(json3);
            if (item_phone_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Создаем датчики
            /////////////////

            //3. Напряжение АКБ
            string akb_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Напряжение АКБ\","
                + "\"t\":\"voltage\","
                + "\"d\":\"\","
                + "\"m\":\"В\","
                + "\"p\":\"VPWR\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //6. Напряжение внутреннего АКБ
            string v_akb__answer = macros.WialonRequest("&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Напряжение внутреннего АКБ\","
                + "\"t\":\"voltage\","
                + "\"d\":\"\","
                + "\"m\":\"В\","
                + "\"p\":\"VBAT\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //7. Качество связи GSM
            string GMS_answer = macros.WialonRequest("&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Качество связи GSM\","
                + "\"t\":\"custom\","
                + "\"d\":\"\","
                + "\"m\":\"%25\","
                + "\"p\":\"par21\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[{\"x\":0,\"a\":20,\"b\":0},{\"x\":1,\"a\":20,\"b\":0},{\"x\":2,\"a\":20,\"b\":0},{\"x\":3,\"a\":20,\"b\":0},{\"x\":4,\"a\":20,\"b\":0},{\"x\":5,\"a\":20,\"b\":0}]}");


            //11. Зажигание
            string ign_answer = macros.WialonRequest(
                 "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Зажигание\","
                + "\"t\":\"engine operation\","
                + "\"d\":\"\","
                + "\"m\":\"Вкл/Выкл\","
                + "\"p\":\"par5\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");
            //12. Статус GPS
            string gps_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Статус GPS\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Да/%2D\","
                + "\"p\":\"par69\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            /////////////////
            ///Создаем произвольные поля
            /////////////////

            //Произвольное поле 1
            string pp1_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"0 УВАГА\","
                + "\"v\":\"\"}");

            //Произвольное поле 2
            string pp2_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"02 Проект\","
                + "\"v\":\"\"}");

            //Произвольное поле 3
            string pp3_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.1 І Відповідальна особа (основна)\","
                + "\"v\":\"\"}");

            //Произвольное поле 4
            string pp4_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.2 ІІ Відповідальна особа\","
                + "\"v\":\"\"}");

            //Произвольное поле 5
            string pp5_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.3 ІІІ Відповідальна особа\","
                + "\"v\":\"\"}");

            //Произвольное поле 6
            macros.create_custom_field_wl(cr_obj_out.item.id, "2.4 ІV Відповідальна особа", "");

            //Произвольное поле 7
            macros.create_custom_field_wl(cr_obj_out.item.id, "2.5 V Відповідальна особа", "");


            //Произвольное поле 6
            string pp7_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.1.1 Оператор, що тестував\","
                + "\"v\":\"\"}");

            //Произвольное поле 7
            string pp11_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.2.1 Установник: назва, адреса\","
                + "\"v\":\"\"}");

            //Произвольное поле 8
            string pp12_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.2.2 Установник-монтажник: ПІБ, №тел.\","
                + "\"v\":\"\"}");

            //Произвольное поле 9
            string pp13_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.3 Дата установки\","
                + "\"v\":\"\"}");

            //Произвольное поле 10
            string pp14_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.4 Місце установки пристрою ВЕНБЕСТ\","
                + "\"v\":\"\"}");

            //Произвольное поле 11
            string pp24_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.1 Дата активації\","
                + "\"v\":\"\"}");

            //Произвольное поле 12
            string pp25_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.1.1 Оператор, що активував\","
                + "\"v\":\"\"}");

            //Произвольное поле 13
            string pp28_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.4 Обліковий запис WL\","
                + "\"v\":\"\"}");

            //Произвольное поле 14
            string pp29_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.1 Менеджер\","
                + "\"v\":\"\"}");

            //Произвольное поле 15
            string pp30_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.2 Договір обслуговування\","
                + "\"v\":\"\"}");

            //Произвольное поле 16
            string pp31_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.3 Гарантія до\","
                + "\"v\":\"\"}");

            //Произвольное поле 17
            string pp32_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"8.1 Паркінг 1\","
                + "\"v\":\"\"}");

            //Произвольное поле 18
            string pp33_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"8.2 Паркінг 2\","
                + "\"v\":\"\"}");

            //Произвольное поле 19
            string pp34_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.0 Примітки\","
                + "\"v\":\"\"}");

            //Произвольное поле 20
            string pp35_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.1 Техпаспорт\","
                + "\"v\":\"\"}");

            //Произвольное поле 21
            string pp36_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.2.1 Дата перевірки картки\","
                + "\"v\":\"\"}");

            //Произвольное поле 22
            string pp37_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.2.2 Оператор перевірки картки\","
                + "\"v\":\"\"}");

            //Произвольное поле 23
            string pp38_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"10 Кодове слово\","
                + "\"v\":\"\"}");

            /////////////////
            ///Добавляем в группу Объекты All
            /////////////////

            string get_units_on_group = macros.WialonRequest(
                "&svc=core/search_item&params={"
                + "\"id\":\"2612\","
                + "\"flags\":\"1\"}");//получаем все объекты группы

            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

            list_get_units_on_group.item.u.Add(cr_obj_out.item.id);//Доповляем в список новый объект
            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

            string gr_answer = macros.WialonRequest(
                "&svc=unit_group/update_units&params={"
                + "\"itemId\":\"2612\","
                + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

            /////////////////
            ///Создаем команды
            /////////////////
            ///

            //Service
            string cmd_service_answer = macros.WialonRequest(
                "&svc=unit/update_command_definition&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"Service\","
                + "\"c\":\"driver_msg\","
                + "\"l\":\"tcp\","
                + "\"p\":\"TPASS: 081983;\","
                + "\"a\":\"1\"}");//обновляем в Виалоне группу все объекты + новый

            string id_sim = "";

            DataTable t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel_select.Text + "';");
            if (t.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(4) + "';");
            }

            string sql2 = string.Format("insert into btk.Object(Object_id_wl, Object_imei, Object_name, Object_sim_no, products_idproducts, Simcard_idSimcard, TS_info_idTS_info, TS_info_TS_brend_model_idTS_brend_model, Kontakti_idKontakti_serviceman, Dogovora_idDogovora, Objectcol_edit_date, Users_idUsers) "
                + "values('" + cr_obj_out.item.id
                + "', '" + textBox_id_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "','"
                + maskedTextBox_sim_no_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.SelectedValue.ToString() + "', " +
                "'" + id_sim + "'," +
                "'1', '1', '1','1', null, '" + vars_form.user_login_id + "');");
            macros.sql_command(sql2);

            //получаем айди созданного объекта
            string id_object = macros.sql_command("SELECT MAX(idObject) FROM btk.Object;");

            // set datetime create
            macros.sql_command("update btk.Object set date_cteate = now() where idobject = '" + id_object + "';");

            //Создаем подписку для объекта
            string sql3 = string.Format("INSERT INTO btk.Subscription(products_has_Tarif_idproducts_has_Tarif, idobject) values('7'," + id_object + ");");
            macros.sql_command(sql3);

            //Получаем айди созданной подписки
            string sql4 = macros.sql_command("select max(idSubscr) from btk.Subscription;");

            //привязываем айди созданного объекта с айди созданной подписки
            string sql5 = string.Format("insert into btk.object_subscr (Object_idObject, Subscription_idSubscr) values (" + id_object + "," + sql4 + ");");
            macros.sql_command(sql5);

            ///////////////////////
            //Если все прошло успешно - завечиваем зеленым кнопку и затирает текстбоксы
            //////////////////////////
            button_create_object.Text = "Створено: " + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text + "\n Дата та час: " + DateTime.Now.ToString();
            textBox_id_to_create.Text = "";
            maskedTextBox_sim_no_to_create.Text = "";
            comboBox_tel_select.Text = "";
            button_create_object.BackColor = Color.Green;
        }

        private void KP_n()
        {
            /////////////////////
            //////Проверяем корректность введенных данных
            ///

            if (textBox_id_to_create.Text.Length <= 3)
            {
                textBox_id_to_create.BackColor = Color.Red;//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
                return;
            }
            if (maskedTextBox_sim_no_to_create.Text.Length <= 11)//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Red;
                return;
            }

            // Проверям существует ли данный номер в системе
            string unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли указанный ИМЕЙ в системе
            string unswer2 = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_unique_id\"," +
                                                     "\"propValueMask\":\"" + textBox_id_to_create.Text + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m2 = JsonConvert.DeserializeObject<RootObject>(unswer2);
            if (m2.items.Count > 0)
            {
                MessageBox.Show("Указанный IMEI существует в WL");
                return;
            }

            /////////////////
            ///Создаем объект
            /////////////////
            string cr_obj_in = "&svc=core/create_unit&params={\"creatorId\":\"" + vars_form.wl_user_id + "\",\"name\":\"" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "\",\"hwTypeId\":\"9\",\"dataFlags\":\"1\"}";
            string json = macros.WialonRequest(cr_obj_in);
            var cr_obj_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (cr_obj_out.error != 0)
            {
                string text = macros.Get_wl_text_error(cr_obj_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Создаем пароль доступа к объекту
            /////////////////

            string accsess_pass_answer = macros.WialonRequest("&svc=unit/update_access_password&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"accessPassword\":\"11111\"}");
            var accsess_pass_answer_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (accsess_pass_answer_out.error != 0)
            {
                string text = macros.Get_wl_text_error(accsess_pass_answer_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Установливаем имя объекта и ID оборудования и
            /////////////////
            string item_id_in = "&svc=unit/update_device_type&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id
                + "\",\"deviceTypeId\":\"" + "9"
                + "\",\"uniqueId\":\"" + textBox_id_to_create.Text.ToString() + "\"}";
            string json2 = macros.WialonRequest(item_id_in);
            var item_id_out = JsonConvert.DeserializeObject<RootObject>(json2);
            if (item_id_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_id_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Устанавливаем номер СИМ
            /////////////////
            string item_phone_in = "&svc=unit/update_phone&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim_no_to_create.Text) + "\"}";
            string json3 = macros.WialonRequest(item_phone_in);
            var item_phone_out = JsonConvert.DeserializeObject<RootObject>(json3);
            if (item_phone_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Создаем датчики
            /////////////////

            //3. Напряжение АКБ
            string akb_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Напряжение АКБ\","
                + "\"t\":\"voltage\","
                + "\"d\":\"\","
                + "\"m\":\"В\","
                + "\"p\":\"VPWR\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //6. Напряжение внутреннего АКБ
            string v_akb__answer = macros.WialonRequest("&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Напряжение внутреннего АКБ\","
                + "\"t\":\"voltage\","
                + "\"d\":\"\","
                + "\"m\":\"В\","
                + "\"p\":\"VBAT\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //7. Качество связи GSM
            string GMS_answer = macros.WialonRequest("&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Качество связи GSM\","
                + "\"t\":\"custom\","
                + "\"d\":\"\","
                + "\"m\":\"%25\","
                + "\"p\":\"par21\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[{\"x\":0,\"a\":20,\"b\":0},{\"x\":1,\"a\":20,\"b\":0},{\"x\":2,\"a\":20,\"b\":0},{\"x\":3,\"a\":20,\"b\":0},{\"x\":4,\"a\":20,\"b\":0},{\"x\":5,\"a\":20,\"b\":0}]}");

            //9. Тревожная кнопка
            string tk_answer = macros.WialonRequest("&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Тревожная кнопка\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Вкл/Выкл\","
                + "\"p\":\"AIN2\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[{\"x\":0,\"a\":0,\"b\":1},{\"x\":5.9,\"a\":0,\"b\":1},{\"x\":6,\"a\":0,\"b\":0},{\"x\":13,\"a\":0,\"b\":0}]}");

            //11. Зажигание
            string ign_answer = macros.WialonRequest(
                 "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Зажигание\","
                + "\"t\":\"engine operation\","
                + "\"d\":\"\","
                + "\"m\":\"Вкл/Выкл\","
                + "\"p\":\"par5\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");
            //12. Статус GPS
            string gps_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Статус GPS\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Да/%2D\","
                + "\"p\":\"par69\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            /////////////////
            ///Создаем произвольные поля
            /////////////////

            //Произвольное поле 1
            string pp1_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"0 УВАГА\","
                + "\"v\":\"\"}");

            //Произвольное поле 2
            string pp2_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"02 Проект\","
                + "\"v\":\"\"}");

            //Произвольное поле 3
            string pp3_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.1 І Відповідальна особа (основна)\","
                + "\"v\":\"\"}");

            //Произвольное поле 4
            string pp4_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.2 ІІ Відповідальна особа\","
                + "\"v\":\"\"}");

            //Произвольное поле 5
            string pp5_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.3 ІІІ Відповідальна особа\","
                + "\"v\":\"\"}");


            //Произвольное поле 6
            macros.create_custom_field_wl(cr_obj_out.item.id, "2.4 ІV Відповідальна особа", "");

            //Произвольное поле 7
            macros.create_custom_field_wl(cr_obj_out.item.id, "2.5 V Відповідальна особа", "");

            //Произвольное поле 6
            string pp7_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.1.1 Оператор, що тестував\","
                + "\"v\":\"\"}");

            //Произвольное поле 7
            string pp11_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.2.1 Установник: назва, адреса\","
                + "\"v\":\"\"}");

            //Произвольное поле 8
            string pp12_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.2.2 Установник-монтажник: ПІБ, №тел.\","
                + "\"v\":\"\"}");

            //Произвольное поле 9
            string pp13_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.3 Дата установки\","
                + "\"v\":\"\"}");

            //Произвольное поле 10
            string pp14_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.4 Місце установки пристрою ВЕНБЕСТ\","
                + "\"v\":\"\"}");

            //Произвольное поле 11
            string pp16_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.6.1  Реле блокування: місце встановлення\","
                + "\"v\":\"\"}");

            //Произвольное поле 12
            string pp17_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.6.2 Реле блокування: елемент блокування\","
                + "\"v\":\"\"}");

            //Произвольное поле 13
            string pp19_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.8.1 Тривожна кнопка: провідна, безпровідна\","
                + "\"v\":\"\"}");

            //Произвольное поле 14
            string pp20_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.8.2 Місце установки тривожної кнопки\","
                + "\"v\":\"\"}");

            //Произвольное поле 15
            string pp24_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.1 Дата активації\","
                + "\"v\":\"\"}");

            //Произвольное поле 16
            string pp25_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.1.1 Оператор, що активував\","
                + "\"v\":\"\"}");

            //Произвольное поле 17
            string pp28_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.4 Обліковий запис WL\","
                + "\"v\":\"\"}");

            //Произвольное поле 18
            string pp29_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.1 Менеджер\","
                + "\"v\":\"\"}");

            //Произвольное поле 19
            string pp30_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.2 Договір обслуговування\","
                + "\"v\":\"\"}");

            //Произвольное поле 20
            string pp31_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.3 Гарантія до\","
                + "\"v\":\"\"}");

            //Произвольное поле 21
            string pp32_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"8.1 Паркінг 1\","
                + "\"v\":\"\"}");

            //Произвольное поле 22
            string pp33_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"8.2 Паркінг 2\","
                + "\"v\":\"\"}");

            //Произвольное поле 23
            string pp34_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.0 Примітки\","
                + "\"v\":\"\"}");

            //Произвольное поле 24
            string pp35_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.1 Техпаспорт\","
                + "\"v\":\"\"}");

            //Произвольное поле 25
            string pp36_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.2.1 Дата перевірки картки\","
                + "\"v\":\"\"}");

            //Произвольное поле 26
            string pp37_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.2.2 Оператор перевірки картки\","
                + "\"v\":\"\"}");

            //Произвольное поле 27
            string pp38_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"10 Кодове слово\","
                + "\"v\":\"\"}");

            /////////////////
            ///Добавляем в группу Объекты All
            /////////////////

            string get_units_on_group = macros.WialonRequest(
                "&svc=core/search_item&params={"
                + "\"id\":\"2612\","
                + "\"flags\":\"1\"}");//получаем все объекты группы

            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

            list_get_units_on_group.item.u.Add(cr_obj_out.item.id);//Доповляем в список новый объект
            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

            string gr_answer = macros.WialonRequest(
                "&svc=unit_group/update_units&params={"
                + "\"itemId\":\"2612\","
                + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

            /////////////////
            ///Создаем команды
            /////////////////
            ///

            //Start engine
            string cmd_start_answer = macros.WialonRequest(
                "&svc=unit/update_command_definition&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"1. СТАРТ Двигатель\","
                + "\"c\":\"driver_msg\","
                + "\"l\":\"tcp\","
                + "\"p\":\"TPASS: 081983; setdigout 00;\","
                + "\"a\":\"1\"}");//обновляем в Виалоне группу все объекты + новый

            //Stop engine
            string cmd_stop_answer = macros.WialonRequest(
                "&svc=unit/update_command_definition&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2. СТОП Двигатель\","
                + "\"c\":\"driver_msg\","
                + "\"l\":\"tcp\","
                + "\"p\":\"TPASS: 081983; setdigout 01;\","
                + "\"a\":\"1\"}");//обновляем в Виалоне группу все объекты + новый

            //Service
            string cmd_service_answer = macros.WialonRequest(
                "&svc=unit/update_command_definition&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"Service\","
                + "\"c\":\"driver_msg\","
                + "\"l\":\"tcp\","
                + "\"p\":\"TPASS: 081983;\","
                + "\"a\":\"1\"}");//обновляем в Виалоне группу все объекты + новый

            string id_sim = "";

            DataTable t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel_select.Text + "';");
            if (t.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(4) + "';");
            }

            string sql2 = string.Format("insert into btk.Object(Object_id_wl, Object_imei, Object_name, Object_sim_no, products_idproducts, Simcard_idSimcard, TS_info_idTS_info, TS_info_TS_brend_model_idTS_brend_model, Kontakti_idKontakti_serviceman, Dogovora_idDogovora, Objectcol_edit_date, Users_idUsers) "
                + "values('" + cr_obj_out.item.id
                + "', '" + textBox_id_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "','"
                + maskedTextBox_sim_no_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.SelectedValue.ToString() + "', " +
                "'" + id_sim + "'," +
                "'1', '1', '1','1', null, '" + vars_form.user_login_id + "');");
            macros.sql_command(sql2);

            //получаем айди созданного объекта
            string id_object = macros.sql_command("SELECT MAX(idObject) FROM btk.Object;");

            // set datetime create
            macros.sql_command("update btk.Object set date_cteate = now() where idobject = '" + id_object + "';");

            //Создаем подписку для объекта
            string sql3 = string.Format("INSERT INTO btk.Subscription(products_has_Tarif_idproducts_has_Tarif, idobject) values('12'," + id_object + ");");
            macros.sql_command(sql3);

            //Получаем айди созданной подписки
            string sql4 = macros.sql_command("select max(idSubscr) from btk.Subscription;");

            //привязываем айди созданного объекта с айди созданной подписки
            string sql5 = string.Format("insert into btk.object_subscr (Object_idObject, Subscription_idSubscr) values (" + id_object + "," + sql4 + ");");
            macros.sql_command(sql5);

            ///////////////////////
            //Если все прошло успешно - завечиваем зеленым кнопку и затирает текстбоксы
            //////////////////////////
            button_create_object.Text = "Створено: " + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text + "\n Дата та час: " + DateTime.Now.ToString();
            maskedTextBox_BLE_CODE.Text = "";
            maskedTextBox_GSM_CODE.Text = "";
            textBox_id_to_create.Text = "";
            maskedTextBox_PUK.Text = "";
            maskedTextBox_sim_no_to_create.Text = "";
            comboBox_tel_select.Text = "";
            search_tovar_comboBox.Text = "";
            button_create_object.BackColor = Color.Green;
        }

        private void KB_n()
        {
            /////////////////////
            //////Проверяем корректность введенных данных
            ///

            if (textBox_id_to_create.Text.Length <= 3)
            {
                textBox_id_to_create.BackColor = Color.Red;//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
                return;
            }
            if (maskedTextBox_sim_no_to_create.Text.Length <= 11)//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Red;
                return;
            }

            // Проверям существует ли данный номер в системе
            string unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли указанный ИМЕЙ в системе
            string unswer2 = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_unique_id\"," +
                                                     "\"propValueMask\":\"" + textBox_id_to_create.Text + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m2 = JsonConvert.DeserializeObject<RootObject>(unswer2);
            if (m2.items.Count > 0)
            {
                MessageBox.Show("Указанный IMEI существует в WL");
                return;
            }

            /////////////////
            ///Создаем объект
            /////////////////
            string cr_obj_in = "&svc=core/create_unit&params={\"creatorId\":\"" + vars_form.wl_user_id + "\",\"name\":\"" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "\",\"hwTypeId\":\"9\",\"dataFlags\":\"1\"}";
            string json = macros.WialonRequest(cr_obj_in);
            var cr_obj_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (cr_obj_out.error != 0)
            {
                string text = macros.Get_wl_text_error(cr_obj_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Создаем пароль доступа к объекту
            /////////////////

            string accsess_pass_answer = macros.WialonRequest("&svc=unit/update_access_password&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"accessPassword\":\"11111\"}");
            var accsess_pass_answer_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (accsess_pass_answer_out.error != 0)
            {
                string text = macros.Get_wl_text_error(accsess_pass_answer_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Установливаем имя объекта и ID оборудования и
            /////////////////
            string item_id_in = "&svc=unit/update_device_type&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id
                + "\",\"deviceTypeId\":\"" + "9"
                + "\",\"uniqueId\":\"" + textBox_id_to_create.Text.ToString() + "\"}";
            string json2 = macros.WialonRequest(item_id_in);
            var item_id_out = JsonConvert.DeserializeObject<RootObject>(json2);
            if (item_id_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_id_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Устанавливаем номер СИМ
            /////////////////
            string item_phone_in = "&svc=unit/update_phone&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim_no_to_create.Text) + "\"}";
            string json3 = macros.WialonRequest(item_phone_in);
            var item_phone_out = JsonConvert.DeserializeObject<RootObject>(json3);
            if (item_phone_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Создаем датчики
            /////////////////

            //3. Напряжение АКБ
            string akb_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Напряжение АКБ\","
                + "\"t\":\"voltage\","
                + "\"d\":\"\","
                + "\"m\":\"В\","
                + "\"p\":\"VPWR\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //6. Напряжение внутреннего АКБ
            string v_akb__answer = macros.WialonRequest("&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Напряжение внутреннего АКБ\","
                + "\"t\":\"voltage\","
                + "\"d\":\"\","
                + "\"m\":\"В\","
                + "\"p\":\"VBAT\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //7. Качество связи GSM
            string GMS_answer = macros.WialonRequest("&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Качество связи GSM\","
                + "\"t\":\"custom\","
                + "\"d\":\"\","
                + "\"m\":\"%25\","
                + "\"p\":\"par21\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[{\"x\":0,\"a\":20,\"b\":0},{\"x\":1,\"a\":20,\"b\":0},{\"x\":2,\"a\":20,\"b\":0},{\"x\":3,\"a\":20,\"b\":0},{\"x\":4,\"a\":20,\"b\":0},{\"x\":5,\"a\":20,\"b\":0}]}");

            //11. Зажигание
            string ign_answer = macros.WialonRequest(
                 "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Зажигание\","
                + "\"t\":\"engine operation\","
                + "\"d\":\"\","
                + "\"m\":\"Вкл/Выкл\","
                + "\"p\":\"par5\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");
            //12. Статус GPS
            string gps_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Статус GPS\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Да/%2D\","
                + "\"p\":\"par69\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            /////////////////
            ///Создаем произвольные поля
            /////////////////

            //Произвольное поле 1
            string pp1_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"0 УВАГА\","
                + "\"v\":\"\"}");

            //Произвольное поле 2
            string pp2_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"02 Проект\","
                + "\"v\":\"\"}");

            //Произвольное поле 3
            string pp3_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.1 І Відповідальна особа (основна)\","
                + "\"v\":\"\"}");

            //Произвольное поле 4
            string pp4_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.2 ІІ Відповідальна особа\","
                + "\"v\":\"\"}");

            //Произвольное поле 5
            string pp5_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.3 ІІІ Відповідальна особа\","
                + "\"v\":\"\"}");

            //Произвольное поле 6
            macros.create_custom_field_wl(cr_obj_out.item.id, "2.4 ІV Відповідальна особа", "");

            //Произвольное поле 7
            macros.create_custom_field_wl(cr_obj_out.item.id, "2.5 V Відповідальна особа", "");

            //Произвольное поле 6
            string pp7_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.1.1 Оператор, що тестував\","
                + "\"v\":\"\"}");

            //Произвольное поле 7
            string pp11_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.2.1 Установник: назва, адреса\","
                + "\"v\":\"\"}");

            //Произвольное поле 8
            string pp12_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.2.2 Установник-монтажник: ПІБ, №тел.\","
                + "\"v\":\"\"}");

            //Произвольное поле 9
            string pp13_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.3 Дата установки\","
                + "\"v\":\"\"}");

            //Произвольное поле 10
            string pp14_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.4 Місце установки пристрою ВЕНБЕСТ\","
                + "\"v\":\"\"}");

            //Произвольное поле 11
            string pp16_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.6.1  Реле блокування: місце встановлення\","
                + "\"v\":\"\"}");

            //Произвольное поле 12
            string pp17_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.6.2 Реле блокування: елемент блокування\","
                + "\"v\":\"\"}");

            //Произвольное поле 13
            string pp24_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.1 Дата активації\","
                + "\"v\":\"\"}");

            //Произвольное поле 14
            string pp25_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.1.1 Оператор, що активував\","
                + "\"v\":\"\"}");

            //Произвольное поле 15
            string pp28_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.4 Обліковий запис WL\","
                + "\"v\":\"\"}");

            //Произвольное поле 16
            string pp29_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.1 Менеджер\","
                + "\"v\":\"\"}");

            //Произвольное поле 17
            string pp30_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.2 Договір обслуговування\","
                + "\"v\":\"\"}");

            //Произвольное поле 18
            string pp31_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.3 Гарантія до\","
                + "\"v\":\"\"}");

            //Произвольное поле 19
            string pp32_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"8.1 Паркінг 1\","
                + "\"v\":\"\"}");

            //Произвольное поле 20
            string pp33_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"8.2 Паркінг 2\","
                + "\"v\":\"\"}");

            //Произвольное поле 21
            string pp34_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.0 Примітки\","
                + "\"v\":\"\"}");

            //Произвольное поле 22
            string pp35_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.1 Техпаспорт\","
                + "\"v\":\"\"}");

            //Произвольное поле 23
            string pp36_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.2.1 Дата перевірки картки\","
                + "\"v\":\"\"}");

            //Произвольное поле 24
            string pp37_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.2.2 Оператор перевірки картки\","
                + "\"v\":\"\"}");

            //Произвольное поле 25
            string pp38_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"10 Кодове слово\","
                + "\"v\":\"\"}");

            /////////////////
            ///Добавляем в группу Объекты All
            /////////////////

            string get_units_on_group = macros.WialonRequest(
                "&svc=core/search_item&params={"
                + "\"id\":\"2612\","
                + "\"flags\":\"1\"}");//получаем все объекты группы

            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

            list_get_units_on_group.item.u.Add(cr_obj_out.item.id);//Доповляем в список новый объект
            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

            string gr_answer = macros.WialonRequest(
                "&svc=unit_group/update_units&params={"
                + "\"itemId\":\"2612\","
                + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

            /////////////////
            ///Создаем команды
            /////////////////
            ///

            //Start engine
            string cmd_start_answer = macros.WialonRequest(
                "&svc=unit/update_command_definition&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"1. СТАРТ Двигатель\","
                + "\"c\":\"driver_msg\","
                + "\"l\":\"tcp\","
                + "\"p\":\"TPASS: 081983; setdigout 00;\","
                + "\"a\":\"1\"}");//обновляем в Виалоне группу все объекты + новый

            //Stop engine
            string cmd_stop_answer = macros.WialonRequest(
                "&svc=unit/update_command_definition&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2. СТОП Двигатель\","
                + "\"c\":\"driver_msg\","
                + "\"l\":\"tcp\","
                + "\"p\":\"TPASS: 081983; setdigout 01;\","
                + "\"a\":\"1\"}");//обновляем в Виалоне группу все объекты + новый

            //Service
            string cmd_service_answer = macros.WialonRequest(
                "&svc=unit/update_command_definition&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"Service\","
                + "\"c\":\"driver_msg\","
                + "\"l\":\"tcp\","
                + "\"p\":\"TPASS: 081983;\","
                + "\"a\":\"1\"}");//обновляем в Виалоне группу все объекты + новый

            string id_sim = "";

            DataTable t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel_select.Text + "';");
            if (t.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(4) + "';");
            }

            string sql2 = string.Format("insert into btk.Object(Object_id_wl, Object_imei, Object_name, Object_sim_no, products_idproducts, Simcard_idSimcard, TS_info_idTS_info, TS_info_TS_brend_model_idTS_brend_model, Kontakti_idKontakti_serviceman, Dogovora_idDogovora, Objectcol_edit_date, Users_idUsers) "
                + "values('" + cr_obj_out.item.id
                + "', '" + textBox_id_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "','"
                + maskedTextBox_sim_no_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.SelectedValue.ToString() + "', " +
                "'" + id_sim + "'," +
                "'1', '1', '1','1', null, '" + vars_form.user_login_id + "');");
            macros.sql_command(sql2);

            //получаем айди созданного объекта
            string id_object = macros.sql_command("SELECT MAX(idObject) FROM btk.Object;");

            // set datetime create
            macros.sql_command("update btk.Object set date_cteate = now() where idobject = '" + id_object + "';");

            //Создаем подписку для объекта
            string sql3 = string.Format("INSERT INTO btk.Subscription(products_has_Tarif_idproducts_has_Tarif, idobject) values('38'," + id_object + ");");
            macros.sql_command(sql3);

            //Получаем айди созданной подписки
            string sql4 = macros.sql_command("select max(idSubscr) from btk.Subscription;");

            //привязываем айди созданного объекта с айди созданной подписки
            string sql5 = string.Format("insert into btk.object_subscr (Object_idObject, Subscription_idSubscr) values (" + id_object + "," + sql4 + ");");
            macros.sql_command(sql5);

            ///////////////////////
            //Если все прошло успешно - завечиваем зеленым кнопку и затирает текстбоксы
            //////////////////////////

            button_create_object.Text = "Створено: " + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text + "\n Дата та час: " + DateTime.Now.ToString();
            textBox_id_to_create.Text = "";
            maskedTextBox_sim_no_to_create.Text = "";
            comboBox_tel_select.Text = "";
            button_create_object.BackColor = Color.Green;
        }

        private void S()
        {
            /////////////////////
            //////Проверяем корректность введенных данных
            ///

            if (textBox_id_to_create.Text.Length <= 3)
            {
                textBox_id_to_create.BackColor = Color.Red;//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
                return;
            }

            //// Проверям существует ли данный номер в системе
            //string unswer = macros.WialonRequest("&svc=core/search_items&params={" +
            //                                         "\"spec\":{" +
            //                                         "\"itemsType\":\"avl_unit\"," +
            //                                         "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
            //                                         "\"propValueMask\":\"" + "*" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "\", " +
            //                                         "\"sortType\":\"sys_name\"," +
            //                                         "\"or_logic\":\"1\"}," +
            //                                         "\"force\":\"1\"," +
            //                                         "\"flags\":\"1\"," +
            //                                         "\"from\":\"0\"," +
            //                                         "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            //var m = JsonConvert.DeserializeObject<RootObject>(unswer);
            //if (m.items.Count > 0)
            //{
            //    MessageBox.Show("Указанный телефон существует в WL");
            //    return;
            //}

            // Проверям существует ли указанный ИМЕЙ в системе
            string unswer2 = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_unique_id\"," +
                                                     "\"propValueMask\":\"" + textBox_id_to_create.Text + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m2 = JsonConvert.DeserializeObject<RootObject>(unswer2);
            if (m2.items.Count > 0)
            {
                MessageBox.Show("Указанный IMEI существует в WL");
                return;
            }

            /////////////////
            ///Создаем объект
            /////////////////
            string cr_obj_in = "&svc=core/create_unit&params={\"creatorId\":\"" + vars_form.wl_user_id + "\",\"name\":\"" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "\",\"hwTypeId\":\"8\",\"dataFlags\":\"1\"}";
            string json = macros.WialonRequest(cr_obj_in);
            var cr_obj_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (cr_obj_out.error != 0)
            {
                string text = macros.Get_wl_text_error(cr_obj_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Создаем пароль доступа к объекту
            /////////////////

            string accsess_pass_answer = macros.WialonRequest(
                "&svc=unit/update_access_password&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"accessPassword\":\"" + textBox_id_to_create.Text + "\"}");
            var accsess_pass_answer_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (accsess_pass_answer_out.error != 0)
            {
                string text = macros.Get_wl_text_error(accsess_pass_answer_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Установливаем имя объекта и ID оборудования и
            /////////////////
            string item_id_in =
                "&svc=unit/update_device_type&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id
                + "\",\"deviceTypeId\":\"" + "8"
                + "\",\"uniqueId\":\"" + textBox_id_to_create.Text.ToString() + "\"}";
            string json2 = macros.WialonRequest(item_id_in);
            var item_id_out = JsonConvert.DeserializeObject<RootObject>(json2);
            if (item_id_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_id_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Добавляем в группу Объекты All
            /////////////////

            string get_units_on_group = macros.WialonRequest(
                "&svc=core/search_item&params={"
                + "\"id\":\"2612\","
                + "\"flags\":\"1\"}");//получаем все объекты группы

            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

            list_get_units_on_group.item.u.Add(cr_obj_out.item.id);//Доповляем в список новый объект
            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

            string gr_answer = macros.WialonRequest(
                "&svc=unit_group/update_units&params={"
                + "\"itemId\":\"2612\","
                + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

            /////////////////
            ///Создаем произвольные поля
            /////////////////

            //Произвольное поле 1
            string pp1_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"0 УВАГА\","
                + "\"v\":\"\"}");

            //Произвольное поле 2
            string pp2_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"01 Реагування\","
                + "\"v\":\"\"}");

            //Произвольное поле 3
            string pp3_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.0 Обєкт\","
                + "\"v\":\"\"}");

            //Произвольное поле 4
            string pp4_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.1 І Відповідальна особа (основна)\","
                + "\"v\":\"\"}");

            //Произвольное поле 5
            string pp5_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.2 ІІ Відповідальна особа\","
                + "\"v\":\"\"}");

            //Произвольное поле 6
            macros.create_custom_field_wl(cr_obj_out.item.id, "2.3 ІII Відповідальна особа", "");

            //Произвольное поле 6
            macros.create_custom_field_wl(cr_obj_out.item.id, "2.4 ІV Відповідальна особа", "");

            //Произвольное поле 7
            macros.create_custom_field_wl(cr_obj_out.item.id, "2.5 V Відповідальна особа", "");

            //Произвольное поле 6
            string pp6_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3 Особливі прикмети\","
                + "\"v\":\"\"}");

            //Произвольное поле 7
            string pp7_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.1 Місця перебування (місце проживання)\","
                + "\"v\":\"\"}");

            //Произвольное поле 8
            string pp8_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.2 Місця перебування 2\","
                + "\"v\":\"\"}");

            //Произвольное поле 9
            string pp9_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.3 Місця перебування 3\","
                + "\"v\":\"\"}");

            //Произвольное поле 10
            string pp10_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.4 Місця перебування 4\","
                + "\"v\":\"\"}");

            //Произвольное поле 11
            string pp11_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.5 Місця перебування 5\","
                + "\"v\":\"\"}");

            //Произвольное поле 12
            string pp12_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4 Примітки\","
                + "\"v\":\"\"}");

            //Произвольное поле 13
            string pp13_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"8 Дата активації\","
                + "\"v\":\"\"}");

            //Произвольное поле 14
            string pp14_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9 Тривожна кнопка на смартфоні\","
                + "\"v\":\"\"}");

            //Произвольное поле 15
            string pp15_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.2.1 Дата перевірки картки\","
                + "\"v\":\"\"}");

            //Произвольное поле 16
            string pp16_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.2.2 Оператор перевірки картки\","
                + "\"v\":\"\"}");

            //Произвольное поле 17
            string pp17_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"10 Кодове слово\","
                + "\"v\":\"\"}");

            //Произвольное поле 18
            string pp18_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"Документ\","
                + "\"v\":\"\"}");

            //Произвольное поле 19
            string pp19_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"Електронна скринька\","
                + "\"v\":\"\"}");

            string sql2 = string.Format("insert into btk.Object(Object_id_wl, Object_imei, Object_name, Object_sim_no, products_idproducts, Simcard_idSimcard, TS_info_idTS_info, TS_info_TS_brend_model_idTS_brend_model, Kontakti_idKontakti_serviceman, Dogovora_idDogovora, Objectcol_edit_date, Users_idUsers) "
                + "values('" + cr_obj_out.item.id
                + "', '" + textBox_id_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "','"
                + ""
                + "', '" + comboBox_list_poructs.SelectedValue.ToString() + "', '1', '1', '1', '1','1', null, '" + vars_form.user_login_id + "');");
            macros.sql_command(sql2);

            //получаем айди созданного объекта
            string id_object = macros.sql_command("SELECT MAX(idObject) FROM btk.Object;");

            // set datetime create
            macros.sql_command("update btk.Object set date_cteate = now() where idobject = '" + id_object + "';");

            //Создаем подписку для объекта
            string sql3 = string.Format("INSERT INTO btk.Subscription(products_has_Tarif_idproducts_has_Tarif, idobject) values('9'," + id_object + ");");
            macros.sql_command(sql3);

            //Получаем айди созданной подписки
            string sql4 = macros.sql_command("select max(idSubscr) from btk.Subscription;");

            //привязываем айди созданного объекта с айди созданной подписки
            string sql5 = string.Format("insert into btk.object_subscr (Object_idObject, Subscription_idSubscr) values (" + id_object + "," + sql4 + ");");
            macros.sql_command(sql5);

            ///////////////////////
            //Если все прошло успешно - завечиваем зеленым кнопку и затирает текстбоксы
            //////////////////////////
            button_create_object.Text = "Створено: " + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text + "\n Дата та час: " + DateTime.Now.ToString();
            maskedTextBox_BLE_CODE.Text = "";
            maskedTextBox_GSM_CODE.Text = "";
            textBox_id_to_create.Text = "";
            maskedTextBox_PUK.Text = "";
            maskedTextBox_sim_no_to_create.Text = "";
            comboBox_tel_select.Text = "";
            search_tovar_comboBox.Text = "";
            button_create_object.BackColor = Color.Green;
        }

        private void SLED()
        {
            /////////////////////
            //////Проверяем корректность введенных данных
            ///

            if (textBox_id_to_create.Text.Length <= 3)
            {
                textBox_id_to_create.BackColor = Color.Red;//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
                return;
            }
            if (maskedTextBox_sim_no_to_create.Text.Length <= 11)//Если IMEI короче 4х символов останавливается и подсвкечиваем красным
            {
                maskedTextBox_sim_no_to_create.BackColor = Color.Red;
                return;
            }

            // Проверям существует ли данный номер в системе
            string unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_phone_number|sys_phone_number2\"," +
                                                     "\"propValueMask\":\"" + "*" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m = JsonConvert.DeserializeObject<RootObject>(unswer);
            if (m.items.Count > 0)
            {
                MessageBox.Show("Указанный телефон существует в WL");
                return;
            }

            // Проверям существует ли указанный ИМЕЙ в системе
            string unswer2 = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_unique_id\"," +
                                                     "\"propValueMask\":\"" + textBox_id_to_create.Text + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"1\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе
            var m2 = JsonConvert.DeserializeObject<RootObject>(unswer2);
            if (m2.items.Count > 0)
            {
                MessageBox.Show("Указанный IMEI существует в WL");
                return;
            }

            /////////////////
            ///Создаем объект
            /////////////////
            string cr_obj_in = "&svc=core/create_unit&params={\"creatorId\":\"" + vars_form.wl_user_id + "\",\"name\":\"" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "\",\"hwTypeId\":\"37\",\"dataFlags\":\"1\"}";
            string json = macros.WialonRequest(cr_obj_in);
            var cr_obj_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (cr_obj_out.error != 0)
            {
                string text = macros.Get_wl_text_error(cr_obj_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Установливаем имя объекта и ID оборудования и
            /////////////////
            string item_id_in =
                "&svc=unit/update_device_type&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id
                + "\",\"deviceTypeId\":\"" + "37"
                + "\",\"uniqueId\":\"" + textBox_id_to_create.Text.ToString() + "\"}";
            string json2 = macros.WialonRequest(item_id_in);
            var item_id_out = JsonConvert.DeserializeObject<RootObject>(json2);
            if (item_id_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_id_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }
            /////////////////
            ///Устанавливаем номер СИМ
            /////////////////
            string item_phone_in = "&svc=unit/update_phone&params={\"itemId\":\"" + cr_obj_out.item.id + "\",\"phoneNumber\":\"" + WebUtility.UrlEncode(maskedTextBox_sim_no_to_create.Text) + "\"}";
            string json3 = macros.WialonRequest(item_phone_in);
            var item_phone_out = JsonConvert.DeserializeObject<RootObject>(json3);
            if (item_phone_out.error != 0)
            {
                string text = macros.Get_wl_text_error(item_phone_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                return;
            }

            /////////////////
            ///Изменение настроек фильтрации информации о положении объекта в сообщениях
            /////////////////

            string mes_answer = macros.WialonRequest(
                                                                                                                "&svc=unit/update_messages_filter&params={"
                                                                                                                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                                                                                                                + "\"enabled\":\"true\","
                                                                                                                + "\"skipInvalid\":\"true\","
                                                                                                                + "\"minSats\":\"4\","
                                                                                                                + "\"maxHdop\":\"2\","
                                                                                                                + "\"maxSpeed\":\"0\","
                                                                                                                + "\"lbsCorrection\":\"1\"}");

            /////////////////
            ///Создаем датчики
            /////////////////

            //1. Статус Погоня
            string arm_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Погоня\","
                + "\"t\":\"digital\","
                + "\"d\":\"\","
                + "\"m\":\"Вкл/Выкл\","
                + "\"p\":\"par104\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //2. Статус Заряд батареи
            string door_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Заряд батареи\","
                + "\"t\":\"custom\","
                + "\"d\":\"\","
                + "\"m\":\"%\","
                + "\"p\":\"sens103\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[{\"x\":1,\"a\":-0.07142857,\"b\":100}]}");

            //3. Всего сеансов
            string akb_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Всего сеансов\","
                + "\"t\":\"custom\","
                + "\"d\":\"\","
                + "\"m\":\"шт.\","
                + "\"p\":\"sens103\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //4. Причина последнего сна
            string siren_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Причина последнего сна\","
                + "\"t\":\"custom\","
                + "\"d\":\"\","
                + "\"m\":\"\","
                + "\"p\":\"sens101\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //5. Длительность последнего сна
            string d_udara_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Длительность последнего сна\","
                + "\"t\":\"custom\","
                + "\"d\":\"\","
                + "\"m\":\"ч.\","
                + "\"p\":\"sens102\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[{\"x\":1,\"a\":0.0166666666,\"b\":0}]}");

            //6. Источник пробуждения
            string v_akb__answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Источник пробуждения\","
                + "\"t\":\"custom\","
                + "\"d\":\"\","
                + "\"m\":\"\","
                + "\"p\":\"sens111\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            //7. Источник перезагрузки
            string GMS_answer = macros.WialonRequest(
                "&svc=unit/update_sensor&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"unlink\":\"1\","
                + "\"n\":\"Источник перезагрузки\","
                + "\"t\":\"custom\","
                + "\"d\":\"\","
                + "\"m\":\"\","
                + "\"p\":\"sens100\","
                + "\"f\":\"0\","
                + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true}\","
                + "\"vt\":\"1\","
                + "\"vs\":\"0\","
                + "\"tbl\":[]}");

            /////////////////
            ///Создаем произвольные поля
            /////////////////

            //Произвольное поле 1
            string pp1_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"0 УВАГА\","
                + "\"v\":\"\"}");

            //Произвольное поле 2
            string pp2_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"02 Проект\","
                + "\"v\":\"\"}");

            //Произвольное поле 3
            string pp3_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.1 І Відповідальна особа (основна)\","
                + "\"v\":\"\"}");

            //Произвольное поле 4
            string pp4_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.2 ІІ Відповідальна особа\","
                + "\"v\":\"\"}");

            //Произвольное поле 5
            string pp5_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2.3 ІІІ Відповідальна особа\","
                + "\"v\":\"\"}");

            //Произвольное поле 6
            macros.create_custom_field_wl(cr_obj_out.item.id, "2.4 ІV Відповідальна особа", "");

            //Произвольное поле 7
            macros.create_custom_field_wl(cr_obj_out.item.id, "2.5 V Відповідальна особа", "");

            //Произвольное поле 7
            string pp7_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.1.1 Оператор, що тестував\","
                + "\"v\":\"\"}");

            //Произвольное поле 8
            string pp8_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.11 Додатково встановлені сигналізації\","
                + "\"v\":\"\"}");

            //Произвольное поле 9
            string pp9_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.2.1 Установник: назва, адреса\","
                + "\"v\":\"\"}");

            //Произвольное поле 10
            string pp10_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.2.2 Установник-монтажник: ПІБ, №тел.\","
                + "\"v\":\"\"}");

            //Произвольное поле 11
            string pp11_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.3 Дата установки\","
                + "\"v\":\"\"}");

            //Произвольное поле 12
            string pp12_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"3.4 Місце установки\","
                + "\"v\":\"\"}");

            //Произвольное поле 13
            string pp33_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.0 Дата останньої заміни батареї\","
                + "\"v\":\"\"}");

            //Произвольное поле 14
            string pp13_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.1 Дата активації\","
                + "\"v\":\"\"}");

            //Произвольное поле 15
            string pp14_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.1.1 Оператор, що активував\","
                + "\"v\":\"\"}");

            //Произвольное поле 16
            string pp15_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"4.4 Обліковий запис WL\","
                + "\"v\":\"\"}");

            //Произвольное поле 17
            string pp16_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.1 Менеджер\","
                + "\"v\":\"\"}");

            //Произвольное поле 18
            string pp17_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.2 Договір обслуговування\","
                + "\"v\":\"\"}");

            //Произвольное поле 19
            string pp18_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"5.3 Гарантія до\","
                + "\"v\":\"\"}");

            //Произвольное поле 20
            string pp19_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"8.1 Паркінг 1\","
                + "\"v\":\"\"}");

            //Произвольное поле 21
            string pp20_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"8.2 Паркінг 2\","
                + "\"v\":\"\"}");

            //Произвольное поле 22
            string pp21_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.0 Примітки\","
                + "\"v\":\"\"}");

            //Произвольное поле 23
            string pp22_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.1 Техпаспорт\","
                + "\"v\":\"\"}");

            //Административное поле 24
            string pp23_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.2.1 Дата перевірки картки\","
                + "\"v\":\"\"}");

            //Произвольное поле 25
            string pp24_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.2.2 Оператор перевірки картки\","
                + "\"v\":\"\"}");

            //Произвольное поле 26
            string pp25_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"9.9 Колір ТЗ\","
                + "\"v\":\"\"}");

            //Произвольное поле 27
            string pp26_answer = macros.WialonRequest(
                "&svc=item/update_custom_field&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"10 Кодове слово\","
                + "\"v\":\"\"}");

            /////////////////
            ///Добавляем в группу Объекты All
            /////////////////

            string get_units_on_group = macros.WialonRequest(
                "&svc=core/search_item&params={"
                + "\"id\":\"2612\","
                + "\"flags\":\"1\"}");//получаем все объекты группы

            var list_get_units_on_group = JsonConvert.DeserializeObject<RootObject>(get_units_on_group);

            list_get_units_on_group.item.u.Add(cr_obj_out.item.id);//Доповляем в список новый объект
            string units_in_group = JsonConvert.SerializeObject(list_get_units_on_group.item.u);

            string gr_answer = macros.WialonRequest(
                "&svc=unit_group/update_units&params={"
                + "\"itemId\":\"2612\","
                + "\"units\":" + units_in_group + "}");//обновляем в Виалоне группу все объекты + новый

            /////////////////
            ///Создаем команды
            /////////////////
            ///

            //Start engine
            string cmd_start_answer = macros.WialonRequest(
                "&svc=unit/update_command_definition&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"1 - ШТАТНЫЙ РЕЖИМ\","
                + "\"c\":\"custom_msg\","
                + "\"l\":\"\","
                + "\"p\":\"%26setparam 0104 0%26setparam 0204 0%26setparam 0201 0%26saveparams;\","
                + "\"a\":\"1\"}");//обновляем в Виалоне группу все объекты + новый

            //Stop engine
            string cmd_stop_answer = macros.WialonRequest(
                "&svc=unit/update_command_definition&params={"
                + "\"itemId\":\"" + cr_obj_out.item.id + "\","
                + "\"id\":\"0\","
                + "\"callMode\":\"create\","
                + "\"n\":\"2 - ПОИСКОВЫЙ РЕЖИМ\","
                + "\"c\":\"custom_msg\","
                + "\"l\":\"\","
                + "\"p\":\"%26setparam 0104 1%26setparam 0204 120%26setparam 0201 120%26saveparams\","
                + "\"a\":\"1\"}");//обновляем в Виалоне группу все объекты + новый

            string id_sim = "";

            DataTable t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel_select.Text + "';");
            if (t.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(1) + "';");
            }
            else
            {
                //получаем айди SIM-card
                id_sim = macros.sql_command("SELECT idSimcard FROM btk.Simcard where Simcardcol_number = '" + maskedTextBox_sim_no_to_create.Text.Substring(4) + "';");
            }

            string sql2 = string.Format("insert into btk.Object(Object_id_wl, Object_imei, Object_name, Object_sim_no, products_idproducts, Simcard_idSimcard, TS_info_idTS_info, TS_info_TS_brend_model_idTS_brend_model, Kontakti_idKontakti_serviceman, Dogovora_idDogovora, Objectcol_edit_date, Users_idUsers, Objectcol_sn_puk_card) "
                + "values('" + cr_obj_out.item.id
                + "', '" + textBox_id_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text.ToString() + "','"
                + maskedTextBox_sim_no_to_create.Text.ToString()
                + "', '" + comboBox_list_poructs.SelectedValue.ToString() + "', " +
                "'" + id_sim + "'," +
                " '1', '1', '1','1', null, '"
                + vars_form.user_login_id
                + "', '" + textBox_id_to_create.Text.ToString() + "');");
            macros.sql_command(sql2);

            //получаем айди созданного объекта
            string id_object = macros.sql_command("SELECT MAX(idObject) FROM btk.Object;");

            // set datetime create
            macros.sql_command("update btk.Object set date_cteate = now() where idobject = '" + id_object + "';");

            //Создаем подписку для объекта
            string sql3 = string.Format("INSERT INTO btk.Subscription(products_has_Tarif_idproducts_has_Tarif, idobject) values('5'," + id_object + ");");
            macros.sql_command(sql3);

            //Получаем айди созданной подписки
            string sql4 = macros.sql_command("select max(idSubscr) from btk.Subscription;");

            //привязываем айди созданного объекта с айди созданной подписки
            string sql5 = string.Format("insert into btk.object_subscr (Object_idObject, Subscription_idSubscr) values (" + id_object + "," + sql4 + ");");
            macros.sql_command(sql5);

            ///////////////////////
            //Если все прошло успешно - завечиваем зеленым кнопку и затирает текстбоксы
            //////////////////////////
            button_create_object.Text = "Створено: " + comboBox_list_poructs.GetItemText(this.comboBox_list_poructs.SelectedItem) + " " + textBox_id_to_create.Text + "\n Дата та час: " + DateTime.Now.ToString();
            maskedTextBox_BLE_CODE.Text = "";
            maskedTextBox_GSM_CODE.Text = "";
            textBox_id_to_create.Text = "";
            maskedTextBox_PUK.Text = "";
            maskedTextBox_sim_no_to_create.Text = "";
            comboBox_tel_select.Text = "";
            search_tovar_comboBox.Text = "";
            button_create_object.BackColor = Color.Green;
        }

        private void comboBox_tel_select_DropDownClosed(object sender, EventArgs e)
        {
            //if (comboBox_tel_select.Text.Contains("F"))
            //{ comboBox_tel_select.Text = comboBox_tel_select.Text.Remove(comboBox_tel_select.Text.Length - 1); }
            //DataTable t = new DataTable();
            //t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel_select.Text + "';");
            //if (t != null & t.Rows.Count > 0)
            //{
            //    maskedTextBox_sim_no_to_create.Text = t.Rows[0][1].ToString();
            //    //Clipboard.SetText(textBox_id_to_create.Text);
            //}
        }

        private void BarCode()
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_39,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 60,
                    Width = 40,
                    PureBarcode = false,
                    Margin = 0
                },
            };

            writer
                .Write(textBox_id_to_create.Text)
                .Save(streamToPrint);
        }

        private void edit_img()
        {
            string firstText = "Hello";
            string secondText = "World";

            PointF firstLocation = new PointF(10f, 10f);
            PointF secondLocation = new PointF(10f, 50f);

            string imageFilePath = @"C:\Temp\barcode.png";
            Bitmap bitmap = (Bitmap)Image.FromFile(imageFilePath);//load the image file

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                using (Font arialFont = new Font("Arial", 10))
                {
                    graphics.DrawString(firstText, arialFont, Brushes.Blue, firstLocation);
                    graphics.DrawString(secondText, arialFont, Brushes.Red, secondLocation);
                }
            }

            bitmap.Save(@"C:\Temp\barcode1.png");//save the image file
        }

        private void PrintBarCode_Click(object sender, EventArgs e)
        {
            if (comboBox_printers.GetItemText(comboBox_printers.SelectedItem) == "")
            {
                MessageBox.Show("Вибери прінтер");
                return;
            }
            print();
        }

        private void GetPrinters()
        {
            comboBox_printers.Items.Clear();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                comboBox_printers.Items.Add(printer);
                for (int i = 0; i < comboBox_printers.Items.Count; i++)
                {
                    if (comboBox_printers.GetItemText(comboBox_printers.Items[i]).Contains("Xprinter"))
                    {
                        comboBox_printers.SelectedItem = comboBox_printers.Items[i];
                        return;
                    }
                }
            }
        }

        private void print()
        {
            BarCode();

            using (var pd = new System.Drawing.Printing.PrintDocument())
            {
                pd.PrinterSettings.PrinterName = comboBox_printers.GetItemText(comboBox_printers.SelectedItem);
                pd.DefaultPageSettings.PaperSize = new PaperSize("70 x 40 mm", 270, 150);
                pd.PrintPage += (_, e) =>
                {
                    Image img;
                    using (var bmpTemp = new Bitmap(streamToPrint))
                    {
                        img = new Bitmap(bmpTemp);
                    }

                    e.Graphics.DrawImage(img, 50, 50);
                };
                pd.Print();
                pd.Dispose();
            }
        }

        private void search_tovar_comboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            button_create_object.Text = "Створити";
            maskedTextBox_BLE_CODE.Text = "";
            maskedTextBox_GSM_CODE.Text = "";
            textBox_id_to_create.Text = "";
            maskedTextBox_PUK.Text = "";
            textBox_bt_enable.Text = "";
            button_create_object.BackColor = Color.Empty;
            comboBox_tel_select.BackColor = Color.Empty;
            maskedTextBox_sim_no_to_create.BackColor = Color.Empty;
            textBox_id_to_create.BackColor = Color.Empty;

            if (e.KeyChar == (char)13)
            {
                DataTable t = new DataTable();
                t = macros.GetData("SELECT CN, IMEI, BtKey, Puk, gsm_code, tag_access_code, SN FROM btk.Invice_Tovar where SN='" + search_tovar_comboBox.Text + "';");
                if (t != null & t.Rows.Count > 0)
                {
                    textBox_id_to_create.Text = t.Rows[0][0].ToString();
                    maskedTextBox_GSM_CODE.Text = t.Rows[0][4].ToString();
                    maskedTextBox_PUK.Text = t.Rows[0][3].ToString();
                    maskedTextBox_BLE_CODE.Text = t.Rows[0][2].ToString();
                    textBox_bt_enable.Text = t.Rows[0][5].ToString();
                    Clipboard.SetText(textBox_id_to_create.Text);
                    comboBox_tel_select.Focus();
                }
            }
        }

        private void comboBox_tel_select_KeyPress(object sender, KeyPressEventArgs e)
        {
            button_create_object.Text = "Створити";
            button_create_object.BackColor = Color.Empty;
            maskedTextBox_sim_no_to_create.Text = "";
            comboBox_tel_select.BackColor = Color.Empty;
            maskedTextBox_sim_no_to_create.BackColor = Color.Empty;
            if (e.KeyChar == (char)13)
            {
                if (comboBox_tel_select.Text.Contains("F") || comboBox_tel_select.Text.Contains("А"))
                { comboBox_tel_select.Text = comboBox_tel_select.Text.Remove(comboBox_tel_select.Text.Length - 1); }
                DataTable t = new DataTable();
                t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel_select.Text + "';");
                if (t != null & t.Rows.Count > 0)
                {
                    if (t.Rows[0][4].ToString() != "1")// if deactivated retorn error
                    {
                        if (t.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
                        {
                            maskedTextBox_sim_no_to_create.Mask = "";
                            maskedTextBox_sim_no_to_create.Text = "+" + t.Rows[0][1].ToString();
                            Clipboard.SetText(maskedTextBox_sim_no_to_create.Text);
                        }
                        else
                        {
                            maskedTextBox_sim_no_to_create.Mask = "+38\\0000000000";
                            maskedTextBox_sim_no_to_create.Text = t.Rows[0][1].ToString();
                            Clipboard.SetText(maskedTextBox_sim_no_to_create.Text);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Картка деактивована");
                        maskedTextBox_sim_no_to_create.BackColor = Color.Red;
                        maskedTextBox_sim_no_to_create.Text = t.Rows[0][1].ToString();
                        Clipboard.SetText(maskedTextBox_sim_no_to_create.Text);
                    }

                }
                else
                {
                    comboBox_tel_select.Text = "Не знайдено";
                    comboBox_tel_select.Focus();
                    comboBox_tel_select.BackColor = Color.Red;
                }
            }
        }

        private void comboBox_list_poructs_DropDownClosed(object sender, EventArgs e)
        {
            search_tovar_comboBox.Focus();
        }

        private void textBox_id_to_create_KeyPress(object sender, KeyPressEventArgs e)
        {
            button_create_object.Text = "Створити";
            search_tovar_comboBox.Text = "";
            maskedTextBox_BLE_CODE.Text = "";
            maskedTextBox_GSM_CODE.Text = "";
            maskedTextBox_PUK.Text = "";
            textBox_bt_enable.Text = "";
            button_create_object.BackColor = Color.Empty;
            textBox_id_to_create.BackColor = Color.Empty;

            if (e.KeyChar == (char)13)
            {
                DataTable t = new DataTable();
                t = macros.GetData("SELECT CN, IMEI, BtKey, Puk, gsm_code, tag_access_code, SN FROM btk.Invice_Tovar where CN='" + textBox_id_to_create.Text + "';");
                if (t != null & t.Rows.Count > 0)
                {
                    textBox_id_to_create.Text = t.Rows[0][0].ToString();
                    maskedTextBox_GSM_CODE.Text = t.Rows[0][4].ToString();
                    maskedTextBox_PUK.Text = t.Rows[0][3].ToString();
                    maskedTextBox_BLE_CODE.Text = t.Rows[0][2].ToString();
                    textBox_bt_enable.Text = t.Rows[0][5].ToString();
                    search_tovar_comboBox.Text = t.Rows[0][6].ToString();
                    comboBox_tel_select.Focus();
                }
            }
        }

        private void maskedTextBox_sim_no_to_create_KeyPress(object sender, KeyPressEventArgs e)
        {
            button_create_object.Text = "Створити";
            //maskedTextBox_sim_no_to_create.Text = "";
            comboBox_tel_select.Text = "";
            button_create_object.BackColor = Color.Empty;
            maskedTextBox_sim_no_to_create.BackColor = Color.Empty;

            if (e.KeyChar == (char)13)
            {
                string t = "SELECT Simcardcol_imsi FROM btk.Simcard where Simcardcol_number ='" + maskedTextBox_sim_no_to_create.Text.Substring(4) + "';";
                if (t != "")
                {
                    comboBox_tel_select.Text = macros.sql_command(t);
                }
            }
        }

        private void comboBox_user_to_crate_obj_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCreatedObjectsByUser(dateTimePicker_date_created_by_user.Value.Date);
        }


        private void dataGridView_CreatedObjects_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                return;
            }

            if (dataGridView_CreatedObjects.Rows[e.RowIndex].Cells[3].Value.ToString() != "")//Згруповано до ID тривоги(9)
            {
                comboBox_list_poructs.SelectedValue = Convert.ToInt16(dataGridView_CreatedObjects.Rows[e.RowIndex].Cells[10].Value.ToString());
                textBox_id_to_create.Text = dataGridView_CreatedObjects.Rows[e.RowIndex].Cells[1].Value.ToString();
                search_tovar_comboBox.Text = macros.sql_command("SELECT SN FROM btk.Invice_Tovar where CN = '" + textBox_id_to_create.Text + "';");
                textBox_bt_enable.Text = dataGridView_CreatedObjects.Rows[e.RowIndex].Cells[9].Value.ToString();
                maskedTextBox_BLE_CODE.Text = dataGridView_CreatedObjects.Rows[e.RowIndex].Cells[8].Value.ToString();
                maskedTextBox_GSM_CODE.Text = dataGridView_CreatedObjects.Rows[e.RowIndex].Cells[7].Value.ToString();
                maskedTextBox_PUK.Text = dataGridView_CreatedObjects.Rows[e.RowIndex].Cells[6].Value.ToString();
                //textBox_bt_enable.Text = dataGridView_CreatedObjects.Rows[e.RowIndex].Cells[8].Value.ToString();

                comboBox_tel_select.Text = dataGridView_CreatedObjects.Rows[e.RowIndex].Cells[4].Value.ToString();

                string d = dataGridView_CreatedObjects.Rows[e.RowIndex].Cells[5].Value.ToString();

                DataTable t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_number ='" + dataGridView_CreatedObjects.Rows[e.RowIndex].Cells[5].Value.ToString() + "';");
                if (t.Rows.Count > 0)
                {
                    if (t.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
                    {
                        maskedTextBox_sim_no_to_create.Mask = "";
                    }
                    else
                    {
                        maskedTextBox_sim_no_to_create.Mask = "+38\\0000000000";
                    }
                }
                maskedTextBox_sim_no_to_create.Text = dataGridView_CreatedObjects.Rows[e.RowIndex].Cells[5].Value.ToString();
            }
        }

        private void label11_DoubleClick(object sender, EventArgs e)
        {
            if (button_add_910.Visible == false)
            {
                button_add_910.Visible = true;
                textBox_id_to_create.Enabled = true;
                maskedTextBox_GSM_CODE.Enabled = true;
                maskedTextBox_PUK.Enabled = true;
                maskedTextBox_BLE_CODE.Enabled = true;
                textBox_bt_enable.Enabled = true;
                search_tovar_comboBox.Enabled = true;
                comboBox_tel_select.Enabled = false;
                maskedTextBox_sim_no_to_create.Enabled = false;
                comboBox_list_poructs.Enabled = false;

                textBox_id_to_create.ReadOnly = false;
                maskedTextBox_GSM_CODE.ReadOnly = false;
                maskedTextBox_PUK.ReadOnly = false;
                maskedTextBox_BLE_CODE.ReadOnly = false;
                textBox_bt_enable.ReadOnly = false;
                search_tovar_comboBox.KeyPress -= search_tovar_comboBox_KeyPress;
                textBox_id_to_create.KeyPress -= textBox_id_to_create_KeyPress;
                button_create_object.Enabled = false;
            }
            else
            {
                button_add_910.Visible = false;
                textBox_id_to_create.Enabled = false;
                maskedTextBox_GSM_CODE.Enabled = false;
                maskedTextBox_PUK.Enabled = false;
                maskedTextBox_BLE_CODE.Enabled = false;
                textBox_bt_enable.Enabled = false;
                search_tovar_comboBox.Enabled = false;
                comboBox_list_poructs.Enabled = true;
                maskedTextBox_PUK.ReadOnly = true;
                maskedTextBox_BLE_CODE.ReadOnly = true;
                maskedTextBox_GSM_CODE.ReadOnly = true;
                search_tovar_comboBox.KeyPress += search_tovar_comboBox_KeyPress;
                textBox_id_to_create.KeyPress += textBox_id_to_create_KeyPress;
                button_create_object.Enabled = true;
            }
        }

        private void button_add_910_Click(object sender, EventArgs e)
        {
            DataTable get = macros.GetData("SELECT SN, CN FROM btk.Invice_Tovar where SN = '" + search_tovar_comboBox.Text + "' or CN = '" + textBox_id_to_create.Text + "';");
            if (get.Rows.Count == 0)
            {
                macros.sql_command("insert into btk.Invice_Tovar (" +
                    "Tovar_idTovar," +
                    "Invoice_data_idInvoice_data," +
                    "SN," +
                    "CN," +
                    "BtKey," +
                    "Puk," +
                    "gsm_code," +
                    "tag_access_code" +
                    ") values (" +
                    "'1'," +
                    "'1'," +
                    "'" + search_tovar_comboBox.Text + "'," +
                    "'" + textBox_id_to_create.Text + "'," +
                    "'" + maskedTextBox_BLE_CODE.Text + "'," +
                    "'" + maskedTextBox_PUK.Text + "'," +
                    "'" + maskedTextBox_GSM_CODE.Text + "'," +
                    "'" + textBox_bt_enable.Text + "');");
                MessageBox.Show("Prizrak SN: " + search_tovar_comboBox.Text + " добавлен в систему");
            }
            else
            { MessageBox.Show("Блок с таким SN или IMEI существует в системе, проверь данные"); }
        }

        private void textBox_search_testing_TextChanged(object sender, EventArgs e)
        {

        }

        private void search_close_alarm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                mysql_close_alarm();
            }
        }

        private void dateTimePicker_from_for_zayavki_na_activation_W2_ValueChanged(object sender, EventArgs e)
        {
            update_zayavki_na_aktivation_2W();
        }

        private void checkBox_zayavki_za_ves_chas_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_zayavki_za_ves_chas.Checked == true)
            {
                dateTimePicker_from_for_zayavki_na_activation_W2.Enabled = false;
                dateTimePicker_for_zayavki_na_activation_W2.Enabled = false;
            }
            else
            {
                dateTimePicker_from_for_zayavki_na_activation_W2.Enabled = true;
                dateTimePicker_for_zayavki_na_activation_W2.Enabled = true;
            }
            update_zayavki_na_aktivation_2W();
        }

        private void dataGridView_zayavki_na_activation_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            dataGridView_zayavki_na_activation.SuspendLayout();
            //dataGridView_zayavki_na_activation.DataSource;
            string g = dataGridView_zayavki_na_activation.Rows[e.RowIndex].Cells[8].Value.ToString();

                if (dataGridView_zayavki_na_activation.Rows[e.RowIndex].Cells[8].Value.ToString() != "1")
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#FBFFAF");
                }
                else
                {
                    e.CellStyle.BackColor = Color.White;
                }
            if (dataGridView_zayavki_na_activation.Rows[e.RowIndex].Cells[9].Value.ToString().Contains("Успішно"))
                {
                    e.CellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#C6FFB2");
                }
                

            dataGridView_zayavki_na_activation.ResumeLayout();
        }

        private void checkBox_activation_za_ves_chas_search_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_activation_za_ves_chas_search.Checked == true )
            { 
                dateTimePicker_activation_filter_start.Enabled = false; 
                dateTimePicker_activation_filter_end.Enabled = false; 
            }
            else 
            { 
                dateTimePicker_activation_filter_start.Enabled = true; 
                dateTimePicker_activation_filter_end.Enabled = true; 
            }
            update_actication_dgv();
        }

        private void textBox_search_zayavki_TextChanged(object sender, EventArgs e)
        {
            if (textBox_search_zayavki.Text == "" & comboBox_reason_zayavki.Text=="")
            {
                checkBox_zayavki_za_ves_chas.Enabled = false;
                checkBox_zayavki_za_ves_chas.Checked = false;
            }
            else { checkBox_zayavki_za_ves_chas.Enabled = true; }
            update_zayavki_na_aktivation_2W();
        }

        private void checkBox_activation_uspishno_pin_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_activation_uspishno_pin.Checked is true)
            { textBox_search_object_name_activation.Text = "Успішно (PIN)"; }
            else { textBox_search_object_name_activation.Text = ""; }
        }

        private void checkBox_activation_neuspishno_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_activation_neuspishno.Checked is true)
            { textBox_search_object_name_activation.Text = "Не проводилось"; }
            else { textBox_search_object_name_activation.Text = ""; }
        }

        private void checkBox_activation_uspishno_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_activation_uspishno.Checked is true)
            { textBox_search_object_name_activation.Text = "Успішно"; }
            else { textBox_search_object_name_activation.Text = ""; }
        }


        private void Request_button_Click(object sender, EventArgs e)
        {
            macros.Vodafone_request(request_textBox.Text,"");
            //var t = GetToken().Result;
        }

        private void checkBox_inshe_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_inshe.Checked is true)
            { textBox_search_object_name_activation.Text = "Інше"; }
            else { textBox_search_object_name_activation.Text = ""; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            macros.Vodafone_GetToken_v2();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            macros.Vodafone_TokenRefresh();
        }

        private void comboBox_tel2_select_KeyPress(object sender, KeyPressEventArgs e)
        {
            button_create_object.Text = "Створити";
            button_create_object.BackColor = Color.Empty;
            maskedTextBox_sim2_no_to_create.Text = "";
            comboBox_tel2_select.BackColor = Color.Empty;
            maskedTextBox_sim2_no_to_create.BackColor = Color.Empty;
            if (e.KeyChar == (char)13)
            {
                if (comboBox_tel2_select.Text.Contains("F") || comboBox_tel2_select.Text.Contains("А"))
                { comboBox_tel2_select.Text = comboBox_tel2_select.Text.Remove(comboBox_tel2_select.Text.Length - 1); }
                DataTable t = new DataTable();
                t = macros.GetData("SELECT idSimcard, Simcardcol_number, Simcardcol_imsi, Simcardcol_international, Simcardcol_deactivated FROM btk.Simcard where Simcardcol_imsi ='" + comboBox_tel2_select.Text + "';");
                if (t != null & t.Rows.Count > 0)
                {
                    if (t.Rows[0][4].ToString() != "1")// if deactivated retorn error
                    {
                        if (t.Rows[0][3].ToString() == "1")//if selected vodafome interalional sim - chenge sim no mask in maskedTextBox_sim_no_to_create
                        {
                            maskedTextBox_sim2_no_to_create.Mask = "";
                            maskedTextBox_sim2_no_to_create.Text = "+" + t.Rows[0][1].ToString();
                            Clipboard.SetText(maskedTextBox_sim2_no_to_create.Text);
                        }
                        else
                        {
                            maskedTextBox_sim2_no_to_create.Mask = "+38\\0000000000";
                            maskedTextBox_sim2_no_to_create.Text = t.Rows[0][1].ToString();
                            Clipboard.SetText(maskedTextBox_sim2_no_to_create.Text);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Картка деактивована");
                        maskedTextBox_sim2_no_to_create.BackColor = Color.Red;
                        maskedTextBox_sim2_no_to_create.Text = t.Rows[0][1].ToString();
                        Clipboard.SetText(maskedTextBox_sim2_no_to_create.Text);
                    }

                }
                else
                {
                    comboBox_tel2_select.Text = "Не знайдено";
                    comboBox_tel2_select.Focus();
                    comboBox_tel2_select.BackColor = Color.Red;
                }
            }
        }

        private void maskedTextBox_sim2_no_to_create_KeyPress(object sender, KeyPressEventArgs e)
        {
            button_create_object.Text = "Створити";
            //maskedTextBox_sim_no_to_create.Text = "";
            comboBox_tel2_select.Text = "";
            button_create_object.BackColor = Color.Empty;
            maskedTextBox_sim2_no_to_create.BackColor = Color.Empty;

            if (e.KeyChar == (char)13)
            {
                string t = "SELECT Simcardcol_imsi FROM btk.Simcard where Simcardcol_number ='" + maskedTextBox_sim2_no_to_create.Text.Substring(4) + "';";
                if (t != "")
                {
                    comboBox_tel2_select.Text = macros.sql_command(t);
                }
            }
        }

        private void linkLabel_zvit_vidpracuvannya_Click(object sender, EventArgs e)
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
                                  "notification.Status, " +
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

        private void linkLabel_zvit_activation_Click(object sender, EventArgs e)
        {
            DataSet table = macros.GetData_dataset(
                    "SELECT " +
                    "Zayavki.idZayavki as 'Заявка №', " +
                    "Activation_object.new_name_obj as 'Назва обєкту', " +
                    "Zayavki.Zayavkicol_VIN as 'VIN', " +
                    "Users.username as 'ПІБ співробітника, що активував', " +
                    "Activation_object.Activation_objectcol_result as 'Результат', " +
                    "Activation_object.comment as 'Коментар', " +
                    "Activation_object.Activation_date as 'Дата активації' " +
                    "FROM " +
                    "btk.Users," +
                    "btk.products," +
                    "btk.Activation_object," +
                    "btk.Zayavki," +
                    "btk.TS_brand," +
                    "btk.TS_model " +
                    "where " +
                    "Activation_object.Object_idObject != '10' " +
                    "and Activation_object.Activation_date between '" + Convert.ToDateTime(dateTime_rep_from.Value).Date.ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(dateTime_rep_to.Value).Date.ToString("yyyy-MM-dd") + "' " +
                    "and(idZayavki like '%" + textBox_search_object_name_activation.Text + "%' " +
                    "or Activation_object.new_name_obj like '%" + textBox_search_object_name_activation.Text + "%' " +
                    "or Zayavkicol_name like '%" + textBox_search_object_name_activation.Text + "%' " +
                    "or Activation_objectcol_result like '" + textBox_search_object_name_activation.Text + "' " +
                    "or Zayavkicol_VIN like '%" + textBox_search_object_name_activation.Text + "%') " +
                    "and TS_brand.idTS_brand = Zayavki.TS_brand_idTS_brand " +
                    "and TS_model.idTS_model = Zayavki.TS_model_idTS_model " +
                    "and products.idproducts = Zayavki.products_idproducts " +
                    "and Activation_object.Users_idUsers=Users.idUsers " +
                    "and Zayavki.Activation_object_idActivation_object = Activation_object.idActivation_object " +
                    "order by idZayavki desc" +
                    "; ");
            macros.ExportDataSet(table);
        }

        private void linkLabel_zvit_testing_Click(object sender, EventArgs e)
        {
            DataSet table = macros.GetData_dataset("SELECT " +
                                   "testing_object.idtesting_object as '№ тестування', " +
                                   "Object.Object_name as 'Назва обекту', " +
                                   "TS_info.TS_infocol_vin as 'VIN', " +
                                   "Users.username as 'ПІБ співробітника, що тестував', " +
                                   "testing_object.testing_objectcol_result as 'Результат', " +
                                   "testing_object.testing_objectcol_edit_timestamp as 'Дата тестування', " +
                                   "testing_object.testing_objectcol_comments as 'Коментар' " +
                                   "FROM " +
                                   "btk.TS_info, " +
                                   "btk.testing_object, " +
                                   "btk.Object, " +
                                   "btk.Users " +
                                   "where " +
                                   "TS_info.idTS_info=Object.TS_info_idTS_info and " +
                                   "testing_object.Object_idObject=Object.idObject and " +
                                   "testing_object.Users_idUsers=Users.idUsers and " +
                                   "(testing_object.testing_objectcol_edit_timestamp  between '" + Convert.ToDateTime(dateTime_rep_from.Value).ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + Convert.ToDateTime(dateTime_rep_to.Value).ToString("yyyy-MM-dd HH:mm:ss") + "') " +
                                   ";");
            macros.ExportDataSet(table);
        }

        private void comboBox_reason_zayavki_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBox_search_zayavki.Text == "" & comboBox_reason_zayavki.Text == "")
            {
                checkBox_zayavki_za_ves_chas.Enabled = false;
                checkBox_zayavki_za_ves_chas.Checked = false;
            }
            else { checkBox_zayavki_za_ves_chas.Enabled = true; }
            
            update_zayavki_na_aktivation_2W();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox_test_search_result.SelectedItems.Count <= 0)
            {
                return;
            }

            vars_form.id_wl_object_for_test = listBox_test_search_result.SelectedValue.ToString();

            string unswer = macros.WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +
                                                     "\"itemsType\":\"avl_unit\"," +
                                                     "\"propName\":\"sys_id\"," +
                                                     "\"propValueMask\":\"" + Convert.ToInt32(listBox_test_search_result.SelectedValue) + "\", " +
                                                     "\"sortType\":\"sys_name\"," +
                                                     "\"or_logic\":\"1\"}," +
                                                     "\"force\":\"1\"," +
                                                     "\"flags\":\"513\"," +
                                                     "\"from\":\"0\"," +
                                                     "\"to\":\"0\"}");// Проверям существует ли данный номер в системе

            var m = JsonConvert.DeserializeObject<RootObject>(unswer);
            int id_cmd_ = 0;
            foreach (var keyvalue in m.items[0].cmds) //Get user account where name like email, contains @
            {
                id_cmd_ ++;
                if (keyvalue.n.Contains("2 - Автозапуск стоп"))
                    create_commads_wl_(Convert.ToInt32(listBox_test_search_result.SelectedValue), id_cmd_, "2 - Автозапуск стоп", "%23autostart=0%23", 16777216);
                if (keyvalue.n.Contains("2 - Автозапуск старт"))
                    create_commads_wl_(Convert.ToInt32(listBox_test_search_result.SelectedValue), id_cmd_, "2 - Автозапуск старт", "%23autostart=1%23", 16777216);

                
            }
            //MessageBox.Show("Ok");
        }
        private string create_commads_wl_(int id_object, int id_cmd, string name_command, string command, int acl_flag)
        {
            string answer = macros.WialonRequest("&svc=unit/update_command_definition&params={"
                                                                                                    + "\"itemId\":\"" + id_object + "\","
                                                                                                    + "\"id\":\""+ id_cmd + "\","
                                                                                                    + "\"callMode\":\"update\","
                                                                                                    + "\"n\":\"" + name_command + "\","
                                                                                                    + "\"c\":\"custom_msg\","
                                                                                                    + "\"l\":\"tcp\","
                                                                                                    + "\"p\":\"" + command + "\","
                                                                                                    + "\"a\":\"" + acl_flag + "\"}");


            return answer;
        }

        private void checkBox_map_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_map.Checked is true)
            {
                Map map = new Map();
                map.Show();
            }
            else 
            {
                Map f = new Map();
                f.Close();
            }
        }

        private void Delet_Testing_button_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Видалити?", "Видалити", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                int rowindex = dataGridView_testing.CurrentCell.RowIndex;
                string testing = dataGridView_testing.Rows[rowindex].Cells[0].Value.ToString();
                string activation = macros.sql_command("Select Activation_object_idActivation_object FROM btk.Zayavki where testing_object_idtesting_object = '" + testing + "';");
                string zayavka = macros.sql_command("Select idZayavki FROM btk.Zayavki where testing_object_idtesting_object = '" + testing + "';");

                macros.sql_command("Delete FROM btk.Zayavki_has_Activation_object where Activation_object_idActivation_object = '" + activation + "' and Zayavki_idZayavki = '" + zayavka + "';");
                macros.sql_command("Delete FROM btk.Zayavki where testing_object_idtesting_object = '" + zayavka + "';");
                macros.sql_command("Delete FROM btk.testing_object where idtesting_object = '" + testing + "';");
                macros.sql_command("Delete FROM btk.Activation_object where idActivation_object = '" + activation + "';");
                MessageBox.Show("Тестирование: " + testing + ", " + "Активація: " + activation + ", " + "Заявка: " + zayavka + " - Удалены!");
            }
            else if (dialogResult == DialogResult.No)
            {
                
                return;
            }  
        }

        private void checkBox_testing_za_ves_chas_search_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_testing_za_ves_chas_search.Checked == true)
            {
                dateTimePicker_testig_filter_start.Enabled = false;
                dateTimePicker_testing_filter_end.Enabled = false;
            }
            else
            {
                dateTimePicker_testig_filter_start.Enabled = true;
                dateTimePicker_testing_filter_end.Enabled = true;
            }
            update_testing_dgv();
        }

        private void textBox_search_object_name_testing_TextChanged(object sender, EventArgs e)
        {
            if (textBox_search_object_name_testing.Text == "")
            {
                checkBox_testing_za_ves_chas_search.Enabled = false;
                checkBox_testing_za_ves_chas_search.Checked = false;
            }
            else { checkBox_testing_za_ves_chas_search.Enabled = true; }
            update_testing_dgv();
        }

        private void dateTimePicker_testig_filter_start_ValueChanged(object sender, EventArgs e)
        {
            update_testing_dgv();
        }

        private void dateTimePicker_testing_filter_end_ValueChanged(object sender, EventArgs e)
        {
            update_testing_dgv();
        }

        private void dateTimePicker_testing_filter_end_DropDown(object sender, EventArgs e)
        {
            
        }

        private void dateTimePicker_testig_filter_start_DropDown(object sender, EventArgs e)
        {

        }

        private void checkBox_testing_nezaversheno_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_testing_nezaversheno.Checked is true)
            { textBox_search_object_name_testing.Text = "Не заверше"; }
            else { textBox_search_object_name_testing.Text = ""; }
        }

        private void checkBox_testing_uspishno_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_testing_uspishno.Checked is true)
            { textBox_search_object_name_testing.Text = "Успішно"; }
            else { textBox_search_object_name_testing.Text = ""; }
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
        public static int kontakts_opened_from { get; set; }//electrik=0, vo=1
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
        public static string Vodafone_AccessToken { get; set; }
        public static int Vodafone_ExpiresInMilliseconds { get; set; }
        public static string Vodafone_RefreshToken { get; set; }
        public static int Vodafone_RefreshTokenExpiresInMilliseconds { get; set; }
        public static DateTime Vodafone_DateTimeTokenCreate { get; set; }
        public static string MapFormIdObject { get; set; }
        public static string MapFormName { get; set; }
    }
}