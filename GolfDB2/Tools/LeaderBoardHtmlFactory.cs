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
                    sbHead.Append("<tr><th>Time</th><th>Hole</th><th>Player/Team</th>");

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

            sbTable.Append("<table>\r\n");

            sbTable.Append(sbHead.ToString());

            foreach(SortableRowObject sro in htmlDetailRows)
                sbTable.Append(sro.HtmlRow);

            sbTable.Append("</table>\r\n");

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
                        Name = ttd.Name
                    };

                    int number = MiscLists.GetHoleNumberByHoleId(tt.HoleId, connectionString);
                    sb.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td>", tt.Tee_Time.ToString("hh:mm"), number, card.Names));

                    int total = 0;
                    int nineTotal = 0;

                    foreach (int i in holesToPlayList)
                    {
                        ScoreEntry entry = TeeTimeTools.GetAddScoreEntry(card.Id, i, tt.HoleId, connectionString);
                        sro.scores.Add(entry);
                        sb.Append(string.Format("<td><input style=\"width: 20px!important;\" type=\"text\" maxlength=\"2\" id=\"Hole{0}_{1}\" name=\"Hole{0}_{1}\" value=\"{2}\" /></td>", card.Id, i, entry.Score));

                        if (i%9==0)
                        {
                            sb.Append(string.Format("<td><input style=\"width: 30px!important;\" type=\"text\" id=\"Total{0}\" name=\"Total{0}\" value=\"{1}\" /></td>", card.Id, nineTotal));
                            nineTotal = 0;
                        }

                        total += entry.Score;
                        nineTotal += entry.Score;
                    }

                    sb.Append(string.Format("<td><input style=\"width: 45px!important;\" type=\"text\" id=\"Division_{0}\" name=\"Division_{0}\" value=\"{1}\"/></td>", card.Id, card.Division));
                    sb.Append(string.Format("<td><input style=\"width: 45px!important;\" type=\"text\" id=\"Handicap_{0}\" name=\"Handicap_{0}\" value=\"{1}\"/></td>", card.Id, 0));
                    sb.Append(string.Format("<td><input style=\"width: 30px!important;\" type=\"text\" id=\"Gross_{0}\" name=\"Gross_{0}\" value=\"{1}\"/></td>", card.Id, total));
                    sb.Append(string.Format("<td><input style=\"width: 30px!important;\" type=\"text\" id=\"Net_{0}\" name=\"Net_{0}\" value=\"{1}\"/></td>", card.Id, total));

                    sb.Append("</tr>");
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