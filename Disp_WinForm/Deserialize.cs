﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Disp_WinForm
{
    
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
        public int z { get; set; }
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
        public string eid { get; set; }
        public User user { get; set; }
        public deserialize_Item item { get; set; }

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

}
