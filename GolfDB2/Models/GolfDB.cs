using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GolfDB2.Models
{
    public partial class GolfDB : DbContext
    {
        public GolfDB()
            : base("name=DefaultConnection")
        {
        }

        public virtual DbSet<CourseData> CourseDatas { get; set; }
        public virtual DbSet<GeoData> GeoDatas { get; set; }
        public virtual DbSet<GeoObjectType> GeoObjectTypes { get; set; }
        public virtual DbSet<CourseRatings> Holes { get; set; }
        public virtual DbSet<GeoSpatialTable> GeoSpatialTables { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GeoData>()
                .Property(e => e.GeoObjectDescription)
                .IsUnicode(false);

            modelBuilder.Entity<GeoObjectType>()
                .Property(e => e.GeoObjectType1)
                .IsUnicode(false);

            modelBuilder.Entity<GeoObjectType>()
                .Property(e => e.GeoObjectDescription)
                .IsUnicode(false);
        }

        public System.Data.Entity.DbSet<GolfDB2.Models.Labels> Labels { get; set; }
    }
}
