using System.Windows;
using System.Windows.Controls;
using Miedviediev_04.Managers;
using Miedviediev_04.Navigation;

namespace Miedviediev_04.ViewModels
{
    public class MainVm : BaseVm, ILoaderOwner, IContentOwner
    {
        public ContentControl ContentControl { get; }

        private Visibility _loaderVisibility;

        public Visibility LoaderVisibility
        {
            get => _loaderVisibility;
            set
            {
                _loaderVisibility = value;
                OnPropertyChanged(nameof(LoaderVisibility));
            }
        }

        private bool _isControlEnabled;

        public bool IsControlEnabled
        {
            //used in SignUp
            get => _isControlEnabled;
            set
            {
                _isControlEnabled = value;
                OnPropertyChanged(nameof(IsControlEnabled));
            }
        }

        public MainVm(ContentControl control)
        {
            LoaderManager.Instance.Initialize(this);
            LoaderManager.Instance.HideLoader();
            ContentControl = control;
            NavigationManager.Instance.Initialize(new InitializationNavigationModel(this));
            NavigationManager.Instance.Navigate(ViewType.GridView);
        }
    }
}