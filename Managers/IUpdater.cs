using System.Collections.ObjectModel;
using Miedviediev_04.Models;

namespace Miedviediev_04.Managers
{
    public interface IUpdater
    {
        int IntervalList { set;}
        
        int IntervalMeta { set;}
        
        void StartUpdate();
        
        void StopUpdate();

        ObservableCollection<MyProcess> GetProcesses();

        void RemoveProcess(MyProcess myProcess);
    }
}