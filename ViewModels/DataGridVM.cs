﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Miedviediev_04.Managers;
using Miedviediev_04.Models;
using Miedviediev_04.Navigation;

namespace Miedviediev_04.ViewModels
{
    public class DataGridVm:BaseVm, INavigatableDataContext, IUpdaterOwner<MyProcess>
    {
        private AsyncObservableCollection<MyProcess> _processes;
        public AsyncObservableCollection<MyProcess> CurrCollection
        {
            get => _processes;
            set
            {
                _processes = value;
                OnPropertyChanged(nameof(CurrCollection));
            }
        }

        public SortedSet<int> CurrIds { get; set; }

        public RelayCommand<MyProcess> InfoCommand { get; } 
        public RelayCommand<MyProcess> FolderCommand { get; }
        public RelayCommand<MyProcess> StopCommand { get; }

        public DataGridVm()
        {
            InfoCommand = new RelayCommand<MyProcess>(NavigateInfo);
            FolderCommand = new RelayCommand<MyProcess>(OpenFolder);
            StopCommand = new RelayCommand<MyProcess>(StopProcess);
            UpdateManager<MyProcess>.Instance.Initialize(this);
            Updater = new ProcessUpdater(1000, 1);
            _processes = ProcessUpdater.GetProcesses();
            CurrIds = ProcessUpdater.GetCurrIds();
            Updater.StartUpdate();
        }

        private void StopProcess(MyProcess obj)
        {
            try
            {
                obj.OriginProcess.Kill();
                Updater.RemoveProcess(obj);
            }
            catch (Exception e)
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
            OnPropertyChanged(nameof(CurrCollection));
        }
    }
}