using CommunityToolkit.Mvvm.ComponentModel;
using Domain.Entities;

namespace Presentation.Wpf.ViewModels;

public partial class DeviceDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private Device? model;

    // Null-safe: avoids WPF binding lifecycle crashes
    public bool IsNew => (Model?.Id ?? 0) <= 0;
}

