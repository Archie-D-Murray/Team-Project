using System.Linq;

using Entity;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class ManaBar : MonoBehaviour {
        [Header("Debug")]
        [SerializeField] private Image manaBar;
        [SerializeField] private TMP_Text manaText;
        [SerializeField] private float currentMana;

        [Header("Editor")]
        [SerializeField] private float lerpSpeed = 5f;
        [SerializeField] private Mana mana;

        private void Start() {
            manaBar = GetComponentsInChildren<Image>().FirstOrDefault((Image image) => image.type == Image.Type.Filled);
            manaText = GetComponentInChildren<TMP_Text>();
            if (!manaBar || !manaText) {
                Debug.LogError($"Could not find {(manaBar ? "manaBar" : "manaText")} in children");
                Destroy(this);
                return;
            }
            if (!mana) {
                Debug.LogError("Mana was not assigned in the editor!");
                Destroy(this);
                return;
            }
            currentMana = mana.getPercentMana;
            manaText.text = $"{mana.getCurrentMana} / {mana.getMaxMana} ({mana.getPercentMana:0%})";
            mana.onManaUse += OnManaUse;
        }

        private void FixedUpdate() {
            currentMana = Mathf.MoveTowards(currentMana, mana.getPercentMana, lerpSpeed * Time.fixedDeltaTime);
            manaBar.fillAmount = currentMana;
        }

        private void OnManaUse(float damage) {
            manaText.text = $"{mana.getCurrentMana} / {mana.getMaxMana} ({mana.getPercentMana:0%})";
        }
    }
}