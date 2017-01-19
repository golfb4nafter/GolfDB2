using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GolfDB2.Models;
using Newtonsoft.Json;

namespace GolfDB2.Tools
{
    public static class TeeTimeTools
    {
        public static EventDetail GetEventDetail(int eventId, string connectionString)
        {
            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);
            return (from ed in db.GetTable<EventDetail>() where ed.EventId == eventId select ed).SingleOrDefault();
        }

        public static Event GetCalendarEvent(int id, string connectionString)
        {
            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);
            return (from e in db.GetTable<Event>() where e.id == id select e).SingleOrDefault();
        }

        public static TeeTime GetTeeTime(int id, string connectionString)
        {
            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);
            return (from c in db.GetTable<TeeTime>() where c.Id == id select c).SingleOrDefault();
        }

        public static TeeTime GetTeeTime(int eventId, int teeTimeOffset, string connectionString)
        {
            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);
            return (from c in db.GetTable<TeeTime>() where (c.EventId == eventId && c.TeeTimeOffset == teeTimeOffset) select c).SingleOrDefault();
        }

        public static TeeTimeDetail GetTeeTimeDetailByTeeTimeId(int ttdId, int ordinal, string connectionString)
        {
            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);
            List<TeeTimeDetail> ttdList = (from c in db.GetTable<TeeTimeDetail>()
                                            where c.TeeTimeId == ttdId
                                            orderby c.Id
                                            select c).ToList<TeeTimeDetail>();

            if (ttdList != null && ordinal < ttdList.Count)
                return ttdList[ordinal];

            return null;
        }

        public static List<TeeTime> GetTeeTimeTimesByEventId(int eventId, string connectionString)
        {
            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);
            return (from c in db.GetTable<TeeTime>()
                    where c.EventId == eventId
                    orderby c.TeeTimeOffset
                    select c).ToList<TeeTime>();
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

        public static int GetMaxTeamsByPlayListId(int playListId, string connectionString)
        {
            // We need the HoleList and BList from HoleList table.
            HoleList hl = ShotgunHoleCalculator.GetHoleListById(playListId, connectionString);

            // the ordinal value is the offset into the HoleList array for holes 1-n
            // after 1-n+1 we assign as ordered in the BList for b,c,d teams
            string[] holeList = hl.HoleList1.Split(',');

            string[] bList;
            int numBHoles = 0;

            if (!string.IsNullOrEmpty(hl.BList))
            {
                bList = hl.BList.Split(',');
                numBHoles = bList.Length;
            }

            int numHoles = holeList.Length;
            return (numHoles + numBHoles);
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

            // Calculate the max number of teams.
            int maxTeeTimes = GetMaxTeamsByPlayListId(detail.PlayListId, connectionString);

            for (int i = 0; i < maxTeeTimes; i++)
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

                tt.TeeTimeOffset = i;

                if (detail.IsShotgunStart)
                    tt.Tee_Time = evt.start;
                else
                    tt.Tee_Time = curDateTime.AddMinutes(i * 10);

                tt.TelephoneNumber = "n/a";
                tt.NumberOfPlayers = detail.NumPerGroup;
                tt.PlayerNames = "TBD";
                tt.ReservedByName = detail.Sponsor;


                // Get tee time where eventId and tee time offset 
                TeeTime ttTmp = GetTeeTime(eventId, i, connectionString);

                // If found fix up with current times and add to list.
                if (ttTmp != null)
                {
                    tt.Id = ttTmp.Id;

                    tt.PlayerNames = ttTmp.PlayerNames;
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
            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);

            try
            {
                db.TeeTimes.InsertOnSubmit(tt);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                GolfDB2Logger.LogError("InsertTeeTime", ex.ToString());
            }
        }

        public static bool DeleteTeeTimes(int eventId, string connectionString)
        {
            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);

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


        public static bool DeleteTeeTimeDetailById(int id, string connectionString)
        {
            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);

            try
            {
                var listToRemove = (from a in db.TeeTimeDetails
                                    where a.TeeTimeId == id
                                    select a).ToList();
                db.TeeTimeDetails.DeleteAllOnSubmit(listToRemove);
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                GolfDB2Logger.LogError("DeleteTeeTimeDetailById", ex.ToString());
                return false;
            }
        }

        public static bool DeleteTeeTimeById(int id, string connectionString)
        {
            DeleteTeeTimeDetailById(id, connectionString);

            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);

            try
            {
                var listToRemove = (from a in db.TeeTimes
                                    where a.Id == id
                                    select a).ToList();
                db.TeeTimes.DeleteAllOnSubmit(listToRemove);
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                GolfDB2Logger.LogError("DeleteTeeTimeById", ex.ToString());
                return false;
            }
        }

        public static TeeTimeDetail UpdateTeeTimeDetail(TeeTimeDetail ttd, string connectionString)
        {
            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);

            try
            {
                var teeTimeDetail = db.TeeTimeDetails
                    .Where(w => w.Id == ttd.Id)
                    .SingleOrDefault();

                if (teeTimeDetail != null)
                {
                    teeTimeDetail.AmountPaid = ttd.AmountPaid;
                    teeTimeDetail.Cart = ttd.Cart;
                    teeTimeDetail.Division = ttd.Division;
                    teeTimeDetail.Name = ttd.Name;
                    teeTimeDetail.Pass = ttd.Pass;
                    db.SubmitChanges();

                    return teeTimeDetail;
                }
            }
            catch (Exception ex)
            {
                GolfDB2Logger.LogError("UpdateTeeTimeDetail", ex.ToString());
            }

            return null;
        }


        public static ScoreCard InsertOrUpdateScoreCard(ScoreCard card, string connectionString)
        {
            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);

            try
            {
                var scoreCard = db.ScoreCards
                    .Where(w => w.TeeTimeDetailId == card.TeeTimeDetailId)
                    .SingleOrDefault();

                if (scoreCard != null)
                {
                    scoreCard.EventId = card.EventId;
                    scoreCard.StartingHole = card.StartingHole;
                    scoreCard.Division = card.Division;
                    scoreCard.Names = card.Names;
                    db.SubmitChanges();
                    return scoreCard;
                }
                else
                {
                    db.ScoreCards.InsertOnSubmit(card);
                    db.SubmitChanges();
                    return card;
                }
            }
            catch (Exception ex)
            {
                GolfDB2Logger.LogError("InsertOrUpdateScoreCard", ex.ToString());
            }

            return null;
        }

        public static ScoreEntry GetAddScoreEntry(int scoreCardId, int ordinal, int holeId, string connectionString)
        {
            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);

            try
            {
                var scoreEntry = db.ScoreEntries
                    .Where(w => (w.ScoreCardId == scoreCardId && w.Ordinal == ordinal))
                    .SingleOrDefault();

                if (scoreEntry != null)
                {
                    return scoreEntry;
                }
                else
                {
                    ScoreEntry entry = new ScoreEntry()
                    {
                        ScoreCardId = scoreCardId,
                        Score = 0,
                        Ordinal = ordinal,
                        HoleId = holeId
                    };

                    db.ScoreEntries.InsertOnSubmit(entry);
                    db.SubmitChanges();
                    return entry;
                }
            }
            catch (Exception ex)
            {
                GolfDB2Logger.LogError("InsertOrUpdateScoreCard", ex.ToString());
            }

            return null;
        }

        public static TeeTime UpdateTeeTime(TeeTime tt, string connectionString)
        {
            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);

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

        public static List<TeeTimeDetail> GetTeeTimeDetailList(EventDetail eventDetail, TeeTime tt, string connectionString)
        {
            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);

            List<TeeTimeDetail> ttd = (from c in db.GetTable<TeeTimeDetail>()
                    where c.TeeTimeId == tt.Id
                    orderby c.Id
                    select c).ToList<TeeTimeDetail>();

            // Group event only has one scorecard
            if (!IsStrokePlayFormat(eventDetail.PlayFormat))
            {
                if (ttd.Count == 1)
                    return ttd;

                TeeTimeDetail d = new TeeTimeDetail() {
                    AmountPaid = 0,
                    Name = tt.PlayerNames,
                    Pass = false,
                    Cart = false,
                    TeeTimeId = tt.Id
                };

                try
                {
                    db.TeeTimeDetails.InsertOnSubmit(d);
                    db.SubmitChanges();
                    ttd.Add(d);
                    return ttd;
                }
                catch (Exception ex)
                {
                    GolfDB2Logger.LogError("GetTeeTimeDetailList", ex.ToString());
                }
            }

            // Check to see if we have detail and the proper number
            if (ttd == null)
                ttd = new List<TeeTimeDetail>();

            if (ttd.Count < tt.NumberOfPlayers)
            {
                string[] golfers = tt.PlayerNames.Split(',');

                for (int i=0;i<tt.NumberOfPlayers; i++)
                {
                    string name = "TBD";

                    if (golfers != null && i <golfers.Length)
                        name = golfers[i];

                    // If it's already there ignore it
                    if (ttd.Count > i)
                        continue;

                    // Create and insert new tee time detail record.
                    TeeTimeDetail d = new TeeTimeDetail()
                    {
                        AmountPaid = 0,
                        Name = name,
                        Pass = false,
                        Cart = false,
                        TeeTimeId = tt.Id,
                        Division = "open"
                    };

                    try
                    {
                        db.TeeTimeDetails.InsertOnSubmit(d);
                        db.SubmitChanges();
                        ttd.Add(d);
                    }
                    catch (Exception ex)
                    {
                        GolfDB2Logger.LogError("GetTeeTimeDetailList", ex.ToString());
                    }
                }
            }

            return ttd;
        }

        public static bool IsStrokePlayFormat(int playFormat)
        {
            // Quick and dirty for testing
            // Todo: needs to query the label table poroperly

            // 7   1   0   PlayFormat Stroke Play Stroke play.
            // 8   1   1   PlayFormat Best Shot Best shot.
            // 9   1   2   PlayFormat Best Ball Best Ball.
            // 10  1   3   PlayFormat Alternate Shot Alternate Shot.
            // 11  1   4   PlayFormat Blind   Blind.

            if (playFormat == 7 || playFormat == 9 || playFormat == 11)
                return true;

            return false;
        }
    }
}