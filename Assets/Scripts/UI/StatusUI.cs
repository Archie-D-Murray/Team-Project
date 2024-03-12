using System;
using System.Linq;

using Entity;
using Entity.Player;

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
        [SerializeField] private Level level;
        [Tooltip("Prefab to spawn - should have a TMP_Text and Image components somewhere (can be in children)")]
        [SerializeField] private GameObject statPrefab;
        [SerializeField] private StatIcon[] statIcons = new StatIcon[Enum.GetValues(typeof(StatType)).Length];
        [SerializeField] private bool hideOnStart = true;

        [Header("Debug")]
        [Tooltip("Stat layout group")]
        [SerializeField] private StatSlot[] statSlots;
        [SerializeField] private VerticalLayoutGroup statLayout;
        [Tooltip("Canvas Group for whole UI")]
        [SerializeField] private CanvasGroup canvasGroup;
        private bool isOpen => canvasGroup.alpha == 1f;

        private void Start() {
            if (!health || !stats || !level) {
                Debug.LogError("Health or Stats or Level were not assigned in editor!");
                Destroy(this);
                return;
            }

            SetupCanvas();
            InitStats();        

            level.onLevelUp += (int _) => UpdateStats();
        }

        private void SetupCanvas() {
            canvasGroup = GetComponent<CanvasGroup>();

            if (!canvasGroup) {
                Debug.LogError("Canvas group not assigned");
                Destroy(this); 
                return;
            }
            canvasGroup.alpha = hideOnStart ? 0f : 1f;
            canvasGroup.interactable = !hideOnStart;
            canvasGroup.blocksRaycasts = !hideOnStart;
            Utilities.Input.instance.playerControls.UI.Status.started += (InputAction.CallbackContext context) => {
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
            statSlots = new StatSlot[stats.statDict.Length];
            foreach ((StatType type, int i) item in stats.statDict.Select((stat, i) => (stat.type, i))) {
                GameObject statSlot = Instantiate(statPrefab, statLayout.transform);
                statSlots[item.i] = new StatSlot();
                statSlots[item.i].readout = statSlot.GetComponentsInChildren<TMP_Text>().First((TMP_Text text) => !text.transform.parent.gameObject.HasComponent<Button>());
                statSlots[item.i].readout.text = stats.GetStatDisplay(item.type);
                statSlots[item.i].icon = statSlot.GetComponentsInChildren<Image>().FirstOrDefault((Image image) => image.gameObject.HasComponent<StatusIcon>());
                statSlots[item.i].icon.sprite = Array.Find(statIcons, (StatIcon statIcon) => statIcon.type == item.type).icon;
                statSlots[item.i].level = statSlot.GetComponentInChildren<Button>();
                statSlots[item.i].level.onClick.AddListener(() => LevelUpClick(item.type));
                statSlots[item.i].level.interactable = false;
                statSlots[item.i].increase = statSlots[item.i].level.GetComponentInChildren<TMP_Text>();
                statSlots[item.i].increase.text = string.Empty;
            }
        }

        public void LevelUpClick(StatType type) {
            level.LevelUpStat(type);
            UpdateStats();
        }

        public void Show() {
            UpdateStats();
            if (!isOpen) {
                canvasGroup.FadeCanvas(0.1f, false, this);
            }
        }

        public void Hide() {
            if (isOpen) {
                canvasGroup.FadeCanvas(0.1f, true, this);
            }
        }

        private void UpdateStats() {
            // Magic iteration that gives both index and variable using a tuple
            foreach ((StatType type, int i) item in stats.statDict.Select((stat, i) => (stat.type, i))) {
                statSlots[item.i].readout.text = stats.GetStatDisplay(item.type);
                if (level.unappliedLevels > 0) {
                    statSlots[item.i].level.interactable = true;
                    statSlots[item.i].increase.text = $"+{level.GetStatIncrease(item.type)}";
                }
            }
        }
    }
}