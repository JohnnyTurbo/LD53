using Unity.Burst;
using Unity.Entities;

namespace TMG.LD53
{
    public partial struct ApplyEnemyDamageSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            
            new UpdateEnemyAttackTimerJob { DeltaTime = deltaTime }.ScheduleParallel();
            new ApplyEnemyDamageJob
            {
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct UpdateEnemyAttackTimerJob : IJobEntity
    {
        public float DeltaTime;
        
        [BurstCompile]
        private void Execute(ref EnemyAttack attack)
        {
            if (attack.Timer <= 0f) return;
            attack.Timer -= DeltaTime;
        }
    }

    
    [BurstCompile]
    public partial struct ApplyEnemyDamageJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        
        [BurstCompile]
        private void Execute(Entity enemyEntity, ref EnemyAttack attack, in EnemyAttackTarget target,
            [ChunkIndexInQuery] int sortKey)
        {
            ECB.SetComponentEnabled<EnemyAttackTarget>(sortKey, enemyEntity, false);
            if (attack.Timer > 0f) return;
            ECB.AppendToBuffer(sortKey, target.Value, new DamageBufferElement { Value = attack.Strength });
            attack.Timer = attack.Interval;
        }
    }
}