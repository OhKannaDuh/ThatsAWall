
using Ocelot.Modules;

namespace ThatsAWall.Modules.Core;

[OcelotModule]
public class CoreModule : Module<Plugin, Config>
{
    public override CoreConfig config {
        get => _config.CoreConfig;
    }

    public CoreModule(Plugin plugin, Config config)
        : base(plugin, config) { }
}
