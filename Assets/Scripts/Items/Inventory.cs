using System;
using System.Collections.Generic;

using UnityEngine;

using Data;

namespace Items {
    public class Inventory : MonoBehaviour, ISerialize {
        public const int MAX_ITEMS = 10;
        public Item[] items = new Item[MAX_ITEMS];
        public SpellData[] spells = new SpellData[3];
        public Action onAddItem;
        public Action onRemoveItem;

        public void RemoveItem(Item item, int count = 1) { 
            int index = Array.IndexOf(items, Array.Find(items, ((Item inventoryItem) => inventoryItem.itemData == item.itemData)));
            if (index >= 0) {
                if (items[index].count <= count) {
                    items[index].itemData = null;
                    items[index].count = 0;
                    items[index].type = ItemType.ITEM;
                } else {
                    items[index].count -= count;
                }
                onRemoveItem?.Invoke();
            }
        }

        public void AddItem(Item item, int count = 1) {
            if (item.itemData is SpellData) {
                for (int i = 0; i < spells.Length; i++) {
                    if (!spells[i]) {
                        spells[i] = item.itemData as SpellData;
                    }
                }
            } else {
                int index = Array.IndexOf(items, Array.Find(items, ((Item inventoryItem) => inventoryItem.itemData == item.itemData)));
                if (index >= 0 && index < items.Length) {
                    if (items[index].count + count < item.itemData.maxCount) {
                        items[index].count += count;
                        onAddItem?.Invoke();
                        return;
                    }
                } else {
                    for (int i = 0; i < items.Length; i++) {
                        if (!items[i].itemData) {
                            items[i] = item;
                            onAddItem?.Invoke();
                            return;
                        }
                    }
                }
            }
            Debug.LogWarning($"Could not add item {item.itemData.name} to inventory!");
        }

        public void OnSerialize(ref GameData data) {
            data.playerData.items = new List<SerializableItem>(items.Length);
            for (int i = 0; i < items.Length; i++) {
                data.playerData.items.Add(items[i].ToSerializable());
            }
            data.playerData.spells = new List<int>(spells.Length);
            foreach (SpellData spell in spells) {
                data.playerData.spells.Add(spell.id);
            }
        }

        public void OnDeserialize(GameData data) {
            for (int i = 0; i < data.playerData.items.Count; i++) {
                items[i] = data.playerData.items[i].ToItem();
            }
            spells = new SpellData[data.playerData.spells.Count]; 
            for (int i = 0; i < data.playerData.spells.Count; i++) {
                spells[i] = Array.Find(AssetServer.instance.spells, (SpellData spellData) => spellData.id == data.playerData.spells[i]);
            }
        }

        public bool CanAddItem(ItemData item, int count = 1) {
            if (item is SpellData) {
                for (int i = 0; i < spells.Length; i++) {
                    if (!spells[i]) {
                        return true;
                    }
                }
                return false;
            } else {
                int index = Array.IndexOf(items, Array.Find(items, (Item inventoryItem) => inventoryItem.itemData == item));
                if (index >= 0) {
                    return (count + items[index].count) < items[index].itemData.maxCount;
                } else {
                    for (int i = 0; i < items.Length; i++) {
                        if (!items[i].itemData) {
                            return true;
                        }
                    }
                    return false;
                }
            }
        }
    }
}