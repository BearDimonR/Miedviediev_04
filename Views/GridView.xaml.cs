using System.Windows.Controls;
using System.Windows.Data;
using Miedviediev_04.Navigation;

namespace Miedviediev_04.Views
{
    public partial class GridView : INavigatable
    {
        public GridView()
        {
            InitializeComponent();
        }

        public INavigatableDataContext NavigatableDataContext => (INavigatableDataContext)DataContext;
    }
}