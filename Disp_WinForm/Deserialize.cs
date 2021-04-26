using System;
using System.Collections.Generic;

using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Disp_WinForm
{
    public class BearerToken
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresInMilliseconds { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty(PropertyName = "refresh_token_expires_in")]
        public int RefreshTokenExpiresInMilliseconds { get; set; }
    }

    public class locator
    {
        public string h { get; set; }
        public string app { get; set; }
        public int at { get; set; }
        public int ct { get; set; }
        public int dur { get; set; }
        public int fl { get; set; }
        public IList<object> items { get; set; }
        public string p { get; set; }
    }
    public class Flds
    {
        public int id { get; set; }
        public string n { get; set; }
        public string v { get; set; }
    }

    public class Aflds
    {
        public int id { get; set; }
        public string n { get; set; }
        public string v { get; set; }
    }
    
    public class pflds
    {
        public int id { get; set; }
        public string n { get; set; }
        public string v { get; set; }
    }


    public class deserialize_Item
    {
        public string nm { get; set; }
        public int cls { get; set; }
        public int id { get; set; }
        public int mu { get; set; }
        public string uid { get; set; }
        public string uid2 { get; set; }
        public int hw { get; set; }
        public string ph { get; set; }
        public string ph2 { get; set; }
        public string psw { get; set; }
        public Dictionary<int, Flds> flds { get; set; }
        public Dictionary<int, Aflds> aflds { get; set; }
        public Dictionary<int, Aflds> pflds { get; set; }
        public List<Cmds> cmds { get; set; }
        public Dictionary<int, Sens> sens { get; set; }
        public Dictionary<string, string> pos { get; set; }
        public Dictionary<int, int> value { get; set; }
        public int netconn { get; set; }
        //public Dictionary<int, Lmsg> lmsg { get; set; }
        public Lmsg lmsg { get; set; }
        public List<int> u { get; set; }
        
        public Dictionary<string, Unf1> Unf { get; set; }

        public partial class Unf1
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("n")]
            public string N { get; set; }

            [JsonProperty("txt")]
            public string Txt { get; set; }

            [JsonProperty("ta")]
            public long Ta { get; set; }

            [JsonProperty("td")]
            public long Td { get; set; }

            [JsonProperty("ma")]
            public long Ma { get; set; }

            [JsonProperty("fl")]
            public long Fl { get; set; }

            [JsonProperty("ac")]
            public long Ac { get; set; }

            [JsonProperty("un")]
            public int[] Un { get; set; }

            [JsonProperty("trg")]
            public string Trg { get; set; }

            [JsonProperty("crc")]
            public long Crc { get; set; }

            [JsonProperty("ct")]
            public long Ct { get; set; }

            [JsonProperty("mt")]
            public long Mt { get; set; }
        }
    }

    public class Lmsg
    {
        public int t { get; set; }
        public int f { get; set; }
        public string tp { get; set; }
        public Pos2 pos { get; set; }
        public P p { get; set; }
    }

    public class Pos2
    {
        public double y { get; set; }
        public double x { get; set; }
        public string z { get; set; }
        public int s { get; set; }
        public int c { get; set; }
        public int sc { get; set; }
    }

    public class P
    {
        public int par5 { get; set; }
        public int par21 { get; set; }
        public int par6 { get; set; }
        public int par1 { get; set; }
        public int mcc { get; set; }
        public int mnc { get; set; }
        public int par69 { get; set; }
        public int par154 { get; set; }
        public double VPWR { get; set; }
        public double AIN1 { get; set; }
        public double AIN2 { get; set; }
        public double VBAT { get; set; }
        public int lac { get; set; }
        public int cell_id { get; set; }
        public int rx_level { get; set; }
        public int io_180 { get; set; }
        public int io_380 { get; set; }
        public int io_381 { get; set; }
        public int io_239 { get; set; }
        public int io_69 { get; set; }
        public double io_11 { get; set; }
        public double io_14 { get; set; }
        public double pwr_ext { get; set; }
        public int io_66 { get; set; }
        public double pwr_int { get; set; }
        public double iccid { get; set; }


        public string color { get; set; }
        public string email_to { get; set; }
        public string html { get; set; }
        public string img_attach { get; set; }
        public string subj { get; set; }
        public string flags { get; set; }
        public string apps { get; set; }
        public string lower_bound { get; set; }
        public string merge { get; set; }
        public string prev_msg_diff { get; set; }
        public string sensor_name_mask { get; set; }
        public string sensor_type { get; set; }
        public string type { get; set; }
        public string upper_bound { get; set; }
    }



    public class Pos
    {
        public string x { get; set; }
        public string y { get; set; }
    }

    public class Cmds
    {
        public string n { get; set; }
        public string p { get; set; }
    }


    public class ReturnCode
    {
        public string majorReturnCode { get; set; }
        public string minorReturnCode { get; set; }
    }

    public class DeviceInformationItem
    {
        public string itemName { get; set; }
        public string itemType { get; set; }
        public object itemValue { get; set; }
    }

    public class DeviceInformationList
    {
        public List<DeviceInformationItem> deviceInformationItem { get; set; }
    }

    public class ApnListItem
    {
        public string apnName { get; set; }
        public string staticIpAddress { get; set; }
    }

    public class ApnList
    {
        public ApnListItem apnListItem { get; set; }
    }

    public class Return
    {
        public ReturnCode returnCode { get; set; }
        public string deviceId { get; set; }
        public string customerServiceProfile { get; set; }
        public string state { get; set; }
        public string imei { get; set; }
        public string baseCountry { get; set; }
        public string customAttribute1 { get; set; }
        public DeviceInformationList deviceInformationList { get; set; }
        public ApnList apnList { get; set; }
        public string failureReason { get; set; }
    }

    public class GetDeviceDetailsv2Response
    {
        public Return @return { get; set; }
    }




    public class SetDeviceDetailsv4Response
    {
        public Return @return { get; set; }
    }



    public class RootObject
    {
        public SetDeviceDetailsv4Response setDeviceDetailsv4Response { get; set; }
        public GetDeviceDetailsv2Response getDeviceDetailsv2Response { get; set; }
        public long flags { get; set; }
        public int error { get; set; }
        public List<deserialize_Item> items { get; set; }
        public string eid { get; set; }
        public User user { get; set; }
        public deserialize_Item item { get; set; }
        public string h { get; set; }
        public string app { get; set; }
        public int at { get; set; }
        public int ct { get; set; }
        public int dur { get; set; }
        public int fl { get; set; }
        public string p { get; set; }
        public string au { get; set; }
        public string reason { get; set; }
        public SubmitTransactionalSMSResponse submitTransactionalSMSResponse { get; set; }
        public Fault fault { get; set; }


        public int id { get; set; }
        public string n { get; set; }
        public string txt { get; set; }
        public int ta { get; set; }
        public int td { get; set; }
        public int ma { get; set; }
        public int mmtd { get; set; }
        public int cdt { get; set; }
        public int mast { get; set; }
        public int mpst { get; set; }
        public int cp { get; set; }
        public int tz { get; set; }
        public string la { get; set; }
        public int ac { get; set; }
        public Sch sch { get; set; }
        public CtrlSch ctrl_sch { get; set; }
        public List<int> un { get; set; }
        public List<Act> act { get; set; }
        public Trg trg { get; set; }
        public int mt { get; set; }

    }
    public class Sch
    {
        public int f1 { get; set; }
        public int f2 { get; set; }
        public int t1 { get; set; }
        public int t2 { get; set; }
        public int m { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int fl { get; set; }
    }

    public class CtrlSch
    {
        public int f1 { get; set; }
        public int f2 { get; set; }
        public int t1 { get; set; }
        public int t2 { get; set; }
        public int m { get; set; }
        public int y { get; set; }
        public int w { get; set; }
        public int fl { get; set; }
    }


    public class Act
    {
        public string t { get; set; }
        public P p { get; set; }
    }

    public class Trg
    {
        public string t { get; set; }
        public P p { get; set; }
    }

    public class Detail
    {
        public string errorcode { get; set; }
    }

    public class Fault
    {
        public string faultstring { get; set; }
        public Detail detail { get; set; }
    }

    public class SubmitTransactionalSMSResponse
    {
        public Return @return { get; set; }
    }

    public class accounts
    {
        public string acl { get; set; }
        public string dacl { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string nm { get; set; }
    }

    public class Value_
    {
        public string n { get; set; }
        public string d { get; set; }
    }


    public class Sens
    {
        public int id { get; set; }
        public string n { get; set; }
    }
    public class TestConnection
    {
        public int error { get; set; }
    }

}
