using NUnit.Framework;
using Domain.Entities;

namespace UnitTests.Domain;

public class DeviceTests
{
    [Test]
    public void Device_ShouldInitializeWithDefaultValues()
    {
        var device = new Device();

        Assert.That(device.Id, Is.EqualTo(0));
        Assert.That(device.Name, Is.Not.Null);
        Assert.That(device.SerialNumber, Is.Not.Null);
        Assert.That(device.Location, Is.Not.Null);
        Assert.That(device.LastServiceDate, Is.Null);
    }

    [Test]
    public void Device_ShouldAllowSettingProperties()
    {
        var dt = new System.DateTime(2026, 2, 15);

        var device = new Device
        {
            Name = "Laptop",
            SerialNumber = "SN-001",
            Location = "Office",
            LastServiceDate = dt
        };

        Assert.That(device.Name, Is.EqualTo("Laptop"));
        Assert.That(device.SerialNumber, Is.EqualTo("SN-001"));
        Assert.That(device.Location, Is.EqualTo("Office"));
        Assert.That(device.LastServiceDate, Is.EqualTo(dt));
    }
}

