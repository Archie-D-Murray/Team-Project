using System;

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
    }
}