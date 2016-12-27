using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;
using GolfDB2.Tools;

namespace GolfDB2.Models
{
    // Todo:
    //    1) Error handling.
    //    2) Application logging
    //    3) Statistics

    public class KeyValuePair
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

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
            parms.Add(new SqlListParam() { name = "Value", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Text", ordinal = 1, type = ParamType.charString });
            string query = string.Format("SELECT Id, Label FROM Labels WHERE OwnerId={0} AND LabelType='{1}' ORDER BY Ordinal", courseId, labelType);
            return SqlLists.SqlQuery(query, parms, connectionString);
        }

        public static List<SelectListItem> GetNineLabelsSelectListByCourseIdAndType(int courseId, string connectionString)
        {
            string json = GetNineLabelsByCourseIdAndType(courseId, "Nine", connectionString);
            List<SelectListItem> items = JsonConvert.DeserializeObject<List<SelectListItem>>(json);
            return items;
        }

        public static string GetNineNameByCourseIdAndZeroBasedOrdinal(int courseId, int ordinal, string connectionString)
        {
            //if (id == null || id < 0)
            //    return "Course name not found";
            try
            {
                string query = string.Format("SELECT Id, Label FROM Labels WHERE LabelType='Nine' AND OwnerId={0} AND Ordinal={1}", courseId, ordinal);
                List<SqlListParam> parms = new List<SqlListParam>();
                parms.Add(new SqlListParam() { name = "Key", ordinal = 0, type = ParamType.int32 });
                parms.Add(new SqlListParam() { name = "Value", ordinal = 1, type = ParamType.charString });
                string resp = SqlLists.SqlQuery(query, parms, connectionString);

                if (!string.IsNullOrEmpty(resp))
                {
                    KeyValuePair kvp = JsonConvert.DeserializeObject<KeyValuePair>(resp);
                    return kvp.Value;
                }
            }
            catch (Exception ex)
            {
                GolfDB2Logger.LogError("GetNineNameByCourseIdAndZeroBasedOrdinal", ex.ToString());
            }

            GolfDB2Logger.LogDebug("GetNineNameByCourseIdAndZeroBasedOrdinal",
                string.Format("CourseId={0}, ordinal={1}, connectionString={2}", courseId, ordinal, connectionString));

            return "unknown";
        }

        public static string GetCourseNameById(int? id, string connectionString)
        {
            if (id == null || id < 0)
                return "Course name not found";

            string query = string.Format("SELECT Id, CourseName FROM CourseData WHERE Id={0}", id);
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Key", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Value", ordinal = 1, type = ParamType.charString });
            string resp = SqlLists.SqlQuery(query, parms, connectionString);
            KeyValuePair kvp = JsonConvert.DeserializeObject<KeyValuePair>(resp);
            return kvp.Value;
        }

        /// <summary>
        /// api/RapiCourse
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string GetCourseNamesList(string connectionString)
        {
            string query = "SELECT Id, CourseName FROM CourseData ORDER BY CourseName";
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Value", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Text", ordinal = 1, type = ParamType.charString });
            return SqlLists.SqlQuery(query, parms, connectionString);
        }

        public static List<SelectListItem> GetCourseNamesSelectList()
        {
            string json = GetCourseNamesList(null);
            List<SelectListItem> items = JsonConvert.DeserializeObject<List<SelectListItem>>(json);
            return items;
        }

        /// <summary>
        /// api/RapiHoleList/{id}
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string GetHoleListByCourseId(int courseId, string connectionString)
        {
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Value", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Text", ordinal = 1, type = ParamType.int32 });
            string query = string.Format("SELECT Id, Number FROM Hole WHERE CourseId={0} ORDER BY Number", courseId);
            return SqlLists.SqlQuery(query, parms, connectionString);
        }

        public static List<SelectListItem> GetHoleListByCourseId(int courseId)
        {
            string json = GetHoleListByCourseId(courseId, null);
            List<SelectListItem> items = JsonConvert.DeserializeObject<List<SelectListItem>>(json);
            return items;
        }

        /// <summary>
        /// api/RAPIHoleNumber/1
        /// </summary>
        /// <param name="holeId"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static int GetHoleNumberByHoleId(int holeId, string connectionString)
        {
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Key", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Value", ordinal = 1, type = ParamType.int32 });
            string query = string.Format("SELECT id, Number FROM Hole WHERE Id ={0}", holeId);
            string resp = SqlLists.SqlQuery(query, parms, connectionString);
            KeyValuePair kvp = JsonConvert.DeserializeObject<KeyValuePair>(resp);
            return int.Parse(kvp.Value);
        }

        /// <summary>
        /// api/RApiGpsDescription/1
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string GetGeoSpatialDataPointDescriptionById(int id, string connectionString)
        {
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Key", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Value", ordinal = 1, type = ParamType.charString });
            string query = string.Format("SELECT Id, LocationDescription FROM GeoSpatialTable WHERE Id={0}", id);
            string resp = SqlLists.SqlQuery(query, parms, connectionString);
            KeyValuePair kvp = JsonConvert.DeserializeObject<KeyValuePair>(resp);
            return kvp.Value;
        }

        /// <summary>
        /// api/RapiGPS/1
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string GetGeoSpatialDataPointsByCourseId(int courseId, string connectionString)
        {
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Value", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Text", ordinal = 1, type = ParamType.charString });
            string query = string.Format("SELECT Id, LocationDescription FROM GeoSpatialTable WHERE CourseId = {0} ORDER BY cast(LocationDescription as nvarchar(max))", courseId);
            return SqlLists.SqlQuery(query, parms, connectionString);
        }

        public static List<SelectListItem> GetGeoSpatialDataPointsByCourseId(int courseId)
        {
            string json = GetGeoSpatialDataPointsByCourseId(courseId, null);
            List<SelectListItem> items = JsonConvert.DeserializeObject<List<SelectListItem>>(json);
            return items;
        }

        /// <summary>
        /// api/RApiObjectType/3
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string GetObjectTypeById(int id, string connectionString)
        {
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Key", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Value", ordinal = 1, type = ParamType.charString });
            string query = string.Format("SELECT ID, GeoObjectType FROM GeoObjectType WHERE ID={0}", id);
            string resp = SqlLists.SqlQuery(query, parms, connectionString);
            KeyValuePair kvp = JsonConvert.DeserializeObject<KeyValuePair>(resp);
            return kvp.Value;
        }

        /// <summary>
        /// api/RApiObjectList
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string GetObjectTypeList(string connectionString)
        {
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Value", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Text", ordinal = 1, type = ParamType.charString });
            string query = string.Format("SELECT ID, GeoObjectType FROM GeoObjectType ORDER BY GeoObjectType");
            return SqlLists.SqlQuery(query, parms, connectionString);
        }

        public static List<SelectListItem> GetObjectTypeSelectList()
        {
            string json = GetObjectTypeList(null);
            List<SelectListItem> items = JsonConvert.DeserializeObject<List<SelectListItem>>(json);
            return items;
        }

        public static string GetCourseRatings(string filter, string connectionString)
        {
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Id", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "CourseId", ordinal = 1, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "TeeName", ordinal = 2, type = ParamType.charString });
            parms.Add(new SqlListParam() { name = "Course_Rating", ordinal = 3, type = ParamType.numeric });
            parms.Add(new SqlListParam() { name = "SlopeRating18", ordinal = 4, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Front9", ordinal = 5, type = ParamType.charString });
            parms.Add(new SqlListParam() { name = "Back9", ordinal = 6, type = ParamType.charString });
            parms.Add(new SqlListParam() { name = "BogeyRating", ordinal = 7, type = ParamType.numeric });
            parms.Add(new SqlListParam() { name = "Gender", ordinal = 8, type = ParamType.charString });
            parms.Add(new SqlListParam() { name = "HolesListDescription", ordinal = 9, type = ParamType.charString });
            parms.Add(new SqlListParam() { name = "HandicapByHole", ordinal = 10, type = ParamType.charString });

            string query = string.Format("SELECT Id, CourseId, TeeName, Course_Rating, SlopeRating18, Front9, Back9, BogeyRating, Gender, HolesListDescription, HandicapByHole FROM CourseRatings");
            return SqlLists.SqlQuery(query + " " + filter, parms, connectionString);
        }

        public static List<CourseRating> GetCourseRatingsList(string connectionString)
        {
            string json = GetCourseRatings("", connectionString);
            List<CourseRating> items = JsonConvert.DeserializeObject<List<CourseRating>>(json);
            return items;
        }

        public static int GetHoleHandicap(int courseId, string tee, string gender, string holesListDescription, int holeNumber, string connectionString)
        {
            CourseRatingsCache cache = new CourseRatingsCache(connectionString);
            CourseRating r = cache.GetCourseRatingByCourseIdTeeAndGender(courseId, tee, gender, holesListDescription);
            return int.Parse(r.HandicapByHole.Split(',')[holeNumber - 1]);
        }

        public static List<SelectListItem> GetLogLevelSelectList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "FATAL", Value = "1" });
            items.Add(new SelectListItem() { Text = "ERROR", Value = "2" });
            items.Add(new SelectListItem() { Text = "WARN", Value = "3" });
            items.Add(new SelectListItem() { Text = "INFO", Value = "4" });
            items.Add(new SelectListItem() { Text = "DEBUG", Value = "5" });
            return items;
        }
    }
}