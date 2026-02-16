using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Presentation.Wpf.ViewModels
{
    /// <summary>
    /// Base class for ViewModels.
    ///
    /// Why INotifyPropertyChanged:
    /// - WPF data binding listens for PropertyChanged.
    /// - When a property changes, the UI updates automatically.
    ///
    /// Why SetProperty:
    /// - avoids duplicate code in every property setter
    /// - only raises PropertyChanged when the value actually changes
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises PropertyChanged for a property name.
        /// Some scenarios need this even when there is no backing field set,
        /// e.g. computed properties like NameError or SaveTooltip.
        /// </summary>
        protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}
