using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.LD53
{
    [RequireMatchingQueriesForUpdate]
    public partial struct EnemyMoveSystem : ISystem, ISystemStartStop
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
        }

        private Entity _playerEntity;
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var playerPosition = SystemAPI.GetComponent<LocalTransform>(_playerEntity).Position;
            new EnemyMoveJob
            {
                DeltaTime = deltaTime,
                PlayerPosition = playerPosition
            }.ScheduleParallel();
        }

        public void OnStartRunning(ref SystemState state)
        {
            _playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        }

        public void OnStopRunning(ref SystemState state)
        {
        }
    }

    [BurstCompile]
    [WithAll(typeof(EnemyTag))]
    public partial struct EnemyMoveJob : IJobEntity
    {
        public float DeltaTime;
        public float3 PlayerPosition;
        
        [BurstCompile]
        private void Execute(ref LocalTransform transform, in BaseMoveSpeed moveSpeed)
        {
            var directionToPlayer = math.normalizesafe(PlayerPosition - transform.Position);
            transform.Position += directionToPlayer * moveSpeed.Value * DeltaTime;
        }
    }
}