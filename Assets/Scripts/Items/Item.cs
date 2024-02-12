using System;

using UnityEngine;

namespace Items {
    [CreateAssetMenu(menuName = "Items/Item")]
    public class Item : ScriptableObject {
        public Texture2D icon;
        public string itemName;
        public int count = 0;

        public override bool Equals(object other) {
            return itemName == (other as Item).itemName;
        }

        public override int GetHashCode() {
            return HashCode.Combine(base.GetHashCode(), itemName);
        }
    }
}