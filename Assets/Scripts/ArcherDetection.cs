using System.Collections;
using UnityEngine;

public class TowerScr : MonoBehaviour
{
    public GameObject arrowPrefab; // Use a prefab instead of a direct GameObject reference
    public float range = 5f;
    public float cooldown = 1f; // Renamed for clarity

    private Transform targetEnemy;
    private float currentCooldown;
    private Animator animator; // Add a variable for the animator

    private void Start()
    {
        currentCooldown = cooldown; // Initialize cooldown on start
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    private void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            return; // Exit Update if cooldown is not over
        }

        FindClosestTarget(); // Find a target before shooting

        if (targetEnemy != null)
        {
            Shoot();
        }
    }

    private bool CanShoot()
    {
        return currentCooldown <= 0;
    }

    private void FindClosestTarget()
    {
        targetEnemy = null;
        float closestDistance = Mathf.Infinity;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range); // Use Physics2D

        foreach (Collider2D collider in colliders)
        {
            if (!collider.CompareTag("Enemy")) continue; // Skip non-enemy objects

            Transform enemyTransform = collider.transform;
            float distance = Vector2.Distance(transform.position, enemyTransform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                targetEnemy = enemyTransform;
            }
        }

        // Add trigger to shoot when a new target is found
        if (targetEnemy != null && CanShoot())
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        currentCooldown = cooldown;

        // Trigger the shooting animation
        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }
    }

   
 
    private void FireArrow()
    {
        if (targetEnemy == null) return;

        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity); // Instantiate the arrow properly

        // Assume the Arrow script has a SetTarget method with two arguments
        if (arrow.TryGetComponent<Arrow>(out Arrow arrowScript))
        {
            // Pass both the target enemy and the reference to the EnemyHealth component
            arrowScript.SetTarget(targetEnemy, targetEnemy.GetComponent<EnemyHealth>());
        }
        else
        {
            Debug.LogError("Arrow script not found on arrow prefab!");
        }
    }

}
