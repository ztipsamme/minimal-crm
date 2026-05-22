using System;
using User = Domain.Models.User;
using Application.Contracts;

namespace Application.Interfaces
{
    public interface IUserRepo
    {
        Task<IEnumerable<User>> GetAll(UserQueryParams queryParams);

        Task<User?> GetById(string vendorId, string id);

        Task<User?> Create(User user);


        Task<User?> Patch(PatchUserDto patched, string vendorId, string id);


        Task<bool> Delete(string vendorId, string id);
    }
}