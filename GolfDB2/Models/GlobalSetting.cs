using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace GolfDB2.Models
{
    [Table("GlobalSettings")]
    public class GlobalSetting
    {
        public int Id { get; set; }
        public string SettingName { get; set; }
        public string Value { get; set; }
        public DateTime LastWritten { get; set; }
        public int LastUserId  { get; set; }
    }
}