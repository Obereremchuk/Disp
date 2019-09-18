using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Disp_WinForm
{
    public partial class Activation_Form : Form
    {
        public Activation_Form()
        {
            InitializeComponent();
            //build_activation_listwiev();
        }

        private void build_activation_listwiev()
        {
            var column = new DataGridViewComboBoxColumn();

            DataTable data = new DataTable();

            data.Columns.Add(new DataColumn("Value", typeof(string)));
            data.Columns.Add(new DataColumn("Description", typeof(string)));

            data.Rows.Add("item1", "123");
            data.Rows.Add("item2", "234");
            data.Rows.Add("item3", "245");

            column.DataSource = data;
            column.ValueMember = "Value";
            column.DisplayMember = "Description";

            dataGridView_activation.Columns.Add(column);
            dataGridView_activation.Columns[0].Name = "Фамілія";

            //dataGridView_activation.Columns.Add(column);
            //dataGridView_activation.Columns[0].Name = "Імя";

            //dataGridView_activation.ColumnCount = 3;
            //dataGridView_activation.Columns[0].Name = "Фамілія";
            //dataGridView_activation.Columns[1].Name = "Імя";
            //dataGridView_activation.Columns[2].Name = "Телефон";


            ////string[] row = new string[] { "1", "Product 1", "1000" };
            ////dataGridView_activation.Rows.Add(row);
            ////row = new string[] { "2", "Product 2", "2000" };
            ////dataGridView_activation.Rows.Add(row);
            ////row = new string[] { "3", "Product 3", "3000" };
            ////dataGridView_activation.Rows.Add(row);
            ////row = new string[] { "4", "Product 4", "4000" };
            ////dataGridView_activation.Rows.Add(row);

            //DataGridViewComboBoxColumn cmb = new DataGridViewComboBoxColumn();
            //cmb.HeaderText = "Select Data";
            //cmb.Name = "cmb";
            //cmb.MaxDropDownItems = 4;
            //cmb.Items.Add("True");
            //cmb.Items.Add("False");
            //dataGridView_activation.Columns.Add(cmb);



        }


        private void dataGridView_activation_EditingControlShowing(object sender,
            DataGridViewEditingControlShowingEventArgs e)
        {


            if (dataGridView_activation.CurrentCell.ColumnIndex == 0)
            {
                ComboBox combo = e.Control as ComboBox;

                if (combo == null)
                    return;

                combo.DropDownStyle = ComboBoxStyle.DropDown;
            }
        }
    }
}
