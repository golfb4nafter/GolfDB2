namespace GolfDB2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Hole")]
    public partial class Hole
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int Nine { get; set; }

        public int Number { get; set; }

        public string PhotoUrl { get; set; }

        public string Description { get; set; }
    }
}
