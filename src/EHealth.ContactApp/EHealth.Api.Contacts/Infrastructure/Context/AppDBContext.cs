using EHealth.Api.Contacts.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EHealth.Api.Contacts.Infrastructure.Context
{
    public class AppDBContext : DbContext
    {

        public DbSet<ContactEntity> Contacts { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ContactEntity>().ToTable("Contacts");
            builder.Entity<ContactEntity>().HasKey(c => c.Id);
            builder.Entity<ContactEntity>().Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<ContactEntity>().Property(c => c.FirstName).IsRequired().HasMaxLength(50);
            builder.Entity<ContactEntity>().Property(c => c.LastName).IsRequired().HasMaxLength(50);
            builder.Entity<ContactEntity>().Property(c => c.Email).HasMaxLength(50);
            builder.Entity<ContactEntity>().Property(c => c.PhoneNumber).HasMaxLength(15);
            builder.Entity<ContactEntity>().Property(c => c.Status).HasColumnType("smallint");

        }

    }
}
