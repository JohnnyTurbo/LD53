using Unity.Entities;
using UnityEngine.Serialization;

namespace TMG.LD53
{
    public struct DestroyOnTriggers : IComponentData
    {
        public int Value;
    }
}