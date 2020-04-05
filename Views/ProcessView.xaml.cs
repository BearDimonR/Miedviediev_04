using System.Windows.Controls;
using Miedviediev_04.Navigation;

namespace Miedviediev_04.Views
{
    public partial class ProcessView : INavigatable
    {
        public INavigatableDataContext NavigatableDataContext => (INavigatableDataContext) DataContext;

        public ProcessView()
        {
            InitializeComponent();
        }
    }
}