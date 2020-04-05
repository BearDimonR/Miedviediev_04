using System.Collections.ObjectModel;
using Miedviediev_04.Models;

namespace Miedviediev_04.Managers
{
    public interface IUpdater
    {
        int IntervalList { set;}
        
        int IntervalMeta { set;}

        ObservableCollection<MyProcess> ActiveProcesses { get; }

        void StartUpdate();
        
        void StopUpdate();
        
    }
}