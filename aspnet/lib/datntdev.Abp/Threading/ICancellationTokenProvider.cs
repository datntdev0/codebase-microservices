using System;
using System.Threading;

namespace datntdev.Abp.Threading
{
    public interface ICancellationTokenProvider
    {
        CancellationToken Token { get; }
        IDisposable Use(CancellationToken cancellationToken);
    }
}
