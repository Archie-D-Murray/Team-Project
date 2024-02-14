using System;
using System.Linq;

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
        private PlayerController playerController;
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
            itemIcon.sprite = item.itemData.icon.ToSprite();
            this.item = item;
            use.interactable = item.type != ItemType.ITEM;
            Show();
        }

        public void Start() {
            playerController = FindFirstObjectByType<PlayerController>();
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
            switch (item.type) {
                case ItemType.ITEM:
                    Debug.Log("Item doesn't do anything");
                    return;
                case ItemType.CONSUMABLE:
                    Debug.Log("Not implemented yet!");
                    break;
                case ItemType.RANGED:
                    Debug.Log("Set bow!");
                    playerController.SetBow(item.itemData as BowData);
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
        }

        public void Drop() {
            if (!item.itemData) {
                Debug.Log("Preview showing when no item is selected...");
                return;
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