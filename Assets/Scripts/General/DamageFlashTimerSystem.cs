using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;

namespace TMG.LD53
{
    public partial struct DamageFlashTimerSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var deltaTime = SystemAPI.Time.DeltaTime;

            new DamageFlashTimerJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    [WithAll(typeof(URPMaterialPropertyBaseColor))]
    public partial struct DamageFlashTimerJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        public float DeltaTime;
        
        [BurstCompile]
        private void Execute(Entity entity, ref DamageFlashTimer timer, [ChunkIndexInQuery] int sortKey)
        {
            timer.Value -= DeltaTime;
            if (timer.Value <= 0f)
            {
                ECB.RemoveComponent<URPMaterialPropertyBaseColor>(sortKey, entity);
                ECB.RemoveComponent<DamageFlashTimer>(sortKey, entity);
            }
        }
    }
}