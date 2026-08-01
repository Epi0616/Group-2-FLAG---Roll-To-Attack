using System;
using System.Collections;
using UnityEditor.Build;
using UnityEditor.Localization.Reporting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    public Vector3 offset = new Vector3(0, 30, -30);
    [SerializeField] private float speed = 5f;
    private Quaternion startRotation;

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0f;
    private Vector3 desiredPosition;
    private Vector3 startingOffset;
    private Vector3 zoomInOffset = new Vector3(0, 15, -12);

    private void OnEnable()
    {
        JumpAction.ShakeScreen += AddScreenShake;
        FallFromTheSky.BossFallingFromSky += HandleTrackEnemy;

        Initialize();
    }

    private void OnDisable()
    {
        JumpAction.ShakeScreen -= AddScreenShake;
        FallFromTheSky.BossFallingFromSky -= HandleTrackEnemy;
    }


    private void Initialize()
    {
        if (!target)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        startRotation = transform.rotation;
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

    private void HandleTrackEnemy(float duration, Transform transform)
    {
        StartCoroutine(TrackEnemy(duration, transform));
    }

    private IEnumerator TrackEnemy(float duration, Transform EnemyTransform)
    {
        float fithTime = duration / 5;
        float timer = fithTime * 2;
        float t = 0;
        float easeOutT = 0;

        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = ((fithTime * 2) - timer) / (fithTime * 2);
            easeOutT = 1 - Mathf.Pow(1 - t, 2);

            Quaternion enemyRotation = Quaternion.LookRotation(EnemyTransform.position - transform.position);
            transform.rotation = Quaternion.Lerp(startRotation, enemyRotation, easeOutT);
            yield return null;
        }

        t = fithTime * 2;

        while (t > 0)
        { 
            t-= Time.deltaTime;
            Quaternion enemyRotation = Quaternion.LookRotation(EnemyTransform.position - transform.position);
            transform.rotation = enemyRotation;
            yield return null;
        }

        t = 0;
        timer = fithTime;
        Quaternion targetRotation = transform.rotation;

        while (t < 1)
        {
            timer -= Time.deltaTime;
            t = (fithTime - timer) / fithTime;

            transform.rotation = Quaternion.Lerp(targetRotation, startRotation, t);
            yield return null;
        }
    }
}
