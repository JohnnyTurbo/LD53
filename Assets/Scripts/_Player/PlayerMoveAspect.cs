using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TMG.LD53
{
    public readonly partial struct PlayerMoveAspect : IAspect
    {
        private readonly RefRW<LocalTransform> _localTransform;

        private readonly RefRO<PlayerMoveInput> _playerMoveInput;
        private readonly RefRO<BaseMoveSpeed> _baseMoveSpeed;

        private float2 PlayerMoveInput => _playerMoveInput.ValueRO.Value;
        private float BaseMoveSpeed => _baseMoveSpeed.ValueRO.Value;
        
        public void MovePlayer(float deltaTime)
        {
            _localTransform.ValueRW.Position.xz += PlayerMoveInput * BaseMoveSpeed * deltaTime;
            if (math.lengthsq(PlayerMoveInput) > float.Epsilon)
            {
                var playerMove3 = new float3(PlayerMoveInput.x, 0f, PlayerMoveInput.y);
                _localTransform.ValueRW.Rotation = quaternion.LookRotation(playerMove3, math.up());
            }
        }
    }
}