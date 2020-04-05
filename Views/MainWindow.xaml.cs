using Miedviediev_04.ViewModels;

namespace Miedviediev_04.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVm(ContentControl);
        }
    }
}