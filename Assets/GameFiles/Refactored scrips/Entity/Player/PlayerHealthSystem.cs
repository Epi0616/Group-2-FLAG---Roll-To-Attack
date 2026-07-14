using System;
using System.Collections;
using UnityEngine;

public class PlayerHealthSystem : EntityHealthSystem
{
    public static event Action<int, int> UpdateHealthBar;
    public static event Action<float> ShowIFrames;
    public static event Action GameOver;

    private float iFrameTimer = 0;

    public override void OnTakeDamage(int damageAmount)
    {
        if (iFrameTimer > 0) return;

        currentHealth -= damageAmount;
        UpdateHealthBar?.Invoke(currentHealth, maxHealth);
        IFrames();

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    public override void OnDeath()
    {
        if (isDead) return;
        isDead = true;
        GameOver?.Invoke();
    }

    private void IFrames()
    {
        float iTime = 1;

        StartCoroutine(IFrameCounter(iTime));
        ShowIFrames?.Invoke(iTime);
    }

    private IEnumerator IFrameCounter(float iTime)
    { 
        iFrameTimer = iTime;
        while (iFrameTimer > 0)
        {
            iFrameTimer -= Time.deltaTime;
            yield return null;
        }
    }
}
