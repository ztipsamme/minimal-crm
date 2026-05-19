using System;
using System.Linq.Expressions;
using api.Dto;
using api.Enums;
using Microsoft.Azure.Cosmos;

namespace api.Helpers
{
    public abstract class BaseQueryHelper<TEntity, TQueryParams>
        where TEntity : class
        where TQueryParams : BaseQueryParameters, new()
    {
        public async Task<IEnumerable<TEntity>> ExecuteQuery(
            Container container,
            TQueryParams queryParams)
        {
            var query = BuildQuery(queryParams);

            var iterator = container.GetItemQueryIterator<TEntity>(query);

            var results = new List<TEntity>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public abstract Expression<Func<TEntity, object>> ApplyOrder(TQueryParams queryParams);

        public abstract QueryDefinition BuildQuery(TQueryParams queryParams);

        public string SortBy(string sql, TQueryParams queryParams)
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

        public abstract string BuildOrderField(TQueryParams queryParams);

    }
}