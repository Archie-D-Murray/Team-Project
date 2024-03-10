using Items;

using Utilities;

using UnityEngine;

namespace Data {
    public class AssetServer : Singleton<AssetServer> {
        public ItemData[] items;
        public SpellData[] spells;
        public Material flashMaterial;
        public GameObject slash;
        public Material meleeMaterial;
        public Material rangedMaterial;
        public Material mageMaterial;
    }
}