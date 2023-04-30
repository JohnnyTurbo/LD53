using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace TMG.LD53
{
    [UpdateInGroup(typeof(CapabilitySystemGroup))]
    public partial struct DamageOnTriggerSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            new DamageOnTriggerJob
            {
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                CanTakeDamageLookup = SystemAPI.GetComponentLookup<CanTakeDamageTag>(true)
            }.ScheduleParallel();
        }
        
    }
    
    [BurstCompile]
    public partial struct DamageOnTriggerJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        [ReadOnly] public ComponentLookup<CanTakeDamageTag> CanTakeDamageLookup;

        [BurstCompile]
        private void Execute(in DynamicBuffer<HitBufferElement> hitBuffer, in DamageOnTrigger damageOnTrigger,
            [ChunkIndexInQuery] int sortKey)
        {
            foreach (var hit in hitBuffer)
            {
                if(hit.IsHandled) continue;
                if (!CanTakeDamageLookup.HasComponent(hit.HitEntity)) continue;
                ECB.AppendToBuffer(sortKey, hit.HitEntity, new DamageBufferElement { Value = damageOnTrigger.Value });
            }
        }
    }
}