using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimpleMvvm
{
    /// <summary>
    /// Notifies clients that a property value has changed.
    /// </summary>
    public abstract class NotificationObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify property changed.
        /// </summary>
        protected void RaisePropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Notify property changed.
        /// </summary>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            RaisePropertyChanged(this, propertyName);
        }

        /// <summary>
        /// Updates field value and notifies property changed.
        /// </summary>
        protected void UpdateValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            //if ((field == null && value == null) ||
            //    (field != null && field.Equals(value)) ||
            //    (value != null && value.Equals(value)))
            //    return;

            field = value;
            RaisePropertyChanged(this, propertyName);
        }
    }
}
