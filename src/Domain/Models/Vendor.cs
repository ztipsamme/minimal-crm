using System;

namespace Domain.Models;

public class Vendor : User
{
    public Vendor(string name, string phoneNumber, string email)
    {

        Name = name;
        PhoneNumber = phoneNumber;
        Email = email;

        Role = "vendor";
        Id = Guid.NewGuid().ToString();
        VendorId = Id;
    }
}