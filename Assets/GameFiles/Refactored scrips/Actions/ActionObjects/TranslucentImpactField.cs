using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TranslucentImpactField : ImpactFieldVisual
{

    public override void PassInValuesColorRadiusChargeTimeFlash(Color color, float radius, float chargeTime, bool flash)
    {
        hasBeenDestroyed = false;
        this.color = color;
        this.color.a = 1f;
        this.radius = radius;
        this.chargeTime = chargeTime;
        flashRed = flash;

        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            fadeRoutine = null;
        }
        if (block == null) { Debug.Log("Block Missing"); }
        if (meshRenderer == null) { Debug.Log("Renderer Missing"); }
        SetColor(this.color);

        AdjustRadiusSize();
        fadeRoutine = StartCoroutine(ImpactFadeIn());
    }
    protected override IEnumerator ImpactFadeIn()
    {
        //Debug.Log(color.a);
        while (color.a > 0f)
        {
            //Debug.Log("Decaying");
            color.a += Time.deltaTime * -2f;
            SetColor(color);
            yield return null;
        }

        fadeRoutine = null;
        DestroyMe();
    }
}
