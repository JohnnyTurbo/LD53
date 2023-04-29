using Unity.Entities;

namespace TMG.LD53
{
    public struct BaseHitPoints : IComponentData
    {
        public int Value;
    }

    public struct CurHitPoints : IComponentData
    {
        public int Value;
    }
}