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
    public partial class Add_kontakt : Form
    {
        Macros macros = new Macros();
        public Add_kontakt()
        {
            InitializeComponent();

        }

        private void button_create_Click(object sender, EventArgs e)
        {
            if (comboBox_work_in.SelectedIndex == -1)
            {
                comboBox_work_in.BackColor = Color.Red;
                return;
            }

            if (textBox_name.Text == "")
            {
                textBox_name.BackColor = Color.Red;
                return;
            }

            string emailID = "";
            if (maskedTextBox_mail.Text != "")
            {
                macros.sql_command("insert into btk.Emails(Emailscol_email) values('" + maskedTextBox_mail.Text + "');");
                emailID= macros.sql_command("SELECT MAX(idEmails) FROM btk.Emails;");
            }
            else
            { emailID = "1"; }

            string email2ID = "";
            if (maskedTextBox_mail2.Text != "")
            {
                macros.sql_command("insert into btk.Emails(Emailscol_email) values('" + maskedTextBox_mail2.Text + "');");
                email2ID = macros.sql_command("SELECT MAX(idEmails) FROM btk.Emails;");
            }
            else
            { email2ID = "1"; }


            string phoneID = "";
            if (maskedTextBox_tel1.Text != "")
            {
                macros.sql_command("insert into btk.Phonebook(Phonebookcol_phone) values('" + maskedTextBox_tel1.Text.ToString() + "');");
                phoneID = macros.sql_command("SELECT MAX(idPhonebook) FROM btk.Phonebook;");
            }
            else
            { phoneID = "1"; }

            string phone2ID = "";
            if (maskedTextBox_tel2.Text != "")
            {
                macros.sql_command("insert into btk.Phonebook(Phonebookcol_phone) values('" + maskedTextBox_tel2.Text.ToString() + "');");
                phone2ID = macros.sql_command("SELECT MAX(idPhonebook) FROM btk.Phonebook;");
            }
            else
            { phone2ID = "1"; }


            macros.sql_command("insert into btk.Kontakti (Emails_idEmails1,Phonebook_idPhonebook1,Kontakti_user_creator,Kontakti_user_edit, Kontakti_imya, Kontakti_familia,Emails_idEmails,Phonebook_idPhonebook) values('" + email2ID + "','" + phone2ID + "','" + vars_form.user_login_id + "','" + vars_form.user_login_id+"','" + textBox_name.Text.ToString() + "','" + textBox_familia.Text.ToString() + "','" + emailID + "','" + phoneID + "');");

            string contactID = macros.sql_command("SELECT MAX(idKontakti) FROM btk.Kontakti;");

            if (comboBox_work_in.SelectedValue!=null)
            {
                macros.sql_command("insert into btk.Kontakti_has_Kontragenti (Kontakti_idKontakti, Kontragenti_idKontragenti) values ('" + contactID + "','" + comboBox_work_in.SelectedValue + "')");
            }

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox_work_in_DropDown(object sender, EventArgs e)
        {
            comboBox_work_in.BackColor = Color.Empty;
            string sql = string.Format("select Kontragenti.idKontragenti, concat(btk.Kontragenti.Kontragenti_short_name, '(',btk.Kontragenti.Kontragenti_full_name, ')') AS full_short  FROM btk.Kontragenti where Kontragenticol_kategory='Диллер/СТО'");
            comboBox_work_in.DisplayMember = "full_short";
            comboBox_work_in.ValueMember = "idKontragenti";
            comboBox_work_in.DataSource = macros.GetData(sql);
        }

        private void comboBox_work_in_TextUpdate(object sender, EventArgs e)
        {
            string from_textbox = comboBox_work_in.Text.ToString();
            string sql = string.Format("select Kontragenti.idKontragenti, concat(btk.Kontragenti.Kontragenti_short_name, '(',btk.Kontragenti.Kontragenti_full_name, ')') AS full_short  FROM btk.Kontragenti where Kontragenticol_kategory = 'Диллер/СТО' and (Kontragenti_short_name like '%"+comboBox_work_in.Text+"%' or Kontragenti_full_name like '%"+comboBox_work_in.Text+"%')");
            comboBox_work_in.DataSource = macros.GetData(sql);
            comboBox_work_in.DisplayMember = "full_short";
            comboBox_work_in.ValueMember = "idKontragenti";
            comboBox_work_in.Text = from_textbox;
            comboBox_work_in.SelectionStart = comboBox_work_in.Text.Length;
            comboBox_work_in.DroppedDown = true;
        }

        private void button_add_kontragent_Click(object sender, EventArgs e)
        {
            Add_kontragent form_Add_kontragent = new Add_kontragent();
            form_Add_kontragent.Activated += new EventHandler(form_Add_kontragent_activated);
            form_Add_kontragent.FormClosed += new FormClosedEventHandler(form_Add_kontragent_deactivated);
            form_Add_kontragent.Show();
        }

        private void form_Add_kontragent_activated(object sender, EventArgs e)
        {
            this.Visible = false;// блокируем окно контрагентов пока открыто окно добавления контрагента
        }
        private void form_Add_kontragent_deactivated(object sender, FormClosedEventArgs e)
        {
            this.Visible = true;// разблокируем окно контрагентов кактолько закрыто окно добавления контрагента
        }
    }
}
