using Unity.Entities;

namespace TMG.LD53
{
    public struct MarinaraMaelstromProperties : IComponentData
    {
        public int NumberToSpawn;
        public int BaseHitPoints;
        public float TimeBetweenSpawns;
        public float Timer;
        public int NumberSpawned;
    }
}