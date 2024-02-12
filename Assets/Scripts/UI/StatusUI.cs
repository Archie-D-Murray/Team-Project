using System;
using System.Linq;

using Tags.UI.Status;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(LayoutGroup))]
    public class StatusUI : MonoBehaviour {
        [Header("Editor")]
        [SerializeField] private Health health;
        [SerializeField] private Stats stats;
        [Tooltip("Prefab to spawn - should have a TMP_Text and Image components somewhere (can be in children)")]
        [SerializeField] private GameObject statPrefab;
        [SerializeField] private StatIcon[] statIcons = new StatIcon[Enum.GetValues(typeof(StatType)).Length];

        [Header("Debug")]
        [Tooltip("Stat layout group")]
        [SerializeField] private StatSlot[] statSlots;
        [SerializeField] private VerticalLayoutGroup statLayout;
        [SerializeField] private bool isOpen;
        [Tooltip("Canvas Group for whole UI")]
        [SerializeField] private CanvasGroup canvasGroup;

        private void Start() {
            if (!health || !stats) {
                Debug.LogError("Health or Stats or Inventory were not assigned in editor!");
                Destroy(this);
                return;
            }

            SetupCanvas();
            InitStats();        
        }

        private void SetupCanvas() {
            if (!canvasGroup) {
                Debug.LogError("Canvas group not assigned");
                Destroy(this); 
                return;
            }

            canvasGroup = GetComponent<CanvasGroup>();
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

        private void InitStats() {
            statLayout = canvasGroup.GetComponent<VerticalLayoutGroup>();
            if (!statLayout) {
                Debug.LogError("Could not find VerticalLayoutGroup!");
                Destroy(this); 
                return;
            }
            statSlots = new StatSlot[stats.statDict.Count];
            foreach ((StatType type, int i) item in stats.statDict.Keys.Select((type, i) => (type, i))) {
                GameObject statSlot = Instantiate(statPrefab, statLayout.transform);
                statSlots[item.i].readout = statSlot.GetComponentInChildren<TMP_Text>();
                statSlots[item.i].readout.text = stats.GetStatDisplay(item.type);
                statSlots[item.i].icon = statSlot.GetComponentsInChildren<Image>().FirstOrDefault((Image image) => image.gameObject.HasComponent<StatusIcon>());
                statSlots[item.i].icon.sprite = Array.Find(statIcons, (StatIcon statIcon) => statIcon.type == item.type).icon;
            }
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
                statSlots[item.i].readout.text = stats.GetStatDisplay(item.type);
            }
        }
    }

    public enum StatType { Health, Speed, Damage, Magic, AttackSpeed }

    [Serializable] public class StatIcon {
        public StatType type;
        public Sprite icon;
    }

    [Serializable] public class StatSlot {
        public TMP_Text readout;
        public Image icon;
    }
}