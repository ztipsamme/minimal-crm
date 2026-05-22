using System;
using Application.Contracts;
using Application.Interfaces;
using Infrastructure.Helpers;
using Infrastructure.Services;
using Microsoft.Azure.Cosmos;
using User = Domain.Models.User;

namespace Infrastructure.Repo
{
    public class UserRepo : IUserRepo
    {
        private readonly Container _container;
        private readonly UserQueryHelper _queryHelper;

        public UserRepo(CosmosService cosmos, UserQueryHelper queryHelper)
        {
            _container = cosmos.GetContainer("users");
            _queryHelper = queryHelper;
        }

        public async Task<IEnumerable<User>> GetAll(UserQueryParams queryParams)
        {
            return await _queryHelper.ExecuteQuery(_container, queryParams);
        }

        public async Task<User?> GetById(string vendorId, string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<User>(
                         id,
                         new PartitionKey(vendorId)
                     );

                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<User?> Create(User user)
        {
            try
            {
                var response = await _container.CreateItemAsync(user, new PartitionKey(user.VendorId));

                return response.Resource;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<User?> Patch(PatchUserDto patched, string vendorId, string id)
        {
            try
            {
                var user = await GetById(vendorId, id);
                if (user is null) return null;

                if (user.Role == "vendor")
                {
                    if (patched.Title != null || patched.Address != null)
                    {
                        throw new Exception("Vendors cannot have Title or Address");
                    }
                }

                var patchOperations = PatchBuilder.From(patched);

                var response = await _container.PatchItemAsync<User>(
                    id,
                    new PartitionKey(vendorId),
                    patchOperations
                );

                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<bool> Delete(string vendorId, string id)
        {
            try
            {
                await _container.DeleteItemAsync<User>(
                      id,
                      new PartitionKey(vendorId)
                  );

                return true;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
        }
    }
}