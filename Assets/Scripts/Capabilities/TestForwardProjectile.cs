using Unity.Entities;

namespace TMG.LD53
{
    public struct TestForwardProjectile : IComponentData
    {
        public Entity Prefab;
        public float CooldownTime;
        public float Timer;
    }
}