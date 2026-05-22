using System;

namespace Api.Dto
{
    public class UserDto
    {
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string VendorId { get; set; } = null!;
    }

    // public class UserDto
    // {
    //     public string? Id { get; set; }
    //     public string? Role { get; set; }

    //     public string? Name { get; set; }
    //     public string? PhoneNumber { get; set; }
    //     public string? Email { get; set; }

    //     public DateTime? CreatedAt { get; set; }
    //     public DateTime? UpdatedAt { get; set; }

    //     // Partition key
    //     public string? VendorId { get; set; }
    // }
}