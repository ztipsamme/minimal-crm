using System;
using Api.Dto;

namespace Api.Dto
{
    public class CustomerQueryParams : BaseQueryParameters
    {
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? VendorId { get; set; }
    }
}