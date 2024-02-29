using UnityEngine;

namespace Items {
    [CreateAssetMenu(menuName = "Items/Bow Data")]
    public class BowData : ItemData {
        [Min(0f)] public float damageModifier = 1f;
        [Min(0.001f)] public float drawTimeModifier = 1f;
        [Min(0.001f)] public float projectileSpeed = 1f;
        [Min(0.001f)] public float missileDuration = 1f;
        public int projectiles = 1;
        public float spreadAngle = 0f;
        public GameObject projectile;
        public Sprite[] frames;
    }
}