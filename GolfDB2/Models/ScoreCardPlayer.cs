namespace GolfDB2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ScoreCardPlayer")]
    public partial class ScoreCardPlayer
    {
        public int Id { get; set; }

        public int ScoreCardId { get; set; }
        public string GolferName { get; set; }
        public int GolferId { get; set; }
    }
}
