using APIPractice.Models.Domain;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;

namespace APIPractice.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<TaskHistory> TasksHistory { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }  
        public DbSet<StockUpdateHistory> StockUpdateHistories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderStatus>().HasData(
                new OrderStatus { Id = Guid.Parse("eef01fc4-7307-4413-a72b-7541f96010d2"), Name = "Billed" },
                new OrderStatus { Id = Guid.Parse("1897c6ce-92d2-4f11-ad44-8a9db3a2bca6"), Name = "Accepted" },
                new OrderStatus { Id = Guid.Parse("ac5e5767-dd5d-4c46-bdea-947155ddeda4"), Name = "Packed" },
                new OrderStatus { Id = Guid.Parse("4ba5ec5f-543f-47a9-bfe4-c9ea5e2b7ef5"), Name = "Delivered"}
            );
        }
    }     
}
