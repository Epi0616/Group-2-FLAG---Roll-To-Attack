using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    public Vector3 offset = new Vector3(0, 30, -30);
    [SerializeField] private float speed = 5f;
    private Quaternion rotation;

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0f;
    private Vector3 desiredPosition;
    private Vector3 startingOffset;
    private Vector3 zoomInOffset = new Vector3(0, 15, -12);

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
        startingOffset = offset;
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

    public IEnumerator ZoomIn(float duration)
    {
        Vector3 Start = startingOffset;
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / duration;
            t = Mathf.SmoothStep(0f, 1f, t);
            float y = Mathf.Lerp(Start.y, zoomInOffset.y, t);
            float z = Mathf.Lerp(Start.z, zoomInOffset.z, t);
            offset = new Vector3(0, y, z);
            yield return null;
        }
        offset = zoomInOffset;
    }

    public IEnumerator ZoomOut(float duration)
    {
        Vector3 Start = zoomInOffset;
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / duration;
            t = Mathf.SmoothStep(0f, 1f, t);
            float y = Mathf.Lerp(Start.y, startingOffset.y, t);
            float z = Mathf.Lerp(Start.z, startingOffset.z, t);
            offset = new Vector3(0, y, z);
            yield return null;
        }
        offset = startingOffset;
    }
}
