using System;
using ECommons.DalamudServices;
using Ocelot;
using ThatsAWall.Modules.Core;

namespace ThatsAWall;

[Serializable]
public class Config : IOcelotConfig
{
    public int Version { get; set; } = 1;

    public CoreConfig CoreConfig { get; set; } = new();

    public void Save() => Svc.PluginInterface.SavePluginConfig(this);
}
