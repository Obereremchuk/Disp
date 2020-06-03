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
    public partial class Add_kontragent : Form
    {
        Macros macros = new Macros();
        public Add_kontragent()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            // Строим 
            this.comboBox_kategory.DataSource = macros.GetData("SELECT * FROM btk.kontragent_type;");
            this.comboBox_kategory.DisplayMember = "type";
            this.comboBox_kategory.ValueMember = "idkontragent_type";

        }

        private void button_add_kontragets_Click(object sender, EventArgs e)
        {
            if (comboBox_kategory.SelectedIndex == -1)
            {
                comboBox_kategory.BackColor = Color.Red;
                return;
            }

            if (textBox_full_name.Text == "")
            {
                textBox_full_name.BackColor = Color.Red;
                return;
            }



            //Создаем имел, в ответ получаем айди созданой записи. Если пусто то присваиваем айди №1 = пусто.
            string emailID1 = "";
            if (textBox_mail1.Text != "")
            {
                macros.sql_command("insert into btk.Emails(Emailscol_email) values('" + textBox_mail1.Text + "');");
                emailID1 = macros.sql_command("SELECT MAX(idEmails) FROM btk.Emails;");
            }
            else
            { emailID1 = "1"; }

            //Создаем имел2, в ответ получаем айди созданой записи. Если пусто то присваиваем айди №1 = пусто.
            string emailID2 = "";
            if (textBox_mail2.Text != "")
            {
                macros.sql_command("insert into btk.Emails(Emailscol_email) values('" + textBox_mail2.Text + "');");
                emailID2 = macros.sql_command("SELECT MAX(idEmails) FROM btk.Emails;");
            }
            else
            { emailID2 = "1"; }

            //Создаем тел1, в ответ получаем айди созданой записи. Если пусто то присваиваем айди №1 = пусто.
            string phoneID1 = "";
            if (maskedTextBox_phone1.Text != "")
            {
                macros.sql_command("insert into btk.Phonebook(Phonebookcol_phone) values('" + maskedTextBox_phone1.Text + "');");
                phoneID1 = macros.sql_command("SELECT MAX(idPhonebook) FROM btk.Phonebook;");
            }
            else
            { phoneID1 = "1"; }

            //Создаем тел2, в ответ получаем айди созданой записи. Если пусто то присваиваем айди №1 = пусто.
            string phoneID2 = "";
            if (maskedTextBox_phone2.Text != "")
            {
                macros.sql_command("insert into btk.Phonebook(Phonebookcol_phone) values('" + maskedTextBox_phone2.Text + "');");
                phoneID2 = macros.sql_command("SELECT MAX(idPhonebook) FROM btk.Phonebook;");
            }
            else
            { phoneID2 = "1"; }

            macros.sql_command("insert into btk.Kontragenti (Kontragenti_full_name, Kontragenti_short_name, Kontragenti_type_vlastnosti, Kontragenti_rekvezity, Kontragenti_user_creator, Kontragenti_create_datetime, Kontragenti_edit_datetime, Kontragenti_comment, Kontragenticol_kategory, Emails_idEmails, Phonebook_idPhonebook, Users_idUsers_manager, Emails_idEmails1, Phonebook_idPhonebook1, Kontragenticol_misto, Kontragenticol_vulitsa, kontragent_type_idkontragent_type ) values('"
                + textBox_full_name.Text+"','"
                +textBox_short_name.Text+"','"
                +comboBox_type_vlastnosti.SelectedItem + "','"
                +textBox_rekvizity.Text+"','"
                +vars_form.user_login_id+"','"
                +Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss")+"','"
                +Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss")+"','"
                +textBox_comment.Text+"','"
                +comboBox_kategory.GetItemText(comboBox_kategory.SelectedItem)+"','"
                +emailID1+"','"
                +phoneID1+"','"
                +"1"+"','"
                +emailID2+"','"
                +phoneID2+"','"
                +comboBox_misto.SelectedItem+"','"
                + comboBox_kategory.SelectedValue + "','"
                + textBox_vulitsa.Text+"');");

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox_kategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_kategory.BackColor = Color.Empty;
        }
    }
}
