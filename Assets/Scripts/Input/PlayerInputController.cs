using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TMG.LD53
{
    public class PlayerInputController : MonoBehaviour
    {
        private DeliveryGuyInputActions _deliveryGuyInput;

        private EntityManager _entityManager;
        private Entity _playerEntity;
        private EntityQuery _playerQuery;
        
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
            float lastHorizontalInput;
            if (math.abs(curInput.x) <= float.Epsilon)
            {
                lastHorizontalInput = math.sign(_entityManager.GetComponentData<PlayerMoveInput>(_playerEntity).LastHorizontal);
            }
            else
            {
                lastHorizontalInput = math.sign(curInput.x);
            }
            _entityManager.SetComponentData(_playerEntity, new PlayerMoveInput
            {
                Value = curInput,
                LastHorizontal = lastHorizontalInput
            });
        }

        private bool CheckPlayerEntity()
        {
            return _entityManager.Exists(_playerEntity) ||
                   _playerQuery.TryGetSingletonEntity<PlayerTag>(out _playerEntity);
        }
    }
}