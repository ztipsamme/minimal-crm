using System;
using System.Linq.Expressions;
using Application.Contracts;
using Application.Enums;
using Microsoft.Azure.Cosmos;
using User = Domain.Models.User;

namespace Infrastructure.Helpers
{
    public class UserQueryHelper
    {
        public async Task<IEnumerable<User>> ExecuteQuery(
            Container container,
            UserQueryParams queryParams)
        {
            var query = BuildQuery(queryParams);

            var iterator = container.GetItemQueryIterator<User>(query);

            var results = new List<User>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public Expression<Func<User, object>> ApplyOrder(UserQueryParams queryParams) => queryParams?.OrderBy switch
        {
            OrderBy.Name => c => c.Name,
            OrderBy.Title => c => c.Title,
            _ => a => a.CreatedAt
        };

        public QueryDefinition BuildQuery(UserQueryParams queryParams)
        {
            var filters = new List<string>();
            var sql = "SELECT * FROM c";

            if (!string.IsNullOrEmpty(queryParams.Name))
                filters.Add("CONTAINS(c.name, @name)");

            if (!string.IsNullOrEmpty(queryParams.Title))
                filters.Add("CONTAINS(c.title, @title)");

            if (!string.IsNullOrEmpty(queryParams.Role))
                filters.Add("c.role = @role");

            if (!string.IsNullOrEmpty(queryParams.VendorId))
                filters.Add("c.vendorId = @vendorId");

            if (filters.Any())
                sql += " WHERE " + string.Join(" AND ", filters);

            sql = SortBy(sql, queryParams);

            var query = new QueryDefinition(sql);


            if (!string.IsNullOrEmpty(queryParams.Name))
                query = query.WithParameter("@name", queryParams.Name);

            if (!string.IsNullOrEmpty(queryParams.Title))
                query = query.WithParameter("@title", queryParams.Title);

            if (!string.IsNullOrEmpty(queryParams.Role))
                query = query.WithParameter("@role", queryParams.Role);

            if (!string.IsNullOrEmpty(queryParams.VendorId))
                query = query.WithParameter("@vendorId", queryParams.VendorId);

            return query;
        }

        public string BuildOrderField(UserQueryParams queryParams)
        {
            return queryParams.OrderBy switch
            {
                OrderBy.Name => "c.name",
                OrderBy.Title => "c.title",
                _ => "c.createdAt"
            };
        }

        public string SortBy(string sql, UserQueryParams queryParams)
        {
            var orderField = BuildOrderField(queryParams);
            var direction = queryParams.Descending ?? false ? "DESC" : "ASC";


            var page = queryParams.Page ?? 1;
            var limit = queryParams.Limit ?? 10;

            page = Math.Max(1, page);
            limit = Math.Clamp(limit, 1, 100);

            sql += $" ORDER BY {orderField} {direction}";
            sql += $" OFFSET {(page - 1) * limit} LIMIT {limit}";

            return sql;
        }
    }
}