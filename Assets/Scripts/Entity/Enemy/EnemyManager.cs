using System.Collections.Generic;

using Entity.Player;

using UnityEngine;

namespace Entity.Enemy {
    public class EnemyManager : MonoBehaviour {
        [SerializeField] private List<EnemyScript> enemies;
        public Level playerLevel;

        private void Awake() {
            playerLevel = FindObjectOfType<PlayerController>().GetComponent<Level>();
        }

        public void RegisterEnemy(EnemyScript enemy) {
            if (!enemies.Contains(enemy)) {
                enemies.Add(enemy);
            } else {
                Debug.LogWarning($"Tried to register enemy {enemy.name} that has already been registered?...");
            }
        }

        public void UnregisterEnemy(EnemyScript enemy) {
            if (enemies.Contains(enemy)) {
                enemies.Remove(enemy);
                if (enemies.Count == 0) {
                    GameManager.instance.onLevelClear?.Invoke();
                }
            } else {
                Debug.LogWarning($"Tried to unregister enemy {enemy.name} that has already been unregistered?...");
            }
        }
    }
}