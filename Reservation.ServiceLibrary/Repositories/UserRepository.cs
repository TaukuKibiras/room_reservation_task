using Microsoft.EntityFrameworkCore;
using Reservation.ServiceLibrary.Entities;

namespace Reservation.ServiceLibrary.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserEntity>> GetAll()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<UserEntity?> GetById(int id)
        {
            return await _context.User.FindAsync(id);
        }

        public async Task<UserEntity> Add(UserEntity user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task Update()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Delete(UserEntity user)
        {
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
