using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using GolfDB2.Models;

namespace GolfDB2.Tools
{
    public class StoredProcedures
    {
        public static bool InsertTeeTime(string connectionString, TeeTime obj )
        {
            return InsertTeeTime(connectionString, 
                obj.TeeTimeOffset, 
                obj.Tee_Time, 
                obj.CourseId, 
                obj.EventId, 
                obj.ReservedByName, 
                obj.TelephoneNumber, 
                obj.HoleId, 
                obj.NumberOfPlayers, 
                obj.PlayerNames);
        }

        public static bool InsertTeeTime(string connectionString, 
                                         int teeTimeOffset,
                                         DateTime teeTime,
                                         int courseId,
                                         int eventId,
                                         string reservedByName,
                                         string telephoneNumber,
                                         int holeId,
                                         int numberOfPlayers,
                                         string playerNames)
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                var command = new SqlCommand("insertTeeTime", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@TeeTimeOffset", teeTimeOffset);
                command.Parameters.AddWithValue("@Tee_Time", teeTime);
                command.Parameters.AddWithValue("@CourseId", courseId);
                command.Parameters.AddWithValue("@EventId", eventId);
                command.Parameters.AddWithValue("@ReservedByName", reservedByName);
                command.Parameters.AddWithValue("@TelephoneNumber", telephoneNumber);
                command.Parameters.AddWithValue("@HoleId", holeId);
                command.Parameters.AddWithValue("@NumberOfPlayers", numberOfPlayers);
                command.Parameters.AddWithValue("@PlayerNames", playerNames);
                connection.Open();
                command.ExecuteNonQuery();

                connection.Close();
            } catch (Exception ex)
            {
                Logger.LogError("InsertTeeTime", ex.ToString());
                return false;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }

            return true;
        }
    }
}