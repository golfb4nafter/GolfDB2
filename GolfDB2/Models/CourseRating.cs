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

        public Decimal? Course_Rating { get; set; }

        public int SlopeRating18 { get; set; }

        [StringLength(50)]
        public string Front9 { get; set; }

        [StringLength(50)]
        public string Back9 { get; set; }

        public Decimal? BogeyRating { get; set; }

        [StringLength(1)]
        public string Gender { get; set; }

        public string HandicapByHole { get; set; }

        public int TeeBoxMenuColorsId { get; set; }

        public int HoleListId { get; set; }
    }
}
