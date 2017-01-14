using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfDB2.Tools
{
    public class ShotgunHoleCalculator
    {

        public static HoleList GetHoleListById(int id, string connectionString)
        {
            GolfDB2DataContext db = null;

            if (!string.IsNullOrEmpty(connectionString))
                db = new GolfDB2DataContext(connectionString);
            else
                db = new GolfDB2DataContext();

            return (from hl in db.GetTable<HoleList>() where hl.Id == id select hl).SingleOrDefault();
        }

        public static Hole GetHoleByHoleNumber(int holeNumber, string connectionString)
        {
            GolfDB2DataContext db = null;

            int courseId = GlobalSettingsApi.GetInstance().CourseId;

            if (!string.IsNullOrEmpty(connectionString))
                db = new GolfDB2DataContext(connectionString);
            else
                db = new GolfDB2DataContext();

            return (from h in db.GetTable<Hole>() where (h.Number == holeNumber && h.CourseId == courseId) select h).SingleOrDefault();
        }

        public static Hole GetHoleById(int id, string connectionString)
        {
            GolfDB2DataContext db = null;

            int courseId = GlobalSettingsApi.GetInstance().CourseId;

            if (!string.IsNullOrEmpty(connectionString))
                db = new GolfDB2DataContext(connectionString);
            else
                db = new GolfDB2DataContext();

            return (from h in db.GetTable<Hole>() where (h.Id == id && h.CourseId == courseId) select h).SingleOrDefault();
        }

        public static int GetHoleIdByOrdinalAndEventId(int ordinal, int playListId, int eventId, string connectionString)
        {
            // We need the HoleList and BList from HoleList table.
            HoleList hl = GetHoleListById(playListId, connectionString);

            // the ordinal value is the offset into the HoleList array for holes 1-n
            // after 1-n+1 we assign as ordered in the BList for b,c,d teams
            string[] holeList = hl.HoleList1.Split(',');

            string[] bList = null;
            int numBHoles = 0;

            if (!string.IsNullOrEmpty(hl.BList))
            {
                bList = hl.BList.Split(',');
                numBHoles = bList.Length;
            }

            int numHoles = holeList.Length;
            int holeNum;

            // We should also establish a max number of teams based on the sise of the HoleList array size 
            // added to the Blist array size.

            if ((ordinal + 1) > (numHoles + numBHoles))
                return -1;

            if ((ordinal + 1) > numHoles)
                holeNum = int.Parse(bList[ordinal - numHoles]);
            else
                holeNum = int.Parse(holeList[ordinal]);

            Hole h = GetHoleByHoleNumber(holeNum, connectionString);

            return h.Id;
        }
    }
}