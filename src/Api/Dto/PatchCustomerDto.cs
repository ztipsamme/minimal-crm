using System;

namespace api.Dto
{
    public class PatchCustomerDto : PatchUserDto
    {
        public string? Title { get; set; }
        public PatchAddressDto? Address { get; set; }
    }
}