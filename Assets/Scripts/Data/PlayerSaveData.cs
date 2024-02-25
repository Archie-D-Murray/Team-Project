using System;
using System.Collections.Generic;
using Entity;
using UnityEngine;

namespace Data {

[Serializable]
    public class GameData {
        public List<Stat> stats;
        public List<(StatType type, StatModifier mod)> statModifiers;
        public Vector3 playerPos;
        public int weaponIndex;
        public float playerCurrentHealth;
        public float playerCurrentMana;
        public List<SerializeableItem> items;
    }
}