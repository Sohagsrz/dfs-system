using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Scash.Data;
using Scash.Models;
using System.Collections.Generic;
using System.Linq;

namespace Scash.Services
{
    public class UserService
    {
        private readonly ScashDbContext _context;

        public UserService(ScashDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateUser(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

            if (user == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;

            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> CreateUser(User user, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                throw new Exception("Username already exists");

            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                throw new Exception("Email already exists");

            if (await _context.Users.AnyAsync(u => u.PhoneNumber == user.PhoneNumber))
                throw new Exception("Phone number already exists");

            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task UpdateUserBalance(int userId, decimal amount)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            user.Balance += amount;
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.OrderBy(u => u.CreatedAt).ToListAsync();
        }

        public async Task SetUserActive(int userId, bool isActive)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");
            user.IsActive = isActive;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("User not found");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public void RegisterUser(User user)
        {
            // Check for duplicate username or email
            if (_context.Users.Any(u => u.Username == user.Username))
            {
                throw new Exception("Username already exists.");
            }
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                throw new Exception("Email already exists.");
            }
            if (_context.Users.Any(u => u.PhoneNumber == user.PhoneNumber))
            {
                throw new Exception("Phone number already exists.");
            }

            // Add user to database
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
} 