using UnityEngine;

namespace Items {
    [CreateAssetMenu(menuName = "Items/Sword")]
    public class SwordData : ItemData {
        public float radius = 1f;
        public float damageModifier = 1f;
        public float attackSpeedModifier = 1f;
    }
}