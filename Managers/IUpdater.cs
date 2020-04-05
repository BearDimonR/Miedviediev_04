using System.Collections.ObjectModel;
using System.ComponentModel;
using Miedviediev_04.Models;

namespace Miedviediev_04.Managers
{
    public interface IUpdater
    {
        int IntervalList { set;}
        
        int IntervalMeta { set;}
        
        void StartUpdate();
        
        void StopUpdate();
        
        void RemoveProcess(MyProcess myProcess);
    }
}