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
    [HarmonyPatch(typeof(AiTaskFleeEntity), nameof(AiTaskFleeEntity.LoadConfig))]
    public static void PostLoadConfig(AiTaskFleeEntity __instance, JsonObject taskConfig)
    {
        ExtendedAiTaskFleeEntity extended = ExtendedAiTaskFleeEntity.FromOriginal(__instance);
        extended.InitialMoveSpeed = taskConfig["movespeed"].AsFloat(0.02f);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AiTaskFleeEntity), nameof(AiTaskFleeEntity.StartExecute))]
    public static void PreStartExecute(AiTaskFleeEntity __instance)
    {
        ExtendedAiTaskFleeEntity extended = ExtendedAiTaskFleeEntity.FromOriginal(__instance);
        Traverse traverse = Traverse.Create(__instance);
        Traverse moveSpeed = traverse.Field("moveSpeed");

        EntityBehaviorHealth health = __instance.entity.GetBehavior<EntityBehaviorHealth>();
        if (health != null)
        {
            moveSpeed.SetValue(Math.Min(extended.InitialMoveSpeed,
                extended.InitialMoveSpeed * (health.Health / health.MaxHealth)));
        }
    }
}