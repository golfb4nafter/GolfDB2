    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

namespace GolfDB2.Models
{

    [Table("MobileScoreCard")]
    public partial class MobileScoreCard
    {
        public int Id { get; set; }
        public string Names { get; set; }
        public int ScoreCardId { get; set; }
    }
}
