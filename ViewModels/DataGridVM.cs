using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using Miedviediev_04.Managers;
using Miedviediev_04.Models;
using Miedviediev_04.Navigation;

namespace Miedviediev_04.ViewModels
{
    public class DataGridVm:BaseVm, INavigatableDataContext, IUpdaterOwner
    {        
        private ObservableCollection<MyProcess> _processes;
        public ObservableCollection<MyProcess> Processes => _processes;
        
        public RelayCommand<MyProcess> InfoCommand { get; } 
        public RelayCommand<MyProcess> FolderCommand { get; }
        public RelayCommand<MyProcess> StopCommand { get; }

        public DataGridVm()
        {
            InfoCommand = new RelayCommand<MyProcess>(NavigateInfo);
            FolderCommand = new RelayCommand<MyProcess>(OpenFolder);
            StopCommand = new RelayCommand<MyProcess>(StopProcess);
            
            UpdateManager.Instance.Initialize(this);
            _processes = new ObservableCollection<MyProcess>();
            Updater = new ProcessUpdater(2000, 5000);
            UpdateUi();
            Updater.StartUpdate();
        }

        private void StopProcess(MyProcess obj)
        {
            try
            {
                obj.OriginProcess.Kill();
                _processes.Remove(obj);
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                MessageBox.Show(e.Message,
                    "Can't be stopped!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
        }

        private void OpenFolder(MyProcess obj)
        {
            try
            {
                Process.Start(Path.GetDirectoryName(obj.StartFrom));
            }
            catch (Exception)
            {
                MessageBox.Show("System Process! Don't have access!",
                    "No folder found!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }
            
        }

        private void NavigateInfo(MyProcess obj)
        {
            NavigationManager.Instance.Execute(ViewType.ProcessView, obj);
            NavigationManager.Instance.Navigate(ViewType.ProcessView);
        }

        public void Execute(object obj) {}

        public IUpdater Updater { get; set; }
        
        public void UpdateUi()
        {
            _processes = new ObservableCollection<MyProcess>(Updater.ActiveProcesses);
            OnPropertyChanged(nameof(Processes));
        }
    }
}