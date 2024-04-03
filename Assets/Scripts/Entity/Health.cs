using System;
using System.Linq;

using UnityEngine;

using Data;
using Utilities;
using System.Collections;

namespace Entity {
    [RequireComponent(typeof(Stats))]
    public class Health : MonoBehaviour, ISerialize {
        public float getPercentHealth => Mathf.Clamp01(currentHealth / maxHealth);
        public float getCurrentHealth => currentHealth;
        public float getMaxHealth => maxHealth;
        public bool isInvulnerable => invulnerable;
        public Action<float, KnockbackData> onDamage;
        public Action<float> onHeal;
        public Action onDeath;
        public Action onInvulnerableDamage;
        
        private float invulnerabilityTimer = 0f;
        private bool invulnerable = false;
        private Coroutine invulnerabilityReset = null;

        [SerializeField] private float currentHealth;
        [SerializeField] private float maxHealth;

        private void Awake() {
            Stats stats = GetComponent<Stats>();
            if (stats.GetStat(StatType.HEALTH, out float health)) { 
                maxHealth = health;
                currentHealth = health;
            } else {
                Debug.LogError("HEALTH was not present in Stats!");
                Destroy(this);
                return;
            }
            stats.updateStat += UpdateMaxHealth;
        }

        private void UpdateMaxHealth(StatType type, float health) {
            Debug.Log($"Updated max health for {name} to {health}");
            if (type != StatType.HEALTH) {
                return;
            }
            float diff = health - currentHealth;
            maxHealth = health;
            Heal(diff);
        }

        /// <summary>Damages an entity</summary>
        /// <param name="damage">Damage to apply</param>
        /// <param name="entityPos">Position of entity that applied the knockback</param>
        public void Damage(float damage, Vector2? entityPos = null) {
            if (currentHealth == 0.0f) { //Don't damage dead things!
                return;
            } else if (invulnerable) {
                onInvulnerableDamage?.Invoke();
                return;
            }
            damage = Mathf.Max(damage, 0.0f);
            if (damage != 0.0f) {
                currentHealth = Mathf.Max(0.0f, currentHealth - damage);
                onDamage?.Invoke(damage, entityPos != null ? new KnockbackData(entityPos.Value, true) : KnockbackData.Null());
            }
            if (currentHealth == 0.0f) {
                Debug.Log($"{name} is dead");
                onDeath?.Invoke();
            }
        }

        public void Heal(float amount) {
            amount = Mathf.Max(0f, amount);
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            onHeal?.Invoke(amount);
        }

        public void OnSerialize(ref GameData data) {
            data.playerData.playerCurrentHealth = currentHealth;
        }

        public void OnDeserialize(GameData data) {
            currentHealth = data.playerData.playerCurrentHealth;
        }

        public void SetInvulnerable(float time) {
            if (invulnerabilityReset != null) {
                StopCoroutine(invulnerabilityReset);
            }
            invulnerabilityTimer += time;
            invulnerabilityReset = StartCoroutine(InvulnerabilityReset());
        }

        private IEnumerator InvulnerabilityReset() {
            while (invulnerabilityTimer >= 0f) {
                invulnerabilityTimer -= Time.fixedDeltaTime;
                yield return Yielders.waitForFixedUpdate;
            }
        }
    }
}