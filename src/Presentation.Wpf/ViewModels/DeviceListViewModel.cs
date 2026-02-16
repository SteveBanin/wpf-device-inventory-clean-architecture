using Application.Devices;
using Domain.Entities;
using Presentation.Wpf.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace Presentation.Wpf.ViewModels;

public sealed class DeviceListViewModel : ObservableObject
{
    private readonly GetDevicesQuery _getDevices;
    private readonly DeleteDeviceCommand _deleteDevice;

    // AppState is shared UI state (navigation/status/busy flags).
    private readonly AppState _appState;

    public AppState AppState => _appState;

    // ObservableCollection notifies the UI when items are added/removed.
    public ObservableCollection<Device> Devices { get; } = new();

    private Device? _selectedDevice;
    public Device? SelectedDevice
    {
        get => _selectedDevice;
        set
        {
            if (!SetProperty(ref _selectedDevice, value)) return;

            // Selection affects whether Edit/Delete buttons should be enabled.
            (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DeleteCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
        }
    }

    public event Action? RequestCreate;
    public event Action<Device>? RequestEdit;

    public ICommand LoadCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }

    // ✅ Updated: AppState added
    public DeviceListViewModel(
        GetDevicesQuery getDevices,
        DeleteDeviceCommand deleteDevice,
        AppState appState)
    {
        _getDevices = getDevices;
        _deleteDevice = deleteDevice;
        _appState = appState;

        LoadCommand = new AsyncRelayCommand(LoadAsync);

        AddCommand = new RelayCommand(() => RequestCreate?.Invoke());

        EditCommand = new RelayCommand(
            execute: () =>
            {
                if (SelectedDevice is not null)
                    RequestEdit?.Invoke(SelectedDevice);
            },
            canExecute: () => SelectedDevice is not null
        );

        DeleteCommand = new AsyncRelayCommand(
            executeAsync: DeleteSelectedAsync,
            canExecute: () => SelectedDevice is not null
        );
    }


    public Task LoadAsync() => ReloadAsync(clearMessages: true, setLoadedMessage: true);

    private async Task ReloadAsync(bool clearMessages, bool setLoadedMessage)
    {
        _appState.IsBusy = true;
        if (clearMessages) _appState.ClearMessages();

        try
        {
            Devices.Clear();

            var items = await _getDevices.ExecuteAsync();

            foreach (var d in items)
                Devices.Add(d);

            // Optional: avoid selection pointing to a removed/old object after reload
            SelectedDevice = null;

            if (setLoadedMessage)
                _appState.StatusMessage = $"Loaded {Devices.Count} devices.";
        }
        catch (Exception)
        {
            _appState.ErrorMessage = "Failed to load devices.";
        }
        finally
        {
            _appState.IsBusy = false;
        }
    }

    private async Task DeleteSelectedAsync()
    {
        if (SelectedDevice is null) return;

        // 1) Confirm with user before deleting (simple + effective for a showcase)
        var result = MessageBox.Show(
            $"Are you sure you want to delete '{SelectedDevice.Name}'?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes)
            return;

        // 2) Busy flag + messages are shared via AppState
        _appState.IsBusy = true;
        _appState.ClearMessages();

        try
        {
            await _deleteDevice.ExecuteAsync(SelectedDevice.Id);

            // Reload list WITHOUT clearing/overwriting the "deleted" message
            await ReloadAsync(clearMessages: false, setLoadedMessage: false);

            _appState.StatusMessage = "Device deleted.";
        }
        catch (Exception)
        {
            _appState.ErrorMessage = "Failed to delete device.";
        }
        finally
        {
            _appState.IsBusy = false;
        }
    }


}
