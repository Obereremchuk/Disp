namespace Disp_WinForm
{
    partial class Edit_kontakts
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
            this.button_exit = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button_add_kontragent = new System.Windows.Forms.Button();
            this.comboBox_work_in = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.maskedTextBox_tel1 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBox_tel2 = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_mail2 = new System.Windows.Forms.TextBox();
            this.textBox_mail = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_coment = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_familia = new System.Windows.Forms.TextBox();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button_create = new System.Windows.Forms.Button();
            this.textBox_otchestvo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_exit
            // 
            this.button_exit.Location = new System.Drawing.Point(665, 277);
            this.button_exit.Name = "button_exit";
            this.button_exit.Size = new System.Drawing.Size(75, 23);
            this.button_exit.TabIndex = 43;
            this.button_exit.Text = "Відмінити";
            this.button_exit.UseVisualStyleBackColor = true;
            this.button_exit.Click += new System.EventHandler(this.button_exit_whithout_saving_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button_add_kontragent);
            this.groupBox4.Controls.Add(this.comboBox_work_in);
            this.groupBox4.Location = new System.Drawing.Point(12, 120);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(424, 54);
            this.groupBox4.TabIndex = 42;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Працює в:";
            // 
            // button_add_kontragent
            // 
            this.button_add_kontragent.Location = new System.Drawing.Point(343, 17);
            this.button_add_kontragent.Name = "button_add_kontragent";
            this.button_add_kontragent.Size = new System.Drawing.Size(75, 23);
            this.button_add_kontragent.TabIndex = 31;
            this.button_add_kontragent.Text = "Новий";
            this.button_add_kontragent.UseVisualStyleBackColor = true;
            this.button_add_kontragent.Click += new System.EventHandler(this.button_add_kontragent_Click);
            // 
            // comboBox_work_in
            // 
            this.comboBox_work_in.FormattingEnabled = true;
            this.comboBox_work_in.Location = new System.Drawing.Point(77, 18);
            this.comboBox_work_in.Name = "comboBox_work_in";
            this.comboBox_work_in.Size = new System.Drawing.Size(260, 21);
            this.comboBox_work_in.TabIndex = 35;
            this.comboBox_work_in.DropDown += new System.EventHandler(this.comboBox_work_in_DropDown);
            this.comboBox_work_in.TextUpdate += new System.EventHandler(this.comboBox_work_in_TextUpdate);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.maskedTextBox_tel1);
            this.groupBox3.Controls.Add(this.maskedTextBox_tel2);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.textBox_mail2);
            this.groupBox3.Controls.Add(this.textBox_mail);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(443, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(297, 162);
            this.groupBox3.TabIndex = 41;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Контактна інформація";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Тел. дод.";
            // 
            // maskedTextBox_tel1
            // 
            this.maskedTextBox_tel1.Location = new System.Drawing.Point(82, 17);
            this.maskedTextBox_tel1.Mask = "999-000-0000";
            this.maskedTextBox_tel1.Name = "maskedTextBox_tel1";
            this.maskedTextBox_tel1.Size = new System.Drawing.Size(202, 20);
            this.maskedTextBox_tel1.TabIndex = 31;
            // 
            // maskedTextBox_tel2
            // 
            this.maskedTextBox_tel2.Location = new System.Drawing.Point(82, 42);
            this.maskedTextBox_tel2.Mask = "999-000-0000";
            this.maskedTextBox_tel2.Name = "maskedTextBox_tel2";
            this.maskedTextBox_tel2.Size = new System.Drawing.Size(202, 20);
            this.maskedTextBox_tel2.TabIndex = 44;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Е-mail";
            // 
            // textBox_mail2
            // 
            this.textBox_mail2.Location = new System.Drawing.Point(82, 94);
            this.textBox_mail2.Name = "textBox_mail2";
            this.textBox_mail2.Size = new System.Drawing.Size(202, 20);
            this.textBox_mail2.TabIndex = 30;
            // 
            // textBox_mail
            // 
            this.textBox_mail.Location = new System.Drawing.Point(82, 68);
            this.textBox_mail.Name = "textBox_mail";
            this.textBox_mail.Size = new System.Drawing.Size(202, 20);
            this.textBox_mail.TabIndex = 8;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(16, 98);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(35, 13);
            this.label14.TabIndex = 29;
            this.label14.Text = "Е-mail";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Тел. осн.";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox_coment);
            this.groupBox2.Location = new System.Drawing.Point(12, 180);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(728, 91);
            this.groupBox2.TabIndex = 40;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Коментар";
            // 
            // textBox_coment
            // 
            this.textBox_coment.Location = new System.Drawing.Point(6, 19);
            this.textBox_coment.Multiline = true;
            this.textBox_coment.Name = "textBox_coment";
            this.textBox_coment.Size = new System.Drawing.Size(709, 50);
            this.textBox_coment.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_otchestvo);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBox_familia);
            this.groupBox1.Controls.Add(this.textBox_name);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(424, 102);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ПІБ";
            // 
            // textBox_familia
            // 
            this.textBox_familia.Location = new System.Drawing.Point(77, 42);
            this.textBox_familia.Name = "textBox_familia";
            this.textBox_familia.Size = new System.Drawing.Size(341, 20);
            this.textBox_familia.TabIndex = 3;
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(77, 16);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(341, 20);
            this.textBox_name.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Імя";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Прізвище";
            // 
            // button_create
            // 
            this.button_create.Location = new System.Drawing.Point(584, 277);
            this.button_create.Name = "button_create";
            this.button_create.Size = new System.Drawing.Size(75, 23);
            this.button_create.TabIndex = 38;
            this.button_create.Text = "Зберегти";
            this.button_create.UseVisualStyleBackColor = true;
            this.button_create.Click += new System.EventHandler(this.button_create_Click);
            // 
            // textBox_otchestvo
            // 
            this.textBox_otchestvo.Location = new System.Drawing.Point(77, 68);
            this.textBox_otchestvo.Name = "textBox_otchestvo";
            this.textBox_otchestvo.Size = new System.Drawing.Size(341, 20);
            this.textBox_otchestvo.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Побатькові";
            // 
            // Edit_kontakts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 312);
            this.Controls.Add(this.button_exit);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_create);
            this.Name = "Edit_kontakts";
            this.Text = "Edit_kontakts";
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_exit;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button_add_kontragent;
        private System.Windows.Forms.ComboBox comboBox_work_in;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_mail2;
        private System.Windows.Forms.TextBox textBox_mail;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_coment;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_familia;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_create;
        private System.Windows.Forms.MaskedTextBox maskedTextBox_tel1;
        private System.Windows.Forms.MaskedTextBox maskedTextBox_tel2;
        private System.Windows.Forms.TextBox textBox_otchestvo;
        private System.Windows.Forms.Label label6;
    }
}