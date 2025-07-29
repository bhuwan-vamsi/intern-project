using CsvHelper.Configuration.Attributes;

namespace APIPractice.Models.DTO
{
    public class ProductCsvDto
    {
        [Name("name")]
        public required string Name { get; set; }

        [Name("price")]
        public decimal Price { get; set; }

        [Name("image_url")]
        public string? ImageUrl { get; set; }

        [Name("units")]
        public required string Units { get; set; }

        [Name("category")]
        public required string Category { get; set; }
    }
}
