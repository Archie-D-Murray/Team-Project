using System;
using System.Collections;

using Data;

using UnityEngine;

namespace Entity.Player {
    public class Level : MonoBehaviour, ISerialize {
        [SerializeField] private LevelConfig config;
        [SerializeField] private Stats stats;

        [SerializeField] private int level = 0;
        [SerializeField] private int xp = 0;

        public int unappliedLevels { get; private set; }

        private int unappliedXP = 0;
        private Coroutine xpTick = null;

        private void Start() {
            stats = GetComponent<Stats>();
        }

        public int currentXP => xp;
        public int levelXP => config.XPForLevel(level + 1);
        public float xpProgress => (float) xp / (float) config.XPForLevel(level + 1);

        public int currentLevel => level;
        public int maxLevel => config.maxLevel;
        public float levelProgress => (float) level / (float) config.maxLevel;

        public Action<int> onLevelUp;
        public Action<float> onEarnXP;

        public void AddXP(int amount) {
            unappliedXP = amount;
            if (xpTick == null) {
                xpTick = StartCoroutine(XPTick());
            }
            onEarnXP?.Invoke(amount);
        }

        private IEnumerator XPTick() {
            while (unappliedXP > 0) {
                if (level == config.maxLevel) {
                    unappliedXP = 0;
                    xpTick = null;
                    yield break;
                }
                if (unappliedXP >= RemainingXPForLevel(level + 1)) {
                    unappliedXP -= RemainingXPForLevel(level + 1);
                    unappliedLevels++;
                    level++;
                    xp = 0;
                    onLevelUp?.Invoke(level + 1);
                } else {
                    xp += unappliedXP;
                    unappliedXP = 0;
                }
            }
            yield return null;
            xpTick = null;
        }

        private int RemainingXPForLevel(int level) {
            return Mathf.Max(0, config.XPForLevel(level) - xp);
        }

        public float GetStatIncrease(StatType type) {
            return Array.Find(config.levelIncrements, (Stat stat) => stat.type == type)?.value ?? 0f;
        }

        public void LevelUpStat(StatType type) {
            stats.IncrementStat(type, Array.Find(config.levelIncrements, (Stat stat) => stat.type == type).value);
            unappliedLevels--;
            Debug.Log($"Levelled up stat: {type}");
        }
        
        public void SetConfig(LevelConfig config) {
            this.config = config;
        }

        public void OnSerialize(ref GameData data) {
            data.playerData.level = level;
            data.playerData.xp = xp;
            data.playerData.unappliedLevels = unappliedLevels;
        }

        public void OnDeserialize(GameData data) {
            this.level = data.playerData.level;
            this.xp = data.playerData.xp;
            this.unappliedLevels = data.playerData.unappliedLevels;
        }
    }
}