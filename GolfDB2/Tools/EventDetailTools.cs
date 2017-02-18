using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using GolfDB2.Models;

namespace GolfDB2.Tools
{
    public static class EventDetailTools
    {
        public static int LookupOrCreateEventDetailRecord(int eventId, string connectionString)
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
                                          0, // 18 holes for default
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
                Logger.LogError("LookupOrCreateEventDetailRecord", ex.ToString());
            }

            return -1;
        }

        public static int GetPlayListIdByLabel(string label, string connectionString)
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

        public static GolfDB2DataContext GetDB(string connectionString)
        {
            if (!string.IsNullOrEmpty(connectionString))
                return new GolfDB2DataContext(connectionString);
            else
                return new GolfDB2DataContext();
        }

        public static Event GetEventRecord(int id, string connectionString)
        {
            GolfDB2DataContext db = GetDB(connectionString);
            return (from c in db.GetTable<Event>() where c.id == id select c).SingleOrDefault();
        }

        public static EventDetail GetEventDetailRecord(int id, string connectionString)
        {
            GolfDB2DataContext db = GetDB(connectionString);
            return (from c in db.GetTable<EventDetail>() where c.Id == id select c).SingleOrDefault();
        }

        public static EventDetail GetEventDetailRecordByEventId(int eventId, string connectionString)
        {
            GolfDB2DataContext db = GetDB(connectionString);
            return (from c in db.GetTable<EventDetail>() where c.EventId == eventId select c).SingleOrDefault();
        }

        public static TeeTime GetTeeTime(int eventId, int teeTimeOffset, string connectionString)
        {
            GolfDB2DataContext db = GetDB(connectionString);
            return (from c in db.GetTable<TeeTime>() where (c.EventId == eventId && c.TeeTimeOffset == teeTimeOffset) select c).SingleOrDefault();
        }

        public static int AddEventDetail(int eventId, 
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
            GolfDB2DataContext db = GetDB(connectionString);

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
                Logger.LogError("AddEventDetail", ex.ToString());
            }

            return -1;
        }

        public static bool EventDetailUpdate(EventDetail eventDetail, string connectionString)
        {
            GolfDB2DataContext db = GetDB(connectionString);

            try
            {
                var ed = (from c in db.GetTable<EventDetail>() where c.Id == eventDetail.Id select c).SingleOrDefault();

                if (ed != null)
                {
                    ed.Id = eventDetail.Id;
                    ed.EventId = eventDetail.EventId;
                    ed.CourseId = eventDetail.CourseId;
                    ed.PlayFormat = eventDetail.PlayFormat;
                    ed.NumberOfHoles = eventDetail.NumberOfHoles;
                    ed.IsShotgunStart = eventDetail.IsShotgunStart;
                    ed.Sponsor = eventDetail.Sponsor;
                    ed.PlayListId = eventDetail.PlayListId;
                    ed.OrgId = eventDetail.OrgId;
                    ed.StartHoleId = eventDetail.StartHoleId;
                    ed.NumGroups = eventDetail.NumGroups;
                    ed.NumPerGroup = eventDetail.NumPerGroup;
                    db.SubmitChanges();
                }
            }
            catch (Exception exInner)
            {
                Logger.LogError("EventDetailUpdate", exInner.ToString());
                return false;
            }

            return true;
        }
    }
}