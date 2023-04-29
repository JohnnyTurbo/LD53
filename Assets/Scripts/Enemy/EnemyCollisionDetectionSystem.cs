using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace TMG.LD53
{
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(PhysicsSimulationGroup))]
    [UpdateBefore(typeof(ExportPhysicsWorld))]
    public partial struct EnemyCollisionDetectionSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimulationSingleton>();
            state.RequireForUpdate<PhysicsWorldSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var simSingleton = SystemAPI.GetSingleton<SimulationSingleton>();
            state.Dependency = new EnemyCollisionDetectionJob
            {
                PlayerLookup = SystemAPI.GetComponentLookup<PlayerTag>(true),
                TargetLookup = SystemAPI.GetComponentLookup<EnemyAttackTarget>(false)
            }.Schedule(simSingleton, state.Dependency);
        }
    }

    public struct EnemyCollisionDetectionJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentLookup<PlayerTag> PlayerLookup;
        public ComponentLookup<EnemyAttackTarget> TargetLookup;
        
        public void Execute(CollisionEvent collisionEvent)
        {
            var entityA = collisionEvent.EntityA;
            var entityB = collisionEvent.EntityB;

            var isEntityAPlayer = PlayerLookup.HasComponent(entityA);
            var isEntityBPlayer = PlayerLookup.HasComponent(entityB);

            if (!isEntityAPlayer && !isEntityBPlayer) return;

            var playerEntity = isEntityAPlayer ? entityA : entityB;
            var enemyEntity = isEntityAPlayer ? entityB : entityA;

            TargetLookup.SetComponentEnabled(enemyEntity, true);
            TargetLookup[enemyEntity] = new EnemyAttackTarget { Value = playerEntity };
        }
    }
    
}