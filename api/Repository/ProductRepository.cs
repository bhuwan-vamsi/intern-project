using APIPractice.Data;
using APIPractice.Infrastructure;
using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace APIPractice.Repository
{
    public class ProductRepository : IProductRepository<Product>
    {
        private readonly ApplicationDbContext _db;
        private readonly TransactionManager transactionManager;

        public ProductRepository(ApplicationDbContext db, TransactionManager transactionManager) 
        {
            _db = db;
            this.transactionManager = transactionManager;
        }
        public async Task<List<Product>> GetAllAsync(string? category, string? filterQuery = null,string? sortBy=null, bool isAscending= true, int pageNumber=1, int pageSize= int.MaxValue)
        {
            var products = _db.Products.Include("Category").AsQueryable();

            // filtering
            if (!string.IsNullOrWhiteSpace(category))
            {
                products = products.Where(x => x.Category.Name.Equals(category));
            }
            if(!string.IsNullOrWhiteSpace(filterQuery))
            {
                products = products.Where(x => x.Name.ToLower().Contains(filterQuery.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("UnitPrice", StringComparison.OrdinalIgnoreCase))
                {
                    products = isAscending ? products.OrderBy(x => x.Price) : products.OrderByDescending(x => x.Price);
                }
                else if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    products = isAscending ? products.OrderBy(x => x.Name) : products.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Quantity", StringComparison.OrdinalIgnoreCase))
                {
                    products = isAscending ? products.OrderBy(x => x.Quantity) : products.OrderByDescending(x => x.Quantity);
                }
                else if(sortBy.Equals("Threshold", StringComparison.OrdinalIgnoreCase))
                {
                    products = isAscending ? products.OrderBy(x => x.Threshold) : products.OrderByDescending(x => x.Units);
                }
            }
            var skip = (pageNumber - 1) * pageSize;
            return await products.Skip(skip).Take(pageSize).ToListAsync();
        }

        public async Task<Product> GetAsync(Guid id)
        {
            var product= await _db.Products.Include("Category").FirstOrDefaultAsync(u=> u.Id == id);
            if (product == null)
            {
                throw new KeyNotFoundException("Product Not Found");
            }
            return product;
        }

        public async Task<Product> CreateAsync(Product entity, Guid managerId)
        {
            entity.IsActive = true;
            var stockUpdate = new StockUpdateHistory
            {
                Id = Guid.NewGuid(),
                ProductId = entity.Id,
                ManagerId = managerId,
                QuantityIn = entity.Quantity,
                UpdatedAt = DateTime.Now
            };
            await _db.Products.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(Product existingProduct, UpdateProductDto updatedProduct, Guid managerId)
        {
            await transactionManager.ExecuteInTransactionAsync(async () =>
            {
                if (existingProduct.Quantity < updatedProduct.Quantity)
                {
                    var stockUpdate = new StockUpdateHistory
                    {
                        Id = Guid.NewGuid(),
                        ProductId = existingProduct.Id,
                        ManagerId = managerId,
                        QuantityIn = updatedProduct.Quantity,
                        UpdatedAt = DateTime.Now
                    };
                    await _db.StockUpdateHistories.AddAsync(stockUpdate);
                    await _db.SaveChangesAsync();
                }
                else if (existingProduct.Quantity >= updatedProduct.Quantity)
                {
                    throw new InvalidOperationException("Cannot reduce quantity below current stock.");
                }
                existingProduct.Name = updatedProduct.Name;
                existingProduct.Price = updatedProduct.Price;
                existingProduct.Units = updatedProduct.Units;
                existingProduct.Quantity = updatedProduct.Quantity;
                existingProduct.Threshold = updatedProduct.Threshold;
                existingProduct.ImageUrl = updatedProduct.ImageUrl;
                existingProduct.CategoryId = updatedProduct.CategoryId;
                _db.Products.Update(existingProduct);
                await _db.SaveChangesAsync();
            });
        }
        public async Task UpdateQuantityAsync(Guid id, Product product)
        {
            await transactionManager.ExecuteInTransactionAsync(async () =>
            {
                if (product.Quantity < 0)
                {
                    throw new InvalidOperationException("Quantity cannot be negative.");
                }

                var existingProduct = _db.Products.Include("Category").FirstOrDefault(p => p.Id == id);
                if (existingProduct == null)
                {
                    throw new KeyNotFoundException("Product Not Found");
                }
                existingProduct.Quantity = product.Quantity;
                _db.Products.Update(existingProduct);
                await _db.SaveChangesAsync();
            });
        }

        public async Task DeleteAsync(Product product)
        {
            await transactionManager.ExecuteInTransactionAsync(async () =>
            {
                if (product == null)
                {
                    throw new KeyNotFoundException("Product Not Found");
                }
                product.IsActive = !product.IsActive;
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
            });
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _db.Categories.ToListAsync();
        }

        public async Task<List<Product>> GetAllByIdsAsync(List<Guid> productIds)
        {
            return await _db.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
        }

        public async Task UpdateAllQuantityAsync(Dictionary<Guid, Product> productDict)
        {
            _db.Products.UpdateRange(productDict.Values);
            await _db.SaveChangesAsync();
        }
    }
}
