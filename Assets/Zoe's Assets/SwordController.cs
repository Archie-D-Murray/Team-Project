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

        Vector3 direction = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition) - transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
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
