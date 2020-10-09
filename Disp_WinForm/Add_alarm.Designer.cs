namespace Disp_WinForm
{
    partial class Add_alarm
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
            this.textBox_filter_object_ohrani = new System.Windows.Forms.TextBox();
            this.listBox_object_ohrani = new System.Windows.Forms.ListBox();
            this.button_add_new_alarm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_filter_object_ohrani
            // 
            this.textBox_filter_object_ohrani.Location = new System.Drawing.Point(13, 12);
            this.textBox_filter_object_ohrani.Name = "textBox_filter_object_ohrani";
            this.textBox_filter_object_ohrani.Size = new System.Drawing.Size(331, 20);
            this.textBox_filter_object_ohrani.TabIndex = 1;
            this.textBox_filter_object_ohrani.TextChanged += new System.EventHandler(this.textBox_filter_object_ohrani_TextChanged);
            this.textBox_filter_object_ohrani.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_filter_object_ohrani_KeyPress);
            // 
            // listBox_object_ohrani
            // 
            this.listBox_object_ohrani.FormattingEnabled = true;
            this.listBox_object_ohrani.Location = new System.Drawing.Point(13, 38);
            this.listBox_object_ohrani.Name = "listBox_object_ohrani";
            this.listBox_object_ohrani.Size = new System.Drawing.Size(331, 95);
            this.listBox_object_ohrani.TabIndex = 2;
            this.listBox_object_ohrani.DoubleClick += new System.EventHandler(this.listBox_object_ohrani_DoubleClick);
            // 
            // button_add_new_alarm
            // 
            this.button_add_new_alarm.Location = new System.Drawing.Point(13, 139);
            this.button_add_new_alarm.Name = "button_add_new_alarm";
            this.button_add_new_alarm.Size = new System.Drawing.Size(331, 51);
            this.button_add_new_alarm.TabIndex = 4;
            this.button_add_new_alarm.Text = "Обрати";
            this.button_add_new_alarm.UseVisualStyleBackColor = true;
            this.button_add_new_alarm.Click += new System.EventHandler(this.button_add_new_alarm_Click);
            // 
            // Add_alarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 202);
            this.Controls.Add(this.button_add_new_alarm);
            this.Controls.Add(this.listBox_object_ohrani);
            this.Controls.Add(this.textBox_filter_object_ohrani);
            this.Name = "Add_alarm";
            this.Text = "Пошук об\'єкта";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox_filter_object_ohrani;
        private System.Windows.Forms.ListBox listBox_object_ohrani;
        private System.Windows.Forms.Button button_add_new_alarm;
    }
}