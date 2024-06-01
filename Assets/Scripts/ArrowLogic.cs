using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    
    private EnemyHealth enemyHealth;
    private Transform target;
    private float speed = 7f;
    private int damage = 3;

    void Update()
    {
        Move();
    }

    public void SetTarget(Transform enemy, EnemyHealth health)
    {
        target = enemy;
        enemyHealth = health;
    }

    private void Move()
    {
        if (target != null)
        {
            // Check if the target has been destroyed
            if (!target.gameObject.activeSelf)
            {
                Destroy(gameObject);
                return;
            }

            // Check if the distance to the target is less than a threshold
            if (Vector2.Distance(transform.position, target.position) < 4f)
            {
                // Destroy the arrow
                Destroy(gameObject);

                // Deal damage to the enemy
                enemyHealth.TakeDamageEnemy(damage);
            }
            else
            {
                // Move towards the target
                Vector2 dir = target.position - transform.position;
                transform.Translate(dir.normalized * Time.deltaTime * speed);
            }
        }
        else
        {
            // Destroy the arrow if there is no target
            Destroy(gameObject);
        }
    }
}
