using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Disp_WinForm
{
    class Deserialize
    {
    }

    public class Flds
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
        public int uacl { get; set; }
        public string uid { get; set; }
        public string uid2 { get; set; }
        public int hw { get; set; }
        public string ph { get; set; }
        public string ph2 { get; set; }
        public string psw { get; set; }
        //public Dictionary<string, Pos> pos { get; set; }
        public Dictionary<int, Flds> flds { get; set; }
        public List<Cmds> cmds { get; set; }
        public Dictionary<int, Sens> sens { get; set; }
        public Dictionary<string, string> pos { get; set; }

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

    public class RootObject
    {
        public int flags { get; set; }
        public int error { get; set; }
        public List<deserialize_Item> items { get; set; }
    }


    public class Sens
    {
        public int id { get; set; }
        public string n { get; set; }
    }

}
