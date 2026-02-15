using System;
namespace Domain.Entities;



public class Device
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string SerialNumber { get; set; } = "";

    public string Location { get; set; } = "";

    public DateTime? LastServiceDate { get; set; }

    public string? Description { get; set; } 
}
