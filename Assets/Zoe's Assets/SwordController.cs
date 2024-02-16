using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    private int damage;

    private void Start()
    {
        damage = 5;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                //enemies[i].GetComponent<EnemyController>().TakeDamage(damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            enemies.Add(coll.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            enemies.Remove(coll.gameObject);
        }
    }
}
