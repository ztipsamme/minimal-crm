using System;
using api.Dto;

namespace api.Dto
{
    public class CustomerQueryParams : BaseQueryParameters
    {
        public string? Name { get; set; }
        public string? Title { get; set; }
    }
}