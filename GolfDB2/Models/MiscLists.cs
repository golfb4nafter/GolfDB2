using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.SqlClient;

namespace GolfDB2.Models
{
    public class MiscLists
    {
        private static List<SelectListItem> courseNamesList = new List<SelectListItem>();
        private static List<SelectListItem> objectTypeList = new List<SelectListItem>();
        private static List<SelectListItem> currentHoleList = new List<SelectListItem>();
        private static List<SelectListItem> _GeoSpatialDataList = new List<SelectListItem>();

        public static List<SelectListItem> ObjectTypeList
        {
            get
            {
                return objectTypeList;
            }

            set
            {
                objectTypeList = value;
            }
        }

        public static List<SelectListItem> CourseNamesList
        {
            get
            {
                return courseNamesList;
            }

            set
            {
                courseNamesList = value;
            }
        }

        public static List<SelectListItem> GeoSpatialDataList
        {
            get
            {
                return _GeoSpatialDataList;
            }

            set
            {
                _GeoSpatialDataList = value;
            }
        }

        public static List<SelectListItem> CurrentHoleList
        {
            get
            {
                return currentHoleList;
            }

            set
            {
                currentHoleList = value;
            }
        }

        /// <summary>
        /// Returns a list of json formatted key value pairs from the labels table by Id and labelType.
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="labelType"></param>
        /// <returns></returns>
        public static string GetLabelsByCourseIdAndType(int courseId, string labelType)
        {
            try
            {
                using (SqlConnection c = new SqlConnection(GetConnectionString("DefaultConnection")))
                {
                    c.Open();

                    string query = string.Format("SELECT Id, Label FROM Labels WHERE CourseId={0} AND LabelType='{1}' ORDER BY Ordinal", courseId, labelType);

                    SqlCommand cmd = new SqlCommand(query, c);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        StringBuilder jsonString = new StringBuilder();

                        jsonString.Append("[");

                        while (rdr.Read())
                        {
                            if (jsonString.Length > 1)
                                jsonString.Append(",");

                            string tmp = "{\"key\":\"" + rdr.GetInt32(0).ToString() + "\",\"val\":\"" + rdr.GetString(1) + "\"}";

                            jsonString.Append(tmp);
                        }

                        return jsonString.ToString() + "]";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex);
            }

            return "[{key:\"-1\", val:\"-1\"}]";
        }


        public static string GetNineNameById(int id)
        {
            try
            {
                using (SqlConnection c = new SqlConnection(GetConnectionString("DefaultConnection")))
                {
                    c.Open();

                    string query = string.Format("SELECT Id, Label FROM Labels WHERE Id={0}", id);

                    SqlCommand cmd = new SqlCommand(query, c);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        StringBuilder jsonString = new StringBuilder();

                        jsonString.Append("[");

                        while (rdr.Read())
                        {
                            if (jsonString.Length > 1)
                                jsonString.Append(",");

                            string tmp = "{\"key\":\"" + rdr.GetInt32(1).ToString() + "\",\"val\":\"" + rdr.GetInt32(0).ToString() + "\"}";

                            jsonString.Append(tmp);
                        }

                        return jsonString.ToString() + "]";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex);
            }

            return "[{text:\"-1\", val:\"-1\"}]";
        }

        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;

        }
        
        public static string GetCourseNameById(int? id)
        {
            if (id.HasValue)
            {
                foreach (SelectListItem item in CourseNamesList)
                {
                    if (item.Text == id.ToString())
                    {
                        return item.Value;
                    }
                }
            }

            return "unknown or undefined";
        }

        public static string GetObjectTypeById(int id)
        {
            foreach (SelectListItem item in ObjectTypeList)
            {
                if (item.Text == id.ToString())
                {
                    return item.Value;
                }
            }

            return "unknown or undefined";
        }

