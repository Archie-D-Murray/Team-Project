using System.Linq;

using Entity;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class ManaBar : MonoBehaviour {
        [Header("Debug")]
        [SerializeField] private Image manaBar;
        [SerializeField] private float currentMana;

        [Header("Editor")]
        [SerializeField] private float lerpSpeed = 5f;
        [SerializeField] private Mana mana;

        private void Start() {
            manaBar = GetComponent<Image>();
            if (!manaBar) {
                Debug.LogError($"Could not find manaBar");
                Destroy(this);
                return;
            }
            if (!mana) {
                Debug.LogError("Mana was not assigned in the editor!");
                Destroy(this);
                return;
            }
            currentMana = mana.getPercentMana;
        }

        private void FixedUpdate() {
            currentMana = Mathf.MoveTowards(currentMana, mana.getPercentMana, lerpSpeed * Time.fixedDeltaTime);
            manaBar.fillAmount = currentMana;
        }
    }
}