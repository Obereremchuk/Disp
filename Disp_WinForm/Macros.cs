using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using FontStyle = System.Drawing.FontStyle;

namespace Disp_WinForm
{
    public class Macros
    {
        public static bool chk_conn()
        {
            Conn_error subwindow = new Conn_error();
            bool connDB = test_DB_Conn();
            while (connDB == false)
            {
                if (IsFormOpen(typeof(Conn_error)))
                {
                    //Conn_error subwindow = new Conn_error();
                    subwindow.Show();
                }
                System.Threading.Thread.Sleep(2000);
                connDB = test_DB_Conn();
            }
            subwindow.Dispose();            

            return connDB;
        }// until connaction 2DB is faild show error dialog 
        public static bool test_DB_Conn()
        {
            var conn_info = "server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True";
            bool isConn = false;
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(conn_info);
                conn.Open();
                isConn = true;
            }
            catch (Exception ex)
            {
                isConn = false;

            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return isConn;
        }// test 2DB connection
        public static bool IsFormOpen(Type formType)
        {
            foreach (Form form in System.Windows.Forms.Application.OpenForms)
                if (formType.Name == form.Name)
                    return false;
            return true;
        }//check form (by name) is opened?
        public object sql_request_dataTable(string sql)
        {
            try
            {
                MySqlConnection myConnection = new MySqlConnection(
                    "server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(sql, myConnection);
                DataTable data = new DataTable();

                myDataAdapter.Fill(data);

                myDataAdapter.Dispose();
                myConnection.Close();

                return data;
            }
            catch (Exception)
            {
                DataTable data = new DataTable();
                return data;
            }


        }//Request 2DB - dataTable
        public void sql_command(string sql)
        {
            try
            {
                MySqlConnection myConnection = new MySqlConnection(
                    "server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True ; charset=utf8");
                myConnection.Open();
                MySqlCommand command = myConnection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();
                myConnection.Close();
            }   
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message.ToString() + "");
            }
        }
        public DataSet sql_request_dataSet(string sql)
        {
            try
            {
                MySqlConnection myConnection = new MySqlConnection(
                    "server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(sql, myConnection);
                DataSet data = new DataSet();

                myDataAdapter.Fill(data);

                myDataAdapter.Dispose();
                myConnection.Close();

                return data;
            }
            catch (Exception)
            {
                DataSet data = new DataSet();
                return data;
            }
        }//Request 2DB - dataSet
        public DataTable sql_request_dataTable_new(string sql)
        {
            try
            {
                MySqlConnection myConnection = new MySqlConnection(
                    "server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(sql, myConnection);
                DataTable data = new DataTable();

                myDataAdapter.Fill(data);

                myDataAdapter.Dispose();
                myConnection.Close();

                return data;
            }
            catch (Exception)
            {
                DataTable data = new DataTable();
                return data;
            }
        }//Request 2DB - dataTable
        public string sql_command2(string sql)
        {
            try
            {
                MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True; charset=utf8");
                MySqlCommand command = new MySqlCommand(sql, myConnection);
                command.CommandType = CommandType.Text;
                myConnection.Open();
                string answer = command.ExecuteScalar().ToString();
                myConnection.Close();
                return answer;
            }
            catch (Exception)
            {
                string answer = "";
                return answer;

            }
            
        }

        public string wialon_request_new(string request)
        {
            MyWebRequest myRequest = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?sid=" + vars_form.eid + request);
            string json = myRequest.GetResponse();
            var test_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (test_out.error != 0)
            {
                get_eid_from_token();
                json = myRequest.GetResponse();
                test_out = JsonConvert.DeserializeObject<RootObject>(json);
                if (test_out.error != 0)
                {
                    string text = Get_wl_text_error(test_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                    System.Windows.Forms.MessageBox.Show("Wialon Error: " + text);
                    return json;
                }
            }
            return json;
        }
        public string wialon_request_lite(string request)
        {
            MyWebRequest myRequest = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?sid=" + vars_form.eid + request);
            string json = myRequest.GetResponse();
            try
            {
                var test_out = JsonConvert.DeserializeObject<RootObject>(json);
                if (test_out.error == 1)
                {
                    get_eid_from_token();
                    myRequest = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?sid=" + vars_form.eid +
                                                 request);
                    json = myRequest.GetResponse();
                    return json;
                }
                else if (test_out.error >= 2 & test_out.error !=6) //Виалон ругается ошибкой 6 на изменение поля ВИН кузова в пусто, хотя все верно. исключаем єту ошибку
                {
                    string text =
                        Get_wl_text_error(test_out.error); //Показіваем диалог бокс с ошибкой, преріваем создание
                    System.Windows.Forms.MessageBox.Show("Wialon Error: " + text);
                    return json;
                }
            }
            catch
            {
            }
           

            return json;
        }
        public void get_eid_from_token()
        {
            MyWebRequest myRequest = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "&svc=token/login&params={\"token\":\"" + vars_form.user_token + "\"}");
            string json = myRequest.GetResponse();
            var m = JsonConvert.DeserializeObject<RootObject>(json);
            if (m.error == 1)
            {
                get_eid_from_token();
                myRequest = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "&svc=token/login&params={\"token\":\"" + vars_form.user_token + "\"}");
                json = myRequest.GetResponse();
            }
            else if (m.error >= 2)
            {
                string text =
                    Get_wl_text_error(m.error); //Показіваем диалог бокс с ошибкой, преріваем создание
                System.Windows.Forms.MessageBox.Show("Wialon Error: " + text);
            }
            vars_form.eid = m.eid;
            vars_form.wl_user_id = m.user.id;
            vars_form.wl_user_nm = m.user.nm;
        }//Получаем eid из Виалона по токену из БД присвоенному каждому юзеру
        public bool Check_conn_wl()
        {
            //////////
            //ПРоверяем жив ли Єсайди
            /////////
            string test_in = "&svc=core/search_items&params={\"spec\":{" 
                             + "\"itemsType\":\"avl_unit\"," 
                             + "\"propName\":\"sys_unique_id|sys_name|rel_customfield_value|rel_profilefield_value\"," 
                             + "\"propValueMask\":\"" + "*" + "\", "
                             + "\"sortType\":\"sys_name\"," 
                             + "\"or_logic\":\"1\"}," 
                             + "\"or_logic\":\"1\"," 
                             + "\"force\":\"1\"," 
                             + "\"flags\":\"16008907\"," 
                             + "\"from\":\"0\"," 
                             + "\"to\":\"5\"}";
            string json = wialon_request_new(test_in);
            var test_out = JsonConvert.DeserializeObject<RootObject>(json);
            if (test_out.error == 0)
            {
                return true;
            }
            else if(test_out.error != 0)
            {
                get_eid_from_token();
                json = wialon_request_new(test_in);
                test_out = JsonConvert.DeserializeObject<RootObject>(json);
                if (test_out.error != 0)
                {
                    string text = Get_wl_text_error(test_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
                    System.Windows.Forms.MessageBox.Show("Wialon Error: " + text);
                    return false;
                }
            }
            return false;
        }
        public string Get_wl_text_error(int wl_code_error)
        {
            string Get_wl_text_error = "";
            if (wl_code_error == 0)
            {
                Get_wl_text_error = "";
                return Get_wl_text_error;
            }
            else if (wl_code_error == 1)
            {
                Get_wl_text_error = "Недействительная сессия";
                return Get_wl_text_error;
            }
            else if (wl_code_error == 2)
            {
                Get_wl_text_error = "Неверное имя сервиса";
                return Get_wl_text_error;
            }
            else if (wl_code_error == 3)
            {
                Get_wl_text_error = "Неверный результат";
                return Get_wl_text_error;
            }
            else if (wl_code_error == 4)
            {
                Get_wl_text_error = "Неверный ввод";
                return Get_wl_text_error;
            }
            else if (wl_code_error == 5)
            {
                Get_wl_text_error = "Ошибка выполнения запроса";
                return Get_wl_text_error;
            }
            else if (wl_code_error == 6)
            {
                Get_wl_text_error = "Неизвестная ошибка";
                return Get_wl_text_error;
            }
            else if (wl_code_error == 7)
            {
                Get_wl_text_error = "	Доступ запрещен";
                return Get_wl_text_error;
            }
            else if (wl_code_error == 8)
            {
                Get_wl_text_error = "Неверный пароль или имя пользователя";
                return Get_wl_text_error;
            }
            else if (wl_code_error == 9)
            {
                Get_wl_text_error = "Сервер авторизации недоступен";
                return Get_wl_text_error;
            }
            else if (wl_code_error == 1002)
            {
                Get_wl_text_error = "Элемент с таким уникальным свойством уже существует";
                return Get_wl_text_error;
            }

            System.Windows.Forms.MessageBox.Show(
                    "",
                    "Ошибка Wialon: " + Get_wl_text_error,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);

            return Get_wl_text_error;
        }
        
        public DataTable GetData(string sqlCommand)
        {
            string connectionString = "server = 10.44.30.32; " +
                                      "user id=lozik;" +
                                      "password=lozik;" +
                                      "database=btk;" +
                                      "pooling=false;" +
                                      "SslMode=none;" +
                                      "Convert Zero Datetime = True;" + 
                                      "charset=utf8;";

            MySqlConnection northwindConnection = new MySqlConnection(connectionString);

            MySqlCommand command = new MySqlCommand(sqlCommand, northwindConnection);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = command;

            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;
            adapter.Fill(table);

            return table;
        }

    }
    public class PlaceHolderTextBox : TextBox
    {

