using Unity.Entities;
using Unity.Mathematics;

namespace TMG.LD53
{
    public struct PlayerMoveInput : IComponentData
    {
        public float2 Value;
        public float LastHorizontal;
    }
}