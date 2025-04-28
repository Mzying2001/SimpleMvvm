namespace SimpleMvvm.Ioc
{
    /// <summary>
    /// Lifetime of the instance.
    /// </summary>
    public enum Lifetime
    {
        /// <summary>
        /// The instance is created once and shared.
        /// </summary>
        Singleton,

        /// <summary>
        /// The instance is created each time it is requested.
        /// </summary>
        Transient,
    }
}
