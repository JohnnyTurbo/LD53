using Unity.Entities;

namespace TMG.LD53
{
    [InternalBufferCapacity(1)]
    public struct ExperienceBufferElement : IBufferElementData
    {
        public int Value;
    }
}