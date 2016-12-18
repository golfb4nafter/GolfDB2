namespace GolfDB2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ScoreCard")]
    public partial class ScoreCard
    {
        public int Id { get; set; }
        [StringLength(10)]
        public string HoleList { get; set; }
        public int StartingHole { get; set; }
        public bool Team { get; set; }
        public int Division { get; set; }
        public string TeamName { get; set; }
    }
}
