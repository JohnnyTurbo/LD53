using System;
using Unity.Entities;
using UnityEngine;

namespace TMG.LD53
{
    public class PlayerInputController : MonoBehaviour
    {
        private DeliveryGuyInputActions _deliveryGuyInput;

        private EntityManager _entityManager;
        private Entity _playerEntity;
        private EntityQuery _playerQuery;
        private bool _hasPlayerEntity;
        
        private void Awake()
        {
            _deliveryGuyInput = new DeliveryGuyInputActions();
        }

        private void OnEnable()
        {
            _deliveryGuyInput.Enable();
        }

        private void Start()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _playerQuery = _entityManager.CreateEntityQuery(typeof(PlayerMoveInput));
        }

        private void OnDisable()
        {
            _deliveryGuyInput.Disable();
        }

        private void Update()
        {
            if(!CheckPlayerEntity()) return;
            var curInput = _deliveryGuyInput.GameplayMap.PlayerMovement.ReadValue<Vector2>();
            _entityManager.SetComponentData(_playerEntity, new PlayerMoveInput { Value = curInput });
        }

        private bool CheckPlayerEntity()
        {
            if (_hasPlayerEntity) return true;
            _hasPlayerEntity = _playerQuery.TryGetSingletonEntity<PlayerMoveInput>(out _playerEntity);
            return _hasPlayerEntity;
        }
    }
}