using System;
using User = Domain.Models.User;
using Application.Contracts;
using Application.Interfaces;

namespace Application.Services
{
    public class UserService
    {
        private readonly IUserRepo _repo;

        public UserService(IUserRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<User>> GetAll(UserQueryParams queryParams)
        {
            return await _repo.GetAll(queryParams);
        }

        public async Task<User?> GetById(string vendorId, string id)
        {
            return await _repo.GetById(vendorId, id);
        }

        public async Task<User?> Create(User user)
        {
            return await _repo.Create(user);
        }

        public async Task<User?> Patch(PatchUserDto patched, string vendorId, string id)
        {
            return await _repo.Patch(patched, vendorId, id);
        }

        public async Task<bool> Delete(string vendorId, string id)
        {
            return await _repo.Delete(vendorId, id);
        }
    }
}