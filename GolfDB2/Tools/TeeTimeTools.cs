using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GolfDB2.Models;
using Newtonsoft.Json;

namespace GolfDB2.Tools
{
    public class TeeTimeTools
    {
        public static EventDetail GetEventDetail(int eventId, string connectionString)
        {
            EventDetailDataContext db = null;

            if (!string.IsNullOrEmpty(connectionString))
                db = new EventDetailDataContext(connectionString);
            else
                db = new EventDetailDataContext();

            return (from ed in db.GetTable<EventDetail>() where ed.EventId == eventId select ed).SingleOrDefault();
        }

        public static Event GetCalendarEvent(int id, string connectionString)
        {
            CalendarDataContext db = null;

            if (!string.IsNullOrEmpty(connectionString))
                db = new CalendarDataContext(connectionString);
            else
                db = new CalendarDataContext();

            return (from e in db.GetTable<Event>() where e.id == id select e).SingleOrDefault();
        }

        public static TeeTime GetTeeTime(int id, string connectionString)
        {
            TeeTimeDataContext db = null;
            TeeTime tt = null;

            if (!string.IsNullOrEmpty(connectionString))
                db = new TeeTimeDataContext(connectionString);
            else
                db = new TeeTimeDataContext();

            return (from c in db.GetTable<TeeTime>() where c.Id == id select c).SingleOrDefault();
        }

        public static TeeTime GetTeeTime(int eventId, int teeTimeOffset, string connectionString)
        {
            TeeTimeDataContext db = null;
            TeeTime tt = null;

            if (!string.IsNullOrEmpty(connectionString))
                db = new TeeTimeDataContext(connectionString);
            else
                db = new TeeTimeDataContext();

            return (from c in db.GetTable<TeeTime>() where (c.EventId == eventId && c.TeeTimeOffset == teeTimeOffset) select c).SingleOrDefault();
        }
       
        public static string GetStartingHoleByHoleId(int eventId, int ordinal, int holeId, string connectionString)
        {
            // Dereference holeId to HoleNumber.
            Hole hole = ShotgunHoleCalculator.GetHoleById(holeId, connectionString);

            EventDetail detail = GetEventDetail(eventId, connectionString);
            
            // Get the number of holes to be played
            HoleList hl = ShotgunHoleCalculator.GetHoleListById(detail.PlayListId, connectionString);

            // the ordinal value is the offset into the HoleList array for holes 1-n
            // after 1-n+1 we assign as ordered in the BList for b,c,d teams
            string[] holeList = hl.HoleList1.Split(',');
            int numHoles = holeList.Length;

            string bStr = "";

            if ((ordinal + 1) > numHoles)
                bStr = "B";

            return hole.Number.ToString() + bStr;
        }

        public static List<TeeTime> MakeTeeTimes(int eventId, string connectionString)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            GolfDB2Logger.LogDebug("MakeTeeTimes", "(" + eventId.ToString() + ")");

            List<TeeTime> items = new List<TeeTime>();
            Event evt = GetCalendarEvent(eventId, connectionString);

            if (evt == null)
            {
                GolfDB2Logger.LogError("MakeTeeTimes", "Event not found.");
                return items;
            }

            DateTime curDateTime = evt.start;
            EventDetail detail = GetEventDetail(eventId, connectionString);

            if (detail == null)
            {
                GolfDB2Logger.LogError("MakeTeeTimes", "Event detail not found.");
                return items;
            }

            for (int i = 0; i < 60; i++)
            {
                // Do not exceed the window or number of tee times
                if (detail.IsShotgunStart && (i > (detail.NumGroups - 1)))
                    break;

                if (!detail.IsShotgunStart && (curDateTime.AddMinutes(i * 10) >= evt.end || i > (detail.NumGroups - 1)))
                    break;

                TeeTime tt = new TeeTime();
                tt.CourseId = GlobalSettingsApi.GetInstance(connectionString).CourseId;
                tt.EventId = eventId;


                if (!detail.IsShotgunStart)
                    tt.HoleId = detail.StartHoleId;  // need to resolve to label
                else
                {
                    // assign holeId in ordinal value order
                    // when ordinal is > number of holes
                    // assign as b/c/d group in order on par 4s and 5s as designated.
                    tt.HoleId = ShotgunHoleCalculator.GetHoleIdByOrdinalAndEventId(i, detail.PlayListId, eventId, connectionString);
                }

                tt.NumberOfPlayers = detail.NumPerGroup;
                tt.PlayerNames = "TBD";
                tt.ReservedByName = detail.Sponsor;
                tt.TeeTimeOffset = i;

                if (detail.IsShotgunStart)
                    tt.Tee_Time = evt.start;
                else
                    tt.Tee_Time = curDateTime.AddMinutes(i * 10);

                tt.TelephoneNumber = "n/a";

                // Get tee time where eventId and tee time offset 
                TeeTime ttTmp = GetTeeTime(eventId, i, connectionString);

                // If found fix up with current times and add to list.
                if (ttTmp != null)
                {
                    tt.Id = ttTmp.Id;
                    items.Add(UpdateTeeTime(tt, connectionString));
                }
                else
                {
                    // If none found do insert. And add to list.
                    // Insert into table and add the id to tt object
                    InsertTeeTime(tt, connectionString);
                    ttTmp = GetTeeTime(eventId, i, connectionString);
                    items.Add(ttTmp);
                }
            }

            return items;
        }

        public static void InsertTeeTime(TeeTime tt, string connectionString)
        {
            TeeTimeDataContext db = null;

            if (!string.IsNullOrEmpty(connectionString))
                db = new TeeTimeDataContext(connectionString);
            else
                db = new TeeTimeDataContext();

            try
            {
                db.TeeTimes.InsertOnSubmit(tt);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DeleteTeeTimes(int eventId, string connectionString)
        {
            TeeTimeDataContext db = null;

            if (!string.IsNullOrEmpty(connectionString))
                db = new TeeTimeDataContext(connectionString);
            else
                db = new TeeTimeDataContext();

            try
            {
                var listToRemove = (from a in db.TeeTimes
                                    where a.EventId == eventId
                                    select a).ToList();
                db.TeeTimes.DeleteAllOnSubmit(listToRemove);
                db.SubmitChanges();
                return true;
            } catch(Exception ex)
            {
                GolfDB2Logger.LogError("DeleteTeeTimes", ex.ToString());
                return false;
            }
        }

        public static TeeTime UpdateTeeTime(TeeTime tt, string connectionString)
        {
            TeeTimeDataContext db = null;

            if (!string.IsNullOrEmpty(connectionString))
                db = new TeeTimeDataContext(connectionString);
            else
                db = new TeeTimeDataContext();

            try
            {
                var teeTime = db.TeeTimes
                    .Where(w => w.Id == tt.Id)
                    .SingleOrDefault();

                if (teeTime != null)
                {
                    teeTime.EventId = tt.EventId;
                    teeTime.CourseId = tt.CourseId;
                    teeTime.HoleId = tt.HoleId;
                    teeTime.NumberOfPlayers = tt.NumberOfPlayers;
                    teeTime.PlayerNames = tt.PlayerNames;
                    teeTime.ReservedByName = tt.ReservedByName;
                    teeTime.TeeTimeOffset = tt.TeeTimeOffset;
                    teeTime.Tee_Time = tt.Tee_Time;
                    teeTime.TelephoneNumber = tt.TelephoneNumber;
                    db.SubmitChanges();

                    return teeTime;
                }
            }
            catch (Exception ex)
            {
                GolfDB2Logger.LogError("UpdateTeeTime", ex.ToString());
            }

            return null;
        }
    }
}