using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Miedviediev_04.Models;
using System.Timers;
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

        private static List<MyProcess> _activeProcesses;

        internal ProcessUpdater(int metaTimer, int listTimer)
        {
            _activeProcesses = new List<MyProcess>();
            _metaTimer = new Timer(metaTimer);
            _metaTimer.Elapsed += MetaUpdate;
            _metaTimer.AutoReset = true;
            _listTimer = new Timer(listTimer);
            _listTimer.Elapsed += ListUpdate;
            _listTimer.AutoReset = true;
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    _activeProcesses.Add(new MyProcess(process));
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        public ObservableCollection<MyProcess> GetProcesses()
        {
            lock (_locker)
            {
                return new ObservableCollection<MyProcess>(_activeProcesses);
            }
        }

        public void RemoveProcess(MyProcess myProcess)
        {
            lock (_locker)
            {
                _activeProcesses.Remove(myProcess);
            }
        }

        public void StartUpdate()
        {
            _metaTimer.Start();
            _listTimer.Start();
        }

        public void StopUpdate()
        {
            _metaTimer.Stop();
            _listTimer.Stop();
        }

        private void MetaUpdate(object o, ElapsedEventArgs args)
        {
            lock (_locker)
            {
                foreach (var myProcess in _activeProcesses)
                    myProcess.Update(Process.GetProcessById(myProcess.Id));
                UpdateManager.Instance.Owner.UpdateUi();
            }
        }

        private void ListUpdate(object o, ElapsedEventArgs args)
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

            lock (_locker)
            {
                _activeProcesses = list;
                UpdateManager.Instance.Owner.UpdateUi();
            }
        }
    }
}