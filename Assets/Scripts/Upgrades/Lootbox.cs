using UnityEngine;
using Utilities;

namespace Upgrades {
    public class LootBox : MonoBehaviour {
        [SerializeField] private bool looted = false;
        [SerializeField] private bool canShow = true;
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
            if (!looted && Utilities.Input.instance.playerControls.Gameplay.Interact.ReadValue<float>() == 1f && canShow) {
                Debug.Log("Showing Upgrade Canvas");
                UpgradeManager.instance.ShowUpgrades(upgrades, this);
                canShow = false;
            }
        }

        public void ResetCanShow() {
            canShow = true;
        }

        public void Loot() {
            looted = true;
            upgrades = null;
            GetComponent<SpriteRenderer>().FadeColour(Color.clear, 0.5f, this);
            Destroy(gameObject, 0.6f);
        }
    }
}