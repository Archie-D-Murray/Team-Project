using System;

using UnityEngine;

namespace UI {
    [RequireComponent(typeof(Stats))]
    public class Health : MonoBehaviour {
        public float getPercentHealth => Mathf.Clamp01(currentHealth / maxHealth);
        public float getCurrentHealth => currentHealth;
        public float getMaxHealth => maxHealth;

        public Action<float> onDamage;
        public Action onDeath;

        [SerializeField] private float currentHealth;
        [SerializeField] private float maxHealth;

        private void Awake() {
            Stats stats = GetComponent<Stats>();
            if (stats.GetStat(StatType.Health, out float health)) { 
                maxHealth = health;
                currentHealth = health;
            } else {
                Debug.LogError("Health was not present in Stats!");
                Destroy(this);
                return;
            }
            stats.updateStat += UpdateMaxHealth;
        }

        private void Update() {
            if (UnityEngine.Input.GetKeyDown(KeyCode.X)) {
                Damage(10);
            }
        }

        private void UpdateMaxHealth(StatType type, float health) {
            if (type != StatType.Health) {
                return;
            }
            float diff = health - currentHealth;
            maxHealth = health;
            Damage(diff);
        }

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