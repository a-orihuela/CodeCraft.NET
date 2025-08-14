using CodeCraft.NET.Cross.Domain;

namespace CodeCraft.NET.Domain.Model
{
    public class Customer : BaseDomainModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
    }
}