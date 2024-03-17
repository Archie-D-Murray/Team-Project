using System;
using System.Collections.Generic;
using Entity;
using Entity.Player;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Data {

    [Serializable] public class GameData {
        public PlayerData playerData;
        public int sceneID;

        public GameData() {
            playerData = new PlayerData();
        }
    }

    [Serializable] public class PlayerData {
        public PlayerClass playerClass;
        public List<Stat> stats;
        public List<(StatType type, StatModifier mod)> statModifiers;
        public Vector3 playerPos;
        public int weaponIndex;
        public float playerCurrentHealth;
        public float playerCurrentMana;
        public List<SerializableItem> items;
        public List<int> spells;
        public int level;
        public int xp;
        public int unappliedLevels;
    }
}