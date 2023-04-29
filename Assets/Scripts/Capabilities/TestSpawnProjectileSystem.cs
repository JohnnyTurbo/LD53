using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace TMG.LD53
{
    public partial struct TestSpawnProjectileSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            
            foreach (var (testForwardProjectile, transform) in SystemAPI.Query<RefRW<TestForwardProjectile>, LocalTransform>())
            {
                testForwardProjectile.ValueRW.Timer -= deltaTime;
                if (testForwardProjectile.ValueRO.Timer <= 0f)
                {
                    var newProjectile = ecb.Instantiate(testForwardProjectile.ValueRO.Prefab);
                    var newTransform = LocalTransform.FromPositionRotation(transform.Position, transform.Rotation);
                    ecb.SetComponent(newProjectile, newTransform);
                    testForwardProjectile.ValueRW.Timer = testForwardProjectile.ValueRO.CooldownTime;
                }
            }
        }
    }
}