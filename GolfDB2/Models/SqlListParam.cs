using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace GolfDB2.Models
{

    public enum ParamType
    {
        int32,
        charString,
        boolVal
    }

    public class SqlListParam
    {
        public int ordinal { get; set; }

        public ParamType type { get; set; }

        public string name { get; set; }
    }
}