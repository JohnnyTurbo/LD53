using Unity.Entities;
using Unity.Mathematics;

namespace TMG.LD53
{
    [InternalBufferCapacity(4)]
    public struct HitBufferElement : IBufferElementData
    {
        public Entity HitEntity;
        public float3 HitPosition;
        public bool IsHandled;
    }
}