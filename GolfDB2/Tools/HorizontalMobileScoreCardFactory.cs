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
            sb.Append("      <tr><td>" + makeEventSelect(null) + "</td><td>" + makeTeamSelect(0, connectionString) + "</td><td>" + makeTeeBoxSelect(null) + "</td><td>" + makeGenderSelect() + "</td></tr>\r\n");
            sb.Append("    </table>\r\n");
            sb.Append("    <br />\r\n");
            sb.Append("    <div id=\"scoresDiv\"/>\r\n");

            return sb.ToString();
        }
        public string makeScoresDiv(int cardId, int ordinal, string gender, string connectionString)
        {
            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);

            List<ScoreEntry> scoreList = db.ScoreEntries.Where(w => w.ScoreCardId == cardId).ToList();

            StringBuilder sb = new StringBuilder();
            StringBuilder sbHoleRow = new StringBuilder();
            StringBuilder sbYardsRow = new StringBuilder();
            StringBuilder sbHcpRow = new StringBuilder();
            StringBuilder sbScoresRow = new StringBuilder();

            sb.Append("    <table border=\"1\">\r\n");

            string holeRow =  "      <tr><th>Hole</th>";
            string yardsRow = "      <tr><th>Yards</th>";
            string hcpRow =   "      <tr><th>HCP</th>";
            string scoreRow = "      <tr><th>Score</th>";

            foreach (ScoreEntry entry in scoreList)
            {
                int holeNumber = MiscLists.GetHoleNumberByHoleId(entry.HoleId, connectionString);

                sbHoleRow.Append(string.Format("        <th>{0}</th>\r\n", holeNumber));

                // lookup yards
                if (ordinal != -1)
                {
                    GeoData d = TeeTimeTools.GetGeoData(1,
                                                        entry.HoleId,
                                                        GlobalSettingsApi.GetInstance().CourseId,
                                                        ordinal,
                                                        null);
                    sbYardsRow.Append(string.Format("        <th>{0}</th>\r\n", d.YardsToFront));  // yards
                }
                else
                    sbYardsRow.Append(string.Format("        <th></th>\r\n"));  // yards

                sbHcpRow.Append(string.Format("        <th>{0}</th>\r\n", holeNumber));  // handicap
                sbScoresRow.Append(string.Format("        <td><input style=\"width: 20px!important;\" type=\"text\" maxlength=\"2\" id=\"Hole_{0}_{1}\" name=\"Hole_{0}_{1}\" value=\"{2}\" onchange=\"updateScore('Hole_{0}_{1}');\" /></td>\r\n", cardId, entry.Id, entry.Score));
            }

            sb.Append(sbHoleRow.ToString() + "</tr>\r\n");
            sb.Append(sbYardsRow.ToString() + "</tr>\r\n");
            sb.Append(sbHcpRow.ToString() + "</tr>\r\n");
            sb.Append(sbScoresRow.ToString() + "</tr>\r\n");
            sb.Append("    </table>\r\n");

            return sb.ToString();
        }
    }
}
