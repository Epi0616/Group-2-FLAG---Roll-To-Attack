using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class UpdatedImpactFieldVisual : ImpactFieldVisual
{ 
    [SerializeField] GameObject ringObj;
    [SerializeField] GameObject fieldObj;
    [SerializeField] private bool ScaleField = true;
    [SerializeField] private bool ScaleRing = false;
    [SerializeField] private bool invertGrowDirection = false;
    [SerializeField] GameObject fullRingObj;
    [SerializeField] private bool UsesTwoRings = false;
    [SerializeField] protected MeshRenderer secondRingRenderer;
    protected override void Awake()
    {
        ringMeshRenderer = ringObj.GetComponent<MeshRenderer>();
        meshRenderer = fieldObj.GetComponent<MeshRenderer>();
        block = new MaterialPropertyBlock();
    }

    public override void PassInValuesColorRadiusChargeTimeFlash(Color color, float radius, float chargeTime, bool flash)
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

        if (ScaleRing)
        {
            fadeRoutine = StartCoroutine(RingFadeIn());
        }
        else
        {
            fadeRoutine = StartCoroutine(ImpactFadeIn());
        }
        
    }

    protected override void AdjustRadiusSize()
    {
        fieldObj.transform.localScale = new Vector3(1.0f, 0.1f, 1.0f);
        Vector3 tempScale = transform.localScale;
        tempScale.x = radius * 2;
        tempScale.z = radius * 2;
        fieldObj.transform.localScale = tempScale;
        tempScale.x = radius * 2 * 0.125f;
        tempScale.z = radius * 2 * 0.125f;
        ringObj.transform.localScale = tempScale;
        if (UsesTwoRings && fullRingObj != null)
        {
            tempScale.x = radius * 2 * 0.125f;
            tempScale.z = radius * 2 * 0.125f;
            fullRingObj.transform.localScale = tempScale;
        }
    }

    protected override void SetColor(Color color)
    {

        Color darkerColour = new Color(color.r * 0.7f, color.g * 0.7f, color.b * 0.7f, color.a);
        Color lighterColour = new Color(color.r * 1.2f, color.g * 1.2f, color.b * 1.2f, color.a);

        if (color.a < 0f) { color.a = 0; }
        else if (color.a > 1f) { color.a = 1f; }
        if (usesRing)
        {
            ringMeshRenderer.GetPropertyBlock(block);
            block.SetColor("_RingColour", lighterColour);
            block.SetFloat("_Opacity", color.a);
            ringMeshRenderer.SetPropertyBlock(block);

        }
        if (usesField)
        {
            meshRenderer.GetPropertyBlock(block);
            block.SetColor("_BaseColor", darkerColour);
            meshRenderer.SetPropertyBlock(block);
        }
        if (UsesTwoRings && fullRingObj != null)
        {
            secondRingRenderer.GetPropertyBlock(block);
            block.SetColor("_RingColour", lighterColour);
            block.SetFloat("_Opacity", color.a);
            secondRingRenderer.SetPropertyBlock(block);
        }

    }

    protected IEnumerator RingFadeIn()
    {
        //Debug.Log("Fade in started");
        float timeElapsed = 0f;
        float a = 0f;
        Vector3 startScale;
        Vector3 endScale;
        if (invertGrowDirection)
        {
            startScale = ringObj.transform.localScale;
            endScale = ringObj.transform.localScale * 0.05f;
        }
        else
        {
            startScale = Vector3.zero;
            endScale = ringObj.transform.localScale;
        }
        

        while (timeElapsed < chargeTime)
        {
            a = Mathf.Lerp(0f, 0.75f, easeOutBack(timeElapsed / chargeTime));

            Color color = this.color;

            color.a = a;
            SetColor(color);
            ringObj.transform.localScale = Vector3.Lerp(startScale, endScale, (timeElapsed / chargeTime));

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        ringObj.transform.localScale = endScale;

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
            float t = Mathf.Clamp01(timeElapsed / 0.4f);
            float fadeA = Mathf.Lerp(1f, 0f, t);

            Color color = this.color;

            color.a = fadeA;
            SetColor(color);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        fadeRoutine = null;
        DestroyMe();
    }


    protected override IEnumerator ImpactFadeIn()
    {
        //Debug.Log("Fade in started");
        float timeElapsed = 0f;
        float a = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = fieldObj.transform.localScale;

        while (timeElapsed < chargeTime)
        {
            a = Mathf.Lerp(0f, 0.75f, easeOutBack(timeElapsed / chargeTime));

            Color color = this.color;

            color.a = a;
            SetColor(color);
            fieldObj.transform.localScale = Vector3.Lerp(startScale, endScale, (timeElapsed / chargeTime));

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        fieldObj.transform.localScale = endScale;

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
            float t = Mathf.Clamp01(timeElapsed / 0.4f);
            float fadeA = Mathf.Lerp(1f, 0f, t);

            Color color = this.color;

            color.a = fadeA;
            SetColor(color);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        fadeRoutine = null;
        DestroyMe();
    }
}
