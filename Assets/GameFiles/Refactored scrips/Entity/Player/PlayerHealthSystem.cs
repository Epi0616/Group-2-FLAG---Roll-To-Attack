using System;
using System.Collections;
using UnityEngine;


public class PlayerHealthSystem : EntityHealthSystem
{
    public static event Action<int, int> UpdateHealthBar;
    public static event Action<float> DamageConfirmed;
    public static event Action GameOver;

    private float iFrameTimer = 0;
    private Color damageColor = Color.red;

    private void OnEnable()
    {
        HealthOption.HealthChosen += OnHeal;
    }

    private void OnDisable()
    {
        HealthOption.HealthChosen -= OnHeal;
    }

    public override void OnTakeDamage(int damageAmount, DamageType type)
    {
        if (iFrameTimer > 0) return;

        currentHealth -= damageAmount;
        ownerEntity.textDisplaySystem.DisplayText(damageAmount.ToString(), damageColor, (int)55);
        UpdateHealthBar?.Invoke(currentHealth, (int)maxHealth.GetFinalValue());
        IFrames();

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    public override void OnHeal(int healAmount)
    {
        base.OnHeal(healAmount);
        UpdateHealthBar?.Invoke(currentHealth, (int)maxHealth.GetFinalValue());
    }

    public override void OnDeath()
    {
        if (isDead) return;
        isDead = true;
        GameOver?.Invoke();
    }

    private void IFrames()
    {
        float iTime = 0.5f;

        StartCoroutine(IFrameCounter(iTime));
        DamageConfirmed?.Invoke(iTime);
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
