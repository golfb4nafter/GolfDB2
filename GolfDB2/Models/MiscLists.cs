using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GolfDB2.Models
{
    public static class MiscLists
    {

        /// <summary>
        /// Returns a list of json formatted key value pairs from the labels table by Id and labelType.
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="labelType"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string GetNineLabelsByCourseIdAndType(int courseId, string labelType, string connectionString)
        {
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "key", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "val", ordinal = 1, type = ParamType.charString });
            string query = string.Format("SELECT Id, Label FROM Labels WHERE OwnerId={0} AND LabelType='{1}' ORDER BY Ordinal", courseId, labelType);
            return SqlLists.SqlQuery(query, parms, connectionString);
        }


        public static string GetNineNameByCourseIdAndZeroBasedOrdinal(int courseId, int ordinal, ref int id)
        {
            string query = string.Format("SELECT Id, Label FROM Labels WHERE CourseId={0} AND Ordinal={1}", courseId, ordinal);

            return "";
        }


        public static string GetCourseNameById(int? id)
        {

            return null;
        }

        public static string GetObjectTypeById(int id)
        {
            return "unknown or undefined";
        }

        public static string GetCourseNamesList()
        {
            return GetCourseNamesList(null);
        }

        public static string GetCourseNamesList(string connectionString)
        {
            string query = "SELECT Id, CourseName FROM CourseData ORDER BY CourseName";
            List<SqlListParam> parms = new List<SqlListParam>();
            return SqlLists.SqlQuery(query, parms, connectionString);
        }

        public static string GetHoleListByCourseId(int courseId, string connectionString)
        {
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "key", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "val", ordinal = 1, type = ParamType.int32 });
            string query = string.Format("SELECT Id, Number FROM Hole WHERE CourseId={0}", courseId);
            return SqlLists.SqlQuery(query, parms, connectionString);
        }

        public static string GetHoleListByCourseIdInternal(int courseId)
        {
            string query = "SELECT Id, Number FROM Hole WHERE CourseId='" + courseId.ToString() + "' ORDER BY Number";

            return null;
        }

        public static string GetGeoSpatialDataPointDescriptionById(int id)
        {
            string query = "SELECT LocationDescription FROM GeoSpatialTable WHERE Id ='" + id.ToString() + "'";

            return null;
        }

        public static string GetGeoSpatialDataPointsByCourseId(int courseId)
        {
            string query = string.Format("SELECT Id, LocationDescription FROM GeoSpatialTable WHERE CourseId = {0} ORDER BY cast(LocationDescription as nvarchar(max))", courseId);

            return null;
        }

        public static int GetHoleNumberByHoleId(int holeId)
        {
            string query = "SELECT Number FROM Hole WHERE Id = '" + holeId.ToString() + "'";

            return 0;
        }

        public static string UpdateObjectTypeList()
        {
            string query = "SELECT ID, GeoObjectDescription FROM GeoObjectType";

            return null;
        }
    }
}