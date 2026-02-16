using Presentation.Wpf.Services;

namespace Presentation.Wpf.ViewModels;

public sealed class MainViewModel : ObservableObject
{
    private readonly AppState _appState;

    /// <summary>
    /// Expose AppState so the View can bind to it (AppState.CurrentViewModel).
    /// </summary>
    public AppState AppState => _appState;

    public DeviceListViewModel ListVm { get; }
    public DeviceDetailViewModel DetailVm { get; }

    public MainViewModel(AppState appState, DeviceListViewModel listVm, DeviceDetailViewModel detailVm)
    {
        _appState = appState;

        ListVm = listVm;

        DetailVm = detailVm;

        // Child VMs raise events, MainViewModel handles navigation.
        ListVm.RequestCreate += () =>
        {
            DetailVm.BeginCreate();
            _appState.CurrentViewModel = DetailVm;
        };

        ListVm.RequestEdit += (device) =>
        {
            DetailVm.BeginEdit(device);
            _appState.CurrentViewModel = DetailVm;
        };

        DetailVm.RequestClose += async (reload) =>
        {
            _appState.CurrentViewModel = ListVm;
            if (reload)
                await ListVm.LoadAsync();
        };

           _appState.CurrentViewModel = ListVm;

        // Fire-and-forget initial load (UI stays responsive).
        _ = ListVm.LoadAsync();
    }
}
