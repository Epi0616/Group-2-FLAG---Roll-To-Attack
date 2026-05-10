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

    [SerializeField] Vector3 rotationAmount = new Vector3(2f, 2f, 2f);
    [SerializeField] float rotationSpeed = 0.8f;

    [SerializeField] float maxSpinSpeed = 1000f;
    private float currentSpinAngle;

    private Vector3 startPosition;
    private Quaternion startRotation, targetRotation;

    private float seed;
    private bool activated = false;
    private bool waveStarted = false;
    private float timeBetweenWaves = 5f;

    private void OnEnable()
    {
        WaveStartPedestal += HandleWaveStarted;
        DiceFaceSelectionUIManager.DiceFaceSelectionOver += HandleDiceSelectionPhaseOver;
    }

    private void OnDisable()
    {
        WaveStartPedestal -= HandleWaveStarted;
        DiceFaceSelectionUIManager.DiceFaceSelectionOver -= HandleDiceSelectionPhaseOver;
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

        float x = (Mathf.PerlinNoise(seed, time * positionSpeed) - 0.5f) * 2f;
        float y = (Mathf.PerlinNoise(seed + 1f, time * positionSpeed) - 0.5f) * 2f;
        float z = (Mathf.PerlinNoise(seed + 2f, time * positionSpeed) - 0.5f) * 2f;

        Vector3 offset = new Vector3(x, y, z) * positionAmount;
        diceBody.transform.localPosition = startPosition + offset;


        if (activated) return;
        float xRotation = (Mathf.PerlinNoise(seed + 3f, time * rotationSpeed) - 0.5f) * 2f;
        float yRotation = (Mathf.PerlinNoise(seed + 4f, time * rotationSpeed) - 0.5f) * 2f;
        float zRotation = (Mathf.PerlinNoise(seed + 5f, time * rotationSpeed) - 0.5f) * 2f;

        Quaternion randomRot = Quaternion.Euler(xRotation * rotationAmount.x, yRotation * rotationAmount.y, zRotation * rotationAmount.z);

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
        float timer = 0f;
        float x = Random.Range(-20f, 20f);
        float y = Random.Range(-20f, 20f);
        float z = Random.Range(-20f, 20f);
        targetRotation = startRotation * Quaternion.Euler(x, y, z);
        Quaternion beginRotation = startRotation;

        while (timer < 3)
        { 
            timer += Time.deltaTime;

            ApplyRotation(timer / 3f, beginRotation, targetRotation);
            yield return null;
        }

        startRotation = diceBody.transform.localRotation;
        activated = false;
    }

    private IEnumerator DiceStartWaveReaction()
    {
        float timer = 0f;
        currentSpinAngle = 0f;

        float x = Random.Range(-20f, 20f) * 10;
        float y = Random.Range(-20f, 20f) * 10;
        float z = Random.Range(-20f, 20f) * 10;
        targetRotation = startRotation * Quaternion.Euler(x, y, z);
        Quaternion beginRotation = startRotation;

        while (timer < 6)
        {
            timer += Time.deltaTime;

            ApplyRotationFast(timer / 6f, beginRotation, targetRotation);
            yield return null;
        }

        startRotation = diceBody.transform.localRotation;
        activated = false;
    }

    private void ApplyRotation(float jumpProgress, Quaternion beginRotation, Quaternion targetRotation)
    {
        float t = Mathf.SmoothStep(0f, 1f, jumpProgress);

        Quaternion rotation = Quaternion.Slerp(beginRotation, targetRotation, t);
        diceBody.transform.localRotation = rotation;

        Quaternion visualSpin = Quaternion.Euler(360f * t, 360f * t, 360f * t);
        diceBody.transform.localRotation *= visualSpin;
    }

    private void ApplyRotationFast(float jumpProgress, Quaternion beginRotation, Quaternion targetRotation)
    {
        float t = Mathf.Clamp01(jumpProgress);
        float rotationPercent = Mathf.SmoothStep(0f, 1f, t);

        Quaternion rotation = Quaternion.Slerp(beginRotation, targetRotation, rotationPercent);

        float spinCurve = Mathf.Sin(t * Mathf.PI);
        spinCurve = Mathf.SmoothStep(0f, 1f, spinCurve);

        currentSpinAngle += maxSpinSpeed * spinCurve * Time.deltaTime;

        Quaternion visualSpin = Quaternion.Euler(currentSpinAngle, currentSpinAngle * 0.7f, currentSpinAngle * 1.3f);
        diceBody.transform.localRotation = rotation * visualSpin;
    }

    private void HandleWaveStarted(float time)
    {
        waveStarted = true;
    }

    private void HandleDiceSelectionPhaseOver(float time)
    {
        timeBetweenWaves = time;
        waveStarted = false;
    }
}
