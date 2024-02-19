using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Entity;
using Utilities;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] protected float enemySpeed;
    [SerializeField] protected float enemyHealth;
    [SerializeField] protected int enemyDamage;

    [SerializeField] private LayerMask playerLayer;


    [SerializeField] protected float distanceToPlayer;

    protected Rigidbody2D rigidBody;
    protected BoxCollider2D boxCollider;
    protected Stats stats;
    protected Health health;
    protected Animator animator;
    protected CountDownTimer timer = new CountDownTimer(0f);

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        stats = GetComponent<Stats>();
        health = GetComponent<Health>();
        health.onDeath += () => {
            // Could spawn particles, give player xp, etc...
            Destroy(gameObject);
        };
    }

    // Update is called once per frame
    private void Update()
    {
        EnemyMovement();
        EnemyAttacks();
    }

    private void FixedUpdate() {
        timer.Update(Time.fixedDeltaTime);
    }


    protected virtual void EnemyMovement() 
    {

        //none

    }

    protected virtual void EnemyAttacks() 
    {
        //none
    }

    

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        // if(collision.CompareTag("Player Attack")) 
        // {
        //     //example damage
        //     int damage = 2;
        //     //need help implmenting the other scripts
        //     TakeDamage(damage);
        // }
        if (collision.gameObject.layer != playerLayer.value) {
            return;
        }
        if (collision.TryGetComponent(out Health health) && stats.GetStat(StatType.DAMAGE, out float damage) && timer.isFinished) {
            if (stats.GetStat(StatType.ATTACK_SPEED, out float attackSpeed)) {
                health.Damage(damage);
                timer.Restart(1f / attackSpeed);
            }
        }
    }

}