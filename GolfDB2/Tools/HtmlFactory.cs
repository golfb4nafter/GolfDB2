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
        // Notes:
        //
        // if it's stroke play, best ball, or blind then we use seperate score cards 
        // otherwise it's a team event and we build a single score card.
        public static string MakeTeeTimeTable(List<TeeTime> teeTimeList)
        {
            StringBuilder htmlDoc = new StringBuilder();

            htmlDoc.Append("    <table>\r\n");
            htmlDoc.Append(MakeTeeTimeHeaderRow());

            foreach (TeeTime tt in teeTimeList)
            {
                string startingHole = TeeTimeTools.GetStartingHoleByHoleId(tt.EventId, tt.TeeTimeOffset, tt.HoleId, null);
                htmlDoc.Append(MakeTeeTimeDetailRow(tt, startingHole));
            }

            htmlDoc.Append("    </table>\r\n");
            return htmlDoc.ToString();
        }

        public static string MakeTeeTimeHeaderRow()
        {
            return "        <tr><th>Tee Time</th><th>Start On</th><th>Golfer</th><th>Division</th><th>Cart</th><th>Pass</th><th>Skins</th><th>Amount</th></tr>\r\n";
        }


        public static string MakeTeeTimeDetailRow(TeeTime tt, string startingHole)
        {
            string[] golfers = tt.PlayerNames.Split(',');
            int index = tt.Id;

            string col1 = string.Format("        <td>{0}</td>\r\n", tt.Tee_Time.ToString("HH:mm"));
            string col2 = string.Format("        <td>{0}</td>\r\n", startingHole);

            StringBuilder col3 = new StringBuilder();
            List<string> gList = new List<string>();

            for (int i = 0; i < tt.NumberOfPlayers; i++)
            {
                if (golfers != null && golfers.Length > i)
                    gList.Add(golfers[i]);
                else
                    gList.Add("TBD");
            }

            for (int i = 0; i < tt.NumberOfPlayers; i++)
            {
                col3.Append("    <tr>\r\n");

                if (i==0)
                {
                    col3.Append(col1);
                    col3.Append(col2);
                }
                else
                {
                    col3.Append("        <td></td>\r\n        <td>        </td>\r\n");
                }

                TeeTimeDetail ttd = TeeTimeTools.GetTeeTimeDetailByTeeTimeId(tt.Id, i, null);

                if (ttd == null)
                    col3.Append(string.Format("        <td><input type =\"text\" id=\"golfers_{0}_{1}\" name=\"golfers_{0}_{1}\" value =\"{2}\"  size=\"50\" onchange=\"document.getElementById('dirty_{0}_{1}').value = 'true';\" />", index, i, gList[i]));
                else
                    col3.Append(string.Format("        <td><input type =\"text\" id=\"golfers_{0}_{1}\" name=\"golfers_{0}_{1}\" value =\"{2}\"  size=\"50\" onchange=\"document.getElementById('dirty_{0}_{1}').value = 'true';\" />", index, i, ttd.Name));

                col3.Append(string.Format("<input type=\"hidden\" id=\"dirty_{0}_{1}\" name=\"dirty_{0}_{1}\" value=\"false\" /></td>\r\n", index, i));

                if (ttd == null)
                    col3.Append(MakeDivisionSelect(index, i, "open"));
                else
                    col3.Append(MakeDivisionSelect(index, i, ttd.Division.Trim()));

                if (ttd == null)
                    col3.Append(string.Format("        <td><input type=\"checkbox\" id=\"cart_{0}_{1}\" name=\"cart_{0}_{1}\" onchange=\"document.getElementById('dirty_{0}_{1}').value = 'true';\" /></td>\r\n", index, i));
                else
                    col3.Append(string.Format("        <td><input type=\"checkbox\" id=\"cart_{0}_{1}\" name=\"cart_{0}_{1}\" {2} onchange=\"document.getElementById('dirty_{0}_{1}').value = 'true';\" /></td>\r\n", index, i, ttd.Cart ? "checked" : ""));

                if (ttd == null)
                    col3.Append(string.Format("        <td><input type=\"checkbox\" id=\"pass_{0}_{1}\" name=\"pass_{0}_{1}\" onchange=\"document.getElementById('dirty_{0}_{1}').value = 'true';\" /></td>\r\n", index, i));
                else
                    col3.Append(string.Format("        <td><input type=\"checkbox\" id=\"pass_{0}_{1}\" name=\"pass_{0}_{1}\" {2} onchange=\"document.getElementById('dirty_{0}_{1}').value = 'true';\" /></td>\r\n", index, i, ttd.Pass ? "checked" : ""));

                if (ttd == null)
                    col3.Append(string.Format("        <td><input type=\"checkbox\" id=\"skins_{0}_{1}\" name=\"skins_{0}_{1}\" onchange=\"document.getElementById('dirty_{0}_{1}').value = 'true';\" /></td>\r\n", index, i));
                else
                    col3.Append(string.Format("        <td><input type=\"checkbox\" id=\"skins_{0}_{1}\" name=\"skins_{0}_{1}\" {2} onchange=\"document.getElementById('dirty_{0}_{1}').value = 'true';\" /></td>\r\n", index, i, ttd.Skins ? "checked" : ""));

                if (ttd == null)
                    col3.Append(string.Format("        <td><input type =\"text\" style=\"color: #FFFFFF; font-family: Verdana; font-weight: bold; font-size: 12px; background-color: #72A4D2;\" id=\"paid_{0}_{1}\" name=\"paid_{0}_{1}\" value =\"{2}\"  size=\"8\" onchange=\"document.getElementById('dirty_{0}_{1}').value = 'true';\" /></td>\r\n", index, i, "0.00"));
                else
                {
                    if (ttd.AmountPaid == decimal.Parse("0.0"))
                        col3.Append(string.Format("        <td><input type =\"text\" style=\"color: #FFFFFF; font-family: Verdana; font-weight: bold; font-size: 12px; background-color: #72A4D2;\" id=\"paid_{0}_{1}\" name=\"paid_{0}_{1}\" value =\"{2}\"  size=\"8\" onchange=\"document.getElementById('dirty_{0}_{1}').value = 'true';\" /></td>\r\n", index, i, ttd.AmountPaid.ToString("C")));
                    else
                        col3.Append(string.Format("        <td><input type =\"text\" id=\"paid_{0}_{1}\" name=\"paid_{0}_{1}\" value =\"{2}\"  size=\"8\" onchange=\"document.getElementById('dirty_{0}_{1}').value = 'true';\" /></td>\r\n", index, i, ttd.AmountPaid.ToString("C")));
                }

                col3.Append("    </tr>\r\n");
            }

            return col3.ToString();
        }

        public static string MakeSelectOption(string value, string selection)
        {
            if (value == selection)
                return "                <option selected>" + value + "</option>\r\n";
            else
                return "                <option>" + value + "</option>\r\n";
        }

        public static string MakeDivisionSelect(int index, int i, string selection)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("        <td>\r\n            <select id=\"division_{0}_{1}\" name=\"division_{0}_{1}\" onchange=\"document.getElementById('dirty_{0}_{1}').value = 'true';\">\r\n", index, i));

            sb.Append(MakeSelectOption("open", selection));
            sb.Append(MakeSelectOption("mixed", selection));
            sb.Append(MakeSelectOption("Senior Men", selection));
            sb.Append(MakeSelectOption("Senior Women", selection));
            sb.Append("            </select>\r\n        </td>\r\n");

            return sb.ToString();
        }
    }
}