using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace GolfDB2.Tools
{
    public static class MobileScoresHtmlFactory
    {
        public static string makeMobileScoreCardHtml(bool isVertical, string connectionString)
        {
            if (isVertical)
                return new VerticalMobileScoreCardFactory().makeVerticalMobileScoreCardHtml(connectionString);
            else
                return new HorizontalMobileScoreCardFactory().makeHorizontalMobileScoreCardHtml(connectionString);
        }

        public static String makeScoresDiv(bool isVertical, int eventId, int cardId, int ordinal, string gender, string connectionString)
        {
            //if (isVertical)
            //    return new VerticalMobileScoreCardFactory().makeScoresDiv(cardId, ordinal, gender, teeColorId, playListId, connectionString);
            //else
                return new HorizontalMobileScoreCardFactory().makeScoresDiv(cardId, eventId, ordinal, gender, connectionString);
        }
    }
}