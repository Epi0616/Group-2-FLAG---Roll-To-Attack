using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class DicePedestal : MonoBehaviour
{
    public static Action<float> WaveStartPedestal;
    [SerializeField] GameObject diceBody;
    [SerializeField] float positionAmount = 0.05f;
    [SerializeField] float positionSpeed = 0.5f;
    [SerializeField] Vector3 rotationAmount = new Vector3(2, 2, 2);
    [SerializeField] float rotationSpeed = 0.8f;
    [SerializeField] float maxSpinSpeed = 1000;

    private bool autoStart = false;
    private static bool autoStartHandled = false;

    private Vector3 startPosition;
    private Quaternion startRotation, targetRotation;

    private float seed;
    private bool activated = false;
    private static bool waveStarted = false;
    private float rotationWeight = 1;
    private float timeBetweenWaves = 5;

    private void OnEnable()
    {
        WaveStartPedestal += HandleWaveStarted;
        DiceFaceSelectionUIManager.DiceFaceSelectionOver += HandleDiceSelectionPhaseOver;
        GameSettings.autoStart += HandleAutoStart;
    }

    private void OnDisable()
    {
        WaveStartPedestal -= HandleWaveStarted;
        DiceFaceSelectionUIManager.DiceFaceSelectionOver -= HandleDiceSelectionPhaseOver;
        GameSettings.autoStart -= HandleAutoStart;
    }

    private void Start()
    {
        startPosition = diceBody.transform.localPosition;
        startRotation = diceBody.transform.localRotation;

        seed = Random.Range(0, 10000);
    }

    private void Update()
    {
        PerlinNoiseBob();
    }

    private void PerlinNoiseBob()
    {
        float time = Time.time;

        float x = (Mathf.PerlinNoise(seed, time * positionSpeed) - 0.5f) * 2;
        float y = (Mathf.PerlinNoise(seed + 1, time * positionSpeed) - 0.5f) * 2;
        float z = (Mathf.PerlinNoise(seed + 2, time * positionSpeed) - 0.5f) * 2;

        Vector3 offset = new Vector3(x, y, z) * positionAmount;
        diceBody.transform.localPosition = startPosition + offset;

        float xRotation = (Mathf.PerlinNoise(seed + 3, time * rotationSpeed) - 0.5f) * 2;
        float yRotation = (Mathf.PerlinNoise(seed + 4, time * rotationSpeed) - 0.5f) * 2;
        float zRotation = (Mathf.PerlinNoise(seed + 5, time * rotationSpeed) - 0.5f) * 2;

        Quaternion randomRot = Quaternion.Euler(xRotation * rotationAmount.x * rotationWeight, yRotation * rotationAmount.y * rotationWeight, zRotation * rotationAmount.z * rotationWeight);

        diceBody.transform.localRotation = startRotation * randomRot;
    }

    public void ActivatePedestal()
    {
        if (activated) return;
        activated = true;
        StartCoroutine(DiceHitReaction());
    }

    public void ActivatePedestalWithHeavy()
    {
        if (waveStarted) return;
        WaveStartPedestal?.Invoke(timeBetweenWaves);
        activated = true;
        StartCoroutine(DiceStartWaveReaction());
    }

    private IEnumerator DiceHitReaction()
    {
        rotationWeight = 0;
        float timer = 0;
        float x = Random.Range(-20, 20);
        float y = Random.Range(-20, 20);
        float z = Random.Range(-20, 20);
        targetRotation = startRotation * Quaternion.Euler(x, y, z);
        Quaternion beginRotation = diceBody.transform.localRotation;

        while (timer < 3)
        { 
            timer += Time.deltaTime;

            ApplyRotation(timer / 3, beginRotation, targetRotation);
            yield return null;
        }

        diceBody.transform.localRotation = targetRotation;
        startRotation = targetRotation;

        float blendTimer = 0;
        while (blendTimer < 0.4)
        {
            blendTimer += Time.deltaTime;
            rotationWeight = Mathf.SmoothStep(0, 1, blendTimer / 0.4f);
            yield return null;
        }

        rotationWeight = 1;
        activated = false;
    }

    private IEnumerator DiceStartWaveReaction()
    {
        rotationWeight = 0;
        float timer = 0;

        float x = Random.Range(-20, 20) * 10;
        float y = Random.Range(-20, 20) * 10;
        float z = Random.Range(-20, 20) * 10;
        targetRotation = startRotation * Quaternion.Euler(x, y, z);
        Quaternion beginRotation = diceBody.transform.localRotation;

        while (timer < 6)
        {
            timer += Time.deltaTime;

            ApplyRotationFast(timer / 6, beginRotation, targetRotation);
            yield return null;
        }

        diceBody.transform.localRotation = targetRotation;
        startRotation = targetRotation;

        float blendTimer = 0;
        while (blendTimer < 0.4)
        {
            blendTimer += Time.deltaTime;
            rotationWeight = Mathf.SmoothStep(0, 1, blendTimer / 0.4f);
            yield return null;
        }

        rotationWeight = 1;
        activated = false;
    }

    private void ApplyRotation(float jumpProgress, Quaternion beginRotation, Quaternion targetRotation)
    {
        float t = Mathf.SmoothStep(0f, 1f, jumpProgress);

        Quaternion rotation = Quaternion.Slerp(beginRotation, targetRotation, t);
        diceBody.transform.localRotation = rotation;

        Quaternion visualSpin = Quaternion.Euler(360f * t, 360 * t, 360 * t);
        diceBody.transform.localRotation *= visualSpin;
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

    private void HandleAutoStart(bool autoStart)
    {
        this.autoStart = autoStart;
    }

    private void HandleWaveStarted(float time)
    {
        waveStarted = true;
    }

    private IEnumerator DelayWaveStarted(float time)
    {
        yield return new WaitForSecondsRealtime(0.1f);

        autoStartHandled = false;
        WaveStartPedestal?.Invoke(time);
    }

    private void HandleDiceSelectionPhaseOver(float time)
    {
        if (autoStart)
        {
            if (autoStartHandled) return;
            autoStartHandled = true;
            StartCoroutine(DelayWaveStarted(time));
            return;
        }

        timeBetweenWaves = time;
        waveStarted = false;
    }
}
