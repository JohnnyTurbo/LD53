using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace TMG.LD53
{
    public class FollowPlayerEntity : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        
        private EntityManager _entityManager;
        private Entity _playerEntity;
        private EntityQuery _playerQuery;
        
        private void Start()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _playerQuery = _entityManager.CreateEntityQuery(typeof(PlayerTag), typeof(LocalTransform));
        }
        
        private void LateUpdate()
        {
            if(!CheckPlayerEntity()) return;
            var playerPosition = _entityManager.GetComponentData<LocalTransform>(_playerEntity).Position;
            transform.position = _offset + (Vector3)playerPosition;
        }

        private bool CheckPlayerEntity()
        {
            return _entityManager.Exists(_playerEntity) ||
                   _playerQuery.TryGetSingletonEntity<PlayerTag>(out _playerEntity);
        }
    }
}