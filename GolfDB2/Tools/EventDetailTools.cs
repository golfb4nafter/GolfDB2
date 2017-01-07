﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using GolfDB2.Models;

namespace GolfDB2.Tools
{
    public class EventDetailTools
    {

        public string MakeEventLabelString(int eventId, string connectionString)
        {
            try
            {
                string query = string.Format("SELECT [Id], [CourseId], [text], [start], [end] FROM Event WHERE Id={0}", eventId);

                List<SqlListParam> parms = new List<SqlListParam>();
                parms.Add(new SqlListParam() { name = "Id", ordinal = 0, type = ParamType.int32 });
                parms.Add(new SqlListParam() { name = "CourseId", ordinal = 1, type = ParamType.int32 });
                parms.Add(new SqlListParam() { name = "text", ordinal = 2, type = ParamType.charString });
                parms.Add(new SqlListParam() { name = "start", ordinal = 3, type = ParamType.dateTime });
                parms.Add(new SqlListParam() { name = "end", ordinal = 4, type = ParamType.dateTime });

                string resp = SqlLists.SqlQuery(query, parms, connectionString);
                Event evt = JsonConvert.DeserializeObject<Event>(resp);

                return string.Format("{0},{1},{2}", evt.text, evt.start, evt.end);
            }
            catch (Exception ex)
            {
                GolfDB2Logger.LogError("MakeEventLabelString", ex.ToString());
            }

            return "";
        }

        public int LookupOrCreateEventDetailRecord(int eventId, string connectionString)
        {
            try
            {
                // using EventId lookup EventDetails Id
                string query = string.Format("SELECT Id, Sponsor from EventDetail WHERE EventId={0}", eventId);
                List<SqlListParam> parms = new List<SqlListParam>();
                parms.Add(new SqlListParam() { name = "Value", ordinal = 0, type = ParamType.int32 });
                parms.Add(new SqlListParam() { name = "Text", ordinal = 1, type = ParamType.charString });
                string resp = SqlLists.SqlQuery(query, parms, connectionString);

                if (string.IsNullOrEmpty(resp))
                {
                    // If no detail record exists create an empty entry with reasonable defaults.
                     return AddEventDetail(eventId,
                                          GlobalSettingsApi.GetInstance(connectionString).CourseId,
                                          1,
                                          12, // 18 holes for default
                                          false, // not shotgun start
                                          "unknown", // sponsor name
                                          GetPlayListIdByLabel("1-18", connectionString), // 
                                          1,
                                          1,
                                          connectionString);
                }
                else
                {
                    KeyValuePair kvp = JsonConvert.DeserializeObject<KeyValuePair>(resp);
                    return int.Parse(kvp.Value);
                }
            } catch (Exception ex)
            {
                GolfDB2Logger.LogError("LookupOrCreateEventDetailRecord", ex.ToString());
            }

            return -1;
        }

        public int GetPlayListIdByLabel(string label, string connectionString)
        {
            // using EventId lookup EventDetails Id
            string query = string.Format("SELECT Id, Label from HoleList WHERE Label LIKE '{0}%'", label);
            List<SqlListParam> parms = new List<SqlListParam>();
            parms.Add(new SqlListParam() { name = "Value", ordinal = 0, type = ParamType.int32 });
            parms.Add(new SqlListParam() { name = "Text", ordinal = 1, type = ParamType.charString });
            string resp = SqlLists.SqlQuery(query, parms, connectionString);

            KeyValuePair kvp = JsonConvert.DeserializeObject<KeyValuePair>(resp);
            return int.Parse(kvp.Value);
        }

        public int AddEventDetail(int eventId, 
                                  int courseId, 
                                  int playFormat, 
                                  int numberOfHoles, 
                                  bool isShotgunStart, 
                                  string sponsor, 
                                  int playListId, 
                                  int orgId,
                                  int startHoleId,
                                  string connectionString)
        {
            EventDetailDataContext db = null;

            if (db == null)
            {
                if (!string.IsNullOrEmpty(connectionString))
                    db = new EventDetailDataContext(connectionString);
                else
                    db = new EventDetailDataContext();
            }

            EventDetail obj = null;

            try
            {
                obj = new EventDetail() {
                    EventId = eventId,
                    CourseId = courseId,
                    PlayFormat = playFormat,
                    NumberOfHoles = numberOfHoles,
                    IsShotgunStart = isShotgunStart,
                    Sponsor = sponsor,
                    PlayListId = playListId,
                    OrgId = orgId,
                    StartHoleId = startHoleId
                };

                db.EventDetails.InsertOnSubmit(obj);
                db.SubmitChanges();
                return obj.Id;
            }
            catch (Exception ex)
            {
                GolfDB2Logger.LogError("AddEventDetail", ex.ToString());
            }

            return -1;
        }
    }
}