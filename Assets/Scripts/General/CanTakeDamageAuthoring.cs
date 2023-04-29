using Unity.Entities;
using UnityEngine;

namespace TMG.LD53
{
    public class CanTakeDamageAuthoring : MonoBehaviour
    {
        public class CanTakeDamageTagBaker : Baker<CanTakeDamageAuthoring>
        {
            public override void Bake(CanTakeDamageAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<CanTakeDamageTag>(entity);
                AddBuffer<DamageBufferElement>(entity);
            }
        }
    }
}