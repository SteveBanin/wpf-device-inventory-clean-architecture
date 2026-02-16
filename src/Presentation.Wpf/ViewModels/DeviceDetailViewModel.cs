using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Application.Devices;
using Application.Validation;
using Domain.Entities;
using Presentation.Wpf.Services;

namespace Presentation.Wpf.ViewModels;

public sealed class DeviceDetailViewModel : ObservableObject
{
    private readonly CreateDeviceCommand _create;
    private readonly UpdateDeviceCommand _update;
    private readonly IDeviceValidator _validator;

    public event Action<bool>? RequestClose; // bool = shouldReloadList

    // "Touched" flags:
    // We only show validation errors after the user has interacted with a field.
    private bool _nameTouched;
    private bool _serialTouched;

    // When we populate fields during BeginCreate/BeginEdit we don't want to mark them "touched".
    private bool _isInitializing;

    // Raw validation messages from the Application-layer validator (always computed).
    private string? _nameErrorRaw;
    private string? _serialErrorRaw;

    // UI-facing validation messages:
    // Only show after the field was touched.
    public string? NameError => _nameTouched ? _nameErrorRaw : null;
    public string? SerialError => _serialTouched ? _serialErrorRaw : null;

    private int _id;
    public int Id
    {
        get => _id;
        private set => SetProperty(ref _id, value);
    }

    private string _name = "";
    public string Name
    {
        get => _name;
        set
        {
            if (!SetProperty(ref _name, value)) return;

            if (!_isInitializing)
                _nameTouched = true;

            ValidateAndRefresh();
        }
    }

    private string _serialNumber = "";
    public string SerialNumber
    {
        get => _serialNumber;
        set
        {
            if (!SetProperty(ref _serialNumber, value)) return;

            if (!_isInitializing)
                _serialTouched = true;

            ValidateAndRefresh();
        }
    }

    private string _location = "";
    public string Location
    {
        get => _location;
        set
        {
            if (!SetProperty(ref _location, value)) return;
            // Location isn't required in our minimal rules, so no "touched" needed.
        }
    }

    private DateTime? _lastServiceDate;
    public DateTime? LastServiceDate
    {
        get => _lastServiceDate;
        set => SetProperty(ref _lastServiceDate, value);
    }

    private string? _description;
    public string? Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    // Helpful UX: explain why Save is disabled (tooltip works even when disabled)
    public string SaveTooltip
    {
        get
        {
            if (CanSave()) return "Save device";
            // Show required fields in tooltip even if errors are hidden (not touched yet)
            return "Fill required fields: Name and Serial Number.";
        }
    }

    public DeviceDetailViewModel(
        CreateDeviceCommand create,
        UpdateDeviceCommand update,
        IDeviceValidator validator)
    {
        _create = create;
        _update = update;
        _validator = validator;

        // ICommand concept:
        // - Execute runs when button is clicked
        // - CanExecute controls whether the button is enabled
        SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
        CancelCommand = new RelayCommand(() => RequestClose?.Invoke(false));

        ValidateAndRefresh();
    }

    public void BeginCreate()
    {
        _isInitializing = true;

        Id = 0;
        _nameTouched = false;
        _serialTouched = false;

        Name = "";
        SerialNumber = "";
        Location = "";
        LastServiceDate = null;
        Description = null;

        _isInitializing = false;

        ValidateAndRefresh();
    }

    public void BeginEdit(Device device)
    {
        _isInitializing = true;

        Id = device.Id;
        _nameTouched = false;
        _serialTouched = false;

        Name = device.Name;
        SerialNumber = device.SerialNumber;
        Location = device.Location;
        LastServiceDate = device.LastServiceDate;
        Description = device.Description;

        _isInitializing = false;

        ValidateAndRefresh();
    }

    private void ValidateAndRefresh()
    {
        Validate();

        // These are computed properties, so we must manually notify the UI to refresh them
        RaisePropertyChanged(nameof(NameError));
        RaisePropertyChanged(nameof(SerialError));
        RaisePropertyChanged(nameof(SaveTooltip));

        // Re-evaluate CanExecute for Save button
        (SaveCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
    }

    private void Validate()
    {
        var snapshot = new Device
        {
            Id = Id,
            Name = Name,
            SerialNumber = SerialNumber,
            Location = Location,
            LastServiceDate = LastServiceDate,
            Description = Description
        };

        var result = _validator.Validate(snapshot);

        _nameErrorRaw = result.Get("Name");
        _serialErrorRaw = result.Get("SerialNumber");
    }

    private bool CanSave()
    {
        // Important: Save should depend on RAW validation (not "touched" display).
        return string.IsNullOrWhiteSpace(_nameErrorRaw)
            && string.IsNullOrWhiteSpace(_serialErrorRaw);
    }

    private async Task SaveAsync()
    {
        // Just in case
        ValidateAndRefresh();
        if (!CanSave())
        {
            // User attempted to save → reveal required field errors
            _nameTouched = true;
            _serialTouched = true;

            ValidateAndRefresh();
            return;
        }

        var device = new Device
        {
            Id = Id,
            Name = Name.Trim(),
            SerialNumber = SerialNumber.Trim(),
            Location = Location.Trim(),
            LastServiceDate = LastServiceDate,
            Description = string.IsNullOrWhiteSpace(Description) ? null : Description.Trim()
        };

        if (device.Id == 0)
            await _create.ExecuteAsync(device);
        else
            await _update.ExecuteAsync(device);

        RequestClose?.Invoke(true);
    }
}
