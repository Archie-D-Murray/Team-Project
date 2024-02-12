using System;
using System.Linq;

using Items;

using Tags.UI.Item;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI {
    public class InventoryUI : MonoBehaviour {

        [Header("Editor")]
        [Tooltip("Inventory")]
        [SerializeField] private Inventory inventory;
        [SerializeField] private GameObject itemSlotPrefab;

        [Header("Debug")]
        [Tooltip("Canvas Group for whole UI")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private GridLayoutGroup inventoryLayout;
        [SerializeField] private bool isOpen = false;
        
        [SerializeField] private ItemSlot[] itemSlots;
        private void Start() {
            if (!inventory) {
                Debug.LogError("Health or Stats or Inventory were not assigned in editor!");
                Destroy(this);
                return;
            }
            SetupCanvas();
            InitInventory();
        }

        private void SetupCanvas() {
            canvasGroup = GetComponent<CanvasGroup>();

            if (!canvasGroup) {
                Debug.LogError("Canvas group not assigned");
                Destroy(this);
                return;
            }
            
            canvasGroup.alpha = isOpen ? 1 : 0;
            canvasGroup.interactable = isOpen;
            canvasGroup.blocksRaycasts = isOpen;
            Input.instance.playerControls.UI.Inventory.started += (InputAction.CallbackContext context) => {
                if (isOpen) {
                    Hide(); 
                } else { 
                    Show(); 
                }
            };
        }

        private void InitInventory() {
            inventoryLayout = GetComponent<GridLayoutGroup>();
            if (!inventoryLayout) {
                Debug.LogError("Could not find GridLayoutGroup!");
                Destroy(this); 
                return;
            }
            itemSlots = new ItemSlot[Inventory.MAX_ITEMS];
            for (int i = 0; i < itemSlots.Length; i++) {
                GameObject itemSlot = Instantiate(itemSlotPrefab, inventoryLayout.transform);
                itemSlots[i] = ItemSlot.FromItem(inventory.items[i], itemSlot);
            }
        }

        public void Show() {
            UpdateInventory();
            canvasGroup.FadeCanvas(0.1f, false, this);
            isOpen = true;
        }

        private void UpdateInventory() {
            for (int i = 0; i < inventory.items.Length; i++) {
                itemSlots[i].Update(inventory.items[i]);
            }
        }

        public void Hide() {
            canvasGroup.FadeCanvas(0.1f, true, this);
            isOpen = false;
        }

        [Serializable] class ItemSlot {
            public string name;
            public Sprite sprite;
            public int count;
            public bool isEmpty = false;
            public Image icon;
            public TMP_Text itemText, itemCount;

            public ItemSlot(string name, Sprite sprite, int count, Image icon, TMP_Text itemText, TMP_Text itemCount) {
                Debug.Log($"Creating item slot: {(isEmpty ? "empty" : name)}");
                this.name = name;
                this.count = count;
                this.sprite = sprite;
                this.icon = icon;
                this.itemText = itemText;
                this.itemCount = itemCount;
                isEmpty = count == -1;
                if (!isEmpty) {
                    icon.sprite = sprite;
                    itemText.text = name;
                    itemCount.text = count.ToString();
                } else {
                    icon.color = Color.clear;
                    itemText.alpha = 0f;
                    itemCount.alpha = 0f;
                }
            }

            public bool EqualTo(Item item) {
                return item.itemData.itemName == name;
            }

            public void Update(Item item) {
                if (item.itemData) {
                    if (item.itemData.itemName == name && item.count == count) {
                        return;
                    } else {
                        name = item.itemData.name;
                        count = item.count;
                        icon.sprite = item.itemData.icon.ToSprite();
                        UpdateUIElements();
                    } 
                } else {
                    icon.color = Color.clear;
                    itemText.alpha = 0f;
                    itemCount.alpha = 0f;
                }
            }

            private void UpdateUIElements() {
                itemText.text = name;
                itemCount.text = count.ToString();
            }

            public static ItemSlot Empty(Image icon, TMP_Text itemText, TMP_Text itemCount) => new ItemSlot("", null, -1, icon, itemText, itemCount);

            public static ItemSlot FromItem(Item item, GameObject gameObject) {
                Image icon = gameObject.GetComponentsInChildren<Image>().FirstOrDefault((Image image) => image.gameObject.HasComponent<ItemIcon>());
                TMP_Text name = null, count = null;
                foreach (TMP_Text text in gameObject.GetComponentsInChildren<TMP_Text>()) {
                    if (text.gameObject.HasComponent<ItemCount>()) {
                        count = text;
                    } else if (text.gameObject.HasComponent<ItemName>()) {
                        name = text;
                    }
                }
                if (!name || !count || !icon) {
                    Debug.LogError("Could not find Count or Name TMP_Text or Image Icon components on prefab!");
                    return null;
                }
                return item.itemData
                    ? new ItemSlot(item.itemData.name, item.itemData.icon.ToSprite(), item.count, icon, name, count)
                    : Empty(icon, name, count);
            }
        }
    }
}