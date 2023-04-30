using Unity.Entities;
using Unity.Physics.Authoring;
using UnityEngine;

namespace TMG.LD53
{
    [RequireComponent(typeof(PhysicsShapeAuthoring))]
    [RequireComponent(typeof(CapabilityTagAuthoring))]
    public class TriggerDetectorAuthoring : MonoBehaviour
    {
        public class TriggerDetectorBaker : Baker<TriggerDetectorAuthoring>
        {
            public override void Bake(TriggerDetectorAuthoring authoring)
            {
                var capabilityEntity = GetEntity(TransformUsageFlags.Dynamic);
                AddBuffer<HitBufferElement>(capabilityEntity);
            }
        }
    }
}