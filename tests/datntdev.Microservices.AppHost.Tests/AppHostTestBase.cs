using Aspire.Hosting;

namespace datntdev.Microservices.AppHost.Tests
{
    [TestClass]
    public abstract class AppHostTestBase
    {
        private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30);

        protected static DistributedApplication App { private set; get; } = default!;

        [AssemblyInitialize]
        public static async Task AssemblyInitAsync(TestContext context)
        {
            var cancellationToken = new CancellationTokenSource(_defaultTimeout).Token;

            App = await DistributedApplicationTestingBuilder
                .CreateAsync<Projects.datntdev_Microservices_AppHost>(cancellationToken)
                .ContinueWith(x => x.Result.BuildAsync(cancellationToken)).Unwrap();

            await App.StartAsync(cancellationToken);

            await Task.WhenAll(
                App.ResourceNotifications.WaitForResourceHealthyAsync("srv-identity", cancellationToken),
                App.ResourceNotifications.WaitForResourceHealthyAsync("srv-admin", cancellationToken)
            );
        }
    }
}
