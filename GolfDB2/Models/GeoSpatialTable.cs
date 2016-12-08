namespace GolfDB2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GeoSpatialTable")]
    public partial class GeoSpatialTable
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        [StringLength(20)]
        public string Latitude { get; set; }

        [StringLength(20)]
        public string Longitude { get; set; }

        [StringLength(10)]
        public string Altitude { get; set; }

        public string LocationDescription { get; set; }

        public string GoogleMapsViewUrl { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CourseId { get; set; }
    }
}
