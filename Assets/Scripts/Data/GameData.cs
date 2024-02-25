using System;
using System.Collections.Generic;
using Entity;
using UnityEngine;

namespace Data {

    [Serializable] public class GameData {
        public PlayerData playerData;
        public List<EnemyData> enemies;
    }

    [Serializable] public class PlayerData {
        public List<Stat> stats;
        public List<(StatType type, StatModifier mod)> statModifiers;
        public Vector3 playerPos;
        public int weaponIndex;
        public float playerCurrentHealth;
        public float playerCurrentMana;
        public List<SerializeableItem> items;
    }

    [Serializable] public class EnemyData {
        public int id;
        public EnemyType type;
        public List<Stat> stats;
        public List<(StatType type, StatModifier mod)> statModifiers;
        public Vector3 enemyPos;
        public float enemyCurrentHealth;
        public float enemyCurrentMana;

        public EnemyData(int id, EnemyType type, Vector3 enemyPos) {
            this.id = id;
            this.type = type;
            this.enemyPos = enemyPos;
        } 
    }
}