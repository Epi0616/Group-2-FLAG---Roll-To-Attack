using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] float sens;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, 0);
    [SerializeField] private float speed = 5f;

    Vector3 desiredPosition;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        offset = Quaternion.Euler(0, Input.GetAxisRaw("Mouse X") * sens * Time.deltaTime, 0) * offset;
        //offset = Quaternion.Euler(Input.GetAxisRaw("Mouse Y") * sens * Time.deltaTime, 0) * offset;

        desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }
}
