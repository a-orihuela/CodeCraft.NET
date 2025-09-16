namespace CodeCraft.NET.Cross.Domain
{
    /// <summary>
    /// System component categories for organizing application layers and responsibilities
    /// Used for logging, metrics, and architectural organization
    /// </summary>
    public enum SystemComponent
    {
        /// <summary>
        /// Infrastructure layer - Entity Framework, repositories, external services, file I/O
        /// </summary>
        Infrastructure = 0,

        /// <summary>
        /// Application layer - Use cases, DTOs, CQRS handlers, business orchestration
        /// </summary>
        Application = 1,

        /// <summary>
        /// Domain layer - Domain entities, value objects, domain services, business rules
        /// </summary>
        Domain = 2,

        /// <summary>
        /// WebAPI layer - REST API controllers, HTTP endpoints
        /// </summary>
        WebAPI = 3,

        /// <summary>
        /// DesktopAPI layer - Business logic services, orchestration services
        /// </summary>
        DesktopAPI = 4,

        /// <summary>
        /// MAUI layer - Presentation layer, UI, ViewModels, Pages
        /// </summary>
        MAUI = 5,

        /// <summary>
        /// Cross-cutting concerns - Logging, configuration, shared utilities, extensions
        /// </summary>
        Cross = 6,

        /// <summary>
        /// Code generation tools and utilities
        /// </summary>
        Generator = 7,

        /// <summary>
        /// Authentication and authorization services
        /// </summary>
        Authentication = 8,

        /// <summary>
        /// Database layer and data access
        /// </summary>
        Database = 9,

        /// <summary>
        /// External services and third-party integrations
        /// </summary>
        External = 10
    }
}