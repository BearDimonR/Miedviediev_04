using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Management;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.Devices;
using Miedviediev_04.Annotations;

namespace Miedviediev_04.Models
{
    public sealed class MyProcess : IEquatable<MyProcess>, INotifyPropertyChanged
    {

        private Process _process;
        public Process OriginProcess => _process;

        public string Name { get; }

        public int Id { get; }

        private bool _responding;
        public bool Responding => _responding;

        private int _threads;
        public int Threads => _threads;

        public DateTime StartTime { get; }

        public string User { get; }

        public string StartFrom { get; }

        private readonly PerformanceCounter _cpu;

        public float Cpu
        {
            get
            {
                try
                {
                    return _cpu.NextValue() / Environment.ProcessorCount * 2;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        private readonly PerformanceCounter _ram;

        public int Ram
        {
            get
            {
                try
                {
                    return Convert.ToInt32(_ram.NextValue()) / 1024;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        private static readonly ulong TotalRam = new ComputerInfo().TotalPhysicalMemory;

        public float RamPers => (float)Ram * 1024 / TotalRam * 100;

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
            
            
            Name = process.ProcessName;
            Id = process.Id;
            StartFrom = process.MainModule == null ? string.Empty : process.MainModule.FileName;
            StartTime = process.StartTime;
            User = GetProcessOwner(process);
        }

        public void Update(Process process)
        {
            _process = process;
            _responding = process.Responding && !process.HasExited;
            _threads = process.Threads.Count;
            OnPropertyChanged(nameof(Cpu));
            OnPropertyChanged(nameof(Ram));
            OnPropertyChanged(nameof(RamPers));
            OnPropertyChanged(nameof(Responding));
            OnPropertyChanged(nameof(Threads));
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
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}