using System;
using System.Collections.Generic;
using System.Diagnostics;
using Miedviediev_04.Models;
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
        private static bool _lockerMeta = true;
        private static bool _lockerList = true;

        public static bool Locker
        {
            get => _lockerMeta;
            set => _lockerMeta = value;
        }

        internal ProcessUpdater(int metaTimer, int listTimer)
        {
            _metaTimer = new Timer(metaTimer);
            _metaTimer.Elapsed += (timersender, timerevent) =>
                Update();
            _metaTimer.AutoReset = true;
            _listTimer = new Timer(listTimer);
            _listTimer.Elapsed += (timersender, timerevent) =>
                Add();
            _listTimer.AutoReset = true;
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

        public void RemoveProcess(MyProcess myProcess)
        {
            UpdateManager<MyProcess>.Instance.Owner.CurrIds.Remove(myProcess.Id);
            UpdateManager<MyProcess>.Instance.Owner.CurrCollection.Remove(myProcess);
        }

        public void AddProcess(MyProcess myProcess)
        {
            UpdateManager<MyProcess>.Instance.Owner.CurrIds.Add(myProcess.Id);
            UpdateManager<MyProcess>.Instance.Owner.CurrCollection.Add(myProcess);
        }

        public void Update(MyProcess myProcess)
        {
            try
            {
                Process process = Process.GetProcessById(myProcess.Id);
                myProcess.Update(process);
                //DataGridSortBehaviour.SortValue(activeProcesses[i], i);
            }
            catch (ArgumentException)
            {
                UpdateManager<MyProcess>.Instance.Owner.Updater.RemoveProcess(myProcess);
            }
        }

        private void Add()
        {
            if (!_lockerList) return;
            _lockerList = false;
            foreach (var t in Process.GetProcesses())
            {
                try
                {
                    if (UpdateManager<MyProcess>.Instance.Owner.CurrIds.Contains(t.Id)) continue;
                    if (!t.HasExited && t.Modules.Count != 0)
                        AddProcess(new MyProcess(t));
                    UpdateManager<MyProcess>.Instance.Owner.UpdateUi();
                }
                catch (Exception)
                {
                    //ignore
                }
            }
            _lockerList = true;
        }

        private void Update()
        {
            if (!_lockerMeta) return;
            _lockerMeta = false;
            for (int i = 0; i < UpdateManager<MyProcess>.Instance.Owner.CurrCollection.Count; ++i)
            {
                Update(UpdateManager<MyProcess>.Instance.Owner.CurrCollection[i]);
                UpdateManager<MyProcess>.Instance.Owner.UpdateUi();
            }

            _lockerMeta = true;
        }

        public static AsyncObservableCollection<MyProcess> GetProcesses()
        {
            List<MyProcess> list = new List<MyProcess>();
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    if (process.Modules.Count != 0)
                    {
                        list.Add(new MyProcess(process));
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return new AsyncObservableCollection<MyProcess>(list);
        }

        public static SortedSet<int> GetCurrIds()
        {
            SortedSet<int> list = new SortedSet<int>();
            foreach (var myProcess1 in UpdateManager<MyProcess>.Instance.Owner.CurrCollection)
                list.Add(myProcess1.Id);
            return list;
        }
    }
}