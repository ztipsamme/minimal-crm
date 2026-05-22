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
}