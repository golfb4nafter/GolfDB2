namespace GolfDB2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GeoObjectType")]
    public partial class GeoObjectType
    {
        public int ID { get; set; }

        [Column("GeoObjectType")]
        [Required]
        [StringLength(40)]
        public string GeoObjectType1 { get; set; }

        [Required]
        public string GeoObjectDescription { get; set; }
    }
}
