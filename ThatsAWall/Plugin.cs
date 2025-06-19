using System.Numerics;
using Dalamud.Plugin;
using ECommons;
using ECommons.DalamudServices;
using Ocelot;
using Ocelot.Chain;
using Ocelot.IPC;

namespace ThatsAWall;

public sealed class Plugin : OcelotPlugin
{
    public override string Name {
        get => "ThatsAWall";
    }

    public Config config { get; init; }

    public override IOcelotConfig _config => config;

    public static ChainQueue Chain => ChainManager.Get("ThatsAWall##main");

    public Plugin(IDalamudPluginInterface plugin)
        : base(plugin, Module.DalamudReflector)
    {
        config = plugin.GetPluginConfig() as Config ?? new Config();

        I18N.SetDirectory(plugin.AssemblyLocation.Directory?.FullName!);
        I18N.LoadFromFile("en", "Translations/en.json");

        OcelotInitialize();

        ChainManager.Initialize();

        Svc.Framework.RunOnTick(() => {
            var vnav = ipc.GetProvider<VNavmesh>();
            vnav.MoveToPath([new Vector3(814f, 72f, -679f)], false);
        });
    }


    public override bool ShouldTick() => true;

    public override void Dispose()
    {
        base.Dispose();
        ChainManager.Close();
    }
}
