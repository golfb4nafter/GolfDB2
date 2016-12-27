using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace GolfDB2.Models
{

    [Table("EventDetail")]
    public class EventDetail
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int CourseId { get; set; }
        public string PlayFormat { get; set; }
        public int NumberOfHoles { get; set; }
        public byte IsShotgunStart { get; set; }
        public string Sponsor { get; set; }
        public int PlayListId { get; set; }
    }
}