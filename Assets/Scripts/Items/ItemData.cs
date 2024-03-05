using System;

using UnityEngine;

namespace Items {
    [CreateAssetMenu(menuName = "Items/ItemData")]
    public class ItemData : ScriptableObject {
        public int id;
        public Sprite icon;
        public string itemName;
        public int maxCount = 1;

        public override bool Equals(object other) {
            return itemName == (other as ItemData).itemName;
        }

        public override int GetHashCode() {
            return HashCode.Combine(base.GetHashCode(), itemName);
        }

        public ItemType InferItemType() {
            if (this is SwordData) {
                return ItemType.MELEE;
            } else if (this is MageStaffData) {
                return ItemType.MAGE;
            } else if (this is BowData) {
                return ItemType.RANGED;
            } else if (this is ConsumableData) {
                return ItemType.CONSUMABLE;
            } else {
                return ItemType.ITEM;
            }
        }
    }
}