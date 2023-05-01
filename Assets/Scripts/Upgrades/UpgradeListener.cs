using Unity.Entities;
using UnityEngine;

namespace TMG.LD53
{
    public class UpgradeListener : MonoBehaviour
    {
        public int Level { get; private set; }
        protected EntityManager EntityManager;
        protected Entity WeaponEntity;
        //protected CapabilityPrefabs PrefabContainer;

        private bool _hasContainer;
        private EntityQuery _prefabEntityQuery;
        
        [SerializeField] private UpgradeElement _upgradeElement;

        private void OnEnable()
        {
            _upgradeElement.Register(this);
        }
        
        private void OnDisable()
        {
            _upgradeElement.Deregister(this);
        }

        private void Start()
        {
            EntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _prefabEntityQuery = EntityManager.CreateEntityQuery(typeof(CapabilityPrefabs));
        }

        public virtual void UpgradeWeapon()
        {
            Level++;
        }

        /*protected bool CheckPrefabContainer()
        {
            if (_hasContainer) return true;
            _hasContainer = _prefabEntityQuery.TryGetSingleton(out PrefabContainer);
            return _hasContainer;
        }*/
    }
}