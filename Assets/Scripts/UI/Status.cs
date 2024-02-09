using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.InputSystem;

namespace UI {
    public class Status : MonoBehaviour {
        [Header("Editor")]
        [SerializeField] private Health health;
        [SerializeField] private Stats stats;
        [SerializeField] private GameObject statPrefab;
        [SerializeField] private CanvasGroup canvasGroup;
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