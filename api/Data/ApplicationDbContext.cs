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
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = Guid.Parse("b1f5a121-d4ad-48f2-8736-30bb6080b2cc"), Name = "Fruits & Vegetables" },
                new Category { Id = Guid.Parse("5394e988-0a56-41c5-bd6a-3a0b9f2f3177"), Name = "Eggs, Meat & Fish" },
                new Category { Id = Guid.Parse("5633ef72-1953-49b0-b587-95f8a2fc1684"), Name = "Snacks & Branded Foods" },
                new Category { Id = Guid.Parse("75bd72c3-9150-4ec7-b2a9-a47e38744cac"), Name = "Baby Care" },
                new Category { Id = Guid.Parse("e6340996-3826-4488-a93a-53f3ddbc4b33"), Name = "Bakery, Cakes & Diary" },
                new Category { Id = Guid.Parse("d20b902c-05df-4f4e-8647-639de3319d5b"), Name = "Beverages" }
                );

            modelBuilder.Entity<OrderStatus>().HasData(
                new OrderStatus { Id = Guid.Parse("eef01fc4-7307-4413-a72b-7541f96010d2"), Name = "Billed" },
                new OrderStatus { Id = Guid.Parse("1897c6ce-92d2-4f11-ad44-8a9db3a2bca6"), Name = "Accepted" },
                new OrderStatus { Id = Guid.Parse("ac5e5767-dd5d-4c46-bdea-947155ddeda4"), Name = "Packed" },
                new OrderStatus { Id = Guid.Parse("4ba5ec5f-543f-47a9-bfe4-c9ea5e2b7ef5"), Name = "Delivered"}
            );
        }
    }     
}
