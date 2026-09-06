using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class DicePedestal : MonoBehaviour
{
    public static event Action<float> WaveHeavyStartPedestal;
    public static event Action<float> WaveAutoStartPedestal;
    public static event Action<float> ChargeTextAppear;

    [SerializeField] GameObject diceBody;
    [SerializeField] float positionAmount = 0.05f;
    [SerializeField] float positionSpeed = 0.5f;
    [SerializeField] Vector3 rotationAmount = new Vector3(2, 2, 2);
    [SerializeField] float rotationSpeed = 0.8f;
    [SerializeField] float maxSpinSpeed = 1000;

    [SerializeField] private Vector3 startPosition, targetPosition;
    [SerializeField] private float timeBetweenWaves = 2;

    private Vector3 diceStartPosition;
    private Quaternion diceStartRotation, diceTargetRotation;

    private bool autoStart = false;

    private float seed;
    private static bool waveStarted = false;
    private float rotationWeight = 1;
    

    private void OnEnable()
    {
        DiceFaceSelectionUIManager.DiceFaceSelectionOver += HandleDiceSelectionPhaseOver;
    }

    private void OnDisable()
    {
        DiceFaceSelectionUIManager.DiceFaceSelectionOver -= HandleDiceSelectionPhaseOver;
    }

    private void Awake()
    {
        waveStarted = false;
    }

    private void Start()
    {
        diceStartRotation = diceBody.transform.localRotation;
        diceStartPosition = diceBody.transform.localPosition;
        transform.localPosition = startPosition;

        seed = Random.Range(0, 10000);
    }

    private void Update()
    {
        PerlinNoiseBob();
    }

    private void HandleAutoStart(bool autoStart)
    {
        this.autoStart = autoStart;
    }

    private void HandleDiceSelectionPhaseOver(float time)
    {
        if (autoStart)
        {
            WaveAutoStartPedestal?.Invoke(0);
            waveStarted = true;
            return;
        }

        timeBetweenWaves = time;
        waveStarted = false;
        ChargeTextAppear?.Invoke(time);
        StartCoroutine(MoveToFrom(startPosition, targetPosition, 5f));
    }

    public void ActivatePedestalWithHeavy()
    {
        if (waveStarted) return;
        waveStarted = true;

        WaveHeavyStartPedestal?.Invoke(timeBetweenWaves);
        autoStart = true;
        StartCoroutine(MoveToFrom(targetPosition, startPosition, 5));
        StartCoroutine(DiceStartWaveReaction());
    }

    private void PerlinNoiseBob()
    {
        float time = Time.time;

        float x = (Mathf.PerlinNoise(seed, time * positionSpeed) - 0.5f) * 2;
        float y = (Mathf.PerlinNoise(seed + 1, time * positionSpeed) - 0.5f) * 2;
        float z = (Mathf.PerlinNoise(seed + 2, time * positionSpeed) - 0.5f) * 2;

        Vector3 offset = new Vector3(x, y, z) * positionAmount;
        diceBody.transform.localPosition = diceStartPosition + offset;

        float xRotation = (Mathf.PerlinNoise(seed + 3, time * rotationSpeed) - 0.5f) * 2;
        float yRotation = (Mathf.PerlinNoise(seed + 4, time * rotationSpeed) - 0.5f) * 2;
        float zRotation = (Mathf.PerlinNoise(seed + 5, time * rotationSpeed) - 0.5f) * 2;

        Quaternion randomRot = Quaternion.Euler(xRotation * rotationAmount.x * rotationWeight, yRotation * rotationAmount.y * rotationWeight, zRotation * rotationAmount.z * rotationWeight);

        diceBody.transform.localRotation = diceStartRotation * randomRot;
    }

    private IEnumerator DiceStartWaveReaction()
    {
        rotationWeight = 0;
        float timer = 0;

        float x = Random.Range(-20, 20) * 10;
        float y = Random.Range(-20, 20) * 10;
        float z = Random.Range(-20, 20) * 10;
        diceTargetRotation = diceStartRotation * Quaternion.Euler(x, y, z);
        Quaternion beginRotation = diceBody.transform.localRotation;

        while (timer < 6)
        {
            timer += Time.deltaTime;

            ApplyRotationFast(timer / 6, beginRotation, diceTargetRotation);
            yield return null;
        }

        diceBody.transform.localRotation = diceTargetRotation;
        diceStartRotation = diceTargetRotation;

        float blendTimer = 0;
        while (blendTimer < 0.4)
        {
            blendTimer += Time.deltaTime;
            rotationWeight = Mathf.SmoothStep(0, 1, blendTimer / 0.4f);
            yield return null;
        }

        rotationWeight = 1;
    }

    private void ApplyRotationFast(float jumpProgress, Quaternion beginRotation, Quaternion targetRotation)
    {
        float t = Mathf.Clamp01(jumpProgress);
        float rotationPercent = Mathf.SmoothStep(0, 1, t);

        Quaternion rotation = Quaternion.Slerp(beginRotation, targetRotation, rotationPercent);

        float spinCurve = Mathf.Sin(t * Mathf.PI);
        spinCurve = Mathf.SmoothStep(0, 1, spinCurve);

        float totalSpinAngle = 360f * 12f;
        float spinAngle = totalSpinAngle * rotationPercent;

        Quaternion visualSpin = Quaternion.Euler(spinAngle, spinAngle, spinAngle);
        diceBody.transform.localRotation = rotation * visualSpin;
    }

    private IEnumerator MoveToFrom(Vector3 to, Vector3 from, float duration)
    {
        float timer = 0;
        float t = 0;

        while (timer < duration)
        { 
            timer += Time.deltaTime;
            t = timer / duration;

            transform.localPosition = Vector3.Lerp(from, to, t);
            yield return null;
        }

        transform.localPosition = to;
    }
}
