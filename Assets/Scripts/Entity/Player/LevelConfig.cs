using System;

using UnityEngine;

namespace Entity.Player {
    [CreateAssetMenu(menuName = "Level Config")]
    public class LevelConfig : ScriptableObject {
        
        public Stat[] levelIncrements = new Stat[5] {
            new Stat(StatType.HEALTH,       0f),  
            new Stat(StatType.SPEED,        0f),
            new Stat(StatType.DAMAGE,       0f),
            new Stat(StatType.MAGIC,        0f),
            new Stat(StatType.ATTACK_SPEED, 0f)        
        };

        public int baseLevelXP = 100;
        [Range(1f, 5f)] public float levelIncrease = 1f;
        public int maxLevel = 10;

        public int XPForLevel(int level) {
            return Mathf.CeilToInt(baseLevelXP * Mathf.Pow(levelIncrease, level));
        }
    }
}