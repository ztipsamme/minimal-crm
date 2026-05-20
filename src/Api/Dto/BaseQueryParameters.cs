using System;
using Api.Enums;

namespace Api.Dto
{
    public class BaseQueryParameters
    {
        public int? Page { get; set; }
        public int? Limit { get; set; }
        public bool? Descending { get; set; }
        public OrderBy? OrderBy { get; set; }
    }
}