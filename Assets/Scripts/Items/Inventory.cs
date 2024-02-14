using System;
using System.Collections.Generic;

using UnityEngine;

namespace Items {
    public class Inventory : MonoBehaviour {
        public const int MAX_ITEMS = 10;
        public Item[] items = new Item[MAX_ITEMS];

        public void RemoveItem(Item item, int count = 1) { 
            int index = Array.IndexOf(items, item);
            if (index >= 0) {
                if (items[index].count <= count) {
                    items[index].itemData = null;
                    items[index].count = 0;
                    items[index].type = ItemType.ITEM;
                } else {
                    items[index].count -= count;
                }
            }
        }
    }
}