using System;

using UnityEngine;

namespace Items {
    [CreateAssetMenu(menuName = "Items/ItemData")]
    public class ItemData : ScriptableObject {
        public int id;
        public Texture2D icon;
        public string itemName;
        public int maxCount = 1;

        public override bool Equals(object other) {
            return itemName == (other as ItemData).itemName;
        }

        public override int GetHashCode() {
            return HashCode.Combine(base.GetHashCode(), itemName);
        }
    }
}