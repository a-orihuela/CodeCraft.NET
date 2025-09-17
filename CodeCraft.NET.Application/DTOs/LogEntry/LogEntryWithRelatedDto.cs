using CodeCraft.NET.Application.DTOs.LogEntries;

namespace CodeCraft.NET.Application.DTOs.Custom.LogEntries
{
    /// <summary>
    /// Extended DTO for LogEntry with related entities
    /// This file is never regenerated, so your customizations are safe
    /// </summary>
    public partial class LogEntryWithRelatedDto : LogEntryDto
    {
        // ============================================
        // NAVIGATION PROPERTIES
        // ============================================
        // Add related entities here as needed
        
        // Example:
        // public List<RelatedEntityDto> RelatedItems { get; set; } = new();
        // public ParentEntityDto? Parent { get; set; }
        
        // ============================================
        // COMPUTED PROPERTIES
        // ============================================
        // Add computed or derived properties here
        
        // Example:
        // public string DisplayName => $"{FirstName} {LastName}";
        // public int RelatedItemsCount => RelatedItems?.Count ?? 0;
        
        // ============================================
        // CUSTOM METHODS
        // ============================================
        // Add custom methods if needed
        
        // Example:
        // public bool HasActiveRelatedItems() => RelatedItems?.Any(x => x.IsActive) ?? false;
    }
}