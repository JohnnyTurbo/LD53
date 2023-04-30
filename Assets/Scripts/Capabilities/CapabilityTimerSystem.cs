using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

namespace TMG.LD53
{
    public partial struct CapabilityTimerSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            
            new CapabilityTimerJob
            {
                DeltaTime = deltaTime,
                PerformCapabilityLookup = SystemAPI.GetComponentLookup<PerformCapabilityTag>(false)
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct CapabilityTimerJob : IJobEntity
    {
        public float DeltaTime;
        [NativeDisableParallelForRestriction] public ComponentLookup<PerformCapabilityTag> PerformCapabilityLookup;
        
        [BurstCompile]
        private void Execute(Entity entity, ref CapabilityTimer timer)
        {
            timer.Timer -= DeltaTime;

            if (timer.Timer <= 0f)
            {
                timer.Timer = timer.CooldownTime;
                PerformCapabilityLookup.SetComponentEnabled(entity, true);
            }
        }
    }
}