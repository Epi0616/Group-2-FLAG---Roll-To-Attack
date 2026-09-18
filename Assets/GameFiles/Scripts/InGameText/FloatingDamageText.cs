using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FloatingDamageText : MonoBehaviour
{
    [SerializeField] private Renderer myRenderer;
    [SerializeField] private TextMeshPro tmp;
    private Camera targetCamera;
    private float lifeTime = 2.5f;
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
        StartCoroutine(FadeAway());
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
        //transform.localScale *= 0.999f;
        
    }

    private IEnumerator DestroyRoutine()
    {
        while (transform.localScale.x > 0.3f) { yield return null; }
        ObjectPoolManager.ReturnObjectToPool(gameObject);
        yield return null;
    }

    private IEnumerator FadeAway()
    {
        Vector3 startScale = originalScale;
        Vector3 endScale = Vector3.zero;
        float timer = 0;
        float t;
        while (timer < lifeTime)
        {
            timer += Time.deltaTime;
            t = timer / lifeTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
