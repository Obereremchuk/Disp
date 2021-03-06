﻿using Newtonsoft.Json;
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
    public partial class Add_alarm : Form
    {
        public Add_alarm()
        {
            InitializeComponent();
        }

        private void update_session()
        {
            try
            {
                MyWebRequest myRequest = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "&svc=token/login&params={\"token\":\"d1207c47958c32b224682b5b080fb908CAF3A4507360712CD18AE69617A54F2FC61EFF5D\"}");
                string json = myRequest.GetResponse();
                var m = JsonConvert.DeserializeObject<RootObject>(json);
                vars_form.eid = m.eid;
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void textBox_filter_object_ohrani_TextChanged(object sender, EventArgs e)
        {
            try
            {
                MyWebRequest myRequest = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + vars_form.eid + "&svc=core/search_items&params={\"spec\":{" + "\"itemsType\":\"avl_unit\"," + "\"propName\":\"sys_unique_id|sys_name|rel_customfield_value|rel_profilefield_value\"," + "\"propValueMask\":\"" + "*" + textBox_filter_object_ohrani.Text + "*" + "\", " + "\"sortType\":\"sys_name\"," + "\"or_logic\":\"1\"}," + "\"or_logic\":\"1\"," + "\"force\":\"1\"," + "\"flags\":\"257\"," + "\"from\":\"0\"," + "\"to\":\"5\"}");//16008907
                string json = myRequest.GetResponse();
                var m = JsonConvert.DeserializeObject<RootObject>(json);
                if (m.error == 1)
                {
                    update_session();
                    myRequest = new MyWebRequest("https://navi.venbest.com.ua/wialon/ajax.html?", "POST", "sid=" + vars_form.eid + "&svc=core/search_items&params={\"spec\":{" + "\"itemsType\":\"avl_unit\"," + "\"propName\":\"sys_unique_id|sys_name|rel_customfield_value|rel_profilefield_value\"," + "\"propValueMask\":\"" + "*" + textBox_filter_object_ohrani.Text + "*" + "\", " + "\"sortType\":\"sys_name\"," + "\"or_logic\":\"1\"}," + "\"or_logic\":\"1\"," + "\"force\":\"1\"," + "\"flags\":\"257\"," + "\"from\":\"0\"," + "\"to\":\"5\"}");//15208907
                    json = myRequest.GetResponse();
                    m = JsonConvert.DeserializeObject<RootObject>(json);

                }
                List<object> list_add_alarm = new List<object>();
                foreach (var keyvalue in m.items)
                {
                    list_add_alarm.Add(new List_add_alarm() { Id = keyvalue.id, Name = keyvalue.nm });
                }
                listBox_object_ohrani.DataSource = list_add_alarm;
                listBox_object_ohrani.DisplayMember = "Name";
                listBox_object_ohrani.ValueMember = "Id";

                if (textBox_filter_object_ohrani.Text == "")
                {
                    listBox_object_ohrani.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "textBox_filter_object_ohrani_TextChanged");
            }
        }
        private class List_add_alarm
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private void button_add_new_alarm_Click(object sender, EventArgs e)
        {
            if (listBox_object_ohrani.SelectedIndex <= -1)
            {
                MessageBox.Show("Необхідно вибрати об'єкт");
                return;
            }
            vars_form.add_alarm_unit_id = listBox_object_ohrani.SelectedValue.ToString();
            add_alarm_full form = new add_alarm_full();
            form.Show();
            this.Close();
        }

        private void listBox_object_ohrani_DoubleClick(object sender, EventArgs e)
        {
            if (listBox_object_ohrani.SelectedIndex <= -1)
            {
                MessageBox.Show("Необхідно вибрати об'єкт");
                return;
            }
            vars_form.add_alarm_unit_id = listBox_object_ohrani.SelectedValue.ToString();
            add_alarm_full form = new add_alarm_full();
            form.Show();
            this.Close();

        }
    }
}
