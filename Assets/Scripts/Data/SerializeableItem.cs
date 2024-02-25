using System;
using Items;

namespace Data {
    [Serializable]
    public class SerializeableItem {
        public int count;
        public ItemType type;
        public int uid;

        public static readonly SerializeableItem Null = new SerializeableItem(-1, ItemType.ITEM, int.MinValue);

        public SerializeableItem(int count, ItemType type, int uid) {
            this.count = count;
            this.type = type;
            this.uid = uid;
        }

        public Item ToItem() {
            if (uid != Null.uid) {
                return new Item(Array.Find(ItemServer.instance.items, (ItemData itemData) => itemData ? itemData.id == uid : false), type, count);
            } else {
                return new Item(null, Null.type, Null.count);
            }
        }
    }
}