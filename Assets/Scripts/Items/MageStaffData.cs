using UnityEngine;

namespace Items {
    [CreateAssetMenu(menuName = "Items/Mage Staff")]
    public class MageStaffData : ItemData {
        public float damageAmplifier = 1f;
        public float cooldownModifier = 1f;
        public Sprite sprite;
    }
}