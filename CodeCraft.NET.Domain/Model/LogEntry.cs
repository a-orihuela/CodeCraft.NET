using CodeCraft.NET.Cross.Domain;

namespace CodeCraft.NET.Domain.Model
{
    public class LogEntry : BaseDomainModel
    {
        /// <summary>
        /// Timestamp del log con precisión de milisegundos
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Nivel del log (Debug, Info, Warning, Error, Critical)
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// Mensaje principal del log
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Categoría o origen del log (ej: "DatabaseService", "ApiController", "AuthenticationService")
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Componente de la aplicación (Infrastructure, Application, MAUI, WebAPI, etc.)
        /// </summary>
        public string Component { get; set; } = string.Empty;

        /// <summary>
        /// Categoría del componente del sistema
        /// </summary>
        public SystemComponent ComponentCategory { get; set; } = SystemComponent.Application;

        /// <summary>
        /// Información detallada de la excepción si es aplicable
        /// </summary>
        public string? Exception { get; set; }

        /// <summary>
        /// Stack trace de la excepción
        /// </summary>
        public string? StackTrace { get; set; }

        /// <summary>
        /// Información adicional en formato JSON
        /// </summary>
        public string? AdditionalData { get; set; }

        /// <summary>
        /// ID de correlación para rastrear operaciones relacionadas
        /// </summary>
        public string? CorrelationId { get; set; }

        /// <summary>
        /// ID de la sesión del usuario
        /// </summary>
        public string? SessionId { get; set; }

        /// <summary>
        /// ID del usuario que generó el log (si está disponible)
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Nombre del método donde se originó el log
        /// </summary>
        public string? MethodName { get; set; }

        /// <summary>
        /// Nombre de la clase donde se originó el log
        /// </summary>
        public string? ClassName { get; set; }

        /// <summary>
        /// Número de línea donde se originó el log
        /// </summary>
        public int? LineNumber { get; set; }

        /// <summary>
        /// Nombre del archivo donde se originó el log
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// IP del cliente (para logs de API)
        /// </summary>
        public string? ClientIP { get; set; }

        /// <summary>
        /// User Agent del cliente (para logs de API)
        /// </summary>
        public string? UserAgent { get; set; }

        /// <summary>
        /// URL solicitada (para logs de API)
        /// </summary>
        public string? RequestUrl { get; set; }

        /// <summary>
        /// Método HTTP (para logs de API)
        /// </summary>
        public string? HttpMethod { get; set; }

        /// <summary>
        /// Tiempo de duración de la operación en milisegundos
        /// </summary>
        public long? DurationMs { get; set; }

        /// <summary>
        /// Código de estado de respuesta (para logs de API)
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// Indica si este log ha sido procesado o revisado
        /// </summary>
        public bool IsProcessed { get; set; } = false;

        /// <summary>
        /// Comentarios adicionales del administrador del sistema
        /// </summary>
        public string? AdminNotes { get; set; }

        /// <summary>
        /// Hash del mensaje para evitar duplicados
        /// </summary>
        public string? MessageHash { get; set; }

        /// <summary>
        /// Contador de ocurrencias del mismo log
        /// </summary>
        public int OccurrenceCount { get; set; } = 1;

        /// <summary>
        /// Última vez que ocurrió este log
        /// </summary>
        public DateTime? LastOccurrence { get; set; }
    }
}