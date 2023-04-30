using System.Collections.Generic;
using UnityEngine;

namespace TMG.LD53
{
    [CreateAssetMenu(fileName = "UpgradeElement", menuName = "ScriptableObjects/Upgrade Element", order = 0)]
    public class UpgradeElement : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
        public string[] Descriptions;

        public int Level => _listener == null ? 0 : _listener.Level;

        private UpgradeListener _listener;
        
        public void InvokeUpgrade()
        {
            if (_listener == null) return;
            _listener.UpgradeWeapon();
        }

        public void Register(UpgradeListener upgradeListener) => _listener = upgradeListener;
        public void Deregister(UpgradeListener upgradeListener) => _listener = null;
    }
}