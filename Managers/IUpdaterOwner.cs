using System.Collections.ObjectModel;
using System.ComponentModel;
using Miedviediev_04.Models;

namespace Miedviediev_04.Managers
{
    internal interface IUpdaterOwner<T> : INotifyPropertyChanged
    {
        IUpdater Updater { set; get; }

         ObservableCollection<T>  CurrCollection { get; set; }

        void UpdateUi(); }
}