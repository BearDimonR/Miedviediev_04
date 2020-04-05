using System;
using Miedviediev_04.Views;

namespace Miedviediev_04.Navigation
{
    internal class InitializationNavigationModel : BaseNavigationModel
    {
        public InitializationNavigationModel(IContentOwner contentOwner) : base(contentOwner)
        {
            
        }
   
        protected override void InitializeView(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.GridView:
                    ViewsDictionary.Add(viewType, new GridView());
                    break;
                case ViewType.ProcessView:
                    ViewsDictionary.Add(viewType, new ProcessView());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }
    }
}
