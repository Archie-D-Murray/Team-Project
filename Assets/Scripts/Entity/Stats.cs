using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Entity {
    public class Stats : MonoBehaviour {

        #region Enum => String cache
        private const string Health = "Max HEALTH";
        private const string AttackSpeed = "Attack SPEED";
        private const string Speed = "SPEED";
        private const string Damage = "DAMAGE";
        private const string Magic = "MAGIC";
        private const string Mana = "Max MANA";
        #endregion

        public Stat[] statDict;
        public Action<StatType, float> updateStat;
        public Dictionary<StatType, StatModifier> statModifers = new Dictionary<StatType, StatModifier>();

        private float lastUpdate = 0;
        private List<StatType> toRemove = new List<StatType>();

        private void Start() {
            lastUpdate = Time.time;
            InvokeRepeating(nameof(UpdateStatModifiers), 0.0f, 0.25f);
        }

        public string GetStatDisplay(StatType type) {
            return $"{GetStatName(type)}: {FindStat(type)?.value ?? float.NaN}";
        }

        public string GetStatName(StatType type) {
            return type switch {
                StatType.HEALTH => Health,
                StatType.ATTACK_SPEED => AttackSpeed,
                StatType.SPEED => Speed,
                StatType.DAMAGE => Damage,
                StatType.MAGIC => Magic,
                StatType.MANA => Mana,
                _ => string.Empty
            };
        }

        public bool GetStat(StatType type, out float stat) { 
            Stat statInstance = FindStat(type);
            stat = float.NaN;

            if (statInstance != null) {
                stat = statInstance.value;
            }

            return statInstance != null;
        }

        private Stat FindStat(StatType type) => Array.Find(statDict, (Stat stat) => stat.type == type);

        public void UpdateStat(StatType type, float amount, bool setToValue = false) {
            Stat stat = FindStat(type);
            stat.value = setToValue ? amount : stat.value + amount;
            updateStat?.Invoke(type, stat.value);
        }

        public void AddStatModifer(StatType type, float amount, float duration) {
            if (statModifers.ContainsKey(type)) {
                statModifers[type].value += amount;
                statModifers[type].duration = duration;
                Array.Find(statDict, (Stat stat) => stat.type == type).Add(amount);
            } else {
                statModifers.Add(type, new StatModifier(amount, duration));
                Array.Find(statDict, (Stat stat) => stat.type == type)?.Add(amount);
            }
        }

        private void UpdateStatModifiers() {
            float delta = Time.time - lastUpdate;
            toRemove.Clear();
            foreach (KeyValuePair<StatType, StatModifier> item in statModifers) {
                if (item.Value.isFinished) {
                    toRemove.Add(item.Key);
                    continue;
                }
                item.Value.Update(delta);
            }
            toRemove.ForEach((StatType type) => {
                Array.Find(statDict, (Stat stat) => stat.type == type).Remove(statModifers[type].value);
                statModifers.Remove(type);
            });
            lastUpdate = Time.time;
        }
    }
}