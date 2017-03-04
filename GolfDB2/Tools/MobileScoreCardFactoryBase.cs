using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GolfDB2.Tools
{
    public class MobileScoreCardFactoryBase
    {
        public string makeGenderSelect()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("      <select id =\"genderSelect\" name=\"genderSelect\" onchange=\"updateCardSelect();\">");
            sb.Append("        <option selected value=\"M\">Male</ option >");
            sb.Append("        <option value=\"F\">Female</ option >");
            sb.Append("      </select>");
            return sb.ToString();
        }

        public string makeTeeBoxSelect(string connectionString)
        {
            List<TeeBoxMenuColor> colorList = TeeTimeTools.GetTeeBoxMenuColorList(GlobalSettingsApi.GetInstance().CourseId, null);

            // needs to be done by table query
            StringBuilder sb = new StringBuilder();

            sb.Append("      <select onchange=\"updateCardSelect();\" id=\"teeSelect\" name=\"teeSelect\" >\r\n");
            sb.Append("        <option value=\"-1\">--</ option >\r\n");

            foreach (TeeBoxMenuColor t in colorList)
                sb.Append(string.Format("        <option value=\"{0}\">{1}</ option >\r\n", t.ordinal, t.color));

            sb.Append("      </select>\r\n");
            return sb.ToString();
        }

        public string makeEventSelect(string connectionString)
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

        public string makeTeamSelectOptions(int eventId, string connectionString)
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
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.ToString());
            }

            sb.Append("]");

            return sb.ToString();
        }

        public string makeTeamSelect(int eventId, string connectionString)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("    <select onchange=\"loadScoresDiv();\" id=\"cardSelect\" name=\"cardSelect\">\r\n");
            sb.Append("        <option value=\"0\">--</ option >\r\n");
            sb.Append("    </select>\r\n");
            return sb.ToString();
        }
    }
}