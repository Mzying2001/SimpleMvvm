using System.Collections.Generic;
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
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            RaisePropertyChanged(this, propertyName);
        }

        /// <summary>
        /// Updates field value and notifies property changed.
        /// </summary>
        /// <returns>True if the value was updated, false otherwise.</returns>
        protected bool UpdateValue<T>(ref T field, T value, bool forceUpdate, [CallerMemberName] string propertyName = "")
        {
            if (forceUpdate || !EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                RaisePropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates field value and notifies property changed.
        /// </summary>
        /// <returns>True if the value was updated, false otherwise.</returns>
        protected bool UpdateValue<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            return UpdateValue(ref field, value, false, propertyName);
        }
    }
}
