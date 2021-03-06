﻿using System;
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
            Bild_listview_kontragets();
            Get_kontragents();
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
            MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True");
            string sql = string.Format("SELECT * FROM btk.Kontragenti where Kontragenti_full_name like '%"+textBox_search_kontragents.Text+ "%' or Kontragenti_short_name like '%" + textBox_search_kontragents.Text + "%' or Kontragenticol_kategory like '%" + textBox_search_kontragents.Text + "%' or Kontragenticol_misto like '%" + textBox_search_kontragents.Text + "%';");
            MySqlCommand myDataAdapter = new MySqlCommand(sql, myConnection);
            myConnection.Open();
            MySqlDataReader reader = myDataAdapter.ExecuteReader();
            List<string> results = new List<string>();

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
    }
}
