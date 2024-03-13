using UnityEngine;
using UI;
using Utilities;
using Entity;

namespace Upgrades {
    public class UpgradeManager : Singleton<UpgradeManager> {
        [SerializeField] private UpgradeUI upgradeUI;
        [SerializeField] private Stats playerStats;

        private void Start() {
            upgradeUI = FindFirstObjectByType<UpgradeUI>();
            playerStats = FindFirstObjectByType<Entity.Player.PlayerController>().OrNull()?.GetComponent<Stats>() ?? null;
            if (!playerStats || !upgradeUI) {
                Debug.LogError($"Could not find {(playerStats ? "" : "playerStats") + (upgradeUI ? "" : "upgradeUI")}!");
                Destroy(this);
            }
        }

        public void ShowUpgrades(Upgrade[] upgrades, LootBox lootBox) {
            upgradeUI.Show(upgrades, playerStats, lootBox);
        }
    }
}