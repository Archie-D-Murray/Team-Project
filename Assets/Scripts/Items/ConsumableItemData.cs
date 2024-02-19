using Entity;

using UnityEngine;

namespace Items {
    [CreateAssetMenu(menuName = "Items/Consumable")]
    public class ConsumableItemData : ItemData {
        [Tooltip("Will consumable work like a potion or a stat modifier?")]
        public bool isStats;
        public StatType targetStat;
        public float amount;
        public float duration;
    }
}