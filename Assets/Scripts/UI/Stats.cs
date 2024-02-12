using System;
using System.Collections.Generic;

using UnityEngine;

namespace UI {
    public class Stats : MonoBehaviour {

        #region Enum => String cache
        private const string Health = "Health";
        private const string AttackSpeed = "Attack Speed";
        private const string Speed = "Speed";
        private const string Damage = "Damage";
        private const string Magic = "Magic";
        #endregion

        public Dictionary<StatType, float> statDict => statDict;
        public Action<StatType, float> updateStat;

        public string GetStatDisplay(StatType type) {
            return $"{GetStatName(type)}: {statDict[type]}";
        }

        public string GetStatName(StatType type) {
            return type switch {
                StatType.Health => Health,
                StatType.AttackSpeed => AttackSpeed,
                StatType.Speed => Speed,
                StatType.Damage => Damage,
                StatType.Magic => Magic,
                _ => string.Empty
            };
        }

        public bool GetStat(StatType type, out float stat) { 
            return statDict.TryGetValue(type, out stat);
        }

        public void UpdateStat(StatType type, float amount, bool setToValue = false) {
            if (!statDict.ContainsKey(type)) {
                Debug.LogError($"Stat type {type} was not an entry in stats!");
                return;
            }
            statDict[type] = setToValue ? amount : statDict[type] + amount;
            updateStat?.Invoke(type, statDict[type]);
        }
    }
}