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
using System.Collections.Specialized;
using System.Web;

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
            vars_form.version = "0.845";
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
            if (response.Contains("svc_error"))
            {
                NameValueCollection qscoll = HttpUtility.ParseQueryString(response);
                if (qscoll["svc_error"] == "0")
                {
                    vars_form.user_token = qscoll["access_token"];
                    macros.sql_command("update btk.Users set user_token='" + vars_form.user_token + "' where username='" + qscoll["user_name"] + "';");
                    DataTable data = macros.GetData("Select " +
                                            "idUsers," +
                                            "username," +
                                            "user_mail," +
                                            "user_token," +
                                            "user_font," +
                                            "accsess_lvl " +
                                            "From btk.Users WHERE " +
                                            "username='" + qscoll["user_name"] + "' ;");
                    if (data.Rows.Count == 1)
                    {
                        vars_form.user_login_id = data.Rows[0][0].ToString();
                        vars_form.user_login_name = data.Rows[0][1].ToString();
                        vars_form.user_login_email = data.Rows[0][2].ToString();
                        vars_form.setting_font_size = Convert.ToInt32(data.Rows[0][4]);
                        vars_form.user_accsess_lvl = Convert.ToInt32(data.Rows[0][5]);

                        if (textBox_font.Text == "")
                        {
                            //Font = new Font("Microsoft Sans Serif", vars_form.setting_font_size);
                        }
                        else if (Int16.Parse(textBox_font.Text) >= 8 & Int16.Parse(textBox_font.Text) <= 18)
                        {
                            vars_form.setting_font_size = Int16.Parse(textBox_font.Text);
                            //Font = new Font("Microsoft Sans Serif", vars_form.setting_font_size);

                            macros.sql_command("update btk.Users set user_font='" + vars_form.setting_font_size +
                                                "' where username='" + qscoll["user_name"] + "';");
                        }
                        else
                        {
                            MessageBox.Show("Введіть коректний розмір шрифта");
                            return;
                        }

                        //macros.GetEidFromToken();

                        Main_window form = new Main_window();
                        form.Show();
                        this.Hide();
                    }
                }
            }
        }
    }
}
