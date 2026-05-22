using System;
using Application.Enums;

namespace Application.Contracts
{
    public class UserQueryParams
    {
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Role { get; set; }
        public string? VendorId { get; set; }

        public int? Page { get; set; }
        public int? Limit { get; set; }
        public bool? Descending { get; set; }
        public OrderBy? OrderBy { get; set; }
    }
}