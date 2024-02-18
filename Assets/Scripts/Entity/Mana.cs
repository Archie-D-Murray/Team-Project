using System;

using UnityEngine;

namespace Entity {
    [RequireComponent(typeof(Stats))]
    public class Mana : MonoBehaviour {
        public float getPercentMana => Mathf.Clamp01(currentMana / maxMana);
        public float getCurrentMana => currentMana;
        public float getMaxMana => maxMana;

        public Action<float> onManaUse;
        public Action<float> onManaRecover;
        public Action oom;

        private bool doingRegen = false;

        [SerializeField] private float currentMana;
        [SerializeField] private float maxMana;
        [SerializeField] private float manaRegen;

        private void Awake() {
            Stats stats = GetComponent<Stats>();
            if (stats.GetStat(StatType.MANA, out float mana)) { 
                maxMana = mana;
                currentMana = mana;
            } else {
                Debug.LogError("MANA was not present in Stats!");
                Destroy(this);
                return;
            }

            if (stats.GetStat(StatType.MAGIC, out float magic)) {
                manaRegen = magic / 10f;
            } else {
                Debug.LogError("Magic was not present in Stats!");
                Destroy(this);
                return;
            }
            stats.updateStat += UpdateMaxMana;
            InvokeRepeating(nameof(ManaRegen), 1f, 0.5f);
            doingRegen = true;
        }

        private void UpdateMaxMana(StatType type, float stat) {
            switch (type) {
                case StatType.MANA:
                    float diff = stat - currentMana;
                    maxMana = stat;
                    UseMana(diff);
                    break;
                case StatType.MAGIC:
                    manaRegen = stat / 10f;
                    break;
                default:
                    break;
            }
        }

        public void UseMana(float manaCost) {
            if (currentMana == 0.0f) { //Currently OOM
                return;
            }
            manaCost = Mathf.Max(manaCost, 0.0f);
            if (manaCost != 0.0f) {
                currentMana = Mathf.Max(0.0f, currentMana - manaCost);
                onManaUse?.Invoke(manaCost);
            }
            if (currentMana == 0.0f) {
                Debug.Log($"{name} has no mana");
                oom?.Invoke();
            }
        }

        public void RecoverMana(float amount) {
            amount = Mathf.Max(amount, 0.0f);
            if (amount != 0.0f) {
                currentMana = Mathf.Min(maxMana, currentMana + amount);
                onManaRecover?.Invoke(amount);
            }
        }

        public void ManaRegen() {
            RecoverMana(manaRegen);    
        }

        private void OnEnable() {
            if (!doingRegen) {
                InvokeRepeating(nameof(ManaRegen), 1f, 0.5f);
            }
        }

        private void OnDisable() {
            if (doingRegen) {
                CancelInvoke(nameof(ManaRegen));
            }
        }
    }
}