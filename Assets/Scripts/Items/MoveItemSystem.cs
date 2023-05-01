using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.LD53
{
    [BurstCompile]
    public partial struct MoveItemSystem : ISystem
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
            
            new MoveItemJob
            {
                TransformLookup = SystemAPI.GetComponentLookup<LocalTransform>(),
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct MoveItemJob : IJobEntity
    {
        [ReadOnly, NativeDisableContainerSafetyRestriction] public ComponentLookup<LocalTransform> TransformLookup;
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;
        
        [BurstCompile]
        private void Execute(Entity itemEntity, ref LocalTransform transform, in BaseMoveSpeed moveSpeed, 
            in ItemEntityTarget target, in ExperiencePointValue experiencePointValue, [ChunkIndexInQuery] int sortKey)
        {
            var targetPosition = TransformLookup[target.Value].Position;
            var directionToTarget = math.normalizesafe(targetPosition - transform.Position);
            transform.Position += directionToTarget * moveSpeed.Value * DeltaTime;

            if (math.distancesq(targetPosition, transform.Position) <= 0.001f)
            {
                ECB.AppendToBuffer(sortKey, target.Value,
                    new ExperienceBufferElement { Value = experiencePointValue.Value });
                ECB.AddComponent<DestroyEntityTag>(sortKey, itemEntity);
            }
        }
    }
}