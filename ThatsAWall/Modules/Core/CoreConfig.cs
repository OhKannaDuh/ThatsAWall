using Ocelot.Config.Attributes;
using Ocelot.Modules;

namespace ThatsAWall.Modules.Core;

[Title("Core Config")]
public class CoreConfig : ModuleConfig
{
    [Checkbox]
    [Label("generic.label.enabled")]
    public bool Enabled { get; set; } = true;
}
