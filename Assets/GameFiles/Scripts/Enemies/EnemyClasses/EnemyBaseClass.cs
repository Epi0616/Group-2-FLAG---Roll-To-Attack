using UnityEngine;

public class EnemyBaseClass : MonoBehaviour, IEnemy
{
    protected int maxHealth, currentHealth;
    protected float moveSpeed;
    protected float attackRange;
    protected float knockbackWeightModifier;
    [SerializeField] protected bool canMove;
    private Rigidbody rb;
    private Transform playerTransform;
    private bool hasBeenKnockedBack;

    protected EnemyBaseClass()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        moveSpeed = 10f;
        knockbackWeightModifier = 1f;
        attackRange = 2.5f;
        canMove = true;
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        if (hasBeenKnockedBack)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, 1.1f * Time.deltaTime);
            if (rb.linearVelocity.magnitude <= 2f)
            {
                hasBeenKnockedBack = false;
                canMove = true;
            }
        }
    }
    protected virtual void FixedUpdate()
    {
        if (canMove)
        {
            MoveTowardsPlayer();
        }
    }

    public virtual void MoveTowardsPlayer()
    {
        Vector3 targetVector = (playerTransform.position - transform.position);
        Vector3 targetDirection = targetVector.normalized;
        targetDirection.y = 0;
        if (targetVector.magnitude < attackRange)
        {
            rb.linearVelocity = Vector3.zero;
            //OnTakeKnockback(10f);
            return;
        }
        rb.linearVelocity = targetDirection * moveSpeed;
    }

    public virtual void OnTakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    public virtual void OnTakeKnockback(float knockbackForce)
    {
        canMove = false;
        hasBeenKnockedBack = true;
        Vector3 targetVector = (transform.position - playerTransform.position);
        Vector3 targetDirection = targetVector.normalized;
        rb.AddForce(targetDirection * (knockbackForce * knockbackWeightModifier), ForceMode.Impulse);
    }

    public virtual void OnDeath()
    {
        // ADD EVENT INVOKE FOR DEATH SO SPAWNER MANAGER KNOWS AN ENEMY DIED TO KEEP TRACK OF HOW MANY ENEMIES ARE LEFT IN CURRENT WAVE
        Destroy(this.gameObject);
    }
}
