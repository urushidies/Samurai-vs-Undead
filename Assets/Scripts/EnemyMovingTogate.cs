using JetBrains.Annotations;
using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour
{
    public Transform gate;

    private PlayerHealth playerHP;
    private towerhealth towerHealth;
    private Rigidbody2D rb;
    private EnemyHealth enemyHealth;

    public float speed = 1f;
    public bool playerInSight = false;
    public bool towerInSight = false;
    public float detectionRadius = 0.4f;

    public Animator animator;

    public string animationTrigger = "Attack";
    public StunnedBounceBack stunnedBounceBackScript;
    private bool facingRight = true;
    private bool stunned = false;

    [SerializeField]
    private int damage = 3;

    [SerializeField]
    private float radiusOffset = 0.5f;

    private void Awake()
    {
        FindGate();
        if (gate == null)
        {
            Debug.LogError("Gate not found!");
            return;
        }

        playerHP = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerHealth>();
        towerHealth = GameObject.FindGameObjectWithTag("Good Gate")?.GetComponent<towerhealth>();
        if (playerHP == null)
        {
            Debug.LogError("PlayerHealth component not found on Player!");
        }

        enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth == null)
        {
            Debug.LogError("EnemyHealth component not found!");
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found!");
        }
    }

    private void Update()
    {
        if (stunned) return; // If stunned, do nothing

        FindGate(); // Ensure we always have the correct gate

        playerInSight = false;
        towerInSight = false;
        if (enemyHealth != null && playerHP != null)
        {
            if (enemyHealth.EnemyHealthAmount > 0 && playerHP.PlayerHP > 0)
            {
                Vector2 detectionCenter = (Vector2)transform.position + new Vector2(radiusOffset * (facingRight ? 1 : -1), 0);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(detectionCenter, detectionRadius);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("Player"))
                    {
                        playerInSight = true;
                        break;
                    }
                    if (collider.CompareTag("Good Gate"))
                    {
                        towerInSight = true;
                        break;
                    }
                }

                if (playerInSight || towerInSight)
                {
                    animator.ResetTrigger("NoPlayerNear");
                    animator.SetTrigger(animationTrigger);
                }
                else
                {
                    animator.ResetTrigger(animationTrigger);
                    Vector3 gateDirection = (gate.position - transform.position).normalized;
                    Vector3 newPosition = transform.position + new Vector3(gateDirection.x * speed * Time.deltaTime, 0, 0);
                    transform.position = newPosition;
                    animator.SetTrigger("NoPlayerNear");
                }
            }
        }
    }

    private void FindGate()
    {
        if (gate == null)
        {
            GameObject gateObject = GameObject.FindGameObjectWithTag("Good Gate");
            if (gateObject != null)
            {
                gate = gateObject.transform;
            }
        }
    }

    public void EnemyAttack()
    {
        if (playerHP != null && playerInSight)
        {
            playerHP.TakeDamage(damage);
        }
        if (towerHealth != null && towerInSight)
        {
            towerHealth.TakeDamage(damage);
        }
    }

    public void Hit()
    {
        // Call the Flip function when the enemy is hit
        Flip();

        // Check for stun chance
        if (Random.value < 0.15f)
        {
            // Trigger the stunned bounce back effect
            if (stunnedBounceBackScript != null)
            {
                stunnedBounceBackScript.BounceBack();
            }

            // Resize the collider when stunned
            ResizeCollider(new Vector2(1.5f, 1.5f)); // Example values, adjust as needed

            // Start coroutine to unstun after duration
            StartCoroutine(UnstunAfterDelay(1f)); // Example duration, adjust as needed
        }

        // Pause any ongoing movement
        rb.velocity = Vector2.zero;
    }

    private IEnumerator UnstunAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        stunned = false;
        animator.SetTrigger("Unstunned");

        // Reset collider size after unstunned
        ResizeCollider(new Vector2(1f, 1f)); // Reset to original size, adjust as needed
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;

        // Adjust position after flip if necessary
        if (facingRight)
        {
            transform.position += new Vector3(1f, 0, 0);
        }
        else
        {
            transform.position += new Vector3(-1f, 0, 0);
        }
    }

    private void ResizeCollider(Vector2 newSize)
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            if (collider is BoxCollider2D boxCollider)
            {
                boxCollider.size = newSize;
                boxCollider.offset = new Vector2(0, -newSize.y / 2);
            }
            else if (collider is CircleCollider2D circleCollider)
            {
                circleCollider.radius = newSize.x / 2f;
                circleCollider.offset = new Vector2(0, -newSize.x / 2);
            }
            // Add other collider types as needed
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 detectionCenter = (Vector2)transform.position + new Vector2(radiusOffset * (facingRight ? 1 : -1), 0);
        Gizmos.DrawWireSphere(detectionCenter, detectionRadius);
    }
}
