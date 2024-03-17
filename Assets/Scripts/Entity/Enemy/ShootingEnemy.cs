using UnityEngine;
using System;
using Entity;

using UnityEngine.AI;

using Data;
using Utilities;

namespace Entity {
    public class ShootingEnemy : EnemyScript {

        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float aggroRange;
        [SerializeField] private float projectileSpeed;
        NavMeshAgent agent;

        private CountDownTimer attackTimer = new CountDownTimer(0f);
        protected override void InitEnemy() {
            type = EnemyType.SHOOTING;
            id = HashCode.Combine(type.ToString(), name);
        }

        protected override void Start() {
            base.Start(); // Still need to get component references so need to call base
            agent = GetComponent<NavMeshAgent>();
            agent.updateUpAxis = false;
            agent.updateRotation = false;
            health.onDamage += GetComponent<KnockbackHandler>().Knockback;
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
            attackRange = 10;
        }

        protected override void EnemyMovement() {
            distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer > attackRange && Vector3.Distance(agent.destination, playerTransform.position) > 1.0f) {
                agent.destination = playerTransform.position;
            }
        }

        protected override void EnemyAttacks() {
            distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer < attackRange && timer.isFinished) {
                if (stats.GetStat(StatType.ATTACK_SPEED, out float attackSpeed) && stats.GetStat(StatType.DAMAGE, out float damage)) {
                    Instantiate(projectilePrefab, transform.position, Quaternion.identity)
                        .GetOrAddComponent<EnemyProjectile>()
                        .Init(projectileSpeed, damage, (Vector2) (playerTransform.position - transform.position).normalized);
                    timer.Restart(1f / Mathf.Max(0.001f, attackSpeed));
                }
                
            }
        } 
    }
}