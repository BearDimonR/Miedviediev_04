using System.Collections.ObjectModel;
using System.ComponentModel;
using Miedviediev_04.Models;

namespace Miedviediev_04.Managers
{
    internal interface IUpdaterOwner : INotifyPropertyChanged
    {
        IUpdater Updater { set; get; }

        void UpdateUi(); }
}