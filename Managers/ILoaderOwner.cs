using System.ComponentModel;
using System.Windows;

namespace Miedviediev_04.Managers
{
    internal interface ILoaderOwner : INotifyPropertyChanged
    {
        Visibility LoaderVisibility
        {
            set;
        }

        bool IsControlEnabled { set; } 
    }
}