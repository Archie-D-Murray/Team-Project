using Entity;

using UnityEngine;
using UnityEngine.AI;

public class ChasingEnemy : EnemyScript
{
    private Transform playerTransform;
    [SerializeField] private float aggroRange;
    NavMeshAgent agent;
    // Start is called before the first frame update
    protected override void Start()
    {
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

        playerTransform = FindObjectOfType<PlayerController>().OrNull()?.transform ?? null;
        if (!playerTransform) {
            Debug.LogError("Could not find player!");
            Destroy(this);
        }
        aggroRange = 5;
    }

    protected override void EnemyMovement() 
    {
        distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < aggroRange && Vector3.Distance(agent.destination, playerTransform.position) > 0.05f) 
        {
            agent.destination = playerTransform.position;
        }
    }
}