        public static void UpdateCourseIdNameList()
        {
            CourseNamesList.Clear();

            try
            {
                using (SqlConnection c = new SqlConnection(GetConnectionString("DefaultConnection")))
                {
                    c.Open();

                    SqlCommand cmd = new SqlCommand("SELECT Id, CourseName FROM CourseData ORDER BY CourseName", c);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int index = rdr.GetInt32(0);
                            string value = rdr.GetString(1);
                            CourseNamesList.Add(new SelectListItem { Text = index.ToString(), Value = value });
                        }
                    }
                }
            } catch(Exception ex)
            {
                Console.Out.WriteLine(ex);
            }
        }

        public static string GetHoleListByCourseId(int courseId)
        {
            try
            {
                using (SqlConnection c = new SqlConnection(GetConnectionString("DefaultConnection")))
                {
                    c.Open();

                    SqlCommand cmd = new SqlCommand("SELECT Id, Number FROM Hole WHERE CourseId='" + courseId.ToString() + "' ORDER BY Number", c);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        StringBuilder jsonString = new StringBuilder();

                        jsonString.Append("[");

                        while (rdr.Read())
                        {
                            if (jsonString.Length > 1)
                                jsonString.Append(",");

                            string tmp = "{\"text\":\"" + rdr.GetInt32(1).ToString() + "\",\"val\":\"" + rdr.GetInt32(0).ToString()  + "\"}";

                            jsonString.Append(tmp);
                        }

                        return jsonString.ToString() + "]";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex);
            }

            return "[{text:\"-1\", val:\"-1\"}]";
        }

        public static void GetHoleListByCourseIdInternal(int courseId)
        {
            currentHoleList.Clear();

            try
            {
                using (SqlConnection c = new SqlConnection(GetConnectionString("DefaultConnection")))
                {
                    c.Open();

                    SqlCommand cmd = new SqlCommand("SELECT Id, Number FROM Hole WHERE CourseId='" + courseId.ToString() + "' ORDER BY Number", c);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int index = rdr.GetInt32(0);
                            int value = rdr.GetInt32(1);
                            currentHoleList.Add(new SelectListItem { Text = index.ToString(), Value = value.ToString() });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex);
            }
        }

        public static string GetGeoSpatialDataPointDescriptionById(int id)
        {
            List<SelectListItem> dataList = new List<SelectListItem>();

            try
            {
                using (SqlConnection c = new SqlConnection(GetConnectionString("DefaultConnection")))
                {
                    c.Open();

                    SqlCommand cmd = new SqlCommand("SELECT LocationDescription FROM GeoSpatialTable WHERE Id ='" + id.ToString() + "'", c);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                            return rdr.GetString(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex);
            }

            return "unavailable";
        }

        public static void GetGeoSpatialDataPointsByCourseId(int courseId)
        {
            GeoSpatialDataList.Clear();

            try
            {
                using (SqlConnection c = new SqlConnection(GetConnectionString("DefaultConnection")))
                {
                    c.Open();
                    string query = string.Format("SELECT Id, LocationDescription FROM GeoSpatialTable WHERE CourseId = {0} ORDER BY cast(LocationDescription as nvarchar(max))", courseId);

                    SqlCommand cmd = new SqlCommand(query, c);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int index = rdr.GetInt32(0);
                            string value = rdr.GetString(1);
                            GeoSpatialDataList.Add(new SelectListItem { Text = index.ToString(), Value = value });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex);
            }
        }

        public static int GetHoleNumberByHoleId(int holeId)
        {
            try
            {
                using (SqlConnection c = new SqlConnection(GetConnectionString("DefaultConnection")))
                {
                    c.Open();

                    SqlCommand cmd = new SqlCommand("SELECT Number FROM Hole WHERE Id = '" + holeId.ToString() + "'", c);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            return rdr.GetInt32(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex);
            }

            return -1;
        }

        public static void UpdateObjectTypeList()
        {
            ObjectTypeList.Clear();

            try
            {
                using (SqlConnection c = new SqlConnection(GetConnectionString("DefaultConnection")))
                {
                    c.Open();

                    SqlCommand cmd = new SqlCommand("SELECT ID, GeoObjectDescription FROM GeoObjectType", c);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int index = rdr.GetInt32(0);
                            string value = rdr.GetString(1);
                            ObjectTypeList.Add(new SelectListItem { Text = index.ToString(), Value = value });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex);
            }
        }
    }
}