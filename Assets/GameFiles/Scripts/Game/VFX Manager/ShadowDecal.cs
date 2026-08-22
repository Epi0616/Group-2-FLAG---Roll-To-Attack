using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShadowDecal : MonoBehaviour
{
    [SerializeField] private DecalProjector projector;
    private GameObject currentTarget;
    private bool followsPos;
    private Vector3 followOffset;
    private bool followsRot;
    private Coroutine GrowAndShrinkRoutine;
    private Vector2 startingWidthHeight;
    private bool hasBeenDestroyed;

    public void SetupProjector(Vector2 WnH, Quaternion rot, Vector3 posOffset, bool followsTargetPos, bool followsTargetRot, GameObject target = null)
    {
        hasBeenDestroyed = false;
        if (target == null)
        {
            followsPos = false;
            followsRot = false;
        }
        else
        {
            followsPos = followsTargetPos;
            followOffset = posOffset;
            followsRot = followsTargetRot;
            currentTarget = target;
        }
        projector.size = new Vector3(WnH.x, WnH.y, projector.size.z);
        startingWidthHeight = WnH;
        transform.rotation = rot;
    }

    public void Update()
    {
        if (followsPos) 
        {
            transform.position = currentTarget.transform.position + followOffset;  
        }
        if (followsRot)
        {
            transform.rotation = currentTarget.transform.rotation;
        }
    }

    public void StartGrowAndShrink(float sizeMultiplier, float totalDuration)
    {
        if (GrowAndShrinkRoutine != null)
        {
            StopCoroutine(GrowAndShrinkRoutine);
        }
        Vector3 smallestSize = new Vector3(projector.size.x * sizeMultiplier, projector.size.y * sizeMultiplier, projector.size.z);
        GrowAndShrinkRoutine = StartCoroutine(ShadowShrinkandGrow(smallestSize, totalDuration));
    }

    private IEnumerator ShadowShrinkandGrow(Vector3 smallestSize, float totalDuration)
    {
        float segmentDuration = totalDuration / 2;
        float timer = 0;
        Vector3 currentStartingDimensions = projector.size;
        while (timer < segmentDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / segmentDuration);
            Vector3 newSize = Vector3.Lerp(currentStartingDimensions, smallestSize, easeOutQuart(t));
            newSize.z = currentStartingDimensions.z;
            projector.size = newSize;
            yield return null;
        }
        projector.size = smallestSize;

        yield return null;

        timer = 0;
        while (timer < segmentDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / segmentDuration);

            Vector3 newSize = Vector3.Lerp(smallestSize, currentStartingDimensions, easeOutQuart(t));
            newSize.z = currentStartingDimensions.z;
            projector.size = newSize;
            yield return null;
        }
        projector.size = currentStartingDimensions;

    }

    public void SetNewWidthHeight(Vector2 newWidthHeight, bool lerp, float duration = 0)
    {
        if (GrowAndShrinkRoutine != null)
        {
            StopCoroutine(GrowAndShrinkRoutine);
        }
        if (lerp)
        {
            StartCoroutine(LerpToNewWidthHeight(newWidthHeight, duration));
        }
        else
        {
            Vector3 newSize = new Vector3(newWidthHeight.x, newWidthHeight.y, projector.size.z);
        }
    }

    private IEnumerator LerpToNewWidthHeight(Vector2 newWidthHeight, float duration)
    {
        float timer = 0;
        Vector3 currentStartingDimensions = projector.size;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);
            Vector3 newSize = Vector3.Lerp(currentStartingDimensions, newWidthHeight, easeOutQuart(t));
            newSize.z = currentStartingDimensions.z;
            projector.size = newSize;
            yield return null;
        }
        projector.size = newWidthHeight;
    }

    public void SetProjectorDepth(float depth)
    {
        Vector3 newSize = new Vector3(projector.size.x, projector.size.y, depth);
        projector.pivot = new Vector3(0, 0, depth / 2);
        projector.size = newSize;

    }

    public void SetNewOffset(Vector3 newOffset, bool lerp, float duration = 0)
    {
        if (lerp)
        {
            StartCoroutine(LerpToNewOffset(newOffset, duration));
        }
        else
        {
            followOffset = newOffset;
        }
            
    }

    public IEnumerator LerpToNewOffset(Vector3 newOffset, float duration)
    {
        float timer = 0;
        Vector3 currentOffset = followOffset;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);
            followOffset = Vector3.Lerp(currentOffset, newOffset, t);

            yield return null;
        }
        followOffset = newOffset;
    }

    public void DestroyMe()
    {
        if (hasBeenDestroyed) { return; }
        hasBeenDestroyed = true;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
    

    private float easeOutQuart(float t)
    {
        return 1 - Mathf.Pow(1 - t, 4);
    }
}
