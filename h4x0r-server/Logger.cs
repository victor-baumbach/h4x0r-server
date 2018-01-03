using System;
using System.Collections.Generic;
using System.Threading;

namespace h4x0r
{
    /////////////////////////////////////////////////////////////////////
    // Logger
    // Provides multi-target logging functionality. Thread-safe.
    /////////////////////////////////////////////////////////////////////

    class Logger
    {
        public enum Level
        {
            Info,
            Warning,
            Error
        }

        public Logger()
        {
            m_Delegates = new List<LogDelegate>();
            m_Mutex = new Mutex();
        }

        public delegate void LogDelegate(Level level, string text);

        public static void AddTarget(LogDelegate logDelegate)
        {
            m_Mutex.WaitOne();
            m_Delegates.Add(logDelegate);
            m_Mutex.ReleaseMutex();
        }

        public static void RemoveTarget(LogDelegate logDelegate)
        {
            m_Mutex.WaitOne();
            m_Delegates.Remove(logDelegate);
            m_Mutex.ReleaseMutex();
        }

        public static void Write(Level level, string text)
        {
            m_Mutex.WaitOne();

            DateTime dt = DateTime.Now;
            text = "[" + dt.ToShortDateString() + " " + dt.ToShortTimeString() + "] ("+ level.ToString() + "): " + text + Environment.NewLine;

            foreach (LogDelegate del in m_Delegates)
            {
                del(level, text);
            }
    
            m_Mutex.ReleaseMutex();
        }

        public static void Write(Level level, string format, object arg0)
        {
            Write(level, String.Format(format, arg0));
        }

        public static void Write(Level level, string format, object arg0, object arg1)
        {
            Write(level, String.Format(format, arg0, arg1));
        }

        public static void Write(Level level, string format, object arg0, object arg1, object arg2)
        {
            Write(level, String.Format(format, arg0, arg1, arg2));
        }

        private static List<LogDelegate> m_Delegates;
        private static Mutex m_Mutex;
    }
}
