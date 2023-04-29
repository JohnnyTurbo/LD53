using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace TMG.LD53
{
    public partial struct MoveProjectileSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;

            new MoveProjectileJob
            {
                DeltaTime = deltaTime
            }.ScheduleParallel();
            }
    }
    
    [BurstCompile]
    public partial struct MoveProjectileJob : IJobEntity
    {
        public float DeltaTime;
        
        [BurstCompile]
        private void Execute(ref LocalTransform transform, in ProjectileSpeed speed)
        {
            transform.Position += transform.Forward() * speed.Value * DeltaTime;
        }
    }
}