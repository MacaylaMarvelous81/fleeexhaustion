using HarmonyLib;
using Vintagestory.API.Common;

namespace FleeExhaustion;

public class FleeExhaustionModSystem : ModSystem
{
    private Harmony _harmony;
    
    public override void Start(ICoreAPI api)
    {
        if (!Harmony.HasAnyPatches(Mod.Info.ModID))
        {
            _harmony = new Harmony(Mod.Info.ModID);
            _harmony.PatchAll();
        }
    }
}