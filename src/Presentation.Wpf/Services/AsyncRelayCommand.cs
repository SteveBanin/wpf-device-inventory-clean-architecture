using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Presentation.Wpf.Services
{
    /// <summary>
    /// ICommand = WPF "button action" contract:
    /// - Execute() runs when button is clicked
    /// - CanExecute() controls whether the button is enabled/disabled
    /// - CanExecuteChanged tells WPF to re-check CanExecute()
    ///
    /// AsyncRelayCommand wraps an async Task method for WPF commands.
    /// </summary>
    public sealed class AsyncRelayCommand : ICommand
    {
        private readonly Func<Task> _executeAsync;
        private readonly Func<bool>? _canExecute;

        private bool _isExecuting;

        public AsyncRelayCommand(Func<Task> executeAsync, Func<bool>? canExecute = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
            => !_isExecuting && (_canExecute?.Invoke() ?? true);

        public async void Execute(object? parameter)
        {
            if (!CanExecute(parameter))
                return;

            try
            {
                _isExecuting = true;
                RaiseCanExecuteChanged(); // disables button while running
                await _executeAsync();
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged(); // re-enables button
            }
        }

        /// <summary>
        /// Forces WPF to re-check CanExecute() and refresh button enabled state.
        /// </summary>
        public void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
