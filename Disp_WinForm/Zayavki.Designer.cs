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
            this.label3 = new System.Windows.Forms.Label();
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
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_name_zayavka = new System.Windows.Forms.TextBox();
            this.comboBox_date_vipuska_zayavki = new System.Windows.Forms.ComboBox();
            this.dataGridView_tested_objects_zayavki = new System.Windows.Forms.DataGridView();
            this.dateTimePicker_filter_tested_zayavki = new System.Windows.Forms.DateTimePicker();
            this.checkBox_createt_zayavka = new System.Windows.Forms.CheckBox();
            this.textBox_Coments = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_sobstvennik_avto = new System.Windows.Forms.TextBox();
            this.textBox_kont_osoba1 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox_kont_osoba2 = new System.Windows.Forms.TextBox();
            this.textBox_tel2 = new System.Windows.Forms.TextBox();
            this.textBox_tel1 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox_selected_object = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox_id_testing = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_tested_objects_zayavki)).BeginInit();
            this.SuspendLayout();
            // 
            // button_create
            // 
            this.button_create.Location = new System.Drawing.Point(708, 24);
            this.button_create.Name = "button_create";
            this.button_create.Size = new System.Drawing.Size(75, 23);
            this.button_create.TabIndex = 0;
            this.button_create.Text = "Створити";
            this.button_create.UseVisualStyleBackColor = true;
            this.button_create.Click += new System.EventHandler(this.button_create_Click);
            // 
            // textBox_vin_zayavki
            // 
            this.textBox_vin_zayavki.Location = new System.Drawing.Point(94, 188);
            this.textBox_vin_zayavki.Name = "textBox_vin_zayavki";
            this.textBox_vin_zayavki.Size = new System.Drawing.Size(291, 20);
            this.textBox_vin_zayavki.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 191);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "VIN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 269);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Продукт";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 243);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Модель";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 217);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Бренд";
            // 
            // dateTimePicker_plan_date_zayavki
            // 
            this.dateTimePicker_plan_date_zayavki.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_plan_date_zayavki.Location = new System.Drawing.Point(94, 47);
            this.dateTimePicker_plan_date_zayavki.Name = "dateTimePicker_plan_date_zayavki";
            this.dateTimePicker_plan_date_zayavki.Size = new System.Drawing.Size(291, 20);
            this.dateTimePicker_plan_date_zayavki.TabIndex = 10;
            // 
            // comboBox_reason_zayavki
            // 
            this.comboBox_reason_zayavki.FormattingEnabled = true;
            this.comboBox_reason_zayavki.Items.AddRange(new object[] {
            "Монтаж",
            "Демонтаж",
            "Інше"});
            this.comboBox_reason_zayavki.Location = new System.Drawing.Point(94, 73);
            this.comboBox_reason_zayavki.Name = "comboBox_reason_zayavki";
            this.comboBox_reason_zayavki.Size = new System.Drawing.Size(291, 21);
            this.comboBox_reason_zayavki.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 295);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Встановлює";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 321);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Держ. Номер";
            // 
            // textBox_kontragent_sto_zayavki
            // 
            this.textBox_kontragent_sto_zayavki.Enabled = false;
            this.textBox_kontragent_sto_zayavki.Location = new System.Drawing.Point(94, 292);
            this.textBox_kontragent_sto_zayavki.Name = "textBox_kontragent_sto_zayavki";
            this.textBox_kontragent_sto_zayavki.Size = new System.Drawing.Size(239, 20);
            this.textBox_kontragent_sto_zayavki.TabIndex = 14;
            // 
            // textBox_license_plate_zayavki
            // 
            this.textBox_license_plate_zayavki.Location = new System.Drawing.Point(94, 318);
            this.textBox_license_plate_zayavki.Name = "textBox_license_plate_zayavki";
            this.textBox_license_plate_zayavki.Size = new System.Drawing.Size(291, 20);
            this.textBox_license_plate_zayavki.TabIndex = 13;
            // 
            // button_select_kontragent_sto_zayavki
            // 
            this.button_select_kontragent_sto_zayavki.Location = new System.Drawing.Point(339, 290);
            this.button_select_kontragent_sto_zayavki.Name = "button_select_kontragent_sto_zayavki";
            this.button_select_kontragent_sto_zayavki.Size = new System.Drawing.Size(46, 23);
            this.button_select_kontragent_sto_zayavki.TabIndex = 142;
            this.button_select_kontragent_sto_zayavki.Text = "+";
            this.button_select_kontragent_sto_zayavki.UseVisualStyleBackColor = true;
            this.button_select_kontragent_sto_zayavki.Click += new System.EventHandler(this.button_select_kontragent_sto_zayavki_Click);
            // 
            // comboBox_product_zayavki
            // 
            this.comboBox_product_zayavki.FormattingEnabled = true;
            this.comboBox_product_zayavki.Location = new System.Drawing.Point(94, 266);
            this.comboBox_product_zayavki.Name = "comboBox_product_zayavki";
            this.comboBox_product_zayavki.Size = new System.Drawing.Size(291, 21);
            this.comboBox_product_zayavki.TabIndex = 143;
            // 
            // comboBox_model_zayavki
            // 
            this.comboBox_model_zayavki.FormattingEnabled = true;
            this.comboBox_model_zayavki.Location = new System.Drawing.Point(94, 241);
            this.comboBox_model_zayavki.Name = "comboBox_model_zayavki";
            this.comboBox_model_zayavki.Size = new System.Drawing.Size(291, 21);
            this.comboBox_model_zayavki.TabIndex = 144;
            // 
            // comboBox_brand_zayavki
            // 
            this.comboBox_brand_zayavki.FormattingEnabled = true;
            this.comboBox_brand_zayavki.Location = new System.Drawing.Point(94, 214);
            this.comboBox_brand_zayavki.Name = "comboBox_brand_zayavki";
            this.comboBox_brand_zayavki.Size = new System.Drawing.Size(291, 21);
            this.comboBox_brand_zayavki.TabIndex = 145;
            this.comboBox_brand_zayavki.DropDown += new System.EventHandler(this.comboBox_brand_zayavki_DropDown);
            this.comboBox_brand_zayavki.DropDownClosed += new System.EventHandler(this.comboBox_brand_zayavki_DropDownClosed);
            // 
            // button_select_kontraget_zakazchik_zayavki
            // 
            this.button_select_kontraget_zakazchik_zayavki.Location = new System.Drawing.Point(339, 342);
            this.button_select_kontraget_zakazchik_zayavki.Name = "button_select_kontraget_zakazchik_zayavki";
            this.button_select_kontraget_zakazchik_zayavki.Size = new System.Drawing.Size(46, 23);
            this.button_select_kontraget_zakazchik_zayavki.TabIndex = 148;
            this.button_select_kontraget_zakazchik_zayavki.Text = "+";
            this.button_select_kontraget_zakazchik_zayavki.UseVisualStyleBackColor = true;
            this.button_select_kontraget_zakazchik_zayavki.Click += new System.EventHandler(this.button_select_kontraget_zakazchik_zayavki_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 347);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 147;
            this.label8.Text = "Замовник";
            // 
            // textBox_kontragent_zakazchik
            // 
            this.textBox_kontragent_zakazchik.Enabled = false;
            this.textBox_kontragent_zakazchik.Location = new System.Drawing.Point(94, 344);
            this.textBox_kontragent_zakazchik.Name = "textBox_kontragent_zakazchik";
            this.textBox_kontragent_zakazchik.Size = new System.Drawing.Size(239, 20);
            this.textBox_kontragent_zakazchik.TabIndex = 146;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 376);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 150;
            this.label9.Text = "Рік вироб.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 54);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 13);
            this.label10.TabIndex = 151;
            this.label10.Text = "План дата";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 76);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 13);
            this.label11.TabIndex = 152;
            this.label11.Text = "Заявка на:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(58, 13);
            this.label12.TabIndex = 154;
            this.label12.Text = "Заявка №";
            // 
            // textBox_name_zayavka
            // 
            this.textBox_name_zayavka.Location = new System.Drawing.Point(94, 21);
            this.textBox_name_zayavka.Name = "textBox_name_zayavka";
            this.textBox_name_zayavka.Size = new System.Drawing.Size(291, 20);
            this.textBox_name_zayavka.TabIndex = 153;
            // 
            // comboBox_date_vipuska_zayavki
            // 
            this.comboBox_date_vipuska_zayavki.FormattingEnabled = true;
            this.comboBox_date_vipuska_zayavki.Location = new System.Drawing.Point(94, 373);
            this.comboBox_date_vipuska_zayavki.Name = "comboBox_date_vipuska_zayavki";
            this.comboBox_date_vipuska_zayavki.Size = new System.Drawing.Size(291, 21);
            this.comboBox_date_vipuska_zayavki.TabIndex = 155;
            // 
            // dataGridView_tested_objects_zayavki
            // 
            this.dataGridView_tested_objects_zayavki.AllowUserToAddRows = false;
            this.dataGridView_tested_objects_zayavki.AllowUserToDeleteRows = false;
            this.dataGridView_tested_objects_zayavki.AllowUserToResizeColumns = false;
            this.dataGridView_tested_objects_zayavki.AllowUserToResizeRows = false;
            this.dataGridView_tested_objects_zayavki.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView_tested_objects_zayavki.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_tested_objects_zayavki.Location = new System.Drawing.Point(18, 457);
            this.dataGridView_tested_objects_zayavki.MultiSelect = false;
            this.dataGridView_tested_objects_zayavki.Name = "dataGridView_tested_objects_zayavki";
            this.dataGridView_tested_objects_zayavki.ReadOnly = true;
            this.dataGridView_tested_objects_zayavki.RowHeadersVisible = false;
            this.dataGridView_tested_objects_zayavki.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_tested_objects_zayavki.Size = new System.Drawing.Size(766, 172);
            this.dataGridView_tested_objects_zayavki.TabIndex = 156;
            this.dataGridView_tested_objects_zayavki.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_tested_objects_zayavki_CellDoubleClick);
            // 
            // dateTimePicker_filter_tested_zayavki
            // 
            this.dateTimePicker_filter_tested_zayavki.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker_filter_tested_zayavki.Location = new System.Drawing.Point(587, 393);
            this.dateTimePicker_filter_tested_zayavki.Name = "dateTimePicker_filter_tested_zayavki";
            this.dateTimePicker_filter_tested_zayavki.Size = new System.Drawing.Size(80, 20);
            this.dateTimePicker_filter_tested_zayavki.TabIndex = 157;
            this.dateTimePicker_filter_tested_zayavki.ValueChanged += new System.EventHandler(this.dateTimePicker_filter_tested_zayavki_ValueChanged);
            // 
            // checkBox_createt_zayavka
            // 
            this.checkBox_createt_zayavka.AutoSize = true;
            this.checkBox_createt_zayavka.Location = new System.Drawing.Point(587, 370);
            this.checkBox_createt_zayavka.Name = "checkBox_createt_zayavka";
            this.checkBox_createt_zayavka.Size = new System.Drawing.Size(182, 17);
            this.checkBox_createt_zayavka.TabIndex = 159;
            this.checkBox_createt_zayavka.Text = "Створена заявка на активацію";
            this.checkBox_createt_zayavka.UseVisualStyleBackColor = true;
            this.checkBox_createt_zayavka.CheckedChanged += new System.EventHandler(this.checkBox_createt_zayavka_CheckedChanged);
            // 
            // textBox_Coments
            // 
            this.textBox_Coments.Location = new System.Drawing.Point(436, 83);
            this.textBox_Coments.Multiline = true;
            this.textBox_Coments.Name = "textBox_Coments";
            this.textBox_Coments.Size = new System.Drawing.Size(347, 81);
            this.textBox_Coments.TabIndex = 160;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 403);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 162;
            this.label7.Text = "Власник авто";
            // 
            // textBox_sobstvennik_avto
            // 
            this.textBox_sobstvennik_avto.Location = new System.Drawing.Point(94, 400);
            this.textBox_sobstvennik_avto.Name = "textBox_sobstvennik_avto";
            this.textBox_sobstvennik_avto.Size = new System.Drawing.Size(291, 20);
            this.textBox_sobstvennik_avto.TabIndex = 161;
            // 
            // textBox_kont_osoba1
            // 
            this.textBox_kont_osoba1.Location = new System.Drawing.Point(94, 100);
            this.textBox_kont_osoba1.Name = "textBox_kont_osoba1";
            this.textBox_kont_osoba1.Size = new System.Drawing.Size(184, 20);
            this.textBox_kont_osoba1.TabIndex = 163;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(14, 129);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(62, 13);
            this.label14.TabIndex = 166;
            this.label14.Text = "Кон. особа";
            // 
            // textBox_kont_osoba2
            // 
            this.textBox_kont_osoba2.Location = new System.Drawing.Point(94, 129);
            this.textBox_kont_osoba2.Name = "textBox_kont_osoba2";
            this.textBox_kont_osoba2.Size = new System.Drawing.Size(184, 20);
            this.textBox_kont_osoba2.TabIndex = 165;
            // 
            // textBox_tel2
            // 
            this.textBox_tel2.Location = new System.Drawing.Point(284, 129);
            this.textBox_tel2.Name = "textBox_tel2";
            this.textBox_tel2.Size = new System.Drawing.Size(101, 20);
            this.textBox_tel2.TabIndex = 167;
            // 
            // textBox_tel1
            // 
            this.textBox_tel1.Location = new System.Drawing.Point(284, 100);
            this.textBox_tel1.Name = "textBox_tel1";
            this.textBox_tel1.Size = new System.Drawing.Size(101, 20);
            this.textBox_tel1.TabIndex = 169;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 103);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(62, 13);
            this.label13.TabIndex = 164;
            this.label13.Text = "Кон. особа";
            // 
            // textBox_selected_object
            // 
            this.textBox_selected_object.Location = new System.Drawing.Point(138, 426);
            this.textBox_selected_object.Name = "textBox_selected_object";
            this.textBox_selected_object.ReadOnly = true;
            this.textBox_selected_object.Size = new System.Drawing.Size(247, 20);
            this.textBox_selected_object.TabIndex = 170;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(15, 428);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(66, 13);
            this.label15.TabIndex = 171;
            this.label15.Text = "Тестування";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(449, 393);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 172;
            // 
            // textBox_id_testing
            // 
            this.textBox_id_testing.Location = new System.Drawing.Point(94, 426);
            this.textBox_id_testing.Name = "textBox_id_testing";
            this.textBox_id_testing.ReadOnly = true;
            this.textBox_id_testing.Size = new System.Drawing.Size(38, 20);
            this.textBox_id_testing.TabIndex = 173;
            // 
            // Zayavki
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 643);
            this.Controls.Add(this.textBox_id_testing);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.textBox_selected_object);
            this.Controls.Add(this.textBox_tel1);
            this.Controls.Add(this.textBox_tel2);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.textBox_kont_osoba2);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBox_kont_osoba1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox_sobstvennik_avto);
            this.Controls.Add(this.textBox_Coments);
            this.Controls.Add(this.checkBox_createt_zayavka);
            this.Controls.Add(this.dateTimePicker_filter_tested_zayavki);
            this.Controls.Add(this.dataGridView_tested_objects_zayavki);
            this.Controls.Add(this.comboBox_date_vipuska_zayavki);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBox_name_zayavka);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button_select_kontraget_zakazchik_zayavki);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox_kontragent_zakazchik);
            this.Controls.Add(this.comboBox_brand_zayavki);
            this.Controls.Add(this.comboBox_model_zayavki);
            this.Controls.Add(this.comboBox_product_zayavki);
            this.Controls.Add(this.button_select_kontragent_sto_zayavki);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox_kontragent_sto_zayavki);
            this.Controls.Add(this.textBox_license_plate_zayavki);
            this.Controls.Add(this.comboBox_reason_zayavki);
            this.Controls.Add(this.dateTimePicker_plan_date_zayavki);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_vin_zayavki);
            this.Controls.Add(this.button_create);
            this.Name = "Zayavki";
            this.Text = "Zayavki";
            this.Load += new System.EventHandler(this.Zayavki_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_tested_objects_zayavki)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_create;
        private System.Windows.Forms.TextBox textBox_vin_zayavki;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
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
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_name_zayavka;
        private System.Windows.Forms.ComboBox comboBox_date_vipuska_zayavki;
        private System.Windows.Forms.DataGridView dataGridView_tested_objects_zayavki;
        private System.Windows.Forms.DateTimePicker dateTimePicker_filter_tested_zayavki;
        private System.Windows.Forms.CheckBox checkBox_createt_zayavka;
        private System.Windows.Forms.TextBox textBox_Coments;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_sobstvennik_avto;
        private System.Windows.Forms.TextBox textBox_kont_osoba1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox_kont_osoba2;
        private System.Windows.Forms.TextBox textBox_tel2;
        private System.Windows.Forms.TextBox textBox_tel1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox_selected_object;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox_id_testing;
    }
}