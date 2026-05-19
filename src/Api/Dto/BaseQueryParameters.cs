using System;
using api.Enums;

namespace api.Dto
{
    public class BaseQueryParameters
    {
        public int? Page { get; set; }
        public int? Limit { get; set; }
        public bool? Descending { get; set; }
        public OrderBy? OrderBy { get; set; }
    }
}