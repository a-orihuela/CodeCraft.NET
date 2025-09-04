using CodeCraft.NET.Cross.Domain;

namespace CodeCraft.NET.Domain.Model
{
    /// <summary>
    /// Modelo para estadísticas y resumen de logs del sistema
    /// </summary>
    public class LogSummary : BaseDomainModel
    {
        /// <summary>
        /// Fecha del resumen (normalmente agrupado por día)
        /// </summary>
        public DateTime SummaryDate { get; set; }

        /// <summary>
        /// Componente del sistema
        /// </summary>
        public SystemComponent ComponentCategory { get; set; }

        /// <summary>
        /// Nombre específico del componente
        /// </summary>
        public string Component { get; set; } = string.Empty;

        /// <summary>
        /// Total de logs de nivel Debug
        /// </summary>
        public int DebugCount { get; set; } = 0;

        /// <summary>
        /// Total de logs de nivel Info
        /// </summary>
        public int InfoCount { get; set; } = 0;

        /// <summary>
        /// Total de logs de nivel Warning
        /// </summary>
        public int WarningCount { get; set; } = 0;

        /// <summary>
        /// Total de logs de nivel Error
        /// </summary>
        public int ErrorCount { get; set; } = 0;

        /// <summary>
        /// Total de logs de nivel Critical
        /// </summary>
        public int CriticalCount { get; set; } = 0;

        /// <summary>
        /// Total de logs para este día/componente
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// Tamaño total en bytes de los logs
        /// </summary>
        public long TotalSizeBytes { get; set; } = 0;

        /// <summary>
        /// Tiempo promedio de respuesta en milisegundos (para operaciones web)
        /// </summary>
        public double? AverageResponseTimeMs { get; set; }

        /// <summary>
        /// Número de usuarios únicos que generaron logs
        /// </summary>
        public int UniqueUsersCount { get; set; } = 0;

        /// <summary>
        /// Número de sesiones únicas
        /// </summary>
        public int UniqueSessionsCount { get; set; } = 0;

        /// <summary>
        /// Primera ocurrencia del día
        /// </summary>
        public DateTime? FirstOccurrence { get; set; }

        /// <summary>
        /// Última ocurrencia del día
        /// </summary>
        public DateTime? LastOccurrence { get; set; }

        /// <summary>
        /// Top 5 categorías más frecuentes (JSON)
        /// </summary>
        public string? TopCategories { get; set; }

        /// <summary>
        /// Top 5 errores más frecuentes (JSON)
        /// </summary>
        public string? TopErrors { get; set; }

        /// <summary>
        /// Nota adicional del resumen
        /// </summary>
        public string? Notes { get; set; }
    }
}