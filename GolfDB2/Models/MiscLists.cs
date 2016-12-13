﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;

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
            parms.Add(new SqlListParam() { name = "Key", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Value", ordinal = 1, type = ParamType.charString });
            string query = string.Format("SELECT Id, Label FROM Labels WHERE OwnerId={0} AND LabelType='{1}' ORDER BY Ordinal", courseId, labelType);
            return SqlLists.SqlQuery(query, parms, connectionString);
        }

        public static string GetNineNameByCourseIdAndZeroBasedOrdinal(int courseId, int ordinal, ref int id, string connectionString)
        {
            if (id == null || id < 0)
                return "Course name not found";

            string query = string.Format("SELECT Id, Label FROM Labels WHERE OwnerId={0} AND Ordinal={1}", courseId, ordinal);
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Key", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Value", ordinal = 1, type = ParamType.charString });
            string resp = SqlLists.SqlQuery(query, parms, connectionString);

            KeyValuePair kvp = JsonConvert.DeserializeObject<KeyValuePair>(resp);
            return kvp.Value;
        }

        public static string GetCourseNameById(int? id)
        {
            return GetCourseNameById(id, null);
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
        public static string GetCourseNamesList()
        {
            return GetCourseNamesList(null);
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
            parms.Add(new SqlListParam() { name = "Key", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Value", ordinal = 1, type = ParamType.charString });
            return SqlLists.SqlQuery(query, parms, connectionString);
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
            parms.Add(new SqlListParam() { name = "Key", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Value", ordinal = 1, type = ParamType.int32 });
            string query = string.Format("SELECT Id, Number FROM Hole WHERE CourseId={0} ORDER BY Number", courseId);
            return SqlLists.SqlQuery(query, parms, connectionString);
        }

        public static int GetHoleNumberByHoleId(int holeId)
        {
            return GetHoleNumberByHoleId(holeId, null);
        }

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

        public static string GetGeoSpatialDataPointsByCourseId(int courseId, string connectionString)
        {
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Key", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Value", ordinal = 1, type = ParamType.charString });
            string query = string.Format("SELECT Id, LocationDescription FROM GeoSpatialTable WHERE CourseId = {0} ORDER BY cast(LocationDescription as nvarchar(max))", courseId);
            return SqlLists.SqlQuery(query, parms, connectionString);
        }

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

        public static string GetObjectTypeList()
        {
            return GetObjectTypeList(null);
        }

        public static string GetObjectTypeList(string connectionString)
        {
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Key", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Value", ordinal = 1, type = ParamType.charString });
            string query = string.Format("SELECT ID, GeoObjectType FROM GeoObjectType ORDER BY GeoObjectType");
            return SqlLists.SqlQuery(query, parms, connectionString);
        }
    }
}