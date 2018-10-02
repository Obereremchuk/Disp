namespace Disp_WinForm
{
    partial class detail
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle41 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle43 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle44 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle42 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle45 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle46 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle47 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle48 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle49 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle50 = new System.Windows.Forms.DataGridViewCellStyle();
            this.comboBox_status_trevogi = new System.Windows.Forms.ComboBox();
            this.textBox_otrabotka_trevogi = new System.Windows.Forms.TextBox();
            this.dateTimePicker_nachalo_dejstvia = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.treeView_client_info = new System.Windows.Forms.TreeView();
            this.dateTimePicker_nachalo_dejstvia_data = new System.Windows.Forms.DateTimePicker();
            this.checkBox_send_email = new System.Windows.Forms.CheckBox();
            this.button_vnesti_zapis = new System.Windows.Forms.Button();
            this.button_cancel_edit = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView_hronologija_trivog = new System.Windows.Forms.DataGridView();
            this.Column_alarm_text = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_time_start_ack = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_time_end_ack = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Users_chenge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.dataGridView_rezultat_trevog = new System.Windows.Forms.DataGridView();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.dataGridView_trivogi_objecta = new System.Windows.Forms.DataGridView();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.dataGridView_group_alarm = new System.Windows.Forms.DataGridView();
            this.Column_id_alarm_group_alarm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_unit_name_group_alarm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_type_alarm_group_alarm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_curr_time_group_alarm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_msg_time_group_alarm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_group_alarm_group_alarm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_unit_id_group_alarm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_group_alarm = new System.Windows.Forms.Button();
            this.button_ungroup_alarm = new System.Windows.Forms.Button();
            this.label_arm = new System.Windows.Forms.Label();
            this.label_door = new System.Windows.Forms.Label();
            this.label_udar = new System.Windows.Forms.Label();
            this.label_relay_bl = new System.Windows.Forms.Label();
            this.label_akb = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.button_send_cmd_wl = new System.Windows.Forms.Button();
            this.listBox_commads_wl = new System.Windows.Forms.ListBox();
            this.dateTimePicker_okonchanie_dejstvija = new System.Windows.Forms.DateTimePicker();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_hronologija_trivog)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_rezultat_trevog)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_trivogi_objecta)).BeginInit();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_group_alarm)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox_status_trevogi
            // 
            this.comboBox_status_trevogi.FormattingEnabled = true;
            this.comboBox_status_trevogi.Items.AddRange(new object[] {
            "Відкрито",
            "Обробляется",
            "Закрито"});
            this.comboBox_status_trevogi.Location = new System.Drawing.Point(538, 167);
            this.comboBox_status_trevogi.Name = "comboBox_status_trevogi";
            this.comboBox_status_trevogi.Size = new System.Drawing.Size(105, 21);
            this.comboBox_status_trevogi.TabIndex = 0;
            // 
            // textBox_otrabotka_trevogi
            // 
            this.textBox_otrabotka_trevogi.Location = new System.Drawing.Point(16, 166);
            this.textBox_otrabotka_trevogi.Multiline = true;
            this.textBox_otrabotka_trevogi.Name = "textBox_otrabotka_trevogi";
            this.textBox_otrabotka_trevogi.Size = new System.Drawing.Size(291, 63);
            this.textBox_otrabotka_trevogi.TabIndex = 1;
            // 
            // dateTimePicker_nachalo_dejstvia
            // 
            this.dateTimePicker_nachalo_dejstvia.CustomFormat = "hh:mm";
            this.dateTimePicker_nachalo_dejstvia.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePicker_nachalo_dejstvia.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_nachalo_dejstvia.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dateTimePicker_nachalo_dejstvia.Location = new System.Drawing.Point(313, 166);
            this.dateTimePicker_nachalo_dejstvia.Name = "dateTimePicker_nachalo_dejstvia";
            this.dateTimePicker_nachalo_dejstvia.ShowUpDown = true;
            this.dateTimePicker_nachalo_dejstvia.Size = new System.Drawing.Size(73, 30);
            this.dateTimePicker_nachalo_dejstvia.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.treeView_client_info);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(760, 146);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Информація про кліента";
            // 
            // treeView_client_info
            // 
            this.treeView_client_info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_client_info.Location = new System.Drawing.Point(3, 16);
            this.treeView_client_info.Name = "treeView_client_info";
            this.treeView_client_info.Size = new System.Drawing.Size(754, 127);
            this.treeView_client_info.TabIndex = 21;
            // 
            // dateTimePicker_nachalo_dejstvia_data
            // 
            this.dateTimePicker_nachalo_dejstvia_data.CustomFormat = "dd/MM/yy";
            this.dateTimePicker_nachalo_dejstvia_data.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePicker_nachalo_dejstvia_data.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_nachalo_dejstvia_data.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dateTimePicker_nachalo_dejstvia_data.Location = new System.Drawing.Point(392, 166);
            this.dateTimePicker_nachalo_dejstvia_data.Name = "dateTimePicker_nachalo_dejstvia_data";
            this.dateTimePicker_nachalo_dejstvia_data.Size = new System.Drawing.Size(104, 30);
            this.dateTimePicker_nachalo_dejstvia_data.TabIndex = 10;
            // 
            // checkBox_send_email
            // 
            this.checkBox_send_email.AutoSize = true;
            this.checkBox_send_email.Location = new System.Drawing.Point(537, 209);
            this.checkBox_send_email.Name = "checkBox_send_email";
            this.checkBox_send_email.Size = new System.Drawing.Size(99, 17);
            this.checkBox_send_email.TabIndex = 9;
            this.checkBox_send_email.Text = "Надіслати звіт";
            this.checkBox_send_email.UseVisualStyleBackColor = true;
            // 
            // button_vnesti_zapis
            // 
            this.button_vnesti_zapis.Location = new System.Drawing.Point(660, 167);
            this.button_vnesti_zapis.Name = "button_vnesti_zapis";
            this.button_vnesti_zapis.Size = new System.Drawing.Size(110, 23);
            this.button_vnesti_zapis.TabIndex = 0;
            this.button_vnesti_zapis.Text = "Внести запис";
            this.button_vnesti_zapis.UseVisualStyleBackColor = true;
            this.button_vnesti_zapis.Click += new System.EventHandler(this.button_vnesti_zapis_Click);
            // 
            // button_cancel_edit
            // 
            this.button_cancel_edit.Location = new System.Drawing.Point(658, 206);
            this.button_cancel_edit.Name = "button_cancel_edit";
            this.button_cancel_edit.Size = new System.Drawing.Size(111, 23);
            this.button_cancel_edit.TabIndex = 8;
            this.button_cancel_edit.Text = "Закрити вікно";
            this.button_cancel_edit.UseVisualStyleBackColor = true;
            this.button_cancel_edit.Click += new System.EventHandler(this.button_cancel_edit_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tabControl1);
            this.groupBox4.Location = new System.Drawing.Point(13, 235);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(759, 314);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 16);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(753, 295);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView_hronologija_trivog);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(745, 269);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Хронологія обробки тривоги";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView_hronologija_trivog
            // 
            this.dataGridView_hronologija_trivog.AllowUserToAddRows = false;
            this.dataGridView_hronologija_trivog.AllowUserToDeleteRows = false;
            this.dataGridView_hronologija_trivog.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataGridViewCellStyle41.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle41.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle41.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle41.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle41.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle41.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle41.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_hronologija_trivog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle41;
            this.dataGridView_hronologija_trivog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_hronologija_trivog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_alarm_text,
            this.Column_time_start_ack,
            this.Column_time_end_ack,
            this.Column_Users_chenge,
            this.Column1});
            dataGridViewCellStyle43.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle43.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle43.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle43.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle43.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle43.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle43.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_hronologija_trivog.DefaultCellStyle = dataGridViewCellStyle43;
            this.dataGridView_hronologija_trivog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_hronologija_trivog.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_hronologija_trivog.Name = "dataGridView_hronologija_trivog";
            dataGridViewCellStyle44.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle44.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle44.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle44.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle44.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle44.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle44.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_hronologija_trivog.RowHeadersDefaultCellStyle = dataGridViewCellStyle44;
            this.dataGridView_hronologija_trivog.RowHeadersWidth = 50;
            this.dataGridView_hronologija_trivog.Size = new System.Drawing.Size(739, 263);
            this.dataGridView_hronologija_trivog.TabIndex = 0;
            // 
            // Column_alarm_text
            // 
            this.Column_alarm_text.DataPropertyName = "alarm_text";
            dataGridViewCellStyle42.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_alarm_text.DefaultCellStyle = dataGridViewCellStyle42;
            this.Column_alarm_text.HeaderText = "Виконана дія";
            this.Column_alarm_text.Name = "Column_alarm_text";
            this.Column_alarm_text.ReadOnly = true;
            this.Column_alarm_text.Width = 350;
            // 
            // Column_time_start_ack
            // 
            this.Column_time_start_ack.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Column_time_start_ack.DataPropertyName = "time_start_ack";
            this.Column_time_start_ack.HeaderText = "Розпочато дію";
            this.Column_time_start_ack.Name = "Column_time_start_ack";
            this.Column_time_start_ack.ReadOnly = true;
            this.Column_time_start_ack.Width = 96;
            // 
            // Column_time_end_ack
            // 
            this.Column_time_end_ack.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Column_time_end_ack.DataPropertyName = "time_end_ack";
            this.Column_time_end_ack.HeaderText = "Завершено дію";
            this.Column_time_end_ack.Name = "Column_time_end_ack";
            this.Column_time_end_ack.ReadOnly = true;
            this.Column_time_end_ack.Width = 99;
            // 
            // Column_Users_chenge
            // 
            this.Column_Users_chenge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Column_Users_chenge.DataPropertyName = "username";
            this.Column_Users_chenge.HeaderText = "Запис додав";
            this.Column_Users_chenge.Name = "Column_Users_chenge";
            this.Column_Users_chenge.ReadOnly = true;
            this.Column_Users_chenge.Width = 88;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.Column1.DataPropertyName = "current_status_alarm";
            this.Column1.HeaderText = "Новий статус";
            this.Column1.Name = "Column1";
            this.Column1.Width = 92;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox6);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(745, 279);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Архів тривог об\'єкта";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.dataGridView_rezultat_trevog);
            this.groupBox6.Location = new System.Drawing.Point(312, 7);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(427, 266);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Результ тобробки тривог";
            // 
            // dataGridView_rezultat_trevog
            // 
            dataGridViewCellStyle45.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle45.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle45.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle45.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle45.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle45.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle45.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_rezultat_trevog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle45;
            this.dataGridView_rezultat_trevog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle46.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle46.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle46.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle46.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle46.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle46.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle46.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_rezultat_trevog.DefaultCellStyle = dataGridViewCellStyle46;
            this.dataGridView_rezultat_trevog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_rezultat_trevog.Location = new System.Drawing.Point(3, 16);
            this.dataGridView_rezultat_trevog.Name = "dataGridView_rezultat_trevog";
            dataGridViewCellStyle47.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle47.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle47.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle47.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle47.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle47.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle47.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_rezultat_trevog.RowHeadersDefaultCellStyle = dataGridViewCellStyle47;
            this.dataGridView_rezultat_trevog.Size = new System.Drawing.Size(421, 247);
            this.dataGridView_rezultat_trevog.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.dataGridView_trivogi_objecta);
            this.groupBox5.Location = new System.Drawing.Point(7, 7);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(298, 266);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Тривоги";
            // 
            // dataGridView_trivogi_objecta
            // 
            dataGridViewCellStyle48.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle48.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle48.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle48.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle48.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle48.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle48.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_trivogi_objecta.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle48;
            this.dataGridView_trivogi_objecta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle49.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle49.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle49.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle49.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle49.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle49.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle49.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_trivogi_objecta.DefaultCellStyle = dataGridViewCellStyle49;
            this.dataGridView_trivogi_objecta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_trivogi_objecta.Location = new System.Drawing.Point(3, 16);
            this.dataGridView_trivogi_objecta.Name = "dataGridView_trivogi_objecta";
            dataGridViewCellStyle50.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle50.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle50.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle50.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle50.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle50.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle50.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_trivogi_objecta.RowHeadersDefaultCellStyle = dataGridViewCellStyle50;
            this.dataGridView_trivogi_objecta.Size = new System.Drawing.Size(292, 247);
            this.dataGridView_trivogi_objecta.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.webBrowser1);
            this.groupBox7.Location = new System.Drawing.Point(779, 13);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(565, 536);
            this.groupBox7.TabIndex = 10;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Об\"єкт на мапі";
            // 
            // webBrowser1
            // 
            this.webBrowser1.AllowWebBrowserDrop = false;
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(3, 16);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(559, 517);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.WebBrowserShortcutsEnabled = false;
            // 
            // dataGridView_group_alarm
            // 
            this.dataGridView_group_alarm.AllowUserToAddRows = false;
            this.dataGridView_group_alarm.AllowUserToDeleteRows = false;
            this.dataGridView_group_alarm.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView_group_alarm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_group_alarm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_id_alarm_group_alarm,
            this.Column_unit_name_group_alarm,
            this.Column_type_alarm_group_alarm,
            this.Column_curr_time_group_alarm,
            this.Column_msg_time_group_alarm,
            this.Column_group_alarm_group_alarm,
            this.Column_unit_id_group_alarm});
            this.dataGridView_group_alarm.Location = new System.Drawing.Point(23, 553);
            this.dataGridView_group_alarm.Name = "dataGridView_group_alarm";
            this.dataGridView_group_alarm.ReadOnly = true;
            this.dataGridView_group_alarm.Size = new System.Drawing.Size(743, 141);
            this.dataGridView_group_alarm.TabIndex = 11;
            this.dataGridView_group_alarm.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView_group_alarm_CellFormatting);
            // 
            // Column_id_alarm_group_alarm
            // 
            this.Column_id_alarm_group_alarm.DataPropertyName = "idnotification";
            this.Column_id_alarm_group_alarm.HeaderText = "ID Тривоги";
            this.Column_id_alarm_group_alarm.Name = "Column_id_alarm_group_alarm";
            this.Column_id_alarm_group_alarm.ReadOnly = true;
            this.Column_id_alarm_group_alarm.Width = 88;
            // 
            // Column_unit_name_group_alarm
            // 
            this.Column_unit_name_group_alarm.DataPropertyName = "unit_name";
            this.Column_unit_name_group_alarm.HeaderText = "unit_name";
            this.Column_unit_name_group_alarm.Name = "Column_unit_name_group_alarm";
            this.Column_unit_name_group_alarm.ReadOnly = true;
            this.Column_unit_name_group_alarm.Visible = false;
            // 
            // Column_type_alarm_group_alarm
            // 
            this.Column_type_alarm_group_alarm.DataPropertyName = "type_alarm";
            this.Column_type_alarm_group_alarm.HeaderText = "type_alarm";
            this.Column_type_alarm_group_alarm.Name = "Column_type_alarm_group_alarm";
            this.Column_type_alarm_group_alarm.ReadOnly = true;
            this.Column_type_alarm_group_alarm.Width = 83;
            // 
            // Column_curr_time_group_alarm
            // 
            this.Column_curr_time_group_alarm.DataPropertyName = "curr_time";
            this.Column_curr_time_group_alarm.HeaderText = "curr_time";
            this.Column_curr_time_group_alarm.Name = "Column_curr_time_group_alarm";
            this.Column_curr_time_group_alarm.ReadOnly = true;
            this.Column_curr_time_group_alarm.Width = 75;
            // 
            // Column_msg_time_group_alarm
            // 
            this.Column_msg_time_group_alarm.DataPropertyName = "msg_time";
            this.Column_msg_time_group_alarm.HeaderText = "msg_time";
            this.Column_msg_time_group_alarm.Name = "Column_msg_time_group_alarm";
            this.Column_msg_time_group_alarm.ReadOnly = true;
            this.Column_msg_time_group_alarm.Width = 76;
            // 
            // Column_group_alarm_group_alarm
            // 
            this.Column_group_alarm_group_alarm.DataPropertyName = "group_alarm";
            this.Column_group_alarm_group_alarm.HeaderText = "group_alarm";
            this.Column_group_alarm_group_alarm.Name = "Column_group_alarm_group_alarm";
            this.Column_group_alarm_group_alarm.ReadOnly = true;
            this.Column_group_alarm_group_alarm.Width = 90;
            // 
            // Column_unit_id_group_alarm
            // 
            this.Column_unit_id_group_alarm.DataPropertyName = "unit_id";
            this.Column_unit_id_group_alarm.HeaderText = "unit_id";
            this.Column_unit_id_group_alarm.Name = "Column_unit_id_group_alarm";
            this.Column_unit_id_group_alarm.ReadOnly = true;
            this.Column_unit_id_group_alarm.Width = 63;
            // 
            // button_group_alarm
            // 
            this.button_group_alarm.Location = new System.Drawing.Point(772, 579);
            this.button_group_alarm.Name = "button_group_alarm";
            this.button_group_alarm.Size = new System.Drawing.Size(187, 38);
            this.button_group_alarm.TabIndex = 12;
            this.button_group_alarm.Text = "Зчепити з відкритою тривогою";
            this.button_group_alarm.UseVisualStyleBackColor = true;
            this.button_group_alarm.Click += new System.EventHandler(this.button_group_alarm_Click);
            // 
            // button_ungroup_alarm
            // 
            this.button_ungroup_alarm.Location = new System.Drawing.Point(772, 652);
            this.button_ungroup_alarm.Name = "button_ungroup_alarm";
            this.button_ungroup_alarm.Size = new System.Drawing.Size(187, 37);
            this.button_ungroup_alarm.TabIndex = 13;
            this.button_ungroup_alarm.Text = "Від\"єднати від відкритої тривоги";
            this.button_ungroup_alarm.UseVisualStyleBackColor = true;
            this.button_ungroup_alarm.Click += new System.EventHandler(this.button_ungroup_alarm_Click);
            // 
            // label_arm
            // 
            this.label_arm.AutoSize = true;
            this.label_arm.Location = new System.Drawing.Point(6, 43);
            this.label_arm.Name = "label_arm";
            this.label_arm.Size = new System.Drawing.Size(13, 13);
            this.label_arm.TabIndex = 14;
            this.label_arm.Text = "  ";
            this.label_arm.Click += new System.EventHandler(this.label_arm_Click);
            // 
            // label_door
            // 
            this.label_door.AutoSize = true;
            this.label_door.Location = new System.Drawing.Point(6, 65);
            this.label_door.Name = "label_door";
            this.label_door.Size = new System.Drawing.Size(16, 13);
            this.label_door.TabIndex = 15;
            this.label_door.Text = "   ";
            this.label_door.Click += new System.EventHandler(this.label_door_Click);
            // 
            // label_udar
            // 
            this.label_udar.AutoSize = true;
            this.label_udar.Location = new System.Drawing.Point(6, 87);
            this.label_udar.Name = "label_udar";
            this.label_udar.Size = new System.Drawing.Size(16, 13);
            this.label_udar.TabIndex = 16;
            this.label_udar.Text = "   ";
            this.label_udar.Click += new System.EventHandler(this.label_udar_Click);
            // 
            // label_relay_bl
            // 
            this.label_relay_bl.AutoSize = true;
            this.label_relay_bl.Location = new System.Drawing.Point(6, 109);
            this.label_relay_bl.Name = "label_relay_bl";
            this.label_relay_bl.Size = new System.Drawing.Size(16, 13);
            this.label_relay_bl.TabIndex = 17;
            this.label_relay_bl.Text = "   ";
            this.label_relay_bl.Click += new System.EventHandler(this.label_relay_bl_Click);
            // 
            // label_akb
            // 
            this.label_akb.AutoSize = true;
            this.label_akb.Location = new System.Drawing.Point(6, 21);
            this.label_akb.Name = "label_akb";
            this.label_akb.Size = new System.Drawing.Size(13, 13);
            this.label_akb.TabIndex = 18;
            this.label_akb.Text = "  ";
            this.label_akb.Click += new System.EventHandler(this.label_akb_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label_door);
            this.groupBox8.Controls.Add(this.label_akb);
            this.groupBox8.Controls.Add(this.label_arm);
            this.groupBox8.Controls.Add(this.label_relay_bl);
            this.groupBox8.Controls.Add(this.label_udar);
            this.groupBox8.Location = new System.Drawing.Point(1159, 558);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(185, 138);
            this.groupBox8.TabIndex = 19;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Датчики";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.button_send_cmd_wl);
            this.groupBox9.Controls.Add(this.listBox_commads_wl);
            this.groupBox9.Location = new System.Drawing.Point(985, 558);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(168, 138);
            this.groupBox9.TabIndex = 20;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Відправити команду";
            // 
            // button_send_cmd_wl
            // 
            this.button_send_cmd_wl.Location = new System.Drawing.Point(6, 108);
            this.button_send_cmd_wl.Name = "button_send_cmd_wl";
            this.button_send_cmd_wl.Size = new System.Drawing.Size(156, 23);
            this.button_send_cmd_wl.TabIndex = 1;
            this.button_send_cmd_wl.Text = "Відправити";
            this.button_send_cmd_wl.UseVisualStyleBackColor = true;
            this.button_send_cmd_wl.Click += new System.EventHandler(this.button_send_cmd_wl_Click);
            // 
            // listBox_commads_wl
            // 
            this.listBox_commads_wl.FormattingEnabled = true;
            this.listBox_commads_wl.Location = new System.Drawing.Point(6, 19);
            this.listBox_commads_wl.Name = "listBox_commads_wl";
            this.listBox_commads_wl.Size = new System.Drawing.Size(156, 82);
            this.listBox_commads_wl.TabIndex = 0;
            // 
            // dateTimePicker_okonchanie_dejstvija
            // 
            this.dateTimePicker_okonchanie_dejstvija.CustomFormat = "hh:mm";
            this.dateTimePicker_okonchanie_dejstvija.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePicker_okonchanie_dejstvija.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_okonchanie_dejstvija.Location = new System.Drawing.Point(313, 199);
            this.dateTimePicker_okonchanie_dejstvija.Name = "dateTimePicker_okonchanie_dejstvija";
            this.dateTimePicker_okonchanie_dejstvija.ShowUpDown = true;
            this.dateTimePicker_okonchanie_dejstvija.Size = new System.Drawing.Size(73, 30);
            this.dateTimePicker_okonchanie_dejstvija.TabIndex = 3;
            // 
            // detail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1356, 705);
            this.Controls.Add(this.comboBox_status_trevogi);
            this.Controls.Add(this.checkBox_send_email);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.dateTimePicker_nachalo_dejstvia_data);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.button_ungroup_alarm);
            this.Controls.Add(this.button_vnesti_zapis);
            this.Controls.Add(this.button_group_alarm);
            this.Controls.Add(this.button_cancel_edit);
            this.Controls.Add(this.dataGridView_group_alarm);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.dateTimePicker_nachalo_dejstvia);
            this.Controls.Add(this.textBox_otrabotka_trevogi);
            this.Controls.Add(this.dateTimePicker_okonchanie_dejstvija);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Name = "detail";
            this.Text = "detail";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.detail_FormClosing);
            this.Load += new System.EventHandler(this.detail_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_hronologija_trivog)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_rezultat_trevog)).EndInit();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_trivogi_objecta)).EndInit();
            this.groupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_group_alarm)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_status_trevogi;
        private System.Windows.Forms.TextBox textBox_otrabotka_trevogi;
        private System.Windows.Forms.DateTimePicker dateTimePicker_nachalo_dejstvia;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_vnesti_zapis;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dataGridView_hronologija_trivog;
        private System.Windows.Forms.TreeView treeView_client_info;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.DataGridView dataGridView_rezultat_trevog;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView dataGridView_trivogi_objecta;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.DataGridView dataGridView_group_alarm;
        private System.Windows.Forms.Button button_group_alarm;
        private System.Windows.Forms.Button button_ungroup_alarm;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_id_alarm_group_alarm;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_unit_name_group_alarm;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_type_alarm_group_alarm;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_curr_time_group_alarm;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_msg_time_group_alarm;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_group_alarm_group_alarm;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_unit_id_group_alarm;
        private System.Windows.Forms.Button button_cancel_edit;
        private System.Windows.Forms.CheckBox checkBox_send_email;
        private System.Windows.Forms.Label label_arm;
        private System.Windows.Forms.Label label_door;
        private System.Windows.Forms.Label label_udar;
        private System.Windows.Forms.Label label_relay_bl;
        private System.Windows.Forms.Label label_akb;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.ListBox listBox_commads_wl;
        private System.Windows.Forms.Button button_send_cmd_wl;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_alarm_text;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_time_start_ack;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_time_end_ack;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Users_chenge;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DateTimePicker dateTimePicker_nachalo_dejstvia_data;
        private System.Windows.Forms.DateTimePicker dateTimePicker_okonchanie_dejstvija;
    }
}