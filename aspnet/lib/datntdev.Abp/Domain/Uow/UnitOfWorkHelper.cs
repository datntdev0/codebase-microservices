using System.Reflection;

namespace datntdev.Abp.Domain.Uow
{
    /// <summary>
    /// A helper class to simplify unit of work process.
    /// </summary>
    public static class UnitOfWorkHelper
    {
        /// <summary>
        /// Returns true if given method has UnitOfWorkAttribute attribute.
        /// </summary>
        /// <param name="memberInfo">Method info to check</param>
        public static bool HasUnitOfWorkAttribute(MemberInfo memberInfo)
        {
            return memberInfo.IsDefined(typeof(UnitOfWorkAttribute), true);
        }
    }
}