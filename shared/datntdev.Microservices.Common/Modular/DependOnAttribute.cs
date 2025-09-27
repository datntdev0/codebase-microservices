namespace datntdev.Microservices.Common.Modular
{
    /// <summary>
    /// Used to define dependencies of an ABP module to other modules.
    /// It should be used for a class derived from <see cref="BaseModule"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependOnAttribute(params Type[] dependedModuleTypes) : Attribute
    {
        /// <summary>
        /// Types of depended modules.
        /// </summary>
        public Type[] DependedModuleTypes { get; } = dependedModuleTypes;
    }
}
