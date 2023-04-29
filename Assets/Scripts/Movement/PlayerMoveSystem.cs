using Unity.Burst;
using Unity.Entities;

namespace TMG.LD53
{
    public partial struct PlayerMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var playerMove in SystemAPI.Query<PlayerMoveAspect>())
            {
                playerMove.MovePlayer(deltaTime);
            }
        }
    }
}