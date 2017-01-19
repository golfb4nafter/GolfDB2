using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using GolfDB2.Models;

namespace GolfDB2.Tools
{
    public static class SqlLists
    {
        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public static string MakeLabelValuePair(string label, string value, string seperator)
        {
            return seperator + "\"" + label + "\":\"" + value + "\"";
        }

        public static string DumpParmList(List<SqlListParam> parms)
        {
            StringBuilder resp = new StringBuilder();

            resp.Append("{\r\n");

            foreach (SqlListParam p in parms)
                resp.Append(p.ToText());

            resp.Append("}\r\n");

            return resp.ToString();
        }

        private static string FormatAsJson(SqlDataReader rdr, List<SqlListParam> parms)
        {
            GolfDB2Logger.LogDebug("FormatAsJson", DumpParmList(parms));

            StringBuilder jsonString = new StringBuilder();
            string seperator = "";

            jsonString.Append("{");

            try
            {

                foreach (SqlListParam p in parms)
                {
                    if (p.ordinal > 0)
                        seperator = ",";

                    string value = "";

                    switch (p.type)
                    {
                        case ParamType.int32:
                            value = rdr.GetInt32(p.ordinal).ToString(CultureInfo.CurrentCulture);
                            break;

                        case ParamType.charString:
                            value = rdr.GetString(p.ordinal);
                            break;

                        case ParamType.boolVal:
                            value = rdr.GetBoolean(p.ordinal).ToString(CultureInfo.CurrentCulture);
                            break;

                        case ParamType.numeric:
                            value = rdr.GetDecimal(p.ordinal).ToString(CultureInfo.CurrentCulture);
                            break;

                        case ParamType.dateTime:
                            value = rdr.GetDateTime(p.ordinal).ToLongTimeString();
                            break;
                    }

                    jsonString.Append(MakeLabelValuePair(p.name, value, seperator));
                }
            }
            catch (Exception ex)
            {
                GolfDB2Logger.LogError("FormatAsJson", ex.ToString());
            }

            jsonString.Append("}");

            return jsonString.ToString();
        }

        public static string SqlQuery(string query, List<SqlListParam> parms, string connectionString)
        {
            GolfDB2Logger.LogDebug("SqlQuery", string.Format("Query: {0}", query));

            StringBuilder jsonString = new StringBuilder();
            int count = 0;

            try
            {
                if (connectionString == null)
                    connectionString = GetConnectionString("DefaultConnection");

                using (SqlConnection c = new SqlConnection(connectionString))
                {
                    c.Open();
                    SqlCommand cmd = new SqlCommand(query, c);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (count > 0)
                                jsonString.Append(",");

                            jsonString.Append(FormatAsJson(rdr, parms)); // Converts a single row

                            count++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GolfDB2Logger.LogError("SqlQuery", ex.ToString());
            }

            if (count > 1)
                return "[" + jsonString.ToString() + "]";

            return jsonString.ToString();
        }
    }
}