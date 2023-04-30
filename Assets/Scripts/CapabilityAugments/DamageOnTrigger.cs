using Unity.Entities;
using UnityEngine.Serialization;

namespace TMG.LD53
{
    public struct DamageOnTrigger : IComponentData
    {
        public int Value;
    }
}