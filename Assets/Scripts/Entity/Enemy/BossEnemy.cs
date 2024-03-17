using UnityEngine;
using System;
using Entity;
using UnityEngine.AI;
using Data;
using Utilities;

using System.Collections;
using Entity.Enemy;


namespace Entity {
    [Serializable] public enum BossState { NONE, ALIVE, DEAD }

    public class BossEnemy : EnemyScript {

        public GameObject bossProjectilePrefab;
        public GameObject enemyMinion;
        NavMeshAgent agent;
        private CountDownTimer attackTimer = new CountDownTimer(0f);
        [SerializeField] private float bossProjectileSpeed;
        [SerializeField] private float chargeSpeed;
        [SerializeField] private float enemySpawnRate;
        protected override void InitEnemy() {
            type = EnemyType.BOSS;
            id = HashCode.Combine(type.ToString(), name);
        }

        protected override void Start() {
            base.Start(); // Still need to get component references so need to call base
            agent = GetComponent<NavMeshAgent>();
            agent.updateUpAxis = false;
            agent.updateRotation = false;
            if (stats.GetStat(StatType.SPEED, out float speed)) {
                agent.speed = speed;
            }

            stats.updateStat += (StatType type, float amount) => {
                if (type != StatType.SPEED) { return; }
                agent.speed = amount;
            };
            GameManager.instance.RegisterBossSpawn();

            if (!playerTransform) {
                Debug.LogError("Could not find player!");
                Destroy(this);
            }
        }

        protected override void EnemyMovement() {
            agent.destination = playerTransform.position;
        }

        protected override void EnemyAttacks() {

            if (timer.isFinished) {
                // numberOfAttack = UnityEngine.Random.Range(1, 21);
                if (stats.GetStat(StatType.ATTACK_SPEED, out float attackSpeed) && stats.GetStat(StatType.DAMAGE, out float damage)) {
                    if (UnityEngine.Random.value <= 0.4f) { //40% Chance to do charge
                        StartCoroutine(ChargeAttack());
                    } else {
                        GameObject bossProjectile = Instantiate(bossProjectilePrefab, transform.position, Quaternion.identity);
                        bossProjectile.GetOrAddComponent<EnemyProjectile>().Init(bossProjectileSpeed, damage, (Vector2)(playerTransform.position - transform.position).normalized);
                    }


                    if (health.getCurrentHealth <= health.getMaxHealth / 2) {
                        StartCoroutine(SpawnEnemy());
                    }
                    timer.Restart(1f / Mathf.Max(0.001f, attackSpeed));
                }

            }
        }

        public override void SetEnemyManager(EnemyManager enemyManager) {
            health.onDeath += () => GameManager.instance.RegisterBossDeath();
            base.SetEnemyManager(enemyManager);
        }

        private IEnumerator ChargeAttack() {
            if (stats.GetStat(StatType.SPEED, out float speed)) {
                yield return new WaitForSeconds(3f);
                agent.speed = chargeSpeed;
                if ((Physics2D.OverlapCircle(rigidBody.position, attackRange, playerLayer).OrNull()?.TryGetComponent(out Health health) ?? false) && stats.GetStat(StatType.DAMAGE, out float damage)) {
                    health.Damage(damage);
                }
                yield return new WaitForSeconds(0.2f);

                agent.speed = speed;

            }
        }

        private IEnumerator SpawnEnemy() {
            yield return new WaitForSeconds(enemySpawnRate);
            Instantiate(enemyMinion, transform.position, Quaternion.identity);
        }
    }
}