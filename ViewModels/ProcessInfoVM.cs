using System;
using System.Diagnostics;
using Miedviediev_04.Managers;
using Miedviediev_04.Models;
using Miedviediev_04.Navigation;

namespace Miedviediev_04.ViewModels
{
    public class ProcessInfoVm:BaseVm, INavigatableDataContext
    {

        private Process _process;
        public Process Process => _process;

        public RelayCommand<object> BackCommand { get; }

        public ProcessInfoVm()
        {
            BackCommand = new RelayCommand<object>(NavigateBack);
        }

        private void NavigateBack(object obj)
        {
            NavigationManager.Instance.Navigate(ViewType.GridView);
        }

        public void Execute(object obj)
        {
            _process = ((MyProcess) obj).OriginProcess;
            OnPropertyChanged(nameof(Process));
        }
    }
}