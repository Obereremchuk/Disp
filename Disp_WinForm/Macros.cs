using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web.Util;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Disp_WinForm
{
    public class Macros
    {
        public void log_write(string data)
        {
            string path = Directory.GetCurrentDirectory();
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(path + "\\log.txt", true))
            {
                file.WriteLine("\r\n" + DateTime.Now.ToString() + " win_un: " + Environment.UserName + "\r\n" + data);
            }
        }
        //public static bool chk_conn()
        //{
        //    Conn_error subwindow = new Conn_error();
        //    bool connDB = test_DB_Conn();
        //    while (connDB == false)
        //    {
        //        if (IsFormOpen(typeof(Conn_error)))
        //        {
        //            //Conn_error subwindow = new Conn_error();
        //            subwindow.Show();
        //        }
        //        System.Threading.Thread.Sleep(2000);
        //        connDB = test_DB_Conn();
        //    }
        //    subwindow.Dispose();            

        //    return connDB;
        //}// until connaction 2DB is faild show error dialog 
        //public static bool test_DB_Conn()
        //{
        //    var conn_info = "server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=false; SslMode=none; Convert Zero Datetime = True";
        //    bool isConn = false;
        //    MySqlConnection conn = null;
        //    try
        //    {
        //        conn = new MySqlConnection(conn_info);
        //        conn.Open();
        //        isConn = true;
        //    }
        //    catch (Exception)
        //    {
        //        isConn = false;

        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //    }
        //    return isConn;
        //}// test 2DB connection
        //public static bool IsFormOpen(Type formType)
        //{
        //    foreach (Form form in System.Windows.Forms.Application.OpenForms)
        //        if (formType.Name == form.Name)
        //            return false;
        //    return true;
        //}//check form (by name) is opened?

        public string WialonRequest(string request)
        {
            string json = "";
            try
            {
                
                MyWebRequest myRequest = new MyWebRequest("http://navi.venbest.com.ua/wialon/ajax.html?sid=" + vars_form.eid + request);
                json = myRequest.GetResponse();
                if (json.Contains("error"))
                {
                    var test_out = JsonConvert.DeserializeObject<TestConnection>(json);
                    if (test_out.error == 1)
                    {
                        GetEidFromToken();
                        myRequest = new MyWebRequest("http://navi.venbest.com.ua/wialon/ajax.html?sid=" + vars_form.eid + request);
                        json = myRequest.GetResponse();
                        if (json.Contains("error"))
                        {
                            test_out = JsonConvert.DeserializeObject<TestConnection>(json);
                        }
                        else { test_out.error = 0; }
                    }

                    else if (test_out.error > 1)
                    {

                        vars_form.error = Get_wl_text_error(test_out.error); //Показіваем диалог бокс с ошибкой, преріваем создание
                        if (!System.Windows.Forms.Application.OpenForms.OfType<Conn_error>().Any())
                        {
                            Conn_error error_box = new Conn_error();
                            error_box.Show();
                        }
                        return json;
                    }
                    if (test_out.error == 0)
                    {
                        if (System.Windows.Forms.Application.OpenForms.OfType<Conn_error>().Any())
                        {
                            Conn_error obj = (Conn_error)System.Windows.Forms.Application.OpenForms["Conn_error"];
                            obj.Close();
                            vars_form.error = "";
                            return json;
                        }
                    }
                }
                return json;
            }
            catch (Exception e)
            {
                GetEidFromToken();
                return json;
            }
        }
        //public string WialonRequest22(string request)
        //{


        //    MyWebRequest myRequest = new MyWebRequest("http://navi.venbest.com.ua/wialon/ajax.html?sid=" + vars_form.eid + request);
        //    string json = myRequest.GetResponse();
        //    try
        //    {
        //        var test_out = JsonConvert.DeserializeObject<RootObject>(json);
        //        if (test_out.error == 1)
        //        {
        //            get_eid_from_token();
        //            myRequest = new MyWebRequest("http://navi.venbest.com.ua/wialon/ajax.html?sid=" + vars_form.eid +
        //                                         request);
        //            json = myRequest.GetResponse();
        //            return json;
        //        }
        //        else if (test_out.error >= 2 & test_out.error !=6) //Виалон ругается ошибкой 6 на изменение поля ВИН кузова в пусто, хотя все верно. исключаем єту ошибку
        //        {
        //            string text =
        //                Get_wl_text_error(test_out.error); //Показіваем диалог бокс с ошибкой, преріваем создание
        //            System.Windows.Forms.MessageBox.Show("Wialon Error: " + text);
        //            return json;
        //        }
        //    }
        //    catch
        //    {
        //    }


        //    return json;
        //}
        public string WialonRequest_(string Request)
        {
            string contents="";
            try
            {
                var client = new HttpClient();
                var content = new StringContent(Request, Encoding.UTF8, "application/x-www-form-urlencoded");
                var address = "http://navi.venbest.com.ua/wialon/ajax.html?sid=" + vars_form.eid;
                var result = client.PostAsync(address, content).Result;
                contents = result.Content.ReadAsStringAsync().Result;
                var answer = JsonConvert.DeserializeObject<TestConnection>(contents);
                if (answer.error != 0)
                {
                    log_write("Error " + "WialonRequest(): " + answer.ToString());
                    GetEidFromToken();

                    client = new HttpClient();
                    content = new StringContent(Request, Encoding.UTF8, "application/x-www-form-urlencoded");
                    address = "http://navi.venbest.com.ua/wialon/ajax.html?sid=" + vars_form.eid;
                    result = client.PostAsync(address, content).Result;
                    contents = result.Content.ReadAsStringAsync().Result;
                    answer = JsonConvert.DeserializeObject<TestConnection>(contents);
                    if (answer.error != 0)
                    {
                        return "";
                    }
                }
                return contents;
            }
            catch (Exception e)
            {
                log_write(e.StackTrace.ToString());
                return "";
            }
        }
        public string WialonRequestSimple(string Request)
        {

            var client = new HttpClient();
            var content = new StringContent(Request, Encoding.UTF8, "application/x-www-form-urlencoded");
            var address = "http://navi.venbest.com.ua/wialon/ajax.html?sid=" + vars_form.eid;
            var result = client.PostAsync(address, content).Result;
            string contents = result.Content.ReadAsStringAsync().Result;
            return contents;
        }


        public void GetEidFromToken()
        {
            try
            {
                var client = new HttpClient();
                var text = "svc=token/login&params={\"token\":\"" + vars_form.user_token + "\"}";
                var content = new StringContent(text, Encoding.UTF8, "application/x-www-form-urlencoded");
                var address = "http://navi.venbest.com.ua/wialon/ajax.html";
                var result = client.PostAsync(address, content).Result;
                var contents = result.Content.ReadAsStringAsync().Result;
                var answer = JsonConvert.DeserializeObject<RootObject>(contents);
                if (answer.error >= 1)
                {
                    MessageBox.Show("Необхідна повторна авторизація (" + answer.reason + ")");
                    log_write("Error " + "get_eid_from_token(): " + answer.ToString());
                    Application.Restart();
                    return;
                }
                else 
                {
                    vars_form.eid = answer.eid;
                    vars_form.wl_user_id = answer.user.id;
                    vars_form.wl_user_nm = answer.user.nm;
                    //sql_command("update btk.Users set user_id_wl='" + vars_form.wl_user_id + "' where username='" + vars_form.wl_user_nm + "';");
                }

                log_write("Ok " + "get_eid_from_token(): " + "login as: " + answer.user.nm);
            }
            catch (Exception e)
            {
                log_write(e.Message);
            }
        }

        //public void get_eid_from_token_()
        //{
        //    MyWebRequest myRequest = new MyWebRequest("http://navi.venbest.com.ua/wialon/ajax.html?" + "&svc=token/login&params={\"token\":\"" + vars_form.user_token + "\"}");
            
        //    //loginAs
        //    //MyWebRequest myRequest = new MyWebRequest("http://navi.venbest.com.ua/wialon/ajax.html?" + "&svc=token/login&params={\"token\":\"" + vars_form.user_token + "\",\"operateAs\":\"service\"}");
        //    string json = myRequest.GetResponse();
        //    var m = JsonConvert.DeserializeObject<RootObject>(json);
        //    if (m.error == 1)
        //    {
        //        get_eid_from_token();
        //        myRequest = new MyWebRequest("http://navi.venbest.com.ua/wialon/ajax.html?", "POST", "&svc=token/login&params={\"token\":\"" + vars_form.user_token + "\"}");
        //        json = myRequest.GetResponse();
        //    }
        //    else if (m.error >= 2)
        //    {
        //        string text =
        //            Get_wl_text_error(m.error); //Показіваем диалог бокс с ошибкой, преріваем создание
        //        System.Windows.Forms.MessageBox.Show("Wialon Error: " + text);
        //    }

        //    if (m.error == 8)
        //    {
        //        //List<Form> openForms = new List<Form>();
        //        //foreach (Form f in System.Windows.Forms.Application.OpenForms)
        //        //    openForms.Add(f);
        //        //foreach (Form f in openForms)
        //        //{
        //        //    if (f.Name != "Login_Form")
        //        //    {
        //        //        f.Dispose();
        //        //        Login_Form login_Form = new Login_Form();
        //        //        login_Form.Show();
        //        //    }

        //        //}
        //        return;
        //    }
        //    vars_form.eid = m.eid;
        //    vars_form.wl_user_id = m.user.id;
        //    vars_form.wl_user_nm = m.user.nm;
        //}//Получаем eid из Виалона по токену из БД присвоенному каждому юзеру
        //public bool Check_conn_wl()
        //{
        //    //////////
        //    //ПРоверяем жив ли Єсайди
        //    /////////
        //    string test_in = "&svc=core/search_items&params={\"spec\":{" 
        //                     + "\"itemsType\":\"avl_unit\"," 
        //                     + "\"propName\":\"sys_unique_id|sys_name|rel_customfield_value|rel_profilefield_value\"," 
        //                     + "\"propValueMask\":\"" + "*" + "\", "
        //                     + "\"sortType\":\"sys_name\"," 
        //                     + "\"or_logic\":\"1\"}," 
        //                     + "\"or_logic\":\"1\"," 
        //                     + "\"force\":\"1\"," 
        //                     + "\"flags\":\"16008907\"," 
        //                     + "\"from\":\"0\"," 
        //                     + "\"to\":\"5\"}";
        //    string json = WialonRequest(test_in);
        //    var test_out = JsonConvert.DeserializeObject<RootObject>(json);
        //    if (test_out.error == 0)
        //    {
        //        return true;
        //    }
        //    else if(test_out.error != 0)
        //    {
        //        get_eid_from_token();
        //        json = WialonRequest(test_in);
        //        test_out = JsonConvert.DeserializeObject<RootObject>(json);
        //        if (test_out.error != 0)
        //        {
        //            string text = Get_wl_text_error(test_out.error);//Показіваем диалог бокс с ошибкой, преріваем создание
        //            System.Windows.Forms.MessageBox.Show("Wialon Error: " + text);
        //            return false;
        //        }
        //    }
        //    return false;
        //}
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

        public string wl_core_search_items(string itemsType, string propName, string propValueMask, string sortType, int flags, int from, int to)
        {
            string answer = WialonRequest("&svc=core/search_items&params={" +
                                                     "\"spec\":{" +                                         /* https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/core/search_items */
                                                     "\"itemsType\":\"" + itemsType + "\"," +                /* тип искомых элементов: avl_resource, avl_unit, avl_unit_group, user   */
                                                     "\"propName\":\"" + propName + "\"," +                 /* имя свойства: sys_name, sys_id, sys_unique_id, sys_phone_number, sys_user_creator */
                                                     "\"propValueMask\":\"" + propValueMask + "\", " +      /* Искомое значение. Mогут быть использованы * | , > < = */
                                                     "\"sortType\":\"" + sortType + "\"," +                 /* имя свойства, по которому будет осуществляться сортировка ответа */
                                                     "\"or_logic\":\"1\"}," +                               /* флаг «ИЛИ»-логики для propName-поля */
                                                     "\"force\":\"1\"," +                                   /* 0 - если такой поиск уже запрашивался, то вернет полученный результат, 1 - будет искать заново */
                                                     "\"flags\":\"" + flags + "\"," +                       /* https://sdk.wialon.com/wiki/ru/local/remoteapi1904/apiref/format/format */
                                                     "\"from\":\"" + from + "\"," +                         /* индекс, начиная с которого возвращать элементы результирующего списка (для нового поиска используйте значение 0) */
                                                     "\"to\":\"" + to + "\"}");                             /* индекс последнего возвращаемого элемента (если 0, то вернет все элементы, начиная с указанного в параметре «from») */
            return answer;
        }

        // Создание датчиков объекта, информация по по ссылке: http://sdk.wialon.com/wiki/ru/local/remoteapi1704/apiref/unit/update_sensor
        public string create_sensor_wl(int id_object, string name_sensor, string type_sensor, string unit, string parametr, int position, string type_validation, int id_sensor_for_validation, string table_calculation)
        {
            string answer = WialonRequest("http://navi.venbest.com.ua/wialon/ajax.html?sid=" + vars_form.eid
                                                                                          + "&svc=unit/update_sensor&params={"
                                                                                          + "\"itemId\":\"" + id_object + "\","
                                                                                          + "\"id\":\"0\","
                                                                                          + "\"callMode\":\"create\","
                                                                                          + "\"unlink\":\"1\","
                                                                                          + "\"n\":\"" + name_sensor + "\","
                                                                                          + "\"t\":\"" + type_sensor + "\","
                                                                                          + "\"d\":\"\","
                                                                                          + "\"m\":\"" + unit + "\","
                                                                                          + "\"p\":\"" + parametr + "\","
                                                                                          + "\"f\":\"0\","
                                                                                          + "\"c\":\"{%5c\"act%5c\":true,%5c\"appear_in_popup%5c\":true,%5c\"show_time%5c\":true,%5c\"pos%5c\":" + position + "}\","
                                                                                          + "\"vt\":\"" + type_validation + "\","
                                                                                          + "\"vs\":\"" + id_sensor_for_validation + "\","
                                                                                          + "\"tbl\":[" + table_calculation + "]}");
            return answer;
        }

        // Создание датчиков объекта, информация по по ссылке: http://sdk.wialon.com/wiki/ru/local/remoteapi1704/apiref/unit/update_sensor
        public string create_custom_field_wl(int id_object, string name_field, string value)
        {
            string answer = WialonRequest("http://navi.venbest.com.ua/wialon/ajax.html?sid=" + vars_form.eid
                                                                                                              + "&svc=item/update_custom_field&params={"
                                                                                                              + "\"itemId\":\"" + id_object + "\","
                                                                                                              + "\"id\":\"0\","
                                                                                                              + "\"callMode\":\"create\","
                                                                                                              + "\"n\":\"" + name_field + "\","
                                                                                                              + "\"v\":\"" + value + "\"}");
            return answer;
        }

        // Создание админитсративных полей вдля объекта в Виалоне
        public string create_admin_field_wl(int id_object, string name_field, string value)
        {
            string answer = WialonRequest("http://navi.venbest.com.ua/wialon/ajax.html?sid=" + vars_form.eid
                                                                                                      + "&svc=item/update_admin_field&params={"
                                                                                                      + "\"id\":\"0\","
                                                                                                      + "\"callMode\":\"create\","
                                                                                                      + "\"itemId\":\"" + id_object + "\","
                                                                                                      + "\"n\":\"" + name_field + "\","
                                                                                                      + "\"v\":\"" + value + "\"}");
            return answer;
        }

        // Создание команд для объекта в Виалоне
        public string create_commads_wl(int id_object, string name_command, string command, int acl_flag)
        {
            string answer = WialonRequest("http://navi.venbest.com.ua/wialon/ajax.html?sid=" + vars_form.eid
                                                                                                    + "&svc=unit/update_command_definition&params={"
                                                                                                    + "\"itemId\":\"" + id_object + "\","
                                                                                                    + "\"id\":\"0\","
                                                                                                    + "\"callMode\":\"create\","
                                                                                                    + "\"n\":\"" + name_command + "\","
                                                                                                    + "\"c\":\"custom_msg\","
                                                                                                    + "\"l\":\"tcp\","
                                                                                                    + "\"p\":\"" + command + "\","
                                                                                                    + "\"a\":\"" + acl_flag + "\"}");
            return answer;
        }

        // sql command
        public string sql_command(string sql)
        {
            string answer = "";
            using (MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=true; SslMode=none; Convert Zero Datetime = True ; charset=utf8"))
            {
                myConnection.Open();
                using (MySqlCommand command = new MySqlCommand(sql, myConnection))
                {
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        answer = reader[0].ToString();
                    }
                    return answer;
                }
            }
        }
    

        public string sql_command2(string sql)
        {
            try
            {
               
                


                using (MySqlConnection myConnection = new MySqlConnection("server=10.44.30.32; user id=lozik; password=lozik; database=btk; pooling=true; SslMode=none; Convert Zero Datetime = True; charset=utf8"))
                {
                    MySqlCommand command = new MySqlCommand(sql, myConnection);
                    //command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    myConnection.Open();
                    string answer = command.ExecuteScalar().ToString();
                    //myConnection.Close();
                    return answer;
                }
            }
            catch (Exception)
            {
                string answer = "";
                return answer;

            }

        }

        public DataSet GetData_dataset(string sqlCommand)
        {
            string connectionString = "server = 10.44.30.32; " +
                                      "user id=lozik;" +
                                      "password=lozik;" +
                                      "database=btk;" +
                                      "pooling=true;" +
                                      "SslMode=none;" +
                                      "Convert Zero Datetime = True;" + 
                                      "charset=utf8;";

            using (MySqlConnection northwindConnection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(sqlCommand, northwindConnection);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = command;

                DataSet table = new DataSet();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                adapter.Fill(table);
                //northwindConnection.Close();

                return table;
            }
        }

        public DataTable GetData(string sqlCommand)
        {
            string connectionString = "server = 10.44.30.32; " +
                                      "user id=lozik;" +
                                      "password=lozik;" +
                                      "database=btk;" +
                                      "pooling=true;" +
                                      "SslMode=none;" +
                                      "MaximumPoolsize=3;" +
                                      "ConnectionLifeTime=60;" +
                                      "Convert Zero Datetime = True;" +
                                      "charset=utf8;";

            using (MySqlConnection northwindConnection = new MySqlConnection(connectionString))
            { 
                MySqlCommand command = new MySqlCommand(sqlCommand, northwindConnection);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = command;
                DataTable table = new DataTable();
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                adapter.Fill(table);
                return table;
            }

        }

        public string GetProcessPath(string name)
        {
            Process[] processes = Process.GetProcessesByName(name);

            if (processes.Length > 0)
            {
                return processes[0].MainModule.FileName;
            }
            else
            {
                return string.Empty;
            }
        }

        //other
        public void ExportDataSet(DataSet ds)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";      
            saveFileDialog1.Title = "Save text Files";
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "xlsx";
            saveFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //textBox1.Text = saveFileDialog1.FileName;
                string folderPath = saveFileDialog1.FileName;

                using (var workbook = SpreadsheetDocument.Create(folderPath, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();

                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                    foreach (System.Data.DataTable table in ds.Tables)
                    {

                        var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                        var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                        sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                        DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                        string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                        uint sheetId = 1;
                        if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                        {
                            sheetId =
                                sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                        }

                        DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                        sheets.Append(sheet);

                        DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                        List<String> columns = new List<string>();
                        foreach (System.Data.DataColumn column in table.Columns)
                        {
                            columns.Add(column.ColumnName);

                            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                            headerRow.AppendChild(cell);
                        }


                        sheetData.AppendChild(headerRow);

                        foreach (System.Data.DataRow dsrow in table.Rows)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                            foreach (String col in columns)
                            {
                                DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                                cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                                cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString()); //
                                newRow.AppendChild(cell);
                            }

                            sheetData.AppendChild(newRow);
                        }

                    }
                }
            }
        } //export to xlsx

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public void send_mail(string recipient, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.venbest.com.ua");

                mail.From = new MailAddress("noreply@venbest.com.ua");
                mail.To.Add(recipient);
                mail.Subject = subject;
                

                mail.IsBodyHtml = true;
                mail.Body = body;

                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("noreply@venbest.com.ua", "L2TPr6");
                SmtpServer.EnableSsl = false;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "send_email()");
            }
        }

        public void send_mail_auto(string recipient, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.venbest.com.ua");

                mail.From = new MailAddress("auto@venbest.com.ua");
                mail.To.Add(recipient);
                mail.Subject = subject;


                mail.IsBodyHtml = true;
                mail.Body = body;

                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("auto@venbest.com.ua", "876345");
                SmtpServer.EnableSsl = false;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "send_email()");
            }
        }

        public StringBuilder htmlMessageBody(DataGridView dg)
        {
            StringBuilder strB = new StringBuilder();
            //create html & table
            strB.AppendLine("<html><body><left><" +
                            "table border='1' cellpadding='0' cellspacing='0'>");
            strB.AppendLine("<tr>");
            //cteate table header
            for (int i = 0; i < dg.Columns.Count; i++)
            {
                strB.AppendLine("<td align='center' valign='middle'>" +
                                dg.Columns[i].HeaderText + "</td>");
            }
            //create table body
            strB.AppendLine("<tr>");
            for (int i = 0; i < dg.Rows.Count; i++)
            {
                strB.AppendLine("<tr>");
                foreach (DataGridViewCell dgvc in dg.Rows[i].Cells)
                {
                    strB.AppendLine("<td align='center' valign='middle'>" +
                                    dgvc.Value.ToString() + "</td>");
                }
                strB.AppendLine("</tr>");

            }
            //table footer & end of html file
            strB.AppendLine("</table></center></body></html>");
            return strB;
        } //Готовим таблицу для оправки

        public string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<table>";
            //add header row
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }

        public string LogUserAction(string idUser, string reason, string old_data, string new_data, string create_date_time)
        {
            string answer = sql_command("insert into btk.Logging(Users_idUsers, Loggingcol_reason, Loggingcol_new_data, Loggingcol_old_data, Loggingcol_create_date) value ('" + idUser + "', '" + reason + "', '" + old_data + "', '" + new_data + "', '" + create_date_time + "');");
            return answer;
        }


    }


    public class PlaceHolderTextBox : TextBox
    {
        #region Properties

        string _PlaceHolderText = DEFAULT_PlaceHolder;
        bool _isPlaceHolderActive = true;


        /// <summary>
        /// Gets a value indicating whether the PlaceHolder is active.
        /// </summary>
        [Browsable(false)]
        public bool IsPlaceHolderActive
        {
            get { return _isPlaceHolderActive; }
            private set
            {
                if (_isPlaceHolderActive == value) return;

                // Disable operating system handling for mouse events
                // This prevents the user to select the PlaceHolder with mouse or double clicking
                SetStyle(ControlStyles.UserMouse, value);

                // If text equals the PlaceHolder and Reset is called, the actual text doesn't change but the IsPlaceHolderActive does.
                // Thus the style (Text or PlaceHolder) is not updated.
                // Invalidate forces that
                Invalidate();

                _isPlaceHolderActive = value;
                OnPlaceHolderActiveChanged(value);
            }
        }


        /// <summary>
        /// Gets or sets the PlaceHolder in the PlaceHolderTextBox.
        /// </summary>
        [Description("The PlaceHolder associated with the control."), Category("PlaceHolder"), DefaultValue(DEFAULT_PlaceHolder)]
        public string PlaceHolderText
        {
            get { return _PlaceHolderText; }
            set
            {
                _PlaceHolderText = value;

                // Only use the new value if the PlaceHolder is active.
                if (IsPlaceHolderActive)
                    Text = value;
            }
        }


        /// <summary>
        /// Gets or sets the current text in the TextBox.
        /// </summary>
        [Browsable(false)]
        public override string Text
        {
            get
            {
                // Check 'base.Text == PlaceHolder' because in some cases IsPlaceHolderActive changes too late although it isn't the PlaceHolder anymore.
                if (IsPlaceHolderActive && base.Text == PlaceHolderText)
                    return "";

                return base.Text;
            }
            set { base.Text = value; }
        }



        Color _PlaceHolderTextColor;
        /// <summary>
        /// Gets or sets the foreground color of the control.
        /// </summary>
        [Description("The foreground color of this component, which is used to display the PlaceHolder."), Category("Appearance"), DefaultValue(typeof(Color), "InactiveCaption")]
        public Color PlaceHolderTextColor
        {
            get { return _PlaceHolderTextColor; }
            set
            {
                if (_PlaceHolderTextColor == value) return;
                _PlaceHolderTextColor = value;

                // Force redraw to show new color in designer instantly
                if (DesignMode)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the foreground color of the control.
        /// </summary>
        [Description("The foreground color of this component, which is used to display text."), Category("Appearance"), DefaultValue(typeof(Color), "WindowText")]
        public Color TextColor { get; set; }

        /// <summary>
        /// Do not access directly. Use TextColor.
        /// </summary>
        [Browsable(false)]
        public override Color ForeColor
        {
            get
            {
                if (IsPlaceHolderActive)
                    return PlaceHolderTextColor;

                return TextColor;
            }
            set { TextColor = value; }
        }

        /// <summary>
        /// Gets the length of text in the control.
        /// </summary>
        public override int TextLength => IsPlaceHolderActive ? 0 : base.TextLength;

        /// <summary>
        /// Occurs when the value of the IsPlaceHolderActive property has changed.
        /// </summary>
        [Description("Occurs when the value of the IsPlaceHolderInside property has changed.")]
        public event EventHandler<PlaceHolderActiveChangedEventArgs> PlaceHolderActiveChanged;

        #endregion


        #region Global Variables

        /// <summary>
        /// Specifies the default PlaceHolder text.
        /// </summary>
        const string DEFAULT_PlaceHolder = "<Input>";

        // Doc: https://msdn.microsoft.com/en-us/library/windows/desktop/bb761661(v=vs.85).aspx
        const int EM_SETSEL = 0x00B1;

        /// <summary>
        /// Flag to avoid the TextChanged Event. Don't access directly, use Method 'ActionWithoutTextChanged(Action act)' instead.
        /// </summary>
        bool avoidTextChanged;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the PlaceHolderTextBox class.
        /// </summary>
        public PlaceHolderTextBox()
        {
            // Through this line the default PlaceHolder gets displayed in designer
            base.Text = PlaceHolderText;
            TextColor = SystemColors.WindowText;
            PlaceHolderTextColor = SystemColors.InactiveCaption;

            SetStyle(ControlStyles.UserMouse, true);
        }

        #endregion


        #region Functions

        /// <summary>
        /// Inserts PlaceHolder, assigns PlaceHolder style and sets cursor to first position.
        /// </summary>
        public void Reset()
        {
            if (IsPlaceHolderActive) return;

            IsPlaceHolderActive = true;

            Text = PlaceHolderText;
            Select(0, 0);
        }

        /// <summary>
        /// Run an action with avoiding the TextChanged event.
        /// </summary>
        /// <param name="act">Specifies the action to run.</param>
        private void ActionWithoutTextChanged(Action act)
        {
            avoidTextChanged = true;

            act.Invoke();

            avoidTextChanged = false;
        }

        private void UpdateText()
        {
            // Run code with avoiding recursive call
            ActionWithoutTextChanged(delegate
            {
                // If the Text is empty, insert PlaceHolder and set cursor to the first position
                if (!IsPlaceHolderActive && String.IsNullOrEmpty(Text))
                {
                    // Allow default length for the PlaceHolder
                    // If we wouldn't do this, the PlaceHolder will never disappear if
                    // PlaceHolderText.Length > MaxLength because the user cannot type anything
                    MaxLength = 32767;
                    Reset();
                }
                // If the PlaceHolder has been active but now the text changed,
                // set the textbox to its usual state
                else if (IsPlaceHolderActive && Text.Length > 0)
                {
                    MaxLength = customMaxLength;

                    IsPlaceHolderActive = false;

                    // If you set Text programmatically it won't contain the PlaceHolderText.
                    // Thus we do not have to remove it
                    // An issue is you cannot set a Text programmatically which has the structure [prefix][PlaceHolder]
                    // Note the missing suffix because we use "EndsWith"
                    // Well, you can but the PlaceHolder becomes removed, the prefix will be in the textbox
                    // The reason is we cannot distinguish if a user typed something or the Text has been set programmatically
                    if (Text.EndsWith(PlaceHolderText))
                    {
                        Text = Text.Substring(0, TextLength - PlaceHolderText.Length);
                    }

                    // If we copied something, trim it to the MaxLength
                    if (Text.Length > MaxLength)
                        Text = Text.Substring(0, MaxLength);

                    // Set selection to last position
                    Select(TextLength, 0);
                }
            });
        }

        #endregion


        #region Events

        int customMaxLength;
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            // Save the user specified MaxLength
            customMaxLength = MaxLength;
            // Set to default for the PlaceHolder
            MaxLength = 32767;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            // Check flag
            if (avoidTextChanged) return;

            UpdateText();

            base.OnTextChanged(e);
        }

        protected override void WndProc(ref Message m)
        {
            // Prevent selection through Windows default controls. ("Select all" in context menu)
            if (IsPlaceHolderActive && m.Msg == EM_SETSEL) return;
            base.WndProc(ref m);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (IsPlaceHolderActive)
            {
                // Prevents navigating through the PlaceHolder with navigation keys and PlaceHolder is not deletable with delete key
                if (e.KeyCode == Keys.Left ||
                    e.KeyCode == Keys.Right ||
                    e.KeyCode == Keys.Up ||
                    e.KeyCode == Keys.Down ||
                    e.KeyCode == Keys.Delete ||
                    e.KeyCode == Keys.Home ||
                    e.KeyCode == Keys.End ||
                    e.KeyCode == Keys.Back)
                {
                    e.SuppressKeyPress = true;
                }

                // Prevent selecting the PlaceHolder text
                if (e.KeyCode == Keys.A && e.Modifiers.HasFlag(Keys.Control))
                {
                    e.SuppressKeyPress = true;
                }
            }

            base.OnKeyDown(e);
        }

        protected virtual void OnPlaceHolderActiveChanged(bool newValue)
        {
            PlaceHolderActiveChanged?.Invoke(this, new PlaceHolderActiveChangedEventArgs(newValue));
        }

        #endregion


        #region Avoid full text selection after first display with TabIndex = 0

        // Base class has a selectionSet property, but it is private.
        // We need to shadow with our own variable. If true, this means
        // "don't mess with the selection, the user did it."
        bool selectionSet;

        protected override void OnGotFocus(EventArgs e)
        {
            bool needToDeselect = false;

            // We don't want to avoid calling the base implementation
            // completely. We mirror the logic that we are trying to avoid;
            // if the base implementation will select all of the text, we
            // set a boolean.
            if (!selectionSet)
            {
                selectionSet = true;

                if (SelectionLength == 0 && MouseButtons == MouseButtons.None)
                {
                    needToDeselect = true;
                }
            }

            // Call the base implementation
            base.OnGotFocus(e);

            // Did we notice that the text was selected automatically? Let's
            // de-select it and put the caret at the end.
            if (!needToDeselect) return;

            SelectionStart = 0;
            DeselectAll();
        }

        #endregion
    }

    /// <summary>
    /// Provides data for the PlaceHolderActiveChanged event.
    /// </summary>
    public class PlaceHolderActiveChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the PlaceHolderActiveChangedEventArgs class.
        /// </summary>
        /// <param name="isActive">Specifies whether the PlaceHolder is currently active.</param>
        public PlaceHolderActiveChangedEventArgs(bool isActive)
        {
            IsActive = isActive;
        }

        /// <summary>
        /// Gets the new value of the IsPlaceHolderActive property.
        /// </summary>
        public bool IsActive { get; private set; }
    }


    //public class PlaceHolderTextBox : TextBox
    //{

    //    bool isPlaceHolder = true;
    //    string _PlaceHolderText;
    //    public string PlaceHolderText
    //    {
    //        get { return _PlaceHolderText; }
    //        set
    //        {
    //            _PlaceHolderText = value;
    //            setPlaceHolder();
    //        }
    //    }

    //    public new string Text
    //    {
    //        get => isPlaceHolder ? string.Empty : base.Text;
    //        set => base.Text = value;
    //    }

    //    //when the control loses focus, the PlaceHolder is shown
    //    private void setPlaceHolder()
    //    {
    //        if (string.IsNullOrEmpty(base.Text))
    //        {
    //            base.Text = PlaceHolderText;
    //            this.ForeColor = Color.Gray;
    //            this.Font = new Font(this.Font, FontStyle.Italic);
    //            isPlaceHolder = true;
    //        }
    //    }

    //    //when the control is focused, the PlaceHolder is removed
    //    private void removePlaceHolder()
    //    {

    //        if (isPlaceHolder)
    //        {
    //            base.Text = "";
    //            this.ForeColor = System.Drawing.SystemColors.WindowText;
    //            this.Font = new Font(this.Font, FontStyle.Regular);
    //            isPlaceHolder = false;
    //        }
    //    }
    //    public PlaceHolderTextBox()
    //    {
    //        GotFocus += removePlaceHolder;
    //        LostFocus += setPlaceHolder;
    //    }

    //    private void setPlaceHolder(object sender, EventArgs e)
    //    {
    //        setPlaceHolder();
    //    }

    //    private void removePlaceHolder(object sender, EventArgs e)
    //    {
    //        removePlaceHolder();
    //    }
    //}

}

