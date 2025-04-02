using System;
using FleeExhaustion.Configuration;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace FleeExhaustion;

public class FleeExhaustionModSystem : ModSystem
{
    private Harmony _harmony;
    
    public ServerConfiguration ServerConfig { get; private set; }
    
    public override void Start(ICoreAPI api)
    {
        if (!Harmony.HasAnyPatches(Mod.Info.ModID))
        {
            _harmony = new Harmony(Mod.Info.ModID);
            _harmony.PatchAll();
        }
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        try
        {
            ServerConfig = api.LoadModConfig<ServerConfiguration>($"{Mod.Info.ModID}-server.json") ?? new ServerConfiguration();
            api.StoreModConfig<ServerConfiguration>(ServerConfig, $"{Mod.Info.ModID}-server.json");
        }
        catch (Exception e)
        {
            Mod.Logger.Error("An exception occured while loading server config, so default values are being used instead.");
            Mod.Logger.Error(e);
            ServerConfig = new ServerConfiguration();
        }
    }
}