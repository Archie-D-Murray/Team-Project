using Entity;

using UnityEngine;

namespace Upgrades {
    [CreateAssetMenu(menuName = "Upgrade")]
    public class Upgrade : ScriptableObject {
        public StatType stat;
        public float min;
        public float max;
        public Sprite icon;
        [Min(0.01f)] public float step = 1f;

        public float GetRandomStat() {
            return min + Random.Range(0f, (max - min) / step) * step;
        }
    }
}