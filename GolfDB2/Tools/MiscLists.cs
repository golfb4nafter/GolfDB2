using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using GolfDB2.Models;

namespace GolfDB2.Tools
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
        public static string GetLabelsByCourseIdAndType(int courseId, string labelType, string connectionString)
        {
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Value", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Text", ordinal = 1, type = ParamType.charString });
            string query = string.Format("SELECT Id, Label FROM Labels WHERE OwnerId={0} AND LabelType='{1}' ORDER BY Ordinal", courseId, labelType);
            return SqlLists.SqlQuery(query, parms, connectionString);
        }

        public static List<SelectListItem> GetPlayFormatListByCourseIdAndType(int courseId, string connectionString)
        {
            string json = GetLabelsByCourseIdAndType(courseId, "PlayFormat", connectionString);
            List<SelectListItem> items = JsonConvert.DeserializeObject<List<SelectListItem>>(json);
            return items;
        }

        public static List<SelectListItem> GetNineLabelsSelectListByCourseIdAndType(int courseId, string connectionString)
        {
            string json = GetLabelsByCourseIdAndType(courseId, "Nine", connectionString);
            List<SelectListItem> items = JsonConvert.DeserializeObject<List<SelectListItem>>(json);
            return items;
        }

        public static List<SelectListItem> GetScoreBoardSortOptions(string connectionString)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem() { Text = "Default", Value = "ordinal" });
            items.Add(new SelectListItem() { Text = "Division", Value = "division" });
            items.Add(new SelectListItem() { Text = "Player/Team", Value = "name" });
            items.Add(new SelectListItem() { Text = "Hole", Value = "startingholenumber" });
            items.Add(new SelectListItem() { Text = "Gross", Value = "totalscore" });

            return items;
        }

        public static List<SelectListItem> GetNumberOfHolesSelectListByCourseIdAndType(int courseId, int selection, string connectionString)
        {
            string json = GetLabelsByCourseIdAndType(courseId, "NumberOfHoles", connectionString);
            List<SelectListItem> items = JsonConvert.DeserializeObject<List<SelectListItem>>(json);

            foreach (SelectListItem item in items)
            {
                if (int.Parse(item.Value) == selection)
                {
                    item.Selected = true;
                    break;
                }
            }

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

        public static List<SelectListItem> GetHoleListSelectListByCourseId(int courseId, string connectionString)
        {
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Value", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Text", ordinal = 1, type = ParamType.charString });
            string query = string.Format("SELECT Id, Label FROM HoleList WHERE CourseId={0}", courseId);
            string json = SqlLists.SqlQuery(query, parms, connectionString);
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

        public static int GetIdFromEventDetailByEventDetailId(int eventDetailId, string connectionString)
        {
            // using EventId lookup EventDetails Id
            string query = string.Format("SELECT Id from EventDetail WHERE EventId={0}", eventDetailId);
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Value", ordinal = 0, type = ParamType.int32 });
            string resp = SqlLists.SqlQuery(query, parms, connectionString);

            KeyValuePair kvp = JsonConvert.DeserializeObject<KeyValuePair>(resp);
            return int.Parse(kvp.Value);
        }

        public static string GetEventTextByEventId(int eventId, string connectionString)
        {
            // using EventId lookup EventDetails Id
            string query = string.Format("SELECT text from Event WHERE id={0}", eventId);
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Value", ordinal = 0, type = ParamType.charString });
            string resp = SqlLists.SqlQuery(query, parms, connectionString);

            KeyValuePair kvp = JsonConvert.DeserializeObject<KeyValuePair>(resp);

            if (kvp == null)
                return "open";

            if (string.IsNullOrEmpty(kvp.Value))
                return "open";

            return kvp.Value;
        }

        public static List<SelectListItem> GetEventSelectList(int courseId, string connectionString)
        {
            string query = string.Format("SELECT [Id], [text] from [Event] WHERE [CourseId]={0} AND [start] >= '{1}' ORDER BY [start]", courseId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Value", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Text", ordinal = 1, type = ParamType.charString });
            string json = SqlLists.SqlQuery(query, parms, connectionString);
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Value = "0", Text = "open" });

            if (!string.IsNullOrEmpty(json))
            {
                if (json.StartsWith("["))
                    items.AddRange(JsonConvert.DeserializeObject<List<SelectListItem>>(json));
                else
                    items.Add(JsonConvert.DeserializeObject<SelectListItem>(json));
            }

            return items;
        }

        public static Dictionary<int, int> GetListOfInUseTeeTimesFor(int month, int day, int year, int holeId, string connectionString)
        {
            string startTime = new DateTime(year, month, day, 7, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = new DateTime(year, month, day, 17, 0, 0).ToString("yyyy-MM-dd HH:mm:ss");

            string query = string.Format("SELECT TeeTimeOffset FROM TeeTime WHERE HoleId={0} AND Tee_Time >= '{1}.000' AND Tee_Time <= '{2}.000'", holeId, startTime, endTime);

            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Value", ordinal = 0, type = ParamType.int32 });

            string resp = SqlLists.SqlQuery(query, parms, connectionString);

            Dictionary<int, int> dict = new Dictionary<int, int>();

            if (resp.StartsWith("["))
            {
                List<KeyValuePair> kvpList = JsonConvert.DeserializeObject<List<KeyValuePair>>(resp);
                foreach (KeyValuePair p in kvpList)
                {
                    dict.Add(int.Parse(p.Value), int.Parse(p.Value));
                }
            }
            else if (string.IsNullOrEmpty(resp))
            {
                return dict;
            }
            else
            {
                KeyValuePair kvp = JsonConvert.DeserializeObject<KeyValuePair>(resp);
                dict.Add(int.Parse(kvp.Value), int.Parse(kvp.Value));
            }

            return dict;
        }

        public static List<SelectListItem> MakeListOfAvailableTeeTimes(int month, int day, int year, int holeId, int eventId, string connectionString)
        {
            // Assume number 1, not associated with an event
            // Starting at 7:00 AM with a 10 minute interval.
            // Ending at 5:00 PM 
            // 6 tee times per hour * 10 hours.

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            DateTime curDateTime = new DateTime(year, month, day, 7, 0, 0);

            Dictionary<int, int> reserved = GetListOfInUseTeeTimesFor(month, day, year, holeId, connectionString);

            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 0; i < 60; i++)
            {
                // Filter for tee times in the past.
                if (curDateTime.AddMinutes(i * 10) < DateTime.Now)
                    continue;

                // Filter out any in use tee times.
                try
                {
                    int v = reserved[i];  // Already in use ?

                    continue;
                }
                catch { } // It's not in the dictionary so it can be ignored safely.

                string tmp = curDateTime.AddMinutes(i * 10).ToString("MM/dd/yyyy HH:mm:ss");

                // Slyly hide the ordinal of the tee time in the milliseconds date field..
                SelectListItem item = new SelectListItem() { Text = tmp, Value = tmp + "." + i.ToString() };

                items.Add(item);
            }

            return items;
        }

        public static List<SelectListItem> OrgSelectList(string connectionString)
        {
            string query = "SELECT [Id], [OrgName] from [Organization] ORDER BY [OrgName]";

            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Value", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Text", ordinal = 1, type = ParamType.charString });
            string json = SqlLists.SqlQuery(query, parms, connectionString);
            List<SelectListItem> items = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(json))
            {
                if (json.StartsWith("["))
                    items.AddRange(JsonConvert.DeserializeObject<List<SelectListItem>>(json));
                else
                    items.Add(JsonConvert.DeserializeObject<SelectListItem>(json));
            }

            return items;
        }

        public static List<SelectListItem> StartingHoleSelectList(int courseId, int playlistId, bool isShotgunStart, string connectionString)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            // GetHoleListByPlayListId
            string query = string.Format("SELECT [HoleList] From [HoleList] WHERE CourseId={0} AND Id = {1}", courseId, playlistId);
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Value", ordinal = 0, type = ParamType.charString });
            string json = SqlLists.SqlQuery(query, parms, connectionString);
            KeyValuePair kvp = JsonConvert.DeserializeObject<KeyValuePair>(json);

            string[] holeList = kvp.Value.Split(',');

            foreach (string s in holeList)
            {
                if (!isShotgunStart && !(s == "1" || s == "10" || s == "19"))
                    continue;

                int id = GetHoleIdByHoleNumber(courseId, int.Parse(s), connectionString);
                SelectListItem item = new SelectListItem() { Value = id.ToString(), Text = s };
                items.Add(item);
            }

            return items;
        }

        public static List<SelectListItem> StartingHoleSelectListByCourseId(int courseId, string connectionString)
        {
            string query = string.Format("SELECT [NumberOfNines] FROM [CourseData] WHERE [Id]={0}", courseId);

            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Value", ordinal = 0, type = ParamType.int32 });
            string json = SqlLists.SqlQuery(query, parms, connectionString);

            int count = 1;

            if (!string.IsNullOrEmpty(json))
            {
                KeyValuePair kvp = JsonConvert.DeserializeObject<KeyValuePair>(json);
                count = int.Parse(kvp.Value);
            }

            List<SelectListItem> items = new List<SelectListItem>();

            for (int i = 0; i < count; i++)
            {
                // Lookup Id from Hole table.
                int id = GetHoleIdByHoleNumber(courseId, (i * 9) + 1, connectionString);
                SelectListItem item = new SelectListItem() { Value = ((i * 9) + 1).ToString(), Text = ((i * 9) + 1).ToString() };

                items.Add(item);
            }

            return items;
        }

        public static int GetHoleIdByHoleNumber(int courseId, int holeNumber, string connectionString)
        {
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Value", ordinal = 0, type = ParamType.int32 });
            string query = string.Format("SELECT id FROM Hole WHERE CourseId={0} AND Number={1}", courseId, holeNumber);
            string resp = SqlLists.SqlQuery(query, parms, connectionString);
            KeyValuePair kvp = JsonConvert.DeserializeObject<KeyValuePair>(resp);
            return int.Parse(kvp.Value);
        }
     
    }
}