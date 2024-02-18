using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] protected float enemySpeed;
    [SerializeField] protected float enemyHealth;
    [SerializeField] protected int enemyDamage;


    [SerializeField] protected float distanceToPlayer;

    private Rigidbody2D rigidBody;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        enemySpeed = 0;
        enemyHealth = 20;
        enemyDamage = 10;
    }

    // Update is called once per frame
    private void Update()
    {
        EnemyMovement();
        EnemyAttacks();
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
        if(collision.CompareTag("Player Attack")) 
        {
            //example damage
            int damage = 2;
            //need help implmenting the other scripts
            TakeDamage(damage);
        }
        else if(collision.CompareTag("Player"))
        {
           // collision.GetComponent<PlayerController>().ReceiveDamage();
        }
    }

    private void TakeDamage(int damageDealt) 
    {
        enemyHealth -= damageDealt;
    }


    private void CheckHealth() 
    {
        if(enemyHealth <= 0) 
        {
            Destroy(this.gameObject);
        }
    }

}
