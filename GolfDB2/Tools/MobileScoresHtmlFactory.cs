using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GolfDB2.Tools
{
    public class MobileScoresHtmlFactory
    {
        public static string makeMobileScoreCardHtml(string connectionString)
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
            sb.Append("    </table>\r\n");
            sb.Append("    <div id=\"scoresDiv\"/>\r\n");

            return sb.ToString();
        }

        public static string makeScoresDiv(int cardId, string connectionString)
        {
            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);

            List<ScoreEntry> scoreList = db.ScoreEntries.Where(w => w.ScoreCardId == cardId).ToList();

            StringBuilder sb = new StringBuilder();

            sb.Append("    <table border=\"1\">\r\n");

            sb.Append("      <tr>\r\n");
            sb.Append("        <th>Hole</th>\r\n");
            sb.Append("        <th>Score</th>\r\n");
            sb.Append("        <th>Net</th>\r\n");
            sb.Append("      </tr>\r\n");

            foreach (ScoreEntry entry in scoreList)
            {
                int holeNumber = MiscLists.GetHoleNumberByHoleId(entry.HoleId, connectionString);
                sb.Append("      <tr>\r\n");
                sb.Append(string.Format("        <th>{0}</th>\r\n", holeNumber));
                sb.Append(string.Format("        <td><input style=\"width: 20px!important;\" type=\"text\" maxlength=\"2\" id=\"Hole_{0}_{1}\" name=\"Hole_{0}_{1}\" value=\"{2}\" onchange=\"updateScore('Hole_{0}_{1}');\" /></td>\r\n", cardId, entry.Id, entry.Score));
                sb.Append("      </tr>\r\n");
            }

            sb.Append("    </table>\r\n");

            return sb.ToString();
        }

        public static string makeEventSelect(string connectionString)
        {
            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);

            List<Event> eventList = new List<Event>();

            try
            {
                eventList = db.Events.Where(w => w.start.Day == DateTime.Now.Day).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogError("MobileScoresHtmlFactory.makeEventSelect", ex.ToString());
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("      <select onchange=\"updateCardSelect();\" id=\"eventSelect\" name=\"eventSelect\" >\r\n");

            sb.Append("        <option value=\"0\">--</ option >\r\n");

            foreach (Event evt in eventList)
                sb.Append(string.Format("        <option value=\"{0}\">{1}</ option >\r\n", evt.id, evt.text));
            
            sb.Append("      </select>\r\n");
            return sb.ToString();
        }

        public static string makeTeamSelectOptions(int eventId, string connectionString)
        {
            List<ScoreCard> cardList = new List<ScoreCard>();

            GolfDB2DataContext db = EventDetailTools.GetDB(connectionString);

            try
            {
                cardList = db.ScoreCards.Where(w => w.EventId == eventId).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogError("MobileScoresHtmlFactory.makeTeamSelectOptions", ex.ToString());
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            try
            {
                int count = 0;
                foreach (ScoreCard card in cardList)
                {
                    if (count++ > 0)
                        sb.Append(",");

                    sb.Append("{\"Value\":\"" + card.Id + "\", \"Text\":\"" + card.Names + "\"}");
                }
            } catch(Exception ex)
            {
                Console.Out.WriteLine(ex.ToString());
            }

            sb.Append("]");

            return sb.ToString();
        }

        public static string makeTeamSelect(int eventId, string connectionString)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("    <select onchange=\"loadScoresDiv();\" id=\"cardSelect\" name=\"cardSelect\">\r\n");
            sb.Append("        <option value=\"0\">--</ option >\r\n");
            sb.Append("    </select>\r\n");
            return sb.ToString();
        }
    }
}