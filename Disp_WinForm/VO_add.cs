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
    public partial class VO_add : Form
    {
        Macros macros = new Macros();
        
        public VO_add()
        {
            InitializeComponent();
            init();
        }
        private void init()
        {
            string sql = string.Format("SELECT idKontakti, concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,' ',  COALESCE (Phonebook.Phonebookcol_phone,''))  as fio FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and Kontakti.Kontact_type_idKontact_type = '1' and Kontakti_familia like '%" + textBox_familiya_vo.Text + "%'; ");
            listBox_kontakts_list.DataSource = macros.GetData(sql);
            listBox_kontakts_list.DisplayMember = "fio";
            listBox_kontakts_list.ValueMember = "idKontakti";
        }

        private void textBox_familiya_vo_TextChanged(object sender, EventArgs e)
        {
            if (textBox_familiya_vo.Text == "" || textBox_familiya_vo.Text != listBox_kontakts_list.GetItemText(listBox_kontakts_list.SelectedItem))
            {
                textBox_imya_vo.Text = "";
                //textBox_familiya_vo.Text = "";
                maskedTextBox_phone1_vo.Text = "";
                maskedTextBox_phone2_vo.Text = "";
                textBox_otchestvo_vo.Text = "";

                comboBox_messanger_1.Items.Clear();
                comboBox_messanger_2.Items.Clear();

                string sql = string.Format("SELECT idKontakti, concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,' ',  COALESCE (Phonebook.Phonebookcol_phone,''))  as fio FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and Kontakti.Kontact_type_idKontact_type = '1' and Kontakti_familia like '%" + textBox_familiya_vo.Text + "%'; ");
                listBox_kontakts_list.DataSource = macros.GetData(sql);
                listBox_kontakts_list.DisplayMember = "fio";
                listBox_kontakts_list.ValueMember = "idKontakti";
            }
            else
            {
                string sql = string.Format("SELECT idKontakti, concat(COALESCE (Kontakti_familia,'') ,' ', COALESCE (Kontakti_imya,'') ,' ', COALESCE (Kontakti_otchestvo,'') ,' ',  COALESCE (Phonebook.Phonebookcol_phone,''))  as fio FROM btk.Kontakti, btk.Phonebook where Phonebook.idPhonebook=Kontakti.Phonebook_idPhonebook and Kontakti.Kontact_type_idKontact_type = '1' and Kontakti_familia like '%" + textBox_familiya_vo.Text + "%'; ");
                listBox_kontakts_list.DataSource = macros.GetData(sql);
                listBox_kontakts_list.DisplayMember = "fio";
                listBox_kontakts_list.ValueMember = "idKontakti";
            }
        }


        private void listBox_kontakts_list_DoubleClick(object sender, EventArgs e)
        {
            if (listBox_kontakts_list.Items.Count > 0)
            {
                string sql = string.Format("SELECT Kontakti_imya ,Kontakti_familia, Kontakti_comment,Emails_idEmails,Emails_idEmails1,Phonebook_idPhonebook,Phonebook_idPhonebook1, Kontakti_otchestvo FROM btk.Kontakti where idKontakti = " + listBox_kontakts_list.SelectedValue.ToString() + "; ");
                DataTable table = new DataTable();
                table = macros.GetData(sql);


                textBox_familiya_vo.Text = table.Rows[0][1].ToString();
                textBox_imya_vo.Text = table.Rows[0][0].ToString();
                textBox_comment.Text = table.Rows[0][2].ToString();
                textBox_otchestvo_vo.Text = table.Rows[0][7].ToString();

                maskedTextBox_phone1_vo.Text = macros.sql_command("SELECT Phonebookcol_phone FROM btk.Phonebook where idPhonebook = '" + table.Rows[0][5].ToString() + "';");
                maskedTextBox_phone2_vo.Text = macros.sql_command("SELECT Phonebookcol_phone FROM btk.Phonebook where idPhonebook = '" + table.Rows[0][6].ToString() + "';");

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_familiya_vo.Text == "")
            {
                textBox_familiya_vo.BackColor = Color.Red;
                return;
            }
            else 
            {
                textBox_familiya_vo.BackColor = Color.Empty;
            }

            if (textBox_imya_vo.Text == "")
            {
                textBox_imya_vo.BackColor = Color.Red;
                return;
            }
            else
            {
                textBox_imya_vo.BackColor = Color.Empty;
            }

            string emailID = "1";
            string email2ID = "1";
            

            string phoneID = "";
            if (maskedTextBox_phone1_vo.Text != "")
            {
                macros.sql_command("insert into btk.Phonebook(Phonebookcol_phone) values('" + maskedTextBox_phone1_vo.Text.ToString() + "');");
                phoneID = macros.sql_command("SELECT MAX(idPhonebook) FROM btk.Phonebook;");
            }
            else
            { phoneID = "1"; }

            string phone2ID = "";
            if (maskedTextBox_phone1_vo.Text != "")
            {
                macros.sql_command("insert into btk.Phonebook(Phonebookcol_phone) values('" + maskedTextBox_phone2_vo.Text.ToString() + "');");
                phone2ID = macros.sql_command("SELECT MAX(idPhonebook) FROM btk.Phonebook;");
            }
            else
            { phone2ID = "1"; }


            macros.sql_command("insert into btk.Kontakti (Emails_idEmails1,Phonebook_idPhonebook1,Kontakti_user_creator,Kontakti_user_edit, Kontakti_imya, Kontakti_familia,Emails_idEmails,Phonebook_idPhonebook, Kontakti_comment, Kontact_type_idKontact_type, Kontakti_otchestvo) values('" + email2ID + "','" + phone2ID + "','" + vars_form.user_login_id + "','" + vars_form.user_login_id + "','" + textBox_imya_vo.Text.ToString() + "','" + textBox_familiya_vo.Text.ToString() + "','" + emailID + "','" + phoneID + "','" +textBox_comment.Text + "','1','"+textBox_otchestvo_vo.Text+"');");

            string contactID = macros.sql_command("SELECT MAX(idKontakti) FROM btk.Kontakti;");
            init();
        }

        private void button_update_kontakt_Click(object sender, EventArgs e)
        {
            vars_form.btk_idkontragents = listBox_kontakts_list.SelectedValue.ToString();
            Edit_kontakts edit_Kontakts = new Edit_kontakts();
            edit_Kontakts.Show();

        }

        private void button_select_Click(object sender, EventArgs e)
        {
            if (vars_form.num_vo == 1)
            {
                vars_form.transfer_vo1_vo_form = listBox_kontakts_list.SelectedValue.ToString();
            }
            else if (vars_form.num_vo == 2)
            {
                vars_form.transfer_vo2_vo_form = listBox_kontakts_list.SelectedValue.ToString();
            }
            else if (vars_form.num_vo == 3)
            {
                vars_form.transfer_vo3_vo_form = listBox_kontakts_list.SelectedValue.ToString();
            }
            else if (vars_form.num_vo == 4)
            {
                vars_form.transfer_vo4_vo_form = listBox_kontakts_list.SelectedValue.ToString();
            }
            else if (vars_form.num_vo == 5)
            {
                vars_form.transfer_vo5_vo_form = listBox_kontakts_list.SelectedValue.ToString();
            }
            this.Close();
        }
    }
}
