using VContainer;
using VContainer.Unity;

namespace __Scripts._DIContainer_Bootstrappers.GameplayContainer
{
    public class GameplayInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameplayBootstrapper>();
        }
    }
}