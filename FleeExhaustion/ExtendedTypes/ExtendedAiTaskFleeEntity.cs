using System.Runtime.CompilerServices;
using Vintagestory.GameContent;

namespace FleeExhaustion.ExtendedTypes;

public class ExtendedAiTaskFleeEntity
{
    private static readonly ConditionalWeakTable<AiTaskFleeEntity, ExtendedAiTaskFleeEntity> ExtendedAiTasks = new();
    
    public readonly AiTaskFleeEntity AiTask;
    
    public float InitialMoveSpeed;
    public float MinimumMoveSpeed;
    public bool Exhausts;

    public ExtendedAiTaskFleeEntity(AiTaskFleeEntity aiTask)
    {
        AiTask = aiTask;
    }

    public static ExtendedAiTaskFleeEntity FromOriginal(AiTaskFleeEntity aiTask)
    {
        return ExtendedAiTasks.GetValue(aiTask, key => new ExtendedAiTaskFleeEntity(key));
    }
}