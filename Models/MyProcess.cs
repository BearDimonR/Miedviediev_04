using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Management;
using System.Runtime.CompilerServices;
using Miedviediev_04.Annotations;

namespace Miedviediev_04.Models
{
    public class MyProcess:IEquatable<MyProcess>, INotifyPropertyChanged 
    {

        private Process _process;
        public Process OriginProcess => _process;

        public string Name => _process.ProcessName;
        
        public int Id => _process.Id;
        
        public bool Responding => _process.Responding;
        
        public int Threads => _process.Threads.Count;

        public DateTime StartTime => _process.StartTime;

        public string User { get; }

        public string StartFrom => _process.MainModule?.FileName;

        private PerformanceCounter _cpu;
        public float Cpu => _cpu.NextValue() / Environment.ProcessorCount;

        private PerformanceCounter _ram;
        public int Ram => Convert.ToInt32(_ram.NextValue()) / (int)(1024);

        public MyProcess(Process process)
        {
            Update(process);
            _cpu =  new PerformanceCounter("Process", 
                "% Processor Time", 
                process.ProcessName, true);
            _cpu.NextValue();
            _ram =  new PerformanceCounter("Process",
                "Working Set - Private", 
                process.ProcessName, true);
            _ram.NextValue(); 
            //User = GetProcessOwner(process);
           
        }

        public void Update(Process process)
        {
            _process = process;
            OnPropertyChanged(nameof(Cpu));
            OnPropertyChanged(nameof(Ram));
            OnPropertyChanged(nameof(Responding));
            OnPropertyChanged(nameof(Threads));
            OnPropertyChanged(nameof(Id));
            OnPropertyChanged(nameof(StartFrom));
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(StartTime));
        }
        
        private static string GetProcessOwner(Process process)
        {
            ObjectQuery objQuery32 = new ObjectQuery("Select * From Win32_Process where ProcessId='" + process.Id + "'");
            ManagementObjectSearcher mos32 = new ManagementObjectSearcher(objQuery32);
            string processOwner = string.Empty;
            foreach (ManagementObject mo in mos32.Get())
            {
                string[] s = new string[2];
                mo.InvokeMethod("GetOwner", (object[])s);
                try
                {
                    processOwner = s[0];
                }
                catch (Exception)
                {
                    //ignore
                }
                break;
            }
            return processOwner;
        }


        public bool Equals(MyProcess other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _process.Id == other._process.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MyProcess) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}