using UnityEngine;

using Entity;
using Utilities;

using System;
using Entity.Enemy;
using Entity.Player;

[Serializable] public enum EnemyType { STATIC, CHASING, SHOOTING, BOSS }

public class EnemyScript : MonoBehaviour {
    [SerializeField] public EnemyType type;
    [SerializeField] public int id;

    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected int xpAmount = 60;

    [SerializeField] protected float distanceToPlayer;
    [SerializeField] protected float attackRange = 2f;
    [SerializeField] private bool standalone = false;
    [SerializeField] protected Transform playerTransform;

    protected Rigidbody2D rigidBody;
    protected BoxCollider2D boxCollider;
    protected Stats stats;
    protected Health health;
    protected Animator animator;
    [SerializeField] protected CountDownTimer timer = new CountDownTimer(0f);
    private EnemyManager enemyManager;

    protected int attackHash = Animator.StringToHash("attack");

    // Start is called before the first frame update
    protected virtual void Start() {
        InitEnemy();
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        stats = GetComponent<Stats>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        playerTransform = FindFirstObjectByType<PlayerController>().transform;
        playerLayer = 1 << LayerMask.NameToLayer("Player");
        if (standalone) {
            SetEnemyManager(FindFirstObjectByType<EnemyManager>());
        }
    }

    protected virtual void InitEnemy() {
        type = EnemyType.STATIC;
        id = Utilities.DeterministicHashCode.Hash(type.ToString() + name);
    }

    // Update is called once per frame
    protected virtual void Update() {
        EnemyMovement();
        EnemyAttacks();
    }

    private void FixedUpdate() {
        timer.Update(Time.fixedDeltaTime);
    }


    protected virtual void EnemyMovement() {

        //none

    }

    protected virtual void EnemyAttacks() {
        distanceToPlayer = Vector2.Distance(playerTransform.position, rigidBody.position);
        if (distanceToPlayer <= attackRange && timer.isFinished) {
            if (Physics2D.OverlapCircle(rigidBody.position, attackRange, playerLayer).TryGetComponent(out Health health) && stats.GetStat(StatType.DAMAGE, out float damage) && stats.GetStat(StatType.ATTACK_SPEED, out float attackSpeed)) {
                health.Damage(damage);
                timer.Restart(1f / Mathf.Max(0.001f, attackSpeed));
                animator.SetTrigger(attackHash);
            }
        }
    }


    // NOTE: OnTriggerStay2D is only called if the collider keeps moving (therefore doesn't keep attacking if the player
    // stands still inside attackRange), so I refactored this to the above code
    // protected virtual void OnTriggerStay2D(Collider2D collision) {
    //     if (1 << collision.gameObject.layer != playerLayer.value) {
    //         return;
    //     }
    //     if (collision.TryGetComponent(out Health health) && stats.GetStat(StatType.DAMAGE, out float damage) && timer.isFinished) {
    //         if (stats.GetStat(StatType.ATTACK_SPEED, out float attackSpeed)) {
    //             health.Damage(damage, transform.position);
    //             timer.Restart(1f / Mathf.Max(0.001f, attackSpeed));
    //         }
    //     }
    // }

    public virtual void SetEnemyManager(EnemyManager enemyManager) {
        this.enemyManager = enemyManager;
        health.onDeath += () => {
            enemyManager.playerLevel.AddXP(xpAmount);
            Destroy(gameObject);
        };
    }
}