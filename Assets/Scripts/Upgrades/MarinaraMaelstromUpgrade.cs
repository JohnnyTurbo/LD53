using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace TMG.LD53
{
    public class MarinaraMaelstromUpgrade : UpgradeListener
    {
        public override void UpgradeWeapon()
        {
            //if (!CheckPrefabContainer()) return;
            base.UpgradeWeapon();

            switch (Level)
            {
                case 1:
                    WeaponEntity = EntityManager.CreateEntity(ComponentType.ReadOnly<PerformCapabilityTag>());
                    EntityManager.SetName(WeaponEntity, "SausageSlingshotProperties");
                    EntityManager.AddComponentData(WeaponEntity, new CapabilityTimer
                    {
                        CooldownTime = 2.5f,
                        Timer = 0.15f
                    });
                    EntityManager.SetComponentEnabled<PerformCapabilityTag>(WeaponEntity, false);
                    EntityManager.AddComponentData(WeaponEntity, new MarinaraMaelstromProperties
                    {
                        BaseHitPoints = 5,
                        NumberToSpawn = 3,
                        SpreadAngle = math.radians(15f),
                    });
                    EntityManager.AddComponentData(WeaponEntity,
                        new EntityRandom { Value = Random.CreateFromIndex(100) });
                    break;
                
                case 2:

                    break;
                
                case 3:

                    break;
                
                case 4:

                    break;
                
                default:
                    Debug.LogError($"Error undefined level: {Level}", gameObject);
                    break;
            }
        }
    }
}