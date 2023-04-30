using Unity.Entities;

namespace TMG.LD53
{
    public struct CapabilityTimer : IComponentData
    {
        public float CooldownTime;
        public float Timer;
    }
}