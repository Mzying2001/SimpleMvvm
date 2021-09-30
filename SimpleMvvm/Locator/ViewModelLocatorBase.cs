using System;
using System.Collections.Generic;

namespace SimpleMvvm.Locator
{
    /// <summary>
    /// Base class for ViewModelLocator
    /// </summary>
    public abstract class ViewModelLocatorBase
    {
        private static readonly Dictionary<Type, ViewModelBase> _dic
            = new Dictionary<Type, ViewModelBase>();

        /// <summary>
        /// Register a global ViewModel instance.
        /// </summary>
        protected static void Register<ViewModelType>() where ViewModelType : ViewModelBase, new()
        {
            _dic.Add(typeof(ViewModelType), new ViewModelType());
        }

        /// <summary>
        /// Unregister the ViewModel.
        /// </summary>
        protected static void Unregister<ViewModelType>() where ViewModelType : ViewModelBase
        {
            _dic.Remove(typeof(ViewModelType));
        }

        /// <summary>
        /// Get the instance of ViewModel.
        /// </summary>
        protected static ViewModelType GetInstance<ViewModelType>() where ViewModelType : ViewModelBase
        {
            return (ViewModelType)_dic[typeof(ViewModelType)];
        }
    }
}
