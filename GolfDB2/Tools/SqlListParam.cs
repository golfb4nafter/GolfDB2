using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace GolfDB2.Tools
{

    public enum ParamType
    {
        int32,
        charString,
        boolVal,
        numeric,
        dateTime
    }

    public class SqlListParam
    {
        public int ordinal { get; set; }

        public ParamType type { get; set; }

        public string name { get; set; }

        public string ToText()
        {
            return string.Format("ordinal={0}, type={1}, name={2}\r\n",ordinal ,type.ToString("G"), name);
        }
    }
}