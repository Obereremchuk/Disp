namespace Disp_WinForm
{
    partial class Main_window
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView_open_alarm = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView_close_alarm = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Status2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_unit_id2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Gruop_id_3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_time_stamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Users_idUsers_ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_locked_user_ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_idnotification = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_product = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_unit_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_type_alarm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1_speed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_msg_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_curr_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_unit_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_group_alarm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_User_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_alarm_locked_user = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_open_alarm)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_close_alarm)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1109, 598);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView_open_alarm);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1101, 572);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Відкриті тривоги";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView_open_alarm
            // 
            this.dataGridView_open_alarm.AllowUserToAddRows = false;
            this.dataGridView_open_alarm.AllowUserToDeleteRows = false;
            this.dataGridView_open_alarm.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView_open_alarm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_open_alarm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_idnotification,
            this.Column_product,
            this.Column_unit_name,
            this.Column_type_alarm,
            this.Column1_speed,
            this.Column_msg_time,
            this.Column_curr_time,
            this.Column_Status,
            this.Column_unit_id,
            this.Column_group_alarm,
            this.Column_User_id,
            this.Column_alarm_locked_user});
            this.dataGridView_open_alarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_open_alarm.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_open_alarm.Name = "dataGridView_open_alarm";
            this.dataGridView_open_alarm.ReadOnly = true;
            this.dataGridView_open_alarm.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_open_alarm.Size = new System.Drawing.Size(1095, 566);
            this.dataGridView_open_alarm.TabIndex = 0;
            this.dataGridView_open_alarm.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView_open_alarm.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.dataGridView2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1101, 572);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Архів відпрацованих тривог";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView_close_alarm);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 63);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1095, 506);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Архів відпрацьованих тривог";
            // 
            // dataGridView_close_alarm
            // 
            this.dataGridView_close_alarm.AllowUserToAddRows = false;
            this.dataGridView_close_alarm.AllowUserToDeleteRows = false;
            this.dataGridView_close_alarm.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView_close_alarm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_close_alarm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.Column_Status2,
            this.Column_unit_id2,
            this.Column_Gruop_id_3,
            this.Column_time_stamp,
            this.Column_Users_idUsers_,
            this.Column_locked_user_});
            this.dataGridView_close_alarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_close_alarm.Location = new System.Drawing.Point(3, 16);
            this.dataGridView_close_alarm.Name = "dataGridView_close_alarm";
            this.dataGridView_close_alarm.ReadOnly = true;
            this.dataGridView_close_alarm.Size = new System.Drawing.Size(1089, 487);
            this.dataGridView_close_alarm.TabIndex = 1;
            this.dataGridView_close_alarm.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView3_CellContentClick);
            this.dataGridView_close_alarm.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView3_CellFormatting);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dateTimePicker2);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1095, 60);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Часовий проміжок.";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(268, 19);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(145, 20);
            this.dateTimePicker2.TabIndex = 3;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(47, 19);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(143, 20);
            this.dateTimePicker1.TabIndex = 2;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(3, 3);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(1095, 566);
            this.dataGridView2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1109, 598);
            this.panel1.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "idnotification";
            this.dataGridViewTextBoxColumn1.HeaderText = "ID Тривоги";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 69;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "product";
            this.dataGridViewTextBoxColumn2.HeaderText = "Продукт";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 74;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "unit_name";
            this.dataGridViewTextBoxColumn3.HeaderText = "Назва обєкту";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 101;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "type_alarm";
            this.dataGridViewTextBoxColumn4.HeaderText = "Тип тривоги";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 94;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "speed";
            this.dataGridViewTextBoxColumn5.HeaderText = "Швидкість";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Visible = false;
            this.dataGridViewTextBoxColumn5.Width = 84;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn6.DataPropertyName = "msg_time";
            this.dataGridViewTextBoxColumn6.HeaderText = "Час тривоги";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 95;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn7.DataPropertyName = "curr_time";
            this.dataGridViewTextBoxColumn7.HeaderText = "Час отримання тривоги";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 140;
            // 
            // Column_Status2
            // 
            this.Column_Status2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column_Status2.DataPropertyName = "Status";
            this.Column_Status2.HeaderText = "Статус";
            this.Column_Status2.Name = "Column_Status2";
            this.Column_Status2.ReadOnly = true;
            this.Column_Status2.Width = 66;
            // 
            // Column_unit_id2
            // 
            this.Column_unit_id2.DataPropertyName = "Unit_id";
            this.Column_unit_id2.HeaderText = "ID Об\"єкту";
            this.Column_unit_id2.Name = "Column_unit_id2";
            this.Column_unit_id2.ReadOnly = true;
            this.Column_unit_id2.Width = 80;
            // 
            // Column_Gruop_id_3
            // 
            this.Column_Gruop_id_3.DataPropertyName = "group_alarm";
            this.Column_Gruop_id_3.HeaderText = "Згруповано до ID Тривоги:";
            this.Column_Gruop_id_3.Name = "Column_Gruop_id_3";
            this.Column_Gruop_id_3.ReadOnly = true;
            this.Column_Gruop_id_3.Width = 113;
            // 
            // Column_time_stamp
            // 
            this.Column_time_stamp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column_time_stamp.DataPropertyName = "time_stamp";
            this.Column_time_stamp.HeaderText = "Остання зміни";
            this.Column_time_stamp.Name = "Column_time_stamp";
            this.Column_time_stamp.ReadOnly = true;
            this.Column_time_stamp.Width = 97;
            // 
            // Column_Users_idUsers_
            // 
            this.Column_Users_idUsers_.DataPropertyName = "Users_idUsers";
            this.Column_Users_idUsers_.HeaderText = "Остання зміна користувачем";
            this.Column_Users_idUsers_.Name = "Column_Users_idUsers_";
            this.Column_Users_idUsers_.ReadOnly = true;
            this.Column_Users_idUsers_.Visible = false;
            this.Column_Users_idUsers_.Width = 164;
            // 
            // Column_locked_user_
            // 
            this.Column_locked_user_.DataPropertyName = "alarm_locked_user";
            this.Column_locked_user_.HeaderText = "Обробляєтся";
            this.Column_locked_user_.Name = "Column_locked_user_";
            this.Column_locked_user_.ReadOnly = true;
            this.Column_locked_user_.Width = 99;
            // 
            // Column_idnotification
            // 
            this.Column_idnotification.DataPropertyName = "idnotification";
            this.Column_idnotification.HeaderText = "ID Тривоги";
            this.Column_idnotification.Name = "Column_idnotification";
            this.Column_idnotification.ReadOnly = true;
            this.Column_idnotification.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column_idnotification.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column_idnotification.Width = 62;
            // 
            // Column_product
            // 
            this.Column_product.DataPropertyName = "product";
            this.Column_product.HeaderText = "Продукт";
            this.Column_product.Name = "Column_product";
            this.Column_product.ReadOnly = true;
            this.Column_product.Width = 74;
            // 
            // Column_unit_name
            // 
            this.Column_unit_name.DataPropertyName = "unit_name";
            this.Column_unit_name.HeaderText = "Назва об\"єкту";
            this.Column_unit_name.Name = "Column_unit_name";
            this.Column_unit_name.ReadOnly = true;
            this.Column_unit_name.Width = 97;
            // 
            // Column_type_alarm
            // 
            this.Column_type_alarm.DataPropertyName = "type_alarm";
            this.Column_type_alarm.HeaderText = "Тип тривоги";
            this.Column_type_alarm.Name = "Column_type_alarm";
            this.Column_type_alarm.ReadOnly = true;
            this.Column_type_alarm.Width = 87;
            // 
            // Column1_speed
            // 
            this.Column1_speed.DataPropertyName = "speed";
            this.Column1_speed.HeaderText = "Швидкість";
            this.Column1_speed.Name = "Column1_speed";
            this.Column1_speed.ReadOnly = true;
            this.Column1_speed.Width = 84;
            // 
            // Column_msg_time
            // 
            this.Column_msg_time.DataPropertyName = "msg_time";
            this.Column_msg_time.HeaderText = "Час тривоги";
            this.Column_msg_time.Name = "Column_msg_time";
            this.Column_msg_time.ReadOnly = true;
            this.Column_msg_time.Width = 87;
            // 
            // Column_curr_time
            // 
            this.Column_curr_time.DataPropertyName = "curr_time";
            this.Column_curr_time.HeaderText = "Час отримання тривоги";
            this.Column_curr_time.Name = "Column_curr_time";
            this.Column_curr_time.ReadOnly = true;
            this.Column_curr_time.Width = 140;
            // 
            // Column_Status
            // 
            this.Column_Status.DataPropertyName = "Status";
            this.Column_Status.HeaderText = "Статус";
            this.Column_Status.Name = "Column_Status";
            this.Column_Status.ReadOnly = true;
            this.Column_Status.Width = 66;
            // 
            // Column_unit_id
            // 
            this.Column_unit_id.DataPropertyName = "unit_id";
            this.Column_unit_id.HeaderText = "ID об\"єкту";
            this.Column_unit_id.Name = "Column_unit_id";
            this.Column_unit_id.ReadOnly = true;
            this.Column_unit_id.Width = 78;
            // 
            // Column_group_alarm
            // 
            this.Column_group_alarm.DataPropertyName = "group_alarm";
            this.Column_group_alarm.HeaderText = "Згруповано до ID тривоги";
            this.Column_group_alarm.Name = "Column_group_alarm";
            this.Column_group_alarm.ReadOnly = true;
            this.Column_group_alarm.Width = 113;
            // 
            // Column_User_id
            // 
            this.Column_User_id.DataPropertyName = "Users_idUsers";
            this.Column_User_id.HeaderText = "Змінено";
            this.Column_User_id.Name = "Column_User_id";
            this.Column_User_id.ReadOnly = true;
            this.Column_User_id.Visible = false;
            this.Column_User_id.Width = 73;
            // 
            // Column_alarm_locked_user
            // 
            this.Column_alarm_locked_user.DataPropertyName = "alarm_locked_user";
            this.Column_alarm_locked_user.HeaderText = "Обробляется";
            this.Column_alarm_locked_user.Name = "Column_alarm_locked_user";
            this.Column_alarm_locked_user.ReadOnly = true;
            this.Column_alarm_locked_user.Width = 99;
            // 
            // Main_window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1109, 598);
            this.Controls.Add(this.panel1);
            this.Name = "Main_window";
            this.Text = "Disp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_window_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_open_alarm)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_close_alarm)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView_open_alarm;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView_close_alarm;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Status2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_unit_id2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Gruop_id_3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_time_stamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Users_idUsers_;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_locked_user_;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_idnotification;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_product;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_unit_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_type_alarm;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1_speed;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_msg_time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_curr_time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_unit_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_group_alarm;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_User_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_alarm_locked_user;
    }
}

