using Unity.Entities;

namespace TMG.LD53
{
    public struct EnemyAttack : IComponentData
    {
        public int Strength;
        public float Interval;
        public float Timer;
    }

    public struct EnemyAttackTarget : IComponentData, IEnableableComponent
    {
        public Entity Value;
    }
}