using UnityEngine;

namespace TMG.LD53
{
    [CreateAssetMenu(fileName = "EnemyWave-", menuName = "ScriptableObjects/Enemy Wave", order = 1)]
    public class EnemyWave : ScriptableObject
    {
        public int WeakCount;
        public int FastCount;
        public int MediumCount;
        public int StrongCount;
    }
}