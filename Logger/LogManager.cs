using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace Logger
{
    public static class LogManager
    {
        private static readonly ILog m_logger = log4net.LogManager.GetLogger(typeof(LogManager));
        private static bool m_initDone = false;

        public static void Init()
        {
            if (m_initDone)
                return;

            DOMConfigurator.Configure();
            m_initDone = true;
        }

        public static ILog GetLog()
        {
            Init();
            return m_logger;
        }
    }
}
