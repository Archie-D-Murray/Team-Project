using Entity;
using Entity.Player;

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
        CanvasGroup[] tutorialCanvases;
        private int tutorialIndex = 0;
        private bool finished = false;

        private void Start() {
            canvas = GetComponent<CanvasGroup>();
            tutorialCanvases = new CanvasGroup[tutorials.Length];
            playerController = FindFirstObjectByType<PlayerController>();
            playerHealth = playerController.GetComponent<Health>();
            for (int i = 0; i < tutorialCanvases.Length; i++) {
                tutorialCanvases[i] = Instantiate(tutorialPrefab, canvas.transform).GetComponent<CanvasGroup>();
                tutorialCanvases[i].alpha = 0f;
                tutorialCanvases[i].GetComponentInChildren<TMP_Text>().text = tutorials[i].text;
            }
            if (canvas.alpha != 1f) {
                canvas.FadeAlpha(0.5f, false, this);
            }

            foreach (Tutorial tutorial in tutorials) {
                switch (tutorial.action) {
                    case ActionType.MOVEMENT_PRESS:
                        Utilities.Input.instance.playerControls.Gameplay.Move.started += (InputAction.CallbackContext context) => TryComplete(tutorial);
                        break;
                    case ActionType.DASH_PRESSED:
                        Utilities.Input.instance.playerControls.Gameplay.Dash.started += (InputAction.CallbackContext context) => TryComplete(tutorial);
                        break;
                    case ActionType.I_FRAME:
                        playerHealth.onInvulnerableDamage += () => TryComplete(tutorial);
                        break;
                    case ActionType.ATTACK_PRESSED:
                        Utilities.Input.instance.playerControls.Gameplay.Attack.started += (InputAction.CallbackContext context) => TryComplete(tutorial);
                        break;
                    case ActionType.INTERACT:
                        Utilities.Input.instance.playerControls.Gameplay.Interact.started += (InputAction.CallbackContext context) => TryComplete(tutorial);
                        break;
                    case ActionType.REWIND:
                        Utilities.Input.instance.playerControls.Gameplay.Rewind.started += (InputAction.CallbackContext context) => TryComplete(tutorial);
                        break;
                    case ActionType.EQUIP_ITEM:
                        playerController.onItemEquip += (_, _) => TryComplete(tutorial);
                        break;
                    case ActionType.INVENTORY:
                        Utilities.Input.instance.playerControls.UI.Inventory.started += (InputAction.CallbackContext context) => TryComplete(tutorial);
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
                        Utilities.Input.instance.playerControls.Gameplay.Move.started -= (InputAction.CallbackContext context) => TryComplete(tutorial);
                        break;
                    case ActionType.DASH_PRESSED:
                        Utilities.Input.instance.playerControls.Gameplay.Dash.started -= (InputAction.CallbackContext context) => TryComplete(tutorial);
                        break;
                    case ActionType.I_FRAME:
                        playerHealth.onInvulnerableDamage -= () => TryComplete(tutorial);
                        break;
                    case ActionType.ATTACK_PRESSED:
                        Utilities.Input.instance.playerControls.Gameplay.Attack.started -= (InputAction.CallbackContext context) => TryComplete(tutorial);
                        break;
                    case ActionType.INTERACT:
                        Utilities.Input.instance.playerControls.Gameplay.Interact.started -= (InputAction.CallbackContext context) => TryComplete(tutorial);
                        break;
                    case ActionType.REWIND:
                        Utilities.Input.instance.playerControls.Gameplay.Rewind.started -= (InputAction.CallbackContext context) => TryComplete(tutorial);
                        break;
                    case ActionType.EQUIP_ITEM:
                        playerController.onItemEquip -= (_, _) => TryComplete(tutorial);
                        break;
                    case ActionType.INVENTORY:
                        Utilities.Input.instance.playerControls.UI.Inventory.started -= (InputAction.CallbackContext context) => TryComplete(tutorial);
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
    }
}