using UnityEngine;

namespace Items {
    [CreateAssetMenu(menuName = "Items/Bow Data")]
    public class BowData : ItemData {
        [Min(0f)] public float damageModifier = 1f;
        [Min(0.001f)] public float drawTimeModifier = 1f;
        [Min(0.001f)] public float projectileSpeed = 1f;
    }
}