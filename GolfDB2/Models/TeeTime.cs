using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace GolfDB2.Models
{

    [Table("TeeTime")]
    public class TeeTime
    {
        public int Id { get; set; }
        public int TeeTimeOffset { get; set; }
        public DateTime Tee_Time { get; set; }
        public int CourseId { get; set; }
        public int EventId { get; set; }
        [StringLength(128)]
        public string ReservedByName { get; set; }
        [StringLength(80)]
        public string TelephoneNumber { get; set; }
        public int HoleId { get; set; }
        public int NumberOfPlayers { get; set; }
        [StringLength(256)]
        public string PlayerNames { get; set; }
    }
}