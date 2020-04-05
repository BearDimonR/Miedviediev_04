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

        public static SortField? CurrSortField { get; private set; }
        public static ListSortDirection? CurrSortDirection { get; private set; }

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
                LoaderManager.Instance.ShowLoader();
                var dataGrid = sender as DataGrid;
                if (dataGrid == null || !GetAllowCustomSort(dataGrid)) return;
                e.Handled = true;
                CurrSortDirection = e.Column.SortDirection;
                CurrSortField = GetSortField(e.Column);
                SortValues();
                e.Column.SortDirection = CurrSortDirection == ListSortDirection.Descending || CurrSortDirection == null
                    ? ListSortDirection.Ascending
                    : ListSortDirection.Descending;
                LoaderManager.Instance.HideLoader();
            }
        }

        public static void SortValues()
        {
            ObservableCollection<MyProcess> collection = UpdateManager<MyProcess>.Instance.Owner.CurrCollection;
            IOrderedEnumerable<MyProcess> result;
            switch (CurrSortField)
            {
                case Sorting.SortField.Name:
                    if (CurrSortDirection == ListSortDirection.Ascending)
                        result = from u in collection
                            orderby u.Name
                            select u;
                    else
                        result = from u in collection
                            orderby u.Name descending
                            select u;
                    break;
                case Sorting.SortField.Cpu:
                    if (CurrSortDirection == ListSortDirection.Ascending)
                        result = from u in collection
                            orderby u.Cpu
                            select u;
                    else
                        result = from u in collection
                            orderby u.Cpu descending
                            select u;
                    break;
                case Sorting.SortField.Id:
                    if (CurrSortDirection == ListSortDirection.Ascending)
                        result = from u in collection
                            orderby u.Id
                            select u;
                    else
                        result = from u in collection
                            orderby u.Id descending
                            select u;
                    break;
                case Sorting.SortField.Ram:
                    if (CurrSortDirection == ListSortDirection.Ascending)
                        result = from u in collection
                            orderby u.Ram
                            select u;
                    else
                        result = from u in collection
                            orderby u.Ram descending
                            select u;
                    break;
                case Sorting.SortField.Responding:
                    if (CurrSortDirection == ListSortDirection.Ascending)
                        result = from u in collection
                            orderby u.Responding
                            select u;
                    else
                        result = from u in collection
                            orderby u.Responding descending
                            select u;
                    break;
                case Sorting.SortField.Streams:
                    if (CurrSortDirection == ListSortDirection.Ascending)
                        result = from u in collection
                            orderby u.Threads
                            select u;
                    else
                        result = from u in collection
                            orderby u.Threads descending
                            select u;
                    break;
                case Sorting.SortField.Time:
                    if (CurrSortDirection == ListSortDirection.Ascending)
                        result = from u in collection
                            orderby u.StartTime
                            select u;
                    else
                        result = from u in collection
                            orderby u.StartTime descending
                            select u;
                    break;
                case Sorting.SortField.User:
                    if (CurrSortDirection == ListSortDirection.Ascending)
                        result = from u in collection
                            orderby u.User
                            select u;
                    else
                        result = from u in collection
                            orderby u.User descending
                            select u;
                    break;
                case Sorting.SortField.StartFrom:
                    if (CurrSortDirection == ListSortDirection.Ascending)
                        result = from u in collection
                            orderby u.StartFrom
                            select u;
                    else
                        result = from u in collection
                            orderby u.StartFrom descending
                            select u;
                    break;
                default:
                    throw new ArgumentException("Indefined SortingField!");
            }
            int index = 0;
            foreach (var process in result)
            {
                UpdateManager<MyProcess>.Instance.Owner.CurrCollection
                    .Move(UpdateManager<MyProcess>.Instance.Owner.CurrCollection.IndexOf(process), index);
            }
            //UpdateManager<MyProcess>.Instance.Owner.UpdateUi();
        }

        public static void SortValue(MyProcess activeProcess, int index)
        {
            // if (index == 0 || CurrSortDirection == null || CurrSortField == null)
            //     return;
            // for (int i = index - 1; i >= 0; --i)
            // {
            //     int res = Compare(activeProcess, UpdateManager<MyProcess>.Instance.Owner.CurrCollection[index]);
            //     switch (CurrSortDirection)
            //     {
            //         case ListSortDirection.Ascending:
            //             if (res > 0)
            //             {
            //                 UpdateManager<MyProcess>.Instance.Owner.CurrCollection.Move(i + 1, i);
            //             }
            //             break;
            //         case ListSortDirection.Descending:
            //             if (res < 0)
            //             {
            //                 UpdateManager<MyProcess>.Instance.Owner.CurrCollection.Move(i + 1, i);
            //             }
            //             break;
            //         default:
            //             return;
            //     }
            // }
        }

        private static int Compare(MyProcess u, MyProcess v)
        {
            switch (CurrSortField)
            {
                case Sorting.SortField.Name:
                    return String.Compare(u.Name, v.Name, StringComparison.Ordinal);
                case Sorting.SortField.Cpu:
                    return u.Cpu.CompareTo(v.Cpu);
                case Sorting.SortField.Id:
                    return u.Id.CompareTo(v.Id);
                case Sorting.SortField.Ram:
                    return u.Ram.CompareTo(v.Ram);
                case Sorting.SortField.Responding:
                    return u.Responding.CompareTo(v.Responding);
                case Sorting.SortField.Streams:
                    return u.Threads.CompareTo(v.Threads);
                case Sorting.SortField.Time:
                    return u.StartTime.CompareTo(v.StartTime);
                case Sorting.SortField.User:
                    return String.Compare(u.User, v.User, StringComparison.Ordinal);
                case Sorting.SortField.StartFrom:
                    return String.Compare(u.StartFrom, v.StartFrom, StringComparison.Ordinal);
                default:
                    throw new ArgumentException("Indefined SortingField!");
            }
        }
    }
}