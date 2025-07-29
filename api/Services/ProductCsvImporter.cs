using APIPractice.Data;
using APIPractice.Models.Domain;
using APIPractice.Models.DTO;
using CsvHelper;
using System.Globalization;

public class ProductCsvImporter
{
    private readonly ApplicationDbContext _dbContext;

    public ProductCsvImporter(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ImportProductsFromCsvAsync(string csvPath)
    {
        using var reader = new StreamReader(csvPath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<ProductCsvDto>().ToList();

        foreach (var record in records)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Name == record.Category);

            if (category == null)
            {
                category = new Category
                {
                    Id = Guid.NewGuid(),
                    Name = record.Category
                };

                _dbContext.Categories.Add(category);
                await _dbContext.SaveChangesAsync();
            }

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = record.Name,
                Units = record.Units,
                Quantity = 100,
                Price = record.Price,
                Threshold = 20,
                ImageUrl = record.ImageUrl,
                CategoryId = category.Id,
                Category = category,
                IsActive = true
            };

            _dbContext.Products.Add(product);
        }

        await _dbContext.SaveChangesAsync();
    }
}
