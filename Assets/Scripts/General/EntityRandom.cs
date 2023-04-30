using Unity.Entities;
using Unity.Mathematics;

namespace TMG.LD53
{
    public struct EntityRandom : IComponentData
    {
        public Random Value;
    }
}