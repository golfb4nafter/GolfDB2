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

        public static string SqlQuery(string query, List<SqlListParam> parms, string connectionString)
        {
            try
            {
                string seperator = "";

                if (connectionString == null)
                    connectionString = GetConnectionString("DefaultConnection");

                using (SqlConnection c = new SqlConnection(connectionString))
                {
                    c.Open();
                    SqlCommand cmd = new SqlCommand(query, c);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        StringBuilder jsonString = new StringBuilder();

                        jsonString.Append("[");

                        while (rdr.Read())
                        {
                            if (jsonString.Length > 1)
                                jsonString.Append(",");

                            jsonString.Append("{");

                            foreach(SqlListParam p in parms)
                            {
                                if (p.ordinal > 0)
                                    seperator = ",";

                                string value = "";

                                switch(p.type)
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
                        }

                        return jsonString.ToString() + "]";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex);
            }

            return "[]";
        }
    }
}