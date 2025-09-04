using CodeCraft.NET.Cross.Domain;

namespace CodeCraft.NET.Domain.Model
{
    public class LogEntry : BaseDomainModel
    {
        /// <summary>
        /// Timestamp del log con precisi�n de milisegundos
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
        /// Categor�a o origen del log (ej: "DatabaseService", "ApiController", "AuthenticationService")
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Componente de la aplicaci�n (Infrastructure, Application, MAUI, WebAPI, etc.)
        /// </summary>
        public string Component { get; set; } = string.Empty;

        /// <summary>
        /// Categor�a del componente del sistema
        /// </summary>
        public SystemComponent ComponentCategory { get; set; } = SystemComponent.Application;

        /// <summary>
        /// Informaci�n detallada de la excepci�n si es aplicable
        /// </summary>
        public string? Exception { get; set; }

        /// <summary>
        /// Stack trace de la excepci�n
        /// </summary>
        public string? StackTrace { get; set; }

        /// <summary>
        /// Informaci�n adicional en formato JSON
        /// </summary>
        public string? AdditionalData { get; set; }

        /// <summary>
        /// ID de correlaci�n para rastrear operaciones relacionadas
        /// </summary>
        public string? CorrelationId { get; set; }

        /// <summary>
        /// ID de la sesi�n del usuario
        /// </summary>
        public string? SessionId { get; set; }

        /// <summary>
        /// ID del usuario que gener� el log (si est� disponible)
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Nombre del m�todo donde se origin� el log
        /// </summary>
        public string? MethodName { get; set; }

        /// <summary>
        /// Nombre de la clase donde se origin� el log
        /// </summary>
        public string? ClassName { get; set; }

        /// <summary>
        /// N�mero de l�nea donde se origin� el log
        /// </summary>
        public int? LineNumber { get; set; }

        /// <summary>
        /// Nombre del archivo donde se origin� el log
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
        /// M�todo HTTP (para logs de API)
        /// </summary>
        public string? HttpMethod { get; set; }

        /// <summary>
        /// Tiempo de duraci�n de la operaci�n en milisegundos
        /// </summary>
        public long? DurationMs { get; set; }

        /// <summary>
        /// C�digo de estado de respuesta (para logs de API)
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
        /// �ltima vez que ocurri� este log
        /// </summary>
        public DateTime? LastOccurrence { get; set; }
    }
}