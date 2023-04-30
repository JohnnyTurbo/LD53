using Unity.Burst;
using Unity.Entities;

namespace TMG.LD53
{
    [UpdateInGroup(typeof(CapabilitySystemGroup))]
    public partial struct DestroyOnTriggersSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            new DestroyOnTriggersJob
            {
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct DestroyOnTriggersJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        
        [BurstCompile]
        private void Execute(Entity entity, ref DestroyOnTriggers triggers, 
            in DynamicBuffer<HitBufferElement> hitBuffer, [ChunkIndexInQuery]int sortKey)
        {
            foreach (var hit in hitBuffer)
            {
                if (hit.IsHandled) continue;
                triggers.Value--;
            }

            if (triggers.Value <= 0)
            {
                ECB.AddComponent<DestroyEntityTag>(sortKey, entity);
            }
        }
    }
}