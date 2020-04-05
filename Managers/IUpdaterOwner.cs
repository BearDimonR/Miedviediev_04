using System.ComponentModel;

namespace Miedviediev_04.Managers
{
    internal interface IUpdaterOwner : INotifyPropertyChanged
    {
        IUpdater Updater { set; get; }
        
        void UpdateUi();
    }
}