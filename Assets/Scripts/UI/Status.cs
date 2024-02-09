using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(LayoutGroup))]
    public class Status : MonoBehaviour {
        [Header("Editor")]
        [SerializeField] private Health health;
        [SerializeField] private Stats stats;
        [Tooltip("Prefab to spawn - should have a TMP_Text component somewhere (can be in children)")]
        [SerializeField] private GameObject statPrefab;
        [Tooltip("Canvas Group must also have a layout group attached to allow stats to align correctly")]
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Debug")]
        [SerializeField] private TMP_Text[] statText;
        [SerializeField] private bool isOpen = false;

        private void Start() {
            if (!health || !stats) {
                Debug.LogError("Health or statDict were not assigned in editor!");
                Destroy(this);
                return;
            }
            statText = new TMP_Text[stats.statDict.Count];
            foreach ((StatType type, int i) item in stats.statDict.Keys.Select((type, i) => (type, i))) {
                statText[item.i] = Instantiate(statPrefab, canvasGroup.transform).GetComponentInChildren<TMP_Text>();
                statText[item.i].text = stats.GetStatDisplay(item.type);
            }
            canvasGroup.alpha = isOpen ? 1 : 0;
            canvasGroup.interactable = isOpen;
            canvasGroup.blocksRaycasts = isOpen;
            Input.instance.playerControls.UI.Status.started += (InputAction.CallbackContext context) => {
                if (isOpen) {
                    Hide(); 
                } else { 
                    Show(); 
                }
            };
        }

        public void Show() {
            UpdateStats();
            canvasGroup.FadeCanvas(0.1f, false, this);
        }

        public void Hide() {
            canvasGroup.FadeCanvas(0.1f, true, this);
        }

        private void UpdateStats() {
            // Magic iteration that gives both index and variable using a tuple
            foreach ((StatType type, int i) item in stats.statDict.Keys.Select((type, i) => (type, i))) {
                statText[item.i].text = stats.GetStatDisplay(item.type);
            }
        }
    }

    public enum StatType { Speed, Damage, Magic }

    public class Stats : MonoBehaviour {
        public Dictionary<StatType, float> statDict;

        public string GetStatDisplay(StatType type) {
            return $"{GetStatName(type)}: {statDict[type]}";
        }

        public string GetStatName(StatType type) {
            return type switch {
                StatType.Speed => "Speed",
                StatType.Damage => "Damage",
                StatType.Magic => "Magic",
                _ => string.Empty
            };
        }
    }
}