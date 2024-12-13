using System.Collections.Generic;

using Entity;
using Entity.Player;

using Items;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Tutorial {
    public class TutorialManager : MonoBehaviour {
        public Tutorial[] tutorials;
        [SerializeField] private GameObject tutorialPrefab;
        [SerializeField] private CanvasGroup canvas;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Health playerHealth;
        private Dictionary<ActionType, Tutorial> tutorialLookup;
        CanvasGroup[] tutorialCanvases;
        private int tutorialIndex = 0;
        private bool finished = false;

        private void Start() {
            canvas = GetComponent<CanvasGroup>();
            tutorialCanvases = new CanvasGroup[tutorials.Length];
            playerController = FindFirstObjectByType<PlayerController>();
            playerHealth = playerController.GetComponent<Health>();
            tutorialLookup = new Dictionary<ActionType, Tutorial>();
            for (int i = 0; i < tutorialCanvases.Length; i++) {
                tutorialCanvases[i] = Instantiate(tutorialPrefab, canvas.transform).GetComponent<CanvasGroup>();
                tutorialCanvases[i].alpha = 0f;
                tutorialCanvases[i].GetComponentInChildren<TMP_Text>().text = tutorials[i].text;
                tutorialLookup.Add(tutorials[i].action, tutorials[i]);
            }
            if (canvas.alpha != 1f) {
                canvas.FadeAlpha(0.5f, false, this);
            }

            foreach (Tutorial tutorial in tutorials) {
                switch (tutorial.action) {
                    case ActionType.MOVEMENT_PRESS:
                        Utilities.Input.instance.playerControls.Gameplay.Move.started += CompleteMovement;
                        break;
                    case ActionType.DASH_PRESSED:
                        Utilities.Input.instance.playerControls.Gameplay.Dash.started += CompleteDashPressed;
                        break;
                    case ActionType.I_FRAME:
                        playerHealth.onInvulnerableDamage += CompleteIFrame;
                        break;
                    case ActionType.ATTACK_PRESSED:
                        Utilities.Input.instance.playerControls.Gameplay.Attack.started += CompleteAttackPressed;
                        break;
                    case ActionType.INTERACT:
                        Utilities.Input.instance.playerControls.Gameplay.Interact.started += CompleteInteract;
                        break;
                    case ActionType.REWIND:
                        Utilities.Input.instance.playerControls.Gameplay.Rewind.started += CompleteRewind;
                        break;
                    case ActionType.EQUIP_ITEM:
                        playerController.onItemEquip += CompleteEquipItem;
                        break;
                    case ActionType.INVENTORY:
                        Utilities.Input.instance.playerControls.UI.Inventory.started += CompleteInventory;
                        break;
                }
            }
            tutorialCanvases[0].FadeCanvas(0.5f, false, this);
        }

        private void TryComplete(Tutorial tutorial) {
            Debug.Log("Completed");
            if (finished) {
                return;
            }
            if (tutorials[tutorialIndex] == tutorial) {
                UnsubscribeListener(tutorial);
                NextPrompt();
            }
        }

        private void UnsubscribeListener(Tutorial tutorial) {
            switch (tutorial.action) {
                case ActionType.MOVEMENT_PRESS:
                    Utilities.Input.instance.playerControls.Gameplay.Move.started -= CompleteMovement;
                    break;
                case ActionType.DASH_PRESSED:
                    Utilities.Input.instance.playerControls.Gameplay.Dash.started -= CompleteDashPressed;
                    break;
                case ActionType.I_FRAME:
                    playerHealth.onInvulnerableDamage -= CompleteIFrame;
                    break;
                case ActionType.ATTACK_PRESSED:
                    Utilities.Input.instance.playerControls.Gameplay.Attack.started -= CompleteAttackPressed;
                    break;
                case ActionType.INTERACT:
                    Utilities.Input.instance.playerControls.Gameplay.Interact.started -= CompleteInteract;
                    break;
                case ActionType.REWIND:
                    Utilities.Input.instance.playerControls.Gameplay.Rewind.started -= CompleteRewind;
                    break;
                case ActionType.EQUIP_ITEM:
                    playerController.onItemEquip -= CompleteEquipItem;
                    break;
                case ActionType.INVENTORY:
                    Utilities.Input.instance.playerControls.UI.Inventory.started -= CompleteInventory;
                    break;
            }

        }

        public void NextPrompt() {
            if (tutorialIndex >= tutorials.Length) {
                return;
            }
            tutorialCanvases[tutorialIndex].FadeAlpha(1f, true, this);
            tutorialIndex++;
            if (tutorialIndex < tutorials.Length) {
                tutorialCanvases[tutorialIndex].FadeAlpha(1f, false, this);
            } else if (tutorialIndex >= tutorials.Length) { //Player finished tutorial
                tutorialCanvases[^1].FadeAlpha(1f, true, this);
                Debug.Log("Tutorial Finished");
                finished = true;
                Destroy(gameObject, 1.5f);
            }
        }

        private void CompleteMovement(InputAction.CallbackContext context) { TryComplete(tutorialLookup[ActionType.MOVEMENT_PRESS]); }
        private void CompleteDashPressed(InputAction.CallbackContext context) { TryComplete(tutorialLookup[ActionType.DASH_PRESSED]); }
        private void CompleteIFrame() { TryComplete(tutorialLookup[ActionType.I_FRAME]); }
        private void CompleteAttackPressed(InputAction.CallbackContext context) { TryComplete(tutorialLookup[ActionType.ATTACK_PRESSED]); }
        private void CompleteInteract(InputAction.CallbackContext context) { TryComplete(tutorialLookup[ActionType.INTERACT]); }
        private void CompleteRewind(InputAction.CallbackContext context) { TryComplete(tutorialLookup[ActionType.REWIND]); }
        private void CompleteEquipItem(ItemData itemData, ItemType itemType) { TryComplete(tutorialLookup[ActionType.EQUIP_ITEM]); }
        private void CompleteInventory(InputAction.CallbackContext context) { TryComplete(tutorialLookup[ActionType.INVENTORY]); }
    }
}