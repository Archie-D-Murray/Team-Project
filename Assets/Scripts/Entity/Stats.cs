using System;
using System.Collections.Generic;

using UnityEngine;

using Data;

namespace Entity {
    public class Stats : MonoBehaviour, ISerialize {
        private static Stat[] DefaultStats = new Stat[5] { 
            new Stat(StatType.HEALTH      ,  10f),  
            new Stat(StatType.SPEED       ,   5f),
            new Stat(StatType.DAMAGE      ,   1f),
            new Stat(StatType.MAGIC       ,   0f),
            new Stat(StatType.ATTACK_SPEED,   1f)
        };

        #region Enum => String cache
        private const string Health = "Max HEALTH";
        private const string AttackSpeed = "Attack SPEED";
        private const string Speed = "SPEED";
        private const string Damage = "DAMAGE";
        private const string Magic = "MAGIC";
        private const string Mana = "Max MANA";
        #endregion

        public Stat[] statDict = Stats.DefaultStats;
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
                FindStat(type)?.Add(amount);
            } else {
                statModifers.Add(type, new StatModifier(amount, duration));
                FindStat(type)?.Add(amount);
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
                FindStat(type).Remove(statModifers[type].value);
                statModifers.Remove(type);
            });
            lastUpdate = Time.time;
        }

        public void OnSerialize(ref GameData data) {
            if (!gameObject.HasComponent<PlayerController>()) { return; }
            data.stats = new List<Stat>(statDict.Length);
            foreach (Stat stat in statDict) {
                data.stats.Add(stat);
            }
        }

        public void OnDeserialize(GameData data) {
            if (!gameObject.HasComponent<PlayerController>()) { return; }
            statDict = new Stat[data.stats.Count];
            for (int i = 0; i < data.stats.Count; i++) {
                statDict[i] = data.stats[i];
            }
        }
    }
}