        bool isPlaceHolder = true;
        string _placeHolderText;
        public string PlaceHolderText
        {
            get { return _placeHolderText; }
            set
            {
                _placeHolderText = value;
                setPlaceholder();
            }
        }

        public new string Text
        {
            get => isPlaceHolder ? string.Empty : base.Text;
            set => base.Text = value;
        }

        //when the control loses focus, the placeholder is shown
        private void setPlaceholder()
        {
            if (string.IsNullOrEmpty(base.Text))
            {
                base.Text = PlaceHolderText;
                this.ForeColor = Color.Gray;
                this.Font = new Font(this.Font, FontStyle.Italic);
                isPlaceHolder = true;
            }
        }

        //when the control is focused, the placeholder is removed
        private void removePlaceHolder()
        {

            if (isPlaceHolder)
            {
                base.Text = "";
                this.ForeColor = System.Drawing.SystemColors.WindowText;
                this.Font = new Font(this.Font, FontStyle.Regular);
                isPlaceHolder = false;
            }
        }
        public PlaceHolderTextBox()
        {
            GotFocus += removePlaceHolder;
            LostFocus += setPlaceholder;
        }

        private void setPlaceholder(object sender, EventArgs e)
        {
            setPlaceholder();
        }

        private void removePlaceHolder(object sender, EventArgs e)
        {
            removePlaceHolder();
        }
    }
}
