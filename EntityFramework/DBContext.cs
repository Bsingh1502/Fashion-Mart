using FashionMart.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FashionMart.EntityFramework
{
    public class Context: IdentityDbContext<User>
    {
        public Context(DbContextOptions options) :base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.ApplyConfiguration(new UserRoles());
        }

    }
}
