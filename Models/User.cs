using System;
using System.Collections.Generic;

namespace Scash.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PIN { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LastFailedLogin { get; set; }
        public bool IsLocked { get; set; }
        public string? QRCode { get; set; }
        public decimal DailyTransactionLimit { get; set; }
        public decimal MonthlyTransactionLimit { get; set; }
    }

    public enum UserRole
    {
        Personal = 0,
        Admin = 1,
        Agent = 2,
        Merchant = 3
    }
} 