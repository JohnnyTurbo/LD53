using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMG.LD53
{
    public class EnemyWaveController : MonoBehaviour
    {
        public static EnemyWaveController Instance;
        
        public List<EnemyWave> EnemyWaves;

        private int _currentWave;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            _currentWave = -1;
        }

        public EnemyWave GetNextWave()
        {
            _currentWave++;
            return EnemyWaves[_currentWave];
        }
    }
}