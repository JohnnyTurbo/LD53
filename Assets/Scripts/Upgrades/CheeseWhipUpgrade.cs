using Unity.Entities;
using UnityEngine;

namespace TMG.LD53
{
    public class CheeseWhipUpgrade : UpgradeListener
    {
        public override void UpgradeWeapon()
        {
            //if (!CheckPrefabContainer()) return;
            base.UpgradeWeapon();
            
            switch (Level)
            {
                case 1:
                    WeaponEntity = EntityManager.CreateEntity(ComponentType.ReadOnly<PerformCapabilityTag>());
                    EntityManager.SetName(WeaponEntity, "CheeseWhipProperties");
                    EntityManager.AddComponentData(WeaponEntity, new CapabilityTimer
                    {
                        CooldownTime = 2.5f,
                        Timer = 0.1f
                    });
                    EntityManager.SetComponentEnabled<PerformCapabilityTag>(WeaponEntity, false);
                    EntityManager.AddComponentData(WeaponEntity, new CheeseWhipProperties
                    {
                        BaseHitPoints = 5,
                        NumberToSpawn = 1,
                        TimeBetweenSpawns = 0.2f,
                        NumberSpawned = 0,
                        Timer = 0f
                    });
                    break;
                
                case 2:
                    EntityManager.SetComponentData(WeaponEntity, new CapabilityTimer
                    {
                        CooldownTime = 2f,
                        Timer = 0.1f
                    });
                    EntityManager.SetComponentEnabled<PerformCapabilityTag>(WeaponEntity, false);
                    EntityManager.SetComponentData(WeaponEntity, new CheeseWhipProperties
                    {
                        BaseHitPoints = 10,
                        NumberToSpawn = 2,
                        TimeBetweenSpawns = 0.2f,
                        NumberSpawned = 0,
                        Timer = 0f
                    });
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