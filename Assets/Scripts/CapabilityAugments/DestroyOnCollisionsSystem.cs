using Unity.Burst;
using Unity.Entities;

namespace TMG.LD53
{
    public partial struct DestroyOnCollisionsSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            new DestroyOnCollisionsJob
            {
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct DestroyOnCollisionsJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        
        [BurstCompile]
        private void Execute(Entity entity, ref DestroyOnCollisions collisions, 
            ref DynamicBuffer<HitBufferElement> hitBuffer, [ChunkIndexInQuery]int sortKey)
        {
            for (var i = 0; i < hitBuffer.Length; i++)
            {
                var hit = hitBuffer[i];
                if (hit.IsHandled) continue;
                collisions.Value--;
                hit.IsHandled = true;
                hitBuffer[i] = hit;
            }

            if (collisions.Value <= 0)
            {
                ECB.AddComponent<DestroyEntityTag>(sortKey, entity);
            }
        }
    }
}