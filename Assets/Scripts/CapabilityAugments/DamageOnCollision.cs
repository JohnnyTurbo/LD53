using Unity.Entities;
using UnityEngine.Serialization;

namespace TMG.LD53
{
    public struct DamageOnCollision : IComponentData
    {
        public int Value;
    }
}