﻿using System.Runtime.InteropServices;
using datntdev.Abp.Dependency;

namespace datntdev.Abp.Runtime.System
{
    public class OSPlatformProvider : IOSPlatformProvider, ITransientDependency
    {
        public virtual OSPlatform GetCurrentOSPlatform()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return OSPlatform.OSX; //MAC
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return OSPlatform.Windows;
            }

            return OSPlatform.Linux;
        }
    }
}