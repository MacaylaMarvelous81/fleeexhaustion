using System;
using FleeExhaustion.ExtendedTypes;
using HarmonyLib;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace FleeExhaustion.Patches;

[HarmonyPatch]
public class AiTaskFleeEntityPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(AiTaskFleeEntity), MethodType.Constructor)]
    public static void PostLoadConfig(AiTaskFleeEntity __instance, JsonObject taskConfig)
    {
        ExtendedAiTaskFleeEntity extended = ExtendedAiTaskFleeEntity.FromOriginal(__instance);
        extended.InitialMoveSpeed = taskConfig["movespeed"].AsFloat(0.02f);
        extended.MinimumMoveSpeed = taskConfig["minmovespeed"].AsFloat(extended.InitialMoveSpeed / 2);
        extended.Exhausts = taskConfig["exhausts"].AsBool(true);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AiTaskFleeEntity), nameof(AiTaskFleeEntity.StartExecute))]
    public static void PreStartExecute(AiTaskFleeEntity __instance)
    {
        ExtendedAiTaskFleeEntity extended = ExtendedAiTaskFleeEntity.FromOriginal(__instance);

        if (!extended.Exhausts) return;
        
        Traverse traverse = Traverse.Create(__instance);
        Traverse moveSpeed = traverse.Field("moveSpeed");

        float configExhaustStrength =
            __instance.world.Api.ModLoader.GetModSystem<FleeExhaustionModSystem>().ServerConfig.ExhaustStrength;
        // Reciprocal of the exhaust strength in the config, or 1 if it is configured to be 0.
        float exhaustStrength = configExhaustStrength == 0 ? 1 : 1 / configExhaustStrength;

        EntityBehaviorHealth health = __instance.entity.GetBehavior<EntityBehaviorHealth>();
        if (health != null)
        {
            moveSpeed.SetValue(Math.Clamp(extended.InitialMoveSpeed * (health.Health / health.MaxHealth) * exhaustStrength, extended.MinimumMoveSpeed, extended.InitialMoveSpeed));
        }

        __instance.entity.DebugAttributes.SetFloat("movespeed", moveSpeed.GetValue<float>());
        __instance.entity.DebugAttributes.SetFloat("minspeed", extended.MinimumMoveSpeed);
        __instance.entity.DebugAttributes.SetFloat("exhauststrength", exhaustStrength);
    }
}