using System;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Tutorial {
    [Serializable] public enum ActionType { MOVEMENT_PRESS, DASH_PRESSED, ATTACK_PRESSED }

    [CreateAssetMenu(menuName = "Tutorial")]
    public class Tutorial : ScriptableObject {
        public ActionType action;
        public string text;
    }

    public class TutorialManager : MonoBehaviour {
        public Tutorial[] tutorials;
        [SerializeField] private GameObject tutorialPrefab;
        [SerializeField] private Transform canvas;
        CanvasGroup[] tutorialCanvases;
        private int tutorialIndex = 0;

        private void Start() {
            canvas = transform;
            tutorialCanvases = new CanvasGroup[tutorials.Length];
            for (int i = 0; i < tutorialCanvases.Length; i++) {
                tutorialCanvases[i] = Instantiate(tutorialPrefab, canvas).GetComponent<CanvasGroup>();
                tutorialCanvases[i].alpha = 0f;
                tutorialCanvases[i].GetComponentInChildren<TMP_Text>().text = tutorials[i].text;
            }

            foreach (Tutorial tutorial in tutorials) {
                switch (tutorial.action) {
                    case ActionType.MOVEMENT_PRESS:
                        Utilities.Input.instance.playerControls.Gameplay.Move.started += (InputAction.CallbackContext context) => TryComplete(tutorial);
                        break;

                    case ActionType.DASH_PRESSED:
                        // Utilities.Input.instance.playerControls.Gameplay.Dash.started += (InputAction.CallbackContext context) => TryComplete(tutorial);
                        break;

                    case ActionType.ATTACK_PRESSED:
                        Utilities.Input.instance.playerControls.Gameplay.Attack.started += (InputAction.CallbackContext context) => TryComplete(tutorial);
                        break;
                }
            }
            tutorialCanvases[0].FadeCanvas(1f, false, this);
        }

        private void TryComplete(Tutorial tutorial) {
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
                    // Utilities.Input.instance.playerControls.Gameplay.Dash.started -= (InputAction.CallbackContext context) => TryComplete(tutorial);
                    break;
            }
        }

        public void NextPrompt() {
            if (tutorialIndex >= tutorials.Length) {
                return;
            }
            tutorialCanvases[tutorialIndex].FadeCanvas(1f, true, this);
            tutorialIndex++;
            if (tutorialIndex < tutorials.Length) {
                tutorialCanvases[tutorialIndex].FadeCanvas(1f, false, this);
            } else if (tutorialIndex >= tutorials.Length) { //Player finished tutorial
                tutorialCanvases[^1].FadeCanvas(1f, true, this);
                Debug.Log("Tutorial Finished");
                Destroy(gameObject, 1.5f);
            }
        }
    }
}