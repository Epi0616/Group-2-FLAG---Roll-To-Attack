using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 15, -15);
    [SerializeField] private float speed = 5f;

    Vector3 desiredPosition;

    void LateUpdate()
    {
        desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);
    }
}
