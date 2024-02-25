using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Entity;
using Utilities;

using System;
using Data;
using System.Linq;

[Serializable] public enum EnemyType { STATIC, CHASING }

public class EnemyScript : MonoBehaviour, ISerialize {
    [SerializeField] public EnemyType type;
    [SerializeField] public int id;

    [SerializeField] private LayerMask playerLayer;


    [SerializeField] protected float distanceToPlayer;

    protected Rigidbody2D rigidBody;
    protected BoxCollider2D boxCollider;
    protected Stats stats;
    protected Health health;
    protected Animator animator;
    [SerializeField] protected CountDownTimer timer = new CountDownTimer(0f);

    // Start is called before the first frame update
    protected virtual void Start() {
        InitEnemy();
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        stats = GetComponent<Stats>();
        health = GetComponent<Health>();
        health.onDeath += () => {
            // Could spawn particles, give player xp, etc...
            Destroy(gameObject);
        };
        playerLayer = 1 << LayerMask.NameToLayer("Player");
    }

    protected virtual void InitEnemy() {
        type = EnemyType.STATIC;
        id = Utilities.DeterministicHashCode.Hash(type.ToString() + name);
    }

    // Update is called once per frame
    private void Update() {
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
        //none
    }



    protected void OnTriggerEnter2D(Collider2D collision) {
        // if(collision.CompareTag("Player Attack")) 
        // {
        //     //example damage
        //     int damage = 2;
        //     //need help implmenting the other scripts
        //     TakeDamage(damage);
        // }
        if (1 << collision.gameObject.layer != playerLayer.value) {
            return;
        }
        if (collision.TryGetComponent(out Health health) && stats.GetStat(StatType.DAMAGE, out float damage) && timer.isFinished) {
            if (stats.GetStat(StatType.ATTACK_SPEED, out float attackSpeed)) {
                health.Damage(damage);
                timer.Restart(1f / Mathf.Max(0.001f, attackSpeed));
            } else {
            }
        }
    }

    public virtual void OnSerialize(ref GameData data) {
        data.enemies.Add(new EnemyData(id, type, transform.position));
    }

    public virtual void OnDeserialize(GameData data) {
        transform.position = data.enemies.FirstOrDefault((EnemyData enemyData) => enemyData.id == id)?.enemyPos ?? transform.position;
    }
}