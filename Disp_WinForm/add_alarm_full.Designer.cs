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
            this.textBox_zvernennya = new System.Windows.Forms.TextBox();
            this.comboBox_source_in = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button_vnesty_zapis = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
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
            // 
            // textBox_zvernennya
            // 
            this.textBox_zvernennya.Location = new System.Drawing.Point(133, 46);
            this.textBox_zvernennya.Name = "textBox_zvernennya";
            this.textBox_zvernennya.ReadOnly = true;
            this.textBox_zvernennya.Size = new System.Drawing.Size(571, 20);
            this.textBox_zvernennya.TabIndex = 1;
            this.textBox_zvernennya.Visible = false;
            // 
            // comboBox_source_in
            // 
            this.comboBox_source_in.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_source_in.FormattingEnabled = true;
            this.comboBox_source_in.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBox_source_in.Items.AddRange(new object[] {
            "Звернення від кліента",
            "Звернення від установника",
            "Внутрішне звернення"});
            this.comboBox_source_in.Location = new System.Drawing.Point(133, 19);
            this.comboBox_source_in.Name = "comboBox_source_in";
            this.comboBox_source_in.Size = new System.Drawing.Size(571, 21);
            this.comboBox_source_in.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Джерело звернення";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Суть звернення";
            this.label2.Visible = false;
            // 
            // button_vnesty_zapis
            // 
            this.button_vnesty_zapis.Location = new System.Drawing.Point(9, 378);
            this.button_vnesty_zapis.Name = "button_vnesty_zapis";
            this.button_vnesty_zapis.Size = new System.Drawing.Size(86, 23);
            this.button_vnesty_zapis.TabIndex = 7;
            this.button_vnesty_zapis.Text = "Вести запис";
            this.button_vnesty_zapis.UseVisualStyleBackColor = true;
            this.button_vnesty_zapis.Click += new System.EventHandler(this.button_vnesty_zapis_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(632, 378);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
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
            this.groupBox1.Text = "groupBox1";
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.textBox_zvernennya);
            this.groupBox2.Controls.Add(this.comboBox_source_in);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(3, 298);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(709, 74);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Опції звернення";
            // 
            // add_alarm_full
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 413);
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
        private System.Windows.Forms.TextBox textBox_zvernennya;
        private System.Windows.Forms.ComboBox comboBox_source_in;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_vnesty_zapis;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}