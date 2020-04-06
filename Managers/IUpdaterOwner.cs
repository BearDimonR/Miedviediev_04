using System.Collections.Generic;
using System.ComponentModel;

namespace Miedviediev_04.Managers
{
    internal interface IUpdaterOwner<T> : INotifyPropertyChanged
    {
        IUpdater Updater { set; get; }

        AsyncObservableCollection<T>  CurrCollection { get; set; }
        SortedSet<int> CurrIds { get; set; }

        void UpdateUi(); }
}