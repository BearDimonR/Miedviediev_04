using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Miedviediev_04.Managers;
using Miedviediev_04.Models;

namespace Miedviediev_04.Sorting
{
    public class DataGridSortBehaviour
    {
        public static readonly DependencyProperty AllowCustomSortProperty =
            DependencyProperty.RegisterAttached("AllowCustomSort", typeof(bool),
                typeof(DataGridSortBehaviour), new UIPropertyMetadata(false, OnAllowCustomSortChanged));

        public static readonly DependencyProperty SortField =
            DependencyProperty.RegisterAttached("SortField", typeof(SortField),
                typeof(DataGridSortBehaviour), null);

        public static SortField GetSortField(DataGridColumn grid)
        {
            return (SortField) grid.GetValue(SortField);
        }

        public static void SetSortField(DataGrid grid, SortField value)
        {
            grid.SetValue(SortField, value);
        }

        public static bool GetAllowCustomSort(DataGrid grid)
        {
            return (bool) grid.GetValue(AllowCustomSortProperty);
        }

        public static void SetAllowCustomSort(DataGrid grid, bool value)
        {
            grid.SetValue(AllowCustomSortProperty, value);
        }

        private static void OnAllowCustomSortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var existing = d as DataGrid;
            if (existing == null) return;

            var oldAllow = (bool) e.OldValue;
            var newAllow = (bool) e.NewValue;

            if (!oldAllow && newAllow)
            {
                existing.Sorting += HandleSorting;
            }
            else
            {
                existing.Sorting -= HandleSorting;
            }
        }

        private static void HandleSorting(object sender, DataGridSortingEventArgs e)
        {
            lock (ProcessUpdater.Locker)
            {
                var dataGrid = sender as DataGrid;
                if (dataGrid == null || !GetAllowCustomSort(dataGrid)) return;

                e.Handled = true;

                SortField sortField = GetSortField(e.Column);

                ListSortDirection? sortDirection = e.Column.SortDirection;

                ObservableCollection<MyProcess> persons = UpdateManager<MyProcess>.Instance.Owner.CurrCollection;
                IOrderedEnumerable<MyProcess> result;
                switch (sortField)
                {
                    case Sorting.SortField.Name:
                        if (sortDirection == ListSortDirection.Ascending)
                            result = from u in persons
                                orderby u.Name
                                select u;
                        else
                            result = from u in persons
                                orderby u.Name descending
                                select u;
                        break;
                    case Sorting.SortField.Cpu:
                        if (sortDirection == ListSortDirection.Ascending)
                            result = from u in persons
                                orderby u.Cpu
                                select u;
                        else
                            result = from u in persons
                                orderby u.Cpu descending
                                select u;
                        break;
                    case Sorting.SortField.Id:
                        if (sortDirection == ListSortDirection.Ascending)
                            result = from u in persons
                                orderby u.Id
                                select u;
                        else
                            result = from u in persons
                                orderby u.Id descending
                                select u;
                        break;
                    case Sorting.SortField.Ram:
                        if (sortDirection == ListSortDirection.Ascending)
                            result = from u in persons
                                orderby u.Ram
                                select u;
                        else
                            result = from u in persons
                                orderby u.Ram descending
                                select u;
                        break;
                    case Sorting.SortField.Responding:
                        if (sortDirection == ListSortDirection.Ascending)
                            result = from u in persons
                                orderby u.Responding
                                select u;
                        else
                            result = from u in persons
                                orderby u.Responding descending
                                select u;
                        break;
                    case Sorting.SortField.Streams:
                        if (sortDirection == ListSortDirection.Ascending)
                            result = from u in persons
                                orderby u.Threads
                                select u;
                        else
                            result = from u in persons
                                orderby u.Threads descending
                                select u;
                        break;
                    case Sorting.SortField.Time:
                        if (sortDirection == ListSortDirection.Ascending)
                            result = from u in persons
                                orderby u.StartTime
                                select u;
                        else
                            result = from u in persons
                                orderby u.StartTime descending
                                select u;
                        break;
                    case Sorting.SortField.User:
                        if (sortDirection == ListSortDirection.Ascending)
                            result = from u in persons
                                orderby u.User
                                select u;
                        else
                            result = from u in persons
                                orderby u.User descending
                                select u;
                        break;
                    case Sorting.SortField.StartFrom:
                        if (sortDirection == ListSortDirection.Ascending)
                            result = from u in persons
                                orderby u.StartFrom
                                select u;
                        else
                            result = from u in persons
                                orderby u.StartFrom descending
                                select u;
                        break;
                    default:
                        throw new ArgumentException("Indefined SortingField!");
                }

                UpdateManager<MyProcess>.Instance.Owner.CurrCollection = new ObservableCollection<MyProcess>(result);
                UpdateManager<MyProcess>.Instance.Owner.UpdateUi();
                e.Column.SortDirection = sortDirection == ListSortDirection.Descending || sortDirection == null
                    ? ListSortDirection.Ascending
                    : ListSortDirection.Descending;
                LoaderManager.Instance.HideLoader();
            }
        }
    }
}