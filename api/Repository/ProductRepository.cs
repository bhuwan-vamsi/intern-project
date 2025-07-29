using APIPractice.Data;
using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using APIPractice.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIPractice.Repository
{
    public class ProductRepository : IProductRepository<Product>
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) 
        {
            _db = db;
        }
        public async Task<List<Product>> GetAllAsync(string? categoryName = null, string? filterQuery = null)
        {
            var products = _db.Products.Include("Category").Where(x => x.IsActive == true).AsQueryable();

            // filtering
            if(!string.IsNullOrWhiteSpace(categoryName))
            {
                products = products.Where(x => x.Category.Equals(categoryName));
            }
            if (!string.IsNullOrWhiteSpace(filterQuery))
            {
                products = products.Where(x => x.Name.Contains(filterQuery));
            }
            return await products.ToListAsync();
        }

        public async Task<Product> GetAsync(Guid id)
        {
            var product= await _db.Products.Include("Category").FirstOrDefaultAsync(u=> u.Id == id && u.IsActive==true);
            if (product == null)
            {
                throw new KeyNotFoundException("Product Not Found");
            }
            return product;
        }

        public async Task<Product> CreateAsync(Product entity)
        {
            entity.IsActive = true;
            await _db.Products.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task UpdateAsync(Product existingProduct, UpdateProductDto updatedProduct, Guid managerId)
        {
            if (existingProduct.Quantity != updatedProduct.Quantity && existingProduct.Quantity < updatedProduct.Quantity)
            {
                var stockUpdate = new StockUpdateHistory
                {
                    Id = Guid.NewGuid(),
                    ProductId = existingProduct.Id,
                    ManagerId = managerId,
                    Quantity = updatedProduct.Quantity,
                    TimeStamp = DateTime.UtcNow
                };
                await _db.StockUpdateHistories.AddAsync(stockUpdate);
                await _db.SaveChangesAsync();
            }
            else if (existingProduct.Quantity > updatedProduct.Quantity)
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
        }
        public async Task UpdateQuantityAsync(Guid id, Product product)
        {
            Product existingProduct = await _db.Products.Include("Category").FirstOrDefaultAsync(p => p.Id == id);
            existingProduct.Quantity = product.Quantity;
            _db.Products.Update(existingProduct);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            product.IsActive = false;
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
        }
    }
}
