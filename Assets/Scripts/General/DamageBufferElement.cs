using Unity.Entities;

namespace TMG.LD53
{
    [InternalBufferCapacity(1)]
    public struct DamageBufferElement : IBufferElementData
    {
        public int Value;
    }
}