using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    [SerializeField] private Vector2 targetDestination;
    private List<GameObject> enemies = new List<GameObject>();
    [SerializeField] private float speed;
    private int damage;

    // Start is called before the first frame update
    void Start()
    {
        damage = 25;
        speed = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetDestination, speed * Time.deltaTime);
        
    if (new Vector2(transform.position.x, transform.position.y) == targetDestination)
    {
        Explode();
    }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Enemy")
        {
            enemies.Add(coll.gameObject);
        }
    }

    public void SpawnFireball(Vector2 target)
    {
        targetDestination = target;
    }

    private void Explode()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            //enemies[i].GetComponent<EnemyController>().TakeDamage(damage);
        }

        Destroy(this.gameObject);
    }
}
