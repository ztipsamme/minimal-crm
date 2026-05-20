using System;
using System.Reflection;
using Microsoft.Azure.Cosmos;

namespace api.Helpers
{
    public static class PatchBuilder
    {
        public static List<PatchOperation> From<T>(T dto)
        {
            var operations = new List<PatchOperation>();

            if (dto is null) return operations;

            operations = PatchProps(operations, dto);

            operations.Add(PatchOperation.Set("/updatedAt", DateTime.UtcNow));

            return operations;
        }

        private static List<PatchOperation> PatchProps<T>(List<PatchOperation> operations, T dto)
        {
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                var value = prop.GetValue(dto);
                if (value is null) continue;

                if (IsSimple(prop.PropertyType))
                {
                    var path = $"/{ToCamelCase(prop.Name)}";
                    operations.Add(PatchOperation.Replace(path, value));
                }
                else
                {
                    operations = PatchNestedProps(operations, prop, dto);
                }
            }

            return operations;
        }

        private static List<PatchOperation> PatchNestedProps<T>(List<PatchOperation> operations, PropertyInfo prop, T dto)
        {
            var nestedProps = prop.PropertyType
                      .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var nestedProp in nestedProps)
            {
                var nestedValue = nestedProp.GetValue(dto);
                if (nestedValue is null) continue;

                var path = $"/{ToCamelCase(prop.Name)}/{ToCamelCase(nestedProp.Name)}";

                operations.Add(PatchOperation.Replace(path, nestedValue));
            }

            return operations;
        }

        private static bool IsSimple(Type type)
        {
            return type.IsPrimitive
                || type.IsEnum
                || type == typeof(string)
                || type == typeof(DateTime)
                || type == typeof(decimal)
                || type == typeof(Guid)
                || Nullable.GetUnderlyingType(type)?.IsEnum == true
                || Nullable.GetUnderlyingType(type)?.IsPrimitive == true;
        }

        private static string ToCamelCase(string name) => string.IsNullOrEmpty(name)
            ? name
            : char.ToLowerInvariant(name[0]) + name.Substring(1);
    }
}