namespace Disp_WinForm
{
    partial class Edit_kontragents
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
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox_comment = new System.Windows.Forms.TextBox();
            this.groupBox_phone2 = new System.Windows.Forms.GroupBox();
            this.comboBox_misto = new System.Windows.Forms.ComboBox();
            this.textBox_vulitsa = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_misto_ = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_mail1 = new System.Windows.Forms.TextBox();
            this.textBox_mail2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox_kategory = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_type_vlastnosti = new System.Windows.Forms.ComboBox();
            this.textBox_full_name = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_short_name = new System.Windows.Forms.TextBox();
            this.textBox_rekvizity = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.maskedTextBox_phone2 = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBox_phone1 = new System.Windows.Forms.MaskedTextBox();
            this.groupBox3.SuspendLayout();
            this.groupBox_phone2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(504, 335);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Відмінити";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(585, 335);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Зберегти";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox_comment);
            this.groupBox3.Location = new System.Drawing.Point(11, 236);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(647, 93);
            this.groupBox3.TabIndex = 49;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Коментар";
            // 
            // textBox_comment
            // 
            this.textBox_comment.Location = new System.Drawing.Point(13, 19);
            this.textBox_comment.Multiline = true;
            this.textBox_comment.Name = "textBox_comment";
            this.textBox_comment.Size = new System.Drawing.Size(618, 56);
            this.textBox_comment.TabIndex = 12;
            // 
            // groupBox_phone2
            // 
            this.groupBox_phone2.Controls.Add(this.maskedTextBox_phone2);
            this.groupBox_phone2.Controls.Add(this.maskedTextBox_phone1);
            this.groupBox_phone2.Controls.Add(this.comboBox_misto);
            this.groupBox_phone2.Controls.Add(this.textBox_vulitsa);
            this.groupBox_phone2.Controls.Add(this.label3);
            this.groupBox_phone2.Controls.Add(this.comboBox_misto_);
            this.groupBox_phone2.Controls.Add(this.label2);
            this.groupBox_phone2.Controls.Add(this.label6);
            this.groupBox_phone2.Controls.Add(this.textBox_mail1);
            this.groupBox_phone2.Controls.Add(this.textBox_mail2);
            this.groupBox_phone2.Controls.Add(this.label7);
            this.groupBox_phone2.Controls.Add(this.label8);
            this.groupBox_phone2.Controls.Add(this.label12);
            this.groupBox_phone2.Location = new System.Drawing.Point(339, 12);
            this.groupBox_phone2.Name = "groupBox_phone2";
            this.groupBox_phone2.Size = new System.Drawing.Size(321, 218);
            this.groupBox_phone2.TabIndex = 48;
            this.groupBox_phone2.TabStop = false;
            this.groupBox_phone2.Text = "Контактна інформація";
            // 
            // comboBox_misto
            // 
            this.comboBox_misto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_misto.FormattingEnabled = true;
            this.comboBox_misto.Items.AddRange(new object[] {
            "Київ",
            "Харків",
            "Одесса",
            "Днепр",
            "Запорожье",
            "Львов",
            "Кривой Рог",
            "Николаев",
            "Мариуполь",
            "Винница",
            "Херсон",
            "Полтава",
            "Чернигов",
            "Черкассы",
            "Хмельницкий",
            "Черновцы",
            "Житомир",
            "Сумы",
            "Ровно",
            "Ивано-Франковск",
            "Каменское",
            "Кропивницкий",
            "Тернополь",
            "Кременчуг",
            "Луцк",
            "Краматорск",
            "Ужгород",
            "Павлоград"});
            this.comboBox_misto.Location = new System.Drawing.Point(117, 23);
            this.comboBox_misto.Name = "comboBox_misto";
            this.comboBox_misto.Size = new System.Drawing.Size(186, 21);
            this.comboBox_misto.TabIndex = 39;
            // 
            // textBox_vulitsa
            // 
            this.textBox_vulitsa.Location = new System.Drawing.Point(117, 50);
            this.textBox_vulitsa.Name = "textBox_vulitsa";
            this.textBox_vulitsa.Size = new System.Drawing.Size(186, 20);
            this.textBox_vulitsa.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "Вулиция";
            // 
            // comboBox_misto_
            // 
            this.comboBox_misto_.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_misto_.FormattingEnabled = true;
            this.comboBox_misto_.Items.AddRange(new object[] {
            "Київ",
            "Харків",
            "Дніпро"});
            this.comboBox_misto_.Location = new System.Drawing.Point(25, 186);
            this.comboBox_misto_.Name = "comboBox_misto_";
            this.comboBox_misto_.Size = new System.Drawing.Size(186, 21);
            this.comboBox_misto_.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 37;
            this.label2.Text = "Місто";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "Телефон";
            // 
            // textBox_mail1
            // 
            this.textBox_mail1.Location = new System.Drawing.Point(117, 128);
            this.textBox_mail1.Name = "textBox_mail1";
            this.textBox_mail1.Size = new System.Drawing.Size(186, 20);
            this.textBox_mail1.TabIndex = 10;
            // 
            // textBox_mail2
            // 
            this.textBox_mail2.Location = new System.Drawing.Point(117, 154);
            this.textBox_mail2.Name = "textBox_mail2";
            this.textBox_mail2.Size = new System.Drawing.Size(186, 20);
            this.textBox_mail2.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 131);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 31;
            this.label7.Text = "e-mail";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 105);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "Телефон 2";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 157);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(43, 13);
            this.label12.TabIndex = 36;
            this.label12.Text = "e-mail 2";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox_kategory);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.comboBox_type_vlastnosti);
            this.groupBox1.Controls.Add(this.textBox_full_name);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.textBox_short_name);
            this.groupBox1.Controls.Add(this.textBox_rekvizity);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(321, 218);
            this.groupBox1.TabIndex = 47;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Загальна інформація";
            // 
            // comboBox_kategory
            // 
            this.comboBox_kategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_kategory.FormattingEnabled = true;
            this.comboBox_kategory.Items.AddRange(new object[] {
            "Диллер/СТО",
            "СК",
            "Клієнт",
            "Постачальник"});
            this.comboBox_kategory.Location = new System.Drawing.Point(123, 191);
            this.comboBox_kategory.Name = "comboBox_kategory";
            this.comboBox_kategory.Size = new System.Drawing.Size(186, 21);
            this.comboBox_kategory.TabIndex = 5;
            this.comboBox_kategory.SelectedIndexChanged += new System.EventHandler(this.comboBox_kategory_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 194);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "Категорія";
            // 
            // comboBox_type_vlastnosti
            // 
            this.comboBox_type_vlastnosti.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_type_vlastnosti.FormattingEnabled = true;
            this.comboBox_type_vlastnosti.Items.AddRange(new object[] {
            "ТОВ",
            "ФО",
            "ФОП",
            "АТ",
            "ПрАТ",
            "ДП"});
            this.comboBox_type_vlastnosti.Location = new System.Drawing.Point(123, 76);
            this.comboBox_type_vlastnosti.Name = "comboBox_type_vlastnosti";
            this.comboBox_type_vlastnosti.Size = new System.Drawing.Size(186, 21);
            this.comboBox_type_vlastnosti.TabIndex = 3;
            // 
            // textBox_full_name
            // 
            this.textBox_full_name.Location = new System.Drawing.Point(123, 24);
            this.textBox_full_name.Name = "textBox_full_name";
            this.textBox_full_name.Size = new System.Drawing.Size(186, 20);
            this.textBox_full_name.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "Повна назва";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(94, 13);
            this.label9.TabIndex = 26;
            this.label9.Text = "Скорочена назва";
            // 
            // textBox_short_name
            // 
            this.textBox_short_name.Location = new System.Drawing.Point(123, 50);
            this.textBox_short_name.Name = "textBox_short_name";
            this.textBox_short_name.Size = new System.Drawing.Size(186, 20);
            this.textBox_short_name.TabIndex = 2;
            // 
            // textBox_rekvizity
            // 
            this.textBox_rekvizity.Location = new System.Drawing.Point(123, 103);
            this.textBox_rekvizity.Name = "textBox_rekvizity";
            this.textBox_rekvizity.Size = new System.Drawing.Size(186, 20);
            this.textBox_rekvizity.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "Реквізити";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 79);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(84, 13);
            this.label11.TabIndex = 35;
            this.label11.Text = "Тип Властності";
            // 
            // maskedTextBox_phone2
            // 
            this.maskedTextBox_phone2.Location = new System.Drawing.Point(117, 102);
            this.maskedTextBox_phone2.Mask = "999-000-0000";
            this.maskedTextBox_phone2.Name = "maskedTextBox_phone2";
            this.maskedTextBox_phone2.Size = new System.Drawing.Size(186, 20);
            this.maskedTextBox_phone2.TabIndex = 43;
            // 
            // maskedTextBox_phone1
            // 
            this.maskedTextBox_phone1.Location = new System.Drawing.Point(117, 76);
            this.maskedTextBox_phone1.Mask = "999-000-0000";
            this.maskedTextBox_phone1.Name = "maskedTextBox_phone1";
            this.maskedTextBox_phone1.Size = new System.Drawing.Size(186, 20);
            this.maskedTextBox_phone1.TabIndex = 42;
            // 
            // Edit_kontragents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 365);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox_phone2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Edit_kontragents";
            this.Text = "Edit_kontragents";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox_phone2.ResumeLayout(false);
            this.groupBox_phone2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox_comment;
        private System.Windows.Forms.GroupBox groupBox_phone2;
        private System.Windows.Forms.TextBox textBox_vulitsa;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_misto_;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_mail1;
        private System.Windows.Forms.TextBox textBox_mail2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBox_kategory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox_type_vlastnosti;
        private System.Windows.Forms.TextBox textBox_full_name;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_short_name;
        private System.Windows.Forms.TextBox textBox_rekvizity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBox_misto;
        private System.Windows.Forms.MaskedTextBox maskedTextBox_phone2;
        private System.Windows.Forms.MaskedTextBox maskedTextBox_phone1;
    }
}