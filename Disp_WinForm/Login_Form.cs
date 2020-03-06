using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Gecko;
using System.Net;
//using System.Threading;

namespace Disp_WinForm
{
    public partial class Login_Form : Form
    {
        Macros macros = new Macros();
        public Login_Form()
        {
            
            InitializeComponent();
            string t = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\Firefox";
            Xpcom.Initialize(t);
            Gecko.CertOverrideService.GetService().ValidityOverride += geckoWebBrowser1_ValidityOverride;
            wialon_login_form();
            vars_form.version = "079";
            label_Version.Text= "v." + vars_form.version;
        }

        private void geckoWebBrowser1_ValidityOverride(object sender, Gecko.Events.CertOverrideEventArgs e)
        {
            e.OverrideResult = Gecko.CertOverride.Mismatch | Gecko.CertOverride.Time | Gecko.CertOverride.Untrusted;
            e.Temporary = true;
            e.Handled = true;
        }

        private void wialon_login_form()
        {
            GeckoPreferences.User["browser.xul.error_pages.enabled"] = true;

            geckoWebBrowser1.Navigate("https://navi.venbest.com.ua/login.html?client_id=disp&access_type=-1&activation_time=0&duration=100000&flags=1&lang=ru");

        }

        private void geckoWebBrowser1_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            string response = geckoWebBrowser1.Url.Query.ToString();
            string username = "";
            string token = "";
            response = response.Replace('&', '=');
            var buf_data = response.Split('=');

                if (buf_data.Length >= 4)
                {
                    if (buf_data[2] == "access_token")
                    {
                        token = buf_data[3].ToString();
                        username = buf_data[5].ToString();
                        vars_form.user_token = token;

                    macros.sql_command("update btk.Users set user_token='" + token + "' where username='" +
                                           username +
                                           "';");


                        DataTable data = new DataTable();
                        data = macros.GetData("Select " +
                                              "idUsers," +
                                              "username," +
                                              "user_mail," +
                                              "user_token," +
                                              "user_font," +
                                              "accsess_lvl " +
                                              "From btk.Users WHERE " +
                                              "username='" + username + "' ;");
                        if (data.Rows.Count == 1)
                        {
                            vars_form.user_login_id = data.Rows[0][0].ToString();
                            vars_form.user_login_name = data.Rows[0][1].ToString();
                            vars_form.user_login_email = data.Rows[0][2].ToString();
                            vars_form.user_token = data.Rows[0][3].ToString();
                            vars_form.setting_font_size = Convert.ToInt32(data.Rows[0][4]);
                            vars_form.user_accsess_lvl = Convert.ToInt32(data.Rows[0][5]);
                    }




                        if (textBox_font.Text == "")
                        {
                            //Font = new Font("Microsoft Sans Serif", vars_form.setting_font_size);
                        }
                        else if (Int16.Parse(textBox_font.Text) >= 8 & Int16.Parse(textBox_font.Text) <= 18)
                        {
                            vars_form.setting_font_size = Int16.Parse(textBox_font.Text);
                            //Font = new Font("Microsoft Sans Serif", vars_form.setting_font_size);

                            macros.sql_command("update btk.Users set user_font='" + vars_form.setting_font_size +
                                               "' where username='" + username + "';");
                        }
                        else
                        {
                            MessageBox.Show("Введіть коректний розмір шрифта");
                            return;
                        }

                       macros.get_eid_from_token();



                        Main_window form = new Main_window();
                        form.Show();
                        this.Hide();
                    }
                }
            
        }
    }
}
