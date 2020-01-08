using Newtonsoft.Json;
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
    public partial class Activation_Form : Form
    {
        Macros macros = new Macros();
        public Activation_Form()
        {
            InitializeComponent();
            load_form();
            build_list_account();


        }

        private void build_list_account()
        {
            string json1 = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                    "\"spec\":{" +
                                                    "\"itemsType\":\"\"," +
                                                    "\"propName\":\"sys_id\"," +
                                                    "\"propValueMask\":\"" + "27,57,58,206,207,213,244,356,494,919" + "\", " +
                                                    "\"sortType\":\"sys_name\"," +
                                                    "\"or_logic\":\"1\"}," +
                                                    "\"force\":\"1\"," +
                                                    "\"flags\":\"4611686018427387903\"," +
                                                    "\"from\":\"\"," +
                                                    "\"to\":\"\"}");

            var m = JsonConvert.DeserializeObject<RootObject>(json1);


            string json = macros.wialon_request_new("&svc=core/check_accessors&params={" +
                                                    "\"items\":[\"1098\"]," +
                                                    "\"flags\":\"1\"}");
            
            var wl_accounts = JsonConvert.DeserializeObject < Dictionary <int, Dictionary<int, Dictionary<string, string>>>>(json);
            string t = "";

            for (int index = 0; index < wl_accounts[1098].Values.Count; index++)
            {
                var item = wl_accounts.ElementAt(index);
                var itemKey = item.Key;
                var itemValue = item.Value;
            }


            TreeNode node1 = new TreeNode("Кабінети користувача:");
            treeView_user_accounts.Nodes.Add(node1);
            treeView_user_accounts.Nodes[0].Expand();
            var stuff = JsonConvert.DeserializeObject<string[]>(json);
            try
            {
                foreach (var keyvalue in m.items[0].pflds)
                {
                    if (keyvalue.Value.n.Contains("vehicle_type"))
                    {
                        treeView_user_accounts.Nodes[0].Nodes.Add(new TreeNode(keyvalue.Value.v));
                    }
                    else if (keyvalue.Value.n.Contains("vin"))
                    {
                        treeView_user_accounts.Nodes[0].Nodes.Add(new TreeNode(keyvalue.Value.v));
                    }
                }
            }
            catch (Exception e)
            {
                string er = e.ToString();
            }
            
        }

        private void load_form()
        {
            string json = macros.wialon_request_new("&svc=core/search_items&params={" +
                                                    "\"spec\":{" +
                                                    "\"itemsType\":\"avl_unit\"," +
                                                    "\"propName\":\"sys_id\"," +
                                                    "\"propValueMask\":\"" + vars_form.id_object_for_activation + "\", " +
                                                    "\"sortType\":\"sys_name\"," +
                                                    "\"or_logic\":\"1\"}," +
                                                    "\"force\":\"1\"," +
                                                    "\"flags\":\"4611686018427387903\"," +
                                                    "\"from\":\"0\"," +
                                                    "\"to\":\"5\"}");
            var m = JsonConvert.DeserializeObject<RootObject>(json);

            name_object_current_textBox.Text = m.items[0].nm;
            imei_object_textBox.Text = m.items[0].uid;
            uvaga_textBox.Text = m.items[0].flds[1].v;
            name_obj_new_textBox.Text = m.items[0].nm;
        }

        private void button_chenge_uvaga_Click(object sender, EventArgs e)
        {
            string json = macros.wialon_request_new("&svc=item/update_name&params={" +
                                                    "\"itemId\":\""+vars_form.id_object_for_activation+"\"," +
                                                    "\"name\":\""+name_obj_new_textBox.Text.ToString()+"\"}");
            var m = JsonConvert.DeserializeObject<RootObject>(json);

            if (m.error == 0)
            {
                
                string st =macros.sql_command("UPDATE btk.Object " +
                               "SET " +
                               "Object_name = '" + name_obj_new_textBox.Text + "' " +
                               "WHERE " +
                               "idObject = '" + vars_form.id_db_object_for_activation + "';");
                if (st == "")
                {
                    load_form();
                    MessageBox.Show("Змінено!");
                }
                else
                {
                    MessageBox.Show("Упс, здається не вийшло");
                }
            }
            else
            {
                MessageBox.Show("Упс, здається не вийшло");
            }

        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            if (name_obj_new_textBox.Text == name_object_current_textBox.Text)
            {
                this.Close();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Назва обєкту змінена", "Зберегти?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    button_chenge_uvaga.PerformClick();
                    this.Close();

                }
                else if (dialogResult == DialogResult.No)
                {
                    this.Close();
                }
            }
        }
    }
}
