using Divisima.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divisima.DAL.Context
{
    public class SQLContext:DbContext
    {
        public SQLContext(DbContextOptions<SQLContext> opt):base(opt)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasData(new Admin
            {
                ID = 1,
                NameSurname = "Emir Albayrak",
                LastLoginDate = DateTime.Now,
                LastLoginIP = "",
                UserName = "emir",
                Password = "e54e06aa388a4bcd4d6ad68692154bd5"
            });

            modelBuilder.Entity<Product>().HasOne(x => x.Category).WithMany(x => x.Products).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Product>().HasOne(x => x.Brand).WithMany(x => x.Products).OnDelete(DeleteBehavior.SetNull);
        }

        public DbSet<Admin> Admin { get; set; }
        public DbSet<Slide> Slide { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductPicture> ProductPicture { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }

        
    }
}
