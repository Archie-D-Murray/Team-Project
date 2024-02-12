using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class Healthbar : MonoBehaviour {
        [Header("Debug")]
        [SerializeField] private Image healthBar;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private float currentHealth;

        [Header("Editor")]
        [SerializeField] private float lerpSpeed = 5f;
        [SerializeField] private Health health;

        private void Start() {
            healthBar = GetComponentsInChildren<Image>().FirstOrDefault((Image image) => image.type == Image.Type.Filled);
            healthText = healthBar.GetComponentInChildren<TMP_Text>();
            if (!healthBar || !healthText) {
                Debug.LogError("Could not find filled healthBar in children to use as health bar!");
                Destroy(this);
                return;
            }
            if (health == null) {
                Debug.LogError("Health was not assigned in the editor!");
                Destroy(this);
                return;
            }
            currentHealth = health.getPercentHealth;
            health.onDamage += OnDamage;
        }

        private void FixedUpdate() {
            currentHealth = Mathf.MoveTowards(currentHealth, health.getPercentHealth, lerpSpeed * Time.fixedDeltaTime);
            healthBar.fillAmount = currentHealth;
        }

        private void OnDamage(float damage) {
            healthText.text = $"{health.getCurrentHealth} / {health.getMaxHealth} ({health.getPercentHealth:0%})";
        }
    }
}