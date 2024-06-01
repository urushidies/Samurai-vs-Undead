using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class BlackSamuraiController : MonoBehaviour
{
    private SpriteRenderer spriteRendererCol;
    private BoxCollider2D boxColliderCol;
    private Rigidbody2D rb;
    private Animator anim;

    private float xAxis, yAxis;
    private bool isRunning;
    private bool facingRight = true;

    [Header("Horizontal movement settings")]
    [SerializeField] private float walkSpeed = 1;
    [SerializeField] private float runMultiplier = 1.4f;

    [Header("Ground Checker")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask whatIsGround;

    private PlayerHealth playerHPcontrolling;
    private float lastAttackInputTime;
    [SerializeField] private float attackInputBuffer = 0.5f; // attack input buffer duration
    private float lastAttackTime;

    public BloodEffect bloodvfx;
    public float bloodEffectOffsetX = 0.5f;
    public float bloodEffectOffsetY = 0.3f;

    [SerializeField] private int damage = 3;
    public bool EnemyAtTheAttackArea;
    private bool isAttacking1;
    private bool isAttacking2;
    private bool isAttacking3;
    private int currentAttack = 1;
    [SerializeField] private Transform sideAttackTransform;
    [SerializeField] private Vector2 sideAttackArea;
    [SerializeField] private float radius = 1f;
    [SerializeField] private LayerMask attackableLayer; // Make sure this includes the Enemy layer
    

    private Vector2 savedVelocity; // Для сохранения скорости ГГ перед заморозкой

    private void Awake()
    {
        spriteRendererCol = GetComponent<SpriteRenderer>();
        boxColliderCol = GetComponent<BoxCollider2D>();
        gameObject.layer = LayerMask.NameToLayer("Player"); // Ensure player is on the "Player" layer

        // Initialize groundCheckPoint
        groundCheckPoint = new GameObject("GroundCheckPoint").transform;
        groundCheckPoint.SetParent(transform);
        groundCheckPoint.localPosition = new Vector3(0f, -boxColliderCol.size.y * 0.5f, 0f); // Set at mid-bottom of collider
    }
    private void Start()
    {
        // Получаем компоненты AudioSource
       
        
        playerHPcontrolling = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartCoroutine(ResizeColliderSmoothly());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sideAttackTransform.position, radius);
    }

    private void Update()
    {
        if (!playerHPcontrolling.isAlive) return;

        GetInput();
        CheckRunning();
        Move();
        HandleAttacks();
        UpdateAnimations();
    }

    private IEnumerator ResizeColliderSmoothly()
    {
        while (true)
        {
            if (spriteRendererCol.sprite == null) yield return null;

            Vector2 targetSize = spriteRendererCol.sprite.bounds.size;
            Vector2 targetOffset = spriteRendererCol.sprite.bounds.center;

            boxColliderCol.size = Vector2.Lerp(boxColliderCol.size, targetSize, Time.deltaTime * 10); // Adjust smoothing speed as needed
            boxColliderCol.offset = Vector2.Lerp(boxColliderCol.offset, targetOffset, Time.deltaTime * 10); // Adjust smoothing speed as needed

            yield return null; // Wait for the next frame
        }
    }

    private void GetInput()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            lastAttackInputTime = Time.time;
        }
    }

    private void HandleAttacks()
    {
        EnemyAtTheAttackArea = false;
        if (Time.time - lastAttackInputTime <= attackInputBuffer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (currentAttack == 1 && !isAttacking1)
                {
                    ResetAttackTriggers();
                    anim.SetTrigger("Attack1");
                    lastAttackTime = Time.time;
                    isAttacking1 = true;

                    Invoke("TriggerSideAttack", 0.2f);
                    FreezePlayerPosition(); // Замораживаем позицию ГГ при атаке
                    UpdateCurrentAttack();
                }
                else if (currentAttack == 2 && !isAttacking2)
                {
                    ResetAttackTriggers();
                    anim.SetTrigger("Attack2");
                    lastAttackTime = Time.time;
                    isAttacking2 = true;

                    Invoke("TriggerSideAttack", 0.2f);
                    FreezePlayerPosition(); // Замораживаем позицию ГГ при атаке
                    UpdateCurrentAttack();
                }
                else if (currentAttack == 3 && !isAttacking3)
                {
                    ResetAttackTriggers();
                    anim.SetTrigger("Attack3");
                    lastAttackTime = Time.time;
                    isAttacking3 = true;

                    Invoke("TriggerSideAttack", 0.2f);
                    FreezePlayerPosition(); // Замораживаем позицию ГГ при атаке
                    UpdateCurrentAttack();
                }
            }
        }
    }

    private void FreezePlayerPosition()
    {
        savedVelocity = rb.velocity; // Сохраняем текущую скорость ГГ
        rb.velocity = Vector2.zero; // Замораживаем скорость
        rb.bodyType = RigidbodyType2D.Static; // Замораживаем физическое поведение
    }

    private void UnfreezePlayerPosition()
    {
        rb.bodyType = RigidbodyType2D.Dynamic; // Восстанавливаем физическое поведение
        rb.velocity = savedVelocity; // Восстанавливаем сохраненную скорость
    }

    private void TriggerSideAttack()
    {
        Hit(sideAttackTransform, sideAttackArea);
    }

    private void Hit(Transform _attackTransform, Vector2 _attackArea)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapCircleAll(_attackTransform.position, radius, attackableLayer);
        Debug.Log($"Hit detected. Number of objects hit: {objectsToHit.Length}");

        foreach (Collider2D collider in objectsToHit)
        {
            EnemyHealth enemyHealthComponent = collider.GetComponent<EnemyHealth>();
            if (enemyHealthComponent != null)
            {
                Debug.Log($"Enemy hit: {collider.gameObject.name}, applying damage: {damage}");
                enemyHealthComponent.TakeDamageEnemy(damage);

                if (bloodvfx != null)
                {
                    
                    Vector3 spawnPosition = _attackTransform.position;
                    spawnPosition.x -= bloodEffectOffsetX;
                    spawnPosition.y += bloodEffectOffsetY;
                    bloodvfx.PlayBloodEffect(spawnPosition);
                }
                else
                {
                    Debug.LogError("BloodEffect is not assigned!");
                }
            }
            else
            {
               
                Debug.Log($"No EnemyHealth component found on {collider.gameObject.name}");
            }
        }
    }

    private void UpdateCurrentAttack()
    {
        currentAttack++;
        if (currentAttack > 3)
        {
            currentAttack = 1;
        }

        isAttacking1 = false;
        isAttacking2 = false;
        isAttacking3 = false;

        UnfreezePlayerPosition(); // Восстанавливаем позицию ГГ после атаки
    }

    private void ResetAttackTriggers()
    {
        anim.ResetTrigger("Attack1");
        anim.ResetTrigger("Attack2");
        anim.ResetTrigger("Attack3");
    }

    private void Move()
    {
        float speed = isRunning ? walkSpeed * runMultiplier : walkSpeed;
        rb.velocity = new Vector2(speed * xAxis, rb.velocity.y);

        // Flip the player's direction based on movement
        if (xAxis > 0 && !facingRight)
        {
            Flip();
        }
        else if (xAxis < 0 && facingRight)
        {
            Flip();
        }
    }

    public bool IsGrounded()
    {
        Collider2D groundCheckCollider = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);
        return groundCheckCollider != null;
    }

    private void CheckRunning()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    private void UpdateAnimations()
    {
        bool isMoving = Mathf.Abs(rb.velocity.x) > 0.1f && IsGrounded();
        bool isRunning = isMoving && this.isRunning;
        bool isWalking = isMoving && !isRunning;

        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);

        // Ensure attack triggers are reset based on the animation state
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && isAttacking1)
        {
            isAttacking1 = false;
        }
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && isAttacking2)
        {
            isAttacking2 = false;
        }
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3") && isAttacking3)
        {
            isAttacking3 = false;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
