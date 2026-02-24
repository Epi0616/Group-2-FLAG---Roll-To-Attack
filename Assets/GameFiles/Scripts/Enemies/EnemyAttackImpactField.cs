using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAttackImpactField : MonoBehaviour
{
    private Material material;
    private Color color;
    private float lifeTime;
    private float chargeTime;
    private float radius;


    public void PassInValuesColorRadiusLifeTimeChargeTime(Color color, float radius, float lifeTime, float chargeTime)
    {
        this.color = color;
        this.radius = radius;
        this.lifeTime = lifeTime;
        this.chargeTime = chargeTime;

        material = GetComponent<MeshRenderer>().material;
        this.color.a = 0f;
        material.color = color;

        AdjustRadiusSize();
        StartCoroutine(ManageVisual());
    }

    private void AdjustRadiusSize()
    {
        Vector3 tempScale = transform.localScale;
        tempScale.x = radius * 2;
        tempScale.z = radius * 2;
        transform.localScale = tempScale;
    }

    private IEnumerator ManageVisual()
    {
        StartCoroutine(ImpactFadeIn());
        yield return new WaitForSeconds(chargeTime);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ImpactFadeOut());

    }

    private IEnumerator ImpactFadeIn()
    {
        Debug.Log("Fade in started");
        float timeElapsed = 0f;
        float a = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = transform.localScale;

        while (timeElapsed < chargeTime)
        {
            a = Mathf.Lerp( 0f, 0.75f, easeOutBack(timeElapsed / chargeTime) );

            color.a = a;
            material.color = color;

            transform.localScale = Vector3.Lerp(startScale, endScale, timeElapsed / chargeTime );
            
            timeElapsed += Time.deltaTime;
          
            yield return null;
        }
        transform.localScale = endScale;
        color.a = 1f;
        material.color = color;
        Debug.Log("Fade in ended");
    }

    private IEnumerator ImpactFadeOut()
    {
        Debug.Log("Fade out started");
        float timeElapsed = 0f;
        float a = 1f;

        while (a > 0.25f)
        {
            a = Mathf.Lerp(1f, 0f, timeElapsed / chargeTime);
          
            timeElapsed += Time.deltaTime;

            color.a = a;
            material.color = color;

            yield return null;
        }
        Destroy(gameObject);
    }

    private float easeOutBack(float x)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
    }

    private float easeOutExpo(float x)
    {
        return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
    }

}
