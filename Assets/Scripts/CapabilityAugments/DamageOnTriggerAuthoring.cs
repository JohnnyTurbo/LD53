using Unity.Entities;
using UnityEngine;

namespace TMG.LD53
{
    [RequireComponent(typeof(TriggerDetectorAuthoring))]
    public class DamageOnTriggerAuthoring : MonoBehaviour
    {
        public int DamageValue;

        public class DamageOnTriggerBaker : Baker<DamageOnTriggerAuthoring>
        {
            public override void Bake(DamageOnTriggerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new DamageOnTrigger { Value = authoring.DamageValue });
            }
        }
    }
}