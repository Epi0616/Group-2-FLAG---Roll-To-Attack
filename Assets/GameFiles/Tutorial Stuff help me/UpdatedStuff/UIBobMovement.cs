using UnityEngine;

public class UIBobMovement : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] float positionAmount = 0.05f;
    [SerializeField] float positionSpeed = 0.5f;
    [SerializeField] float Xmult = 0.5f, Ymult = 2f;
    [SerializeField] private bool noise;
    private float seed;
    private Vector3 startPosition;


    private void Awake()
    {
        startPosition = rect.anchoredPosition;
        seed = Random.Range(0, 10000);
    }

    private void Update()
    {
        PerlinNoiseFloat();
    }

    public void UpdateStartPos(Vector3 pos) { startPosition = pos; }

    private void PerlinNoiseFloat()
    {
        float time = Time.time;
        float x, y;
        if (noise)
        {
            x = (Mathf.PerlinNoise(seed, time * positionSpeed) - 0.5f) * Xmult;
            y = (Mathf.PerlinNoise(seed + 1, time * positionSpeed) - 0.5f) * Ymult;
        }
        else
        {
            x = (Mathf.Sin(time) * positionSpeed) * Xmult;
            y = (Mathf.Cos(time) * positionSpeed) * Ymult;
        }

        

        Vector3 offset = new Vector3(x, y, 0) * positionAmount;
        rect.anchoredPosition = startPosition + offset;
    }
}
