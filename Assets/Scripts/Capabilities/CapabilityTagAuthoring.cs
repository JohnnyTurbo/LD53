using Unity.Entities;
using UnityEngine;

namespace TMG.LD53
{
    public class CapabilityTagAuthoring : MonoBehaviour
    {
        public class CapabilityTagBaker : Baker<CapabilityTagAuthoring>
        {
            public override void Bake(CapabilityTagAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<CapabilityTag>(entity);
            }
        }
    }
}