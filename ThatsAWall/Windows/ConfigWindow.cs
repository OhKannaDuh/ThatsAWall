using Ocelot.Windows;

namespace ThatsAWall.Windows;

[OcelotConfigWindow]
public class ConfigWindow : OcelotConfigWindow
{
    public ConfigWindow(Plugin plugin, Config config)
        : base(plugin, config) { }
}
