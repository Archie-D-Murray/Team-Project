using System;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class HealthBar : MonoBehaviour {
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
    public class Health : MonoBehaviour {
        public float getPercentHealth => Mathf.Clamp01(currentHealth / maxHealth);
        public float getCurrentHealth => currentHealth;
        public float getMaxHealth => maxHealth;

        public Action<float> onDamage;
        public Action onDeath;

        private float currentHealth;
        private float maxHealth;

        public void Damage(float damage) {
            if (currentHealth == 0.0f) { //Don't damage dead things!
                return;
            }
            damage = Mathf.Max(damage, 0.0f);
            if (damage != 0.0f) {
                currentHealth = Mathf.Max(0.0f, currentHealth - damage);
                onDamage?.Invoke(damage);
            }
            if (currentHealth == 0.0f) {
                Debug.Log($"{name} is dead");
                onDeath?.Invoke();
            }
        }
    }
}