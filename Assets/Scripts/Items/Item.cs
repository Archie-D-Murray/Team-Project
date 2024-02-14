using System;

namespace Items {
    [Serializable] public class Item {
        public ItemData itemData;
        public ItemType type;
        public int count;
    }

    [Serializable] public enum ItemType { ITEM, CONSUMABLE, RANGED, MELEE, MAGE }
}