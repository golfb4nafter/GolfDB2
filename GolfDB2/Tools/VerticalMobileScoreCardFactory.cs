using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GolfDB2.Tools
{
    public class VerticalMobileScoreCardFactory : MobileScoreCardFactoryBase
    {
        public string makeVerticalMobileScoreCardHtml(string connectionString)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("    <table border=\"1\">\r\n");
            sb.Append("      <tr>\r\n");
            sb.Append("        <th>Event</th>\r\n");
            sb.Append("        <td>" + makeEventSelect(null) + "</td>\r\n");
            sb.Append("      </tr>\r\n");
            sb.Append("      <tr>\r\n");
            sb.Append("        <th>Team/Player</th>\r\n");
            sb.Append("        <td>\r\n");
            sb.Append(makeTeamSelect(0, connectionString));
            sb.Append("        </td>\r\n");
            sb.Append("      </tr>\r\n");
            sb.Append("      <tr>\r\n");
            sb.Append("        <th>Tee Box</th>\r\n");
            sb.Append("        <td>" + makeTeeBoxSelect(null) + "</td>\r\n");
            sb.Append("      </tr>\r\n");
            sb.Append("      <tr>\r\n");
            sb.Append("        <th>Gender</th>\r\n");
            sb.Append("      <td><select onchange=\"updateCardSelect();\" id=\"genderSelect\" name=\"genderSelect\" >\r\n");
            sb.Append("        <option selected value=\"M\">Male</ option >\r\n");
            sb.Append("        <option value=\"F\">Female</ option >\r\n");
            sb.Append("      </select></td>\r\n");
            sb.Append("      </tr>\r\n");
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

            sb.Append("    <table border=\"1\">\r\n");

            sb.Append("      <tr>\r\n");
            sb.Append("        <th>Hole</th>\r\n");
            sb.Append("        <th>Yards</th>\r\n");
            sb.Append("        <th>HCP</th>\r\n");
            sb.Append("        <th>Score</th>\r\n");
            sb.Append("      </tr>\r\n");

            foreach (ScoreEntry entry in scoreList)
            {
                int holeNumber = MiscLists.GetHoleNumberByHoleId(entry.HoleId, connectionString);
                sb.Append("      <tr>\r\n");
                sb.Append(string.Format("        <th>{0}</th>\r\n", holeNumber));

                // lookup yards
                if (ordinal != -1)
                {
                    GeoData d = TeeTimeTools.GetGeoData(1,
                                                        entry.HoleId,
                                                        GlobalSettingsApi.GetInstance().CourseId,
                                                        ordinal,
                                                        null);
                    sb.Append(string.Format("        <th>{0}</th>\r\n", d.YardsToFront));  // yards
                }
                else
                    sb.Append(string.Format("        <th></th>\r\n"));  // yards

                sb.Append(string.Format("        <th>{0}</th>\r\n", holeNumber));  // handicap
                sb.Append(string.Format("        <td><input style=\"width: 20px!important;\" type=\"text\" maxlength=\"2\" id=\"Hole_{0}_{1}\" name=\"Hole_{0}_{1}\" value=\"{2}\" onchange=\"updateScore('Hole_{0}_{1}');\" /></td>\r\n", cardId, entry.Id, entry.Score));
                sb.Append("      </tr>\r\n");
            }

            sb.Append("    </table>\r\n");

            return sb.ToString();
        }
    }
}