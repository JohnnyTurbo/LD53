using Unity.Burst;
using Unity.Entities;

namespace TMG.LD53
{
    public partial struct InitializePlayerRendererSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerRendererTag>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            var playerRendererEntity = SystemAPI.GetSingletonEntity<PlayerRendererTag>();
            foreach (var playerRenderer in SystemAPI.Query<RefRW<PlayerRenderer>>())
            {
                playerRenderer.ValueRW.Value = playerRendererEntity;
            }
        }
    }
}