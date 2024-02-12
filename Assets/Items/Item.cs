using UnityEngine;

namespace Items {
    [CreateAssetMenu(menuName = "Items/Item")]
    public class Item : ScriptableObject {
        public Texture2D icon;
        public string itemName;
        public int count = 0;
    }
}