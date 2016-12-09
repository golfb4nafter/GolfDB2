using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace GolfDB2.Models
{
    [Table("Labels")]
    public partial class Labels
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public int Ordinal { get; set; }
        public string Label { get; set; }
        public string Notes { get; set; }
    }
}
