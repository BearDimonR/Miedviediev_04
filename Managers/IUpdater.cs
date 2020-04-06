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
        void AddProcess(MyProcess myProcess);
        void Update(MyProcess myProcess);
    }
}