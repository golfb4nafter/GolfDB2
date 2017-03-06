using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GolfDB2.Tools
{
    public class HorizontalMobileScoreCardFactory : MobileScoreCardFactoryBase
    {
        public string makeHorizontalMobileScoreCardHtml(string connectionString)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("    <table border=\"1\">\r\n");
            sb.Append("      <tr><th>Event</th><th>Team/Player</th><th>Tee Box</th><th>Gender</th></tr>\r\n");
            sb.Append("      <tr><td>" + makeEventSelect(null) + 
                                         "</td><td>" + 
                                         makeTeamSelect(0, connectionString) + 
                                         "</td><td>" + 
                                         makeTeeBoxSelect(null) + 
                                         "</td><td>" + 
                                         makeGenderSelect() +
                                         "</td></tr>\r\n");
            sb.Append("    </table>\r\n");
            sb.Append("    <br />\r\n");
            sb.Append("    <div id=\"scoresDiv\"/>\r\n");

            return sb.ToString();
        }

        public string makeScoresDiv(int cardId, int eventId, int ordinal, string gender, string connectionString)
        {
            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);

            int total = 0;
            int yards = 0;
            int net = 0;
            string tbStyle = ordinal != 1 ? "" : " style=\"color:white;background-color:red\"";
            int strokesGiven = TeeTimeTools.GetScoreCard(cardId, null).Handicap;

            List<ScoreEntry> scoreList = db.ScoreEntries.Where(w => w.ScoreCardId == cardId).ToList();

            StringBuilder sb = new StringBuilder();
            StringBuilder sbHoleRow = new StringBuilder();
            StringBuilder sbYardsRow = new StringBuilder();
            StringBuilder sbHcpRow = new StringBuilder();
            StringBuilder sbScoresRow = new StringBuilder();
            StringBuilder sbNetRow = new StringBuilder();

            sb.Append("    <table border=\"1\">\r\n");

            sb.Append("      <tr style=\"color:white;background-color:green\" ><th>Hole</th>");
            sbYardsRow.Append("      <tr" + tbStyle + "><th>Yards</th>");
            sbHcpRow.Append("      <tr style=\"background-color:orange\"><th>HCP</th>");
            sbScoresRow.Append("      <tr><th>Score</th>");
            sbNetRow.Append("      <tr><th >Net</th>");

            foreach (ScoreEntry entry in scoreList)
            {
                int holeNumber = MiscLists.GetHoleNumberByHoleId(entry.HoleId, connectionString);
                int handicap = 0;
                string handicapByHoleList = null;

                sbHoleRow.Append(string.Format("        <th style=\"text-align:center\">{0}</th>\r\n", holeNumber));

                // lookup yards
                if (ordinal != -1)
                {
                    GeoData d = TeeTimeTools.GetGeoData(1,
                                                        entry.HoleId,
                                                        GlobalSettingsApi.GetInstance().CourseId,
                                                        ordinal,
                                                        null);

                    EventDetail detail = TeeTimeTools.GetEventDetail(eventId, null);
                    TeeBoxMenuColor color = TeeTimeTools.GetTeeBoxMenuColors(GlobalSettingsApi.GetInstance().CourseId, ordinal, null);

                    handicap = TeeTimeTools.GetHoleHandicap(GlobalSettingsApi.GetInstance().CourseId,
                                         color.Id,
                                         gender,
                                         detail.PlayListId,
                                         holeNumber,
                                         ref handicapByHoleList,
                                         null);

                    sbYardsRow.Append(string.Format("        <th style=\"text-align:center\">{0}</th>\r\n", d.YardsToFront));  // yards

                    yards += d.YardsToFront.Value;
                }
                else
                    sbYardsRow.Append(string.Format("        <th style=\"text-align:center\"></th>\r\n"));  // yards

                sbHcpRow.Append(string.Format("        <th style=\"text-align:center\">{0}</th>\r\n", handicap));  // handicap

                sbScoresRow.Append(string.Format("        <td style=\"text-align:center\"><input style=\"width: 20px!important;\" type=\"text\" maxlength=\"2\" id=\"Hole_{0}_{1}\" name=\"Hole_{0}_{1}\" value=\"{2}\" onchange=\"updateScore('Hole_{0}_{1}');\" /></td>\r\n", cardId, entry.Id, entry.Score));

                total += entry.Score;

                int netScore = TeeTimeTools.CalculateNetFromScore(strokesGiven, entry.Score, holeNumber, handicapByHoleList);

                net += netScore;

                if (netScore < entry.Score)
                    sbNetRow.Append("        <td style=\"text-align:center;background-color:yellow\">" + netScore + "</td>");
                else
                    sbNetRow.Append("        <td style=\"text-align:center\">" + netScore + "</td>");
            }

            sb.Append(sbHoleRow.ToString() + "<th>Total</th></tr>\r\n");
            sb.Append(sbYardsRow.ToString() + "<th>" + yards + "</th></tr>\r\n");
            sb.Append(sbHcpRow.ToString() + "<th></th></tr>\r\n");
            sb.Append(sbScoresRow.ToString() + "<th>" + total + "</th></tr>\r\n");
            sb.Append(sbNetRow.ToString() + "<th>" + net + "</th></tr>\r\n");
            sb.Append("    </table>\r\n");

            return sb.ToString();
        }
    }
}
