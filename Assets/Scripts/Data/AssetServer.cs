using Items;

using Utilities;

namespace Data {
    public class AssetServer : Singleton<AssetServer> {
        public ItemData[] items;
        public SpellData[] spells;
        public UnityEngine.Material flashMaterial;
    }
}