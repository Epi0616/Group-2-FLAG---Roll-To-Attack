using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class UpdatedImpactFieldVisual : ImpactFieldVisual
{ 
    [SerializeField] GameObject ringObj;
    [SerializeField] GameObject fieldObj;

    protected override void Awake()
    {
        ringMeshRenderer = ringObj.GetComponent<MeshRenderer>();
        meshRenderer = fieldObj.GetComponent<MeshRenderer>();
        block = new MaterialPropertyBlock();
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
}
