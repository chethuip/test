/***********************************************************
 * (C) Copyright 2022 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using OXPd2ExamplesHost.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;


namespace OXPd2ExamplesHost.Services
{
    public interface ILogService
    {
        public void CreateLog(string logName);

        public void Log(string logName, LogItem logItem);

        public void ClearLog(string logName);

        public IEnumerable<LogItem> GetLog(string logName);
    }

    public class LogService : ILogService
    {
        private ConcurrentDictionary<string, List<LogItem>> log;

        public LogService()
        {
            log = new ConcurrentDictionary<string, List<LogItem>>();
        }

        public void CreateLog(string logName)
        {
            
            log.TryAdd(logName, new List<LogItem>());
            
        }

        public void Log(string logName, LogItem logItem)
        {
            if (!log.ContainsKey(logName)) {
                log.TryAdd(logName, new List<LogItem>());
            }
            log[logName].Add(logItem);
        }

        public void ClearLog(string logName)
        {
            if (log.ContainsKey(logName))
            {
                log[logName].Clear();
            }
        }

        public IEnumerable<LogItem> GetLog(string logName)
        {
            if (log.ContainsKey(logName))
            {
                return log[logName].AsEnumerable<LogItem>();
            }
            return new List<LogItem>();
        }
    }
}
