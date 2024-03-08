using System;
using System.Collections.Generic;

using Entity.Player;

using Items;

using UnityEngine;

using Utilities;

namespace Upgrades {
    public class ItemChest : MonoBehaviour {
        [SerializeField] private bool giveAll = true;
        [SerializeField] private List<Item> items;
        [SerializeField] private bool looted = false;
        [SerializeField] private bool canShow = true;
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void ResetCanShow() {
            canShow = true;
        }

        public void Remove(Item item) {
            items.Remove(item);
            if (items.Count == 0) {
                looted = true;
                if (spriteRenderer.color.a == 1f) {
                    spriteRenderer.FadeColour(Color.clear, 0.5f, this);
                    Destroy(gameObject);
                }
            }
        }

        public void RemoveAll() {
            items.Clear();
            looted = true;
            if (spriteRenderer.color.a == 1f) {
                spriteRenderer.FadeColour(Color.clear, 0.5f, this);
                Destroy(gameObject);
            }
        }

        private void OnTriggerStay2D(Collider2D collider) {
            if (!collider.gameObject.HasComponent<PlayerController>() || looted || !canShow) {
                return;
            }
            if (collider.TryGetComponent(out Inventory inventory) && Utilities.Input.instance.playerControls.Gameplay.Interact.ReadValue<float>() == 1f) {
                Debug.Log("Opening item UI");
                ItemUIManager.instance.ShowItems(giveAll ? items.ToArray() : new Item[1] { items[UnityEngine.Random.Range(0, items.Count)] }, this, giveAll);
                canShow = false;
            }
        }
    }
}