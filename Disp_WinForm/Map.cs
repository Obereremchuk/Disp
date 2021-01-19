using Gecko;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Disp_WinForm
{
    public partial class Map : Form
    {
        Macros macros = new Macros();
        private string wl_id;
        private string unit_name;
        private static System.Timers.Timer aTimer;
        public Map()
        {
            InitializeComponent();

            string t1 = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\Firefox";
            Xpcom.Initialize(t1);
            GeckoPreferences.User["dom.max_script_run_time"] = 0;
            //GeckoPreferences.User["javascript.enabled"] = false;

            aTimer = new System.Timers.Timer();
            SetTimer();
        }

        private void SetTimer()
        {
  
            aTimer.Interval = 3000;
            aTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimedEvent_open);
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

        }//Запускаем таймер на 2 сек

        private void OnTimedEvent_open(object sender, EventArgs e)
        {
            new Task(SerchForm).Start();

        }


        private void SerchForm()
        {
            if (vars_form.MapFormIdObject != wl_id)
            {
                wl_id = vars_form.MapFormIdObject;
                unit_name = vars_form.MapFormName;
                RenderMap(wl_id);

            }

        }

        private void CleareMap()
        {
            if (geckoWebBrowser1 != null)
            {
                geckoWebBrowser1 = null;
            }
        }

        private void RenderMap(string _search_id)
        {

            try
            {
                //geckoWebBrowser1.Navigate("http://10.44.30.32/disp_app/HTMLPage_map.html?foo=" + _search_id);

                string json = macros.WialonRequestSimple("&svc=token/update&params={" +
                                                    "\"callMode\":\"create\"," +
                                                    "\"app\":\"locator\"," +
                                                    "\"at\":\"0\"," +
                                                    "\"dur\":\"1800\"," +
                                                    "\"fl\":\"-1\"," +
                                                    "\"p\":\"{" + "\\" + "\"sensorMasks" + "\\" + "\"" + ":[" + "\\" + "\"*" + "\\" + "\"]," + "\\" + "\"note" + "\\" + "\"" + ":" + "\\" + "\"" + unit_name + "" + "\\" + "\"," + "\\" + "\"zones" + "\\" + "\"" + ":" + "\\" + "\"1" + "\\" + "\"," + "\\" + "\"tracks" + "\\" + "\"" + ":" + "\\" + "\"1" + "\\" + "\"" + "}\"," +
                                                    "\"items\":[" + _search_id + "]" +
                                                    "}");

                var m = JsonConvert.DeserializeObject<locator>(json);

                string locator_url = "https://navi.venbest.com.ua/locator/index.html?t=" + m.h;
                if (geckoWebBrowser1.InvokeRequired)
                    geckoWebBrowser1.Invoke(new Action(() => geckoWebBrowser1.Navigate(locator_url)));
                else
                    geckoWebBrowser1.Navigate(locator_url);

            }
            catch { }
        }

    }
}
