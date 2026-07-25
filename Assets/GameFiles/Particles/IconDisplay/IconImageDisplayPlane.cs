using UnityEngine;
using System.Collections;
using System;
using System.Threading;

public class IconImageDisplayPlane : MonoBehaviour
{
    private Material imageMat;
    private bool isDestroyed;
    private float displayTime = 0.25f;
    private Entity ownerEntity;

    public void Awake()
    {
        imageMat = GetComponent<MeshRenderer>().material;
    }
    public virtual void Initialize(Texture2D textureToDisplay, Entity entity)
    {
        imageMat.SetTexture("_IconToDisplay", textureToDisplay);
        imageMat.SetFloat("_Opacity", 0);
        transform.localScale = Vector3.one;
        isDestroyed = false;
        ownerEntity = entity;
        StartCoroutine(DisplayRoutine());
        
    }

    public IEnumerator DisplayRoutine()
    {
        float timer = 0;
        float startHeight = transform.position.y;
        float targetHeight = transform.position.y + 5;
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = Vector3.one * 1.25f;
        while (timer < displayTime)
        {
            timer += Time.deltaTime;
            imageMat.SetFloat("_Opacity", Mathf.Lerp(0f, 0.75f, (timer / displayTime)));
            transform.position = new Vector3(ownerEntity.transform.position.x, Mathf.Lerp(startHeight, targetHeight, (timer / displayTime)), ownerEntity.transform.position.z - 5f); 
            transform.localScale = Vector3.Lerp(startScale, targetScale, (timer / displayTime));
            yield return null;
        }
        imageMat.SetFloat("_Opacity", 0.75f);
        transform.position = new Vector3(ownerEntity.transform.position.x, targetHeight, ownerEntity.transform.position.z - 5f);
        transform.localScale = targetScale;
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
    public AbilityDisplayUI displayUI { get; set; }
}
