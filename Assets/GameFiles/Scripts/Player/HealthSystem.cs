using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth, currentHealth;
    public static event Action<int, int> UpdateHealthBar;
    public static event Action GameOver;
    public static event Action<float> IFrames;
    private float iFrameTimer = 0;

    private void OnEnable()
    {
        EnemyDirector.WaveOver += HealToFull;
    }
    private void OnDisable()
    {
        EnemyDirector.WaveOver -= HealToFull;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar?.Invoke(currentHealth, maxHealth);
    }

    private void Update()
    {
        iFrameTimer -= Time.deltaTime;
    }

    public void OnTakeDamage(int damage)
    {
        if (iFrameTimer > 0) return;

        PlayerIFrames();
        currentHealth -= damage;
        UpdateHealthBar?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }
    public void HealToFull(float waveNumber)
    {
        currentHealth = maxHealth;
        UpdateHealthBar?.Invoke(currentHealth, maxHealth);
    }
    public void OnDeath()
    {
        Debug.Log("Game Over");
        GameOver?.Invoke();
    }

    private void PlayerIFrames()
    {
        iFrameTimer = 1;
        IFrames?.Invoke(iFrameTimer);
    }
}
