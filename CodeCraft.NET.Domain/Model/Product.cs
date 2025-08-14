using CodeCraft.NET.Cross.Domain;

namespace CodeCraft.NET.Domain.Model
{
    public class Product : BaseDomainModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Stock { get; set; }
        public bool IsAvailable { get; set; }
    }
}