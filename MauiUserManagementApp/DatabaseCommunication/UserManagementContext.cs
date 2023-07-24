using MauiUserManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MauiUserManagementApp.DatabaseCommunication
{
    public class UserManagementContext : DbContext
    {
        public UserManagementContext(DbContextOptions<UserManagementContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }
    }
}
