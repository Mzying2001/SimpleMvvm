namespace SimpleMvvm
{
    /// <summary>
    /// Base class for ViewModels.
    /// </summary>
    public abstract class ViewModelBase : NotificationObject
    {
        /// <summary>
        /// Instantiate the ViewModel and call the Init method.
        /// </summary>
        public ViewModelBase()
        {
            Init();
        }

        /// <summary>
        /// Use this method to initialize.
        /// </summary>
        protected virtual void Init() { }
    }
}
