﻿using System.Threading.Tasks;

namespace datntdev.Abp.Web.Features
{
    /// <summary>
    /// This class is used to build feature system script.
    /// </summary>
    public interface IFeaturesScriptManager
    {
        /// <summary>
        /// Gets JavaScript that contains all feature information.
        /// </summary>
        Task<string> GetScriptAsync();
    }
}