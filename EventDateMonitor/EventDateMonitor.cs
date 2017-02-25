using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Text;

namespace EventDateMonitor
{
    public class EventDateMonitor : IDisposable
    {
        private static EventDateMonitor _instance = null;

        public static EventDateMonitor getInstance()
        {
            if (_instance == null)
                _instance = new EventDateMonitor();

            return _instance;
        }

        private readonly GolfDB2DataContext db = null;
        private Timer aTimer = null;

        public EventDateMonitor()
        {
            db = new GolfDB2DataContext();
        }

        public void StopTimer()
        {
            if (aTimer != null)
            {
                aTimer.Stop();
                aTimer.Dispose();
            }
        }

        public void StartTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(50000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0)
                LockPastAndCompletedEvents();
        }

        public Event GetEvent(int eventId, string connectionString)
        {
            return (from e in db.GetTable<Event>() where e.id == eventId select e).SingleOrDefault();
        }

        public void LockPastAndCompletedEvents()
        {
            List<Event> eList = GetEventsForLock();

            foreach(Event e in eList)
                SetEventLocked(e);
        }

        public bool SetEventLocked(Event evt)
        {
            if (evt != null)
            {
                evt.locked = true;
                db.SubmitChanges();
                return true;
            }

            return false;
        }

        public List<Event> GetEventsForLock()
        {
            return (from c in db.GetTable<Event>() where (c.start < DateTime.Now && c.locked == false) select c).ToList();
        }

        public void Dispose()
        {
            StopTimer();
        }
    }
}
