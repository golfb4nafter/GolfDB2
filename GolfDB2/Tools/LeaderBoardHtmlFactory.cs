using System;
using System.Text;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using GolfDB2.Models;
using Newtonsoft.Json;


namespace GolfDB2.Tools
{
    public class LeaderBoardHtmlFactory
    {
        public enum SortOnColumn
        {
            Ordinal,
            StartingHoleNumber,
            TotalScore,
            Division,
            Name
        }

        public static string MakeLeaderBoardTable(int eventId, SortOnColumn sortOn, string connectionString)
        {
            Event evt = EventDetailTools.GetEventRecord(eventId, connectionString);
            EventDetail eventDetail = EventDetailTools.GetEventDetailRecordByEventId(evt.id, connectionString);
            HoleList holeList = ShotgunHoleCalculator.GetHoleListById(eventDetail.PlayListId, connectionString);

            // make a list of the holes to be played
            List<int> holesToPlayList = new List<int>();

            foreach(string s in holeList.HoleList1.Split(','))
                holesToPlayList.Add(int.Parse(s));

            // build the header row.
            StringBuilder sbHead = new StringBuilder();

            int cnt = 0;
            foreach (int i in holesToPlayList)
            {
                // Make Table headers row first.
                if (cnt++ == 0)
                    sbHead.Append("    <tr><th>Time</th><th>Hole</th><th>Player/Team</th>");

                sbHead.Append(string.Format("<th>{0}</th>", i));

                if (i % 9 == 0)
                    sbHead.Append("<th>Total</th>");
            }

            sbHead.Append("<th>Division</th><th>HC</th><th>Gross</th><th>Net</th></tr>\r\n");

            List<SortableRowObject> htmlDetailRows = MakeLeaderBoardTable(holesToPlayList, evt, eventDetail, connectionString);

            StringBuilder sbTable = new StringBuilder();

            // Sort goes here

            if (sortOn == SortOnColumn.Name)
                htmlDetailRows.Sort(new SortNamesHelper());
            else if (sortOn == SortOnColumn.TotalScore)
                htmlDetailRows.Sort(new SortTotalScoreHelper());
            else if (sortOn == SortOnColumn.StartingHoleNumber)
                htmlDetailRows.Sort(new SortStartingHoleHelper());
            else if (sortOn == SortOnColumn.Division)
                htmlDetailRows.Sort(new SortDivisionHelper());
            else
                htmlDetailRows.Sort();

            sbTable.Append("  <table>\r\n");

            sbTable.Append(sbHead.ToString());

            foreach(SortableRowObject sro in htmlDetailRows)
                sbTable.Append(sro.HtmlRow);

            sbTable.Append("  </table>\r\n");

            return sbTable.ToString();
        }

        public static List<SortableRowObject> MakeLeaderBoardTable(List<int> holesToPlayList, Event evt, EventDetail eventDetail, string connectionString)
        {
            List<SortableRowObject> tableRows = new List<SortableRowObject>();
            List<TeeTime> teeTimes = TeeTimeTools.GetTeeTimeTimesByEventId(eventDetail.EventId, connectionString);

            int ordinal = 0;

            // foreach tee Time in event 
            foreach (TeeTime tt in teeTimes)
            {
                // Now get the tee time detail
                List<TeeTimeDetail> ttdList = TeeTimeTools.GetTeeTimeDetailList(eventDetail, tt, connectionString);

                int ttdCount = 0;

                foreach (TeeTimeDetail ttd in ttdList)
                {
                    if (++ttdCount > tt.NumberOfPlayers)
                        continue;

                    if (ttd.Name == "TBD")
                        continue;

                    // Get/Create the ScoreCard and ScoreCard Entries
                    ScoreCard card = new ScoreCard();
                    card.EventId = evt.id;
                    card.TeeTimeDetailId = ttd.Id;
                    card.StartingHole = tt.HoleId;
                    card.Division = ttd.Division;
                    card.Handicap = ttd.Handicap;
                    card.Names = ttd.Name;

                    card = TeeTimeTools.InsertOrUpdateScoreCard(card, connectionString);

                    StringBuilder sb = new StringBuilder();

                    SortableRowObject sro = new SortableRowObject()
                    {
                        Event = evt,
                        EventDetail = eventDetail,
                        Tee_Time = tt,
                        Card = card,
                        Ordinal = ordinal++,
                        RowTeeTime = tt.Tee_Time,
                        StartingHoleNumber = tt.HoleId,
                        Division = ttd.Division,
                        Handicap = card.Handicap,
                        Name = ttd.Name
                    };

                    int number = MiscLists.GetHoleNumberByHoleId(tt.HoleId, connectionString);
                    sb.Append(string.Format("    <tr>\r\n        <td>{0}</td>\r\n        <td>{1}</td>\r\n        <td>{2}</td>\r\n", tt.Tee_Time.ToString("hh:mm"), number, card.Names));

                    int total = 0;
                    int nineTotal = 0;

                    foreach (int i in holesToPlayList)
                    {
                        ScoreEntry entry = TeeTimeTools.GetAddScoreEntry(card.Id, 
                                                                         i, 
                                                                         MiscLists.GetHoleIdByHoleNumber(GolfDB2.Tools.GlobalSettingsApi.GetInstance(connectionString).CourseId, i, connectionString),
                                                                         -1, // -1 = do not touch.
                                                                         connectionString);

                        sro.scores.Add(entry);
                        sb.Append(string.Format("        <td><input style=\"width: 20px!important;\" type=\"text\" maxlength=\"2\" id=\"Hole_{0}_{1}\" name=\"Hole_{0}_{1}\" value=\"{2}\" onchange=\"updateScore('Hole_{0}_{1}');\" /></td>\r\n", card.Id, i, entry.Score));

                        total += entry.Score;
                        nineTotal += entry.Score;

                        if (i%9==0)
                        {
                            sb.Append(string.Format("        <td>{0}</td>\r\n", nineTotal));
                            nineTotal = 0;
                        }
                    }

                    sb.Append(string.Format("        <td>{0}</td>\r\n", card.Division));
                    sb.Append(string.Format("        <td>{0}</td>\r\n", card.Handicap));
                    sb.Append(string.Format("        <td>{0}</td>\r\n", total));
                    sb.Append(string.Format("        <td>{0}<input type=\"hidden\" id=\"dirty_{1}\" name=\"dirty_{1}\" value=\"false\" /></td>\r\n", total, card.Id));

                    sb.Append("    </tr>\r\n");
                    sro.TotalScore = total;
                    sro.HtmlRow = sb.ToString();

                    tableRows.Add(sro);
                }
            }

            return tableRows;
        }

        public static List<string> GetStartingHoleList(string holeList)
        {
            List<string> startingHoles = new List<string>();

            string[] holes = holeList.Split(',');

            foreach (string s in holes)
            {
                if (s == "1")
                    startingHoles.Add(s);

                if (s == "10")
                    startingHoles.Add(s);

                if (s == "19")
                {
                    startingHoles.Add(s);
                    break;
                }
            }

            return startingHoles;
        }

    }
}