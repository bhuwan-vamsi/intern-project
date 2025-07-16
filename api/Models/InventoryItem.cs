using System.ComponentModel.DataAnnotations;

namespace APIPractice.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        public string Itemname { get; set; } = string.Empty;

        public int Price { get; set; }

        public int Quantity { get; set; }
    }
}
