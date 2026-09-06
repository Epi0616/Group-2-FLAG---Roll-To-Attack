using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FloatingDamageText : MonoBehaviour
{
    [SerializeField] private Renderer myRenderer;
    [SerializeField] private TextMeshPro tmp;
    private Camera targetCamera;
    private float lifeTime = 2f;
    private Vector3 originalScale;

    //set up initialize once enemy spawner is working properly
    public void Initialize(Camera camera, string text, Color color, int fontSize)
    {
        targetCamera = camera;
        transform.localScale = originalScale;
        tmp.text = text;
        color.a = 1f;
        tmp.color = color;
        tmp.fontSize = fontSize;
        StartCoroutine(DestroyRoutine());
    }

    private void Awake()
    {
        originalScale = transform.localScale;
        myRenderer.material.renderQueue = 100;
        myRenderer.enabled = false;
        myRenderer.enabled = true;
    }

    private void Update()
    {      
        if (targetCamera == null) return;
        transform.rotation = targetCamera.transform.rotation;
        transform.position += Vector3.up * Time.deltaTime * 3f;
        transform.localScale *= 0.999f;
    }

    private IEnumerator DestroyRoutine()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject, lifeTime);
        yield return null;
    }
}
