namespace Disp_WinForm
{
    partial class VO_add
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
            this.textBox_familiya_vo = new System.Windows.Forms.TextBox();
            this.textBox_imya_vo = new System.Windows.Forms.TextBox();
            this.textBox_otchestvo_vo = new System.Windows.Forms.TextBox();
            this.maskedTextBox_phone1_vo = new System.Windows.Forms.MaskedTextBox();
            this.maskedTextBox_phone2_vo = new System.Windows.Forms.MaskedTextBox();
            this.listBox_kontakts_list = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_comment = new System.Windows.Forms.TextBox();
            this.comboBox_messanger_1 = new System.Windows.Forms.ComboBox();
            this.comboBox_messanger_2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_update_kontakt = new System.Windows.Forms.Button();
            this.button_delete_kontakt = new System.Windows.Forms.Button();
            this.button_select = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_familiya_vo
            // 
            this.textBox_familiya_vo.Location = new System.Drawing.Point(93, 25);
            this.textBox_familiya_vo.Name = "textBox_familiya_vo";
            this.textBox_familiya_vo.Size = new System.Drawing.Size(282, 20);
            this.textBox_familiya_vo.TabIndex = 0;
            this.textBox_familiya_vo.TextChanged += new System.EventHandler(this.textBox_familiya_vo_TextChanged);
            // 
            // textBox_imya_vo
            // 
            this.textBox_imya_vo.Location = new System.Drawing.Point(93, 52);
            this.textBox_imya_vo.Name = "textBox_imya_vo";
            this.textBox_imya_vo.Size = new System.Drawing.Size(282, 20);
            this.textBox_imya_vo.TabIndex = 1;
            // 
            // textBox_otchestvo_vo
            // 
            this.textBox_otchestvo_vo.Location = new System.Drawing.Point(93, 79);
            this.textBox_otchestvo_vo.Name = "textBox_otchestvo_vo";
            this.textBox_otchestvo_vo.Size = new System.Drawing.Size(282, 20);
            this.textBox_otchestvo_vo.TabIndex = 2;
            // 
            // maskedTextBox_phone1_vo
            // 
            this.maskedTextBox_phone1_vo.Location = new System.Drawing.Point(93, 106);
            this.maskedTextBox_phone1_vo.Mask = "999-000-0000";
            this.maskedTextBox_phone1_vo.Name = "maskedTextBox_phone1_vo";
            this.maskedTextBox_phone1_vo.Size = new System.Drawing.Size(205, 20);
            this.maskedTextBox_phone1_vo.TabIndex = 3;
            // 
            // maskedTextBox_phone2_vo
            // 
            this.maskedTextBox_phone2_vo.Location = new System.Drawing.Point(93, 134);
            this.maskedTextBox_phone2_vo.Mask = "999-000-0000";
            this.maskedTextBox_phone2_vo.Name = "maskedTextBox_phone2_vo";
            this.maskedTextBox_phone2_vo.Size = new System.Drawing.Size(205, 20);
            this.maskedTextBox_phone2_vo.TabIndex = 4;
            // 
            // listBox_kontakts_list
            // 
            this.listBox_kontakts_list.FormattingEnabled = true;
            this.listBox_kontakts_list.Location = new System.Drawing.Point(401, 25);
            this.listBox_kontakts_list.Name = "listBox_kontakts_list";
            this.listBox_kontakts_list.Size = new System.Drawing.Size(338, 186);
            this.listBox_kontakts_list.TabIndex = 5;
            this.listBox_kontakts_list.DoubleClick += new System.EventHandler(this.listBox_kontakts_list_DoubleClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(93, 189);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(282, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Додати в довідник";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Фамілія";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Телефон 2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Телефон 1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "По батьбькові";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Імя";
            // 
            // textBox_comment
            // 
            this.textBox_comment.Location = new System.Drawing.Point(92, 232);
            this.textBox_comment.Multiline = true;
            this.textBox_comment.Name = "textBox_comment";
            this.textBox_comment.Size = new System.Drawing.Size(647, 81);
            this.textBox_comment.TabIndex = 13;
            // 
            // comboBox_messanger_1
            // 
            this.comboBox_messanger_1.FormattingEnabled = true;
            this.comboBox_messanger_1.Location = new System.Drawing.Point(304, 106);
            this.comboBox_messanger_1.Name = "comboBox_messanger_1";
            this.comboBox_messanger_1.Size = new System.Drawing.Size(71, 21);
            this.comboBox_messanger_1.TabIndex = 14;
            // 
            // comboBox_messanger_2
            // 
            this.comboBox_messanger_2.FormattingEnabled = true;
            this.comboBox_messanger_2.Location = new System.Drawing.Point(304, 134);
            this.comboBox_messanger_2.Name = "comboBox_messanger_2";
            this.comboBox_messanger_2.Size = new System.Drawing.Size(71, 21);
            this.comboBox_messanger_2.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 235);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Коментар";
            // 
            // button_update_kontakt
            // 
            this.button_update_kontakt.Location = new System.Drawing.Point(93, 160);
            this.button_update_kontakt.Name = "button_update_kontakt";
            this.button_update_kontakt.Size = new System.Drawing.Size(138, 23);
            this.button_update_kontakt.TabIndex = 17;
            this.button_update_kontakt.Text = "Оновити контакт";
            this.button_update_kontakt.UseVisualStyleBackColor = true;
            this.button_update_kontakt.Click += new System.EventHandler(this.button_update_kontakt_Click);
            // 
            // button_delete_kontakt
            // 
            this.button_delete_kontakt.Enabled = false;
            this.button_delete_kontakt.Location = new System.Drawing.Point(244, 160);
            this.button_delete_kontakt.Name = "button_delete_kontakt";
            this.button_delete_kontakt.Size = new System.Drawing.Size(131, 23);
            this.button_delete_kontakt.TabIndex = 18;
            this.button_delete_kontakt.Text = "Видалити контакт";
            this.button_delete_kontakt.UseVisualStyleBackColor = true;
            // 
            // button_select
            // 
            this.button_select.Location = new System.Drawing.Point(583, 368);
            this.button_select.Name = "button_select";
            this.button_select.Size = new System.Drawing.Size(75, 23);
            this.button_select.TabIndex = 19;
            this.button_select.Text = "Вибрати";
            this.button_select.UseVisualStyleBackColor = true;
            this.button_select.Click += new System.EventHandler(this.button_select_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(664, 368);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 20;
            this.button_cancel.Text = "Відмінити";
            this.button_cancel.UseVisualStyleBackColor = true;
            // 
            // VO_add
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 403);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_select);
            this.Controls.Add(this.button_delete_kontakt);
            this.Controls.Add(this.button_update_kontakt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_messanger_2);
            this.Controls.Add(this.comboBox_messanger_1);
            this.Controls.Add(this.textBox_comment);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBox_kontakts_list);
            this.Controls.Add(this.maskedTextBox_phone2_vo);
            this.Controls.Add(this.maskedTextBox_phone1_vo);
            this.Controls.Add(this.textBox_otchestvo_vo);
            this.Controls.Add(this.textBox_imya_vo);
            this.Controls.Add(this.textBox_familiya_vo);
            this.Name = "VO_add";
            this.Text = "VO_add";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_familiya_vo;
        private System.Windows.Forms.TextBox textBox_imya_vo;
        private System.Windows.Forms.TextBox textBox_otchestvo_vo;
        private System.Windows.Forms.MaskedTextBox maskedTextBox_phone1_vo;
        private System.Windows.Forms.MaskedTextBox maskedTextBox_phone2_vo;
        private System.Windows.Forms.ListBox listBox_kontakts_list;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_comment;
        private System.Windows.Forms.ComboBox comboBox_messanger_1;
        private System.Windows.Forms.ComboBox comboBox_messanger_2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_update_kontakt;
        private System.Windows.Forms.Button button_delete_kontakt;
        private System.Windows.Forms.Button button_select;
        private System.Windows.Forms.Button button_cancel;
    }
}