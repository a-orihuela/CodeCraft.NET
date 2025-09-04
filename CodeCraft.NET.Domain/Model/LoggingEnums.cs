namespace CodeCraft.NET.Domain.Model
{
    /// <summary>
    /// Enumeración de niveles de logging
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Información de debugging para desarrollo
        /// </summary>
        Debug = 0,

        /// <summary>
        /// Información general del funcionamiento de la aplicación
        /// </summary>
        Info = 1,

        /// <summary>
        /// Advertencias que no interrumpen el flujo normal
        /// </summary>
        Warning = 2,

        /// <summary>
        /// Errores que afectan la funcionalidad pero no son críticos
        /// </summary>
        Error = 3,

        /// <summary>
        /// Errores críticos que pueden causar fallo total del sistema
        /// </summary>
        Critical = 4
    }

    /// <summary>
    /// Tipos de destino para el logging
    /// </summary>
    public enum LogDestination
    {
        Database = 1,
        File = 2,
        Console = 4,
        EventLog = 8,
        Network = 16
    }

    /// <summary>
    /// Categorías de componentes del sistema
    /// </summary>
    public enum SystemComponent
    {
        Infrastructure,
        Application,
        Domain,
        WebAPI,
        DesktopAPI,
        MAUI,
        Cross,
        Generator,
        Authentication,
        Database,
        External
    }
}