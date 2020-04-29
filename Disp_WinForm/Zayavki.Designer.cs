namespace Disp_WinForm
{
    partial class Zayavki
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_create = new System.Windows.Forms.Button();
            this.textBox_vin_zayavki = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dateTimePicker_plan_date_zayavki = new System.Windows.Forms.DateTimePicker();
            this.comboBox_reason_zayavki = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_kontragent_sto_zayavki = new System.Windows.Forms.TextBox();
            this.textBox_license_plate_zayavki = new System.Windows.Forms.TextBox();
            this.button_select_kontragent_sto_zayavki = new System.Windows.Forms.Button();
            this.comboBox_product_zayavki = new System.Windows.Forms.ComboBox();
            this.comboBox_model_zayavki = new System.Windows.Forms.ComboBox();
            this.comboBox_brand_zayavki = new System.Windows.Forms.ComboBox();
            this.button_select_kontraget_zakazchik_zayavki = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_kontragent_zakazchik = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_name_zayavka = new System.Windows.Forms.TextBox();
            this.comboBox_date_vipuska_zayavki = new System.Windows.Forms.ComboBox();
            this.dataGridView_tested_objects_zayavki = new System.Windows.Forms.DataGridView();
            this.dateTimePicker_filter_tested_zayavki = new System.Windows.Forms.DateTimePicker();
            this.textBox_Coments = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_kont_osoba1 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox_kont_osoba2 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox_selected_object = new System.Windows.Forms.TextBox();
            this.comboBox_filter = new System.Windows.Forms.ComboBox();
            this.textBox_id_testing = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBox_email = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.maskedTextBox_tel2 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBox_tel1 = new System.Windows.Forms.MaskedTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.placeHolderTextBox_sobstvennik_avto = new Disp_WinForm.PlaceHolderTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button_vidkripyty = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_tested_objects_zayavki)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_create
            // 
            this.button_create.Location = new System.Drawing.Point(719, 489);
            this.button_create.Name = "button_create";
            this.button_create.Size = new System.Drawing.Size(75, 23);
            this.button_create.TabIndex = 21;
            this.button_create.Text = "Створити";
            this.button_create.UseVisualStyleBackColor = true;
            this.button_create.Click += new System.EventHandler(this.button_create_Click);
            // 
            // textBox_vin_zayavki
            // 
            this.textBox_vin_zayavki.Location = new System.Drawing.Point(90, 125);
            this.textBox_vin_zayavki.Name = "textBox_vin_zayavki";
            this.textBox_vin_zayavki.Size = new System.Drawing.Size(319, 20);
            this.textBox_vin_zayavki.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "VIN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Продукт";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Авто";
            // 
            // dateTimePicker_plan_date_zayavki
            // 
            this.dateTimePicker_plan_date_zayavki.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_plan_date_zayavki.Location = new System.Drawing.Point(84, 45);
            this.dateTimePicker_plan_date_zayavki.Name = "dateTimePicker_plan_date_zayavki";
            this.dateTimePicker_plan_date_zayavki.Size = new System.Drawing.Size(357, 20);
            this.dateTimePicker_plan_date_zayavki.TabIndex = 2;
            // 
            // comboBox_reason_zayavki
            // 
            this.comboBox_reason_zayavki.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_reason_zayavki.FormattingEnabled = true;
            this.comboBox_reason_zayavki.Items.AddRange(new object[] {
            "Монтаж",
            "Демонтаж",
            "Інше"});
            this.comboBox_reason_zayavki.Location = new System.Drawing.Point(84, 71);
            this.comboBox_reason_zayavki.Name = "comboBox_reason_zayavki";
            this.comboBox_reason_zayavki.Size = new System.Drawing.Size(357, 21);
            this.comboBox_reason_zayavki.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 207);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Встановлює";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Держ. Номер";
            // 
            // textBox_kontragent_sto_zayavki
            // 
            this.textBox_kontragent_sto_zayavki.Enabled = false;
            this.textBox_kontragent_sto_zayavki.Location = new System.Drawing.Point(90, 203);
            this.textBox_kontragent_sto_zayavki.Name = "textBox_kontragent_sto_zayavki";
            this.textBox_kontragent_sto_zayavki.ReadOnly = true;
            this.textBox_kontragent_sto_zayavki.Size = new System.Drawing.Size(277, 20);
            this.textBox_kontragent_sto_zayavki.TabIndex = 14;
            // 
            // textBox_license_plate_zayavki
            // 
            this.textBox_license_plate_zayavki.Location = new System.Drawing.Point(90, 99);
            this.textBox_license_plate_zayavki.Name = "textBox_license_plate_zayavki";
            this.textBox_license_plate_zayavki.Size = new System.Drawing.Size(319, 20);
            this.textBox_license_plate_zayavki.TabIndex = 13;
            // 
            // button_select_kontragent_sto_zayavki
            // 
            this.button_select_kontragent_sto_zayavki.Location = new System.Drawing.Point(373, 203);
            this.button_select_kontragent_sto_zayavki.Name = "button_select_kontragent_sto_zayavki";
            this.button_select_kontragent_sto_zayavki.Size = new System.Drawing.Size(36, 20);
            this.button_select_kontragent_sto_zayavki.TabIndex = 17;
            this.button_select_kontragent_sto_zayavki.Text = "+";
            this.button_select_kontragent_sto_zayavki.UseVisualStyleBackColor = true;
            this.button_select_kontragent_sto_zayavki.Click += new System.EventHandler(this.button_select_kontragent_sto_zayavki_Click);
            // 
            // comboBox_product_zayavki
            // 
            this.comboBox_product_zayavki.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_product_zayavki.FormattingEnabled = true;
            this.comboBox_product_zayavki.Location = new System.Drawing.Point(90, 45);
            this.comboBox_product_zayavki.Name = "comboBox_product_zayavki";
            this.comboBox_product_zayavki.Size = new System.Drawing.Size(319, 21);
            this.comboBox_product_zayavki.TabIndex = 9;
            // 
            // comboBox_model_zayavki
            // 
            this.comboBox_model_zayavki.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_model_zayavki.FormattingEnabled = true;
            this.comboBox_model_zayavki.Location = new System.Drawing.Point(195, 72);
            this.comboBox_model_zayavki.Name = "comboBox_model_zayavki";
            this.comboBox_model_zayavki.Size = new System.Drawing.Size(151, 21);
            this.comboBox_model_zayavki.TabIndex = 11;
            // 
            // comboBox_brand_zayavki
            // 
            this.comboBox_brand_zayavki.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_brand_zayavki.FormattingEnabled = true;
            this.comboBox_brand_zayavki.Location = new System.Drawing.Point(90, 72);
            this.comboBox_brand_zayavki.Name = "comboBox_brand_zayavki";
            this.comboBox_brand_zayavki.Size = new System.Drawing.Size(99, 21);
            this.comboBox_brand_zayavki.TabIndex = 10;
            this.comboBox_brand_zayavki.DropDown += new System.EventHandler(this.comboBox_brand_zayavki_DropDown);
            this.comboBox_brand_zayavki.DropDownClosed += new System.EventHandler(this.comboBox_brand_zayavki_DropDownClosed);
            // 
            // button_select_kontraget_zakazchik_zayavki
            // 
            this.button_select_kontraget_zakazchik_zayavki.Location = new System.Drawing.Point(373, 19);
            this.button_select_kontraget_zakazchik_zayavki.Name = "button_select_kontraget_zakazchik_zayavki";
            this.button_select_kontraget_zakazchik_zayavki.Size = new System.Drawing.Size(36, 20);
            this.button_select_kontraget_zakazchik_zayavki.TabIndex = 8;
            this.button_select_kontraget_zakazchik_zayavki.Text = "+";
            this.button_select_kontraget_zakazchik_zayavki.UseVisualStyleBackColor = true;
            this.button_select_kontraget_zakazchik_zayavki.Click += new System.EventHandler(this.button_select_kontraget_zakazchik_zayavki_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 147;
            this.label8.Text = "Замовник";
            // 
            // textBox_kontragent_zakazchik
            // 
            this.textBox_kontragent_zakazchik.Enabled = false;
            this.textBox_kontragent_zakazchik.Location = new System.Drawing.Point(90, 19);
            this.textBox_kontragent_zakazchik.Name = "textBox_kontragent_zakazchik";
            this.textBox_kontragent_zakazchik.ReadOnly = true;
            this.textBox_kontragent_zakazchik.Size = new System.Drawing.Size(277, 20);
            this.textBox_kontragent_zakazchik.TabIndex = 146;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 49);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 13);
            this.label10.TabIndex = 151;
            this.label10.Text = "План дата";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 75);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 13);
            this.label11.TabIndex = 152;
            this.label11.Text = "Заявка на:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 23);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(58, 13);
            this.label12.TabIndex = 154;
            this.label12.Text = "Заявка №";
            // 
            // textBox_name_zayavka
            // 
            this.textBox_name_zayavka.Location = new System.Drawing.Point(84, 19);
            this.textBox_name_zayavka.Name = "textBox_name_zayavka";
            this.textBox_name_zayavka.Size = new System.Drawing.Size(357, 20);
            this.textBox_name_zayavka.TabIndex = 1;
            // 
            // comboBox_date_vipuska_zayavki
            // 
            this.comboBox_date_vipuska_zayavki.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_date_vipuska_zayavki.FormattingEnabled = true;
            this.comboBox_date_vipuska_zayavki.Location = new System.Drawing.Point(352, 72);
            this.comboBox_date_vipuska_zayavki.Name = "comboBox_date_vipuska_zayavki";
            this.comboBox_date_vipuska_zayavki.Size = new System.Drawing.Size(57, 21);
            this.comboBox_date_vipuska_zayavki.TabIndex = 12;
            // 
            // dataGridView_tested_objects_zayavki
            // 
            this.dataGridView_tested_objects_zayavki.AllowUserToAddRows = false;
            this.dataGridView_tested_objects_zayavki.AllowUserToDeleteRows = false;
            this.dataGridView_tested_objects_zayavki.AllowUserToResizeColumns = false;
            this.dataGridView_tested_objects_zayavki.AllowUserToResizeRows = false;
            this.dataGridView_tested_objects_zayavki.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView_tested_objects_zayavki.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_tested_objects_zayavki.Location = new System.Drawing.Point(6, 56);
            this.dataGridView_tested_objects_zayavki.MultiSelect = false;
            this.dataGridView_tested_objects_zayavki.Name = "dataGridView_tested_objects_zayavki";
            this.dataGridView_tested_objects_zayavki.ReadOnly = true;
            this.dataGridView_tested_objects_zayavki.RowHeadersVisible = false;
            this.dataGridView_tested_objects_zayavki.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_tested_objects_zayavki.Size = new System.Drawing.Size(862, 165);
            this.dataGridView_tested_objects_zayavki.TabIndex = 156;
            this.dataGridView_tested_objects_zayavki.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_tested_objects_zayavki_CellDoubleClick);
            // 
            // dateTimePicker_filter_tested_zayavki
            // 
            this.dateTimePicker_filter_tested_zayavki.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_filter_tested_zayavki.Location = new System.Drawing.Point(788, 23);
            this.dateTimePicker_filter_tested_zayavki.Name = "dateTimePicker_filter_tested_zayavki";
            this.dateTimePicker_filter_tested_zayavki.Size = new System.Drawing.Size(80, 20);
            this.dateTimePicker_filter_tested_zayavki.TabIndex = 20;
            this.dateTimePicker_filter_tested_zayavki.CloseUp += new System.EventHandler(this.dateTimePicker_filter_tested_zayavki_CloseUp);
            // 
            // textBox_Coments
            // 
            this.textBox_Coments.Location = new System.Drawing.Point(6, 20);
            this.textBox_Coments.Multiline = true;
            this.textBox_Coments.Name = "textBox_Coments";
            this.textBox_Coments.Size = new System.Drawing.Size(435, 39);
            this.textBox_Coments.TabIndex = 160;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 155);
            this.label7.MaximumSize = new System.Drawing.Size(100, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 162;
            this.label7.Text = "Власник авто";
            // 
            // textBox_kont_osoba1
            // 
            this.textBox_kont_osoba1.Location = new System.Drawing.Point(84, 98);
            this.textBox_kont_osoba1.Name = "textBox_kont_osoba1";
            this.textBox_kont_osoba1.Size = new System.Drawing.Size(272, 20);
            this.textBox_kont_osoba1.TabIndex = 4;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 131);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(62, 13);
            this.label14.TabIndex = 166;
            this.label14.Text = "Кон. особа";
            // 
            // textBox_kont_osoba2
            // 
            this.textBox_kont_osoba2.Location = new System.Drawing.Point(84, 127);
            this.textBox_kont_osoba2.Name = "textBox_kont_osoba2";
            this.textBox_kont_osoba2.Size = new System.Drawing.Size(272, 20);
            this.textBox_kont_osoba2.TabIndex = 6;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 102);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(62, 13);
            this.label13.TabIndex = 164;
            this.label13.Text = "Кон. особа";
            // 
            // textBox_selected_object
            // 
            this.textBox_selected_object.Location = new System.Drawing.Point(51, 23);
            this.textBox_selected_object.Name = "textBox_selected_object";
            this.textBox_selected_object.ReadOnly = true;
            this.textBox_selected_object.Size = new System.Drawing.Size(247, 20);
            this.textBox_selected_object.TabIndex = 170;
            // 
            // comboBox_filter
            // 
            this.comboBox_filter.FormattingEnabled = true;
            this.comboBox_filter.Items.AddRange(new object[] {
            "Не прикріплені",
            "Прикріплені",
            "Всі"});
            this.comboBox_filter.Location = new System.Drawing.Point(661, 23);
            this.comboBox_filter.Name = "comboBox_filter";
            this.comboBox_filter.Size = new System.Drawing.Size(121, 21);
            this.comboBox_filter.TabIndex = 19;
            this.comboBox_filter.DropDownClosed += new System.EventHandler(this.comboBox_filter_DropDownClosed);
            // 
            // textBox_id_testing
            // 
            this.textBox_id_testing.Location = new System.Drawing.Point(7, 23);
            this.textBox_id_testing.Name = "textBox_id_testing";
            this.textBox_id_testing.ReadOnly = true;
            this.textBox_id_testing.Size = new System.Drawing.Size(38, 20);
            this.textBox_id_testing.TabIndex = 173;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(12, 181);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(35, 13);
            this.label18.TabIndex = 177;
            this.label18.Text = "E-mail";
            // 
            // textBox_email
            // 
            this.textBox_email.Location = new System.Drawing.Point(90, 177);
            this.textBox_email.Name = "textBox_email";
            this.textBox_email.Size = new System.Drawing.Size(319, 20);
            this.textBox_email.TabIndex = 16;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.maskedTextBox_tel2);
            this.groupBox1.Controls.Add(this.textBox_name_zayavka);
            this.groupBox1.Controls.Add(this.maskedTextBox_tel1);
            this.groupBox1.Controls.Add(this.dateTimePicker_plan_date_zayavki);
            this.groupBox1.Controls.Add(this.comboBox_reason_zayavki);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.textBox_kont_osoba1);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.textBox_kont_osoba2);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(447, 158);
            this.groupBox1.TabIndex = 178;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Загальна інформація";
            // 
            // maskedTextBox_tel2
            // 
            this.maskedTextBox_tel2.Location = new System.Drawing.Point(362, 128);
            this.maskedTextBox_tel2.Mask = "999-000-0000";
            this.maskedTextBox_tel2.Name = "maskedTextBox_tel2";
            this.maskedTextBox_tel2.Size = new System.Drawing.Size(79, 20);
            this.maskedTextBox_tel2.TabIndex = 7;
            // 
            // maskedTextBox_tel1
            // 
            this.maskedTextBox_tel1.Location = new System.Drawing.Point(362, 98);
            this.maskedTextBox_tel1.Mask = "999-000-0000";
            this.maskedTextBox_tel1.Name = "maskedTextBox_tel1";
            this.maskedTextBox_tel1.Size = new System.Drawing.Size(79, 20);
            this.maskedTextBox_tel1.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.placeHolderTextBox_sobstvennik_avto);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.textBox_vin_zayavki);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBox_email);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBox_kontragent_sto_zayavki);
            this.groupBox2.Controls.Add(this.textBox_license_plate_zayavki);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.button_select_kontragent_sto_zayavki);
            this.groupBox2.Controls.Add(this.comboBox_model_zayavki);
            this.groupBox2.Controls.Add(this.comboBox_brand_zayavki);
            this.groupBox2.Controls.Add(this.comboBox_date_vipuska_zayavki);
            this.groupBox2.Controls.Add(this.comboBox_product_zayavki);
            this.groupBox2.Controls.Add(this.textBox_kontragent_zakazchik);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.button_select_kontraget_zakazchik_zayavki);
            this.groupBox2.Location = new System.Drawing.Point(465, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(421, 238);
            this.groupBox2.TabIndex = 179;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Інформація про обєкт";
            // 
            // placeHolderTextBox_sobstvennik_avto
            // 
            this.placeHolderTextBox_sobstvennik_avto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic);
            this.placeHolderTextBox_sobstvennik_avto.Location = new System.Drawing.Point(90, 151);
            this.placeHolderTextBox_sobstvennik_avto.Name = "placeHolderTextBox_sobstvennik_avto";
            this.placeHolderTextBox_sobstvennik_avto.PlaceHolderText = "Фамілія або назва підприємства";
            this.placeHolderTextBox_sobstvennik_avto.Size = new System.Drawing.Size(319, 20);
            this.placeHolderTextBox_sobstvennik_avto.TabIndex = 15;
            this.placeHolderTextBox_sobstvennik_avto.TextColor = System.Drawing.Color.Gray;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox_Coments);
            this.groupBox3.Location = new System.Drawing.Point(12, 176);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(447, 74);
            this.groupBox3.TabIndex = 180;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Коментар";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button_vidkripyty);
            this.groupBox4.Controls.Add(this.dataGridView_tested_objects_zayavki);
            this.groupBox4.Controls.Add(this.dateTimePicker_filter_tested_zayavki);
            this.groupBox4.Controls.Add(this.textBox_selected_object);
            this.groupBox4.Controls.Add(this.comboBox_filter);
            this.groupBox4.Controls.Add(this.textBox_id_testing);
            this.groupBox4.Location = new System.Drawing.Point(12, 256);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(874, 227);
            this.groupBox4.TabIndex = 181;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Прикріплення тестування";
            // 
            // button_vidkripyty
            // 
            this.button_vidkripyty.Location = new System.Drawing.Point(310, 22);
            this.button_vidkripyty.Name = "button_vidkripyty";
            this.button_vidkripyty.Size = new System.Drawing.Size(75, 23);
            this.button_vidkripyty.TabIndex = 18;
            this.button_vidkripyty.Text = "Відкріпити";
            this.button_vidkripyty.UseVisualStyleBackColor = true;
            this.button_vidkripyty.Click += new System.EventHandler(this.button_vidkripyty_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(811, 489);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 22;
            this.button_cancel.Text = "Відмінити";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // Zayavki
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 520);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_create);
            this.Name = "Zayavki";
            this.Text = "Zayavki";
            this.Load += new System.EventHandler(this.Zayavki_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_tested_objects_zayavki)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_create;
        private System.Windows.Forms.TextBox textBox_vin_zayavki;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateTimePicker_plan_date_zayavki;
        private System.Windows.Forms.ComboBox comboBox_reason_zayavki;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_kontragent_sto_zayavki;
        private System.Windows.Forms.TextBox textBox_license_plate_zayavki;
        private System.Windows.Forms.Button button_select_kontragent_sto_zayavki;
        private System.Windows.Forms.ComboBox comboBox_product_zayavki;
        private System.Windows.Forms.ComboBox comboBox_model_zayavki;
        private System.Windows.Forms.ComboBox comboBox_brand_zayavki;
        private System.Windows.Forms.Button button_select_kontraget_zakazchik_zayavki;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_kontragent_zakazchik;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_name_zayavka;
        private System.Windows.Forms.ComboBox comboBox_date_vipuska_zayavki;
        private System.Windows.Forms.DataGridView dataGridView_tested_objects_zayavki;
        private System.Windows.Forms.DateTimePicker dateTimePicker_filter_tested_zayavki;
        private System.Windows.Forms.TextBox textBox_Coments;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_kont_osoba1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox_kont_osoba2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox_selected_object;
        private System.Windows.Forms.ComboBox comboBox_filter;
        private System.Windows.Forms.TextBox textBox_id_testing;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBox_email;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.MaskedTextBox maskedTextBox_tel2;
        private System.Windows.Forms.MaskedTextBox maskedTextBox_tel1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button_vidkripyty;
        private System.Windows.Forms.Button button_cancel;
        private PlaceHolderTextBox placeHolderTextBox_sobstvennik_avto;
    }
}