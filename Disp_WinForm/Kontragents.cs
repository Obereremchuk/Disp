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
            Bild_listview_kontragets();
            Get_kontragents();
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

        private void Bild_listview_kontragets()
        {
            listView_kontragents.View = View.Tile;
            listView_kontragents.FullRowSelect = true;
            listView_kontragents.GridLines = true;
            listView_kontragents.View = View.Details;
            listView_kontragents.ShowItemToolTips = true;

            // Attach Subitems to the ListView
            listView_kontragents.Columns.Add("idKontragenti", 1, HorizontalAlignment.Left);
            listView_kontragents.Columns.Add("Повна назва", -2, HorizontalAlignment.Left);
            listView_kontragents.Columns.Add("Скорочена назва", -2, HorizontalAlignment.Left);
            listView_kontragents.Columns.Add("Місто", -2, HorizontalAlignment.Left);
            listView_kontragents.Columns.Add("Категорія", -2, HorizontalAlignment.Left);
        }

        private void Get_kontragents()
        {
            string sql = "";
            MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True;  charset=utf8");
            
            if (vars_form.select_sto_or_zakazchik_for_zayavki == 0)
            {
                sql = string.Format("SELECT * FROM btk.Kontragenti where (Kontragenti_full_name like '%" + textBox_search_kontragents.Text + "%' or Kontragenti_short_name like '%" + textBox_search_kontragents.Text + "%' or Kontragenticol_misto like '%" + textBox_search_kontragents.Text + "%') and Kontragenticol_kategory like 'СК' ;");
            }
            else if (vars_form.select_sto_or_zakazchik_for_zayavki == 1)
            {
                sql = string.Format("SELECT * FROM btk.Kontragenti where (Kontragenti_full_name like '%" + textBox_search_kontragents.Text + "%' or Kontragenti_short_name like '%" + textBox_search_kontragents.Text + "%' or Kontragenticol_misto like '%" + textBox_search_kontragents.Text + "%') and Kontragenticol_kategory like 'Диллер/СТО' ;");
            }
            else if (vars_form.select_sto_or_zakazchik_for_zayavki == 2)
            {
                sql = string.Format("SELECT * FROM btk.Kontragenti where Kontragenti_full_name like '%" + textBox_search_kontragents.Text + "%' or Kontragenti_short_name like '%" + textBox_search_kontragents.Text + "%' or Kontragenticol_kategory like 'Замовник' or Kontragenticol_misto like '%" + textBox_search_kontragents.Text + "%';");
            }

            MySqlCommand myDataAdapter = new MySqlCommand(sql, myConnection);
            myConnection.Open();
            MySqlDataReader reader = myDataAdapter.ExecuteReader();

            listView_kontragents.Items.Clear();

            while (reader.Read())
            {
                var item = new ListViewItem();
                item.Text = reader["idKontragenti"].ToString();
                item.SubItems.Add(reader["Kontragenti_full_name"].ToString());
                item.SubItems.Add(reader["Kontragenti_short_name"].ToString());
                item.SubItems.Add(reader["Kontragenticol_misto"].ToString());
                item.SubItems.Add(reader["Kontragenticol_kategory"].ToString());
                listView_kontragents.Items.Add(item);
            }
            reader.Close();
            myDataAdapter.Dispose();
            myConnection.Close();

            listView_kontragents.View = View.Details;
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
            if (listView_kontragents.SelectedItems.Count<=0)
            {
                MessageBox.Show("Необхідно вибрати контрагента");
                return;
            }
            string id_selectetd_kontragent = listView_kontragents.SelectedItems[0].SubItems[0].Text;
            string name_selectetd_kontragent = listView_kontragents.SelectedItems[0].SubItems[1].Text;

            DialogResult dialogResult = MessageBox.Show("Видалити " + name_selectetd_kontragent + " ?", "Видалення контрагенту", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                macros.sql_command("DELETE FROM btk.Kontragenti WHERE idKontragenti=" + id_selectetd_kontragent + ";");
                Get_kontragents();
            }
        }

        private void button_edit_kontragents_Click(object sender, EventArgs e)
        {
            if (listView_kontragents.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Необхідно вибрати контрагента");
                return;
            }

            vars_form.btk_idkontragents = listView_kontragents.SelectedItems[0].SubItems[0].Text;

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

        private void listView_kontragents_DoubleClick(object sender, EventArgs e)
        {
            for (int i = 0; i < listView_kontragents.Items.Count; i++)
                // is i the index of the row I selected?
                if (listView_kontragents.Items[i].Selected == true)
                {
                    if (vars_form.select_sto_or_zakazchik_for_zayavki == 1)
                    {
                        vars_form.id_kontragent_sto_for_zayavki = listView_kontragents.Items[i].SubItems[0].Text;
                    }
                    else if (vars_form.select_sto_or_zakazchik_for_zayavki == 0)
                    {
                        vars_form.id_kontragent_zakazchik_for_zayavki = listView_kontragents.Items[i].SubItems[0].Text;
                    }
                    break;
                }
            this.Close();
        }
    }
}
