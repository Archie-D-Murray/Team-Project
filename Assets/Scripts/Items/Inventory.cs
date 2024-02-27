using System;
using System.Collections.Generic;

using UnityEngine;

using Data;

namespace Items {
    public class Inventory : MonoBehaviour, ISerialize {
        public const int MAX_ITEMS = 10;
        public Item[] items = new Item[MAX_ITEMS];
        public SpellData[] spells = new SpellData[1];

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

        public void AddItem(Item item, int count = 1) {
            int index = Array.IndexOf(items, item);
            if (index >= 0) {
                if (item.count + count < item.itemData.maxCount) {
                    item.count += count;
                    return;
                }
            } else {
                for (int i = 0; i < count; i++) {
                    if (!items[i].itemData) {
                        items[i] = item;
                        return;
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
                spells[i] = Array.Find(ItemServer.instance.spells, (SpellData spellData) => spellData.id == data.playerData.spells[i]);
            }
        }
    }
}