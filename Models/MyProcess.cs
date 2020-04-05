using System;
using System.Diagnostics;

namespace Miedviediev_04.Models
{
    public class MyProcess:IEquatable<MyProcess>
    {

        private Process _process;
        public Process OriginProcess => _process;

        public string Name => _process.ProcessName;
        
        public int Id => _process.Id;
        
        public bool Responding => _process.Responding;
        
        public int Threads => _process.Threads.Count;

        public DateTime StartTime => _process.StartTime;

        private string _user;
        public string User => _user;

        public string StartFrom => _process.MainModule?.FileName;

        private PerformanceCounter _cpu;
        public float Cpu => _cpu.NextValue();

        private PerformanceCounter _ram;
        public float Ram => _ram.NextValue();

        public MyProcess(Process process)
        {
            Update(process);
        }

        public void Update(Process process)
        {
            _process = process;
            _user = process.StartInfo.UserName;
            _cpu =  new PerformanceCounter("Process", 
                "% Processor Time", 
                process.ProcessName);;
            _ram =  new PerformanceCounter("Process",
                "Working Set - Private", 
                process.ProcessName);
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
    }
}