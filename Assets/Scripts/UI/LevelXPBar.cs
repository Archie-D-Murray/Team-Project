using Entity.Player;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using System.Linq;
using Tags.UI.Level;

namespace UI {
    public class LevelXPBar : MonoBehaviour {
        [Header("Editor")]
        [SerializeField] private Level level;
        [SerializeField] private float lerpSpeed = 0.05f;
        
        [Header("Debug")]
        [SerializeField] private float currentProgress;
        [SerializeField] private TMP_Text levelReadout;
        [SerializeField] private Image bar;

        private void Start() {
            bar = GetComponent<Image>();
            levelReadout = GetComponentInChildren<TMP_Text>();
            level.onEarnXP += (float _) => UpdateReadouts();
            level.onLevelUp += (int _) => UpdateReadouts();
            UpdateReadouts();
        }

        private void FixedUpdate() {
            currentProgress = Mathf.MoveTowards(currentProgress, level.xpProgress, Time.fixedDeltaTime * lerpSpeed);
            bar.fillAmount = currentProgress;
        }

        private void UpdateReadouts() {
            levelReadout.text = $"LVL: {level.currentLevel}";
        }
    }
}