using Items;
using UnityEditor;

using Utilities;
namespace Data {
    public class ItemServer : Singleton<ItemServer> {
        public ItemData[] items;
        #if UNITY_EDITOR
        public void Start() {
            string[] guids = AssetDatabase.FindAssets("t: ItemData", new string[] { "Assets/Scriptable Objects/Items" });
            items = new ItemData[guids.Length];
            for (int i = 0; i < guids.Length; i++) {
                items[i] = AssetDatabase.LoadAssetAtPath<ItemData>(
                    AssetDatabase.GUIDToAssetPath(guids[i])
                );
            }
        }
        #endif
    }
}