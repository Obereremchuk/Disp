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
    public partial class Edit_kontakts : Form
    {
        Macros macros = new Macros();
        string id_phon1 = "";
        string id_phon2 = "";
        string id_mail1 = "";
        string id_mail2 = "";
        public Edit_kontakts()
        {
            InitializeComponent();
            Get_kontackt();
        }

        private void Get_kontackt()
        {
            MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True");
            string sql = string.Format("SELECT * FROM btk.Kontakti where idKontakti=" + vars_form.btk_idkontragents + ";");
            MySqlCommand myDataAdapter = new MySqlCommand(sql, myConnection);
            myConnection.Open();
            MySqlDataReader reader = myDataAdapter.ExecuteReader();


            while (reader.Read())
            {

                textBox_name.Text = reader["Kontakti_imya"].ToString();
                textBox_familia.Text = reader["Kontakti_familia"].ToString();
                id_phon1 = reader["Phonebook_idPhonebook"].ToString();
                id_phon2 = reader["Phonebook_idPhonebook1"].ToString();
                id_mail1 = reader["Emails_idEmails"].ToString();
                id_mail2 = reader["Emails_idEmails1"].ToString();
                textBox_coment.Text= reader["Kontakti_comment"].ToString();
                textBox_coment.Text= reader["Kontakti_comment"].ToString();
                string t = macros.sql_command2("SELECT Kontragenti_idKontragenti FROM btk.Kontakti_has_Kontragenti where Kontakti_idKontakti='" +
                                               reader["idKontakti"].ToString() + "';");
                comboBox_work_in.Text =
                    macros.sql_command2(
                        "select  concat(Kontragenti.Kontragenti_short_name, '(',Kontragenti.Kontragenti_full_name, ')') AS full_short  FROM btk.Kontragenti where idKontragenti='" +
                        t + "'");

            }
            reader.Close();
            myDataAdapter.Dispose();
            myConnection.Close();

            textBox_tel1.Text = macros.sql_command2("SELECT Phonebookcol_phone FROM btk.Phonebook where idPhonebook=" + id_phon1 + "; ");
            textBox_tel2.Text = macros.sql_command2("SELECT Phonebookcol_phone FROM btk.Phonebook where idPhonebook=" + id_phon2 + "; ");
            textBox_mail.Text = macros.sql_command2("SELECT Emailscol_email FROM btk.Emails where idEmails=" + id_mail1 + "; ");
            textBox_mail2.Text = macros.sql_command2("SELECT Emailscol_email FROM btk.Emails where idEmails=" + id_mail2 + "; ");

        }

        private void button_add_kontragent_Click(object sender, EventArgs e)
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
            Get_kontackt();
        }

        private void button_exit_whithout_saving_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_create_Click(object sender, EventArgs e)
        {
            if (textBox_name.Text == "")
            {
                textBox_name.BackColor = Color.Red;
                return;
            }

            if (id_mail1 == "1")//если редактируем пустіе данніе
            {
                //Создаем имел, в ответ получаем айди созданой записи. Если пусто то присваиваем айди №1 = пусто.
                if (textBox_mail.Text != "")//если поле не пустое
                {
                    macros.sql_command("insert into btk.Emails(Emailscol_email) values('" + textBox_mail.Text + "');");
                    id_mail1 = macros.sql_command2("SELECT MAX(idEmails) FROM btk.Emails;");
                }
                else
                { id_mail1 = "1"; }
            }
            else
            {
                //Создаем имел, в ответ получаем айди созданой записи. Если пусто то присваиваем айди №1 = пусто.

                if (textBox_mail.Text != "")
                {
                    macros.sql_command("update btk.Emails set Emailscol_email='" + textBox_mail.Text + "' where idEmails=" + id_mail1 + ";");
                }
                else
                {
                    id_mail1 = "1";
                }
            }



            if (id_mail2 == "1")//если редактируем пустіе данніе
            {
                //Создаем имел, в ответ получаем айди созданой записи. Если пусто то присваиваем айди №1 = пусто.
                if (textBox_mail2.Text != "")//если поле не пустое
                {
                    macros.sql_command("insert into btk.Emails(Emailscol_email) values('" + textBox_mail2.Text + "');");
                    id_mail2 = macros.sql_command2("SELECT MAX(idEmails) FROM btk.Emails;");
                }
                else
                { id_mail2 = "1"; }
            }
            else
            {
                //Создаем имел, в ответ получаем айди созданой записи. Если пусто то присваиваем айди №1 = пусто.

                if (textBox_mail2.Text != "")
                {
                    macros.sql_command("update btk.Emails set Emailscol_email='" + textBox_mail2.Text + "' where idEmails=" + id_mail2 + ";");
                }
                else
                {
                    id_mail2 = "1";
                }
            }

            if (id_phon1 == "1")//если редактируем пустіе данніе
            {
                //Создаем имел, в ответ получаем айди созданой записи. Если пусто то присваиваем айди №1 = пусто.
                if (textBox_tel1.Text != "")//если поле не пустое
                {
                    macros.sql_command("insert into btk.Phonebook(Phonebookcol_phone) values('" + textBox_tel1.Text + "');");
                    id_phon1 = macros.sql_command2("SELECT MAX(idPhonebook) FROM btk.Phonebook;");
                }
                else
                { id_phon1 = "1"; }
            }
            else
            {
                //Создаем имел, в ответ получаем айди созданой записи. Если пусто то присваиваем айди №1 = пусто.

                if (textBox_tel1.Text != "")
                {
                    macros.sql_command("update btk.Phonebook set Phonebookcol_phone='" + textBox_tel1.Text + "' where idPhonebook=" + id_phon1 + ";");
                }
                else
                {
                    id_phon1 = "1";
                }
            }

            if (id_phon2 == "1")//если редактируем пустіе данніе
            {
                //Создаем имел, в ответ получаем айди созданой записи. Если пусто то присваиваем айди №1 = пусто.
                if (textBox_tel2.Text != "")//если поле не пустое
                {
                    macros.sql_command("insert into btk.Phonebook(Phonebookcol_phone) values('" + textBox_tel2.Text + "');");
                    id_phon2 = macros.sql_command2("SELECT MAX(idPhonebook) FROM btk.Phonebook;");
                }
                else
                { id_phon2 = "1"; }
            }
            else
            {
                //Создаем имел, в ответ получаем айди созданой записи. Если пусто то присваиваем айди №1 = пусто.

                if (textBox_tel2.Text != "")
                {
                    macros.sql_command("update btk.Phonebook set Phonebookcol_phone='" + textBox_tel2.Text + "' where idPhonebook=" + id_phon2 + ";");
                }
                else
                {
                    id_phon2 = "1";
                }
            }

            macros.sql_command("update btk.Kontakti set "
                + "Kontakti_imya='" + textBox_name.Text + "',"
                + "Kontakti_familia='" + textBox_familia.Text + "',"
                + "Kontakti_comment='" + textBox_coment.Text + "',"
                + "Kontakti_user_creator='" + vars_form.user_login_id + "',"
                + "Kontakti_edit_datetime='" + Convert.ToDateTime(DateTime.Now).ToString("yyyy -MM-dd HH:mm:ss") + "',"
                + "Emails_idEmails='" + id_mail1 + "',"
                + "Phonebook_idPhonebook='" + id_phon1 + "',"
                + "Emails_idEmails1='" + id_mail2 + "',"
                + "Phonebook_idPhonebook1='" + id_phon2 + "' where idKontakti=" + vars_form.btk_idkontragents + ";");
            macros.sql_command("update btk.Kontakti_has_Kontragenti set "
                               + "Kontragenti_idKontragenti='" +comboBox_work_in.SelectedValue+ "' where Kontakti_idKontakti='"+vars_form.btk_idkontragents+"';");

            this.Close();
        }

        private void comboBox_work_in_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("select Kontragenti.idKontragenti, concat(btk.Kontragenti.Kontragenti_short_name, '(',btk.Kontragenti.Kontragenti_full_name, ')') AS full_short  FROM btk.Kontragenti where Kontragenticol_kategory='Диллер/СТО'");
            comboBox_work_in.DisplayMember = "full_short";
            comboBox_work_in.ValueMember = "idKontragenti";
            DataSet temp = macros.sql_request_dataSet(sql);
            comboBox_work_in.DataSource = temp.Tables[0];
        }

        private void comboBox_work_in_TextUpdate(object sender, EventArgs e)
        {
            string from_textbox = comboBox_work_in.Text.ToString();
            string sql = string.Format("select Kontragenti.idKontragenti, concat(btk.Kontragenti.Kontragenti_short_name, '(',btk.Kontragenti.Kontragenti_full_name, ')') AS full_short  FROM btk.Kontragenti where Kontragenticol_kategory = 'Диллер/СТО' and (Kontragenti_short_name like '%" + comboBox_work_in.Text + "%' or Kontragenti_full_name like '%" + comboBox_work_in.Text + "%')");
            DataSet temp = macros.sql_request_dataSet(sql);
            comboBox_work_in.DataSource = temp.Tables[0];
            comboBox_work_in.DisplayMember = "full_short";
            comboBox_work_in.ValueMember = "idKontragenti";
            comboBox_work_in.Text = from_textbox;
            comboBox_work_in.SelectionStart = comboBox_work_in.Text.Length;
            comboBox_work_in.DroppedDown = true;
        }
    }
}
