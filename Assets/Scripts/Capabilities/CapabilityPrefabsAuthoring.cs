using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace TMG.LD53
{
    public class CapabilityPrefabsAuthoring : MonoBehaviour
    {
        public GameObject CheeseWhipPrefab;

        public class CapabilityPrefabsBaker : Baker<CapabilityPrefabsAuthoring>
        {
            public override void Bake(CapabilityPrefabsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new CapabilityPrefabs
                {
                    CheeseWhipEntity = GetEntity(authoring.CheeseWhipPrefab, TransformUsageFlags.None)
                });
            }
        }
    }
}