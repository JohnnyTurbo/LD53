using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Physics;

namespace TMG.LD53
{
    [UpdateInGroup(typeof(CapabilitySystemGroup))]
    public partial struct KnockBackOnTriggerSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new KnockBackOnTriggerJob
            {
                VelocityLookup = SystemAPI.GetComponentLookup<PhysicsVelocity>(false)
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct KnockBackOnTriggerJob : IJobEntity
    {
        [NativeDisableContainerSafetyRestriction] public ComponentLookup<PhysicsVelocity> VelocityLookup;
        
        [BurstCompile]
        private void Execute(in DynamicBuffer<HitBufferElement> hitBuffer, in KnockBackOnTrigger knockBackOnTrigger)
        {
            foreach (var hit in hitBuffer)
            {
                if (hit.IsHandled) continue;
                if (!VelocityLookup.HasComponent(hit.HitEntity)) continue;
                var velocity = new PhysicsVelocity { Linear = hit.HitDirection * knockBackOnTrigger.Strength };
                VelocityLookup[hit.HitEntity] = velocity;
            }
        }
    }

}