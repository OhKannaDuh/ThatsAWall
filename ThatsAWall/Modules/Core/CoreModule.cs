using System.Numerics;
using Dalamud.Plugin.Services;
using ECommons.Automation.NeoTaskManager;
using ECommons.GameHelpers;
using ECommons.Throttlers;
using Ocelot.Chain;
using Ocelot.IPC;
using Ocelot.Modules;

namespace ThatsAWall.Modules.Core;

[OcelotModule]
public class CoreModule : Module<Plugin, Config>
{
    public override CoreConfig config {
        get => _config.CoreConfig;
    }

    public bool IsPaused = false;

    public override bool enabled => config.Enabled && !IsPaused;

    private IpcProvider ipcProvider;

    public CoreModule(Plugin plugin, Config config)
        : base(plugin, config)
    {
        ipcProvider = new(this);
    }

    public override void Tick(IFramework _)
    {
        // Check config is enabled and we have vnav installed and ready
        if (!enabled || !TryGetIPCProvider<VNavmesh>(out var vnav) || vnav == null)
        {
            return;
        }

        // Check if we're already watching OR vnav isn't running
        if (Plugin.Chain.IsRunning || !vnav.IsRunning())
        {
            return;
        }

        Info("Detected vnavmesh running, starting watcher");

        Plugin.Chain.Submit(() => {
            if (EzThrottler.Throttle("VnavMeshWatcher", 5000))
            {
                Debug("Vnavmesh watcher running...");
            }

            var lastPosition = Player.Position;
            var idleTicks = 0;

            return Chain.Create("Vnav Watcher")
                .Wait(1000) // Give 1 second for vnav to get started
                .Then(new TaskManagerTask(() => {
                    if (!vnav.IsRunning())
                    {
                        return true;
                    }

                    var currentPosition = Player.Position;
                    var hasMoved = Vector3.Distance(lastPosition, currentPosition) > 0.05f;
                    lastPosition = currentPosition;

                    if (!hasMoved)
                    {
                        idleTicks++;
                    }
                    else
                    {
                        idleTicks = 0;
                    }

                    if (idleTicks >= 30)
                    {
                        vnav.Stop();
                        return true;
                    }

                    return false;
                }, new() { TimeLimitMS = int.MaxValue, TimeoutSilently = true }));
        });
    }
}
