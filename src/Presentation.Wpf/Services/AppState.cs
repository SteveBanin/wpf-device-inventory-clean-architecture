using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Presentation.Wpf.Services
{
    /// <summary>
    /// AppState = shared UI state for the whole app.
    ///
    /// Why it exists:
    /// - Multiple ViewModels need to share state (navigation, loading flags, messages).
    /// - Instead of static globals, we inject this class (testable + clean).
    ///
    /// Why INotifyPropertyChanged:
    /// - WPF data binding listens for PropertyChanged and updates the UI automatically.
    /// </summary>
    public sealed class AppState : INotifyPropertyChanged
    {
        private object? _currentViewModel;

        /// <summary>
        /// Navigation state:
        /// MainWindow binds to this. When it changes, the screen changes.
        /// </summary>
        public object? CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (ReferenceEquals(_currentViewModel, value)) return;
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        private bool _isBusy;

        /// <summary>
        /// Global "busy" flag for showing spinners / disabling actions during DB work.
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        private string? _statusMessage;

        /// <summary>
        /// Short message like "Saved" or "Loaded devices".
        /// </summary>
        public string? StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage == value) return;
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        private string? _errorMessage;

        /// <summary>
        /// Error message for the UI.
        /// </summary>
        public string? ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage == value) return;
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void ClearMessages()
        {
            StatusMessage = null;
            ErrorMessage = null;
        }
    }
}
