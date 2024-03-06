using UnityEngine;

namespace Upgrades {
    public class LootBox : MonoBehaviour {
        [SerializeField] private bool looted = false;
        [SerializeField] private Upgrade[] upgrades;

        private void Start() {
            UpgradeManager.StartSingleton();
            if (upgrades == null) {
                Debug.LogWarning($"Lootbox {name} was not assigned any upgrades!");
                Destroy(this);
            }
        }

        private void OnTriggerStay2D(Collider2D collider) {
            if (!collider.gameObject.HasComponent<Entity.Player.PlayerController>()) {
                return;
            }
            if (!looted && Utilities.Input.instance.playerControls.Gameplay.Interact.ReadValue<float>() == 1f) {
                Debug.Log("Showing Upgrade Canvas");
                UpgradeManager.instance.ShowUpgrades(upgrades);
                looted = true;
            }
        }
    }
}