using CodeCraft.NET.Cross.Domain;

namespace CodeCraft.NET.Domain.Model
{
    /// <summary>
    /// Modelo para estad�sticas y resumen de logs del sistema
    /// </summary>
    public class LogSummary : BaseDomainModel
    {
        /// <summary>
        /// Fecha del resumen (normalmente agrupado por d�a)
        /// </summary>
        public DateTime SummaryDate { get; set; }

        /// <summary>
        /// Componente del sistema
        /// </summary>
        public SystemComponent ComponentCategory { get; set; }

        /// <summary>
        /// Nombre espec�fico del componente
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
        /// Total de logs para este d�a/componente
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// Tama�o total en bytes de los logs
        /// </summary>
        public long TotalSizeBytes { get; set; } = 0;

        /// <summary>
        /// Tiempo promedio de respuesta en milisegundos (para operaciones web)
        /// </summary>
        public double? AverageResponseTimeMs { get; set; }

        /// <summary>
        /// N�mero de usuarios �nicos que generaron logs
        /// </summary>
        public int UniqueUsersCount { get; set; } = 0;

        /// <summary>
        /// N�mero de sesiones �nicas
        /// </summary>
        public int UniqueSessionsCount { get; set; } = 0;

        /// <summary>
        /// Primera ocurrencia del d�a
        /// </summary>
        public DateTime? FirstOccurrence { get; set; }

        /// <summary>
        /// �ltima ocurrencia del d�a
        /// </summary>
        public DateTime? LastOccurrence { get; set; }

        /// <summary>
        /// Top 5 categor�as m�s frecuentes (JSON)
        /// </summary>
        public string? TopCategories { get; set; }

        /// <summary>
        /// Top 5 errores m�s frecuentes (JSON)
        /// </summary>
        public string? TopErrors { get; set; }

        /// <summary>
        /// Nota adicional del resumen
        /// </summary>
        public string? Notes { get; set; }
    }
}