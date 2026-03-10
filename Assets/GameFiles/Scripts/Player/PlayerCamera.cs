using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 15, -15);
    [SerializeField] private float speed = 5f;

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0f;
    private Vector3 desiredPosition;

    private void OnEnable()
    {
        PlayerStateController.ShakeScreen += AddScreenShake;
    }

    private void OnDisable()
    {
        PlayerStateController.ShakeScreen -= AddScreenShake;
    }

    void LateUpdate()
    {
        desiredPosition = target.position + offset;


        if (shakeDuration > 0)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            desiredPosition += shakeOffset;

            shakeDuration -= Time.deltaTime;
        }

        transform.position = Vector3.Lerp(transform.position, desiredPosition, speed * Time.deltaTime);
    }

    private void AddScreenShake(float magnitude)
    {
        shakeDuration = magnitude / 10;
        shakeMagnitude = magnitude;
    }
}
