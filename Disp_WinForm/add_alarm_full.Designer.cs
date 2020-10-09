namespace Disp_WinForm
{
    partial class add_alarm_full
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
            this.treeView_object_info = new System.Windows.Forms.TreeView();
            this.comboBox_source_in = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_vnesty_zapis = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_primitka = new System.Windows.Forms.TextBox();
            this.textBox_email_account = new System.Windows.Forms.TextBox();
            this.comboBox_account = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView_object_info
            // 
            this.treeView_object_info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_object_info.Location = new System.Drawing.Point(3, 16);
            this.treeView_object_info.Name = "treeView_object_info";
            this.treeView_object_info.Size = new System.Drawing.Size(706, 269);
            this.treeView_object_info.TabIndex = 0;
            this.treeView_object_info.Click += new System.EventHandler(this.treeView_object_info_Click);
            // 
            // comboBox_source_in
            // 
            this.comboBox_source_in.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_source_in.FormattingEnabled = true;
            this.comboBox_source_in.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBox_source_in.Items.AddRange(new object[] {
            "Звернення від кліента",
            "Внутрішне звернення",
            "Кабінет користувача"});
            this.comboBox_source_in.Location = new System.Drawing.Point(142, 19);
            this.comboBox_source_in.Name = "comboBox_source_in";
            this.comboBox_source_in.Size = new System.Drawing.Size(562, 21);
            this.comboBox_source_in.TabIndex = 4;
            this.comboBox_source_in.SelectedIndexChanged += new System.EventHandler(this.comboBox_source_in_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Джерело звернення";
            // 
            // button_vnesty_zapis
            // 
            this.button_vnesty_zapis.Location = new System.Drawing.Point(625, 463);
            this.button_vnesty_zapis.Name = "button_vnesty_zapis";
            this.button_vnesty_zapis.Size = new System.Drawing.Size(86, 23);
            this.button_vnesty_zapis.TabIndex = 7;
            this.button_vnesty_zapis.Text = "Вести запис";
            this.button_vnesty_zapis.UseVisualStyleBackColor = true;
            this.button_vnesty_zapis.Click += new System.EventHandler(this.button_vnesty_zapis_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(511, 463);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(86, 23);
            this.button_cancel.TabIndex = 8;
            this.button_cancel.Text = "Скасувати";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.treeView_object_info);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(712, 288);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Інформація про обєкт";
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.textBox_primitka);
            this.groupBox2.Controls.Add(this.textBox_email_account);
            this.groupBox2.Controls.Add(this.comboBox_account);
            this.groupBox2.Controls.Add(this.comboBox_source_in);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(3, 298);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(709, 159);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Опції звернення";
            // 
            // textBox_primitka
            // 
            this.textBox_primitka.Location = new System.Drawing.Point(142, 124);
            this.textBox_primitka.Name = "textBox_primitka";
            this.textBox_primitka.Size = new System.Drawing.Size(561, 20);
            this.textBox_primitka.TabIndex = 8;
            // 
            // textBox_email_account
            // 
            this.textBox_email_account.Location = new System.Drawing.Point(142, 98);
            this.textBox_email_account.Name = "textBox_email_account";
            this.textBox_email_account.Size = new System.Drawing.Size(561, 20);
            this.textBox_email_account.TabIndex = 8;
            this.textBox_email_account.TextChanged += new System.EventHandler(this.textBox_email_account_TextChanged);
            // 
            // comboBox_account
            // 
            this.comboBox_account.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_account.FormattingEnabled = true;
            this.comboBox_account.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBox_account.Items.AddRange(new object[] {
            "Створення облікового запису",
            "Видалення облікового запису",
            "Відновлення пароля",
            "Додання обєкта до облікового запису",
            "Видалення обєтка з облікового запису",
            "Інше"});
            this.comboBox_account.Location = new System.Drawing.Point(142, 71);
            this.comboBox_account.Name = "comboBox_account";
            this.comboBox_account.Size = new System.Drawing.Size(562, 21);
            this.comboBox_account.TabIndex = 7;
            this.comboBox_account.SelectedIndexChanged += new System.EventHandler(this.comboBox_account_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Примітка";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Електронна пошта";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Діії з обліковим записом";
            // 
            // add_alarm_full
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 499);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_vnesty_zapis);
            this.Name = "add_alarm_full";
            this.Text = "Зарееструвати звернення по обраному об\'єкту";
            this.Load += new System.EventHandler(this.add_alarm_full_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView_object_info;
        private System.Windows.Forms.ComboBox comboBox_source_in;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_vnesty_zapis;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_primitka;
        private System.Windows.Forms.TextBox textBox_email_account;
        private System.Windows.Forms.ComboBox comboBox_account;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}