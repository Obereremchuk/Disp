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
    public partial class Edit_kontragents : Form
    {
        Macros macros = new Macros();
        string id_phon1 = "";
        string id_phon2 = "";
        string id_mail1 = "";
        string id_mail2 = "";
        
        public Edit_kontragents()
        {
            InitializeComponent();
            get_kontragents();

        }

        public void get_kontragents()
        {
            MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True");
            string sql = string.Format("SELECT * FROM btk.Kontragenti where idKontragenti="+vars_form.btk_idkontragents+";");
            MySqlCommand myDataAdapter = new MySqlCommand(sql, myConnection);
            myConnection.Open();
            MySqlDataReader reader = myDataAdapter.ExecuteReader();

            

            while (reader.Read())
            {
               
                textBox_full_name.Text = reader["Kontragenti_full_name"].ToString();
                textBox_short_name.Text= reader["Kontragenti_short_name"].ToString();
                comboBox_type_vlastnosti.SelectedIndex = comboBox_type_vlastnosti.FindStringExact(reader["Kontragenti_type_vlastnosti"].ToString());
                textBox_rekvizity.Text= reader["Kontragenti_rekvezity"].ToString();
                comboBox_kategory.SelectedIndex = comboBox_kategory.FindStringExact(reader["Kontragenticol_kategory"].ToString());
                comboBox_misto.SelectedIndex = comboBox_misto.FindStringExact(reader["Kontragenticol_misto"].ToString());
                textBox_vulitsa.Text= reader["Kontragenticol_vulitsa"].ToString();
                id_phon1= reader["Phonebook_idPhonebook"].ToString();
                id_phon2 = reader["Phonebook_idPhonebook1"].ToString();
                id_mail1 = reader["Emails_idEmails"].ToString();
                id_mail2 = reader["Emails_idEmails1"].ToString();
                textBox_comment.Text = reader["Kontragenti_comment"].ToString();
            }
            reader.Close();
            myDataAdapter.Dispose();
            myConnection.Close();

            textBox_phone1.Text= macros.sql_command2("SELECT Phonebookcol_phone FROM btk.Phonebook where idPhonebook="+id_phon1+"; ");
            textBox_phone2.Text = macros.sql_command2("SELECT Phonebookcol_phone FROM btk.Phonebook where idPhonebook=" + id_phon2 + "; ");
            textBox_mail1.Text = macros.sql_command2("SELECT Emailscol_email FROM btk.Emails where idEmails=" + id_mail1 + "; ");
            textBox_mail2.Text = macros.sql_command2("SELECT Emailscol_email FROM btk.Emails where idEmails=" + id_mail2 + "; ");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_full_name.Text == "")
            {
                textBox_full_name.BackColor = Color.Red;
                return;
            }

            if (id_mail1 == "1")//если редактируем пустіе данніе
            {
                //Создаем имел, в ответ получаем айди созданой записи. Если пусто то присваиваем айди №1 = пусто.
                if (textBox_mail1.Text != "")//если поле не пустое
                {
                    macros.sql_command("insert into btk.Emails(Emailscol_email) values('" + textBox_mail1.Text + "');");
                    id_mail1 = macros.sql_command2("SELECT MAX(idEmails) FROM btk.Emails;");
                }
                else
                { id_mail1 = "1"; }
            }
            else
            {
                //Создаем имел, в ответ получаем айди созданой записи. Если пусто то присваиваем айди №1 = пусто.
                
                if (textBox_mail1.Text != "")
                {
                    macros.sql_command("update btk.Emails set Emailscol_email='" + textBox_mail1.Text + "' where idEmails="+ id_mail1 + ";");
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
                if (textBox_phone1.Text != "")//если поле не пустое
                {
                    macros.sql_command("insert into btk.Phonebook(Phonebookcol_phone) values('" + textBox_phone1.Text + "');");
                    id_phon1 = macros.sql_command2("SELECT MAX(idPhonebook) FROM btk.Phonebook;");
                }
                else
                { id_phon1 = "1"; }
            }
            else
            {
                //Создаем имел, в ответ получаем айди созданой записи. Если пусто то присваиваем айди №1 = пусто.

                if (textBox_phone1.Text != "")
                {
                    macros.sql_command("update btk.Phonebook set Phonebookcol_phone='" + textBox_phone1.Text + "' where idPhonebook=" + id_phon1 + ";");
                }
                else
                {
                    id_phon1 = "1";
                }
            }

            if (id_phon2 == "1")//если редактируем пустіе данніе
            {
                //Создаем имел, в ответ получаем айди созданой записи. Если пусто то присваиваем айди №1 = пусто.
                if (textBox_phone2.Text != "")//если поле не пустое
                {
                    macros.sql_command("insert into btk.Phonebook(Phonebookcol_phone) values('" + textBox_phone2.Text + "');");
                    id_phon2 = macros.sql_command2("SELECT MAX(idPhonebook) FROM btk.Phonebook;");
                }
                else
                { id_phon2 = "1"; }
            }
            else
            {
                //Создаем имел, в ответ получаем айди созданой записи. Если пусто то присваиваем айди №1 = пусто.

                if (textBox_phone2.Text != "")
                {
                    macros.sql_command("update btk.Phonebook set Phonebookcol_phone='" + textBox_phone2.Text + "' where idPhonebook=" + id_phon2 + ";");
                }
                else
                {
                    id_phon2 = "1";
                }
            }

            macros.sql_command("update btk.Kontragenti set "
                + "Kontragenti_full_name='" + textBox_full_name.Text + "',"
                + "Kontragenti_short_name='"+ textBox_short_name.Text + "',"
                + "Kontragenti_type_vlastnosti='"+ comboBox_type_vlastnosti.SelectedItem + "',"
                + "Kontragenti_rekvezity='"+ textBox_rekvizity.Text + "',"
                + "Kontragenti_user_creator='"+ vars_form.user_login_id + "',"
                + "Kontragenti_edit_datetime='"+ Convert.ToDateTime(DateTime.Now).ToString("yyyy -MM-dd HH:mm:ss") + "',"
                + "Kontragenti_comment='"+ textBox_comment.Text + "',"
                + "Kontragenticol_kategory='"+ comboBox_kategory.SelectedItem + "',"
                + "Emails_idEmails='"+ id_mail1 + "',"
                + "Phonebook_idPhonebook='"+ id_phon1 + "',"
                + "Users_idUsers_manager='"+ "1" + "',"
                + "Emails_idEmails1='"+ id_mail2 + "',"
                + "Phonebook_idPhonebook1='"+ id_phon2 + "',"
                + "Kontragenticol_misto='"+ comboBox_misto.SelectedItem + "',"
                + "Kontragenticol_vulitsa='"+ textBox_vulitsa.Text + "' where idKontragenti="+vars_form.btk_idkontragents+";");

            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
