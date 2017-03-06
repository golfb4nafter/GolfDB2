using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GolfDB2.Tools
{
    public class VerticalMobileScoreCardFactory : MobileScoreCardFactoryBase
    {
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

            sb.Append("    <table border=\"1\">\r\n");

            sb.Append("      <tr>\r\n");
            sb.Append("        <th style=\"color:white;background-color:green\">Hole</th>\r\n");
            sb.Append("        <th" + tbStyle + ">Yards</th>\r\n");
            sb.Append("        <th style=\"background-color:orange\">HCP</th>\r\n");
            sb.Append("        <th>Score</th>\r\n");
            sb.Append("        <th>Net</th>\r\n");
            sb.Append("      </tr>\r\n");

            foreach (ScoreEntry entry in scoreList)
            {
                int holeNumber = MiscLists.GetHoleNumberByHoleId(entry.HoleId, connectionString);
                int handicap = 0;
                string handicapByHoleList = null;

                sb.Append("      <tr>\r\n");
                sb.Append(string.Format("        <th style=\"text-align:center;color:white;background-color:green\">{0}</th>\r\n", holeNumber));

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

                    sb.Append(string.Format("        <th" + tbStyle + ">{0}</th>\r\n", d.YardsToFront));  // yards
                    yards += d.YardsToFront.Value;
                }
                else
                    sb.Append(string.Format("        <th" + tbStyle + "></th>\r\n"));  // yards

                sb.Append(string.Format("        <th style=\"text-align:center;background-color:orange\">{0}</th>\r\n", handicap));  // handicap
                sb.Append(string.Format("        <td style=\"text-align:center\"><input style=\"width: 20px!important;\" type=\"text\" maxlength=\"2\" id=\"Hole_{0}_{1}\" name=\"Hole_{0}_{1}\" value=\"{2}\" onchange=\"updateScore('Hole_{0}_{1}');\" /></td>\r\n", cardId, entry.Id, entry.Score));

                total += entry.Score;

                int netScore = TeeTimeTools.CalculateNetFromScore(strokesGiven, entry.Score, holeNumber, handicapByHoleList);

                net += netScore;

                if (netScore < entry.Score)
                    sb.Append("        <td style=\"text-align:center;background-color:yellow\">" + netScore + "</td>");
                else
                    sb.Append("        <td style=\"text-align:center\">" + netScore + "</td>");

                sb.Append("      </tr>\r\n");
            }

            sb.Append("<tr><th style=\"color:white;background-color:green;text-align:center\">Tot</th><th" + tbStyle + ">" + yards + "</th><th style=\"background-color:orange\"></th><th style=\"text-align:center\">" + total + "</th><th style=\"text-align:center\">" + net + "</th></tr>\r\n");

            sb.Append("    </table>\r\n");

            return sb.ToString();
        }
    }
}