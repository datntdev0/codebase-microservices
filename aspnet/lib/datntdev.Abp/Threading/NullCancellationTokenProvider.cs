using System.Threading;
using datntdev.Abp.Runtime.Remoting;

namespace datntdev.Abp.Threading
{
    public class NullCancellationTokenProvider : CancellationTokenProviderBase
    {
        public static NullCancellationTokenProvider Instance { get; } = new NullCancellationTokenProvider();

        public override CancellationToken Token => CancellationToken.None;

        private NullCancellationTokenProvider()
        : base(
            new DataContextAmbientScopeProvider<CancellationTokenOverride>(new AsyncLocalAmbientDataContext()))
        {
        }
    }
}
