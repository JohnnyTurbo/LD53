using Unity.Entities;
using UnityEngine;

namespace TMG.LD53
{
    public class GameControlTagAuthoring : MonoBehaviour
    {
        public class GameControlTagBaker : Baker<GameControlTagAuthoring>
        {
            public override void Bake(GameControlTagAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent<GameControlTag>(entity);
            }
        }
    }
}