namespace GolfDB2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GeoData")]
    public partial class GeoData
    {
        public int Id { get; set; }

        public int GeoSpatialDataId { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string GeoObjectDescription { get; set; }

        public int GeoObjectType { get; set; }

        public int HoleId { get; set; }

        public int OrderNumber { get; set; }

        public int? CourseId { get; set; }

        public int? YardsToFront { get; set; }

        public int? YardsToMiddle { get; set; }

        public int? YardsToBack { get; set; }
    }
}
