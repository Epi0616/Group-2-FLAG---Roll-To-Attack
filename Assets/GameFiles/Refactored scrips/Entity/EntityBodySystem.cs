using System.Collections;
using UnityEngine;
using System;

public class EntityBodySystem : MonoBehaviour, IEntitySystem
{
    public Entity OwnerEntity { get; set; }
    public GameObject body;
    public Quaternion originalRotation;
    public MaterialPropertyBlock block;
    public Renderer renderer;
    public Coroutine IceCoroutine;
    public Coroutine WeakenCracksCoroutine;
    public Coroutine PoisonedCoroutine;
   
    public virtual void InitialiseSystem(Entity entity)
    {
        OwnerEntity = entity;
        originalRotation = body.transform.rotation;
        block = new MaterialPropertyBlock();
        if (renderer == null)
        {
            renderer = GetComponentInChildren<Renderer>();
        }
        RemoveAllShaders();
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
    // Ice Shader -------------------------------------------------------------------------
    public void OverrideFreezeShader(float target)
    {
        if (IceCoroutine != null)
        {
            StopCoroutine(IceCoroutine);
        }

        renderer.GetPropertyBlock(block);
        block.SetFloat("_IcePower", target);
        renderer.SetPropertyBlock(block);
    }

    public void ApplyFreezeShader(Color iceColour)
    {
        renderer.GetPropertyBlock(block);
        block.SetColor("_IceColour", iceColour);
        renderer.SetPropertyBlock(block);
        StartFreezeTransition(1, 0.25f);
    }
    public void RemoveFreezeShader()
    {
        StartFreezeTransition(0, 0.2f);
    }
    
    public void StartFreezeTransition(float target, float duratiom)
    {
        if (IceCoroutine != null)
        {
            StopCoroutine(IceCoroutine);
        }

        IceCoroutine = StartCoroutine(FreezeShaderTransition(target, duratiom));
    }

    public IEnumerator FreezeShaderTransition(float target, float duration)
    {
        renderer.GetPropertyBlock(block);
        float timer = 0;
        float startingPower = block.GetFloat("_IcePower");
        while (timer < duration)
        {
            timer += Time.deltaTime;
            block.SetFloat("_IcePower", Mathf.Lerp(startingPower, target, (timer / duration)));
            renderer.SetPropertyBlock(block);
            yield return null;
        }
        block.SetFloat("_IcePower", target);
        renderer.SetPropertyBlock(block);
    }
    // Weaken ----------------------------------------------------------------------------------
    public void OverrideWeakenShader(float target)
    {
        if (WeakenCracksCoroutine != null)
        {
            StopCoroutine(WeakenCracksCoroutine);
        }
        renderer.GetPropertyBlock(block);
        block.SetFloat("_CrackPower", target);
        renderer.SetPropertyBlock(block);
    }

    public void ApplyWeakenShader(Color weakenColour)
    {
        renderer.GetPropertyBlock(block);
        block.SetColor("_CrackColour", weakenColour * 3);
        renderer.SetPropertyBlock(block);
        StartWeakenTransition(1, 0.5f);
    }
    public void RemoveWeakenShader()
    {
        StartWeakenTransition(0, 0.25f);
    }

    public void StartWeakenTransition(float target, float duratiom)
    {
        if (WeakenCracksCoroutine != null)
        {
            StopCoroutine(WeakenCracksCoroutine);
        }

        WeakenCracksCoroutine = StartCoroutine(WeakenShaderTransition(target, duratiom));
    }

    public IEnumerator WeakenShaderTransition(float target, float duration)
    {
        renderer.GetPropertyBlock(block);
        float timer = 0;
        float startingPower = block.GetFloat("_CrackPower");
        while (timer < duration)
        {
            timer += Time.deltaTime;
            block.SetFloat("_CrackPower", Mathf.Lerp(startingPower, target, (timer / duration)));
            renderer.SetPropertyBlock(block);
            yield return null;
        }
        block.SetFloat("_CrackPower", target);
        renderer.SetPropertyBlock(block);
    }

    //public void ApplyWeakenShader(Color weakenColour)
    //{
    //    renderer.GetPropertyBlock(block);
    //    block.SetFloat("_CrackPower", 1);
    //    block.SetColor("_CrackColour", weakenColour * 3);
    //    renderer.SetPropertyBlock(block);
    //}

    //public void RemoveWeakenShader()
    //{
    //    renderer.GetPropertyBlock(block);
    //    block.SetFloat("_CrackPower", 0);
    //    renderer.SetPropertyBlock(block);
    //}
    public void OverridePoisonedShader(float target)
    {
        if (PoisonedCoroutine != null)
        {
            StopCoroutine(PoisonedCoroutine);
        }
        renderer.GetPropertyBlock(block);
        block.SetFloat("_PoisonPower", target);
        renderer.SetPropertyBlock(block);
    }

    public void ApplyPoisonedShader(Color PoisonedColour)
    {
        renderer.GetPropertyBlock(block);
        block.SetColor("_PoisonColour", PoisonedColour * 3);
        renderer.SetPropertyBlock(block);
        StartPoisonedTransition(1, 0.75f);
    }
    public void RemovePoisonedShader()
    {
        StartPoisonedTransition(0, 0.5f);
    }

    public void StartPoisonedTransition(float target, float duratiom)
    {
        if (PoisonedCoroutine != null)
        {
            StopCoroutine(PoisonedCoroutine);
        }

        PoisonedCoroutine = StartCoroutine(PoisonedShaderTransition(target, duratiom));
    }

    public IEnumerator PoisonedShaderTransition(float target, float duration)
    {
        renderer.GetPropertyBlock(block);
        float timer = 0;
        float startingPower = block.GetFloat("_PoisonPower");
        while (timer < duration)
        {
            timer += Time.deltaTime;
            block.SetFloat("_PoisonPower", Mathf.Lerp(startingPower, target, (timer / duration)));
            renderer.SetPropertyBlock(block);
            yield return null;
        }
        block.SetFloat("_PoisonPower", target);
        renderer.SetPropertyBlock(block);
    }
    //public void ApplyPoisonedShader(Color poisonColour)
    //{
    //    renderer.GetPropertyBlock(block);
    //    block.SetFloat("_PoisonPower", 1);
    //    block.SetColor("_PoisonColour", poisonColour * 3);
    //    renderer.SetPropertyBlock(block);
    //}

    //public void RemovePoisonShader()
    //{
    //    renderer.GetPropertyBlock(block);
    //    block.SetFloat("_PoisonPower", 0);
    //    renderer.SetPropertyBlock(block);
    //}
    public virtual void ResetSystem()
    {
        // Reset body system state if needed
        
    }

    public virtual void RemoveAllShaders()
    {
        OverrideFreezeShader(0);
        OverrideWeakenShader(0);
        OverridePoisonedShader(0);
    }

    public virtual void SetVisibility(bool visible)
    {
        body.SetActive(visible);
    }
}
