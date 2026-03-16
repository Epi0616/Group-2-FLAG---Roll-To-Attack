using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAttackImpactField : MonoBehaviour
{
    private Material material;
    private Color color;
    private Color initialColor;
    private float lifeTime;
    private float chargeTime;
    private float radius;

    public bool hasBeenDeletedAlready;

    public void PassInValuesColorRadiusLifeTimeChargeTime(Color color, float radius, float lifeTime, float chargeTime)
    {
        this.color = color;
        this.radius = radius;
        this.lifeTime = lifeTime;
        this.chargeTime = chargeTime;

        material = GetComponent<MeshRenderer>().material;
        this.color.a = 0f;
        material.color = color;

        hasBeenDeletedAlready = false;

        AdjustRadiusSize();
        StartCoroutine(ImpactFadeIn());
    }

    private void AdjustRadiusSize()
    {
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
            a = Mathf.Lerp( 0f, 0.75f, easeOutBack(timeElapsed / chargeTime) );

            color.a = a;
            material.color = color;

            transform.localScale = Vector3.LerpUnclamped(startScale, endScale, easeOutBack(timeElapsed / chargeTime));
            
            timeElapsed += Time.deltaTime;
          
            yield return null;
        }
        transform.localScale = endScale;
        color.a = 1f;
        material.color = color;
        initialColor = material.color;
        
        Color hitColor = new Color(0.5849056f, 0f, 0f, 1f);
        material.color = hitColor;

        //Debug.Log("Fade in ended");

        yield return new WaitForSeconds(0.4f);
        material.color = color;

         timeElapsed = 0f;
         a = 1f;

        while (timeElapsed < 0.4f)
        {
            a = Mathf.Lerp(1f, 0f, timeElapsed / 0.4f);

            timeElapsed += Time.deltaTime;

            color.a = a;
            material.color = color;

            yield return null;
        }

        if (!hasBeenDeletedAlready)
        {
            //ObjectPoolManager.ReturnObjectToPool(gameObject);
            Destroy(gameObject);
        }    

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

    private float easeOutExpo(float x)
    {
        return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
    }

}
