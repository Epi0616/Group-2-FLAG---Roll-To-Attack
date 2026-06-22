using TMPro;
using UnityEngine;

public class TextDisplaySystem : MonoBehaviour, IEntitySystem
{
    public Entity OwnerEntity { get; set; }
    public Camera targetCamera;
    [SerializeField] private GameObject textPrefab;
    public void InitialiseSystem(Entity entity)
    {
        OwnerEntity = entity;
        // Give the Camera Reference to the TextDisplaySystem
    }

    public void ResetSystem() { }

    public void DisplayText(string text, Color color, int fontSize)
    {
        if (targetCamera == null)
        {
            targetCamera = GameObject.FindGameObjectWithTag("Player Camera").GetComponent<Camera>();
        }
        Vector3 randomOffset = new(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(8f, 10f), UnityEngine.Random.Range(-3f, 3f));
        GameObject TextObj = ObjectPoolManager.SpawnObject(textPrefab, OwnerEntity.transform.position + randomOffset, Quaternion.identity);
        TextObj.GetComponent<FloatingDamageText>().Initialize(targetCamera);
        TextMeshPro tempTMPAccess = TextObj.GetComponent<TextMeshPro>();
        tempTMPAccess.text = text;
        color.a = 1f;
        tempTMPAccess.color = color;
        tempTMPAccess.fontSize = fontSize;
    }

    public void DisplayHigherText(string text, Color color, int fontSize)
    {
        if (targetCamera == null)
        {
            targetCamera = GameObject.FindGameObjectWithTag("Player Camera").GetComponent<Camera>();
        }
        Vector3 randomOffset = new(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(8f, 10f), UnityEngine.Random.Range(0f, 6f));
        GameObject TextObj = ObjectPoolManager.SpawnObject(textPrefab, OwnerEntity.transform.position + randomOffset, Quaternion.identity);
        TextObj.GetComponent<FloatingDamageText>().Initialize(targetCamera);
        TextMeshPro tempTMPAccess = TextObj.GetComponent<TextMeshPro>();
        tempTMPAccess.text = text;
        color.a = 1f;
        tempTMPAccess.color = color;
        tempTMPAccess.fontSize = fontSize;
    }

}
