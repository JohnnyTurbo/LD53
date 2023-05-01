using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TMG.LD53
{
    public class DamageUIController : MonoBehaviour
    {
        [SerializeField] private GameObject _damageIconPrefab;
        [SerializeField] private Transform _worldSpaceUICanvas;
        [SerializeField] private Vector3 _offset;
        
        private void OnEnable()
        {
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<DisplayDamageIconSystem>().OnDamage +=
                DisplayDamageIcon;
        }

        private void DisplayDamageIcon(float damageAmount, float3 entityPosition)
        {
            var newIcon = Instantiate(_damageIconPrefab, (Vector3)entityPosition + Vector3.up, Quaternion.identity, _worldSpaceUICanvas);
            var newIconText = newIcon.GetComponent<TextMeshProUGUI>();
            newIconText.text = damageAmount.ToString();
            var newPosition = (Vector3)entityPosition + _offset + Vector3.up;
            var uiController = newIcon.GetComponent<MoveDamageIconText>();
            uiController.EndPosition = newPosition;
        }

        private void OnDisable()
        {
            if (World.DefaultGameObjectInjectionWorld == null) return;
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<DisplayDamageIconSystem>().OnDamage -=
                DisplayDamageIcon;
        }
    }
}