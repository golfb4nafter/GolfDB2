using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GolfDB2.Models;

namespace GolfDB2.Tools
{
    public class CourseRatingsCache
    {
        private List<CourseRating> ratingsList;

        public CourseRatingsCache(string connectionString)
        {
            RatingsList = MiscLists.GetCourseRatingsList(connectionString);
        }

        public List<CourseRating> RatingsList
        {
            get
            {
                return ratingsList;
            }

            set
            {
                ratingsList = value;
            }
        }

        public CourseRating GetCourseRatingByCourseIdTeeAndGender(int id, string tee, string gender, string holesListDescription)
        {
            foreach(CourseRating r in RatingsList)
            {
                if (r.CourseId == id && 
                    r.TeeName.ToLower() == tee.ToLower() && 
                    r.Gender.ToLower() == gender.ToLower() && 
                    r.HolesListDescription == holesListDescription)
                    return r;
            }

            return null;
        }

    }
}