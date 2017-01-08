using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using GolfDB2.Models;

namespace GolfDB2.Tools
{
    public class HtmlFactory
    {
        public static string MakeTeeTimeTable(List<TeeTime> teeTimeList)
        {
            StringBuilder htmlDoc = new StringBuilder();

            htmlDoc.Append("    <table>\r\n");
            htmlDoc.Append(MakeTeeTimeHeaderRow());

            foreach(TeeTime tt in teeTimeList)
            {
                string startingHole = TeeTimeTools.GetStartingHoleByHoleId(tt.EventId, tt.TeeTimeOffset, tt.HoleId, null);
                htmlDoc.Append(MakeTeeTimeDetailRow(tt.Tee_Time.ToString("MM/dd HH:mm"), tt.PlayerNames, startingHole, tt.Id));
            }

            htmlDoc.Append("    </table>\r\n");
            return htmlDoc.ToString();
        }

        public static string MakeTeeTimeHeaderRow()
        {
            return "        <tr><th>Tee Time</th><th>Start On</th><th>Golfers</th><th>Actions</th></tr>\r\n";
        }

        public static string MakeTeeTimeDetailRow(string teeTime, string golfers, string startingHole, int index)
        {
            return string.Format("        <tr><td>{0}</td><td>{3}</td><td><input type=\"text\" style=\"{{ width: 400px!important; }}\" id=\"golfers{2}\" value =\"{1}\"  size=\"128\" /></td><td><input type = \"button\" id=\"btn_sav_{2}\" value= \"Save\" onclick=\"onSaveClicked('{2}')\" /></ td ></ tr >\r\n", 
                teeTime, golfers, index, startingHole);
        }
    }
}