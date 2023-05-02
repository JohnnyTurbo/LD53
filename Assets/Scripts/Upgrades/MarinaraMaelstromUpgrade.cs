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
            base.UpgradeWeapon();

            switch (Level)
            {
                case 1:
                    WeaponEntity = EntityManager.CreateEntity(ComponentType.ReadOnly<PerformCapabilityTag>());
                    EntityManager.SetName(WeaponEntity, "MarinaraMaelstromProperties");
                    EntityManager.AddComponentData(WeaponEntity, new CapabilityTimer
                    {
                        CooldownTime = 8f,
                        Timer = 0.15f
                    });
                    EntityManager.SetComponentEnabled<PerformCapabilityTag>(WeaponEntity, false);
                    EntityManager.AddComponentData(WeaponEntity, new MarinaraMaelstromProperties
                    {
                        BaseHitPoints = 5,
                        NumberToSpawn = 1,
                        SpreadAngle = math.radians(15f),
                    });
                    EntityManager.AddComponentData(WeaponEntity,
                        new EntityRandom { Value = Random.CreateFromIndex(100) });
                    break;
                
                case 2:
                    EntityManager.SetComponentData(WeaponEntity, new CapabilityTimer
                    {
                        CooldownTime = 7f,
                        Timer = 0.1f
                    });
                    EntityManager.SetComponentEnabled<PerformCapabilityTag>(WeaponEntity, false);
                    EntityManager.SetComponentData(WeaponEntity, new MarinaraMaelstromProperties
                    {
                        BaseHitPoints = 5,
                        NumberToSpawn = 2,
                        SpreadAngle = math.radians(15f),
                    });
                    break;
                
                case 3:
                    EntityManager.SetComponentData(WeaponEntity, new CapabilityTimer
                    {
                        CooldownTime = 6f,
                        Timer = 0.1f
                    });
                    EntityManager.SetComponentEnabled<PerformCapabilityTag>(WeaponEntity, false);
                    EntityManager.SetComponentData(WeaponEntity, new MarinaraMaelstromProperties
                    {
                        BaseHitPoints = 5,
                        NumberToSpawn = 3,
                        SpreadAngle = math.radians(15f),
                    });
                    break;
                
                case 4:
                    EntityManager.SetComponentData(WeaponEntity, new CapabilityTimer
                    {
                        CooldownTime = 5f,
                        Timer = 0.1f
                    });
                    EntityManager.SetComponentEnabled<PerformCapabilityTag>(WeaponEntity, false);
                    EntityManager.SetComponentData(WeaponEntity, new MarinaraMaelstromProperties
                    {
                        BaseHitPoints = 5,
                        NumberToSpawn = 4,
                        SpreadAngle = math.radians(15f),
                    });
                    break;
                
                default:
                    Debug.LogError($"Error undefined level: {Level}", gameObject);
                    break;
            }
        }
    }
}