using CodeCraft.NET.Cross.Domain;

namespace CodeCraft.NET.Domain.Model
{
    public class LogConfiguration : BaseDomainModel
    {
        /// <summary>
        /// Nombre del componente, m�dulo o capa (ej: "Infrastructure", "Application", "MAUI", "WebAPI")
        /// </summary>
        public string Component { get; set; } = string.Empty;

        /// <summary>
        /// Categor�a del componente del sistema
        /// </summary>
        public SystemComponent ComponentCategory { get; set; } = SystemComponent.Application;

        /// <summary>
        /// Namespace o clase espec�fica (opcional para configuraciones m�s granulares)
        /// </summary>
        public string? NamespacePath { get; set; }

        /// <summary>
        /// Nivel m�nimo de logging para este componente (Info, Warning, Error, Critical, Debug)
        /// </summary>
        public LogLevel MinimumLevel { get; set; } = LogLevel.Info;

        /// <summary>
        /// Indica si el logging est� habilitado para este componente
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Destinos de logging habilitados (puede ser una combinaci�n usando flags)
        /// </summary>
        public LogDestination EnabledDestinations { get; set; } = LogDestination.Database | LogDestination.Console;

        /// <summary>
        /// Indica si se deben registrar logs en base de datos
        /// </summary>
        public bool LogToDatabase { get; set; } = true;

        /// <summary>
        /// Indica si se deben registrar logs en archivo
        /// </summary>
        public bool LogToFile { get; set; } = false;

        /// <summary>
        /// Indica si se deben registrar logs en consola
        /// </summary>
        public bool LogToConsole { get; set; } = true;

        /// <summary>
        /// Ruta del archivo de log (si LogToFile est� habilitado)
        /// </summary>
        public string? LogFilePath { get; set; }

        /// <summary>
        /// Formato del log (ej: "{Timestamp} [{Level}] {Category}: {Message}")
        /// </summary>
        public string? LogFormat { get; set; }

        /// <summary>
        /// D�as de retenci�n de logs (0 = permanente)
        /// </summary>
        public int RetentionDays { get; set; } = 30;

        /// <summary>
        /// Tama�o m�ximo del archivo de log en MB (0 = sin l�mite)
        /// </summary>
        public int MaxFileSizeMB { get; set; } = 100;

        /// <summary>
        /// Descripci�n de la configuraci�n
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Prioridad de la configuraci�n (para casos donde m�ltiples configuraciones aplican)
        /// </summary>
        public int Priority { get; set; } = 0;
    }
}