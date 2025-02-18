using System.Runtime.InteropServices;

namespace datntdev.Abp.Runtime.System
{
    public interface IOSPlatformProvider
    {
        OSPlatform GetCurrentOSPlatform();
    }
}