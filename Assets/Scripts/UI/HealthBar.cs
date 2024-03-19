using System.Linq;

using Entity;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class HealthBar : MonoBehaviour {
        [Header("Debug")]
        [SerializeField] private Image healthBar;
        [SerializeField] private float currentHealth;

        [Header("Editor")]
        [SerializeField] private float lerpSpeed = 5f;
        [SerializeField] private Health health;

        private void Start() {
            healthBar = GetComponent<Image>();
            if (!healthBar) {
                Debug.LogError($"Could not find healthBar");
                Destroy(this);
                return;
            }
            if (!health) {
                Debug.LogError("HEALTH was not assigned in the editor!");
                Destroy(this);
                return;
            }
            currentHealth = health.getPercentHealth;
        }

        private void FixedUpdate() {
            currentHealth = Mathf.MoveTowards(currentHealth, health.getPercentHealth, lerpSpeed * Time.fixedDeltaTime);
            healthBar.fillAmount = currentHealth;
        }
    }
}