using System;
using System.Collections.Generic;
using System.Linq;

using Items;

using Tags.UI.Item;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using Upgrades;

namespace UI {
    public class ItemUI : MonoBehaviour {
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private CanvasGroup canvas;
        [SerializeField] private List<Item> shownItems;
        [SerializeField] private Inventory inventory;
        [SerializeField] private ItemChest chest;
        [SerializeField] private Button takeAll;

        private void Start() {
            ItemUIManager.StartSingleton();
            canvas = GetComponent<CanvasGroup>();
            takeAll = GetComponentInChildren<Button>();
        }

        public void Show(Item[] items, Inventory playerInventory, ItemChest originChest, bool isMultiAdd) {
            if (chest) {
                return;
            }
            canvas.FadeCanvas(0.1f, false, this);
            shownItems = items.ToList();
            inventory = playerInventory;
            chest = originChest;
            inventory.onAddItem += UpdateItems;
            inventory.onRemoveItem += UpdateItems;
            ShowItems(isMultiAdd);
        }

        public void ShowItems(bool isMultiAdd) {
            foreach (Transform child in canvas.transform) {
                if (child == takeAll.transform) {
                    continue;
                }
                Destroy(child.gameObject);
            }
            foreach (Item item in shownItems) {
                GameObject itemInstance = Instantiate(itemPrefab, canvas.transform);
                itemInstance.GetComponentsInChildren<Image>().First((Image image) => image.gameObject.HasComponent<ItemIcon>()).sprite = item.itemData.icon;
                itemInstance.GetComponentsInChildren<TMP_Text>().First((TMP_Text image) => image.gameObject.HasComponent<ItemCount>()).text = item.count.ToString();
                itemInstance.GetComponentsInChildren<TMP_Text>().First((TMP_Text image) => image.gameObject.HasComponent<ItemName>()).text = item.itemData.itemName;
                Button button = itemInstance.GetComponentInChildren<Button>();
                button.interactable = inventory.CanAddItem(item.itemData, item.count);
                button.onClick.AddListener(() => AddItem(item, inventory, isMultiAdd));
                if (isMultiAdd) {
                    takeAll.onClick.AddListener(() => AddItem(item, inventory, true));
                }
            }
            takeAll.gameObject.SetActive(isMultiAdd);
            bool interactable = true;
            canvas.GetComponentsInChildren<Button>().Where((Button button) => button != takeAll).ToList().ForEach((Button button) => interactable &= button.interactable);
            takeAll.interactable = interactable;
        }

        public void UpdateItems() {
            if (canvas.alpha != 1f || shownItems == null) {
                return;
            }
            ShowItems(true);
        }

        public void AddItem(Item item, Inventory playerInventory, bool isMultiAdd) {
            playerInventory.AddItem(item, item.count);
            if (isMultiAdd) {
                chest.Remove(item);
                shownItems.Remove(item);
                if (shownItems.Count == 0) {
                    Hide();
                } else {
                    UpdateItems();
                }
            } else {
                chest.RemoveAll();
                Hide();
            }
        }

        public void Hide() {
            canvas.FadeCanvas(0.1f, true, this);
            inventory.onAddItem -= UpdateItems;
            inventory.onRemoveItem -= UpdateItems;
            inventory = null;
            shownItems = null;
            takeAll.onClick.RemoveAllListeners();
            takeAll.interactable = false;
            takeAll.gameObject.SetActive(false);
            chest.ResetCanShow();
            chest = null;
            foreach (Transform child in canvas.transform) {
                if (child == takeAll.transform) {
                    continue;
                }
                Destroy(child.gameObject);
            }
        }
    }
}