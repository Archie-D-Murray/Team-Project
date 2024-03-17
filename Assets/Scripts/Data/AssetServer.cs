using Items;

using Utilities;

using UnityEngine;
using Entity.Player;

namespace Data {
    public class AssetServer : Singleton<AssetServer> {
        public ItemData[] items;
        public SpellData[] spells;
        public Material flashMaterial;
        public GameObject slash;
        public Material meleeMaterial;
        public Material rangedMaterial;
        public Material mageMaterial;
        public LevelConfig meleeConfig;
        public LevelConfig rangedConfig;
        public LevelConfig mageConfig;
    }
}