using System.Linq;

using UnityEngine;

using Utilities;

using Tags.Enemy;

namespace Entity.Enemy {
    [Tooltip("Must be placed as child of EnemyManager!")]
    [SerializeField] public class EnemySpawner : MonoBehaviour {

        private const float MIN_SPAWN_RATE = 0.1f;

        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private GameObject enemyPrefab;
        [Tooltip("Must be placed as child of EnemyManager!")]
        [SerializeField] private EnemyManager enemyManager;
        
        private int spawnIndex = 0;
        private int maxSpawnCount;
        private int spawnCount = 0;
        private float spawnDelay;
        private CountDownTimer timer = new CountDownTimer(0f, true);

        private void Start() {
            spawnPoints = GetComponentsInChildren<SpawnPoint>().ToList().ConvertAll<Transform>((SpawnPoint spawnPoint) => spawnPoint.transform).ToArray();
            enemyManager = GetComponentInParent<EnemyManager>();
        }

        public void SetSpawnDelay(float spawnDelay) {
            if (spawnDelay < MIN_SPAWN_RATE) {
                Debug.LogWarning($"Tried to set a new spawn rate ({spawnDelay}) that was too small!");
                return;
            }
            this.spawnDelay = spawnDelay;
        }

        public void IncreaseSpawnCount(int maxSpawnCount) {
            if (maxSpawnCount <= this.maxSpawnCount) {
                Debug.LogWarning($"Tried to set a new spawn count ({maxSpawnCount}) that wasn't larger than current!");
                return;
            }
            this.maxSpawnCount = maxSpawnCount;
        }

        public void TickSpawner(float deltaTime) {
            timer.Update(deltaTime);
            if (timer.isFinished && spawnCount < maxSpawnCount) {
                Spawn();
                spawnCount++;
                spawnIndex = ++spawnIndex % spawnPoints.Length;
                timer.Restart(spawnDelay);
            }
        }

        private void Spawn() {
            EnemyScript enemy = Instantiate(enemyPrefab, spawnPoints[0].position, Quaternion.AngleAxis(Random.Range(-180f, 180f), Vector3.forward)).GetComponent<EnemyScript>();
            enemy.SetEnemyManager(enemyManager);
        }
    }
}