using System;
using System.Linq;

using Entity;
using Entity.Player;

using Items;

using Tags.UI.Item;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class ItemPreview : MonoBehaviour {
       [SerializeField] private TMP_Text count, itemName;
       [SerializeField] private Button drop, use;
       [SerializeField] private Image itemIcon;
       [SerializeField] private Item item;

        private Inventory inventory;
        private InventoryUI inventoryUI;
        private Entity.Player.PlayerController playerController;
        private Health playerHealth;
        private Mana playerMana;
        private Stats playerStats;
        private CanvasGroup canvasGroup;

        public void Init(Inventory inventory, InventoryUI inventoryUI) {
            this.inventory = inventory;
            this.inventoryUI = inventoryUI;
        }

        public void SetItem(Item item) {
            if (item == null) {
                Hide();
                return;
            }
            if (!item.itemData) {
                Hide();
                return;
            }
            count.text = item.count.ToString();
            itemName.text = item.itemData.itemName;
            itemIcon.sprite = item.itemData.icon;
            this.item = item;
            use.interactable = item.type != ItemType.ITEM;
            Show();
        }

        public void Start() {
            playerController = FindFirstObjectByType<Entity.Player.PlayerController>();
            playerHealth = playerController.GetComponent<Health>();
            playerMana = playerController.GetComponent<Mana>();
            playerStats = playerController.GetComponent<Stats>();
            itemIcon = GetComponentsInChildren<Image>().FirstOrDefault((Image image) => image.gameObject.HasComponent<ItemIcon>());
            foreach (TMP_Text text in GetComponentsInChildren<TMP_Text>()) {
                if (text.gameObject.HasComponent<ItemCount>()) {
                    count = text;
                } else if (text.gameObject.HasComponent<ItemName>()) {
                    itemName = text;
                }
            }

            foreach (Button button in GetComponentsInChildren<Button>()) {
                if (button.gameObject.HasComponent<ItemDrop>()) {
                    drop = button;
                } else if (button.gameObject.HasComponent<ItemUse>()) {
                    use = button;
                }
            }
            canvasGroup = GetComponent<CanvasGroup>();
            drop.onClick.AddListener(() => Drop());
            use.onClick.AddListener(() => Use());
        }

        public void Use() {
            bool shouldHide = false;
            switch (item.type) {
                case ItemType.ITEM:
                    Debug.Log("Item doesn't do anything");
                    return;
                case ItemType.CONSUMABLE:
                    if (item.itemData is not ConsumableData) {
                        Debug.LogError($"Item data for {item.itemData.name} is not a ConsumbleItemData instance!\n{Environment.StackTrace}");
                        break;
                    }
                    shouldHide = Consume(item.itemData as ConsumableData, playerHealth, playerMana, playerStats);
                    break;
                case ItemType.RANGED:
                    Debug.Log("Set bow!");
                    playerController.SetWeapon<BowData>(item.itemData as BowData);
                    break;
                case ItemType.MELEE:
                    Debug.Log("Not implemented yet!");
                    break;
                case ItemType.MAGE:
                    Debug.Log("Not implemented yet!");
                    break;
                default:
                    Debug.LogError("Item is no valid!");
                    break;
            }
            inventoryUI.UpdateInventory();
            if (shouldHide) {
                Hide();
            }
        }

        private bool Consume(ConsumableData data, Health playerHealth, Mana playerMana, Stats playerStats) {
            if (data.isStats) {
                playerStats.AddStatModifer(data.targetStat, data.amount, data.duration);
            } else {
                switch (data.targetStat) {
                    case StatType.HEALTH:
                        playerHealth.Heal(data.amount);
                        break;

                    case StatType.MANA:
                        playerMana.RecoverMana(data.amount);
                        break;
                    default:
                        Debug.LogWarning($"Invalid stat type has been set for {data.name} as isStat modifier is not true!");
                        break;
                }
            }
            inventory.RemoveItem(item);
            return item.count > 0;
        }

        public void Drop() {
            if (!item.itemData) {
                Debug.Log("Preview showing when no item is selected...");
                return;
            }
            if (item.type == ItemType.MELEE || item.type == ItemType.RANGED || item.type == ItemType.MAGE) {
                playerController.SetWeapon<ItemData>(null);
            }
            inventory.RemoveItem(item, item.count);
            inventoryUI.UpdateInventory();
            Hide();
        }

        public void Hide() {
            if (canvasGroup.alpha == 1f) {
                canvasGroup.FadeCanvas(0.1f, true, this);
            }
            Unset();
        }

        public void Show() {
            if (!item.itemData) {
                Hide();
            }
            if (canvasGroup.alpha == 0f) {
                canvasGroup.FadeCanvas(0.1f, false, this);
            }
        }

        public void Unset() {
            itemIcon.sprite = null;
            itemName.text = string.Empty;
            count.text = string.Empty;
        }
    }
}