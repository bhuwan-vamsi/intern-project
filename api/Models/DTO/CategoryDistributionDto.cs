namespace APIPractice.Models.DTO
{
    public class CategoryDistributionDto
    {
        public int TotalCategories { get; set; }
        public int TotalItemsInCategories { get; set; }
        public int TotalQuantityInCategories { get; set; }
        public DateTime LastUpdated { get; set; }
        public int RecentlyAddedCategories { get; set; }
        public List<CategoriesList> Categories { get; set; } = new List<CategoriesList>();
        public class CategoriesList()
        {
            public required string Name { get; set; }
            public int Items { get; set; }
            public bool IsNew { get; set; }
        }
    }
}
