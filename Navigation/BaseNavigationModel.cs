﻿using System.Collections.Generic;

namespace Miedviediev_04.Navigation
{
    internal abstract class BaseNavigationModel : INavigationModel
    {
        protected BaseNavigationModel(IContentOwner contentOwner)
        {
            ContentOwner = contentOwner;
            ViewsDictionary = new Dictionary<ViewType, INavigatable>();
        }

        protected IContentOwner ContentOwner { get; }

        protected Dictionary<ViewType, INavigatable> ViewsDictionary { get; }

        public void Navigate(ViewType viewType)
        {
            if (!ViewsDictionary.ContainsKey(viewType))
                InitializeView(viewType);
            ContentOwner.ContentControl.Content = ViewsDictionary[viewType];
        }

        protected abstract void InitializeView(ViewType viewType);

        public void Execute(ViewType viewType, object obj)
        {
            if (!ViewsDictionary.ContainsKey(viewType))
                InitializeView(viewType);
            ViewsDictionary[viewType].NavigatableDataContext.Execute(obj);
        }
    }
}
