using System;
using System.Linq.Expressions;
using api.Dto;
using api.Enums;
using api.Models;
using Microsoft.Azure.Cosmos;

namespace api.Helpers
{
    public class CustomerQueryHelper
        : BaseQueryHelper<Customer, CustomerQueryParams>
    {
        public override Expression<Func<Customer, object>> ApplyOrder(CustomerQueryParams queryParams) => queryParams?.OrderBy switch
        {
            OrderBy.Name => c => c.Name,
            OrderBy.Title => c => c.Title,
            _ => a => a.CreatedAt
        };

        public override QueryDefinition BuildQuery(CustomerQueryParams queryParams)
        {
            var filters = new List<string>();
            var sql = "SELECT * FROM c";

            if (!string.IsNullOrEmpty(queryParams.Name))
                filters.Add("CONTAINS(c.name, @name)");

            if (!string.IsNullOrEmpty(queryParams.Title))
                filters.Add("CONTAINS(c.title, @title)");

            if (!string.IsNullOrEmpty(queryParams.VendorId))
                filters.Add("CONTAINS(c.vendorId, @vendorId)");

            if (filters.Any())
                sql += " WHERE " + string.Join(" AND ", filters);

            sql = SortBy(sql, queryParams);

            var query = new QueryDefinition(sql);

            if (!string.IsNullOrEmpty(queryParams.Name))
                query = query.WithParameter("@name", queryParams.Name);

            if (!string.IsNullOrEmpty(queryParams.Title))
                query = query.WithParameter("@title", queryParams.Title);

            if (!string.IsNullOrEmpty(queryParams.VendorId))
                query = query.WithParameter("@vendorId", queryParams.VendorId);

            return query;
        }

        public override string BuildOrderField(CustomerQueryParams queryParams)
        {
            return queryParams.OrderBy switch
            {
                OrderBy.Name => "c.name",
                OrderBy.Title => "c.title",
                _ => "c.createdAt"
            };
        }
    }
}