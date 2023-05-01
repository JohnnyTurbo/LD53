using Unity.Burst;
using Unity.Entities;

namespace TMG.LD53
{
    [RequireMatchingQueriesForUpdate]
    public partial struct InitializePlayerRendererSystem : ISystem, ISystemStartStop
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerRendererTag>();
        }

        [BurstCompile]
        public void OnStartRunning(ref SystemState state)
        {
            var playerRendererEntity = SystemAPI.GetSingletonEntity<PlayerRendererTag>();
            foreach (var playerRenderer in SystemAPI.Query<RefRW<PlayerRenderer>>())
            {
                playerRenderer.ValueRW.Value = playerRendererEntity;
            }
        }

        public void OnStopRunning(ref SystemState state)
        {
        }
    }
}