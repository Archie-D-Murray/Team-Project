using System;

using Data;

namespace Items {
    [Serializable] public class Item {
        public ItemData itemData;
        public ItemType type;
        public int count;

        public Item(ItemData itemData, ItemType type, int count) {
            this.itemData = itemData;
            this.type = type;
            this.count = count;
        }

        public SerializableItem ToSerializable() {
            if (itemData) {
                return new SerializableItem(count, type, itemData.id);
            } else {
                return SerializableItem.Null;
            }
        }
    }

    [Serializable] public enum ItemType { ITEM, CONSUMABLE, RANGED, MELEE, MAGE }
}