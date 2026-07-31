using System.Collections;
using UnityEngine;

public class EntityBodySystem : MonoBehaviour, IEntitySystem
{
    public Entity OwnerEntity { get; set; }
    public GameObject body;
    public Quaternion originalRotation;
    public MaterialPropertyBlock block;
    public SkinnedMeshRenderer renderer;

    public virtual void InitialiseSystem(Entity entity)
    {
        OwnerEntity = entity;
        originalRotation = body.transform.rotation;
        block = new MaterialPropertyBlock();
        if (renderer == null)
        {
            renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        }
    }

    public virtual void Vibrate()
    {
        float x = Mathf.Sin(Time.time * 50) * 0.1f;
        float z = Mathf.Sin(Time.time * 50) * 0.1f;
        body.transform.localPosition = new Vector3(x, 0, z);
    }

    public virtual void HandleFixedVibrateTime(float duration)
    {
        StartCoroutine(FixedVibrateTime(duration));
    }

    private IEnumerator FixedVibrateTime(float timer)
    {
        Vector3 originalBodyPosition = body.transform.localPosition;
        while (timer > 0)
        { 
            timer -= Time.deltaTime;
            Vibrate();
            yield return null;
        }

        body.transform.localPosition = originalBodyPosition;
    }

    public virtual void Wobble(float magnitude)
    {
        float x = Mathf.Sin(Time.time * 50f) * magnitude;
        float y = Mathf.Sin(Time.time * 50f) * magnitude;
        float z = Mathf.Sin(Time.time * 50f) * magnitude;
        body.transform.rotation = originalRotation * Quaternion.Euler(x, y, z);
    }

    public void ApplyFreezeShader(Color iceColour)
    {
        renderer.GetPropertyBlock(block);
        block.SetFloat("_IcePower", 1);
        block.SetColor("_IceColour", iceColour);
        renderer.SetPropertyBlock(block);
    }

    public void RemoveFreezeShader()
    {
        renderer.GetPropertyBlock(block);
        block.SetFloat("_IcePower", 0);
        renderer.SetPropertyBlock(block);
    }

    public virtual void ResetSystem()
    {
        // Reset body system state if needed
    }

    public virtual void SetVisibility(bool visible)
    {
        body.SetActive(visible);
    }
}
