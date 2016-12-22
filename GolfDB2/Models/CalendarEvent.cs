using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace GolfDB2.Models
{

    [Table("Event")]
    public class CalendarEvent
    {
        public int id { get; set; }
        public int CourseId { get; set; }
        public string text { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }
}