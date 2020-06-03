using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Disp_WinForm
{
    public partial class Kontragents : Form
    {
        Macros macros = new Macros();
        public Kontragents()
        {
            InitializeComponent();
            Right_set();
            init();
            Get_kontragents();
        }

        private void init ()
        {
            // Строим 
            this.comboBox_type_kontragent_filter.DataSource = macros.GetData("SELECT * FROM btk.kontragent_type;");
            this.comboBox_type_kontragent_filter.DisplayMember = "type";
            this.comboBox_type_kontragent_filter.ValueMember = "idkontragent_type";

        }

        private void Right_set()
        {
            if (vars_form.user_accsess_lvl >= 9)
            {
                button_add_kontragents.Enabled = false;
                button_edit_kontragents.Enabled = false;
                button_delete_kontragents.Enabled = false;
            }
        }

        private void Get_kontragents()
        {
            string sql = string.Format("SELECT idKontragenti,  " +
                    "Kontragenti_short_name as 'Скорочена назва', " +
                    "Kontragenti_full_name as 'Повна назва', " +
                    "Kontragenticol_misto as 'Місто', " +
                    "Kontragenticol_kategory as 'Категорія' " +
                    "FROM btk.Kontragenti where (Kontragenti_full_name like '%" + textBox_search_kontragents.Text + "%' or Kontragenti_short_name like '%" + textBox_search_kontragents.Text + "%' or Kontragenticol_misto like '%" + textBox_search_kontragents.Text + "%') and (kontragent_type_idkontragent_type = '" + comboBox_type_kontragent_filter.SelectedValue.ToString() + "' ) ;");

            //if (vars_form.select_sto_or_zakazchik_for_zayavki == 0)
            //{

            //    sql = string.Format("SELECT idKontragenti,  " +
            //        "Kontragenti_short_name as 'Скорочена назва', " +
            //        "Kontragenti_full_name as 'Повна назва', " +
            //        "Kontragenticol_misto as 'Місто', " +
            //        "Kontragenticol_kategory as 'Категорія' " +
            //        "FROM btk.Kontragenti where (Kontragenti_full_name like '%" + textBox_search_kontragents.Text + "%' or Kontragenti_short_name like '%" + textBox_search_kontragents.Text + "%' or Kontragenticol_misto like '%" + textBox_search_kontragents.Text + "%') and (kontragent_type_idkontragent_type = '"+ comboBox_type_kontragent_filter.SelectedValue.ToString() + "' ) ;");
            //}
            //else if (vars_form.select_sto_or_zakazchik_for_zayavki == 1)
            //{
            //    sql = string.Format("SELECT idKontragenti,  " +
            //        "Kontragenti_short_name as 'Скорочена назва', " +
            //        "Kontragenti_full_name as 'Повна назва', " +
            //        "Kontragenticol_misto as 'Місто', " +
            //        "Kontragenticol_kategory as 'Категорія' " +
            //        "FROM btk.Kontragenti where (Kontragenti_full_name like '%" + textBox_search_kontragents.Text + "%' or Kontragenti_short_name like '%" + textBox_search_kontragents.Text + "%' or Kontragenticol_misto like '%" + textBox_search_kontragents.Text + "%') and Kontragenticol_kategory like 'Диллер/СТО';");
            //}
            //else if (vars_form.select_sto_or_zakazchik_for_zayavki == 2)
            //{
            //    sql = string.Format("SELECT idKontragenti,  " +
            //        "Kontragenti_short_name as 'Скорочена назва', " +
            //        "Kontragenti_full_name as 'Повна назва', " +
            //        "Kontragenticol_misto as 'Місто', " +
            //        "Kontragenticol_kategory as 'Категорія' " +
            //        "FROM btk.Kontragenti where Kontragenti_full_name like '%" + textBox_search_kontragents.Text + "%' or Kontragenti_short_name like '%" + textBox_search_kontragents.Text + "%' or Kontragenticol_kategory like 'Замовник' or Kontragenticol_misto like '%" + textBox_search_kontragents.Text + "%';");
            //}
            dataListView_kontragents.BeginUpdate();
            this.dataListView_kontragents.DataSource = macros.GetData(sql);
            dataListView_kontragents.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            dataListView_kontragents.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            dataListView_kontragents.Columns[0].Width = 0;
            dataListView_kontragents.EndUpdate();
        }

        private void button_add_kontragents_Click(object sender, EventArgs e)
        {
            Add_kontragent form_add_kontragents = new Add_kontragent();
            form_add_kontragents.Activated += new EventHandler(form_add_kontragents_activated);
            form_add_kontragents.FormClosed += new FormClosedEventHandler(form_add_kontragents_deactivated);
            form_add_kontragents.Show();
        }

        private void form_add_kontragents_activated(object sender, EventArgs e)
        {
            this.Enabled = false;// блокируем окно контрагентов пока открыто окно добавления контрагента
        }
        private void form_add_kontragents_deactivated(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;// разблокируем окно контрагентов кактолько закрыто окно добавления контрагента
            Get_kontragents();
        }

        private void textBox_search_kontragents_TextChanged(object sender, EventArgs e)
        {
            Get_kontragents();
        }

        private void button_delete_kontragents_Click(object sender, EventArgs e)
        {
            if (dataListView_kontragents.SelectedItems.Count<=0)
            {
                MessageBox.Show("Необхідно вибрати контрагента");
                return;
            }
            string id_selectetd_kontragent = dataListView_kontragents.SelectedItems[0].SubItems[0].Text;
            string name_selectetd_kontragent = dataListView_kontragents.SelectedItems[0].SubItems[1].Text;

            DialogResult dialogResult = MessageBox.Show("Видалити " + name_selectetd_kontragent + " ?", "Видалення контрагенту", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                macros.sql_command("DELETE FROM btk.Kontragenti WHERE idKontragenti=" + id_selectetd_kontragent + ";");
                Get_kontragents();
            }
        }

        private void button_edit_kontragents_Click(object sender, EventArgs e)
        {
            if (dataListView_kontragents.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Необхідно вибрати контрагента");
                return;
            }

            vars_form.btk_idkontragents = dataListView_kontragents.SelectedItems[0].SubItems[0].Text;

            Edit_kontragents form_edit_kontragents = new Edit_kontragents();
            form_edit_kontragents.Activated += new EventHandler(form_edit_kontragents_activated);
            form_edit_kontragents.FormClosed += new FormClosedEventHandler(form_edit_kontragents_deactivated);
            form_edit_kontragents.Show();
        }
        private void form_edit_kontragents_activated(object sender, EventArgs e)
        {
            this.Enabled = false;// блокируем окно контрагентов пока открыто окно добавления контрагента
        }
        private void form_edit_kontragents_deactivated(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;// разблокируем окно контрагентов кактолько закрыто окно добавления контрагента
            Get_kontragents();
        }

        private void dataListView_kontragents_DoubleClick(object sender, EventArgs e)
        {
            if (dataListView_kontragents.SelectedItem != null)
            {
                if (vars_form.select_sto_or_zakazchik_for_zayavki == 1)
                {
                    vars_form.id_kontragent_sto_for_zayavki = dataListView_kontragents.SelectedItem.Text;
                }
                else if (vars_form.select_sto_or_zakazchik_for_zayavki == 0)
                {
                    vars_form.id_kontragent_zakazchik_for_zayavki = dataListView_kontragents.SelectedItem.Text;
                }
                
            }
            this.Close();
        }

        private void comboBox_type_kontragent_filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            Get_kontragents();
        }
    }
}
