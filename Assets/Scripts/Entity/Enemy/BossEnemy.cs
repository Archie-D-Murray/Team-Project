using UnityEngine;
using System;
using Entity;
using UnityEngine.AI;
using Data;
using Utilities;

using Random = System.Random;
using System.Collections;

namespace Entity {
    public class BossEnemy : EnemyScript {

        public GameObject bossProjectile;
        public GameObject enemyMinion;
        NavMeshAgent agent;
        private Random randomAttack = new Random();
        private int numberOfAttack = 0;
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
                numberOfAttack = randomAttack.Next(1, 21);
                if (stats.GetStat(StatType.ATTACK_SPEED, out float attackSpeed) && stats.GetStat(StatType.DAMAGE, out float damage)) {
                    if(numberOfAttack <= 8) {
                        StartCoroutine(ChargeAttack());
                    }
                    else if (numberOfAttack >= 9) {
                        bossProjectile.GetComponent<EnemyProjectile>().SetDamage(damage);
                        bossProjectile.GetComponent<EnemyProjectile>().SetProjectileSpeed(bossProjectileSpeed);
                        Instantiate(bossProjectile, transform.position, Quaternion.identity);
                    }

                    
                    if(health.getCurrentHealth <= health.getMaxHealth/2 ) {
                        StartCoroutine(SpawnEnemy());
                    }
                    
                    
                    timer.Restart(1f / Mathf.Max(0.001f, attackSpeed));
                }

            }
        }


        private IEnumerator ChargeAttack() {
            if (stats.GetStat(StatType.SPEED, out float speed)) {
                yield return new WaitForSeconds(3f);
                agent.speed = chargeSpeed;
                if (Physics2D.OverlapCircle(rigidBody.position, attackRange, playerLayer).TryGetComponent(out Health health) && stats.GetStat(StatType.DAMAGE, out float damage)) {
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