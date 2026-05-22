using System;

namespace Api.Dto
{
    public class PatchUserDto
    {
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public string? Title { get; set; }
        public PatchAddressDto? Address { get; set; }
    }

    public class PatchAddressDto
    {
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }
}
