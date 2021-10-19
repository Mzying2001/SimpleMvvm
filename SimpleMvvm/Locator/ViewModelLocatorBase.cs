using System;
using System.Collections.Generic;

namespace SimpleMvvm.Locator
{
    /// <summary>
    /// Base class for ViewModelLocator
    /// </summary>
    public abstract class ViewModelLocatorBase
    {
        private readonly Dictionary<Type, ViewModelBase> _dic
            = new Dictionary<Type, ViewModelBase>();

        /// <summary>
        /// Register a ViewModel instance.
        /// </summary>
        protected void Register<TViewModel>() where TViewModel : ViewModelBase, new()
        {
            _dic.Add(typeof(TViewModel), null);
        }

        /// <summary>
        /// Unregister the ViewModel.
        /// </summary>
        protected void Unregister<TViewModel>() where TViewModel : ViewModelBase, new()
        {
            _dic.Remove(typeof(TViewModel));
        }

        /// <summary>
        /// Get the instance of ViewModel.
        /// </summary>
        protected TViewModel GetInstance<TViewModel>() where TViewModel : ViewModelBase, new()
        {
            var type = typeof(TViewModel);
            if (_dic[type] == null)
            {
                var vm = new TViewModel();
                _dic[type] = vm;
                return vm;
            }
            else
            {
                return (TViewModel)_dic[type];
            }
        }
    }
}
