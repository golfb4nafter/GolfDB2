using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GolfDB2.Tools
{
    public class SortStartingHoleHelper : IComparer<SortableRowObject>
    {
        int IComparer<SortableRowObject>.Compare(SortableRowObject a, SortableRowObject b)
        {
            int res = a.StartingHoleNumber.CompareTo(b.StartingHoleNumber);

            if (res != 0)
                return res;

            return a.CompareTo(b);
        }
    }

    public class SortDivisionHelper : IComparer<SortableRowObject>
    {
        int IComparer<SortableRowObject>.Compare(SortableRowObject a, SortableRowObject b)
        {
            int res = String.Compare(a.Division, b.Division, false, CultureInfo.CurrentCulture);

            if (res != 0)
                return res;

            return a.CompareTo(b);
        }
    }

    public class SortNamesHelper : IComparer<SortableRowObject>
    {
        int IComparer<SortableRowObject>.Compare(SortableRowObject a, SortableRowObject b)
        {
            int res = String.Compare(a.Name, b.Name, false, CultureInfo.CurrentCulture);

            if (res != 0)
                return res;

            return a.CompareTo(b);
        }
    }

    public class SortTotalScoreHelper : IComparer<SortableRowObject>
    {
        int IComparer<SortableRowObject>.Compare(SortableRowObject a, SortableRowObject b)
        {
            if (a.TotalScore < b.TotalScore)
                return -1;

            if (a.TotalScore > b.TotalScore)
                return 1;

            return a.CompareTo(b);
        }
    }

    public class SortableRowObject : IComparable<SortableRowObject>
    {
        private readonly List<ScoreEntry> scoresList = new List<ScoreEntry>();

        public Event Event { get; set; }
        public EventDetail EventDetail { get; set; }
        public TeeTime Tee_Time { get; set; }
        public ScoreCard Card { get; set; }
        public int Ordinal { get; set; }
        public DateTime RowTeeTime { get; set; }
        public int StartingHoleNumber { get; set; }
        public int TotalScore { get; set; }
        public string Division { get; set; }
        public string Name { get; set; }
        public int Handicap { get; set; }

        public List<ScoreEntry> scores
        {
            get
            {
                return scoresList;
            }
        }

        public string HtmlRow { get; set; }

        public int CompareTo(SortableRowObject x) { return CompareTo(this, x); }
        private static int CompareTo(SortableRowObject x, SortableRowObject y) { return x.Ordinal.CompareTo(y.Ordinal); }
        public bool Equals(SortableRowObject x) { return CompareTo(this, x) == 0; }
        public static bool operator <(SortableRowObject x, SortableRowObject y) { return CompareTo(x, y) < 0; }
        public static bool operator >(SortableRowObject x, SortableRowObject y) { return CompareTo(x, y) > 0; }
        public static bool operator <=(SortableRowObject x, SortableRowObject y) { return CompareTo(x, y) <= 0; }
        public static bool operator >=(SortableRowObject x, SortableRowObject y) { return CompareTo(x, y) >= 0; }
        public static bool operator ==(SortableRowObject x, SortableRowObject y) { return CompareTo(x, y) == 0; }
        public static bool operator !=(SortableRowObject x, SortableRowObject y) { return CompareTo(x, y) != 0; }
        public override bool Equals(object obj)
        {
            return (obj is SortableRowObject) && (CompareTo(this, (SortableRowObject)obj) == 0);
        }
    }
}