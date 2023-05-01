using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.LD53
{
    public partial struct FollowPlayerRendererSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var player = SystemAPI.GetSingletonEntity<PlayerTag>();
            var playerPos = SystemAPI.GetComponent<LocalTransform>(player).Position;
            var playerInput = SystemAPI.GetComponent<PlayerMoveInput>(player).LastHorizontal;
            
            foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<PlayerRendererTag>())
            {
                transform.ValueRW.Position = playerPos;
                var rotFactor = playerInput < 0 ? 1 : 0;
                var rotation = quaternion.Euler(0f, 0f, rotFactor * math.PI);
                transform.ValueRW.Rotation = rotation;
            }
        }
    }
}