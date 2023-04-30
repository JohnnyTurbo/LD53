using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace TMG.LD53
{
    public class CapabilityPrefabsAuthoring : MonoBehaviour
    {
        public GameObject CheeseWhipPrefab;
        public GameObject SausageSlingshotPrefab;
        public GameObject MarinaraMaelstromPrefab;
        
        public class CapabilityPrefabsBaker : Baker<CapabilityPrefabsAuthoring>
        {
            public override void Bake(CapabilityPrefabsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new CapabilityPrefabs
                {
                    CheeseWhipPrefab = GetEntity(authoring.CheeseWhipPrefab, TransformUsageFlags.Dynamic),
                    SausageSlingshotPrefab = GetEntity(authoring.SausageSlingshotPrefab, TransformUsageFlags.Dynamic),
                    MarinaraMaelstromPrefab = GetEntity(authoring.MarinaraMaelstromPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}