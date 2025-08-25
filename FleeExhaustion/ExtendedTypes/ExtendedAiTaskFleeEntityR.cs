using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.GameContent;

namespace FleeExhaustion.ExtendedTypes;

public class ExtendedAiTaskFleeEntityR
{
    private static readonly ConditionalWeakTable<AiTaskFleeEntityR, ExtendedAiTaskFleeEntityR> ExtendedData = new();

    public float InitialMoveSpeed;
    public float MinimumMoveSpeed;
    public bool Exhausts;

    public ExtendedAiTaskFleeEntityR(AiTaskFleeEntityR original)
    {
        Traverse traverse = Traverse.Create(original);
        InitialMoveSpeed = traverse.Property("Config").Field<float>("MoveSpeed").Value;
        MinimumMoveSpeed = InitialMoveSpeed / 2;
        Exhausts = true;
    }

    public static ExtendedAiTaskFleeEntityR FromOriginal(AiTaskFleeEntityR original)
    {
        return ExtendedData.GetValue(original, o => new ExtendedAiTaskFleeEntityR(o));
    }
}