using UnityEngine;

public class AmbientLight : MonoBehaviour
{
    [SerializeField] private Light pointLight;
    [SerializeField] private float lightVariance = 1000;
    [SerializeField] private float rangeVariance = 20;

    private float intensity = 0;
    private float range = 0;

    private float seed = 0;

    private void Start()
    {
        intensity = pointLight.intensity;
        range = pointLight.range;
        seed = Random.Range(0, 10000);
    }

    private void Update()
    {
        VaryLight();
    }

    private void VaryLight()
    {
        float time = Time.time;

        float x = (Mathf.PerlinNoise(seed, time/10) - 0.5f) * lightVariance;
        float y = (Mathf.PerlinNoise(seed+1, time/10) - 0.5f) * rangeVariance;

        pointLight.intensity = x + intensity;
        pointLight.range = y + range;
    }
}