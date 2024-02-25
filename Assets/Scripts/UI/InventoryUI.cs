using System;
using System.Linq;

using Items;

using Tags.UI.Item;

using TMPro;

using Utilities;

using UnityEditor.Rendering;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI {
    public class InventoryUI : MonoBehaviour {

        [Header("Editor")]
        [Tooltip("Inventory")]
        [SerializeField] private Inventory inventory;
        [SerializeField] private GameObject itemSlotPrefab;
        [SerializeField] private bool hideOnStart = true;

        [Header("Debug")]
        [Tooltip("Canvas Group for whole UI")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private ItemPreview preview;
        [SerializeField] private GridLayoutGroup inventoryLayout;
        private bool isOpen => canvasGroup.alpha == 1f;

        
        [SerializeField] private ItemSlot[] itemSlots;
        private void Start() {
            if (!inventory) {
                Debug.LogError("HEALTH or Stats or Inventory were not assigned in editor!");
                Destroy(this);
                return;
            }
            SetupCanvas();
            InitInventory();
        }

        private void SetupCanvas() {
            canvasGroup = GetComponent<CanvasGroup>();
            preview = GetComponentInChildren<ItemPreview>();
            preview.Init(inventory, this);

            if (!canvasGroup || !preview) {
                Debug.LogError("Canvas group not assigned");
                Destroy(this);
                return;
            }
            
            canvasGroup.alpha = hideOnStart ? 0f : 1f;
            canvasGroup.interactable = !hideOnStart;
            canvasGroup.blocksRaycasts = !hideOnStart;
            Utilities.Input.instance.playerControls.UI.Inventory.started += (InputAction.CallbackContext context) => {
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
                itemSlots[i] = ItemSlot.FromItem(inventory.items[i], itemSlot, preview);
            }
        }

        public void Show() {
            UpdateInventory();
            canvasGroup.FadeCanvas(0.1f, false, this);
        }

        public void UpdateInventory() {
            for (int i = 0; i < inventory.items.Length; i++) {
                itemSlots[i].Update(inventory.items[i], preview);
            }
            preview.Hide();
        }

        public void Hide() {
            canvasGroup.FadeCanvas(0.1f, true, this);
        }

        [Serializable] class ItemSlot {
            public string name;
            public Sprite sprite;
            public int count;
            public bool isEmpty = false;
            public Image icon;
            public TMP_Text itemText, itemCount;
            public Button button;

            public ItemSlot(string name, Sprite sprite, int count, Image icon, TMP_Text itemText, TMP_Text itemCount, Button button) {
                this.name = name;
                this.count = count;
                this.sprite = sprite;
                this.icon = icon;
                this.itemText = itemText;
                this.itemCount = itemCount;
                this.button = button;
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

            public void Update(Item item, ItemPreview preview) {
                if (!item.itemData) {
                    icon.color = Color.clear;
                    itemText.alpha = 0f;
                    itemCount.alpha = 0f;
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => preview.SetItem(null));
                    return;
                }
                if (!(item.itemData.itemName == name && item.count == count)) {
                    name = item.itemData.name;
                    count = item.count;
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => preview.SetItem(item));
                    icon.sprite = item.itemData.icon.ToSprite();
                    UpdateUIElements();
                }
            }

            private void UpdateUIElements() {
                itemText.text = name;
                itemCount.text = count.ToString();
            }

            public static ItemSlot Empty(Image icon, TMP_Text itemText, TMP_Text itemCount, Button button) => new ItemSlot("", null, -1, icon, itemText, itemCount, button);

            public static ItemSlot FromItem(Item item, GameObject gameObject, ItemPreview preview) {
                Image icon = gameObject.GetComponentsInChildren<Image>().FirstOrDefault((Image image) => image.gameObject.HasComponent<ItemIcon>());
                TMP_Text name = null, count = null;
                foreach (TMP_Text text in gameObject.GetComponentsInChildren<TMP_Text>()) {
                    if (text.gameObject.HasComponent<ItemCount>()) {
                        count = text;
                    } else if (text.gameObject.HasComponent<ItemName>()) {
                        name = text;
                    }
                }
                Button button = gameObject.GetComponentInChildren<Button>();
                if (!name || !count || !icon || !button) {
                    Debug.LogError("Could not find Count or Name TMP_Text or Image Icon or Button components on prefab!");
                    return null;
                }
                if (item.itemData) {
                    button.onClick.AddListener(() => preview.SetItem(item));
                }
                return item.itemData
                    ? new ItemSlot(item.itemData.name, item.itemData.icon.ToSprite(), item.count, icon, name, count, button)
                    : Empty(icon, name, count, button);
            }
        }
    }
}