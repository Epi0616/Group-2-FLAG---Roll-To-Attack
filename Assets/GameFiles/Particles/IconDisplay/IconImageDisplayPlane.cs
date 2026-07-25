using UnityEngine;
using System.Collections;
using System;
using System.Threading;

public class IconImageDisplayPlane : MonoBehaviour
{
    [SerializeField] private Material imageMat;
    private bool isDestroyed;
    private float displayTime = 1.75f;
    public virtual void Initialize(Texture2D textureToDisplay)
    {
        imageMat.SetTexture("_ImageToDisplay", textureToDisplay);
        imageMat.SetFloat("_Opacity", 0);
    }

    public IEnumerator DisplayRoutine()
    {
        float timer = 0;
        while (timer < displayTime)
        {
            timer += Time.deltaTime;
            imageMat.SetFloat("_Opacity", Mathf.Lerp(0f, 1f, easeOutBack(timer / displayTime))); 

            yield return null;
        }
        imageMat.SetFloat("_Opacity", 1);
        yield return new WaitForSeconds(1f);
        DestroyMe();
    }

    public virtual void DestroyMe()
    {
        if (isDestroyed) return;
        isDestroyed = true;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }

    protected float easeOutBack(float x)
    {
        const float c1 = 2.70158f;
        const float c3 = c1 + 1f;
        return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
    }
}

public interface IIconDisplayer
{
    public GameObject displayPlanePrefab { get; set; }
}
