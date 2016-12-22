using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfDB2.Models
{
    public class GlobalSetting
    {
        public int Id { get; set; }
        public string SettingName { get; set; }
        public string Value { get; set; }
        public DateTime LastWritten { get; set; }
        public int LastUserId  { get; set; }
    }
}