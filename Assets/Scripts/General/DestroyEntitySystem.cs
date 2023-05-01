using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace TMG.LD53
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial struct DestroyEntitySystem : ISystem, ISystemStartStop
    {
        private Entity _gameControlEntity;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GameControlTag>();
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            new DestroyEntityJob
            {
                ECB = ecb,
                GameControlEntity = _gameControlEntity,
                PlayerLookup = SystemAPI.GetComponentLookup<PlayerTag>()
            }.Schedule();
            
        }

        public void OnStartRunning(ref SystemState state)
        {
            _gameControlEntity = SystemAPI.GetSingletonEntity<GameControlTag>();
        }

        public void OnStopRunning(ref SystemState state)
        {
            
        }
    }
    
    [BurstCompile]
    [WithAll(typeof(DestroyEntityTag))]
    public partial struct DestroyEntityJob : IJobEntity
    {
        public EntityCommandBuffer ECB;
        public Entity GameControlEntity;
        
        [ReadOnly] public ComponentLookup<PlayerTag> PlayerLookup;
        
        [BurstCompile]
        private void Execute(Entity entityToDestroy)
        {
            ECB.DestroyEntity(entityToDestroy);
            if (PlayerLookup.HasComponent(entityToDestroy))
            {
                ECB.AddComponent<GameOverTag>(GameControlEntity);
            }
        }
    }
}