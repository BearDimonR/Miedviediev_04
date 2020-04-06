using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
                Update(UpdateManager<MyProcess>.Instance.Owner.CurrCollection);
            _metaTimer.AutoReset = true;
            _listTimer = new Timer(listTimer);
            _listTimer.Elapsed += (timersender, timerevent) =>
                Add(UpdateManager<MyProcess>.Instance.Owner.CurrCollection);
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
            UpdateManager<MyProcess>.Instance.Owner.CurrCollection.Remove(myProcess);
        }

        public void AddProcess(MyProcess myProcess)
        {
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

        private async void Add(AsyncObservableCollection<MyProcess> activeProcesses)
        {
            if (!_lockerList) return;
            _lockerList = false;
            Task<List<Process>> processes = Task.Run((() =>
            {
                List<Process> list = new List<Process>();
                foreach (var process in Process.GetProcesses())
                {
                    try
                    {
                        if (process.Modules.Count != 0)
                        {
                            list.Add(process);
                        }
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                return list;
            }));
            Task<List<int>> act = Task.Run((() => (from process in activeProcesses.ToArray()
                orderby process.Id
                select process.Id).ToList()));
            await Task.WhenAll(act, processes);
            for (int i = 0; i < processes.Result.Count; i++)
            {
                if (act.Result.BinarySearch(processes.Result[i].Id) >= 0) continue;
                if(!processes.Result[i].HasExited)
                    AddProcess(new MyProcess(processes.Result[i]));
                UpdateManager<MyProcess>.Instance.Owner.UpdateUi();
            }
            _lockerList = true;
        }

        private void Update(AsyncObservableCollection<MyProcess> activeProcesses)
        {
            if(!_lockerMeta) return;
            _lockerMeta = false;
            for (int i = 0; i < activeProcesses.Count; ++i)
                {
                    Update(activeProcesses[i]);
                    UpdateManager<MyProcess>.Instance.Owner.UpdateUi();
                }
            _lockerMeta = true;
        }
    }
}