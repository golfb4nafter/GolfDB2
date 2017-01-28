using System;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace GolfDB2.Models
{
    [Table("UploadFiles")]
    public class UploadFiles
    {
        public int Id { get; set; }

        [Required]
        public HttpPostedFileBase File { get; set; }
    }
}