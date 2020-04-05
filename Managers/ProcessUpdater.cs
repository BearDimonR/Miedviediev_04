using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Miedviediev_04.Models;
using System.Timers;
using Microsoft.Win32;
using Timer = System.Timers.Timer;

namespace Miedviediev_04.Managers
{
    public class ProcessUpdater : IUpdater
    {
        public int IntervalList
        {
            set { _listTimer.Interval = value; }
        }

        public int IntervalMeta
        {
            set { _metaTimer.Interval = value; }
        }

        private Timer _metaTimer;
        private Timer _listTimer;
        private static object _locker = new object();
        public static object Locker => _locker;

        internal ProcessUpdater(int metaTimer, int listTimer)
        {
            _metaTimer = new Timer(metaTimer);
            _metaTimer.Elapsed += (timersender, timerevent) => 
                Update(timerevent, timerevent, UpdateManager<MyProcess>.Instance.Owner.CurrCollection, false);
            _metaTimer.AutoReset = true;
            _listTimer = new Timer(listTimer);
            _listTimer.Elapsed += (timersender, timerevent) => 
                Update(timerevent, timerevent, UpdateManager<MyProcess>.Instance.Owner.CurrCollection, true);
            _listTimer.AutoReset = true;
        }

        public void StartUpdate()
        {
            _metaTimer.Start();
            _listTimer.Start();
        }

        public void StopUpdate()
        {
        }

        public void RemoveProcess(MyProcess myProcess)
        {
            lock(_locker)
                UpdateManager<MyProcess>.Instance.Owner.CurrCollection.Remove(myProcess);
        }

        public static ObservableCollection<MyProcess> GetProcesses()
        {
            List<MyProcess> list = new List<MyProcess>();
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    list.Add(new MyProcess(process));
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            return new ObservableCollection<MyProcess>(list);
        }

        private void Update(object o, ElapsedEventArgs args, ObservableCollection<MyProcess> activeProcesses, bool state)
        {
            lock (_locker)
            {
                if (state)
                {
                    ObservableCollection<MyProcess> copy = GetProcesses();
                    ObservableCollection<MyProcess> processes = GetProcesses();
                    foreach (var process in activeProcesses)
                    {
                        if (!processes.Contains(process))
                            activeProcesses.Remove(process);
                        
                    }
                    foreach (var process in processes)
                    {
                        if(!activeProcesses.Contains(process))
                            activeProcesses.Add(process);
                    }
                    UpdateManager<MyProcess>.Instance.Owner.UpdateUi();
                    return;
                }

                foreach (var t in activeProcesses)
                {
                    t.Update(Process.GetProcessById(t.Id));
                }
                UpdateManager<MyProcess>.Instance.Owner.UpdateUi();
            }
        }
    }
}