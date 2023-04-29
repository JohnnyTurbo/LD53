using Unity.Burst;
using Unity.Entities;

namespace TMG.LD53
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(CapabilitySystemGroup))]
    public partial struct ApplyDamageSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            new ApplyDamageJob
            {
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ApplyDamageJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        
        [BurstCompile]
        private void Execute(Entity entity, ref CurHitPoints curHitPoints, ref DynamicBuffer<DamageBufferElement> damageBuffer,
            [ChunkIndexInQuery] int sortKey)
        {
            var totalDamage = 0;
            foreach (var damage in damageBuffer)
            {
                totalDamage += damage.Value;
            }

            curHitPoints.Value -= totalDamage;
            damageBuffer.Clear();

            if (curHitPoints.Value > 0) return;
            ECB.AddComponent<DestroyEntityTag>(sortKey, entity);
        }
    }

}