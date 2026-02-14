using TMPro;
using UnityEngine;

public class EnemyBaseClass : MonoBehaviour
{
    //// code added by matt to show damage text
    //[SerializeField] protected GameObject damageText;

    //protected int maxHealth, currentHealth;
    //protected float moveSpeed;
    //protected float attackRange;
    //protected float knockbackWeightModifier;
    //public bool canMove;
    //public Rigidbody rb;
    //public Transform playerTransform;

    //private IEnemyState currentState;

    //protected EnemyBaseClass()
    //{
    //    maxHealth = 100;
    //    currentHealth = maxHealth;
    //    moveSpeed = 10f;
    //    knockbackWeightModifier = 1f;
    //    attackRange = 2.5f;
    //    canMove = true;
    //}
    //private void Awake()
    //{
    //    rb = GetComponent<Rigidbody>();
    //    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    //}

    //protected void Start()
    //{
    //    ChangeState(new EnemyMoveState(this));
    //}

    //protected virtual void Update()
    //{
    //    currentState?.UpdateState();
    //}
    //protected virtual void FixedUpdate()
    //{
    //    currentState?.FixedUpdateState();
    //}

    //public virtual void MoveTowardsPlayer()
    //{
    //    Vector3 targetVector = (playerTransform.position - transform.position);
    //    Vector3 targetDirection = targetVector.normalized;
    //    targetDirection.y = 0;
    //    if (targetVector.magnitude < attackRange)
    //    {
    //        rb.linearVelocity = Vector3.zero;
    //        return;
    //    }
    //    rb.linearVelocity = targetDirection * moveSpeed;
    //}

    //public virtual void OnTakeDamage(int amount)
    //{
    //    currentHealth -= amount;

    //    ShowDamage(amount);

    //    if (currentHealth <= 0)
    //    {
    //        OnDeath();
    //    }
    //}

    //public virtual void OnTakeKnockback(float knockbackForce)
    //{
    //    Vector3 targetVector = (transform.position - playerTransform.position);
    //    Vector3 targetDirection = targetVector.normalized;
    //    rb.AddForce(targetDirection * (knockbackForce * knockbackWeightModifier), ForceMode.Impulse);
    //}

    //public virtual void OnDeath()
    //{
    //    // ADD EVENT INVOKE FOR DEATH SO SPAWNER MANAGER KNOWS AN ENEMY DIED TO KEEP TRACK OF HOW MANY ENEMIES ARE LEFT IN CURRENT WAVE
    //    Destroy(this.gameObject);
    //}

    //public void RequestStateChange(EnemyBaseState newState)
    //{
    //    ChangeState(newState);
    //}

    ////protected virtual void ChangeState(EnemyBaseState newState)
    ////{
    ////    if (currentState == newState)
    ////    {
    ////        return;
    ////    }
    ////    currentState?.ExitState();
    ////    currentState = newState;
    ////    currentState?.EnterState();
    ////}

    //// code added by matt to show damage text
    //protected void ShowDamage(int damage)
    //{
    //    Vector3 randomOffset = new(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    //    GameObject damageNumber = Instantiate(damageText, rb.position + randomOffset, Quaternion.identity);
    //    damageNumber.GetComponent<TextMeshPro>().text = damage.ToString();
    //}
}
