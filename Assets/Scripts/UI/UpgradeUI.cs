using System;

using Entity;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using Upgrades;

namespace UI {
    public class UpgradeUI : MonoBehaviour {
        [Header("Debug")]
        [SerializeField] private CanvasGroup upgradeCanvas;
        [SerializeField] private LootBox lootBox;

        [Header("Editor")]
        [SerializeField] private GameObject upgradePrefab;

        private void Start() {
            upgradeCanvas = GetComponent<CanvasGroup>();
            if (!upgradeCanvas) {
                Debug.LogError($"Could not find CanvasGroup on {name} for Upgrade UI!");
                Destroy(this);
            }
        }

        public void EscapeMenu(InputAction.CallbackContext _) {
            Utilities.Input.instance.playerControls.UI.Cancel.started -= EscapeMenu;
            lootBox.ResetCanShow();
            upgradeCanvas.FadeCanvas(0.1f, true, this);
        }

        public void Show(Upgrade[] upgrades, Stats stats, LootBox origin) {
            Utilities.Input.instance.playerControls.UI.Cancel.started += EscapeMenu;
            if (!lootBox || lootBox != origin) {
                if (lootBox) {
                    lootBox.ResetCanShow();
                }
                lootBox = origin;
                foreach (Upgrade upgrade in upgrades) {
                    float statAmount = upgrade.GetRandomStat();
                    GameObject prefabInstance = Instantiate(upgradePrefab, upgradeCanvas.transform);
                    Array.Find(prefabInstance.GetComponentsInChildren<Image>(), (Image image) => !image.raycastTarget).sprite = upgrade.icon;
                    prefabInstance.GetComponentInChildren<Button>().onClick.AddListener(() => stats.IncrementStat(upgrade.stat, statAmount));
                    prefabInstance.GetComponentInChildren<Button>().onClick.AddListener(() => origin.Loot());
                    prefabInstance.GetComponentInChildren<Button>().onClick.AddListener(() => Hide());
                    Array.Find(prefabInstance.GetComponentsInChildren<TMP_Text>(), (TMP_Text text) => !text.transform.parent.gameObject.HasComponent<Button>()).text = $"{upgrade.stat}:\n+{statAmount:0.0}";
                }
            }
            upgradeCanvas.FadeCanvas(0.1f, false, this);
        }

        public void Hide() {
            foreach (Transform child in upgradeCanvas.transform) {
                Destroy(child.gameObject);
            }
            upgradeCanvas.FadeCanvas(0.1f, true, this);
            Utilities.Input.instance.playerControls.UI.Cancel.started -= EscapeMenu;
        }
    }
}