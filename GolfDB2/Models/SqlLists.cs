using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace GolfDB2.Models
{
    public class SqlLists
    {
        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public static string MakeLabelValuePair(string label, string value, string seperator)
        {
            return seperator + "\"" + label + "\":\"" + value + "\"";
        }

        private static string formatAsJson(SqlDataReader rdr, List<SqlListParam> parms)
        {
            StringBuilder jsonString = new StringBuilder();
            string seperator = "";

            jsonString.Append("{");

            foreach (SqlListParam p in parms)
            {
                if (p.ordinal > 0)
                    seperator = ",";

                string value = "";

                switch (p.type)
                {
                    case ParamType.int32:
                        value = rdr.GetInt32(p.ordinal).ToString();
                        break;

                    case ParamType.charString:
                        value = rdr.GetString(p.ordinal);
                        break;

                    case ParamType.boolVal:
                        value = rdr.GetBoolean(p.ordinal).ToString();
                        break;
                }

                jsonString.Append(MakeLabelValuePair(p.name, value, seperator));
            }

            jsonString.Append("}");

            return jsonString.ToString();
        }

        public static string SqlQuery(string query, List<SqlListParam> parms, string connectionString)
        {
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

                            jsonString.Append(formatAsJson(rdr, parms)); // Converts a single row

                            count++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex);
            }

            if (count > 1)
                return "[" + jsonString.ToString() + "]";

            return jsonString.ToString();
        }
    }
}