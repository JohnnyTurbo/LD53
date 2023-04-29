using Unity.Burst;
using Unity.Entities;

namespace TMG.LD53
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial struct DestroyEntitySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            new DestroyEntityJob { ECB = ecb }.Schedule();
            
        }
    }
    
    [BurstCompile]
    [WithAll(typeof(DestroyEntityTag))]
    public partial struct DestroyEntityJob : IJobEntity
    {
        public EntityCommandBuffer ECB;
        
        [BurstCompile]
        private void Execute(Entity entityToDestroy)
        {
            ECB.DestroyEntity(entityToDestroy);
        }
    }
}