using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : EnemyScript
{
    private GameObject playerObject;
    [SerializeField] private float aggroRange;
    // Start is called before the first frame update
    protected override void Start()
    {
        playerObject = GameObject.Find("Player");
        enemySpeed = 1;
        enemyHealth = 50;
        enemyDamage = 30;
        aggroRange = 5;
    }

    protected override void EnemyMovement() 
    {
        distanceToPlayer = Vector2.Distance(transform.position, playerObject.transform.position);

        if (distanceToPlayer < aggroRange) 
        {
            transform.position = Vector2.MoveTowards(this.transform.position, playerObject.transform.position, enemySpeed * Time.deltaTime);
        }
    }

    protected override void EnemyAttacks() 
    {
        //none
    }


}
