using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

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
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

            new PlayerFlashOnDamageJob { ECB = ecb }.ScheduleParallel();
            new EnemyFlashOnDamageJob { ECB = ecb }.ScheduleParallel();
            new ApplyDamageJob
            {
                ECB = ecb
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct PlayerFlashOnDamageJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        
        [BurstCompile]
        private void Execute(in DynamicBuffer<DamageBufferElement> damageBuffer, in PlayerRenderer playerRenderer,
            [ChunkIndexInQuery] int sortKey)
        {
            if(damageBuffer.IsEmpty) return;
            ECB.AddComponent(sortKey, playerRenderer.Value, new DamageFlashTimer { Value = 0.2f });
            ECB.AddComponent(sortKey, playerRenderer.Value,
                new URPMaterialPropertyBaseColor { Value = new float4(1, 0, 0, 1) });
        }
    }

    [BurstCompile]
    [WithAll(typeof(EnemyTag))]
    [WithNone(typeof(PlayerTag))]
    public partial struct EnemyFlashOnDamageJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        
        [BurstCompile]
        private void Execute(Entity enemyEntity, in DynamicBuffer<DamageBufferElement> damageBuffer, 
            [ChunkIndexInQuery] int sortKey)
        {
            if(damageBuffer.IsEmpty) return;
            ECB.AddComponent(sortKey, enemyEntity, new DamageFlashTimer { Value = 0.2f });
            ECB.AddComponent(sortKey, enemyEntity, new URPMaterialPropertyBaseColor { Value = new float4(1, 0, 0, 1) });
        }
    }

    
    [BurstCompile]
    public partial struct ApplyDamageJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        
        [BurstCompile]
        private void Execute(Entity entity, ref CurHitPoints curHitPoints, 
            ref DynamicBuffer<DamageBufferElement> damageBuffer, [ChunkIndexInQuery] int sortKey)
        {
            if (damageBuffer.IsEmpty) return;
            
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