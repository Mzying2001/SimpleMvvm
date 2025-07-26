using System;
using System.Threading;

namespace SimpleMvvm
{
    /// <summary>
    /// Base class for ViewModels.
    /// </summary>
    public abstract class ViewModelBase : NotificationObject
    {
        private SynchronizationContext _syncContext
            = SynchronizationContext.Current;

        /// <summary>
        /// Instantiate the ViewModel and call the Init method.
        /// </summary>
        public ViewModelBase()
        {
            Init();
        }

        /// <summary>
        /// Set the UI thread context.
        /// </summary>
        protected void SetUIThreadContext(SynchronizationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            _syncContext = context;
        }

        /// <summary>
        /// Invoke an action on the UI thread.
        /// </summary>
        protected void InvokeOnUIThread(Action action)
        {
            _syncContext.Send(_ => action(), null);
        }

        /// <summary>
        /// Invoke an action on the UI thread asynchronously.
        /// </summary>
        protected void InvokeOnUIThreadAsync(Action action)
        {
            _syncContext.Post(_ => action(), null);
        }

        /// <summary>
        /// Use this method to initialize.
        /// </summary>
        protected virtual void Init() { }
    }
}
