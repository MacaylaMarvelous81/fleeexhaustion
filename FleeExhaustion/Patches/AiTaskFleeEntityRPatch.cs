using System;
using FleeExhaustion.ExtendedTypes;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace FleeExhaustion.Patches;

[HarmonyPatch]
public class AiTaskFleeEntityRPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(AiTaskFleeEntityR), MethodType.Constructor, new[] { typeof(EntityAgent), typeof(JsonObject), typeof(JsonObject) })]
    public static void Postfix(AiTaskFleeEntityR __instance, JsonObject taskConfig)
    {
        ExtendedAiTaskFleeEntityR extended = ExtendedAiTaskFleeEntityR.FromOriginal(__instance);
        extended.InitialMoveSpeed = taskConfig["movespeed"].AsFloat(extended.InitialMoveSpeed);
        extended.MinimumMoveSpeed = taskConfig["minmovespeed"].AsFloat(extended.InitialMoveSpeed / 2);
        extended.Exhausts = taskConfig["exhausts"].AsBool(true);
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(AiTaskFleeEntityR), "StartExecute")]
    public static void PreStartExecute(AiTaskFleeEntityR __instance)
    {
        ExtendedAiTaskFleeEntityR extended = ExtendedAiTaskFleeEntityR.FromOriginal(__instance);

        if (!extended.Exhausts) return;

        var entity = Traverse.Create(__instance).Field<EntityAgent>("entity").Value;

        float configExhaustStrength =
            entity.World.Api.ModLoader.GetModSystem<FleeExhaustionModSystem>().ServerConfig.ExhaustStrength;
        // Reciprocal of the exhaust strength in the config, or 1 if it is configured to be 0.
        float exhaustStrength = configExhaustStrength == 0 ? 1 : 1 / configExhaustStrength;

        EntityBehaviorHealth health = entity.GetBehavior<EntityBehaviorHealth>();
        if (health != null)
        {
            Traverse.Create(__instance).Property("Config").Field<float>("MoveSpeed").Value = Math.Clamp(
                extended.InitialMoveSpeed * (health.Health / health.MaxHealth) * exhaustStrength,
                extended.MinimumMoveSpeed, extended.InitialMoveSpeed);
        }
    }
}