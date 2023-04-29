using Unity.Entities;
using UnityEngine;

namespace TMG.LD53
{
    public class MoneyPrefabsAuthoring : MonoBehaviour
    {
        public GameObject CoinPrefab;
        public GameObject BillPrefab;
        public GameObject StackPrefab;

        public class MoneyPrefabsBaker : Baker<MoneyPrefabsAuthoring>
        {
            public override void Bake(MoneyPrefabsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new MoneyPrefabs
                {
                    CoinPrefab = GetEntity(authoring.CoinPrefab, TransformUsageFlags.Dynamic),
                    BillPrefab = GetEntity(authoring.BillPrefab, TransformUsageFlags.Dynamic),
                    StackPrefab = GetEntity(authoring.StackPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}