using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 30, -30);
    [SerializeField] private float speed = 5f;
    private Quaternion rotation;

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0f;
    private Vector3 desiredPosition;

    private void OnEnable()
    {
        JumpAction.ShakeScreen += AddScreenShake;

        Initialize();
    }

    private void OnDisable()
    {
        JumpAction.ShakeScreen -= AddScreenShake;
    }

    private void Initialize()
    {
        if (!target)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        transform.position = target.position + offset;
    }

    void LateUpdate()
    {
        desiredPosition = target.position + offset;

        if (shakeDuration > 0)
        {
            Vector3 shakeOffset = UnityEngine.Random.insideUnitSphere * shakeMagnitude;
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
