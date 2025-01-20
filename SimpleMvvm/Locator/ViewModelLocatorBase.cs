using System;
using System.Collections.Generic;

namespace SimpleMvvm.Locator
{
    /// <summary>
    /// Base class for ViewModelLocator
    /// </summary>
    public abstract class ViewModelLocatorBase
    {
        private class InstanseEntry
        {
            public ViewModelBase Instance { get; set; }
            public Func<ViewModelBase> Factory { get; set; }
        }

        private readonly Dictionary<Type, InstanseEntry> _viewModelRegistry
            = new Dictionary<Type, InstanseEntry>();

        /// <summary>
        /// Register a ViewModel instance.
        /// </summary>
        public void Register<TViewModel>(TViewModel viewModel) where TViewModel : ViewModelBase
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            var entry = new InstanseEntry
            { Instance = viewModel };
            _viewModelRegistry.Add(typeof(TViewModel), entry);
        }

        /// <summary>
        /// Register a ViewModel instance with a default constructor.
        /// </summary>
        public void Register<TViewModel>(bool createImmediately = false) where TViewModel : ViewModelBase, new()
        {
            var entry = new InstanseEntry
            { Factory = () => new TViewModel() };

            if (createImmediately)
                entry.Instance = entry.Factory();
            _viewModelRegistry.Add(typeof(TViewModel), entry);
        }

        /// <summary>
        /// Register a ViewModel instance with a factory method.
        /// </summary>
        public void Register<TViewModel>(Func<TViewModel> factory, bool createImmediately = false) where TViewModel : ViewModelBase
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var entry = new InstanseEntry
            { Factory = factory };

            if (createImmediately)
                entry.Instance = entry.Factory();
            _viewModelRegistry.Add(typeof(TViewModel), entry);
        }

        /// <summary>
        /// Unregister the ViewModel.
        /// </summary>
        public void Unregister<TViewModel>() where TViewModel : ViewModelBase
        {
            if (!_viewModelRegistry.ContainsKey(typeof(TViewModel)))
                throw new KeyNotFoundException();

            _viewModelRegistry.Remove(typeof(TViewModel));
        }

        /// <summary>
        /// Get the instance of ViewModel.
        /// </summary>
        public ViewModelBase GetInstance(Type type)
        {
            if (!_viewModelRegistry.ContainsKey(type))
                throw new KeyNotFoundException();

            var entry = _viewModelRegistry[type];
            if (entry.Instance == null && entry.Factory != null)
            {
                lock (entry)
                {
                    if (entry.Instance == null && entry.Factory != null)
                        entry.Instance = entry.Factory();
                }
            }
            return entry.Instance;
        }

        /// <summary>
        /// Get the instance of ViewModel.
        /// </summary>
        public TViewModel GetInstance<TViewModel>() where TViewModel : ViewModelBase
        {
            return (TViewModel)GetInstance(typeof(TViewModel));
        }
    }
}
