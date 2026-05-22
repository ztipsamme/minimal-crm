using System;

namespace Domain.Models;

public class Customer : User
{
    public Customer(string name, string phoneNumber, string email, string title, Address address, string vendorId)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        Email = email;
        Title = title;
        Address = address;
        VendorId = vendorId;

        Role = "customer";
    }
}