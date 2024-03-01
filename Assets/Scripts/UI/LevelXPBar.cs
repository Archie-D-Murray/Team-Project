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
        [SerializeField] private TMP_Text xpReadout;
        [SerializeField] private Image bar;

        private void Start() {
            bar = GetComponentsInChildren<Image>().FirstOrDefault((Image image) => image.gameObject.HasComponent<BarImage>());
            foreach (TMP_Text text in GetComponentsInChildren<TMP_Text>()) {
                if (text.gameObject.HasComponent<XPReadout>()) {
                    xpReadout = text;
                } else if (text.gameObject.HasComponent<LevelReadout>()) {
                    levelReadout = text;
                }
            }
            level.onEarnXP += (float _) => UpdateReadouts();
            level.onLevelUp += (int _) => UpdateReadouts();
            UpdateReadouts();
        }

        private void FixedUpdate() {
            currentProgress = Mathf.MoveTowards(currentProgress, level.xpProgress, Time.fixedDeltaTime * lerpSpeed);
            bar.fillAmount = currentProgress;
        }

        private void UpdateReadouts() {
            xpReadout.text = $"XP: {level.currentXP} / {level.levelXP} ({level.xpProgress:0%})";
            levelReadout.text = $"Level: {level.currentLevel} / {level.maxLevel} ({level.levelProgress:0%})";
        }
    }
}