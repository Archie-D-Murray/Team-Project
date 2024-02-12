using System;

using UnityEngine;

namespace Entity {
    public class Stats : MonoBehaviour {

        #region Enum => String cache
        private const string Health = "Health";
        private const string AttackSpeed = "Attack Speed";
        private const string Speed = "Speed";
        private const string Damage = "Damage";
        private const string Magic = "Magic";
        #endregion

        public Stat[] statDict;
        public Action<StatType, float> updateStat;

        public string GetStatDisplay(StatType type) {
            return $"{GetStatName(type)}: {FindStat(type)?.value ?? float.NaN}";
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
    }
}