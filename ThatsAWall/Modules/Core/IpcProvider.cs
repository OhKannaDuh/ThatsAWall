using ECommons.EzIpcManager;
using Ocelot.Chain;

namespace ThatsAWall.Modules.Core;

public class IpcProvider
{
    private CoreModule module;

    public IpcProvider(CoreModule module)
    {
        this.module = module;

        EzIPC.Init(this);
    }

    // Pause
    private ChainQueue PauseQueue => ChainManager.Get("ThatsAWall_IPC_Pause");

    [EzIPC]
    public void Pause(int milliseconds)
    {
        PauseQueue.Abort();
        PauseQueue.Submit(() =>
            Chain.Create()
                .Then(_ => module.IsPaused = true)
                .Wait(milliseconds)
                .Then(_ => module.IsPaused = false)
        );
    }
}
