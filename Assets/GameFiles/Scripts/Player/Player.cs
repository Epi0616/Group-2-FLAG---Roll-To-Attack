using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private InputActionReference move, attack;
    private Vector3 moveDirection, desiredPosition;
    bool attacking = false;

    private void OnEnable()
    {
        move.action.Enable();
        attack.action.Enable();
    }

    private void OnDisable()
    {
        move.action.Disable();
        attack.action.Disable();
    }

    void Update()
    {
        MovePlayer();
        Attack();

        

        if (transform.position.y >= 9.99)
        {
            var pos = transform.position;
            pos.y = 10;
            transform.position = pos;
            desiredPosition = transform.position - new Vector3(0, 10, 0);
        }

        if (attacking && transform.position.y <= 0.01)
        { 
            var pos = transform.position;
            pos.y = 0;
            transform.position = pos;
        }
    }

    private void FixedUpdate()
    {
        if (attacking)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 10f * Time.deltaTime);
        }
        else 
        {
            rb.MovePosition(transform.position + moveDirection * Time.deltaTime * 5f);
        }
    }

    private void MovePlayer()
    {
        if (move.action.IsPressed())
        {
            moveDirection = move.action.ReadValue<Vector3>();
            return;
        }

        moveDirection = new(0, 0, 0);
    }

    private void Attack()
    {
        if (attack.action.WasPressedThisFrame())
        {
            Debug.Log("attack");
            desiredPosition = transform.position + new Vector3(0, 10, 0);
            attacking = true;  
        }
    }
}
