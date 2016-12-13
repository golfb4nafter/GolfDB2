namespace GolfDB2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CourseData")]
    public partial class CourseData
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string CourseName { get; set; }

        [StringLength(128)]
        public string Address1 { get; set; }

        [StringLength(128)]
        public string Address2 { get; set; }

        [StringLength(128)]
        public string City { get; set; }

        [StringLength(128)]
        public string State { get; set; }

        [StringLength(128)]
        public string PostalCode { get; set; }

        [StringLength(128)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        public string Url { get; set; }

        public string GoogleMapUrl { get; set; }

        public int? NumberOfHoles { get; set; }

        public int? NumberOfNines { get; set; }
    }
}
