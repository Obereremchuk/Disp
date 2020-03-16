namespace Disp_WinForm
{
    partial class Kontacts
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
            this.button_edit_kontackts = new System.Windows.Forms.Button();
            this.textBox_search_kontackts = new System.Windows.Forms.TextBox();
            this.button_delete_kontackts = new System.Windows.Forms.Button();
            this.button_add_kontackts = new System.Windows.Forms.Button();
            this.listView_kontackts = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // button_edit_kontackts
            // 
            this.button_edit_kontackts.Location = new System.Drawing.Point(233, 12);
            this.button_edit_kontackts.Name = "button_edit_kontackts";
            this.button_edit_kontackts.Size = new System.Drawing.Size(75, 23);
            this.button_edit_kontackts.TabIndex = 9;
            this.button_edit_kontackts.Text = "Редагувати";
            this.button_edit_kontackts.UseVisualStyleBackColor = true;
            this.button_edit_kontackts.Click += new System.EventHandler(this.button_edit_kontackts_Click);
            // 
            // textBox_search_kontackts
            // 
            this.textBox_search_kontackts.Location = new System.Drawing.Point(12, 13);
            this.textBox_search_kontackts.Name = "textBox_search_kontackts";
            this.textBox_search_kontackts.Size = new System.Drawing.Size(134, 20);
            this.textBox_search_kontackts.TabIndex = 8;
            this.textBox_search_kontackts.TextChanged += new System.EventHandler(this.textBox_search_kontackts_TextChanged);
            // 
            // button_delete_kontackts
            // 
            this.button_delete_kontackts.Location = new System.Drawing.Point(712, 12);
            this.button_delete_kontackts.Name = "button_delete_kontackts";
            this.button_delete_kontackts.Size = new System.Drawing.Size(75, 23);
            this.button_delete_kontackts.TabIndex = 7;
            this.button_delete_kontackts.Text = "Видалити";
            this.button_delete_kontackts.UseVisualStyleBackColor = true;
            this.button_delete_kontackts.Click += new System.EventHandler(this.button_delete_kontackts_Click);
            // 
            // button_add_kontackts
            // 
            this.button_add_kontackts.Location = new System.Drawing.Point(152, 12);
            this.button_add_kontackts.Name = "button_add_kontackts";
            this.button_add_kontackts.Size = new System.Drawing.Size(75, 23);
            this.button_add_kontackts.TabIndex = 6;
            this.button_add_kontackts.Text = "Додати";
            this.button_add_kontackts.UseVisualStyleBackColor = true;
            this.button_add_kontackts.Click += new System.EventHandler(this.button_add_kontackts_Click);
            // 
            // listView_kontackts
            // 
            this.listView_kontackts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_kontackts.HideSelection = false;
            this.listView_kontackts.Location = new System.Drawing.Point(11, 41);
            this.listView_kontackts.Name = "listView_kontackts";
            this.listView_kontackts.Size = new System.Drawing.Size(775, 397);
            this.listView_kontackts.TabIndex = 5;
            this.listView_kontackts.UseCompatibleStateImageBehavior = false;
            // 
            // Kontacts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_edit_kontackts);
            this.Controls.Add(this.textBox_search_kontackts);
            this.Controls.Add(this.button_delete_kontackts);
            this.Controls.Add(this.button_add_kontackts);
            this.Controls.Add(this.listView_kontackts);
            this.Name = "Kontacts";
            this.Text = "Kontacts";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_edit_kontackts;
        private System.Windows.Forms.TextBox textBox_search_kontackts;
        private System.Windows.Forms.Button button_delete_kontackts;
        private System.Windows.Forms.Button button_add_kontackts;
        private System.Windows.Forms.ListView listView_kontackts;
    }
}