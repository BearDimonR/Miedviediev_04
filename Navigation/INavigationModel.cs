namespace Miedviediev_04.Navigation
{
    internal enum ViewType
    {
        GridView,
        ProcessView
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);

        void Execute(ViewType viewType, object obj);
    }
}
