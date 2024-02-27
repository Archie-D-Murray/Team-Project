using Items;

using Utilities;
namespace Data {
    public class ItemServer : Singleton<ItemServer> {
        public ItemData[] items;
        public SpellData[] spells;
    }
}