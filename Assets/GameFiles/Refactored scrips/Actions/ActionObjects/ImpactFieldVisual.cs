using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ImpactFieldVisual : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private MaterialPropertyBlock block;
    private Coroutine fadeRoutine;

    private Color color;
    private float chargeTime;
    private float radius;

    private bool flashRed;
    public bool hasBeenDestroyed;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        block = new MaterialPropertyBlock();
    }

    public void PassInValuesColorRadiusChargeTimeFlash(Color color, float radius, float chargeTime, bool flash)
    {
        hasBeenDestroyed = false;
        this.color = color;
        this.radius = radius;
        this.chargeTime = chargeTime;
        flashRed = flash;

        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            fadeRoutine = null;
        }

        Color startColor = color;
        startColor.a = 0f;
        SetColor(startColor);

        AdjustRadiusSize();
        fadeRoutine = StartCoroutine(ImpactFadeIn());
    }

    private void AdjustRadiusSize()
    {
        transform.localScale = new Vector3(1.0f, 0.2f, 1.0f);
        Vector3 tempScale = transform.localScale;
        tempScale.x = radius * 2;
        tempScale.z = radius * 2;
        transform.localScale = tempScale;
    }

    private IEnumerator ImpactFadeIn()
    {
        //Debug.Log("Fade in started");
        float timeElapsed = 0f;
        float a = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = transform.localScale;

        while (timeElapsed < chargeTime)
        {
            a = Mathf.Lerp(0f, 0.75f, easeOutBack(timeElapsed / chargeTime));

            Color color = this.color;

            color.a = a;
            SetColor(color);

            transform.localScale = Vector3.LerpUnclamped(startScale, endScale, easeOutBack(timeElapsed / chargeTime));

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.localScale = endScale;

        Color fullColor = color;
        fullColor.a = 1f;
        SetColor(fullColor);

        if (flashRed)
        {
            Color hitColor = Color.red;
            SetColor(hitColor);
        }

        //Debug.Log("Fade in ended");

        yield return new WaitForSeconds(0.4f);

        SetColor(fullColor);

        timeElapsed = 0f;

        while (timeElapsed < 0.4f)
        {
            float fadeA = Mathf.Lerp(1f, 0f, timeElapsed / 0.4f);

            Color color = this.color;

            color.a = fadeA;
            SetColor(color);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        fadeRoutine = null;
        DestroyMe();
    }

    private void SetColor(Color color)
    {
        meshRenderer.GetPropertyBlock(block);
        block.SetColor("_BaseColor", color);
        meshRenderer.SetPropertyBlock(block);
    }

    public void DestroyMe()
    {
        if (hasBeenDestroyed) return;
        hasBeenDestroyed = true;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }


    private float easeOutBack(float x)
    {
        const float c1 = 2.70158f;
        const float c3 = c1 + 1f;
        return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
    }

    public static float EaseInOutBack(float t)
    {
        const float c1 = 2.70158f;
        const float c2 = c1 * 1.525f;
        float t2 = t - 1f;
        return t < 0.5
            ? t * t * 2 * ((c2 + 1) * t * 2 - c2)
            : t2 * t2 * 2 * ((c2 + 1) * t2 * 2 + c2) + 1;
    }
}
