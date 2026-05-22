using System;
using Domain.Models;

namespace Api.Dto
{
    public class CustomerDto : UserDto
    {
        public string Title { get; set; } = null!;

        public Address Address { get; set; } = null!;
    }
}