using Unity.Entities;
using UnityEngine;

namespace TMG.LD53
{
    public class ItemAuthoring : MonoBehaviour
    {
        public float MoveSpeed;
        
        public class ItemTagBaker : Baker<ItemAuthoring>
        {
            public override void Bake(ItemAuthoring authoring)
            {
                var itemEntity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<ItemTag>(itemEntity);
                AddComponent(itemEntity, new BaseMoveSpeed { Value = authoring.MoveSpeed });
            }
        }
    }
    
}