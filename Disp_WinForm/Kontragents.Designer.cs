namespace Disp_WinForm
{
    partial class Kontragents
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
            this.listView_kontragents = new System.Windows.Forms.ListView();
            this.button_add_kontragents = new System.Windows.Forms.Button();
            this.button_delete_kontragents = new System.Windows.Forms.Button();
            this.textBox_search_kontragents = new System.Windows.Forms.TextBox();
            this.button_edit_kontragents = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView_kontragents
            // 
            this.listView_kontragents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_kontragents.HideSelection = false;
            this.listView_kontragents.Location = new System.Drawing.Point(12, 41);
            this.listView_kontragents.Name = "listView_kontragents";
            this.listView_kontragents.Size = new System.Drawing.Size(775, 397);
            this.listView_kontragents.TabIndex = 0;
            this.listView_kontragents.UseCompatibleStateImageBehavior = false;
            this.listView_kontragents.DoubleClick += new System.EventHandler(this.listView_kontragents_DoubleClick);
            // 
            // button_add_kontragents
            // 
            this.button_add_kontragents.Location = new System.Drawing.Point(153, 9);
            this.button_add_kontragents.Name = "button_add_kontragents";
            this.button_add_kontragents.Size = new System.Drawing.Size(75, 23);
            this.button_add_kontragents.TabIndex = 1;
            this.button_add_kontragents.Text = "Додати";
            this.button_add_kontragents.UseVisualStyleBackColor = true;
            this.button_add_kontragents.Click += new System.EventHandler(this.button_add_kontragents_Click);
            // 
            // button_delete_kontragents
            // 
            this.button_delete_kontragents.Location = new System.Drawing.Point(713, 12);
            this.button_delete_kontragents.Name = "button_delete_kontragents";
            this.button_delete_kontragents.Size = new System.Drawing.Size(75, 23);
            this.button_delete_kontragents.TabIndex = 2;
            this.button_delete_kontragents.Text = "Видалити";
            this.button_delete_kontragents.UseVisualStyleBackColor = true;
            this.button_delete_kontragents.Click += new System.EventHandler(this.button_delete_kontragents_Click);
            // 
            // textBox_search_kontragents
            // 
            this.textBox_search_kontragents.Location = new System.Drawing.Point(13, 12);
            this.textBox_search_kontragents.Name = "textBox_search_kontragents";
            this.textBox_search_kontragents.Size = new System.Drawing.Size(134, 20);
            this.textBox_search_kontragents.TabIndex = 3;
            this.textBox_search_kontragents.TextChanged += new System.EventHandler(this.textBox_search_kontragents_TextChanged);
            // 
            // button_edit_kontragents
            // 
            this.button_edit_kontragents.Location = new System.Drawing.Point(234, 9);
            this.button_edit_kontragents.Name = "button_edit_kontragents";
            this.button_edit_kontragents.Size = new System.Drawing.Size(75, 23);
            this.button_edit_kontragents.TabIndex = 4;
            this.button_edit_kontragents.Text = "Редагувати";
            this.button_edit_kontragents.UseVisualStyleBackColor = true;
            this.button_edit_kontragents.Click += new System.EventHandler(this.button_edit_kontragents_Click);
            // 
            // Kontragents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_edit_kontragents);
            this.Controls.Add(this.textBox_search_kontragents);
            this.Controls.Add(this.button_delete_kontragents);
            this.Controls.Add(this.button_add_kontragents);
            this.Controls.Add(this.listView_kontragents);
            this.Name = "Kontragents";
            this.Text = "Kontragents";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView_kontragents;
        private System.Windows.Forms.Button button_add_kontragents;
        private System.Windows.Forms.Button button_delete_kontragents;
        private System.Windows.Forms.TextBox textBox_search_kontragents;
        private System.Windows.Forms.Button button_edit_kontragents;
    }
}