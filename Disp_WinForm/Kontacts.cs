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
    public partial class Kontacts : Form
    {
        Macros macros = new Macros();
        public Kontacts()
        {
            
            InitializeComponent();
            Right_set();
            Bild_listview_kontackts();
            Get_kontackts();
        }

        private void Right_set()
        {
            //if (vars_form.user_accsess_lvl >= 9)
            //{
            //    button_add_kontackts.Enabled = false;
            //    button_edit_kontackts.Enabled = false;
            //    button_delete_kontackts.Enabled = false;
            //}
        }

        private void Bild_listview_kontackts()
        {
            listView_kontackts.View = View.Tile;
            listView_kontackts.FullRowSelect = true;
            listView_kontackts.GridLines = true;
            listView_kontackts.View = View.Details;
            listView_kontackts.ShowItemToolTips = true;

            // Attach Subitems to the ListView
            listView_kontackts.Columns.Add("idKontakti", 1, HorizontalAlignment.Left);
            listView_kontackts.Columns.Add("Ім’я", -2, HorizontalAlignment.Left);
            listView_kontackts.Columns.Add("Призвище", -2, HorizontalAlignment.Left);
            listView_kontackts.Columns.Add("Побатькові", -2, HorizontalAlignment.Left);
            listView_kontackts.Columns.Add("Телефон основний", -2, HorizontalAlignment.Left);
            listView_kontackts.Columns.Add("Телефон Додатковий", -2, HorizontalAlignment.Left);
            listView_kontackts.Columns.Add("Працює в", -2, HorizontalAlignment.Left);
        }

        private void Get_kontackts()
        {
            if (vars_form.num_vo >= 1)// vars_form.num_vo = 0 >> run for electrik (testuvannya) (idKontact_type=2(Електрик)), vars_form.num_vo = 1 >> run from VO (idKontact_type=1(Кліент))
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                string sql = string.Format("SELECT * FROM btk.Kontakti, btk.Phonebook " +
                    "where " +
                    "(" +
                    "Kontakti_imya like '%" + textBox_search_kontackts.Text + "%' " +
                    "Kontakti_otchestvo like '%" + textBox_search_kontackts.Text + "%' " + 
                    "or Kontakti_familia like '%" + textBox_search_kontackts.Text + "%' " +
                    "or Phonebook.Phonebookcol_phone like '%" + textBox_search_kontackts.Text + "%' " +
                    ") " +
                    "and (Phonebook.idPhonebook = Kontakti.Phonebook_idPhonebook) " +
                    //"and (Phonebook.idPhonebook = Kontakti.Phonebook_idPhonebook or Phonebook.idPhonebook = Kontakti.Phonebook_idPhonebook1) " +
                    "and Kontact_type_idKontact_type = '1';");
                MySqlCommand myDataAdapter = new MySqlCommand(sql, myConnection);
                myConnection.Open();
                MySqlDataReader reader = myDataAdapter.ExecuteReader();
                List<string> results = new List<string>();

                listView_kontackts.Items.Clear();
                listView_kontackts.View = View.List;


                while (reader.Read())
                {
                    var item = new ListViewItem();
                    item.Text = reader["idKontakti"].ToString();
                    item.SubItems.Add(reader["Kontakti_imya"].ToString());
                    item.SubItems.Add(reader["Kontakti_familia"].ToString());
                    item.SubItems.Add(reader["Kontakti_otchestvo"].ToString());
                    //item.SubItems.Add(reader["Phonebook_idPhonebook"].ToString());
                    item.SubItems.Add(macros.sql_command("SELECT Phonebookcol_phone FROM btk.Phonebook where idPhonebook=" + reader["Phonebook_idPhonebook"].ToString() + ";"));
                    //item.SubItems.Add(reader["Phonebook_idPhonebook1"].ToString());
                    item.SubItems.Add(macros.sql_command("SELECT Phonebookcol_phone FROM btk.Phonebook where idPhonebook=" + reader["Phonebook_idPhonebook1"].ToString() + ";"));
                    string t = macros.sql_command("SELECT Kontragenti_idKontragenti FROM btk.Kontakti_has_Kontragenti where Kontakti_idKontakti='" + reader["idKontakti"].ToString() + "';");
                    item.SubItems.Add(macros.sql_command("select  concat(Kontragenti.Kontragenti_short_name, '(',Kontragenti.Kontragenti_full_name, ')') AS full_short  FROM btk.Kontragenti where idKontragenti='" + t + "'"));
                    listView_kontackts.Items.Add(item);
                }
                reader.Close();
                myDataAdapter.Dispose();
                myConnection.Close();

                listView_kontackts.Visible = true;
                listView_kontackts.View = View.Details;
            }
            else
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                //string sql = string.Format("SELECT * FROM btk.Kontakti where Kontakti_imya like '%" + textBox_search_kontackts.Text + "%' or Kontakti_familia like '%" + textBox_search_kontackts.Text + "%' or Phonebook_idPhonebook like '%" + textBox_search_kontackts.Text + "%' or Phonebook_idPhonebook1 like '%" + textBox_search_kontackts.Text + "%';");
                string sql = string.Format("SELECT * FROM btk.Kontakti where (Kontakti_imya like '%" + textBox_search_kontackts.Text + "%' or Kontakti_familia like '%" + textBox_search_kontackts.Text + "%' or Phonebook_idPhonebook like '%" + textBox_search_kontackts.Text + "%' or Phonebook_idPhonebook1 like '%" + textBox_search_kontackts.Text + "%') and Kontact_type_idKontact_type = '2';");
                MySqlCommand myDataAdapter = new MySqlCommand(sql, myConnection);
                myConnection.Open();
                MySqlDataReader reader = myDataAdapter.ExecuteReader();
                List<string> results = new List<string>();

                listView_kontackts.Items.Clear();
                listView_kontackts.View = View.List;


                while (reader.Read())
                {
                    var item = new ListViewItem();
                    item.Text = reader["idKontakti"].ToString();
                    item.SubItems.Add(reader["Kontakti_imya"].ToString());
                    item.SubItems.Add(reader["Kontakti_familia"].ToString());
                    item.SubItems.Add(reader["Kontakti_otchestvo"].ToString());
                    //item.SubItems.Add(reader["Phonebook_idPhonebook"].ToString());
                    item.SubItems.Add(macros.sql_command("SELECT Phonebookcol_phone FROM btk.Phonebook where idPhonebook=" + reader["Phonebook_idPhonebook"].ToString() + ";"));
                    //item.SubItems.Add(reader["Phonebook_idPhonebook1"].ToString());
                    item.SubItems.Add(macros.sql_command("SELECT Phonebookcol_phone FROM btk.Phonebook where idPhonebook=" + reader["Phonebook_idPhonebook1"].ToString() + ";"));
                    string t = macros.sql_command("SELECT Kontragenti_idKontragenti FROM btk.Kontakti_has_Kontragenti where Kontakti_idKontakti='" + reader["idKontakti"].ToString() + "';");
                    item.SubItems.Add(macros.sql_command("select  concat(Kontragenti.Kontragenti_short_name, '(',Kontragenti.Kontragenti_full_name, ')') AS full_short  FROM btk.Kontragenti where idKontragenti='" + t + "'"));
                    listView_kontackts.Items.Add(item);
                }
                reader.Close();
                myDataAdapter.Dispose();
                myConnection.Close();

                listView_kontackts.Visible = true;
                listView_kontackts.View = View.Details;
            }
        }

        private void button_add_kontackts_Click(object sender, EventArgs e)
        {
            
            Add_kontakt form_Add_kontakt = new Add_kontakt();
            form_Add_kontakt.Activated += new EventHandler(form_Add_kontakt_activated);
            form_Add_kontakt.FormClosed += new FormClosedEventHandler(form_Add_kontakt_deactivated);
            form_Add_kontakt.Show();
        }
        private void form_Add_kontakt_activated(object sender, EventArgs e)
        {
            this.Enabled = false;// блокируем окно контрагентов пока открыто окно добавления контрагента
        }
        private void form_Add_kontakt_deactivated(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;// разблокируем окно контрагентов кактолько закрыто окно добавления контрагента
            Get_kontackts();
        }

        private void button_delete_kontackts_Click(object sender, EventArgs e)
        {
            if (listView_kontackts.SelectedItems.Count <= 0)//если не вібран ниодин из списка просим вібрать и отменяем операцию
            {
                MessageBox.Show("Необхідно вибрати контакт");
                return;
            }
            string id_selectetd_kontackts = listView_kontackts.SelectedItems[0].SubItems[0].Text;//сохраняем айди контакта  для его удаления
            string name_selectetd_kontackts = listView_kontackts.SelectedItems[0].SubItems[1].Text;//сохраняем имя контакта для вопроса об удалении

            DialogResult dialogResult = MessageBox.Show("Видалити " + name_selectetd_kontackts + " ?", "Видалення контрагенту", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                macros.sql_command("DELETE FROM btk.Kontakti_has_Kontragenti WHERE Kontakti_idKontakti=" + id_selectetd_kontackts + ";");//удаляем связи удяляемого контакта с контрагентом
                macros.sql_command("DELETE FROM btk.Kontakti WHERE idKontakti=" + id_selectetd_kontackts + ";");//удаляем сам контакт
                Get_kontackts();
            }
        }
        private void button_edit_kontackts_Click(object sender, EventArgs e)
        {
            if (listView_kontackts.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Необхідно вибрати контрагента");
                return;
            }

            vars_form.btk_idkontragents = listView_kontackts.SelectedItems[0].SubItems[0].Text;//сохраняем айди контакта для использования в другом окне

            Edit_kontakts form_edit_kontakts = new Edit_kontakts();//запускаем окно редактирования контакта
            form_edit_kontakts.Activated += new EventHandler(form_edit_kontakts_activated);
            form_edit_kontakts.FormClosed += new FormClosedEventHandler(form_edit_kontakts_deactivated);
            form_edit_kontakts.Show();
        }
        private void form_edit_kontakts_activated(object sender, EventArgs e)
        {
            this.Enabled = false;// блокируем окно контрагентов пока открыто окно добавления контрагента
        }
        private void form_edit_kontakts_deactivated(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;// разблокируем окно контрагентов кактолько закрыто окно добавления контрагента
            Get_kontackts();
        }

        private void textBox_search_kontackts_TextChanged(object sender, EventArgs e)
        {
            Get_kontackts();
        }

        private void listView_kontackts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (vars_form.num_vo >= 1)
            {
                ListViewHitTestInfo info = listView_kontackts.HitTest(e.X, e.Y);
                ListViewItem item = info.Item;

                if (item != null)
                {
                    if (vars_form.num_vo == 1)
                    {
                        vars_form.transfer_vo1_vo_form = item.Text;
                    }
                    if (vars_form.num_vo == 2)
                    {
                        vars_form.transfer_vo2_vo_form = item.Text;
                    }
                    if (vars_form.num_vo == 3)
                    {
                        vars_form.transfer_vo3_vo_form = item.Text;
                    }
                    if (vars_form.num_vo == 4)
                    {
                        vars_form.transfer_vo4_vo_form = item.Text;
                    }
                    if (vars_form.num_vo == 5)
                    {
                        vars_form.transfer_vo5_vo_form = item.Text;
                    }
                }
            }
            this.Close();
        }
    }
}
