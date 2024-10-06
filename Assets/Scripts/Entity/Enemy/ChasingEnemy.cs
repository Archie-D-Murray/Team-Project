using System;

using Entity;

using UnityEngine;
using UnityEngine.AI;

using Data;

public class ChasingEnemy : EnemyScript {
    [SerializeField] private float aggroRange;
    protected int xHash = Animator.StringToHash("x");
    protected int yHash = Animator.StringToHash("y");
    protected int speedHash = Animator.StringToHash("speed");
    NavMeshAgent agent;
    // Start is called before the first frame update
    protected override void Start() {
        base.Start(); // Still need to get component references so need to call base
        agent = GetComponent<NavMeshAgent>();
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit pos, 20.0f, NavMesh.AllAreas)) {
            agent.Warp(pos.position);
        }
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
        aggroRange = 5;
    }

    protected override void EnemyMovement() {
        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (agent.desiredVelocity.sqrMagnitude > 0.001f) {
            animator.SetFloat(xHash, agent.velocity.x);
            animator.SetFloat(yHash, agent.velocity.y);
        }
        animator.SetFloat(speedHash, agent.velocity.sqrMagnitude);
        if (distanceToPlayer < aggroRange && Vector3.Distance(agent.destination, playerTransform.position) > 0.05f) {
            agent.destination = playerTransform.position;
        }
    }

    protected override void EnemyAttacks() {
        if (distanceToPlayer < attackRange && timer.isFinished) {
            if (Physics2D.OverlapCircle(rigidBody.position, attackRange, playerLayer).TryGetComponent(out Health health) && stats.GetStat(StatType.DAMAGE, out float damage) && stats.GetStat(StatType.ATTACK_SPEED, out float attackSpeed)) {
                animator.SetTrigger(attackHash);
                health.Damage(damage);
                timer.Restart(1f / attackSpeed);
            }
        }
    }

    protected override void InitEnemy() {
        type = EnemyType.CHASING;
        id = Utilities.DeterministicHashCode.Hash(type.ToString() + name);
    }
}