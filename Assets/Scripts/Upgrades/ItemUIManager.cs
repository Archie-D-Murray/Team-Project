using Entity.Player;

using Items;

using UI;
using UnityEngine;

using Utilities;

namespace Upgrades {
    public class ItemUIManager : Singleton<ItemUIManager> {
        [SerializeField] private ItemUI itemUI;
        [SerializeField] private Inventory playerInventory;

        private void Start() {
            itemUI = FindFirstObjectByType<ItemUI>();
            playerInventory = FindFirstObjectByType<PlayerController>().OrNull()?.GetComponent<Inventory>() ?? null;
            if (!playerInventory || !itemUI) {
                Debug.LogError($"Could not find {(playerInventory ? "" : "playerInventory") + (itemUI ? "" : "itemUI")}!");
                Destroy(this);
            }
        }

        public void ShowItems(Item[] items, ItemChest chest, bool giveAll) {
            itemUI.Show(items, playerInventory, chest, giveAll);
        }
    }
}