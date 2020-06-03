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
            this.button_add_kontragents = new System.Windows.Forms.Button();
            this.button_delete_kontragents = new System.Windows.Forms.Button();
            this.textBox_search_kontragents = new System.Windows.Forms.TextBox();
            this.button_edit_kontragents = new System.Windows.Forms.Button();
            this.dataListView_kontragents = new BrightIdeasSoftware.DataListView();
            this.comboBox_type_kontragent_filter = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataListView_kontragents)).BeginInit();
            this.SuspendLayout();
            // 
            // button_add_kontragents
            // 
            this.button_add_kontragents.Location = new System.Drawing.Point(301, 11);
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
            this.button_edit_kontragents.Location = new System.Drawing.Point(382, 11);
            this.button_edit_kontragents.Name = "button_edit_kontragents";
            this.button_edit_kontragents.Size = new System.Drawing.Size(75, 23);
            this.button_edit_kontragents.TabIndex = 4;
            this.button_edit_kontragents.Text = "Редагувати";
            this.button_edit_kontragents.UseVisualStyleBackColor = true;
            this.button_edit_kontragents.Click += new System.EventHandler(this.button_edit_kontragents_Click);
            // 
            // dataListView_kontragents
            // 
            this.dataListView_kontragents.CellEditUseWholeCell = false;
            this.dataListView_kontragents.DataSource = null;
            this.dataListView_kontragents.FullRowSelect = true;
            this.dataListView_kontragents.GridLines = true;
            this.dataListView_kontragents.HideSelection = false;
            this.dataListView_kontragents.Location = new System.Drawing.Point(12, 41);
            this.dataListView_kontragents.MultiSelect = false;
            this.dataListView_kontragents.Name = "dataListView_kontragents";
            this.dataListView_kontragents.ShowGroups = false;
            this.dataListView_kontragents.Size = new System.Drawing.Size(775, 397);
            this.dataListView_kontragents.TabIndex = 6;
            this.dataListView_kontragents.UseCompatibleStateImageBehavior = false;
            this.dataListView_kontragents.View = System.Windows.Forms.View.Details;
            this.dataListView_kontragents.DoubleClick += new System.EventHandler(this.dataListView_kontragents_DoubleClick);
            // 
            // comboBox_type_kontragent_filter
            // 
            this.comboBox_type_kontragent_filter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_type_kontragent_filter.FormattingEnabled = true;
            this.comboBox_type_kontragent_filter.Location = new System.Drawing.Point(162, 12);
            this.comboBox_type_kontragent_filter.Name = "comboBox_type_kontragent_filter";
            this.comboBox_type_kontragent_filter.Size = new System.Drawing.Size(121, 21);
            this.comboBox_type_kontragent_filter.TabIndex = 7;
            this.comboBox_type_kontragent_filter.SelectedIndexChanged += new System.EventHandler(this.comboBox_type_kontragent_filter_SelectedIndexChanged);
            // 
            // Kontragents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.comboBox_type_kontragent_filter);
            this.Controls.Add(this.dataListView_kontragents);
            this.Controls.Add(this.button_edit_kontragents);
            this.Controls.Add(this.textBox_search_kontragents);
            this.Controls.Add(this.button_delete_kontragents);
            this.Controls.Add(this.button_add_kontragents);
            this.Name = "Kontragents";
            this.Text = "Kontragents";
            ((System.ComponentModel.ISupportInitialize)(this.dataListView_kontragents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_add_kontragents;
        private System.Windows.Forms.Button button_delete_kontragents;
        private System.Windows.Forms.TextBox textBox_search_kontragents;
        private System.Windows.Forms.Button button_edit_kontragents;
        private BrightIdeasSoftware.DataListView dataListView_kontragents;
        private System.Windows.Forms.ComboBox comboBox_type_kontragent_filter;
    }
}