using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace GolfDB2.Models
{

    [Table("CourseRatings")]
    public partial class CourseRating
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string TeeName { get; set; }
        public float Course_Rating { get; set; }
        public int SlopeRating18 { get; set; }
        public string Front9 { get; set; }
        public string Back9 { get; set; }
        public float BogeyRating { get; set; }
        public string Gender { get; set; }
        public string HolesListDescription { get; set; }
        public string HandicapByHole { get; set; }
    }
}
