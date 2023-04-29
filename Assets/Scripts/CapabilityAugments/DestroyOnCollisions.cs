using Unity.Entities;
using UnityEngine.Serialization;

namespace TMG.LD53
{
    public struct DestroyOnCollisions : IComponentData
    {
        public int Value;
    }
}