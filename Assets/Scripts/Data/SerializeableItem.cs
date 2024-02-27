using System;
using Items;

namespace Data {
    [Serializable]
    public class SerializableItem {
        public int count;
        public ItemType type;
        public int uid;

        public static readonly SerializableItem Null = new SerializableItem(-1, ItemType.ITEM, int.MinValue);

        public SerializableItem(int count, ItemType type, int uid) {
            this.count = count;
            this.type = type;
            this.uid = uid;
        }

        public Item ToItem() {
            if (uid != Null.uid) {
                return new Item(Array.Find(AssetServer.instance.items, (ItemData itemData) => itemData ? itemData.id == uid : false), type, count);
            } else {
                return new Item(null, Null.type, Null.count);
            }
        }
    }
}