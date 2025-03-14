﻿using System.Threading.Tasks;

namespace datntdev.Abp.Web.Timing
{
    /// <summary>
    /// Define interface to get timing scripts
    /// </summary>
    public interface ITimingScriptManager
    {
        /// <summary>
        /// Gets JavaScript that contains all feature information.
        /// </summary>
        Task<string> GetScriptAsync();
    }
}
