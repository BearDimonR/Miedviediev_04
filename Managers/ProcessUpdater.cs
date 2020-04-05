using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using Miedviediev_04.Models;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Miedviediev_04.Managers
{
    public class ProcessUpdater: IUpdater
    {
        public int IntervalList
        {
            set
            {
                _listTimer.Interval = value;
            }
        }

        public int IntervalMeta
        {
            set
            {
                _metaTimer.Interval = value;
            }
        }
        
        private Timer _metaTimer;
        private bool _metaUpdating;
        private Timer _listTimer;
        private bool _listUpdating;

        public ObservableCollection<MyProcess> ActiveProcesses { get; private set; }

        internal ProcessUpdater(int metaTimer, int listTimer)
        {
            _metaUpdating = false;
            _listUpdating = false;
            ActiveProcesses = new ObservableCollection<MyProcess>();
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
                   // if(process.MainModule != null)
                        ActiveProcesses.Add( new MyProcess(process));
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    // ignored
                }
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
            // while (_listUpdating){}
            // _metaUpdating = true;
            // foreach (var process in ActiveProcesses)
            // {
            //     process.Update(Process.GetProcessById(process.Id));
            // }
            // UpdateManager.Instance.Owner.UpdateUi();
            // _metaUpdating = false;
        }

        private void ListUpdate(object o, ElapsedEventArgs args)
        {
            while (_metaUpdating){}
            _listUpdating = true;
            ActiveProcesses.Clear();
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    ActiveProcesses.Add(new MyProcess(process));
                }
                catch (Exception)
                {
                    // ignored
                }
            }
           // UpdateManager.Instance.Owner.UpdateUi();
            _listUpdating = false;
        }
    }
}