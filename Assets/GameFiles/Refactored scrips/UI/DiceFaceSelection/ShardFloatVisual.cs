using UnityEngine;

public class ShardFloatVisual : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] float positionAmount = 0.05f;
    [SerializeField] float positionSpeed = 0.5f;
    [SerializeField] Vector3 rotationAmount = new Vector3(2, 2, 2);
    [SerializeField] float rotationSpeed = 0.8f;

    private float seed;
    private float rotationWeight = 1;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Awake()
    {
        startRotation = rect.localRotation;
        startPosition = rect.anchoredPosition;
        seed = Random.Range(0, 10000);
    }

    private void Update()
    {
        PerlinNoiseFloat();
    }

    private void PerlinNoiseFloat()
    {
        float time = Time.time;

        float x = (Mathf.PerlinNoise(seed, time * positionSpeed) - 0.5f) * 2;
        float y = (Mathf.PerlinNoise(seed + 1, time * positionSpeed) - 0.5f) * 2;

        Vector3 offset = new Vector3(x, y, 0) * positionAmount;
        rect.anchoredPosition = startPosition + offset;

        float xRotation = (Mathf.PerlinNoise(seed + 3, time * rotationSpeed) - 0.5f) * 2;
        float zRotation = (Mathf.PerlinNoise(seed + 5, time * rotationSpeed) - 0.5f) * 2;

        Quaternion randomRot = Quaternion.Euler(xRotation * rotationAmount.x * rotationWeight, 0, zRotation * rotationAmount.z * rotationWeight);

        rect.localRotation = startRotation * randomRot;
    }
}
