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
        public int PlayFormat { get; set; }
        public int NumberOfHoles { get; set; }
        public bool IsShotgunStart { get; set; }
        public string Sponsor { get; set; }
        public int PlayListId { get; set; }
        public int OrgId { get; set; }
        public int StartHoleId { get; set; }
        public int NumGroups { get; set; }
        public int NumPerGroup { get; set; }
        public string SortOn { get; set; }
        public decimal SkinsAmount { get; set; }
        public int NumberOfFlights { get; set; }
    }
}